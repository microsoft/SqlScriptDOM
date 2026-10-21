//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BuiltInFunctionCasing.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // Emits the name of a built-in function whose name is represented in the AST by a keyword
        // token (for example CONVERT, COALESCE, NULLIF, LEFT, RIGHT). When BuiltInFunctionCasing is
        // Preserve this is a no-op and returns false so the caller emits its default representation
        // (backward-compatible, so the name keeps following KeywordCasing). Otherwise the name is
        // emitted with the configured casing applied, taking precedence over KeywordCasing, and
        // true is returned.
        protected bool TryGenerateBuiltInFunctionName(TSqlTokenType keywordId)
        {
            if (_options.BuiltInFunctionCasing == BuiltInFunctionCasing.Preserve)
            {
                return false;
            }

            string baseText = ScriptGeneratorSupporter.GetLowerCase(keywordId);
            GenerateToken(TSqlTokenType.Identifier, ScriptGeneratorSupporter.GetCasedString(baseText, _options.BuiltInFunctionCasing));
            return true;
        }

        // Emits the name of a built-in function whose name is represented in the AST by a literal
        // string (for example CAST, TRY_CAST, IIF). Behaves like the keyword overload above.
        protected bool TryGenerateBuiltInFunctionName(string name)
        {
            if (_options.BuiltInFunctionCasing == BuiltInFunctionCasing.Preserve)
            {
                return false;
            }

            GenerateToken(TSqlTokenType.Identifier, ScriptGeneratorSupporter.GetCasedString(name, _options.BuiltInFunctionCasing));
            return true;
        }

        // Emits the name of a generic FunctionCall when BuiltInFunctionCasing is not Preserve.
        // Returns false (leaving the caller to emit the name unchanged) for qualified calls, which
        // also covers CLR/XML/spatial method invocations, and for delimited names. Every remaining
        // name is re-cased without consulting a catalog of built-ins, because T-SQL resolves a
        // one-part scalar function name as a built-in function and never as a user-defined one - a
        // scalar UDF must be called with at least a two-part name - so a name reaching this point is
        // either a built-in, whose name is matched case-insensitively under every collation, or
        // already-invalid T-SQL. Table-valued functions, which do resolve one-part names against the
        // default schema, are SchemaObjectFunctionTableReference and never reach this path.
        private bool TryGenerateBuiltInFunctionName(FunctionCall node)
        {
            if (_options.BuiltInFunctionCasing == BuiltInFunctionCasing.Preserve)
            {
                return false;
            }

            if (node.CallTarget != null)
            {
                return false;
            }

            Identifier name = node.FunctionName;
            if (name == null || name.Value == null || name.QuoteType != QuoteType.NotQuoted)
            {
                return false;
            }

            // The replaced path emitted the name through GenerateFragmentIfNotNull, which is what
            // normally runs the comment hooks and advances the last-processed token index. Emitting a
            // raw token instead means they have to be driven explicitly, or comments adjacent to the
            // function name are relocated into the argument list (or dropped for zero-argument calls).
            HandleCommentsBeforeFragment(name);
            GenerateToken(TSqlTokenType.Identifier, ScriptGeneratorSupporter.GetCasedString(name.Value, _options.BuiltInFunctionCasing));
            HandleCommentsAfterFragment(name);
            return true;
        }
    }
}
