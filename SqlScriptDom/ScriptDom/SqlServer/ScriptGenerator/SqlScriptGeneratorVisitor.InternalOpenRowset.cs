//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.InternalOpenRowset.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(InternalOpenRowset node)
        {
            GenerateKeyword(TSqlTokenType.OpenRowSet);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.Identifier);

            if (node.VarArgs.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);  
            }

            GenerateCommaSeparatedList(node.VarArgs);

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
