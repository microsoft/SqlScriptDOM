//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BeginEndAtomicBlockStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(BeginEndAtomicBlockStatement node)
        {
            AlignmentPoint body = new AlignmentPoint();

            GenerateKeyword(TSqlTokenType.Begin);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Atomic);

            GenerateCommaSeparatedWithClause(node.Options, false, true);

            NewLineAndIndent();
            MarkAndPushAlignmentPoint(body);
            GenerateFragmentIfNotNull(node.StatementList);
            PopAlignmentPoint();

            NewLine();
            GenerateKeyword(TSqlTokenType.End);
        }

        public override void ExplicitVisit(LiteralAtomicBlockOption node)
        {
            AtomicBlockOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Value);
        }

        public override void ExplicitVisit(IdentifierAtomicBlockOption node)
        {
            if (node.Value != null)
            {
                AtomicBlockOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndIdentifier(node.Value.Value);
            }
        }
        
        public override void ExplicitVisit(OnOffAtomicBlockOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == AtomicBlockOptionKind.DelayedDurability, "TableOption does not match");
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.DelayedDurability, node.OptionState);
        }
    }
}
