//------------------------------------------------------------------------------
// <copyright file="ServerAuditActionGroupHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ServerAuditActionGroupHelper : OptionsHelper<AuditActionGroup>
    {
        private ServerAuditActionGroupHelper()
        {
            // Server-only level groups
            AddOptionMapping(AuditActionGroup.SuccessfulLogin, CodeGenerationSupporter.SuccessfulLoginGroup);
            AddOptionMapping(AuditActionGroup.Logout, CodeGenerationSupporter.LogoutGroup);
            AddOptionMapping(AuditActionGroup.ServerStateChange, CodeGenerationSupporter.ServerStateChangeGroup);
            AddOptionMapping(AuditActionGroup.FailedLogin, CodeGenerationSupporter.FailedLoginGroup);
            AddOptionMapping(AuditActionGroup.LoginChangePassword, CodeGenerationSupporter.LoginChangePasswordGroup);
            AddOptionMapping(AuditActionGroup.ServerRoleMemberChange, CodeGenerationSupporter.ServerRoleMemberChangeGroup);
            AddOptionMapping(AuditActionGroup.ServerPrincipalImpersonation, CodeGenerationSupporter.ServerPrincipalImpersonationGroup);
            AddOptionMapping(AuditActionGroup.ServerObjectOwnershipChange, CodeGenerationSupporter.ServerObjectOwnershipChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseMirroringLogin, CodeGenerationSupporter.DatabaseMirroringLoginGroup);
            AddOptionMapping(AuditActionGroup.BrokerLogin, CodeGenerationSupporter.BrokerLoginGroup);
            AddOptionMapping(AuditActionGroup.ServerPermissionChange, CodeGenerationSupporter.ServerPermissionChangeGroup);
            AddOptionMapping(AuditActionGroup.ServerObjectPermissionChange, CodeGenerationSupporter.ServerObjectPermissionChangeGroup);
            AddOptionMapping(AuditActionGroup.ServerOperation, CodeGenerationSupporter.ServerOperationGroup);
            AddOptionMapping(AuditActionGroup.TraceChange, CodeGenerationSupporter.TraceChangeGroup);
            AddOptionMapping(AuditActionGroup.ServerObjectChange, CodeGenerationSupporter.ServerObjectChangeGroup);
            AddOptionMapping(AuditActionGroup.ServerPrincipalChange, CodeGenerationSupporter.ServerPrincipalChangeGroup);
            AddOptionMapping(AuditActionGroup.TransactionBegin, CodeGenerationSupporter.TransactionBeginGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionCommit, CodeGenerationSupporter.TransactionCommitGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionRollback, CodeGenerationSupporter.TransactionRollbackGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.StatementRollback, CodeGenerationSupporter.StatementRollbackGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionGroup, CodeGenerationSupporter.TransactionGroup, SqlVersionFlags.TSql110AndAbove);
            
            // Database & server level groups
            AddOptionMapping(AuditActionGroup.DatabasePermissionChange, CodeGenerationSupporter.DatabasePermissionChangeGroup);
            AddOptionMapping(AuditActionGroup.SchemaObjectPermissionChange, CodeGenerationSupporter.SchemaObjectPermissionChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseRoleMemberChange, CodeGenerationSupporter.DatabaseRoleMemberChangeGroup);
            AddOptionMapping(AuditActionGroup.ApplicationRoleChangePassword, CodeGenerationSupporter.ApplicationRoleChangePasswordGroup);
            AddOptionMapping(AuditActionGroup.SchemaObjectAccess, CodeGenerationSupporter.SchemaObjectAccessGroup);
            AddOptionMapping(AuditActionGroup.BackupRestore, CodeGenerationSupporter.BackupRestoreGroup);
            AddOptionMapping(AuditActionGroup.Dbcc, CodeGenerationSupporter.DbccGroup);
            AddOptionMapping(AuditActionGroup.AuditChange, CodeGenerationSupporter.AuditChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseChange, CodeGenerationSupporter.DatabaseChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseObjectChange, CodeGenerationSupporter.DatabaseObjectChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabasePrincipalChange, CodeGenerationSupporter.DatabasePrincipalChangeGroup);
            AddOptionMapping(AuditActionGroup.SchemaObjectChange, CodeGenerationSupporter.SchemaObjectChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabasePrincipalImpersonation, CodeGenerationSupporter.DatabasePrincipalImpersonationGroup);
            AddOptionMapping(AuditActionGroup.DatabaseObjectOwnershipChange, CodeGenerationSupporter.DatabaseObjectOwnershipChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseOwnershipChange, CodeGenerationSupporter.DatabaseOwnershipChangeGroup);
            AddOptionMapping(AuditActionGroup.SchemaObjectOwnershipChange, CodeGenerationSupporter.SchemaObjectOwnershipChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseObjectPermissionChange, CodeGenerationSupporter.DatabaseObjectPermissionChangeGroup);
            AddOptionMapping(AuditActionGroup.DatabaseOperation, CodeGenerationSupporter.DatabaseOperationGroup);
            AddOptionMapping(AuditActionGroup.DatabaseObjectAccess, CodeGenerationSupporter.DatabaseObjectAccessGroup);
            AddOptionMapping(AuditActionGroup.SuccessfulDatabaseAuthenticationGroup, CodeGenerationSupporter.SuccessfulDatabaseAuthenticationGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.FailedDatabaseAuthenticationGroup, CodeGenerationSupporter.FailedDatabaseAuthenticationGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseLogoutGroup, CodeGenerationSupporter.DatabaseLogoutGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.UserChangePasswordGroup, CodeGenerationSupporter.UserChangePasswordGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.UserDefinedAuditGroup, CodeGenerationSupporter.UserDefinedAuditGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.BatchCompletedGroup, CodeGenerationSupporter.BatchCompletedGroup, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(AuditActionGroup.BatchStartedGroup, CodeGenerationSupporter.BatchStartedGroup, SqlVersionFlags.TSql150AndAbove);
        }

        internal static readonly ServerAuditActionGroupHelper Instance = new ServerAuditActionGroupHelper();
    }
}
