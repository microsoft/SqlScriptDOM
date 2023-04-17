//------------------------------------------------------------------------------
// <copyright file="Sql150ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 150
    /// </summary>
    public sealed class Sql150ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql150ScriptGenerator"/> class.
        /// </summary>
        public Sql150ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql150ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql150ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql150ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
