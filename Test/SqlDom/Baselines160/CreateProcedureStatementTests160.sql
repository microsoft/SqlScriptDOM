CREATE PROCEDURE [dbo].[RunJsonValue]
@param1 JSON
AS
SELECT Json_Value(@param1, '$.a');


GO
CREATE PROCEDURE [dbo].[RunVector]
@v1 VECTOR, @v2 VECTOR
AS
SELECT VECTOR_DISTANCE('cosine', @v1, @v2) AS consine;


GO
CREATE PROCEDURE [dbo].[ReturnVector]
@v1 VECTOR
AS
RETURN @v1;