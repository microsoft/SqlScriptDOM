CREATE TABLE A_Schema.A_TABLE (
    A0  VARCHAR (10),
    A1  INT          DEFAULT +23 IDENTITY (5, 10),
    A2  INT          DEFAULT -10 IDENTITY (1, 3) NOT FOR REPLICATION,
    A3  INT          DEFAULT ~23,
    A4  INT          DEFAULT [dbo].t1.c1,
    A5  INT          DEFAULT t2.c2,
    A6  INT          DEFAULT c2,
    A7  INT          DEFAULT IDENTITYCOL,
    A8  INT          DEFAULT ROWGUIDCOL,
    A9  INT          DEFAULT t1.IDENTITYCOL,
    A10 INT          DEFAULT dbo.t2.ROWGUIDCOL,
    A11 INT          DEFAULT +-++~+23,
    A12 INT          DEFAULT (+-++~+23),
    A13 INT          DEFAULT (234),
    A14 INT          DEFAULT ([dbo].t1.c1),
    A15 INT          DEFAULT c1 + c2 - c3 & 10 | 20 ^ c4 * 12 / c1 % 40,
    A16 INT          DEFAULT c1 + c2 + c3 + (12 * 34) - (344 + (23 ^ 23) / 23) + -23 % -23,
    A17 INT          DEFAULT $ROWGUID,
    A18 INT          DEFAULT $IDENTITY,
    A19 INT          DEFAULT dbo.t2.$ROWGUID,
    A20 INT          DEFAULT dbo.t2.$IDENTITY
);

SELECT 42949672960;

SELECT NULLIF (@a, 0);

SELECT COALESCE (@a, 1);

SELECT COALESCE (@a, @b, 2 + 4);

SELECT CASE @a WHEN @b + 10 THEN 20 END;

SELECT CASE @a WHEN @b + 10 THEN 20 END COLLATE SQL_Latin1_General_CP1_CI_AS;

SELECT CASE @a + @e WHEN @b + 10 THEN 20 WHEN @b + 20 THEN 30 WHEN @c THEN 40 END;

SELECT CASE @a WHEN @b + 10 THEN 20 WHEN @c THEN 40 ELSE 0 END;

SELECT CASE WHEN @b < 10 THEN 20 WHEN @b > 20 THEN 30 END;

SELECT CASE WHEN @b > 10
                 AND @c = 1 THEN 20 ELSE 30 END;

SELECT CASE WHEN 1 IS NULL THEN 1 ELSE 0 END;

SELECT CAST (12 AS FLOAT);

SELECT CAST (CAST (@myval AS VARBINARY (20)) AS DECIMAL (10, 5));

SELECT CAST (12 AS FLOAT) COLLATE SQL_Latin1_General_CP1_CI_AS;

SELECT CONVERT (FLOAT, 12);

SELECT CONVERT (FLOAT, 12) COLLATE SQL_Latin1_General_CP1_CI_AS;

SELECT CONVERT (DECIMAL (10, 5), CONVERT (VARBINARY (20), @myval));

SELECT CONVERT (DATETIME, @date, 101);

SELECT USER,
       CURRENT_USER,
       SESSION_USER,
       SYSTEM_USER,
       CURRENT_TIMESTAMP,
       CURRENT_DATE;

SELECT USER COLLATE SQL_Latin1_General_CP1_CI_AS,
       CURRENT_USER;

SELECT count(ALL c1),
       count(DISTINCT c1)
FROM t1;

SELECT t1.count(ALL c1),
       [t2].count(DISTINCT c1)
FROM t1;

SELECT getdate(),
       avg(c1),
       LEFT('Team System', 4),
       RIGHT('Team System', 6),
       count(*)
FROM t1;

SELECT getdate(),
       avg(c1) COLLATE SQL_Latin1_General_CP1_CI_AS;

SELECT [master].[dbo].f1(1, 2),
       dbo.f1(1, 2);

SELECT [master].[dbo].f1(1, 2) COLLATE SQL_Latin1_General_CP1_CI_AS,
       dbo.f1(1, 2);

SELECT IDENTITYCOL,
       ROWGUIDCOL,
       a.IDENTITYCOL,
       a.b.ROWGUIDCOL,
       a COLLATE SQL_Latin1_General_CP1_CI_AS,
       a.b,
       a.b.c;

SELECT (((SELECT *
          FROM t1)
         UNION
         SELECT *
         FROM t2)
        UNION
        SELECT *
        FROM t3);

SELECT (SELECT *
        FROM t1) COLLATE SQL_Latin1_General_CP1_CI_AS;

SELECT { FN convert (@a, sql_int) },
       { FN database () };

SELECT { FN insert (@a, 12) };

SELECT { FN left (@a, @b) };

SELECT { FN right (@a, @b) };

SELECT { FN truncate (10 + 12, @b) };

SELECT { FN user () };

SELECT { FN current_date () };

SELECT { FN current_time };

SELECT { FN current_time () };

SELECT { FN current_time (@a) };

SELECT { FN current_timestamp };

SELECT { FN current_timestamp () };

SELECT { FN current_timestamp (@a) };

SELECT { FN BuiltinFunc1 () };

SELECT { FN BuiltinFunc1 (@a, 12 + 23, { FN user () }) };

SELECT { FN extract ( hour FROM getdate()) };

SELECT { T '1' },
       { T N'1' },
       { D '1' },
       { D N'1' },
       { TS '1' },
       { TS N'1' };

UPDATE [dbo].[Table1]
SET column_1 = ((SELECT count(*)
                 FROM [dbo].[Table2]) * 10);


GO
CREATE TABLE [dbo].[Table2] (
    [EmployerNumber] VARCHAR (6) NOT NULL,
    [EmployeeNumber] VARCHAR (9) NOT NULL,
    [EmployeeId]     AS          (('S' + RIGHT([EmployeeNumber], (4))) + RIGHT(rtrim([EmployerNumber]), (1)))
);

SELECT - -1
FROM t;