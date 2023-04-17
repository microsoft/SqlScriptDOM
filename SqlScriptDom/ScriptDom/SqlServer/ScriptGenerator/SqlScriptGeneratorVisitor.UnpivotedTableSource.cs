//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UnpivotedTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UnpivotedTableReference node)
        {
            // could be
            //      JoinParenthesis
            //      OdbcQualifiedJoin
            //      QualifiedJoin
            //      SimpleTableSource
            //      UnqualifiedJoin
            GenerateFragmentIfNotNull(node.TableReference);
            GenerateSpaceAndKeyword(TSqlTokenType.Unpivot);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.ValueColumn);
            GenerateSpaceAndKeyword(TSqlTokenType.For);
            GenerateSpaceAndFragmentIfNotNull(node.PivotColumn);
            GenerateSpaceAndKeyword(TSqlTokenType.In);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.InColumns, true);

            GenerateSymbol(TSqlTokenType.RightParenthesis);
            GenerateSpaceAndAlias(node.Alias);
        }
    }
}
