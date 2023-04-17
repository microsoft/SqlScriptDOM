SELECT a;

SELECT *,
       [dbo].t1.*,
       @a + 10,
       (IDENTITYCOL * 10),
       IDENTITY (INT),
       IDENTITY (TINYINT, 10, 5),
       IDENTITY (DECIMAL (10, 0), -100, 5),
       c1 + 10 AS column1,
       c1 AS [column1],
       IDENTITY (INT) AS 'column1',
       c1 AS N'column1',
       c1 AS column1,
       IDENTITY (INT) AS 'column1',
       10 + c1 AS column1,
       IDENTITY (INT) AS [column1],
       c1 AS 'column1',
       -c1 - 10 AS N'column1',
       ..t1.c1,
       master.dbo.t1.c1,
       ..t1.*,
       master..t1.IDENTITYCOL,
       master..t1.ROWGUIDCOL;

SELECT @a = 10,
       @b = @a * 10 / 89;

