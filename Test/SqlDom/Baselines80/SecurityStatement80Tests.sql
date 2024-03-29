GRANT SELECT
    ON ..t1 TO PUBLIC;

GRANT INSERT
    ON ..t1 (c1) TO PUBLIC;

GRANT ALL
    ON ..t1 (c1, c2, c3) TO PUBLIC;

GRANT ALL, SELECT (c1, c2), INSERT, DELETE, UPDATE, EXECUTE, EXECUTE, REFERENCES (c1, c2)
    ON t2 TO guest;

GRANT CREATE DATABASE TO PUBLIC AS [clause];

GRANT CREATE DEFAULT TO [guest];

GRANT CREATE FUNCTION TO [guest];

GRANT CREATE PROCEDURE TO [guest];

GRANT CREATE RULE TO [guest];

GRANT CREATE TABLE TO [guest];

GRANT CREATE VIEW TO [guest];

GRANT BACKUP DATABASE TO [guest];

GRANT BACKUP LOG TO [guest];

GRANT CREATE DATABASE, CREATE DEFAULT, CREATE PROCEDURE, CREATE FUNCTION, CREATE RULE, CREATE TABLE, CREATE VIEW, BACKUP DATABASE, BACKUP LOG TO [guest];

GRANT ALL TO NULL
    WITH GRANT OPTION;

GRANT ALL TO PUBLIC
    WITH GRANT OPTION AS t1;

DENY SELECT
    ON t2
    TO NULL;

DENY CREATE VIEW
    TO [guest];

DENY ALL
    TO PUBLIC
    CASCADE;

REVOKE GRANT OPTION FOR SELECT
    ON t2 FROM NULL;

REVOKE CREATE VIEW FROM [guest];

REVOKE CREATE VIEW FROM [guest] AS role1;

REVOKE ALL FROM PUBLIC CASCADE;

REVOKE ALL FROM PUBLIC CASCADE AS [group 2];

