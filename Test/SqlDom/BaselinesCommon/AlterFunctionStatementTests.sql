ALTER FUNCTION ISOweek
(@p1 INT=10)
RETURNS INT
AS
BEGIN
    BREAK;
END


GO
ALTER FUNCTION SalesByStore
(@storeid VARCHAR (30), @p2 INT)
RETURNS TABLE 
WITH SCHEMABINDING
AS
RETURN 
    SELECT title,
           qty
    FROM sales AS s, titles AS t
    WHERE s.stor_id = @storeid
          AND t.title_id = s.title_id


