//------------------------------------------------------------------------------
// <copyright file="DatabaseAuditActionGroupHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class DatabaseAuditActionGroupHelper : OptionsHelper<AuditActionGroup>
    {
        private DatabaseAuditActionGroupHelper()
        {
            AddOptionMapping(AuditActionGroup.DatabasePermissionChange, CodeGenerationSupporter.DatabasePermissionChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.SchemaObjectPermissionChange, CodeGenerationSupporter.SchemaObjectPermissionChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseRoleMemberChange, CodeGenerationSupporter.DatabaseRoleMemberChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.ApplicationRoleChangePassword, CodeGenerationSupporter.ApplicationRoleChangePasswordGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.SchemaObjectAccess, CodeGenerationSupporter.SchemaObjectAccessGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.BackupRestore, CodeGenerationSupporter.BackupRestoreGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.Dbcc, CodeGenerationSupporter.DbccGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.AuditChange, CodeGenerationSupporter.AuditChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseChange, CodeGenerationSupporter.DatabaseChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseObjectChange, CodeGenerationSupporter.DatabaseObjectChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabasePrincipalChange, CodeGenerationSupporter.DatabasePrincipalChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.SchemaObjectChange, CodeGenerationSupporter.SchemaObjectChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabasePrincipalImpersonation, CodeGenerationSupporter.DatabasePrincipalImpersonationGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseObjectOwnershipChange, CodeGenerationSupporter.DatabaseObjectOwnershipChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseOwnershipChange, CodeGenerationSupporter.DatabaseOwnershipChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.SchemaObjectOwnershipChange, CodeGenerationSupporter.SchemaObjectOwnershipChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseObjectPermissionChange, CodeGenerationSupporter.DatabaseObjectPermissionChangeGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseOperation, CodeGenerationSupporter.DatabaseOperationGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseObjectAccess, CodeGenerationSupporter.DatabaseObjectAccessGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(AuditActionGroup.SuccessfulDatabaseAuthenticationGroup, CodeGenerationSupporter.SuccessfulDatabaseAuthenticationGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.FailedDatabaseAuthenticationGroup, CodeGenerationSupporter.FailedDatabaseAuthenticationGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.DatabaseLogoutGroup, CodeGenerationSupporter.DatabaseLogoutGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.UserChangePasswordGroup, CodeGenerationSupporter.UserChangePasswordGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.UserDefinedAuditGroup, CodeGenerationSupporter.UserDefinedAuditGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionGroup, CodeGenerationSupporter.TransactionGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionBegin, CodeGenerationSupporter.TransactionBeginGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionCommit, CodeGenerationSupporter.TransactionCommitGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.TransactionRollback, CodeGenerationSupporter.TransactionRollbackGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(AuditActionGroup.StatementRollback, CodeGenerationSupporter.StatementRollbackGroup, SqlVersionFlags.TSql110AndAbove);            
            AddOptionMapping(AuditActionGroup.BatchCompletedGroup, CodeGenerationSupporter.BatchCompletedGroup, SqlVersionFlags.TSql150AndAbove);
            AddOptionMapping(AuditActionGroup.BatchStartedGroup, CodeGenerationSupporter.BatchStartedGroup, SqlVersionFlags.TSql150AndAbove);
        }

        internal static readonly DatabaseAuditActionGroupHelper Instance = new DatabaseAuditActionGroupHelper();
    }
}