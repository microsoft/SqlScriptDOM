CREATE FUNCTION func_test
( )
RETURNS INT
AS
BEGIN
    RETURN (0);
END


GO
CREATE OR ALTER FUNCTION func_test
( )
RETURNS INT
AS
BEGIN
    RETURN (0);
END


GO
CREATE PROCEDURE sp_test
AS
BEGIN
    RETURN (0);
END


GO
CREATE OR ALTER PROCEDURE sp_test
AS
BEGIN
    RETURN (0);
END


GO
CREATE TRIGGER trg_test
    ON testTable
    FOR INSERT
    AS SELECT sum(col1 + col2)
       FROM inserted;


GO
CREATE OR ALTER TRIGGER trg_test
    ON testTable
    FOR INSERT
    AS SELECT sum(col1 + col2)
       FROM inserted;


GO
CREATE VIEW view_test
AS
SELECT col1
FROM testTable;


GO
CREATE OR ALTER VIEW view_test
AS
SELECT col1
FROM testTable;