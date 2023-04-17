SELECT *
FROM .[MyDb].dbo.t1 TABLESAMPLE (12 + 3);

SELECT *
FROM fun2 TABLESAMPLE SYSTEM (12 + 3 PERCENT) REPEATABLE (-100);

SELECT *
FROM t1 AS table1 TABLESAMPLE (12 + 3 ROWS);


GO
SELECT *
FROM master..sysprocesses AS p CROSS APPLY ::fn_get_sql (p.sql_handle);

SELECT *
FROM ::f1 (a.b * (12 + 20));

SELECT *
FROM ::f2 (DEFAULT);


GO
SELECT c1
FROM t1 AS table1 WITH (READCOMMITTEDLOCK, KEEPIDENTITY, KEEPDEFAULTS, IGNORE_CONSTRAINTS, IGNORE_TRIGGERS);


GO
SELECT c1
FROM @var1.f(DEFAULT, @c * 23) AS t(c), @var2.f() AS [table 1](c, c2), @var3.[g](a.b::C) AS table2(c), @var1.f(DEFAULT, @c * 23) AS t(c1, c2), @var3.[g](a.b::C) AS table2(c1);


GO
SELECT VendorID,
       [164] AS Emp1,
       [198] AS Emp2,
       [223] AS Emp3,
       [231] AS Emp4,
       [233] AS Emp5
FROM (SELECT PurchaseOrderID,
             EmployeeID,
             VendorID
      FROM Purchasing.PurchaseOrderHeader) AS p PIVOT (COUNT (PurchaseOrderID) FOR EmployeeID IN ([164], [198], [223], [231], [233])) AS pvt
ORDER BY VendorID;


GO
SELECT VendorID,
       Employee,
       Orders
FROM (SELECT VendorID,
             Emp1,
             Emp2,
             Emp3,
             Emp4,
             Emp5
      FROM pvt) AS p UNPIVOT (Orders FOR Employee IN (Emp1, Emp2, Emp3, Emp4, Emp5)) AS unpvt;

SELECT *
FROM st PIVOT (AVG (.st.StandardCost) FOR st.DaysToManufacture IN ([0], [1])) AS PivotTable;


GO
SELECT *
FROM t1 PIVOT (dbo.f (c1) FOR [if] IN (a)) AS PTable CROSS JOIN t2 UNPIVOT (c2 FOR T2 IN ([a], b)) AS UPTable;

SELECT *
FROM t1
     INNER REMOTE JOIN
     (t10
      LEFT OUTER JOIN
      t11
      ON t10.c1 > t11.c1)
     ON t1.c1 = t10.c1 PIVOT (dbo.f (c1) FOR [if] IN (a)) AS PTable;

SELECT *
FROM t1 CROSS APPLY t2 UNPIVOT (q FOR n IN (t1.c0, c1)) AS a;

SELECT *
FROM (SELECT *
      FROM t1
      FOR XML AUTO) AS t(c1);


GO
SELECT *
FROM authors AS t1 TABLESAMPLE (1000 ROWS) WITH (NOLOCK);

SELECT *
FROM authors AS t1 TABLESAMPLE (1000 ROWS) WITH (NOLOCK);

SELECT *
FROM authors TABLESAMPLE (1000 ROWS) WITH (HOLDLOCK);