-- Create stored procedure with Json parameter.
CREATE PROCEDURE [dbo].[RunJsonValue]
	@param1 json
AS
	SELECT Json_Value(@param1, '$.a')
GO