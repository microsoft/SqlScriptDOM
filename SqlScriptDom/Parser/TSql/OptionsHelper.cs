//------------------------------------------------------------------------------
// <copyright file="OptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics;

using Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator;
using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Helper to deal with option lists (like in DECLARE CURSOR, for example)
    /// </summary>
    /// <typeparam name="OptionType">Enum representing option</typeparam>
    internal abstract class OptionsHelper<OptionType>
        where OptionType : struct, IConvertible
    {
        private class OptionInfo
        {
            #region Constructors
            public OptionInfo(OptionType optionValue, TSqlTokenType tokenId, SqlVersionFlags appliesToVersion)
            {
                _optionValue = optionValue;
                _tokenId = tokenId;
                _identifier = null;
                _validVersions = appliesToVersion;
            }

            public OptionInfo(OptionType optionValue, string identifier, SqlVersionFlags validVersions)
            {
                _optionValue = optionValue;
                _tokenId = 0;
                _identifier = identifier;
                _validVersions = validVersions;
            }
            #endregion

            #region Private fields

            readonly OptionType _optionValue;
            readonly TSqlTokenType _tokenId;
            readonly string _identifier;
            // This is bit flags variable!
            readonly SqlVersionFlags _validVersions;

            #endregion

            public void GenerateSource(ScriptWriter writer)
            {
                if (_identifier != null)
                    writer.AddIdentifierWithoutCasing(_identifier);
                else
                    writer.AddKeyword(_tokenId);
            }

            public bool IsValidIn(SqlVersionFlags version)
            {
                return (_validVersions & version) != 0;
            }

            public OptionType Value
            {
                get { return _optionValue; }
            }
        }

        private Dictionary<OptionType, OptionInfo> _optionToOptionInfo = new Dictionary<OptionType, OptionInfo>();
        private Dictionary<string, OptionInfo> _stringToOptionInfo = new Dictionary<string, OptionInfo>(StringComparer.OrdinalIgnoreCase);

        protected void AddOptionMapping(OptionType option, string identifier, SqlVersionFlags validVersions)
        {
            OptionInfo optionInfo = new OptionInfo(option, identifier, validVersions);

            _optionToOptionInfo.Add(option, optionInfo);
            _stringToOptionInfo.Add(identifier, optionInfo);
        }

        protected void AddOptionMapping(OptionType option, TSqlTokenType tokenId, SqlVersionFlags validVersions)
        {
            OptionInfo optionInfo = new OptionInfo(option, tokenId, validVersions);

            _optionToOptionInfo.Add(option, optionInfo);
            _stringToOptionInfo.Add(ScriptGeneratorSupporter.GetLowerCase(tokenId), optionInfo);
        }

        protected void AddOptionMapping(OptionType option, string identifier)
        {
            AddOptionMapping(option, identifier, SqlVersionFlags.TSqlAll);
        }

        protected void AddOptionMapping(OptionType option, TSqlTokenType tokenId)
        {
            AddOptionMapping(option, tokenId, SqlVersionFlags.TSqlAll);
        }

        internal bool IsValidKeyword(antlr.IToken token)
        {
            Debug.Assert(token.Type == TSql80ParserInternal.Identifier);
            return _stringToOptionInfo.ContainsKey(token.getText());
        }

        internal SqlVersionFlags MapSqlVersionToSqlVersionFlags(SqlVersion sqlVersion)
        {
            switch (sqlVersion)
            {
                case SqlVersion.Sql80:
                    return SqlVersionFlags.TSql80;
                case SqlVersion.Sql90:
                    return SqlVersionFlags.TSql90;
                case SqlVersion.Sql100:
                    return SqlVersionFlags.TSql100;
                case SqlVersion.Sql110:
                    return SqlVersionFlags.TSql110;
                case SqlVersion.Sql120:
                    return SqlVersionFlags.TSql120;
                case SqlVersion.Sql130:
                    return SqlVersionFlags.TSql130;
                case SqlVersion.Sql140:
                    return SqlVersionFlags.TSql140;
                case SqlVersion.Sql150:
                    return SqlVersionFlags.TSql150;
                case SqlVersion.Sql160:
                    return SqlVersionFlags.TSql160;
                default:
                    throw new ArgumentException(String.Format(CultureInfo.CurrentCulture, SqlScriptGeneratorResource.UnknownEnumValue, sqlVersion, "SqlVersion"), "sqlVersion");
            }
        }

        internal OptionType ParseOption(antlr.IToken token, SqlVersionFlags version)
        {
            OptionInfo optionInfo;
            if (_stringToOptionInfo.TryGetValue(token.getText(), out optionInfo) &&
                optionInfo.IsValidIn(version))
            {
                return optionInfo.Value;
            }

            throw GetMatchingException(token);
        }

        internal bool TryParseOption(antlr.IToken token, SqlVersionFlags version, out OptionType returnValue)
        {
            Debug.Assert(token.Type == TSql80ParserInternal.Identifier);
            return TryParseOption(token.getText(), version, out returnValue);
        }

        internal bool TryParseOption(string tokenString, SqlVersionFlags version, out OptionType returnValue)
        {
            OptionInfo optionInfo;
            if (_stringToOptionInfo.TryGetValue(tokenString, out optionInfo) &&
                optionInfo.IsValidIn(version))
            {
                returnValue = optionInfo.Value;
                return true;
            }
            else
            {
                returnValue = default(OptionType);
                return false;
            }
        }

        internal OptionType ParseOption(antlr.IToken token)
        {
            return ParseOption(token, SqlVersionFlags.TSqlAll);
        }

        internal bool TryParseOption(antlr.IToken token, out OptionType returnValue)
        {
            return TryParseOption(token, SqlVersionFlags.TSqlAll, out returnValue);
        }

        protected virtual TSqlParseErrorException GetMatchingException(antlr.IToken token)
        {
            return TSql80ParserBaseInternal.GetUnexpectedTokenErrorException(token);
        }

        internal void GenerateSourceForOption(ScriptWriter writer, OptionType option)
        {
            OptionInfo optionInfo;
            if (_optionToOptionInfo.TryGetValue(option, out optionInfo))
                optionInfo.GenerateSource(writer);
            else
            {
                System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
            }
        }

        internal bool TryGenerateSourceForOption(ScriptWriter writer, OptionType option)
        {
            OptionInfo optionInfo;
            if (_optionToOptionInfo.TryGetValue(option, out optionInfo))
            {
                optionInfo.GenerateSource(writer);
                return true;
            }
            return false;
        }

        internal void GenerateCommaSeparatedFlagOptions(ScriptWriter writer, OptionType options)
        {
            bool first = true;
            long optionsAsLong = options.ToInt64(CultureInfo.InvariantCulture.NumberFormat);
            foreach (OptionType mask in Enum.GetValues(typeof(OptionType)))
            {
                long maskAsLong = mask.ToInt64(CultureInfo.InvariantCulture.NumberFormat);
                if (!mask.Equals(default(OptionType)) && (optionsAsLong & maskAsLong) == maskAsLong)
                {
                    if (first)
                        first = false;
                    else
                    {
                        writer.AddKeyword(TSqlTokenType.Comma);
                        writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
                    }

                    GenerateSourceForOption(writer, mask);
                }
            }
        }
    }
}