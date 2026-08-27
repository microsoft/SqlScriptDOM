//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorTestHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Template-method pipeline for script-generation option tests. The Generate/Verify/reparse
    // steps are identical across dialects; only the parser and generator types differ. They are
    // supplied as type parameters, and factory delegates create the concrete instances (whose
    // constructors take arguments, so a new() constraint is not usable).
    internal sealed class ScriptGenerationPipeline<TParser, TGenerator>
        where TParser : TSqlParser
        where TGenerator : SqlScriptGenerator
    {
        private readonly Func<TParser> _createParser;
        private readonly Func<SqlScriptGeneratorOptions, TGenerator> _createGenerator;

        public ScriptGenerationPipeline(
            Func<TParser> createParser,
            Func<SqlScriptGeneratorOptions, TGenerator> createGenerator)
        {
            _createParser = createParser;
            _createGenerator = createGenerator;
        }

        // Template method: parse the input, generate a script with the given options, assert the
        // input parses and the generated script reparses, and return the generated script.
        public string Generate(string sql, SqlScriptGeneratorOptions options)
        {
            TSqlFragment fragment = _createParser().Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Input must parse without errors.");

            _createGenerator(options).GenerateScript(fragment, out string generated);

            AssertReparses(generated);
            return generated;
        }

        public void AssertReparses(string sql)
        {
            _createParser().Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Generated script must reparse without errors. Actual:\n" + sql);
        }

        // Generates a script from the given SQL with the given options and asserts it equals the
        // expected value. The expected literals typically start with a newline right after @" so the
        // first SQL line lines up with the rest in source; Trim() removes that leading newline (and
        // any trailing whitespace from the generated output) before comparing.
        public void AssertGenerated(string sql, SqlScriptGeneratorOptions options, string expected)
        {
            Assert.AreEqual(
                ScriptGeneratorTestHelper.Normalize(expected).Trim(),
                ScriptGeneratorTestHelper.Normalize(Generate(sql, options)).Trim());
        }

        // Exact comparison without Trim(), so leading and trailing newline counts are part of the
        // assertion. Use this to lock down the newlines emitted after the final batch's last statement.
        public void AssertGeneratedExact(string sql, SqlScriptGeneratorOptions options, string expected)
        {
            Assert.AreEqual(
                ScriptGeneratorTestHelper.Normalize(expected),
                ScriptGeneratorTestHelper.Normalize(Generate(sql, options)));
        }
    }

    // Shared helpers for script-generation option tests. The default (SQL-170) and Fabric DW
    // pipelines share all logic via ScriptGenerationPipeline<TParser, TGenerator>; these static
    // methods are thin entry points so existing tests keep their call style.
    internal static class ScriptGeneratorTestHelper
    {
        private static readonly ScriptGenerationPipeline<TSql170Parser, Sql170ScriptGenerator> Sql170 =
            new ScriptGenerationPipeline<TSql170Parser, Sql170ScriptGenerator>(
                () => new TSql170Parser(true), options => new Sql170ScriptGenerator(options));

        private static readonly ScriptGenerationPipeline<TSqlFabricDWParser, SqlFabricDWScriptGenerator> FabricDW =
            new ScriptGenerationPipeline<TSqlFabricDWParser, SqlFabricDWScriptGenerator>(
                () => new TSqlFabricDWParser(true), options => new SqlFabricDWScriptGenerator(options));

        // SQL-170 pipeline.
        public static string Generate(string sql, SqlScriptGeneratorOptions options) => Sql170.Generate(sql, options);
        public static void AssertReparses(string sql) => Sql170.AssertReparses(sql);
        public static void AssertGenerated(string sql, SqlScriptGeneratorOptions options, string expected) => Sql170.AssertGenerated(sql, options, expected);

        // Fabric DW pipeline.
        public static string GenerateFabric(string sql, SqlScriptGeneratorOptions options) => FabricDW.Generate(sql, options);
        public static void AssertReparsesFabric(string sql) => FabricDW.AssertReparses(sql);
        public static void AssertGeneratedFabric(string sql, SqlScriptGeneratorOptions options, string expected) => FabricDW.AssertGenerated(sql, options, expected);

        // Normalizes line endings so verbatim expected constants compare equal regardless of the
        // source file's line-ending style.
        public static string Normalize(string value)
        {
            return value.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        // Exact comparison without Trim(), so leading and trailing newline counts are part of the
        // assertion. Use this to lock down the newlines emitted after the final batch's last statement.
        public static void AssertGeneratedExact(string sql, SqlScriptGeneratorOptions options, string expected) => Sql170.AssertGeneratedExact(sql, options, expected);
    }
}
