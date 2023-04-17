UPDATETEXT t1.c1 0xABAA10 @InsertOffset NULL
WITH LOG @str;

UPDATETEXT BULK t1.c1 @var TIMESTAMP = 0xFFFF NULL @DeleteLength
WITH LOG 'hi';

UPDATETEXT BULK t1.c1 1234 @var1 -15
WITH LOG dbo.[t1].[c1] @col;

UPDATETEXT t1.c1 @var @var1 -15
WITH LOG NULL;

UPDATETEXT t1.c1 100 @var1 -15
WITH LOG 'hello';

UPDATETEXT t1.c1 10 @var1 -15
WITH LOG N'hello';

UPDATETEXT t1.c1 @var @var1 -15
WITH LOG 0xFF;

UPDATETEXT t1.c1 @var @var1 -15
WITH LOG t1.c2 @var2;

UPDATETEXT t1.c1 @var @var1 -15
WITH LOG .t1.c2 0xABC;

UPDATETEXT dbo.t1 @ptrval @Position @PropLen
WITH LOG;