//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FetchCursorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FetchCursorStatement node)
        {
            GenerateKeyword(TSqlTokenType.Fetch); 
            if (node.FetchType != null)
            {
                GenerateSpaceAndFragmentIfNotNull(node.FetchType);
                GenerateSpaceAndKeyword(TSqlTokenType.From); 
            }

            GenerateSpaceAndFragmentIfNotNull(node.Cursor);

            if (node.IntoVariables.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Into);

                GenerateSpace();
                GenerateCommaSeparatedList(node.IntoVariables);
            }
        }
    }
}
