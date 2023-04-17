//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeclareTableVariableBody.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DeclareTableVariableBody node)
        {
            GenerateFragmentIfNotNull(node.VariableName);

            if (node.AsDefined)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As); 
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            NewLineAndIndent();

            AlignmentPoint tableBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(tableBody);

            List<TSqlFragment> columnsAndConstraintsAndIndexes = new List<TSqlFragment>();
            columnsAndConstraintsAndIndexes.AddRange(node.Definition.ColumnDefinitions);
            columnsAndConstraintsAndIndexes.AddRange(node.Definition.TableConstraints);
            columnsAndConstraintsAndIndexes.AddRange(node.Definition.Indexes);

            ListGenerationOption option = ListGenerationOption.CreateOptionFromFormattingConfig(_options);

            GenerateCommaSeparatedList(columnsAndConstraintsAndIndexes, true);

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            PopAlignmentPoint();
        }
    }
}
