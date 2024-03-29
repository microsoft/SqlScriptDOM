CREATE INDEX ind1
    ON t1(c1)
    INCLUDE(c1);

CREATE INDEX ind1
    ON t1(c1)
    INCLUDE(c1, c2);


GO
CREATE UNIQUE NONCLUSTERED INDEX ind1
    ON tempdb.dbo.t1(c1) WITH (PAD_INDEX = ON, FILLFACTOR = 23, IGNORE_DUP_KEY = OFF, DROP_EXISTING = ON, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = OFF, ALLOW_PAGE_LOCKS = ON, MAXDOP = 50);


GO
CREATE INDEX ind1
    ON t1(c1)
    ON [partitionScheme] ([columnName]);

CREATE INDEX ind1
    ON t1(c1)
    ON partitionScheme (c1, c2, c3);

