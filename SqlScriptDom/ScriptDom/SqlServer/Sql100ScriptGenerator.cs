//------------------------------------------------------------------------------
// <copyright file="Sql100ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 100
    /// </summary>
    public sealed class Sql100ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql100ScriptGenerator"/> class.
        /// </summary>
        public Sql100ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql100ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql100ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql100ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
