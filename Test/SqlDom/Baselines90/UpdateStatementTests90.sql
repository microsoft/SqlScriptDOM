UPDATE t1
SET a.b.c.d.e = 100 - [udt]::t1.f();

UPDATE TOP (2.5) PERCENT
 t1
SET c1 = 23 + 10;

UPDATE TOP (SELECT *
            FROM t2)
 t1
SET c1 = 23 + 10;


GO
UPDATE t1
SET c1 = 23 + 10
OUTPUT a.b.c1 AS [c2], c2
FROM t1
WHERE t1.c1 < 10;

UPDATE dbo.f1()
SET @a = a.b = 23
OUTPUT c1, c2 INTO @t1 (c1);

UPDATE t1
SET a.b = NULL
OUTPUT c1, c2 INTO @t1
OUTPUT c1 AS [C1], 12 * 12 AS [144];


GO
UPDATE t1
SET a.b.c.d.func();

UPDATE t1
SET a.b.c.d.func(DEFAULT, 12, 102 + 123);