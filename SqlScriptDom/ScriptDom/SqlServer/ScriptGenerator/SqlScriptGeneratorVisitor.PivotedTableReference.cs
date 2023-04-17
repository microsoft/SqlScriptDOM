//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PivotedTableSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(PivotedTableReference node)
        {
            // could be
            //      JoinParenthesis
            //      OdbcQualifiedJoin
            //      QualifiedJoin
            //      SimpleTableSource
            //      UnqualifiedJoin
            GenerateFragmentIfNotNull(node.TableReference);
            GenerateSpaceAndKeyword(TSqlTokenType.Pivot);
            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.AggregateFunctionIdentifier);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateCommaSeparatedList(node.ValueColumns);
            GenerateSymbol(TSqlTokenType.RightParenthesis);

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
