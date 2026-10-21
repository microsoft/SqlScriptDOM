//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CopyOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CopyOption node)
        {
            if (node.Kind == CopyOptionKind.Credential || node.Kind == CopyOptionKind.ErrorFileCredential)
            {
                switch (node.Kind)
                {
                    case CopyOptionKind.ErrorFileCredential:
                        GenerateNameEqualsValue(CodeGenerationSupporter.ErrorFileCredential, "");
                        break;
                    case CopyOptionKind.Credential:
                        GenerateNameEqualsValue(node.Kind.ToString(), "");
                        break;
                    default:
                        break;
                }
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                CopyCredentialOption credential = (CopyCredentialOption)node.Value;
                GenerateNameEqualsValue("Identity", GetCopyOptionStringLiteral(credential.Identity));

                if (credential.Secret != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateNameEqualsValue("Secret", GetCopyOptionStringLiteral(credential.Secret));
                }
                GenerateSymbol(TSqlTokenType.RightParenthesis);
                NewLine();
            }
            else
            {
                CopyOptionKind optionString;
                bool option = Enum.TryParse<CopyOptionKind>(node.Kind.ToString(), true, out optionString);
                if (string.Compare(optionString.ToString(), "ColumnOptions", StringComparison.OrdinalIgnoreCase) == 0)
                {
                    //Skip, as this is the default Column Option, that has been already handled
                }
                else
                {
                    if (node.Kind == CopyOptionKind.FirstRow || node.Kind == CopyOptionKind.MaxErrors)
                        GenerateNameEqualsValue(node.Kind.ToString(), ExtractStringValue(node.Value));
                    else if (node.Kind == CopyOptionKind.File_Format)
                    {
                        Identifier identifier = ((SingleValueTypeCopyOption)node.Value).SingleValue.Identifier;
                        GenerateNameEqualsValue(node.Kind.ToString(), new Identifier
                        {
                            Value = ScriptGeneratorSupporter.GetCasedString(identifier.Value, _options.KeywordCasing),
                            QuoteType = identifier.QuoteType
                        });
                    }
                    else
                        GenerateNameEqualsValue(node.Kind.ToString(), GetCopyOptionStringLiteral(
                            (StringLiteral)((SingleValueTypeCopyOption)node.Value).SingleValue.ValueExpression));
                }
                NewLine();
            }
        }

        /// <summary>
        /// Creates a string literal with the existing COPY option casing.
        /// </summary>
        /// <param name="literal">Copy option literal.</param>
        /// <returns>A cased copy of the literal.</returns>
        private StringLiteral GetCopyOptionStringLiteral(StringLiteral literal)
        {
            if (literal == null)
            {
                throw new ArgumentNullException("Invalid Credential values");
            }

            // Legacy PascalCase applied to the leading quote, leaving the value lowercase.
            KeywordCasing casing = _options.KeywordCasing == KeywordCasing.PascalCase
                ? KeywordCasing.Lowercase : _options.KeywordCasing;
            return new StringLiteral
            {
                Value = ScriptGeneratorSupporter.GetCasedString(literal.Value, casing),
                IsNational = literal.IsNational,
                Collation = literal.Collation
            };
        }

        /// <summary>
        /// Extracts value from copy option.
        /// </summary>
        /// <param name="option">Copy option.</param>
        /// <returns>Extracted string.</returns>
        private string ExtractStringValue(CopyStatementOptionBase option)
        {
            if (option == null)
            {
                throw new ArgumentNullException("Invalid Copy Option value");
            }
            return ((SingleValueTypeCopyOption)option).SingleValue.Value;
        }
    }
}
