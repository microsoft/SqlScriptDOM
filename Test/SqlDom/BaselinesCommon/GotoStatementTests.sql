start:

CREATE TABLE t1 (
    c1 INT
);

IF @a > 3
    GOTO start;
ELSE
    GOTO finish;

finish:


GO
CREATE PROCEDURE __Test
AS
SET NOCOUNT ON;
gogo:
RETURN;