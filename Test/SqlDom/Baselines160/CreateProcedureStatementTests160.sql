CREATE PROCEDURE [dbo].[RunJsonValue]
@param1 JSON
AS
SELECT Json_Value(@param1, '$.a');