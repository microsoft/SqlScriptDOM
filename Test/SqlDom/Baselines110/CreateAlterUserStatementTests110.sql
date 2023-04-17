CREATE USER contained_user
    WITH PASSWORD = 'foo', DEFAULT_LANGUAGE = 1033, DEFAULT_SCHEMA = dbo, SID = 0xdeadbeef;

ALTER USER contained_user
    WITH PASSWORD = 'foo', DEFAULT_LANGUAGE = none, DEFAULT_SCHEMA = dbo;

ALTER USER contained_user
    WITH PASSWORD = 'foo' OLD_PASSWORD = 'old';


GO
CREATE USER [domain\user]
    WITH DEFAULT_SCHEMA = NULL;

ALTER USER user1
    WITH DEFAULT_SCHEMA = NULL;