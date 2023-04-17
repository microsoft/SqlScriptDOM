//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateResourcePoolStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateCryptographicProviderStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();

            GenerateKeywordAndSpace(TSqlTokenType.Create);
            GenerateIdentifier(CodeGenerationSupporter.Cryptographic);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            CryptographicProviderFile(node.File, ap);

        }

        public void CryptographicProviderFile(Literal node, AlignmentPoint ap)
        {
            NewLineAndIndent();
            MarkAndPushAlignmentPoint(ap);
            GenerateKeywordAndSpace(TSqlTokenType.From);
            GenerateNameEqualsValue(TSqlTokenType.File, node);
            PopAlignmentPoint();
        }

        public override void ExplicitVisit(AlterCryptographicProviderStatement node)
        {
            AlignmentPoint ap = new AlignmentPoint();
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateIdentifier(CodeGenerationSupporter.Cryptographic);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.Option == EnableDisableOptionType.None)
                CryptographicProviderFile(node.File,ap);
            else
            {
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(ap);
                EnableDisableOptionTypeHelper.Instance.GenerateSourceForOption(_writer, node.Option);
                PopAlignmentPoint();
            }
       }

        public override void ExplicitVisit(DropCryptographicProviderStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Drop);
            GenerateIdentifier(CodeGenerationSupporter.Cryptographic);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Provider);
            GenerateSpaceAndFragmentIfNotNull(node.Name);
        }

    }
}
