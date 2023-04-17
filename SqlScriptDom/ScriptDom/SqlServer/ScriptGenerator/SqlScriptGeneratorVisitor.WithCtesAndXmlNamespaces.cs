//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WithCommonTableExpressionsAndXmlNamespaces.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WithCtesAndXmlNamespaces node)
        {
            AlignmentPoint start = new AlignmentPoint();
            MarkAndPushAlignmentPoint(start);

            AlignmentPoint clauseBody = GetAlignmentPointForFragment(node, ClauseBody);

            GenerateKeyword(TSqlTokenType.With);

            if (node.ChangeTrackingContext != null)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.ChangeTrackingContext);
                GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.ChangeTrackingContext);
                GenerateKeyword(TSqlTokenType.RightParenthesis);
                // TODO, insivara: add check for trailing xmlnamespaces once xmlnamespaces are allowed to follow CHANGE_TRACKING_CONTEXT
                if (node.CommonTableExpressions.Count > 0)
                    GenerateKeyword(TSqlTokenType.Comma);
            }

            if (node.XmlNamespaces != null)
            {
                MarkClauseBodyAlignmentWhenNecessary(true, clauseBody);
                GenerateSpaceAndFragmentIfNotNull(node.XmlNamespaces);
            }

            if (node.CommonTableExpressions.Count > 0)
            {
                if (node.XmlNamespaces != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    NewLine();
                }

                foreach (var item in node.CommonTableExpressions)
                {
                    AddAlignmentPointForFragment(item, clauseBody);
                }

                GenerateCommaSeparatedList(node.CommonTableExpressions, true);

                foreach (var item in node.CommonTableExpressions)
                {
                    ClearAlignmentPointsForFragment(item);
                }
            }

            PopAlignmentPoint();
        }
    }
}
