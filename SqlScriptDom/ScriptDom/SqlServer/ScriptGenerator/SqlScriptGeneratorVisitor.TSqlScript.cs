//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TSqlScript.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TSqlScript node)
        {
            // Initialize token stream for comment preservation
            if (_options.PreserveComments && node.ScriptTokenStream != null)
            {
                SetTokenStreamForComments(node.ScriptTokenStream);
            }

            Boolean firstItem = true;
            foreach (var item in node.Batches)
            {
                if (firstItem)
                {
                    firstItem = false;
                }
                else
                {
                    // GO always starts a new line, whatever the batch statements left behind.
                    NewLine();
                    GenerateKeyword(TSqlTokenType.Go);
                    GenerateNewLinesAfterBatch();
                }

                GenerateFragmentIfNotNull(item);
            }

            // Preserve the trailing GO separators when the parsed script ended with them and the option is enabled.
            if (_options.PersistTrailingGo && node.TrailingGoCount > 0)
            {
                // Emit comments that precede the trailing GO(s) so they stay above the batch separator.
                EmitCommentsUntilNextNonTriviaToken();
                for (int i = 0; i < node.TrailingGoCount; i++)
                {
                    NewLine();
                    GenerateKeyword(TSqlTokenType.Go);
                    GenerateNewLinesAfterBatch();
                }
            }

            // Emit any remaining comments at end of script (after the last statement)
            EmitRemainingComments();
        }

        private void GenerateNewLinesAfterBatch()
        {
            for (int i = 0; i < _options.NumNewlinesAfterBatches; i++)
            {
                NewLine();
            }
        }
    }
}
