//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OdbcQualifiedJoin.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OdbcQualifiedJoinTableReference node)
        {
            GenerateSymbol(TSqlTokenType.LeftCurly);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Oj);
            GenerateSpaceAndFragmentIfNotNull(node.TableReference);
            GenerateSpaceAndSymbol(TSqlTokenType.RightCurly); 
        }
    }
}
