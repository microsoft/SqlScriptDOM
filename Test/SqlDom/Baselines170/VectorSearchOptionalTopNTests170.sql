SELECT *
FROM VECTOR_SEARCH(
         TABLE = graphnode,
         COLUMN = embedding,
         SIMILAR_TO = @qembedding,
         METRIC = 'euclidean'
     );

SELECT *
FROM VECTOR_SEARCH(
         TABLE = graphnode,
         COLUMN = embedding,
         SIMILAR_TO = @qembedding,
         METRIC = 'euclidean'
     ) WITH (FORCE_ANN_ONLY);

SELECT *
FROM VECTOR_SEARCH(
         TABLE = graphnode,
         COLUMN = embedding,
         SIMILAR_TO = @qembedding,
         METRIC = 'euclidean',
         TOP_N = 20
     );

SELECT *
FROM VECTOR_SEARCH(
         TABLE = graphnode,
         COLUMN = embedding,
         SIMILAR_TO = @qembedding,
         METRIC = 'euclidean',
         TOP_N = 20
     ) WITH (FORCE_ANN_ONLY);
