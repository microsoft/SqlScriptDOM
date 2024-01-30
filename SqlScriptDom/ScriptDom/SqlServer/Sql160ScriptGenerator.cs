//------------------------------------------------------------------------------
// <copyright file="Sql160ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 160
    /// </summary>
    public sealed class Sql160ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql160ScriptGenerator"/> class.
        /// </summary>
        public Sql160ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql160ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql160ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
            options.SqlVersion = SqlVersion.Sql160;
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql160ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
