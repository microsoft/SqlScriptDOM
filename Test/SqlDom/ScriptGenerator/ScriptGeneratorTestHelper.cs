//------------------------------------------------------------------------------
// <copyright file="ScriptGeneratorTestHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    // Shared helpers for script-generation option tests: generate a script from SQL with the given
    // options and verify both the input and the generated output parse without errors.
    internal static class ScriptGeneratorTestHelper
    {
        // Parses the input, generates a script with the given options (using the SQL-170 generator),
        // asserts the input parses and the generated script reparses, and returns the generated script.
        public static string Generate(string sql, SqlScriptGeneratorOptions options)
        {
            var parser = new TSql170Parser(true);
            TSqlFragment fragment = parser.Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Input must parse without errors.");

            var generator = new Sql170ScriptGenerator(options);
            generator.GenerateScript(fragment, out string generated);

            AssertReparses(generated);
            return generated;
        }

        public static void AssertReparses(string sql)
        {
            var reparser = new TSql170Parser(true);
            reparser.Parse(new StringReader(sql), out IList<ParseError> errors);
            Assert.AreEqual(0, errors.Count, "Generated script must reparse without errors. Actual:\n" + sql);
        }

        // Normalizes line endings so verbatim expected constants compare equal regardless of the
        // source file's line-ending style.
        public static string Normalize(string value)
        {
            return value.Replace("\r\n", "\n").Replace("\r", "\n");
        }

        // Generates a script from the given SQL with the given options and asserts it equals the
        // expected value. The expected literals typically start with a newline right after @" so the
        // first SQL line lines up with the rest in source; Trim() removes that leading newline (and
        // any trailing whitespace from the generated output) before comparing.
        public static void AssertGenerated(string sql, SqlScriptGeneratorOptions options, string expected)
        {
            Assert.AreEqual(Normalize(expected).Trim(), Normalize(Generate(sql, options)).Trim());
        }

        // Exact comparison without Trim(), so leading and trailing newline counts are part of the
        // assertion. Use this to lock down the newlines emitted after the final batch's last statement.
        public static void AssertGeneratedExact(string sql, SqlScriptGeneratorOptions options, string expected)
        {
            Assert.AreEqual(Normalize(expected), Normalize(Generate(sql, options)));
        }
    }
}
