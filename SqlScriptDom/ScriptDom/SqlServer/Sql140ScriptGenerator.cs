//------------------------------------------------------------------------------
// <copyright file="Sql140ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 140
    /// </summary>
    public sealed class Sql140ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql140ScriptGenerator"/> class.
        /// </summary>
        public Sql140ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql140ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql140ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql140ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
