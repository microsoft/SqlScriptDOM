CREATE TABLE tbl (
    embedding VECTOR(1)
);

CREATE TABLE tbl (
    embedding VECTOR(1, float32)
);

CREATE TABLE tbl (
    embedding VECTOR(1, FLOAT16)
);

DECLARE @embedding AS VECTOR(2);

DECLARE @embedding AS VECTOR(2, FLOAT32);

DECLARE @embedding AS VECTOR(2, float16);