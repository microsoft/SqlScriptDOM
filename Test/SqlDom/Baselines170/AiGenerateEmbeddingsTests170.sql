SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyDefaultModel);
SELECT AI_GENERATE_EMBEDDINGS(N'My Default Input Text' USE MODEL MyDefaultModel);
SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyDefaultModel PARAMETERS (TRY_CONVERT (JSON, N'{}')));
SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL dbo.MyDefaultModel);
SELECT AI_GENERATE_EMBEDDINGS('My Default Input Text' USE MODEL MyDatabase.dbo.MyDefaultModel);
GO

CREATE FUNCTION dbo.AI_GENERATE_EMBEDDINGS
(@input NVARCHAR (MAX))
RETURNS NVARCHAR (MAX)
AS
BEGIN
    RETURN CONCAT('Embed: ', @input);
END
GO

CREATE TABLE dbo.AI_GENERATE_EMBEDDINGS (
    id   INT            PRIMARY KEY,
    data NVARCHAR (MAX)
);
GO

CREATE VIEW dbo.ai_generate_embeddings
AS
SELECT 'View result' AS result;
GO

CREATE FUNCTION dbo.MyEmbeddingFunction
( )
RETURNS TABLE 
AS
RETURN 
    SELECT AI_GENERATE_EMBEDDINGS('Function Input' USE MODEL MyDefaultModel) AS EmbeddingResult
GO

CREATE VIEW dbo.MyEmbeddingView
AS
SELECT AI_GENERATE_EMBEDDINGS(N'View Input' USE MODEL dbo.MyDefaultModel) AS EmbeddingResult;
GO

CREATE TABLE dbo.SimpleEmbeddingTable (
    InputText NVARCHAR (MAX),
    Embedding AS             CONVERT (NVARCHAR (MAX), AI_GENERATE_EMBEDDINGS(InputText USE MODEL MyDefaultModel))
);
GO

CREATE PROCEDURE dbo.GetEmbedding
@InputText NVARCHAR (MAX)
AS
BEGIN
    SELECT CONVERT (NVARCHAR (MAX), AI_GENERATE_EMBEDDINGS(@InputText USE MODEL MyDefaultModel)) AS Embedding;
END
