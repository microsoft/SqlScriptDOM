CREATE EXTERNAL TABLE [dbo].[et1]
    WITH (
    DATA_SOURCE = [eds1],
    LOCATION = N'/bands.dat',
    FILE_FORMAT = [eff1],
    REJECT_TYPE = PERCENTAGE,
    REJECT_VALUE = 10.5,
    REJECT_SAMPLE_VALUE = 10
    ) AS
SELECT 1;


GO
CREATE EXTERNAL TABLE [dbo].[et2]
    WITH (
    DATA_SOURCE = [eds1],
    LOCATION = N'/bands.dat',
    FILE_FORMAT = [eff1],
    REJECT_TYPE = PERCENTAGE,
    REJECT_VALUE = 10.5,
    REJECT_SAMPLE_VALUE = 10
    ) AS
SELECT *
FROM dbo.T2;


GO
CREATE EXTERNAL TABLE [dbo].[et3]
    WITH (
    DATA_SOURCE = [eds1],
    LOCATION = N'/bands.dat',
    FILE_FORMAT = [eff1],
    REJECT_TYPE = PERCENTAGE,
    REJECT_VALUE = 10.5,
    REJECT_SAMPLE_VALUE = 10
    ) AS
SELECT p.ProductKey
FROM dbo.DimProduct AS p
     RIGHT OUTER JOIN
     dbo.stg_DimProduct AS s
     ON p.ProductKey = s.ProductKey;


GO
CREATE PROCEDURE dwsyntaxforsqldom
AS
CREATE EXTERNAL TABLE [ET].[et4]
    WITH (
    DATA_SOURCE = [eds1],
    LOCATION = N'/bands.dat',
    FILE_FORMAT = [eff1],
    REJECT_TYPE = PERCENTAGE,
    REJECT_VALUE = 10.5,
    REJECT_SAMPLE_VALUE = 10
    ) AS
SELECT *
FROM [dbo].[DimSalesTerritory];