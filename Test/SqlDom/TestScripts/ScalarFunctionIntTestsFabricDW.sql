CREATE FUNCTION dbo.ConvertInput
(@MyValueIn INT)
RETURNS DECIMAL (10, 2)
AS
BEGIN
    DECLARE @MyValueOut AS INT;
    SET @MyValueOut = CAST (@MyValueIn AS DECIMAL (10, 2));
    RETURN (@MyValueOut);
END
GO
SELECT dbo.ConvertInput(15) AS 'ConvertedValue';