//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CopyOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Globalization;

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
                string identityValue = ExtractStringValue(((CopyCredentialOption)node.Value).Identity);
                GenerateNameEqualsValue("Identity", string.Format(CultureInfo.InvariantCulture, "'{0}'", identityValue));

                if (((CopyCredentialOption)node.Value).Secret != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    string secretValue = ExtractStringValue(((CopyCredentialOption)node.Value).Secret);
                    GenerateNameEqualsValue("Secret", string.Format(CultureInfo.InvariantCulture, "'{0}'", secretValue));
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
                    string optionValue = ExtractStringValue(node.Value);

                    if (node.Kind == CopyOptionKind.FirstRow || node.Kind == CopyOptionKind.MaxErrors || node.Kind == CopyOptionKind.File_Format)
                        GenerateNameEqualsValue(node.Kind.ToString(), string.Format(CultureInfo.InvariantCulture, "{0}", optionValue));
                    else
                        GenerateNameEqualsValue(node.Kind.ToString(), string.Format(CultureInfo.InvariantCulture, "'{0}'", optionValue));
                }
                NewLine();
            }
        }

        /// <summary>
        /// Extracts value from copy credential option.
        /// </summary>
        /// <param name="credOption">Copy option.</param>
        /// <returns>Extracted string.</returns>
        private string ExtractStringValue(StringLiteral credOption)
        {
            if (credOption == null)
            {
                throw new ArgumentNullException("Invalid Credential values");
            }
            return credOption.Value;
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
