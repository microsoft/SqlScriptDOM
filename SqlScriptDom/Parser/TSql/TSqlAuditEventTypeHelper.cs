//------------------------------------------------------------------------------
// <copyright file="TSqlAuditEventTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Exposes the audit event types.
    /// </summary>
    public static class TSqlAuditEventTypeHelper
    {
        private static readonly AuditEventTypeHelper HelperInstance = AuditEventTypeHelper.Instance;

        /// <summary>
        /// Tries to parse the input string into the audit event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version"></param>
        /// <param name="returnValue">The output event notificiation event type for the input string.</param>
        /// <returns>Returns true if the input string represents a valid audit event group on the specified sql version, else false.</returns>
        public static bool TryParseOption(string input, SqlVersion version, out EventNotificationEventType returnValue)
        {
			return HelperInstance.TryParseOption(input, HelperInstance.MapSqlVersionToSqlVersionFlags(version), out returnValue);
        }
    }
}
