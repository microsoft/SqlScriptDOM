//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterDbSetStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterDatabaseSetStatement node)
        {
            GenerateAlterDbStatementHead(node);

            NewLineAndIndent();
            bool azureOnlyOption = true;
            foreach(DatabaseOption option in node.Options)
            {
                if (option.OptionKind != DatabaseOptionKind.MaxSize && option.OptionKind != DatabaseOptionKind.Edition && option.OptionKind != DatabaseOptionKind.ServiceObjective)
                {
                    azureOnlyOption = false;
                    break;
                }
            }
            if (azureOnlyOption)
            {
                GenerateIdentifier(CodeGenerationSupporter.Modify);
                GenerateSpace();
                AlignmentPoint items = new AlignmentPoint();
                MarkAndPushAlignmentPoint(items);
                GenerateParenthesisedCommaSeparatedList(node.Options, true);
                PopAlignmentPoint();
            }
            else
            {
                GenerateKeyword(TSqlTokenType.Set);

                GenerateSpace();
                AlignmentPoint items = new AlignmentPoint();
                MarkAndPushAlignmentPoint(items);
                GenerateCommaSeparatedList(node.Options, true);
                PopAlignmentPoint();
            }
            GenerateSpaceAndFragmentIfNotNull(node.Termination);
        }
    }
}
