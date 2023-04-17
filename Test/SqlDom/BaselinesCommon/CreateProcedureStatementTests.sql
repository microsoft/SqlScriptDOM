CREATE PROCEDURE p1
@lastname VARCHAR (20), @firstname VARCHAR (20)
AS
CREATE TABLE t1 (
    int i1
);


GO
CREATE PROCEDURE [Gokhan].p1
@weight INT, @height FLOAT=23, @address NVARCHAR (80), @lastname VARCHAR (20)='Smith', @middlename VARCHAR (20)=false, @firstname VARCHAR (20)
AS
CREATE TABLE t1 (
    int i1
);
CREATE TABLE t2 (
    int i1
);
CREATE TABLE t3 (
    int i1
);


GO
CREATE PROCEDURE [Gokhan].[proc 2]
@weight INT=30 OUTPUT, @height FLOAT, @address NVARCHAR (80) OUTPUT, @lastname VARCHAR (20), @middlename VARCHAR (20), @firstname VARCHAR (20), @c1 CURSOR VARYING OUTPUT
AS
CREATE TABLE t1 (
    int i1
);
CREATE TABLE t2 (
    int i1
);
CREATE TABLE t3 (
    int i1
);


GO
CREATE PROCEDURE [Gokhan].[proc 2]
@lastname VARCHAR (20), @middlename VARCHAR (20), @firstname VARCHAR (20)
WITH RECOMPILE
AS
CREATE TABLE t1 (
    int i1
);
CREATE TABLE t2 (
    int i1
);
CREATE TABLE t3 (
    int i1
);


GO
CREATE PROCEDURE [Gokhan].[proc 2]
@lastname VARCHAR (20), @middlename VARCHAR (20), @firstname VARCHAR (20)
WITH RECOMPILE
FOR REPLICATION
AS
CREATE TABLE t1 (
    int i1
);
CREATE TABLE t2 (
    int i1
);
CREATE TABLE t3 (
    int i1
);


GO
CREATE PROCEDURE [Gokhan].[proc 2]
@lastname VARCHAR (20), @middlename VARCHAR (20), @firstname VARCHAR (20)
WITH ENCRYPTION, RECOMPILE
FOR REPLICATION
AS
CREATE TABLE t1 (
    int i1
);
CREATE TABLE t2 (
    int i1
);
CREATE TABLE t3 (
    int i1
);


GO
CREATE PROCEDURE proc3
@money1 MONEY=$23, @money2 MONEY=-$   10.12, @money3 MONEY=-$-23.00, @money4 MONEY=£10.00, @money5 MONEY=¤10.00, @money6 MONEY=¥10.00, @money7 MONEY=৲10.00, @money8 MONEY=৳10.00, @money9 MONEY=฿10.00, @money10 MONEY=₡10.00, @money11 MONEY=₢10.00, @money12 MONEY=₣10.00, @money13 MONEY=₤10.00, @money14 MONEY=₦10.00, @money15 MONEY=₧10.00, @money16 MONEY=₨10.00, @money17 MONEY=₩10.00, @money18 MONEY=₪10.00, @money19 MONEY=₫10.00, @money20 MONEY=$+10, @i1 INT=12, @i2 INT=-12, @i3 INT=NULL, @r1 REAL=12.01, @r2 REAL=-12.45
AS
SELECT *
FROM t1;


GO
CREATE PROCEDURE [Ten Most Expensive Products]
AS
SELECT *
FROM t1;


GO
CREATE PROCEDURE dbo.Bug
AS
DECLARE @o1 AS INT;
EXECUTE sp_executesql N'set @output = 1', N'@o1 int output', @o1 OUTPUT;


GO
CREATE PROCEDURE dbo.Procedure1
@p INT=DEFAULT
AS
SELECT @p;


GO
CREATE PROCEDURE [dbo].[Procedure1]
AS
IF (1 < 2e)
    PRINT 'hi';
RETURN 0;