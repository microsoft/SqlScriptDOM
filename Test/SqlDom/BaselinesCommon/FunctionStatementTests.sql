CREATE FUNCTION ISOweek
(@DATE DATETIME)
RETURNS INT
AS
BEGIN
    BREAK;
END


GO
CREATE FUNCTION SalesByStore
(@storeid VARCHAR (30))
RETURNS TABLE 
WITH ENCRYPTION, SCHEMABINDING, CALLED ON NULL INPUT
AS
RETURN 
    (SELECT title,
            qty
     FROM sales AS s, titles AS t
     WHERE s.stor_id = @storeid
           AND t.title_id = s.title_id)



GO
CREATE FUNCTION fn_FindReports
(@InEmpId NCHAR (5))
RETURNS 
    @retFindReports TABLE (
        empid   NCHAR (5)     PRIMARY KEY,
        empname NVARCHAR (50) NOT NULL,
        mgrid   NCHAR (5)    ,
        title   NVARCHAR (30))
AS
BEGIN
    BREAK;
END


GO
CREATE FUNCTION SearchAuthorbyArticleID
(@au_lname INT)
RETURNS TABLE 
AS
RETURN 
    (SELECT au_id
     FROM [dbo].[Authors] AS auth WITH (NOLOCK)
     WHERE auth.au_lname = @au_lname)



GO
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



GO
CREATE FUNCTION fn_FindReports
(@InEmpId NCHAR (5))
RETURNS 
    @retFindReports TABLE (
        empid   NCHAR (5)     PRIMARY KEY,
        empname NVARCHAR (50) NOT NULL,
        mgrid   NCHAR (5)    ,
        title   NVARCHAR (30))
WITH SCHEMABINDING
AS
BEGIN
    BREAK;
END