CREATE STATISTICS [stat]
    ON t1(c1) WHERE c1 IS NULL
    WITH FULLSCAN;

CREATE STATISTICS [stat]
    ON t1(c1, c2) WHERE c1 > CAST (10 AS INT)
                        AND c1 < CONVERT (INT, 'aaa')
    WITH FULLSCAN, NORECOMPUTE;

CREATE STATISTICS [stat]
    ON t1(c1, c2, c3) WHERE ((c1 > 5))
    WITH SAMPLE 12 ROWS, NORECOMPUTE;

CREATE STATISTICS [stat]
    ON t1(c1, c2, c3) WHERE c1 IN (10, 20, CAST (30 AS INT), (CONVERT (INT, 'aaa')))
    WITH STATS_STREAM = 0x100;

