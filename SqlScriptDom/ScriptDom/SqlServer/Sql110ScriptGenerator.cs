//------------------------------------------------------------------------------
// <copyright file="Sql110ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 110
    /// </summary>
    public sealed class Sql110ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql110ScriptGenerator"/> class.
        /// </summary>
        public Sql110ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql110ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql110ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
            options.SqlVersion = SqlVersion.Sql110;
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql110ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
