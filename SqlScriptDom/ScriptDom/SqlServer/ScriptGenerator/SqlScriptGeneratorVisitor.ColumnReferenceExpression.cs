//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Column.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ColumnReferenceExpression node)
        {
            GenerateFragmentIfNotNull(node.MultiPartIdentifier);

            if (node.ColumnType != ColumnType.Regular)
            {
                if (node.MultiPartIdentifier != null && node.MultiPartIdentifier.Count > 0)
                    GenerateSymbol(TSqlTokenType.Dot);

                switch (node.ColumnType)
                {
                    case ColumnType.Regular:
                        // Shouldn't get here due to if above
                        break;
                    case ColumnType.IdentityCol:
                        GenerateKeyword(TSqlTokenType.IdentityColumn);
                        break;
                    case ColumnType.RowGuidCol:
                        GenerateKeyword(TSqlTokenType.RowGuidColumn);
                        break;
                    case ColumnType.Wildcard:
                        GenerateSymbol(TSqlTokenType.Star);
                        break;
                    case ColumnType.PseudoColumnIdentity:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Identity);
                        break;
                    case ColumnType.PseudoColumnRowGuid:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Rowguid);
                        break;
                    case ColumnType.PseudoColumnAction:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Action);
                        break;
                    case ColumnType.PseudoColumnCuid:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.Cuid);
                        break;
                    case ColumnType.PseudoColumnGraphEdgeId:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphEdgeId);
                        break;
                    case ColumnType.PseudoColumnGraphNodeId:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphNodeId);
                        break;
                    case ColumnType.PseudoColumnGraphFromId:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphFromId);
                        break;
                    case ColumnType.PseudoColumnGraphToId:
                        GenerateToken(TSqlTokenType.PseudoColumn,
                            CodeGenerationSupporter.DollarSign + CodeGenerationSupporter.GraphToId);
                        break;
                    default:
                        System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                        break;
                }
            }

            GenerateSpaceAndCollation(node.Collation);
        }
    }
}
