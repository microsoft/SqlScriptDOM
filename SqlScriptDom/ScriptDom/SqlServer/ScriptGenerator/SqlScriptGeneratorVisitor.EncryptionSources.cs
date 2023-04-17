//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AssemblyEncryptionSource.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AssemblyEncryptionSource node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Assembly);
            GenerateSpaceAndFragmentIfNotNull(node.Assembly);
        }

        public override void ExplicitVisit(FileEncryptionSource node)
        {
            if (node.IsExecutable)
            {
                GenerateIdentifier(CodeGenerationSupporter.Executable);
                GenerateSpace();
            }

            GenerateNameEqualsValue(TSqlTokenType.File, node.File);
        }

        public override void ExplicitVisit(ProviderEncryptionSource node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Provider);
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            if (node.KeyOptions.Count > 0)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.With);
                GenerateSpace();
                GenerateCommaSeparatedList(node.KeyOptions);
            }
        }
    }
}
