SELECT OrderID,
       CustomerName
FROM Orders
ORDER BY OrderDate
FETCH APPROXIMATE NEXT 20 ROWS ONLY;

SELECT OrderID,
       CustomerName
FROM Orders
ORDER BY OrderDate
FETCH APPROXIMATE NEXT 20 ROWS ONLY;

SELECT ProductID,
       ProductName
FROM Products
ORDER BY Price
FETCH APPROXIMATE NEXT 15 ROWS ONLY;

DECLARE @take AS INT = 50;

SELECT OrderID,
       CustomerName,
       OrderDate
FROM Orders
ORDER BY OrderDate
FETCH APPROXIMATE NEXT @take ROWS ONLY;

SELECT TOP 100 *
FROM (SELECT OrderID,
             CustomerName
      FROM Orders
      ORDER BY OrderDate
      FETCH APPROXIMATE NEXT 1000 ROWS ONLY) AS RecentOrders
ORDER BY OrderID;
