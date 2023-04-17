//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ForeignKeyConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ForeignKeyConstraintDefinition node)
        {
            GenerateConstraintHead(node);

            GenerateKeyword(TSqlTokenType.Foreign);
            GenerateSpaceAndKeyword(TSqlTokenType.Key);

            if (node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }

            GenerateSpaceAndKeyword(TSqlTokenType.References);
            GenerateSpaceAndFragmentIfNotNull(node.ReferenceTableName);

            if (node.ReferencedTableColumns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.ReferencedTableColumns);
            }

            if (node.DeleteAction != DeleteUpdateAction.NotSpecified)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndKeyword(TSqlTokenType.Delete);
                GenerateSpace();
                GenerateDeleteUpdateAction(node.DeleteAction);
            }

            if (node.UpdateAction != DeleteUpdateAction.NotSpecified)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On);
                GenerateSpaceAndKeyword(TSqlTokenType.Update);
                GenerateSpace();
                GenerateDeleteUpdateAction(node.UpdateAction);
            }

            if (node.NotForReplication)
            {
                GenerateSpace();
                GenerateNotForReplication();
            }
        }
    }
}
