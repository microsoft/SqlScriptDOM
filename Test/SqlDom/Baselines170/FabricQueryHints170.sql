SELECT OrderDateKey,
       SUM(SalesAmount) AS TotalSales
FROM FactInternetSales
GROUP BY OrderDateKey
ORDER BY OrderDateKey
OPTION (FOR TIMESTAMP AS OF '2024-03-13T19:39:35.28');

SELECT OrderDateKey,
       SalesAmount
FROM FactInternetSales
OPTION (FORCE SINGLE NODE PLAN);

SELECT OrderDateKey,
       SalesAmount
FROM FactInternetSales
OPTION (FORCE DISTRIBUTED PLAN);

SELECT OrderDateKey,
       SalesAmount
FROM FactInternetSales
OPTION (FORCE DISTRIBUTED PLAN, FOR TIMESTAMP AS OF '2024-03-13T19:39:35.28');
