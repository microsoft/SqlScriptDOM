-- Create function with Json data type.
CREATE FUNCTION [ParameterizedFunction] 
(
	@parameter_name1 [json]
) RETURNS json
AS
  BEGIN
    RETURN '[30]';
  END
GO