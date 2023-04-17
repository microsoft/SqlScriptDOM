//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropExternalStreamStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// EXEC sp_drop_streaming_job statement
        /// </summary>
        /// <param name="node"></param>
        public override void ExplicitVisit(DropExternalStreamingJobStatement node)
        {
            // Until the engine changes are complete, external streaming jobs are dropped via stored procedure:
            // EXEC [sys].[sp_drop_streaming_job] @name = N'JobName'

            GenerateKeyword(TSqlTokenType.Exec);
            GenerateSpace();
            GenerateQuotedIdentifier(CodeGenerationSupporter.Sys.ToLowerInvariant(), QuoteType.SquareBracket);
            GenerateKeyword(TSqlTokenType.Dot);
            GenerateQuotedIdentifier(CodeGenerationSupporter.SpDropStreamingJob.ToLowerInvariant(), QuoteType.SquareBracket);

            GenerateSpace();
            GenerateIdentifierWithoutCasing(CodeGenerationSupporter.AtSymbol + CodeGenerationSupporter.Name.ToLowerInvariant());
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateFragmentIfNotNull(node.Name);
        }
    }
}
