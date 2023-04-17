READTEXT t1.c1 @ptrval 1 @size;

READTEXT ..t1.c1 0xAB10002000FFFFFF @offset 25;

READTEXT master.dbo.t1.c1 @ptr 0 25 HOLDLOCK;

