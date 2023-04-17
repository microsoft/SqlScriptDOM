CREATE TABLE A_Schema.A_TABLE (
    A1 INT CONSTRAINT cons1 CHECK (A1 < 4),
    A2 INT CHECK NOT FOR REPLICATION (A2 <> 21),
    A3 INT,
    A4 INT,
    CONSTRAINT cons2 CHECK (A3 != A4),
    CHECK NOT FOR REPLICATION (A4 IS NOT NULL),
    CHECK (A1 IS NULL
           AND A2 > 34
           OR A3 IS NOT NULL
              AND NOT A4 = 23
              AND NOT NOT A1 < 23
           OR A2 <= 1000
              AND A4 !< 830
              AND A3 !> 10000
           OR A2 >= 98),
    CHECK (A1 IN (23, -23, +100, ~1)),
    CHECK (A1 NOT IN (90)),
    CHECK (A1 BETWEEN -992 AND ~923),
    CHECK NOT FOR REPLICATION (A2 NOT BETWEEN 394 AND 122),
    CHECK ((CASE WHEN col1 = 1 THEN 1 ELSE 0 END) = 0),
    CHECK (A1 IS NULL
           AND (A2 > 34)
           OR (A3 IS NOT NULL)
              AND NOT A4 = (23)
              AND ((NOT NOT A1 < 23)
                   OR (A2 <= 1000
                       AND A4 !< 830
                       AND A3 !> 10000
                       OR A2 >= 98))),
    CHECK ((A1 IN (23, -23, +100, ~1))),
    CHECK ((A1 BETWEEN -992 AND ~923)),
    CHECK ((((100 > A1)))),
    CHECK (A1 LIKE 'foo'),
    CHECK (A1 NOT LIKE '50%% off when 100 or more copies are purchased'),
    CHECK (A1 LIKE '50%% off when 100 or more copies are purchased' ESCAPE '%'),
    CHECK (A1 LIKE '50%% off when 100 or more copies are purchased' {ESCAPE '%' })
);


GO
SELECT e.FirstName,
       e.LastName,
       ep.Rate
FROM HumanResources.vEmployee AS e
     INNER JOIN
     HumanResources.EmployeePayHistory AS ep
     ON e.BusinessEntityID = ep.BusinessEntityID
WHERE ep.Rate BETWEEN 27 AND 30
ORDER BY ep.Rate;


GO
SELECT Name
FROM Production.Product
WHERE CONTAINS ((Name), ' "Mountain" OR "Road" ');


GO
SELECT a.FirstName,
       a.LastName
FROM Person.Person AS a
WHERE EXISTS (SELECT *
              FROM HumanResources.Employee AS b
              WHERE a.BusinessEntityID = b.BusinessEntityID
                    AND a.LastName = 'Johnson');


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((Document), 'vital safety components');


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS (($IDENTITY), @a);


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.*), N'abc');


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.*), N'abc');


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.c1), N'abc', LANGUAGE 0x413);


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.c1, c2, c3), N'abc', LANGUAGE 'English');


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.c1), N'abc', LANGUAGE @current);


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((c1), N'abc');


GO
SELECT Title
FROM Production.Document
WHERE TSEQUAL (CONVERT (TIMESTAMP, getdate()), CONVERT (TIMESTAMP, getdate()));


GO
SELECT Title
FROM Production.Document
WHERE UPDATE (id);


GO
SELECT Title
FROM Production.Document
WHERE (UPDATE (id));


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS ((t1.c1), @a);


GO
SELECT Title
FROM Production.Document
WHERE FREETEXT ((t2.*), N'abc');


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS ((t1.c1, c2, t2.c1), @a);


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS ((t1.c1, c2, t2.c1), @a, LANGUAGE 1043);


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS ((*), @a);


GO
SELECT Title
FROM Production.Document
WHERE CONTAINS ((t1.$IDENTITY), @a);


GO
SELECT p.FirstName,
       p.LastName,
       e.JobTitle
FROM Person.Person AS p
     INNER JOIN
     HumanResources.Employee AS e
     ON p.BusinessEntityID = e.BusinessEntityID
WHERE e.JobTitle IN ('Design Engineer', 'Tool Designer', 'Marketing Assistant');


GO
SELECT Name,
       Weight,
       Color
FROM Production.Product
WHERE Weight < 10.00
      OR Color IS NULL
ORDER BY Name;


GO
SELECT *
FROM t
WHERE RTRIM(col1) LIKE '% King';


GO
CREATE TRIGGER reminder
    ON Person.Address
    AFTER UPDATE
    AS IF (UPDATE (StateProvinceID)
           OR UPDATE (PostalCode))
           BEGIN
               RAISERROR (50009, 16, 10);
           END


GO
IF dbo.ISOWeek() = 110
    PRINT 'hi';


GO
CREATE PROCEDURE p1
AS
SELECT CASE WHEN (isnull((SELECT 1
                          FROM dbo.[syscomments]
                          WHERE 1 = 1), 0)) = 0 THEN 0 ELSE 14 END
FROM dbo.[syscomments];


GO
CREATE PROCEDURE p1
AS
SELECT CASE WHEN (isnull((SELECT 1
                          FROM dbo.[syscomments]
                          WHERE (1 = 1)), 0)) = 0 THEN 0 ELSE 14 END
FROM dbo.[syscomments];


GO
CREATE PROCEDURE p1
AS
SELECT CASE WHEN ((isnull((SELECT 1
                           FROM dbo.[syscomments]
                           WHERE 1 = 1), 0)) = 0) THEN 0 ELSE 14 END
FROM dbo.[syscomments];


GO
IF ((SELECT 1) = 2)
    PRINT 'yes';


GO
CREATE PROCEDURE getOrders
AS
SELECT OrderID,
       OrderDate,
       ShippedDate
FROM Orders
WHERE CustomerID IN ((SELECT CustomerID
                      FROM Customers
                      WHERE CustomerID = 'ALFKI'), (SELECT CustomerID
                                                    FROM Customers
                                                    WHERE CustomerID = 'ANATR'), (SELECT CustomerID
                                                                                  FROM Customers
                                                                                  WHERE CustomerID = 'ANTON'));