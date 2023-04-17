//------------------------------------------------------------------------------
// <copyright file="CopyIdentifierOrValueOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

//------------------------------------------------------------------------------
// Purpose:
// Helper utility for copy command options assignment.
//

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class CopyIdentifierOrValueOptionsHelper : OptionsHelper<CopyOptionKind>
    {
        /// <summary>
        /// Instance to call the methods statically.
        /// </summary>
        public static readonly CopyIdentifierOrValueOptionsHelper Instance = new CopyIdentifierOrValueOptionsHelper();

        /// <summary>
        /// Represents the max length 4K.
        /// </summary>
        private readonly int maxLength = 4000;

        /// <summary>
        /// Maximum allowed ascii value of any character in the access key.
        /// </summary>
        private const int MaxAsciiCharValue = 127;

        /// <summary>
        /// Represents the max length for quotefield delimiter.
        /// </summary>
        private readonly int fieldQuoteMaxLength = 1;

        /// <summary>
        /// Represents the max length for hexadecimal value of quotefield delimiter.
        /// Maximum one can use 4 character for single character.
        /// </summary>
        private readonly int hexDecimalFieldQuoteMaxLen = 4;

        /// <summary>
        /// Mapping of COPY credential identity option string values to supported COPY identity types.
        /// </summary>
        private static Dictionary<string, CopyCommandCredentialType> credentialTypeMapping =
            new Dictionary<string, CopyCommandCredentialType>(StringComparer.OrdinalIgnoreCase)
            {
                { "Shared Access Signature", CopyCommandCredentialType.Sas },
                { "Storage Account Key", CopyCommandCredentialType.AccountKey },
                { "Managed Identity", CopyCommandCredentialType.ManagedIdentity },
                { "Service Principal", CopyCommandCredentialType.ServicePrincipal }
            };

        private CopyIdentifierOrValueOptionsHelper()
        {
            this.AddOptionMapping(CopyOptionKind.File_Format, CodeGenerationSupporter.FileFormat);
            this.AddOptionMapping(CopyOptionKind.File_Type, CodeGenerationSupporter.FileType);
            this.AddOptionMapping(CopyOptionKind.ErrorFile, CodeGenerationSupporter.ErrorFile);
            this.AddOptionMapping(CopyOptionKind.MaxErrors, CodeGenerationSupporter.MaxErrors);
            this.AddOptionMapping(CopyOptionKind.Compression, CodeGenerationSupporter.Compression);
            this.AddOptionMapping(CopyOptionKind.FieldQuote, CodeGenerationSupporter.FieldQuote);
            this.AddOptionMapping(CopyOptionKind.FieldTerminator, CodeGenerationSupporter.FieldTerminator);
            this.AddOptionMapping(CopyOptionKind.RowTerminator, CodeGenerationSupporter.RowTerminator);
            this.AddOptionMapping(CopyOptionKind.FirstRow, CodeGenerationSupporter.FirstRow);
            this.AddOptionMapping(CopyOptionKind.DateFormat, CodeGenerationSupporter.DateFormat);
            this.AddOptionMapping(CopyOptionKind.Encoding, CodeGenerationSupporter.Encoding);
            this.AddOptionMapping(CopyOptionKind.Identity_Insert, CodeGenerationSupporter.Identity_Insert);
            this.AddOptionMapping(CopyOptionKind.Credential, CodeGenerationSupporter.Credential);
            this.AddOptionMapping(CopyOptionKind.ErrorFileCredential, CodeGenerationSupporter.ErrorFileCredential);
        }

        /// <summary>
        /// Helper method to assign copy option values.
        /// </summary>
        /// <param name="copyOption">Copy option.</param>
        /// <param name="singleValueTypeOption">The option value.</param>
        /// <remarks>This method is used by the parser to construct the CopyOption object.
        /// Please look at copyOption rule in Dsql10.g to understand its usage.</remarks>
        internal void AssignValueToCopyOption(CopyOption copyOption, SingleValueTypeCopyOption singleValueTypeOption)
        {
            if (copyOption == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "copyOption"));
            }
            else if (singleValueTypeOption == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "singleValueTypeOption"));
            }
            else if (singleValueTypeOption.SingleValue == null)
            {
                throw new ArgumentNullException(string.Format(CultureInfo.CurrentCulture, "singleValueTypeOption.SingleValue"));
            }

            string literalValue;

            switch (copyOption.Kind)
            {
                case CopyOptionKind.File_Format:
                    Identifier identifier = singleValueTypeOption.SingleValue.Identifier;
                    if (identifier == null || identifier.Value == null)
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.FormatType)));
                    }

                    this.CheckForEmptyValues(CodeGenerationSupporter.FileFormat, identifier.Value.Length);

                    break;
                case CopyOptionKind.File_Type:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.FileType, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);

                    CopyCommandFileFormatType fileType;
                    if (!Enum.TryParse<CopyCommandFileFormatType>(literalValue, true, out fileType))
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.FileType)));
                    }

                    break;
                case CopyOptionKind.ErrorFile:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.ErrorFile, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);
                    ExternalFileOption.CheckXMLValidity(literalValue, CodeGenerationSupporter.ErrorFile);

                    break;
                case CopyOptionKind.MaxErrors:
                    string maxErrors = singleValueTypeOption.SingleValue.Value;
                    int maxErrorsIntValue;
                    if (maxErrors == null
                        || !int.TryParse(maxErrors, out maxErrorsIntValue)
                        || maxErrorsIntValue < 0)
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.MaxErrors)));
                    }

                    break;
                case CopyOptionKind.Compression:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.Compression, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);

                    CopyCommandCompressionType compression;
                    if (!Enum.TryParse<CopyCommandCompressionType>(literalValue, true, out compression))
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.Compression)));
                    }

                    break;
                case CopyOptionKind.FieldQuote:
                    this.ValidateDelimiters(CodeGenerationSupporter.FieldQuote, singleValueTypeOption.SingleValue.ValueExpression);

                    break;
                case CopyOptionKind.FieldTerminator:
                    this.ValidateDelimiters(CodeGenerationSupporter.FieldTerminator, singleValueTypeOption.SingleValue.ValueExpression);

                    break;
                case CopyOptionKind.RowTerminator:
                    this.ValidateDelimiters(CodeGenerationSupporter.RowTerminator, singleValueTypeOption.SingleValue.ValueExpression);

                    break;
                case CopyOptionKind.FirstRow:
                    string firstRow = singleValueTypeOption.SingleValue.Value;

                    int firstRowIntValue;
                    if (firstRow == null
                        || !int.TryParse(firstRow, out firstRowIntValue)
                        || firstRowIntValue <= 0)
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.FirstRow)));
                    }

                    break;
                case CopyOptionKind.DateFormat:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.DateFormat, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);

                    CopyCommandDateFormat dateFormat;
                    if (!Enum.TryParse<CopyCommandDateFormat>(literalValue, true, out dateFormat))
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.DateFormat)));
                    }

                    break;
                case CopyOptionKind.Encoding:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.Encoding, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);

                    CopyCommandEncoding encoding;
                    if (!Enum.TryParse<CopyCommandEncoding>(literalValue, true, out encoding))
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.Encoding)));
                    }

                    break;
                case CopyOptionKind.Identity_Insert:
                    literalValue = this.ValidateAsStringLiteral(CodeGenerationSupporter.Identity_Insert, singleValueTypeOption.SingleValue.ValueExpression, checkForEmptyValues: true);

                    CopyCommandIdentityInsert identityValue;
                    if (!Enum.TryParse<CopyCommandIdentityInsert>(literalValue, true, out identityValue))
                    {
                        throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, CodeGenerationSupporter.Identity_Insert)));
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(string.Format(CultureInfo.CurrentCulture, "Out of range argument: {0}", copyOption.Kind));
            }

            copyOption.Value = singleValueTypeOption;
        }

        /// <summary>
        /// Helper method to assign copy option values.
        /// </summary>
        /// <param name="credentialOption">Copy statement credential option.</param>
        /// <returns>True, if credential option contains valid values, else False.</returns>
        internal bool ValidateCopyCredential(CopyCredentialOption credentialOption)
        {
            CopyCommandCredentialType credType;

            if (GetCredentialType(credentialOption.Identity.Value, out credType))
            {
                // Some credential types require a secret, others don't. Check it here.
                switch (credType)
                {
                    case CopyCommandCredentialType.Sas:
                    case CopyCommandCredentialType.AccountKey:
                    case CopyCommandCredentialType.ServicePrincipal:
                        if (credentialOption.Secret != null && credentialOption.Secret.Value.Trim().Length != 0)
                        {
                            return true;
                        }

                        break;
                    case CopyCommandCredentialType.ManagedIdentity:
                        if (credentialOption.Secret == null)
                        {
                            return true;
                        }

                        break;
                    default:
                        break; /* noop */
                }
            }

            return false;
        }

        /// <summary>
        /// Validates secret option.
        /// </summary>
        /// <param name="valueExpression">Value expression containing the value.</param>
        /// <returns>Value of literal.</returns>
        internal void ValidateSecret(ValueExpression valueExpression)
        {
            string value = this.ValidateAsStringLiteral(CodeGenerationSupporter.Secret, valueExpression);
            if (value.Length > this.maxLength)
            {
                throw new TSqlParseErrorException(new ParseError(46131, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46131Message, CodeGenerationSupporter.Secret)));
            }

            if (value.Length > 0)
            {
                ExternalFileOption.CheckXMLValidity(value, CodeGenerationSupporter.Secret);
                CheckForInvalidChars(value);
            }
        }

        /// <summary>
        /// Checks for non ascii characters in access key.
        /// </summary>
        /// <param name="key">Access key.</param>
        private static void CheckForInvalidChars(string key)
        {
            // Check if the key contains any non ascii (uni code) characters.
            foreach (char token in key)
            {
                if (token > MaxAsciiCharValue)
                {
                    throw new TSqlParseErrorException(new ParseError(46132, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46132Message)));
                }
            }
        }
        /// <summary>
        /// Checks for empty values.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="length">Length of the option's value.</param>
        private void CheckForEmptyValues(string optionName, int length)
        {
            if (length <= 0)
            {
                throw new TSqlParseErrorException(new ParseError(46131, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46131Message, optionName)));
            }
        }

        /// <summary>
        /// Validates the delimiters.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="valueExpression">Value expression containing the value.</param>
        private void ValidateDelimiters(string optionName, ValueExpression valueExpression)
        {
            int length = this.maxLength;

            string value;
            if (optionName.Equals(CodeGenerationSupporter.FieldQuote))
            {
                value = this.ValidateAsStringLiteral(optionName, valueExpression, checkForEmptyValues: false);
            }
            else
            {
                value = this.ValidateAsStringLiteral(optionName, valueExpression, checkForEmptyValues: true);
            }

            // Fieldquote can have 0 or 1 character. If value is passed as hexadecimal than max length is 4.
            if (optionName.Equals(CodeGenerationSupporter.FieldQuote))
            {
                if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
                {
                    length = hexDecimalFieldQuoteMaxLen;
                }
                else
                {
                    length = value.Length == 0 ? 0 : fieldQuoteMaxLength;
                }
            }

            if (value.Length > length)
            {
                throw new TSqlParseErrorException(new ParseError(46131, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46131Message, optionName)));
            }

            ExternalFileOption.CheckDelimiterValidity(value, optionName);
        }

        /// <summary>
        /// Validates the string literal.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="valueExpression">Value expression containing the value.</param>
        /// <param name="checkForEmptyValues">If true then error is thrown if value is empty.</param>
        /// <returns>Value of the expression.</returns>
        private string ValidateAsStringLiteral(string optionName, ValueExpression valueExpression, bool checkForEmptyValues)
        {
            string value = this.ValidateAsStringLiteral(optionName, valueExpression);
            if (checkForEmptyValues)
            {
                this.CheckForEmptyValues(optionName, value.Length);
            }

            return value;
        }

        /// <summary>
        /// Validates the string literal.
        /// </summary>
        /// <param name = "optionName" > Name of the option.</param>
        /// <param name = "valueExpression" > Value expression containing the value.</param>
        /// <returns>Value of the expression.</returns>
        private string ValidateAsStringLiteral(string optionName, ValueExpression valueExpression)
        {
            StringLiteral literal = null;
            if (valueExpression != null)
            {
                literal = valueExpression as StringLiteral;
            }

            if (valueExpression == null || literal == null || literal.Value == null)
            {
                throw new TSqlParseErrorException(new ParseError(46130, 1, 1, 1, string.Format(CultureInfo.CurrentCulture, TSqlParserResource.SQL46130Message, optionName)));
            }

            return literal.Value;
        }
        /// <summary>
        /// Given the string value of the credential identity, it returns the type of credential used.
        /// </summary>
        /// <param name="credentialIdentity">Input string containing the credential identity.</param>
        /// <param name="credentialType">Output credential type.</param>
        /// <returns>Indicates whether the string to type mapping was successful.</returns>
        public static bool GetCredentialType(string credentialIdentity, out CopyCommandCredentialType credentialType)
        {
            credentialType = CopyCommandCredentialType.None;

            // Try looking up a static string disctionary first, then check if service principal
            if (credentialTypeMapping.TryGetValue(credentialIdentity, out credentialType))
            {
                return true;
            }
            return false;
        }

    }
}
