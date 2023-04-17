CREATE PROCEDURE findjobs
@nm sysname=NULL
AS
IF @nm IS NULL
    BEGIN
        PRINT 'You must give a username';
        RETURN;
    END


GO
CREATE PROCEDURE checkstate
@param VARCHAR (11)
AS
IF (SELECT state
    FROM authors
    WHERE au_id = @param) = 'CA'
    RETURN 1;
ELSE
    RETURN 2;


GO
CREATE FUNCTION ISOweek
(@DATE DATETIME)
RETURNS INT
AS
BEGIN
    DECLARE @ISOweek AS INT;
    SET @ISOweek = DATEPART(wk, @DATE) + 1;
    RETURN (@ISOweek);
END

