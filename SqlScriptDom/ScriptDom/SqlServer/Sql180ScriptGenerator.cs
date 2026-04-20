//------------------------------------------------------------------------------
// <copyright file="Sql180ScriptGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Script generator for T-SQL 180
    /// </summary>
    public sealed class Sql180ScriptGenerator : SqlScriptGenerator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sql180ScriptGenerator"/> class.
        /// </summary>
        public Sql180ScriptGenerator()
            : this(new SqlScriptGeneratorOptions())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Sql180ScriptGenerator"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public Sql180ScriptGenerator(SqlScriptGeneratorOptions options)
            : base(options)
        {
            options.SqlVersion = SqlVersion.Sql180;
        }

        internal override SqlScriptGeneratorVisitor CreateSqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter scriptWriter)
        {
            ScriptGeneratorSupporter.CheckForNullReference(options, "options");
            ScriptGeneratorSupporter.CheckForNullReference(scriptWriter, "scriptWriter");

            return new Sql180ScriptGeneratorVisitor(options, scriptWriter);
        }
    }
}
