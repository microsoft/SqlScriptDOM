SELECT *
FROM Sql2000.[MyDb].dbo.t1;

SELECT *
FROM .[MyDb].dbo.t1;

SELECT *
FROM ..dbo.t1;

SELECT *
FROM ...t1;


GO
SELECT *
FROM .[MyDb].dbo.func(DEFAULT);

SELECT *
FROM fun2();

SELECT *
FROM fun2(-$10, 23, DEFAULT, 'hello', N'Test');

SELECT *
FROM fun2(-$10, 23, DEFAULT, 'hello', N'Test') AS table1, dbo.fun3() AS table2;


GO
SELECT c1
FROM t1 AS table1, t3 AS table3;


GO
SELECT c1
FROM t1 AS table1 WITH (INDEX (1));

SELECT c1
FROM t1 WITH (HOLDLOCK), t2 WITH (HOLDLOCK, INDEX (0));

SELECT c1
FROM t1 WITH (INDEX (0));

SELECT c1
FROM t1 WITH (NOLOCK);

SELECT c1
FROM t1 WITH (HOLDLOCK, READPAST, INDEX (0));

SELECT *
FROM t1 WITH (NOLOCK) CROSS APPLY dbo.tvf WITH (NOLOCK);

SELECT c1
FROM t1 AS table1 WITH (INDEX (0, 1, ind2), HOLDLOCK, NOLOCK, PAGLOCK, READCOMMITTED, READPAST, READUNCOMMITTED, REPEATABLEREAD, ROWLOCK, SERIALIZABLE, TABLOCK, TABLOCKX, UPDLOCK, XLOCK, NOWAIT), t2 WITH (NOWAIT);


GO
SELECT c1
FROM t1 AS t1_1 WITH (HOLDLOCK), t2 AS t2_1 WITH (HOLDLOCK);


GO
SELECT c1
FROM ::functionName (), ::functionName () AS table1, ::functionName (1) AS table1, ::functionName (1, NULL, DEFAULT) AS table1;

SELECT c1
FROM @var1, @var2 AS [table 1], @var3 AS table2;


GO
SELECT *
FROM t1 CROSS JOIN t10;

SELECT *
FROM t1 CROSS JOIN (t10 CROSS JOIN t11);

SELECT *
FROM (((t1 CROSS JOIN ((t10 CROSS JOIN t11)))));

SELECT *
FROM t1 CROSS APPLY (t10 CROSS APPLY t11);

SELECT *
FROM (((t1 CROSS APPLY ((t10 CROSS APPLY t11)))));

SELECT *
FROM t1 OUTER APPLY (t10 OUTER APPLY t11);

SELECT *
FROM (((t1 OUTER APPLY ((t10 OUTER APPLY t11)))));

SELECT *
FROM t1
     INNER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     LEFT OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     LEFT OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     RIGHT OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     RIGHT OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     FULL OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     FULL OUTER JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER MERGE JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER HASH JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER LOOP JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER REMOTE JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER MERGE JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER HASH JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER LOOP JOIN
     t10
     ON t1.c1 = t10.c1;

SELECT *
FROM t1
     INNER REMOTE JOIN
     t10
     LEFT OUTER JOIN
     t11
     ON t10.c1 > t11.c1
     ON t1.c1 = t10.c1;

SELECT *
FROM (t1
      INNER REMOTE JOIN
      (t10
       LEFT OUTER JOIN
       t11
       ON t10.c1 > t11.c1)
      ON t1.c1 = t10.c1);

SELECT *
FROM (((((((((((((((((((((SELECT *
                          FROM t1) AS t10
                         INNER JOIN
                         (SELECT *
                          FROM t2) AS t20
                         ON t10.c1 = t20.c2))))))))))))))))))));

SELECT *
FROM (((SELECT *
        FROM t1
        UNION ALL
        SELECT *
        FROM t1)
       UNION
       SELECT *
       FROM t1)) AS t10
     INNER JOIN
     (SELECT *
      FROM t2) AS t20
     ON t10.c1 = t20.c2;

SELECT *
FROM (((SELECT *
        FROM t1
        UNION ALL
        SELECT *
        FROM t1) AS t10
       INNER JOIN
       (SELECT *
        FROM t2) AS t20
       ON t10.c1 = t20.c2)
      INNER JOIN
      (SELECT *
       FROM t3) AS t30
      ON t20.c2 = t30.c3);

SELECT *
FROM (SELECT c1 AS a,
             c2 AS b
      FROM t1) AS t10(c1);

SELECT *
FROM (SELECT *
      FROM t1) AS t10(c1, c2) CROSS JOIN t2;

SELECT *
FROM ((SELECT *
       FROM t1) AS t10
      INNER JOIN
      t2
      ON t10.c1 = t2.c1);

SELECT *
FROM { OJ { OJ t1
     INNER JOIN
     t2
     ON c1 < 10 } };

SELECT *
FROM { OJ t1
     INNER JOIN
     t2
     ON c1 < 10
     INNER JOIN
     t3
     ON c1 < 10 };

SELECT *
FROM { OJ t1 CROSS JOIN t2
     INNER JOIN
     t2
     ON c1 < 10 };

SELECT *
FROM { OJ (t1 CROSS JOIN t2)
     INNER JOIN
     t2
     ON c1 < 10 };

SELECT *
FROM t1
     INNER JOIN
     { OJ t1 CROSS JOIN t2
     INNER JOIN
     t2
     ON c1 < 10 }
     ON c1 < 10 CROSS JOIN t3;

SELECT jc.[JobCandidateID]
FROM [HumanResources].[JobCandidate] AS jc CROSS APPLY jc.[Resume].nodes(N'declare default element namespace "http://schemas.microsoft.com/sqlserver/2004/07/adventure-works/Resume"; 
    /Resume/Employment') AS Employment(ref);

SELECT ecp.*
FROM sys.dm_exec_cached_plans AS ecp OUTER APPLY sys.dm_exec_plan_attributes(ecp.plan_handle) AS epa;

SELECT au_id
FROM [dbo].[Authors] AS auth WITH (NOLOCK)
WHERE auth.au_lname = @au_lname;

SELECT *
FROM t WITH (NOLOCK);

SELECT *
FROM t AS s WITH (NOLOCK);

SELECT *
FROM t1 WITH (NOLOCK) CROSS APPLY dbo.tvf WITH (NOLOCK);

SELECT *
FROM t AS s WITH (NOLOCK);

SELECT *
FROM t WITH (NOLOCK);

SELECT *
FROM t AS s WITH (NOLOCK);

