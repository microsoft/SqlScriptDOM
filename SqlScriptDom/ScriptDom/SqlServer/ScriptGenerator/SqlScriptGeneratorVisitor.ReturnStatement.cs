//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ReturnStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ReturnStatement node)
        {
            GenerateKeyword(TSqlTokenType.Return); 

            if (node.Expression != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.Expression);
            }
        }
    }
}
