MERGE INTO a

USING b ON (a.ProductID = b.ProductID)
WHEN MATCHED THEN UPDATE 
SET pi.Quantity = src.OrderQty;

MERGE INTO Production.ProductInventory
 AS pi
USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN MATCHED THEN UPDATE 
SET pi.Quantity = src.OrderQty;


GO
WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
MERGE TOP (2.5) PERCENT INTO Production.ProductInventory
 AS pi
USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN MATCHED THEN DELETE
WHEN NOT MATCHED AND src.OrderQty <> 0 THEN INSERT DEFAULT VALUES
WHEN NOT MATCHED THEN INSERT (c1) VALUES (10);


GO
WITH CHANGE_TRACKING_CONTEXT (0xff), DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
MERGE TOP (2.5) PERCENT INTO Production.ProductInventory
 AS pi
USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN MATCHED THEN DELETE
WHEN NOT MATCHED AND src.OrderQty <> 0 THEN INSERT DEFAULT VALUES
WHEN NOT MATCHED THEN INSERT (c1) VALUES (10);


GO
MERGE INTO Production.ProductInventory

USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN MATCHED THEN DELETE OUTPUT c1
OPTION (ALTERCOLUMN PLAN);


GO
MERGE INTO pi

USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN NOT MATCHED BY TARGET THEN INSERT (c1) VALUES (10)
WHEN NOT MATCHED BY SOURCE THEN DELETE;


GO
MERGE INTO pi

USING (SELECT *
       FROM someTable) AS src ON (pi.ProductID = src.ProductID)
WHEN NOT MATCHED BY TARGET THEN INSERT ($ACTION, $CUID) VALUES (10);


GO
MERGE INTO pi WITH (INDEX (i1))

USING t1 ON (t1.ProductID = pi.ProductID)
WHEN NOT MATCHED BY SOURCE THEN DELETE;


GO
MERGE INTO [dbo].[test]
 AS [tgt]
USING [dbo].[test] AS [ts] ON [tgt].[TestID] = [ts].[TestID]
WHEN MATCHED AND [tgt].[TestID] IS NOT NULL THEN DELETE OUTPUT [Inserted];