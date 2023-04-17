//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ReadOnlyForClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ReadOnlyForClause node)
        {
            GenerateKeyword(TSqlTokenType.Read);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Only); 
        }
    }
}
