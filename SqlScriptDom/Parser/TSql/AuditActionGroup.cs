//------------------------------------------------------------------------------
// <copyright file="AuditActionGroup.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of audit action group
    /// </summary>   
    public enum AuditActionGroup
    {
        None = 0,

        // Server-level only groups
        SuccessfulLogin,
        Logout,
        ServerStateChange,
        FailedLogin,
        LoginChangePassword,
        ServerRoleMemberChange,
        ServerPrincipalImpersonation,
        ServerObjectOwnershipChange,
        DatabaseMirroringLogin,
        BrokerLogin,
        ServerPermissionChange,
        ServerObjectPermissionChange,
        ServerOperation,
        TraceChange,
        ServerObjectChange,
        ServerPrincipalChange,

        // Database and server-level groups
        DatabasePermissionChange,
        SchemaObjectPermissionChange,
        DatabaseRoleMemberChange,
        ApplicationRoleChangePassword,
        SchemaObjectAccess,
        BackupRestore,
        Dbcc,
        AuditChange,
        DatabaseChange,
        DatabaseObjectChange,
        DatabasePrincipalChange,
        SchemaObjectChange,
        DatabasePrincipalImpersonation,
        DatabaseObjectOwnershipChange,
        DatabaseOwnershipChange,
        SchemaObjectOwnershipChange,
        DatabaseObjectPermissionChange,
        DatabaseOperation,
        DatabaseObjectAccess,

        //110 level actions
        SuccessfulDatabaseAuthenticationGroup,
        FailedDatabaseAuthenticationGroup,
        DatabaseLogoutGroup,
        UserChangePasswordGroup,
        UserDefinedAuditGroup,

        //130 level actions
        TransactionBegin,
        TransactionCommit,
        TransactionRollback,
        StatementRollback,
        TransactionGroup,

        //150 level actions
        BatchCompletedGroup,
        BatchStartedGroup,
    }

#pragma warning restore 1591
}
