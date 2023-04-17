//------------------------------------------------------------------------------
// <copyright file="Sql80ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 80
    /// </summary>
    public sealed class Sql80ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql80ScriptGenerator"/> class.
        /// </summary>
        public Sql80ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql80ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql80ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql80ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
