//------------------------------------------------------------------------------
// <copyright file="TSql90ParserBaseInternal.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using antlr;
using System.Globalization;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal abstract class TSql90ParserBaseInternal : TSql80ParserBaseInternal
    {
        #region Constructors 

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql90ParserBaseInternal(TokenBuffer tokenBuf, int k)
            : base(tokenBuf, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql90ParserBaseInternal(ParserSharedInputState state, int k)
            : base(state, k)
        {
        }

        // Not really needed, here only because ANTLR generates call to this one in derived classes
        protected TSql90ParserBaseInternal(TokenStream lexer, int k)
            : base(lexer, k)
        {
        }

        /// <summary>
        /// Real constructor (the one which is used)
        /// </summary>
        /// <param name="initialQuotedIdentifiersOn">if set to <c>true</c> quoted identifiers will be on.</param>
        public TSql90ParserBaseInternal(bool initialQuotedIdentifiersOn)
            : base(initialQuotedIdentifiersOn)
        {
        }

        #endregion

        /// <summary>
        /// Aggregates AUTHENTICATION option with the current options and check that the new option wasn't there before.
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static AuthenticationTypes AggregateAuthenticationType(AuthenticationTypes current, AuthenticationTypes newOption, antlr.IToken token)
        {
            AuthenticationTypes aggregatedOptions = current | newOption;

            if (aggregatedOptions == current)
                throw GetUnexpectedTokenErrorException(token);

            return aggregatedOptions;
        }

        protected const long BulkInsertOptionsProhibitedInOpenRowset =
                    (1L << (int)BulkInsertOptionKind.BatchSize) |
                    (1L << (int)BulkInsertOptionKind.KilobytesPerBatch);

        const long CheckForFormatFileOptionInOpenRowsetBulkMask =
            (1L << (int)BulkInsertOptionKind.FormatFile) |
            (1L << (int)BulkInsertOptionKind.SingleBlob) |
            (1L << (int)BulkInsertOptionKind.SingleClob) |
            (1L << (int)BulkInsertOptionKind.SingleNClob);

        protected static void CheckForFormatFileOptionInOpenRowsetBulk(long encounteredOptions, TSqlFragment relatedFragment)
        {
            if ((encounteredOptions & CheckForFormatFileOptionInOpenRowsetBulkMask) == 0)
            {
                ThrowParseErrorException("SQL46082", relatedFragment, TSqlParserResource.SQL46082Message);
            }
        }

        /// <summary>
        /// Aggregates PORTS option with the current options and check that the new option wasn't there before.
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static PortTypes AggregatePortType(PortTypes current, PortTypes newOption, antlr.IToken token)
        {
            PortTypes aggregatedOptions = current | newOption;

            if (aggregatedOptions == current)
                throw GetUnexpectedTokenErrorException(token);

            return aggregatedOptions;
        }

        /// <summary>
        /// Checks certification options duplication
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        /// <returns>The aggregated value for options.</returns>
        protected static void CheckCertificateOptionDupication(CertificateOptionKinds current, CertificateOptionKinds newOption, antlr.IToken token)
        {
            if ((current & newOption) == newOption)
                throw GetUnexpectedTokenErrorException(token);
        }

        /// <summary>
        /// Checks endpoint protocol options duplication, and if protocol allows that option as well
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="protocol">The protocol.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        protected static void CheckIfEndpointOptionAllowed(EndpointProtocolOptions current,
            EndpointProtocolOptions newOption, EndpointProtocol protocol, antlr.IToken token)
        {
            if ((current & newOption) == newOption)
                throw GetUnexpectedTokenErrorException(token);

            if ((protocol == EndpointProtocol.Tcp && (newOption & EndpointProtocolOptions.TcpOptions) != newOption) ||
                (protocol == EndpointProtocol.Http && (newOption & EndpointProtocolOptions.HttpOptions) != newOption))
                throw GetUnexpectedTokenErrorException(token);
        }

        /// <summary>
        /// Checks payload options duplication, and if endpoint type allows that option as well
        /// </summary>
        /// <param name="current">The enum that the newOption will be added.</param>
        /// <param name="newOption">The new option to be added.</param>
        /// <param name="endpointType">Type of the endpoint.</param>
        /// <param name="token">The token that was parsed for the new option.</param>
        protected static void CheckIfPayloadOptionAllowed(PayloadOptionKinds current,
            PayloadOptionKinds newOption, EndpointType endpointType, antlr.IToken token)
        {
            if (endpointType == EndpointType.TSql) // No options for TSql
                throw GetUnexpectedTokenErrorException(token);

            if ((endpointType == EndpointType.Soap && (newOption & PayloadOptionKinds.SoapOptions) != newOption) ||
                (endpointType == EndpointType.DatabaseMirroring && (newOption & PayloadOptionKinds.DatabaseMirroringOptions) != newOption) ||
                (endpointType == EndpointType.ServiceBroker && (newOption & PayloadOptionKinds.ServiceBrokerOptions) != newOption))
                throw GetUnexpectedTokenErrorException(token);

            if ((current & newOption) == newOption && newOption != PayloadOptionKinds.WebMethod)
                throw GetUnexpectedTokenErrorException(token);
        }

        /// <summary>
        /// Parses security object kind.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <returns>The type.</returns>
        protected static SecurityObjectKind ParseSecurityObjectKind(Identifier identifier)
        {
            switch (identifier.Value.ToUpperInvariant())
            {
                case CodeGenerationSupporter.Assembly:
                    return SecurityObjectKind.Assembly;
                case CodeGenerationSupporter.Certificate:
                    return SecurityObjectKind.Certificate;
                case CodeGenerationSupporter.Contract:
                    return SecurityObjectKind.Contract;
                case CodeGenerationSupporter.Database:
                    return SecurityObjectKind.Database;
                case CodeGenerationSupporter.Endpoint:
                    return SecurityObjectKind.Endpoint;
                case CodeGenerationSupporter.Login:
                    return SecurityObjectKind.Login;
                case CodeGenerationSupporter.Object:
                    return SecurityObjectKind.Object;
                case CodeGenerationSupporter.Role:
                    return SecurityObjectKind.Role;
                case CodeGenerationSupporter.Route:
                    return SecurityObjectKind.Route;
                case CodeGenerationSupporter.Schema:
                    return SecurityObjectKind.Schema;
                case CodeGenerationSupporter.Server:
                    return SecurityObjectKind.Server;
                case CodeGenerationSupporter.Service:
                    return SecurityObjectKind.Service;
                case CodeGenerationSupporter.Type:
                    return SecurityObjectKind.Type;
                case CodeGenerationSupporter.User:
                    return SecurityObjectKind.User;
                default:
                    throw GetUnexpectedTokenErrorException(identifier);
            }
        }

        /// <summary>
        /// Parses security object kind.
        /// </summary>
        /// <param name="identifier1">The identifier 1.</param>
        /// <param name="identifier2">The identifier 2.</param>
        /// <returns>The type.</returns>
        protected static SecurityObjectKind ParseSecurityObjectKind(Identifier identifier1, Identifier identifier2)
        {
            switch (identifier1.Value.ToUpperInvariant())
            {
                case CodeGenerationSupporter.Application:
                    Match(identifier2, CodeGenerationSupporter.Role);
                    return SecurityObjectKind.ApplicationRole;
                case CodeGenerationSupporter.Asymmetric:
                    Match(identifier2, CodeGenerationSupporter.Key);
                    return SecurityObjectKind.AsymmetricKey;
                case CodeGenerationSupporter.Availability:
                    Match(identifier2, CodeGenerationSupporter.Group);
                    return SecurityObjectKind.AvailabilityGroup;
                case CodeGenerationSupporter.Fulltext:
                    if (TryMatch(identifier2, CodeGenerationSupporter.Catalog))
                        return SecurityObjectKind.FullTextCatalog;
                    else
                    {
                        Match(identifier2, CodeGenerationSupporter.StopList);
                        return SecurityObjectKind.FullTextStopList;
                    }
                case CodeGenerationSupporter.Message:
                    Match(identifier2, CodeGenerationSupporter.Type);
                    return SecurityObjectKind.MessageType;
                case CodeGenerationSupporter.Server:
                    Match(identifier2, CodeGenerationSupporter.Role);
                    return SecurityObjectKind.ServerRole;
                case CodeGenerationSupporter.Symmetric:
                    Match(identifier2, CodeGenerationSupporter.Key);
                    return SecurityObjectKind.SymmetricKey;
                default:
                    throw GetUnexpectedTokenErrorException(identifier1);
            }
        }

        /// <summary>
        /// Parses security object kind.
        /// </summary>
        /// <param name="identifier1">The identifier 1.</param>
        /// <param name="identifier2">The identifier 2.</param>
        /// <param name="identifier3">The identifier 3.</param>
        /// <returns>The type.</returns>
        protected static SecurityObjectKind ParseSecurityObjectKind(Identifier identifier1, Identifier identifier2, Identifier identifier3)
        {
            switch (identifier1.Value.ToUpperInvariant())
            {
                case CodeGenerationSupporter.Xml:
                    Match(identifier2, CodeGenerationSupporter.Schema);
                    Match(identifier3, CodeGenerationSupporter.Collection);
                    return SecurityObjectKind.XmlSchemaCollection;
                case CodeGenerationSupporter.Remote:
                    Match(identifier2, CodeGenerationSupporter.Service);
                    Match(identifier3, CodeGenerationSupporter.Binding);
                    return SecurityObjectKind.RemoteServiceBinding;
                case CodeGenerationSupporter.Search:
                    Match(identifier2, CodeGenerationSupporter.Property);
                    Match(identifier3, CodeGenerationSupporter.List);
                    return SecurityObjectKind.SearchPropertyList;
                default:
                    throw GetUnexpectedTokenErrorException(identifier1);
            }
        }

        protected static bool IsXml(Identifier identifier)
        {
            return String.Equals(identifier.Value, CodeGenerationSupporter.Xml, StringComparison.OrdinalIgnoreCase);
        }

        protected static bool IsSys(Identifier identifier)
        {
            return String.Equals(identifier.Value, CodeGenerationSupporter.Sys, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks if the we should try to parse statement in blocks, ending with END keyword
        /// (e.g. BEGIN/END, BEGIN TRY / END TRY)
        /// </summary>
        /// <returns>True if statement is next to follow, false if block end is next</returns>
        protected bool IsStatementIsNext()
        {
            return (LA(1) != TSql80ParserInternal.End || NextTokenMatches(CodeGenerationSupporter.Conversation, 2));
        }

        /// <summary>
        /// Unquotes a string
        /// </summary>
        /// <param name="value">value to unquote</param>
        /// <returns>The unquoted value if it could be unquoted</returns>
        public static string Unquote(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            int nFirst = value.IndexOf('\'');
            int nLast = value.LastIndexOf('\'');
            string retVal = value;
            if (nFirst == -1 || nLast == nFirst)
                return retVal;

            if (nFirst < 2 && nLast != nFirst && nLast == value.Length - 1)  //this means it is [N]'blah'
            {
                if (nFirst == 1)
                {
                    if (value[0] == 'N')    //only if this started with an N
                    {
                        retVal = value.Substring(nFirst + 1, nLast - nFirst - 1);
                    }
                }
                else
                {       //this is a 'blah' (no N)
                    retVal = value.Substring(nFirst + 1, nLast - nFirst);
                }

            }
            return retVal;
        }


        /// <summary>
        /// Checks, if identifier is equal to AES or RC4
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="tokenForError">The token for error.</param>
        /// <returns>
        /// returns AES or RC4 algorithm preference, or throws an error
        /// </returns>
        protected static EncryptionAlgorithmPreference RecognizeAesOrRc4(Identifier id, antlr.IToken tokenForError)
        {
            string unquotedId = Unquote(id.Value);
            if (String.Equals(unquotedId, CodeGenerationSupporter.Aes, StringComparison.OrdinalIgnoreCase))
                return EncryptionAlgorithmPreference.Aes;

            if (String.Equals(unquotedId, CodeGenerationSupporter.RC4, StringComparison.OrdinalIgnoreCase))
                return EncryptionAlgorithmPreference.Rc4;

            throw new TSqlParseErrorException(GetUnexpectedTokenError(tokenForError));
        }

        /// <summary>
        /// Checks, if identifier specifies NTLM, KERBEROS or NEGOTIATE authentication protocol option
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="tokenForError">The token for error.</param>
        /// <returns>
        /// returns authentication protocol, or throws an error
        /// </returns>
        protected static AuthenticationProtocol RecognizeAuthenticationProtocol(Identifier id, antlr.IToken tokenForError)
        {
            string unquotedId = Unquote(id.Value);
            if (String.Equals(unquotedId, CodeGenerationSupporter.Ntlm, StringComparison.OrdinalIgnoreCase))
                return AuthenticationProtocol.WindowsNtlm;

            if (String.Equals(unquotedId, CodeGenerationSupporter.Kerberos, StringComparison.OrdinalIgnoreCase))
                return AuthenticationProtocol.WindowsKerberos;

            if (String.Equals(unquotedId, CodeGenerationSupporter.Negotiate, StringComparison.OrdinalIgnoreCase))
                return AuthenticationProtocol.WindowsNegotiate;

            throw new TSqlParseErrorException(GetUnexpectedTokenError(tokenForError));
        }

        /// <summary>
        /// Recognizes the alter login sec admin password option.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <param name="astNode">The ast node.</param>
        protected static void RecognizeAlterLoginSecAdminPasswordOption(antlr.IToken token, PasswordAlterPrincipalOption astNode)
        {
            if (TryMatch(token, CodeGenerationSupporter.MustChange))
            {
                if (astNode.MustChange)
                    throw GetUnexpectedTokenErrorException(token);
                else
                    astNode.MustChange = true;
            }
            else if (TryMatch(token, CodeGenerationSupporter.Hashed))
            {
                if (astNode.Hashed)
                    throw GetUnexpectedTokenErrorException(token);
                else
                    astNode.Hashed = true;
            }
            else
            {
                Match(token, CodeGenerationSupporter.Unlock);
                if (astNode.Unlock)
                    throw GetUnexpectedTokenErrorException(token);
                else
                    astNode.Unlock = true;
            }
            UpdateTokenInfo(astNode, token);
        }

        protected static TValue EnableDisableMatcher<TValue>(IToken token, TValue enableValue, TValue disableValue)
        {
            if (TryMatch(token, CodeGenerationSupporter.Enable))
                return enableValue;
            else
            {
                Match(token, CodeGenerationSupporter.Disable);
                return disableValue;
            }
        }

        protected static void AddConstraintToComputedColumn(ConstraintDefinition constraint, ColumnDefinition column)
        {
            bool constraintIsNullable = false;
            if (constraint is NullableConstraintDefinition)
            {
                NullableConstraintDefinition nullableConstraint = (NullableConstraintDefinition) constraint;
                constraintIsNullable = nullableConstraint.Nullable;
            }
            if ((!column.IsPersisted) && !(constraint is UniqueConstraintDefinition) ||
                (column.IsPersisted && constraintIsNullable))
            {
                ThrowParseErrorException("SQL46011", constraint, TSqlParserResource.SQL46011Message);
            }
            AddAndUpdateTokenInfo(column, column.Constraints, constraint);
        }

        protected static IndexAffectingStatement GetAlterIndexStatementKind(AlterIndexStatement alterIndex)
        {
            if (alterIndex.AlterIndexType == AlterIndexType.Reorganize)
            {
                return IndexAffectingStatement.AlterIndexReorganize;
            }
            else if (alterIndex.AlterIndexType == AlterIndexType.Resume)
            {
                return IndexAffectingStatement.AlterIndexResume;
            }
            else
            {
                if (alterIndex.Partition != null && !alterIndex.Partition.All)
                    return IndexAffectingStatement.AlterIndexRebuildOnePartition;
                else
                    return IndexAffectingStatement.AlterIndexRebuildAllPartitions;
            }
        }

        protected static void CheckForDistinctInWindowedAggregate(FunctionCall functionCall, IToken distinctToken)
        {
            if (functionCall.UniqueRowFilter == UniqueRowFilter.Distinct &&
                functionCall.OverClause != null &&
                distinctToken != null)
            {
                ThrowParseErrorException("SQL46086",
                    distinctToken, TSqlParserResource.SQL46086Message);
            }
        }

        #region Utilities to handle IP v4 addresses

        protected Literal CreateIntLiteralFromNumericToken(antlr.IToken token, int textOffset, int textLength)
        {
            IntegerLiteral literal = FragmentFactory.CreateFragment<IntegerLiteral>();
            UpdateTokenInfo(literal, token);
            literal.Value = token.getText().Substring(textOffset, textLength);
            return literal;
        }

        // returns true if both parts are present
        protected bool SplitNumericIntoIpParts(antlr.IToken token, out Literal frag1, out Literal frag2)
        {
            string text = token.getText();

            int textLen = text.Length;

            int dotIndex = text.IndexOf('.');
            Debug.Assert(dotIndex != -1);

            if (dotIndex == 0) // only second part, .2
            {
                frag1 = null;
                frag2 = CreateIntLiteralFromNumericToken(token, 1, textLen - 1);
                return false;
            }
            else if (dotIndex == textLen - 1) // only first part, 1.
            {
                frag1 = CreateIntLiteralFromNumericToken(token, 0, dotIndex);
                frag2 = null;
                return false;
            }
            else // both parts, 1.2
            {
                frag1 = CreateIntLiteralFromNumericToken(token, 0, dotIndex);
                frag2 = CreateIntLiteralFromNumericToken(token, dotIndex + 1, textLen - dotIndex - 1);
                return true;
            }
        }

        protected Literal GetIPv4FragmentFromDotNumberNumeric(antlr.IToken token)
        {
            Literal frag1, frag2;

            SplitNumericIntoIpParts(token, out frag1, out frag2);
            if (frag1 != null || frag2 == null)
                throw GetUnexpectedTokenErrorException(token);

            return frag2;
        }

        protected Literal GetIPv4FragmentFromNumberDotNumeric(antlr.IToken token)
        {
            Literal frag1, frag2;

            SplitNumericIntoIpParts(token, out frag1, out frag2);
            if (frag1 == null || frag2 != null )
                throw GetUnexpectedTokenErrorException(token);

            return frag1;
        }

        protected void GetIPv4FragmentsFromNumberDotNumberNumeric(antlr.IToken token, out Literal frag1, out Literal frag2)
        {
            if (!SplitNumericIntoIpParts(token, out frag1, out frag2))
                throw GetUnexpectedTokenErrorException(token);
        }

        #endregion

        protected static void CheckDmlTriggerActionDuplication(int current, TriggerAction vTriggerAction)
        {
            if ((current & (1 << (int)(vTriggerAction.TriggerActionType))) != 0)
                ThrowParseErrorException("SQL46090", vTriggerAction, TSqlParserResource.SQL46090Message, vTriggerAction.TriggerActionType.ToString());
        }

        // Updates the running variable that keeps track of the options present in the input
        protected static void UpdateDmlTriggerActionEncounteredOptions(ref int encountered, TriggerAction vTriggerAction)
        {
            encountered = encountered | (1 << (int)(vTriggerAction.TriggerActionType));
        }

        protected static void ThrowIfInvalidListenerPortValue(Literal value)
        {
            Int32 outValue;
            if (!Int32.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out outValue) || (outValue > 32767) || (outValue < 1024))
            {
                ThrowParseErrorException("SQL46087", value, TSqlParserResource.SQL46087Message, value.Value);
            }
        }

        protected static void ThrowIfMaxdopValueOutOfRange(Literal value)
        {
            Int32 outValue;
            if (!Int32.TryParse(value.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out outValue) || (outValue > 32767) || (outValue < 0))
            {
                ThrowParseErrorException("SQL46091", value, TSqlParserResource.SQL46091Message, value.Value);
            }
        }

        protected EventTypeContainer CreateEventTypeContainer(EventNotificationEventType eventTypeValue, IToken token)
        {
            EventTypeContainer eventTypeOption = FragmentFactory.CreateFragment<EventTypeContainer>();
            eventTypeOption.EventType = eventTypeValue;
            UpdateTokenInfo(eventTypeOption, token);
            return eventTypeOption;
        }

        protected EventGroupContainer CreateEventGroupContainer(EventNotificationEventGroup eventGroupValue, IToken token)
        {
            EventGroupContainer eventGroupOption = FragmentFactory.CreateFragment<EventGroupContainer>();
            eventGroupOption.EventGroup = eventGroupValue;
            UpdateTokenInfo(eventGroupOption, token);
            return eventGroupOption;
        }
    }
}
