INSERT INTO t1
SELECT *
FROM (INSERT INTO t2
     OUTPUT c1
     SELECT *
     FROM zzz) AS ao;


GO
INSERT INTO t1
SELECT *
FROM (UPDATE t3
     SET c1 = 10
     OUTPUT c1) AS ao;


GO
INSERT INTO t1
SELECT *
FROM (DELETE t3
     OUTPUT c1) AS ao;


GO
INSERT INTO t1
SELECT *
FROM (MERGE INTO pi
     
     USING t1 ON (pi.PID = t1.PID)
     WHEN MATCHED THEN UPDATE 
     SET pi.Qty = t1.Qty OUTPUT c1) AS ao;


GO
SELECT *
FROM CHANGETABLE(CHANGES t1, 10) AS a;

DELETE t1
FROM CHANGETABLE(CHANGES dbo.t1, @v1) AS a(c1);

UPDATE t1
SET c1 = 10
FROM CHANGETABLE(CHANGES d1.dbo.t1, NULL) AS a;

SELECT *
FROM CHANGETABLE(VERSION s1.d1.dbo.t1, (c1), (1)) AS a;

SELECT *
FROM CHANGETABLE(VERSION z..t1, (c1, c2), ('a', 'b')) AS a(z1, z2);


GO
SELECT *
FROM (VALUES (10)) AS derived;

DELETE t1
FROM (VALUES ('a', 'b'), ('c', 'd')) AS derived(c1, c2);


GO
SELECT *
FROM st PIVOT (dbo.z1.MyAggregate (StandardCost, st.OtherColumn) FOR DaysToManufacture IN ([0], [1])) AS PivotTable;

SELECT *
FROM t WITH (FORCESEEK (i134 (c1, c3, c4)))
WHERE (c1 = 0
       AND c3 = 100
       AND c4 = 0.5);

SELECT *
FROM t WITH (FORCESEEK (1 (c1)))
WHERE (c1 = 0
       AND c3 = 100
       AND c4 = 0.5);

SELECT count(*)
FROM outerside AS L, multitvl AS R
WHERE L.a = R.a
      AND (R.b = 0
           OR (R.b > 1))
OPTION (TABLE HINT(R, FORCESEEK (nci_abc (a, b))));

SELECT *
FROM t WITH (FORCESEEK)
WHERE c1 = 0
      AND c3 = 100;

SELECT *
FROM t WITH (FORCESCAN)
WHERE c1 = 0
      AND c3 = 100;

SELECT *
FROM t WITH (FORCESCAN, INDEX (1))
WHERE c1 = 0
      AND c3 = 100;

SELECT *
FROM t
WHERE c1 = 3
OPTION (TABLE HINT(t, FORCESCAN));