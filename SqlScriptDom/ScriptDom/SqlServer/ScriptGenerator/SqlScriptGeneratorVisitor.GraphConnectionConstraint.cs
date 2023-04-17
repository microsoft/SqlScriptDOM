//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GraphConnectionConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// This method scripts the 'GraphConnectionConstraintDefinition' into text.
        /// </summary>
        /// <param name="node">The node to script.</param>
        public override void ExplicitVisit(GraphConnectionConstraintDefinition node)
        {
            GenerateConstraintHead(node);

            GenerateIdentifier(CodeGenerationSupporter.Connection);

            GenerateSpace();
            GenerateParenthesisedCommaSeparatedList(node.FromNodeToNodeList);

            // There is no handler for "Not Specified" because this method
            // is called for both column constraints and for table constraints
            // so we cannot just pass in 'node.DeleteAction'. It is unclear that
            // adding a handler for "Not Specified" to "NO ACTION" is correct in all cases.
            //
            if (node.DeleteAction == DeleteUpdateAction.Cascade ||
                node.DeleteAction == DeleteUpdateAction.NoAction)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndKeyword(TSqlTokenType.Delete);
                GenerateSpace();
                GenerateDeleteUpdateAction(node.DeleteAction);
            }
        }

        /// <summary>
        /// This method scripts a 'GraphConnectionBetweenNodes' into text.
        /// </summary>
        /// <param name="node">The node to script.</param>
        public override void ExplicitVisit(GraphConnectionBetweenNodes node)
        {
            GenerateFragmentIfNotNull(node.FromNode);

            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpace();

            GenerateFragmentIfNotNull(node.ToNode);
        }
    }
}
