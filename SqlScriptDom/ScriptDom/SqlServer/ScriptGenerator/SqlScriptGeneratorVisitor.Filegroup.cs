//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Filegroup.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(FileGroupDefinition node)
        {
            NewLineAndIndent();
            if (node.Name != null)
            {
                GenerateIdentifier(CodeGenerationSupporter.Filegroup);

                GenerateSpaceAndFragmentIfNotNull(node.Name);
            }

            if (node.ContainsFileStream)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Contains);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.FileStream);
            }

            if (node.ContainsMemoryOptimizedData)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Contains);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.MemoryOptimizedData);
                GenerateSpace();
            }

            if (node.IsDefault)
                GenerateSpaceAndKeyword(TSqlTokenType.Default); 

            GenerateCommaSeparatedList(node.FileDeclarations);
        }
    }
}
