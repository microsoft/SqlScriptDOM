CREATE TABLE Customers (
    CustomerID INT           ,
    Name       NVARCHAR (100)
)
WITH (CLUSTER BY (CustomerID));


GO
CREATE TABLE sales.Orders (
    OrderID    INT ,
    CustomerID INT ,
    OrderDate  DATE
)
WITH (CLUSTER BY (CustomerID, OrderDate), DISTRIBUTION = HASH(CustomerID));


GO
CREATE TABLE Inventory (
    ProductID   INT          ,
    ProductName VARCHAR (255),
    InStock     BIT          
)
WITH (CLUSTER BY (ProductID));


GO
ALTER TABLE Customers
    WITH (CLUSTER BY (CustomerID));


GO
ALTER TABLE sales.Orders
    WITH (CLUSTER BY (CustomerID, OrderDate));


GO
ALTER TABLE Inventory
    WITH (CLUSTER BY (ProductID));


GO
CREATE TABLE CustomersClustered
WITH (CLUSTER BY (CustomerID)) AS
SELECT *
FROM Customers;