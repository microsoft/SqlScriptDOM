//------------------------------------------------------------------------------
// <copyright file="TSqlAuditEventGroupHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Exposes the audit event groups.
    /// </summary>
    public static class TSqlAuditEventGroupHelper
    {
        private static readonly AuditEventGroupHelper HelperInstance = AuditEventGroupHelper.Instance;

        /// <summary>
        /// Tries to parse the input string into the audit event group.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="version"></param>
        /// <param name="returnValue">The output event notificiation event group for the input string.</param>
        /// <returns>Returns true if the input string represents a valid Audit Event Group on the specified sql version, else false.</returns>
        public static bool TryParseOption(string input, SqlVersion version, out EventNotificationEventGroup returnValue)
        {
			return HelperInstance.TryParseOption(input, HelperInstance.MapSqlVersionToSqlVersionFlags(version), out returnValue);
        }
    }
}
