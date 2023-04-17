//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateTypeTableStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateTypeTableStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Type);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            GenerateSpaceAndKeyword(TSqlTokenType.Table);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

            NewLineAndIndent();

            AlignmentPoint tableBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(tableBody);

            bool commaNeeded = false;
            GenerateColumnsConstraintsIndexes(node.Definition.ColumnDefinitions, ref commaNeeded);
            GenerateColumnsConstraintsIndexes(node.Definition.TableConstraints, ref commaNeeded);
            GenerateColumnsConstraintsIndexes(node.Definition.Indexes, ref commaNeeded);

            GenerateSymbol(TSqlTokenType.RightParenthesis);

            GenerateCommaSeparatedWithClause(node.Options, false, true);

            PopAlignmentPoint();
        }
        
        private void GenerateColumnsConstraintsIndexes<T>(IList<T> fragmentList, ref bool commaNeeded) where T : TSqlFragment
        {
            if(fragmentList != null && fragmentList.Count > 0)
            {
                if(commaNeeded)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    NewLine();
                }
                
                GenerateCommaSeparatedList(fragmentList, true);
                commaNeeded = true;
            }
        }
    }
}