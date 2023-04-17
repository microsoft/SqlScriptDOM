SELECT value
FROM GENERATE_SERIES (1, 5);

SELECT value
FROM GENERATE_SERIES (1, 32, 7);

SELECT value
FROM GENERATE_SERIES (1, 5)
ORDER BY value;

SELECT DATE_BUCKET(WEEK, 10, SYSUTCDATETIME());

DECLARE @date AS DATETIME2 = '2020-04-30 00:00:00';

SELECT DATE_BUCKET(DAY, 100, @date);

DECLARE @date AS DATETIME2 = '2020-06-15 21:22:11';

DECLARE @origin AS DATETIME2 = '2019-01-01 00:00:00';

SELECT DATE_BUCKET(WEEK, 5, @date, @origin)
FROM Table1;

SELECT GREATEST('6.62', 3.1415, N'7') AS GreatestVal;

SELECT GREATEST('Glacier', N'Joshua Tree', 'Mount Rainier') AS GreatestString;

DECLARE @PredictionA AS DECIMAL (2, 1) = 0.7;

DECLARE @PredictionB AS DECIMAL (3, 1) = 0.65;

SELECT VarX,
       Correlation
FROM dbo.studies
WHERE Correlation > GREATEST(@PredictionA, @PredictionB);

SELECT LEAST('6.62', 3.1415, N'7') AS LeastVal;

SELECT LEAST('Glacier', N'Joshua Tree', 'Mount Rainier') AS LeastString;

DECLARE @PredictionA AS DECIMAL (2, 1) = 0.7;

DECLARE @PredictionB AS DECIMAL (3, 1) = 0.65;

SELECT VarX,
       Correlation
FROM dbo.studies
WHERE Correlation < LEAST(@PredictionA, @PredictionB);

SELECT value
FROM STRING_SPLIT ('Lorem ipsum dolor sit amet.', ' ');

SELECT *
FROM STRING_SPLIT ('Lorem ipsum dolor sit amet.', ' ', 1);

DECLARE @tags AS NVARCHAR (400) = 'clothing,road,,touring,bike';

SELECT value
FROM STRING_SPLIT (@tags, ',')
WHERE RTRIM(value) <> '';

SELECT ProductId,
       Name,
       Tags
FROM Product
     INNER JOIN
     STRING_SPLIT ('1,2,3', ',')
     ON value = ProductId;

SELECT *
FROM STRING_SPLIT ('Austin,Texas,Seattle,Washington,Denver,Colorado', ',', 1)
WHERE ordinal % 2 = 0;

SELECT *
FROM STRING_SPLIT ('E-D-C-B-A', '-', 1)
ORDER BY ordinal DESC;

DECLARE @d AS DATETIME2 = '2021-12-08 11:30:15.1234567';

SELECT 'Year',
       DATETRUNC(year, @d);

SELECT 'Quarter',
       DATETRUNC(quarter, @d);

SELECT DATETRUNC(month, DATEADD(month, 4, TransactionDate))
FROM Sales.CustomerTransactions;