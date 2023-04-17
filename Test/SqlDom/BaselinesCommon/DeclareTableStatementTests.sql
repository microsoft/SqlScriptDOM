DECLARE @t1 TABLE (
    c1 INT         ,
    c2 MONEY       ,
    c3 VARCHAR (20));

DECLARE @t1 TABLE (
    c1 INT UNIQUE NONCLUSTERED,
    c3 INT DEFAULT 12,
    CHECK (c1 > 23));

