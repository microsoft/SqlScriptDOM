//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ConvertCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ConvertCall node)
        {
            GenerateKeyword(TSqlTokenType.Convert);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.DataType);
            GenerateSymbol(TSqlTokenType.Comma);

            GenerateSpaceAndFragmentIfNotNull(node.Parameter);

            if (node.Style != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.Style);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis);

			GenerateSpaceAndCollation(node.Collation);
		}
    }
}
