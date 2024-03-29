SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N-(E)->N2
            AND N2<-(E2)-N);

SELECT *
FROM ANYTHING
WHERE MATCH(A-(B)->C
            AND C-(D)->E
            AND E-(F)->G
            AND G-(H)->I);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N<-(E)-N2
            AND N2-(E2)->N
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE (MATCH(N<-(E)-N2
             AND N2-(E2)->N)
       AND MATCH(N-(E)->N2
                 AND N2<-(E)-N));

SELECT *
FROM NODETABLE, MATCH() AS X
WHERE MATCH(A-(B)->C);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N<-(E)-N2
            AND N2-(E2)->N)
      OR MATCH(N-(E)->N2
               AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N<-(E)-N2
            AND N2-(E2)->N)
      AND NOT MATCH(N-(E)->N2
                    AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N<-(E)-N2
            AND N2-(E2)->N
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(N(-(E)->N2)+)
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(N(-(E)->N2 <-(E2)-N)+)
            AND N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH(N(-(E)->N2)+));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH(N(-(B)->C -(D)->E -(F)->G -(H)->I)+)
            AND SHORTEST_PATH(N(-(E)->N2)+)
            AND N-(E)->N2);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->)+ N2)
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)-> N2<-(E2)-)+ N)
            AND N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH((N-(E)->)+ N2));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH((N-(B)-> C-(D)-> E-(F)-> G-(H)->)+ I)
            AND SHORTEST_PATH((N-(E)->)+ N2)
            AND N-(E)->N2);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->)+ N2)
            AND N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH(N(-(E)->N2)+));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(LAST_NODE(node2) = LAST_NODE(node3)
            AND SHORTEST_PATH(N(-(E)->N2)+)
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(N(-(E)->N2)+)
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH(N(-(E)->N2)+)
            AND LAST_NODE(node2) = LAST_NODE(node3));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(N-(E)->N2
            AND N2<-(E)-N
            AND SHORTEST_PATH(N(-(E)->N2)+)
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND LAST_NODE(node4) = LAST_NODE(node5)
            AND N-(E)->N2
            AND SHORTEST_PATH((N-(E)-> N2<-(E2)-)+ N)
            AND SHORTEST_PATH(N(-(B)->C -(D)->E -(F)->G -(H)->I)+)
            AND LAST_NODE(node6) = LAST_NODE(node7));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->)+ N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->)+ LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(LAST_NODE(N)(-(E)->N2)+)
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->LAST_NODE(N2)
            AND LAST_NODE(N2)<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 3} N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 5} LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(LAST_NODE(N)(-(E)->N2){1, 2})
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->LAST_NODE(N2)
            AND LAST_NODE(N2)<-(E)-N);

SELECT *
FROM node1, node2 FOR PATH AS foo, edge3 FOR PATH;

SELECT *
FROM Nodes AS N1, Edges FOR PATH AS E, Nodes FOR PATH AS N2
WHERE MATCH(SHORTEST_PATH(N1(-(E)->N2)+));

SELECT *
FROM NODE FOR PATH AS N, EDGE AS E, EDGE AS E2, NODE FOR PATH AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 3} N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT *
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 5} LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT *
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(LAST_NODE(N)(-(E)->N2){1, 2})
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->LAST_NODE(N2)
            AND LAST_NODE(N2)<-(E)-N);

SELECT AVG(node2.column1) WITHIN GROUP (ORDER BY node2.column1 ASC)
FROM NODE FOR PATH AS N, EDGE AS E, EDGE AS E2, NODE FOR PATH AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 3} N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT MIN(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 5} LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT MAX(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(LAST_NODE(N)(-(E)->N2){1, 2})
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->LAST_NODE(N2)
            AND LAST_NODE(N2)<-(E)-N);

SELECT SUM(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE FOR PATH AS N, EDGE AS E, EDGE AS E2, NODE FOR PATH AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 3} N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT COUNT(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 5} LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT COUNT_BIG(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH(LAST_NODE(N)(-(E)->N2){1, 2})
            AND LAST_NODE(node2) = LAST_NODE(node3)
            AND N-(E)->LAST_NODE(N2)
            AND LAST_NODE(N2)<-(E)-N);

SELECT STRING_AGG(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE FOR PATH AS N, EDGE AS E, EDGE AS E2, NODE FOR PATH AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 3} N2)
            AND LAST_NODE(N)-(E)->N2
            AND N2<-(E)-N);

SELECT LAST_VALUE(node2.column1) WITHIN GROUP ( GRAPH PATH)
FROM NODE AS N, EDGE AS E, EDGE FOR PATH AS E2, NODE AS N2
WHERE MATCH(SHORTEST_PATH((N-(E)->){1, 5} LAST_NODE(N2))
            AND N-(E)->N2
            AND N2<-(E)-LAST_NODE(N));

SELECT *
FROM N1, (SELECT *
          FROM NODE) FOR PATH AS N2, (SELECT *
                                      FROM EDGE) FOR PATH AS E1
WHERE MATCH(SHORTEST_PATH(N1(-(E1)->N2)+));

SELECT *
FROM NODE AS N, (SELECT *
                 FROM EDGE) FOR PATH AS E([$edge_id], [$from_id], [$to_id]), (SELECT *
                                                                              FROM N) FOR PATH AS N1
WHERE MATCH(SHORTEST_PATH(N(-(E)->N1)+));