CREATE DATABASE AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1;

CREATE DATABASE AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    WITH(STATE = ON);

CREATE DATABASE AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (SELECT, INSERT, UPDATE, DELETE ON t1 BY dbo)
    WITH(STATE = OFF);

CREATE DATABASE AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (EXECUTE, RECEIVE, REFERENCES ON zzz BY PUBLIC, NULL, dbo),
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
    ADD (DATABASE_OBJECT_ACCESS_GROUP);

ALTER DATABASE AUDIT SPECIFICATION AuditSpec1;

ALTER DATABASE AUDIT SPECIFICATION AuditSpec1
    FOR SERVER AUDIT a1
    ADD (SELECT ON t1 BY dbo),
    DROP (INSERT, UPDATE ON t1 BY dbo)
    WITH(STATE = ON);

ALTER DATABASE AUDIT SPECIFICATION AuditSpec1
    DROP (DATABASE_PERMISSION_CHANGE_GROUP);

DROP DATABASE AUDIT SPECIFICATION AuditSpec1;