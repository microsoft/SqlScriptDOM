-- STRING_SPLIT in FROM clause with ordinal functionality
SELECT * FROM STRING_SPLIT(N'Lorem ipsum dolor sit amet.', N' ', NULL);

SELECT * FROM STRING_SPLIT(N'Lorem ipsum dolor sit amet.', N' ', 1);

SELECT value FROM STRING_SPLIT(N'Lorem ipsum dolor sit amet.', N' ', 0);

SELECT ordinal FROM STRING_SPLIT(N'Lorem ipsum dolor sit amet.', N' ', 1);

SELECT t2.value FROM String_Split('Lorem-ipsum dolor-sit amet.', '-', 0) AS t1 CROSS APPLY String_Split(t1.value, ' ') AS t2;

SELECT t2.ordinal FROM String_Split('Lorem-ipsum dolor-sit amet.', '-', 1) AS t1 CROSS APPLY String_Split(t1.ordinal, ' ') AS t2;

SELECT value FROM (VALUES ('Lorem ipsum'), ('dolor sit amet.')) AS x(col) CROSS APPLY STRING_SPLIT(col, ',', 0);

SELECT ordinal FROM (VALUES ('Lorem ipsum'), ('dolor sit amet.')) AS x(col) CROSS APPLY STRING_SPLIT(col, ',', 1);
