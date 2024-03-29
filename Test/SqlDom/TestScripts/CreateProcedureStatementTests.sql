-- Create proc with optional paranthesis
CREATE PROCEDURE p1 (@lastname varchar(20), @firstname varchar(20))
AS 
Create Table t1 (int i1);

GO

-- Create proc with multiple parameters and statements
CREATE PROCEDURE [Gokhan].p1 
   @weight int,
   @height float = 23, -- testing default
   @address nvarchar(80),
   @lastname varchar(20) = 'Smith',
   @middlename varchar(20) = false,
   @firstname varchar(20) 
AS 
Create Table t1 (int i1);
Create Table t2 (int i1);
Create Table t3 (int i1);
GO

-- Add default testing when expressions are done

-- Cursor
CREATE PROCEDURE [Gokhan].[proc 2] 
   @weight int = 30 output,
   @height float,
   @address nvarchar(80) out,
   @lastname varchar(20),
   @middlename varchar(20),
   @firstname varchar(20), 
   @c1 Cursor varying output
AS 
Create Table t1 (int i1);
Create Table t2 (int i1);
Create Table t3 (int i1);
GO

--With option
CREATE PROCEDURE [Gokhan].[proc 2] 
   @lastname varchar(20),
   @middlename varchar(20),
   @firstname varchar(20)
With Recompile
AS 
Create Table t1 (int i1);
Create Table t2 (int i1);
Create Table t3 (int i1);
GO

--with option, for replication
CREATE PROCEDURE [Gokhan].[proc 2] 
   @lastname varchar(20),
   @middlename varchar(20),
   @firstname varchar(20)
With Recompile
For Replication
AS 
Create Table t1 (int i1);
Create Table t2 (int i1);
Create Table t3 (int i1);
GO

-- two with options, for replication
CREATE PROCEDURE [Gokhan].[proc 2] 
   @lastname varchar(20),
   @middlename varchar(20),
   @firstname varchar(20)
With Encryption, Recompile
For Replication
AS 
Create Table t1 (int i1);
Create Table t2 (int i1);
Create Table t3 (int i1);
GO

-- test default parameters, and money
create procedure proc3
(
    @money1 money = $23,
    @money2 money = -$   10.12, -- space after money sign is valid
    @money3 money = -$-23.00, -- valid    
    @money4 money = £10.00, --pound
    @money5 money = ¤10.00, --currency
    @money6 money = ¥10.00, -- yen
    @money7 money = ৲10.00,  -- bengali rupee mark
    @money8 money = ৳10.00,   -- bengali rupee sign
    @money9 money = ฿10.00, -- Thai baht symbol
    @money10 money = ₡10.00,  -- Colon sign
    @money11 money = ₢10.00,  -- Cruzerio sign
    @money12 money = ₣10.00, -- French Franc sign
    @money13 money = ₤10.00, -- Lira sign
    @money14 money = ₦10.00, -- Naira sign
    @money15 money = ₧10.00, -- Peseta sign
    @money16 money = ₨10.00, -- Rupee sign
    @money17 money = ₩10.00, -- Won sign
    @money18 money = ₪10.00,  -- new sheqel sign
    @money19 money = ₫10.00,  -- dong sign
	@money20 money = $+10,    --$ with +
    @i1 int = 12,
    @i2 int = -12,
    @i3 int = null,
    @r1 real = 12.01,
    @r2 real = -12.45    
)
AS
select * from t1;
GO

-- testing no parameters
create procedure [Ten Most Expensive Products] AS
select * from t1

GO

-- testing OUTPUT clause at the end of procedure - Dev10 bug 578178
CREATE PROC dbo.Bug
AS
DECLARE @o1 int
exec sp_executesql N'set @output = 1', N'@o1 int output', @o1 output

GO

-- testing DEFAULT as the value for default parameter - Dev10 bug 604417
CREATE PROC dbo.Procedure1
@p INT = DEFAULT
AS
SELECT @p

GO

-- testing exponents without any number after 'e' - Dev10 bug 677801
CREATE PROCEDURE [dbo].[Procedure1]
AS
IF(1 < 2e)
PRINT 'hi'
RETURN 0

