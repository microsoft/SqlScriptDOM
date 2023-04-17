ALTER TABLE edge
    ADD CONSTRAINT myConstraint CONNECTION (node3 TO node4);

ALTER TABLE edge2
    ADD CONSTRAINT myConstraint CONNECTION (node3 TO node4, node1 TO node2, node1 TO node4);

CREATE TABLE edge3 (
    howmuch INT,
    CONSTRAINT myConstraint CONNECTION (node1 TO node2)
) AS EDGE;

CREATE TABLE edge4 (
    howmuch INT,
    CONSTRAINT myConstraint CONNECTION (node1 TO node2, node3 TO node4)
) AS EDGE;

MERGE INTO Owns

USING (Source
       INNER JOIN
       Person
       ON Source.FirstName = Person.FirstName
       INNER JOIN
       Dog
       ON Source.DogName = Dog.DogName) ON MATCH(Person-(Owns)->Dog)
WHEN NOT MATCHED BY SOURCE THEN DELETE OUTPUT inserted.*, deleted.*;