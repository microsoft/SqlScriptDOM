//------------------------------------------------------------------------------
// <copyright file="AuditEventGroupHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AuditEventGroupHelper : OptionsHelper<EventNotificationEventGroup>
    {
        private AuditEventGroupHelper()
        {
            AddOptionMapping(EventNotificationEventGroup.TrcClr, CodeGenerationSupporter.TrcClr, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcDatabase, CodeGenerationSupporter.TrcDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcDeprecation, CodeGenerationSupporter.TrcDeprecation, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcErrorsAndWarnings, CodeGenerationSupporter.TrcErrorsAndWarnings, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcFullText, CodeGenerationSupporter.TrcFullText, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcLocks, CodeGenerationSupporter.TrcLocks, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcObjects, CodeGenerationSupporter.TrcObjects, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcOledb, CodeGenerationSupporter.TrcOledb, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcPerformance, CodeGenerationSupporter.TrcPerformance, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcQueryNotifications, CodeGenerationSupporter.TrcQueryNotifications, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcSecurityAudit, CodeGenerationSupporter.TrcSecurityAudit, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcServer, CodeGenerationSupporter.TrcServer, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcStoredProcedures, CodeGenerationSupporter.TrcStoredProcedures, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcTSql, CodeGenerationSupporter.TrcTsql, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcUserConfigurable, CodeGenerationSupporter.TrcUserConfigurable, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.TrcAllEvents, CodeGenerationSupporter.TrcAllEvents, SqlVersionFlags.TSql90AndAbove);
        }

        internal static readonly AuditEventGroupHelper Instance = new AuditEventGroupHelper();
    }
}
