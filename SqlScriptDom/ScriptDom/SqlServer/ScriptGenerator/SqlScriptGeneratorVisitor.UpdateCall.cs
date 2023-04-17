//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UpdateCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UpdateCall node)
        {
            GenerateKeyword(TSqlTokenType.Update);

            GenerateSpace();
            GenerateParenthesisedFragmentIfNotNull(node.Identifier);
        }
    }
}
