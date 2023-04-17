//------------------------------------------------------------------------------
// <copyright file="Sql90ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 90
    /// </summary>
    public sealed class Sql90ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql90ScriptGenerator"/> class.
        /// </summary>
        public Sql90ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql90ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql90ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql90ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
