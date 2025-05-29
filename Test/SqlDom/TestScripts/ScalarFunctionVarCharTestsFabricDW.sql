CREATE FUNCTION dbo.GetFullName
(@FirstName VARCHAR (50), @LastName VARCHAR (50))
RETURNS VARCHAR (101)
AS
BEGIN
    RETURN @FirstName + ' ' + @LastName;
END