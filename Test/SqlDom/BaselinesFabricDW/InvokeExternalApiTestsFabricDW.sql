SELECT INVOKE_EXTERNAL_API('myFunctionSet', 'myFunction');


GO
SELECT INVOKE_EXTERNAL_API('myFunctionSet', 'myFunction', 42);


GO
DECLARE @v AS INT = 7;

SELECT INVOKE_EXTERNAL_API(N'mySet', N'doStuff', @v, 'literal', 1 + 2, NULL);


GO
ALTER FUNCTION dbo.TestInvokeExternalApi
( )
RETURNS NVARCHAR (MAX)
AS
BEGIN
    RETURN (INVOKE_EXTERNAL_API('mySet', 'myFn', 'arg1'));
END


GO
SELECT 1
FROM dbo.t
WHERE INVOKE_EXTERNAL_API('mySet', 'isAllowed', col1) = 1;
