CREATE TABLE t1 (
    eXTERrnal   INT,
    pivoT       INT,
    revert      INT,
    tableSample INT,
    UnPivot     INT
);


GO
CREATE TABLE t1 (
    c38 FLOAT NOT NULL
);


GO
SELECT *
FROM (SELECT c1 AS a,
             c2 AS b
      FROM t1
      FOR BROWSE) AS t10;

SELECT *
FROM (SELECT TOP 5 c1 AS a,
                   c2 AS b
      FROM t1
      ORDER BY c2
      FOR BROWSE) AS t10;

SELECT *
FROM ((SELECT TOP 5 *
       FROM t1
       ORDER BY c2
       FOR BROWSE) AS t10
      INNER JOIN
      t2
      ON t10.c1 = t2.c1);


GO
BACKUP LOG MyNwind
    TO DISK = 'C:\MyNwind.log';