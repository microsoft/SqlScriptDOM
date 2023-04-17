//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RollbackTransactionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RollbackTransactionStatement node)
        {
            GenerateKeyword(TSqlTokenType.Rollback); 

            if (node.Name != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Transaction); 

                GenerateSpace();
                GenerateTransactionName(node.Name);
            }
        }
    }
}
