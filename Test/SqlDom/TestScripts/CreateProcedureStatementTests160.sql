-- Create stored procedure with Json parameter.
CREATE PROCEDURE [dbo].[RunJsonValue]
	@param1 json
AS
	SELECT Json_Value(@param1, '$.a')
GO

-- Create stored procedure with vector parameter.
CREATE PROCEDURE [dbo].[RunVector]
	@v1 [vector],
	@v2 [vector]
AS
	SELECT VECTOR_DISTANCE('cosine', @v1, @v2) AS consine
GO

-- Create stored procedure with vector return type.
CREATE PROCEDURE [dbo].[ReturnVector]
	@v1 [vector]
AS
	RETURN @v1
GO