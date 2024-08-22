CREATE FUNCTION [ParameterizedFunction]
(@parameter_name1 JSON)
RETURNS JSON
AS
BEGIN
    RETURN '[30]';
END


GO
CREATE FUNCTION [ParameterizedVectorFunction]
(@parameter_name1 VECTOR (2))
RETURNS VECTOR (2)
AS
BEGIN
    DECLARE @v1 AS VECTOR (2);
    SET @v1 = CAST (N'[1,1]' AS VECTOR (2));
    RETURN @v1;
END