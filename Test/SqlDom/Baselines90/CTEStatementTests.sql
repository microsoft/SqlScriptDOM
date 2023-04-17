WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
INSERT @v1
DEFAULT VALUES;


GO
WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
SELECT c1
FROM t1;

WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1),
 [cte2]
AS (SELECT *
    FROM t2)
SELECT c1
FROM t1;

WITH XMLNAMESPACES ('u' AS n1)
SELECT c1
FROM t1;

WITH XMLNAMESPACES (N'u' AS n1, 'u2' AS n2)
SELECT c1
FROM t1;

WITH XMLNAMESPACES (DEFAULT 'u')
SELECT c1
FROM t1;

WITH XMLNAMESPACES (N'u' AS n1, 'u2' AS n2, DEFAULT 'u'),
 DirReps (c1, c2)
AS (SELECT c1
    FROM t1),
 [cte2]
AS (SELECT *
    FROM t2)
SELECT c1
FROM t1;


GO
WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
UPDATE t1
SET c1 = 23 + 10;


GO
WITH DirReps (c1, c2)
AS (SELECT c1
    FROM t1)
DELETE t1
FROM t1;

DECLARE c CURSOR
    FOR WITH DirReps (c1, c2)
        AS (SELECT c1,
                   1
            FROM t1)
        SELECT c1
        FROM t1;