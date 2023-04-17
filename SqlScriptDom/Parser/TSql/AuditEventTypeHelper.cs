//------------------------------------------------------------------------------
// <copyright file="AuditEventTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AuditEventTypeHelper : OptionsHelper<EventNotificationEventType> 
    {
        private AuditEventTypeHelper()
        {
            // Yukon audit events
            AddOptionMapping(EventNotificationEventType.AssemblyLoad, CodeGenerationSupporter.AssemblyLoad, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAddDBUserEvent, CodeGenerationSupporter.AuditAddDbUserEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAddLoginEvent, CodeGenerationSupporter.AuditAddLoginEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAddLoginToServerRoleEvent, CodeGenerationSupporter.AuditAddLoginToServerRoleEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAddMemberToDBRoleEvent, CodeGenerationSupporter.AuditAddMemberToDbRoleEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAddRoleEvent, CodeGenerationSupporter.AuditAddRoleEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditAppRoleChangePasswordEvent, CodeGenerationSupporter.AuditAppRoleChangePasswordEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditBackupRestoreEvent, CodeGenerationSupporter.AuditBackupRestoreEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditChangeAuditEvent, CodeGenerationSupporter.AuditChangeAuditEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditChangeDatabaseOwner, CodeGenerationSupporter.AuditChangeDatabaseOwner, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseManagementEvent, CodeGenerationSupporter.AuditDatabaseManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseObjectAccessEvent, CodeGenerationSupporter.AuditDatabaseObjectAccessEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseObjectGdrEvent, CodeGenerationSupporter.AuditDatabaseObjectGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseObjectManagementEvent, CodeGenerationSupporter.AuditDatabaseObjectManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseObjectTakeOwnershipEvent, CodeGenerationSupporter.AuditDatabaseObjectTakeOwnershipEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseOperationEvent, CodeGenerationSupporter.AuditDatabaseOperationEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabasePrincipalImpersonationEvent, CodeGenerationSupporter.AuditDatabasePrincipalImpersonationEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabasePrincipalManagementEvent, CodeGenerationSupporter.AuditDatabasePrincipalManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDatabaseScopeGdrEvent, CodeGenerationSupporter.AuditDatabaseScopeGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditDbccEvent, CodeGenerationSupporter.AuditDbccEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLogin, CodeGenerationSupporter.AuditLogin, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLoginChangePasswordEvent, CodeGenerationSupporter.AuditLoginChangePasswordEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLoginChangePropertyEvent, CodeGenerationSupporter.AuditLoginChangePropertyEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLoginFailed, CodeGenerationSupporter.AuditLoginFailed, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLoginGdrEvent, CodeGenerationSupporter.AuditLoginGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditLogout, CodeGenerationSupporter.AuditLogout, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditSchemaObjectAccessEvent, CodeGenerationSupporter.AuditSchemaObjectAccessEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditSchemaObjectGdrEvent, CodeGenerationSupporter.AuditSchemaObjectGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditSchemaObjectManagementEvent, CodeGenerationSupporter.AuditSchemaObjectManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditSchemaObjectTakeOwnershipEvent, CodeGenerationSupporter.AuditSchemaObjectTakeOwnershipEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerAlterTraceEvent, CodeGenerationSupporter.AuditServerAlterTraceEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerObjectGdrEvent, CodeGenerationSupporter.AuditServerObjectGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerObjectManagementEvent, CodeGenerationSupporter.AuditServerObjectManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerObjectTakeOwnershipEvent, CodeGenerationSupporter.AuditServerObjectTakeOwnershipEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerOperationEvent, CodeGenerationSupporter.AuditServerOperationEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerPrincipalImpersonationEvent, CodeGenerationSupporter.AuditServerPrincipalImpersonationEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerPrincipalManagementEvent, CodeGenerationSupporter.AuditServerPrincipalManagementEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AuditServerScopeGdrEvent, CodeGenerationSupporter.AuditServerScopeGdrEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.BlockedProcessReport, CodeGenerationSupporter.BlockedProcessReport, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.BrokerQueueDisabled, CodeGenerationSupporter.BrokerQueueDisabled, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DataFileAutoGrow, CodeGenerationSupporter.DataFileAutoGrow, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DataFileAutoShrink, CodeGenerationSupporter.DataFileAutoShrink, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DatabaseMirroringStateChange, CodeGenerationSupporter.DatabaseMirroringStateChange, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DeadlockGraph, CodeGenerationSupporter.DeadlockGraph, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DeprecationAnnouncement, CodeGenerationSupporter.DeprecationAnnouncement, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DeprecationFinalSupport, CodeGenerationSupporter.DeprecationFinalSupport, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ErrorLog, CodeGenerationSupporter.ErrorLog, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.EventLog, CodeGenerationSupporter.EventLog, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.Exception, CodeGenerationSupporter.Exception, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ExchangeSpillEvent, CodeGenerationSupporter.ExchangeSpillEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ExecutionWarnings, CodeGenerationSupporter.ExecutionWarnings, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.FtCrawlAborted, CodeGenerationSupporter.FtCrawlAborted, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.FtCrawlStarted, CodeGenerationSupporter.FtCrawlStarted, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.FtCrawlStopped, CodeGenerationSupporter.FtCrawlStopped, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.HashWarning, CodeGenerationSupporter.HashWarning, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.LockDeadlock, CodeGenerationSupporter.LockDeadlock, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.LockDeadlockChain, CodeGenerationSupporter.LockDeadlockChain, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.LockEscalation, CodeGenerationSupporter.LockEscalation, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.LogFileAutoGrow, CodeGenerationSupporter.LogFileAutoGrow, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.LogFileAutoShrink, CodeGenerationSupporter.LogFileAutoShrink, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.MissingColumnStatistics, CodeGenerationSupporter.MissingColumnStatistics, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.MissingJoinPredicate, CodeGenerationSupporter.MissingJoinPredicate, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.MountTape, CodeGenerationSupporter.MountTape, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ObjectAltered, CodeGenerationSupporter.ObjectAltered, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ObjectCreated, CodeGenerationSupporter.ObjectCreated, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ObjectDeleted, CodeGenerationSupporter.ObjectDeleted, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.OledbCallEvent, CodeGenerationSupporter.OledbCallEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.OledbDataReadEvent, CodeGenerationSupporter.OledbDataReadEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.OledbErrors, CodeGenerationSupporter.OledbErrors, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.OledbProviderInformation, CodeGenerationSupporter.OledbProviderInformation, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.OledbQueryInterfaceEvent, CodeGenerationSupporter.OledbQueryInterfaceEvent, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.QnDynamics, CodeGenerationSupporter.QnDynamics, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.QnParameterTable, CodeGenerationSupporter.QnParameterTable, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.QnSubscription, CodeGenerationSupporter.QnSubscription, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.QnTemplate, CodeGenerationSupporter.QnTemplate, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.QueueActivation, CodeGenerationSupporter.QueueActivation, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ServerMemoryChange, CodeGenerationSupporter.ServerMemoryChange, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ShowPlanAllForQueryCompile, CodeGenerationSupporter.ShowPlanAllForQueryCompile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ShowPlanXmlForQueryCompile, CodeGenerationSupporter.ShowPlanXmlForQueryCompile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ShowPlanXml, CodeGenerationSupporter.ShowPlanXml, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.ShowPlanXmlStatisticsProfile, CodeGenerationSupporter.ShowPlanXmlStatisticsProfile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SortWarnings, CodeGenerationSupporter.SortWarnings, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SpCacheInsert, CodeGenerationSupporter.SpCacheInsert, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SpCacheMiss, CodeGenerationSupporter.SpCacheMiss, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SpCacheRemove, CodeGenerationSupporter.SpCacheRemove, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SpRecompile, CodeGenerationSupporter.SpRecompile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.SqlStmtRecompile, CodeGenerationSupporter.SqlStmtRecompile, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.TraceFileClose, CodeGenerationSupporter.TraceFileClose, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserErrorMessage, CodeGenerationSupporter.UserErrorMessage, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable0, CodeGenerationSupporter.UserConfigurable0, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable1, CodeGenerationSupporter.UserConfigurable1, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable2, CodeGenerationSupporter.UserConfigurable2, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable3, CodeGenerationSupporter.UserConfigurable3, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable4, CodeGenerationSupporter.UserConfigurable4, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable5, CodeGenerationSupporter.UserConfigurable5, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable6, CodeGenerationSupporter.UserConfigurable6, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable7, CodeGenerationSupporter.UserConfigurable7, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable8, CodeGenerationSupporter.UserConfigurable8, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UserConfigurable9, CodeGenerationSupporter.UserConfigurable9, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.XQueryStaticType, CodeGenerationSupporter.XQueryStaticType, SqlVersionFlags.TSql90AndAbove);

            // Katmai audit events
            AddOptionMapping(EventNotificationEventType.AuditFullText, CodeGenerationSupporter.AuditFullText, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.BitmapWarning, CodeGenerationSupporter.BitmapWarning, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CpuThresholdExceeded, CodeGenerationSupporter.CpuThresholdExceeded, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DatabaseSuspectDataPage, CodeGenerationSupporter.DatabaseSuspectDataPage, SqlVersionFlags.TSql100AndAbove);
        }

        internal static readonly AuditEventTypeHelper Instance = new AuditEventTypeHelper();
    }
}
