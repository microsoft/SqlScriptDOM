-- sacaglar: Most of the functionality is tested at create proc
-- only Create is changed with Alter, please check the comments there.

Alter Procedure p1 (@lastname varchar(20), @firstname varchar(20))
AS 
Create Table t1 (int i1);
GO
Alter Proc p1 (@lastname varchar(20), @firstname varchar(20))
AS 
Create Table t1 (int i1);
