CREATE STATISTICS foo
ON bar(a)
WITH FULLSCAN, INCREMENTAL = ON;

CREATE STATISTICS foo
ON bar(a)
WITH SAMPLE 50 PERCENT, INCREMENTAL = OFF;

UPDATE STATISTICS foo (bar)
WITH FULLSCAN, INCREMENTAL = ON, RESAMPLE ON PARTITIONS (1, 3 TO 7, 10);

UPDATE STATISTICS foo (bar)
WITH FULLSCAN, INCREMENTAL = OFF;

ALTER DATABASE foo
SET AUTO_CREATE_STATISTICS ON(INCREMENTAL = ON),
AUTO_UPDATE_STATISTICS ON;

ALTER DATABASE foo
SET AUTO_UPDATE_STATISTICS ON,
AUTO_CREATE_STATISTICS ON(INCREMENTAL = OFF);

CREATE NONCLUSTERED INDEX foo
ON bar(a) WITH (STATISTICS_NORECOMPUTE = ON, STATISTICS_INCREMENTAL = OFF);

ALTER INDEX foo
ON bar REBUILD WITH(STATISTICS_NORECOMPUTE = ON, STATISTICS_INCREMENTAL = OFF);