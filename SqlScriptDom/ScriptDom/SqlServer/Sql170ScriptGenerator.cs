//------------------------------------------------------------------------------
// <copyright file="Sql170ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 170
    /// </summary>
    public sealed class Sql170ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql170ScriptGenerator"/> class.
        /// </summary>
        public Sql170ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql170ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql170ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
            options.SqlVersion = SqlVersion.Sql170;
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql170ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
