//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.UniqueConstraint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(UniqueConstraintDefinition node)
        {
            GenerateConstraintHead(node);

            if (node.IsPrimaryKey)
            {
                GenerateKeyword(TSqlTokenType.Primary);
                GenerateSpaceAndKeyword(TSqlTokenType.Key);
            }
            else
            {
                GenerateKeyword(TSqlTokenType.Unique);
            }

            // Use the deprecated Clustered property only if the IndexType is null
            // Otherwise, the appropriate keyword will be generated in the IndexDefinition visitor
            //
            if (node.Clustered.HasValue && node.IndexType == null)
            {
                GenerateSpaceAndKeyword(node.Clustered.Value ? TSqlTokenType.Clustered : TSqlTokenType.NonClustered);
            }

            if(node.IndexType != null)
            {
                switch(node.IndexType.IndexTypeKind)
                {
                    case IndexTypeKind.Clustered:
                        GenerateSpaceAndKeyword(TSqlTokenType.Clustered);
                        break;
                    case IndexTypeKind.NonClusteredHash:
						GenerateSpaceAndKeyword(TSqlTokenType.NonClustered);
						GenerateSpaceAndIdentifier(CodeGenerationSupporter.Hash);
                        break;
                    case IndexTypeKind.NonClustered:
                        GenerateSpaceAndKeyword(TSqlTokenType.NonClustered);
                        break;
                }
            }
            if (node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }

            if (node.IndexOptions.Count > 0)
            {
                GenerateIndexOptions(node.IndexOptions);
            }

            if (node.OnFileGroupOrPartitionScheme != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.On); 
                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroupOrPartitionScheme);
            }

            GenerateFileStreamOn(node);

            if (node.IsEnforced == false)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Not);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Enforced);
            }
        }
    }
}
