CREATE PROCEDURE p1
AS
RETURN 0;


GO
CREATE TRIGGER trig1
    ON Employee
    AFTER INSERT
    AS SELECT *
       FROM Employee;


GO
BEGIN TRY
    CREATE TABLE t1 (
        c1 INT
    );
END TRY
BEGIN CATCH
END CATCH