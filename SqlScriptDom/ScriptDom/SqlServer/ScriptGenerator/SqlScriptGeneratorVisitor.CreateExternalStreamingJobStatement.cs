//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateColumnMasterKey.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// EXEC sys.sp_create_streaming_job statement
        /// </summary>
        public override void ExplicitVisit(CreateExternalStreamingJobStatement node)
        {
            // Until the engine changes are complete, external streaming jobs are created via stored procedure:
            // EXEC [sys].[sp_create_streaming_job] @name = N'JobName', @statement = N'JobStatement'

            GenerateKeyword(TSqlTokenType.Exec);
            GenerateSpace();
            GenerateQuotedIdentifier(CodeGenerationSupporter.Sys.ToLowerInvariant(), QuoteType.SquareBracket);
            GenerateKeyword(TSqlTokenType.Dot);
            GenerateQuotedIdentifier(CodeGenerationSupporter.SpCreateStreamingJob.ToLowerInvariant(), QuoteType.SquareBracket);

            GenerateSpace();
            GenerateIdentifierWithoutCasing(CodeGenerationSupporter.AtSymbol + CodeGenerationSupporter.Name.ToLowerInvariant());
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Name);

            GenerateKeyword(TSqlTokenType.Comma);

            GenerateSpace();
            GenerateIdentifierWithoutCasing(CodeGenerationSupporter.AtSymbol + CodeGenerationSupporter.Statement.ToLowerInvariant());
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Statement);
        }
    }
}