CREATE PROCEDURE [dbo].[ExampleProc]
AS
BEGIN
    SELECT ROW_NUMBER() OVER [win] AS [RowNum],
           SUM([Amount]) OVER [win] AS [RunningTotal]
    FROM [dbo].[Transactions]
    WINDOW [win] AS (PARTITION BY [AccountID] ORDER BY [TransactionDate]);
END


GO
SELECT COUNT(*) OVER [win]
FROM t1
WINDOW [win] AS (PARTITION BY c1);


GO
SELECT LAST_VALUE(c1) IGNORE NULLS OVER [win]
FROM t1
WINDOW [win] AS (ORDER BY c2);


GO
SELECT LAG(c1) OVER [my win]
FROM t1
WINDOW [my win] AS (ORDER BY c2);


GO
SELECT SUM(c1) OVER [select]
FROM t1
WINDOW [select] AS (PARTITION BY c2);


GO
SELECT SUM(c1) OVER "win"
FROM t1
WINDOW "win" AS (PARTITION BY c2);


GO
CREATE VIEW v1
AS
SELECT AVG(c1) OVER [win] AS a
FROM t1
WINDOW [win] AS ([base] ORDER BY c2), [base] AS (PARTITION BY c3);
