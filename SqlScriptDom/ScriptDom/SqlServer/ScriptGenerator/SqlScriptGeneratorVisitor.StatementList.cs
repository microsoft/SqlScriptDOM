//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.StatementList.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(StatementList node)
        {
            if (node.Statements != null)
            {
                for (int i = 0; i < node.Statements.Count; i++)
                {
                    var statement = node.Statements[i];
                    if (i > 0)
                    {
                        for (var nl = 0; nl < _options.NumNewlinesAfterStatement; nl++)
                        {
                            NewLine();
                        }
                        // Emit any deferred comments captured from previous statement before generating the next.
                        EmitPendingLeadingComments();
                    }

                    GenerateFragmentIfNotNull(statement);

                    // If we just generated a statement and the last emitted token is a single-line comment
                    // while more statements follow, and PreserveComments + spacing option indicate separation,
                    // defer that comment so it becomes a leading comment for the next statement instead of
                    // trailing inline before the semicolon.
                    if (_options.PreserveComments && i < node.Statements.Count - 1 && _options.NumNewlinesAfterStatement > 0)
                    {
                        var deferred = _writer.PopLastSingleLineCommentIfAny();
                        if (deferred != null)
                        {
                            _pendingLeadingComments.Add(deferred);
                            _suppressNextClauseAlignment = false; // ensure next clause aligns normally
                            _writer.TrimTrailingWhitespace(); // remove space that was inserted for inline comment
                        }
                    }

                    GenerateSemiColonWhenNecessary(statement);
                }
                // In case script ends with deferred comments (edge case), emit them.
                EmitPendingLeadingComments();
            }
        }
    }
}
