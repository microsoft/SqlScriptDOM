//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TablePartitionOptionSpecifications.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TablePartitionOptionSpecifications node)
        {
             GenerateSpaceAndIdentifier(CodeGenerationSupporter.Range);
             switch (node.Range)
             {
                 case PartitionTableOptionRange.Left:
                     GenerateSpaceAndKeyword(TSqlTokenType.Left);
                     break;
                 case PartitionTableOptionRange.Right:
                     GenerateSpaceAndKeyword(TSqlTokenType.Right);
                     break;
             }

             // FOR VALUES
             GenerateSpaceAndKeyword(TSqlTokenType.For);
             GenerateSpaceAndKeyword(TSqlTokenType.Values);
             GenerateSpace();

             GenerateParenthesisedCommaSeparatedList(node.BoundaryValues, true);
        }

    }
}
