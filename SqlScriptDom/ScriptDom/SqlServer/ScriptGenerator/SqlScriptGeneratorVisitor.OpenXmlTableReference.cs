//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OpenXmlTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OpenXmlTableReference node)
        {
            GenerateKeyword(TSqlTokenType.OpenXml);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            GenerateFragmentIfNotNull(node.Variable);
            GenerateSymbol(TSqlTokenType.Comma);

            GenerateSpaceAndFragmentIfNotNull(node.RowPattern);

            if (node.Flags != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);
                GenerateSpaceAndFragmentIfNotNull(node.Flags);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 

            if (node.TableName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpaceAndFragmentIfNotNull(node.TableName);
            }
            else if (node.SchemaDeclarationItems.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With); 

                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.SchemaDeclarationItems);
            }

            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
