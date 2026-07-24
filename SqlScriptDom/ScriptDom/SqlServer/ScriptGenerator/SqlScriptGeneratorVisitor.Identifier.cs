//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Identifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // When true, the IdentifierCasing and IdentifierBracketing options are not applied and
        // the identifier is emitted with its original casing and quoting. Used for contexts where
        // transforming the identifier would change semantics or produce invalid T-SQL, such as
        // function names (governed by a separate option) and GOTO label references (labels cannot
        // be delimited and must continue to match their label declaration).
        private bool _suppressIdentifierFormatting;

        // Lazily-created parser used to probe whether an identifier can safely drop its brackets
        // under IdentifierBracketing.ExcludeBrackets. It uses the configured SqlVersion so the
        // reserved-word list matches the target dialect.
        private TSqlParser _identifierBracketProbeParser;

        public override void ExplicitVisit(Identifier node)
        {
            if (node.Value == null)
            {
                return;
            }

            // Default behavior is preserved: when identifier formatting is suppressed (function
            // names, GOTO labels) or both options are at their default (Preserve), the identifier is
            // emitted exactly as before - same value, same quoting - with no casing or bracketing
            // change. Only when an option is set to a non-default value is any transformation applied.
            //
            // Variable and parameter names are also emitted unchanged: they are unquoted Identifier
            // fragments whose value starts with '@' (the AST keeps the '@'). They must never be
            // bracketed or recased - '[@x]' is invalid T-SQL and the interaction rules exclude
            // @variables. The check is limited to unquoted identifiers so a delimited object identifier
            // like [@Name] still participates in IdentifierCasing/IdentifierBracketing.
            //
            // Empty values (e.g. omitted components of a multi-part name) are also emitted unchanged:
            // bracketing an empty value would produce invalid '[]'.
            bool isVariableName = node.QuoteType == QuoteType.NotQuoted
                && node.Value.Length > 0
                && node.Value[0] == '@';

            if (_suppressIdentifierFormatting ||
                node.Value.Length == 0 ||
                isVariableName ||
                (_options.IdentifierCasing == IdentifierCasing.Preserve &&
                 _options.IdentifierBracketing == IdentifierBracketing.Preserve))
            {
                EmitIdentifier(node.Value, node.QuoteType);
                return;
            }

            // Bracketing is resolved first, then casing is applied to the value, per the documented
            // interaction rule (IdentifierCasing is applied after IdentifierBracketing).
            QuoteType quoteType = ResolveIdentifierQuoteType(node);
            string value = ScriptGeneratorSupporter.GetCasedString(node.Value, _options.IdentifierCasing);

            EmitIdentifier(value, quoteType);
        }

        // Emits an identifier exactly the way the generator did before the IdentifierCasing and
        // IdentifierBracketing options existed: unquoted values are written as-is, quoted values are
        // re-encoded with their quote type.
        private void EmitIdentifier(string value, QuoteType quoteType)
        {
            if (quoteType == QuoteType.NotQuoted)
            {
                GenerateIdentifierWithoutCheck(value);
            }
            else
            {
                GenerateQuotedIdentifier(value, quoteType);
            }
        }

        // Runs the given emit action with identifier casing/bracketing suppressed. Used for Identifier
        // fragments that represent syntax keywords/options rather than object names - function names,
        // GOTO labels, TRIM's LEADING/TRAILING/BOTH, JSON ABSENT/NULL ON NULL, and window
        // IGNORE/RESPECT NULLS - which must never be bracketed or recased (e.g. '[ABSENT] ON NULL' is
        // invalid). Has no effect under default options, where identifiers are emitted unchanged anyway.
        private void GenerateWithoutIdentifierFormatting(Action emit)
        {
            bool previousSuppress = _suppressIdentifierFormatting;
            _suppressIdentifierFormatting = true;
            try
            {
                emit();
            }
            finally
            {
                _suppressIdentifierFormatting = previousSuppress;
            }
        }

        private QuoteType ResolveIdentifierQuoteType(Identifier node)
        {
            switch (_options.IdentifierBracketing)
            {
                case IdentifierBracketing.IncludeBrackets:
                    // An empty value is an omitted component of a multi-part name (e.g. the missing
                    // schema in "db..t"), not an identifier to delimit. Bracketing it to "[]" is
                    // invalid T-SQL (SQL46010), so preserve it as an empty, unquoted component before
                    // returning SquareBracket. (ExplicitVisit already short-circuits empty values; this
                    // keeps ResolveIdentifierQuoteType correct on its own as well.)
                    return string.IsNullOrEmpty(node.Value) ? QuoteType.NotQuoted : QuoteType.SquareBracket;
                case IdentifierBracketing.ExcludeBrackets:
                    // This option controls square brackets only, so preserve other quote types
                    // (a double-quoted identifier keeps its double quotes).
                    if (node.QuoteType == QuoteType.SquareBracket && CanOmitBrackets(node.Value))
                    {
                        return QuoteType.NotQuoted;
                    }
                    return node.QuoteType;
                case IdentifierBracketing.Preserve:
                default:
                    return node.QuoteType;
            }
        }

        // Returns true if the unquoted identifier value is a valid regular identifier that is not a
        // reserved word for the configured SqlVersion (and therefore does not require brackets).
        // This is determined by re-lexing the value with the version-specific parser: a safe
        // identifier lexes to exactly one Identifier token whose text matches the value. Reserved
        // words lex to keyword tokens; special characters or spaces produce multiple tokens or errors.
        //
        // Known limitation: a few identifiers lex to an ordinary Identifier token yet are contextual
        // keywords whose brackets are semantically required (for example a schema-qualified name that
        // collides with the AI_GENERATE_CHUNKS relational operator). This method cannot see the
        // surrounding grammar context, so it may report such an identifier as bracket-optional. This
        // is an accepted, documented limitation of the opt-in ExcludeBrackets option.
        private bool CanOmitBrackets(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (_identifierBracketProbeParser == null)
            {
                _identifierBracketProbeParser = TSqlParser.CreateParser(_options.SqlVersion, true);
            }

            IList<ParseError> errors;
            IList<TSqlParserToken> tokens;
            using (StringReader reader = new StringReader(value))
            {
                tokens = _identifierBracketProbeParser.GetTokenStream(reader, out errors);
            }

            if (errors != null && errors.Count > 0)
            {
                return false;
            }

            // Expect exactly one meaningful token (plus the trailing EndOfFile token).
            TSqlParserToken identifierToken = null;
            foreach (TSqlParserToken token in tokens)
            {
                if (token.TokenType == TSqlTokenType.EndOfFile)
                {
                    continue;
                }
                if (identifierToken != null)
                {
                    // More than one token means this is not a bare identifier.
                    return false;
                }
                identifierToken = token;
            }

            if (identifierToken == null ||
                !string.Equals(identifierToken.Text, value, StringComparison.Ordinal))
            {
                return false;
            }

            // Only omit brackets when the value lexes to a genuine Identifier token. Values that lex
            // to keyword tokens (e.g. AI_GENERATE_CHUNKS, TIMESTAMP) may be accepted as an unquoted
            // identifier in some positions (such as a column alias) yet be invalid unquoted in others
            // (such as a function name), so it is not safe to strip their brackets. Keeping the
            // brackets is always valid.
            return identifierToken.TokenType == TSqlTokenType.Identifier;
        }

        private void GenerateQuotedIdentifier(string identifier, QuoteType quoteType)
        {
            GenerateIdentifierWithoutCheck(Identifier.EncodeIdentifier(identifier, quoteType)); 
        }
    }
}
