CREATE SERVER AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1;

CREATE SERVER AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    WITH(STATE = ON);

CREATE SERVER AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (DATABASE_PERMISSION_CHANGE_GROUP)
    WITH(STATE = OFF);

CREATE SERVER AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (DATABASE_PERMISSION_CHANGE_GROUP),
    ADD (BATCH_COMPLETED_GROUP),
    ADD (BATCH_STARTED_GROUP),
    ADD (SCHEMA_OBJECT_PERMISSION_CHANGE_GROUP),
    ADD (DATABASE_ROLE_MEMBER_CHANGE_GROUP),
    ADD (APPLICATION_ROLE_CHANGE_PASSWORD_GROUP),
    ADD (SCHEMA_OBJECT_ACCESS_GROUP),
    ADD (BACKUP_RESTORE_GROUP),
    ADD (DBCC_GROUP),
    ADD (AUDIT_CHANGE_GROUP),
    ADD (DATABASE_CHANGE_GROUP),
    ADD (DATABASE_OBJECT_CHANGE_GROUP),
    ADD (DATABASE_PRINCIPAL_CHANGE_GROUP),
    ADD (SCHEMA_OBJECT_CHANGE_GROUP),
    ADD (DATABASE_PRINCIPAL_IMPERSONATION_GROUP),
    ADD (DATABASE_OBJECT_OWNERSHIP_CHANGE_GROUP),
    ADD (DATABASE_OWNERSHIP_CHANGE_GROUP),
    ADD (SCHEMA_OBJECT_OWNERSHIP_CHANGE_GROUP),
    ADD (DATABASE_OBJECT_PERMISSION_CHANGE_GROUP),
    ADD (DATABASE_OPERATION_GROUP),
    ADD (DATABASE_OBJECT_ACCESS_GROUP),
    ADD (SUCCESSFUL_LOGIN_GROUP),
    ADD (LOGOUT_GROUP),
    ADD (SERVER_STATE_CHANGE_GROUP),
    ADD (FAILED_LOGIN_GROUP),
    ADD (LOGIN_CHANGE_PASSWORD_GROUP),
    ADD (SERVER_ROLE_MEMBER_CHANGE_GROUP),
    ADD (SERVER_PRINCIPAL_IMPERSONATION_GROUP),
    ADD (SERVER_OBJECT_OWNERSHIP_CHANGE_GROUP),
    ADD (DATABASE_MIRRORING_LOGIN_GROUP),
    ADD (BROKER_LOGIN_GROUP),
    ADD (SERVER_PERMISSION_CHANGE_GROUP),
    ADD (SERVER_OBJECT_PERMISSION_CHANGE_GROUP),
    ADD (SERVER_OPERATION_GROUP),
    ADD (TRACE_CHANGE_GROUP),
    ADD (SERVER_OBJECT_CHANGE_GROUP),
    ADD (SERVER_PRINCIPAL_CHANGE_GROUP);


GO
ALTER SERVER AUDIT SPECIFICATION AuditSpec1;

ALTER SERVER AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (SERVER_PRINCIPAL_CHANGE_GROUP),
    DROP (SERVER_OBJECT_CHANGE_GROUP)
    WITH(STATE = ON);

ALTER SERVER AUDIT SPECIFICATION AuditSpec1
    DROP (SERVER_OBJECT_CHANGE_GROUP);


GO
DROP SERVER AUDIT SPECIFICATION Spec1;