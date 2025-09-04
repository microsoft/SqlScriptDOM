DECLARE @qv AS VECTOR(1536) = AI_GENERATE_EMBEDDINGS(N'Pink Floyd music style' USE MODEL Ada2Embeddings);

SELECT t.id,
       s.distance,
       t.title
FROM VECTOR_SEARCH(
         TABLE = [dbo].[wikipedia_articles_embeddings] AS t,
         COLUMN = [content_vector],
         SIMILAR_TO = @qv,
         METRIC = 'cosine',
         TOP_N = 10
     ) AS s
ORDER BY s.distance;

SELECT *
FROM VECTOR_SEARCH(
         TABLE = wikipedia_articles_embeddings,
         COLUMN = dbo.wikipedia_articles_embeddings.content_vector,
         SIMILAR_TO = @qv,
         METRIC = 'dot',
         TOP_N = 10
     )
ORDER BY distance;

DECLARE @k AS INT = 5;

SELECT t.id,
       s.distance,
       t.title
FROM VECTOR_SEARCH(
         TABLE = graphnode AS src,
         COLUMN = embedding,
         SIMILAR_TO = @qv,
         METRIC = 'cosine',
         TOP_N = @k
     ) AS ann
ORDER BY s.distance;

SELECT outerref.id
FROM graphnode AS outerref
WHERE outerref.id IN (SELECT src.id
                      FROM VECTOR_SEARCH(
                               TABLE = graphnode AS src,
                               COLUMN = embedding,
                               SIMILAR_TO = @qv,
                               METRIC = 'cosine',
                               TOP_N = outerref.max_results
                           ) AS ann);