CREATE SCHEMA
    AUTHORIZATION dev
    GRANT create TO NULL
        WITH GRANT OPTION;


GO
CREATE SCHEMA
    AUTHORIZATION dev
    DENY create control alter
        ON TYPE::a.b..d TO PUBLIC, NULL, [user1], user2;


GO
CREATE SCHEMA
    AUTHORIZATION dev
    REVOKE GRANT OPTION FOR view definition control create alter (c1, c2, c3)
        ON OBJECT::a.b..d TO PUBLIC, NULL, [user1], user2 CASCADE
        AS c1;


GO
CREATE SCHEMA
    AUTHORIZATION dev
    CREATE TABLE t1 (
        c1 INT
    )
    CREATE VIEW schema1.view1
    AS
    SELECT *
    FROM schema1.table2
    REVOKE all privileges TO PUBLIC
        AS [p1];

