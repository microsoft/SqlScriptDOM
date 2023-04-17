//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TryCatchStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TryCatchStatement node)
        {
            AlignmentPoint statementBlock = new AlignmentPoint();

            // BEGIN TRY
            GenerateKeyword(TSqlTokenType.Begin);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Try);

            if (node.TryStatements.Statements.Count > 0)
            {
                // try block
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(statementBlock);
                GenerateFragmentIfNotNull(node.TryStatements);
                PopAlignmentPoint();
            }

            // END TRY
            NewLine();
            GenerateKeyword(TSqlTokenType.End);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Try);

            // BEGIN CATCH
            NewLine();
            GenerateKeyword(TSqlTokenType.Begin);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Catch);

            if (node.CatchStatements.Statements.Count > 0)
            {
                // catch block
                NewLineAndIndent();
                MarkAndPushAlignmentPoint(statementBlock);
                GenerateFragmentIfNotNull(node.CatchStatements);
                PopAlignmentPoint();
            }

            // ENC CATCH
            NewLine();
            GenerateKeyword(TSqlTokenType.End);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Catch); 
        }
    }
}
