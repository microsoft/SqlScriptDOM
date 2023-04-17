CREATE DATABASE db1
    ON 
    (NAME = Sales_dat, FILENAME = 'zzz') FOR LOAD;


GO
SELECT *
FROM sysobjects;


GO
SELECT *
FROM t WITH (TABLOCK, INDEX (myindex));


GO
CREATE STATISTICS sts
    ON uarule(rule_id)
    WITH ROWS;


GO
UPDATE STATISTICS t
    WITH ROWS;


GO
DISK INIT NAME = 'DEVICE1', PHYSNAME = 'c:\sql80\data\device1.dat', VDEVNO = 1, SIZE = 6144;


GO
DISK RESIZE SIZE = 1057, NAME = "tempdev", SIZE = @v, NAME = "templog";


GO
DISK INIT NAME = 'DEVICE1', PHYSNAME = 'c:\sql80\data\device1.dat', VSTART = 1, VDEVNO = 1, SIZE = 6144, NAME = 'DEVICE1';


GO
SELECT c1
FROM t1 WITH (HOLDLOCK, READPAST, INDEX (0));