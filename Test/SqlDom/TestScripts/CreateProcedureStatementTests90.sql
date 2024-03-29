
-- Testing execute as and external name
create proc p1 @lastname nvarchar(100) with execute as caller AS external name [assembly].[class].[method];;;

Go

create proc p1 @lastname nvarchar(100) with recompile, execute as self AS print 'hi'; print 'bye'

Go

create proc p1 @lastname nvarchar(100) with recompile, execute as owner, encryption AS external name A.B.C

Go

create proc p1 @lastname nvarchar(100) with execute as 'dbo' AS select * from t1 select * from t2

Go

