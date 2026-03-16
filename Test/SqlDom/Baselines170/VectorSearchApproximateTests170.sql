SELECT TOP 10 WITH APPROXIMATE qt.qid,
                               src.id,
                               ann.distance
FROM QueryTable AS qt CROSS APPLY VECTOR_SEARCH(
                                      TABLE = graphnode AS src,
                                      COLUMN = embedding,
                                      SIMILAR_TO = qt.qembedding,
                                      METRIC = 'euclidean',
                                      TOP_N = 10
                                  ) AS ann
ORDER BY ann.distance;


GO
SELECT TOP 10 WITH APPROXIMATE qt.qid,
                               src.id,
                               ann.distance
FROM QueryTable AS qt CROSS APPLY VECTOR_SEARCH(
                                      TABLE = graphnode AS src,
                                      COLUMN = embedding,
                                      SIMILAR_TO = qt.qembedding,
                                      METRIC = 'euclidean',
                                      TOP_N = 10
                                  ) AS ann
ORDER BY ann.distance;


GO
SELECT qt.qid,
       src.id,
       ann.distance
FROM QueryTable AS qt CROSS APPLY VECTOR_SEARCH(
                                      TABLE = graphnode AS src,
                                      COLUMN = embedding,
                                      SIMILAR_TO = qt.qembedding,
                                      METRIC = 'euclidean',
                                      TOP_N = 10
                                  ) AS ann
ORDER BY ann.distance
FETCH APPROXIMATE NEXT 10 ROWS ONLY;


GO
SELECT qt.qid,
       src.id,
       ann.distance
FROM QueryTable AS qt CROSS APPLY VECTOR_SEARCH(
                                      TABLE = graphnode AS src,
                                      COLUMN = embedding,
                                      SIMILAR_TO = qt.qembedding,
                                      METRIC = 'euclidean',
                                      TOP_N = 10
                                  ) AS ann
ORDER BY ann.distance
FETCH APPROXIMATE NEXT 10 ROWS ONLY;