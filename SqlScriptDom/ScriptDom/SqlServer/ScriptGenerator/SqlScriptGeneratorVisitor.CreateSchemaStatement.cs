//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateSchemaStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateSchemaStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndKeyword(TSqlTokenType.Schema);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            // owner
            GenerateOwnerIfNotNull(node.Owner);

            if (node.StatementList != null && 
                node.StatementList.Statements != null &&
                node.StatementList.Statements.Count > 0)
            {
                AlignmentPoint ap = new AlignmentPoint();
                // schema elements
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);

                // to avoid generate semicolon for the statements from the StatementList
                Boolean originalValue = _generateSemiColon;
                _generateSemiColon = false;

                GenerateFragmentIfNotNull(node.StatementList);

                // restore the original value
                _generateSemiColon = originalValue;

                PopAlignmentPoint();
            }

        }
    }
}
