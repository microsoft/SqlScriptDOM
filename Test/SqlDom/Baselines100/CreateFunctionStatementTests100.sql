CREATE FUNCTION MyFunc
( )
RETURNS 
     TABLE (
        c1 INT)
ORDER (c1 ASC)
AS
 EXTERNAL NAME a2.c2.f2


GO
CREATE FUNCTION MyFunc
( )
RETURNS 
     TABLE (
        c1 INT)
ORDER (c1, c2 DESC, c3 ASC)
AS
 EXTERNAL NAME a2.c2.f2


GO
CREATE FUNCTION [dbo].[TableFunction2]
(@param1 INT, @param2 NCHAR (5))
RETURNS 
     TABLE (
        c1 INT      ,
        c2 NCHAR (5),
        c3 DATETIME )
WITH EXECUTE AS 'User1'
ORDER (c3 DESC, c1 ASC)
AS
 EXTERNAL NAME CLR1.UserDefinedFunctions.TableFunction1