CREATE FUNCTION ISOweek
(@DATE DATETIME)
RETURNS INT
WITH EXECUTE AS CALLER, RETURNS NULL ON NULL INPUT
AS
BEGIN
    BREAK;
END


GO
CREATE FUNCTION fn_FindReports
(@InEmpId NCHAR (5))
RETURNS 
    @retFindReports TABLE (
        empid   NCHAR (5)     PRIMARY KEY,
        empname NVARCHAR (50) NOT NULL,
        mgrid   NCHAR (5)    ,
        title   NVARCHAR (30))
WITH SCHEMABINDING, EXECUTE AS CALLER
AS
BEGIN
    BREAK;
END


GO
CREATE FUNCTION f1
( )
RETURNS TABLE 
AS
RETURN 
    WITH XMLNAMESPACES (DEFAULT 'u')
    SELECT c1
    FROM t1



GO
CREATE FUNCTION MyFunc
( )
RETURNS INT
AS
 EXTERNAL NAME a1.c1.f1


GO
CREATE FUNCTION MyFunc
( )
RETURNS 
     TABLE (
        c1 INT)
AS
 EXTERNAL NAME a2.c2.f2


GO
CREATE FUNCTION [app].[Workflow_Definition_GetName]
(@workflowDefinition XML( app.WorkflowDefinitionSchemaCollection))
RETURNS NVARCHAR (100)
AS
BEGIN
    RETURN @workflowDefinition.value('declare namespace wd=">http://www.deloitte.com/perhar/WAF/WorkflowDefinition";
(/wd:Workflow/@name)[1]', 'nvarchar(100)');
END