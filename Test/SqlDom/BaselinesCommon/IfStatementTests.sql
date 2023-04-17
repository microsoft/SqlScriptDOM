IF @a > 10
    DECLARE @b AS INT;

IF @a > 10
   AND @a < 20
    DECLARE @b AS INT;
ELSE
    DECLARE @b AS VARCHAR (10);

IF 10 + 5 < @a
    DECLARE @b AS INT;
ELSE
    IF 20 < @a
        DECLARE @b AS FLOAT;
    ELSE
        DECLARE @b AS MONEY;

IF @a < 10
    IF @a > 100
        DECLARE @a AS INT;
    ELSE
        DECLARE @b AS FLOAT;

IF @a < 10
    IF @a > 100
        DECLARE @a AS INT;
    ELSE
        DECLARE @b AS FLOAT;
ELSE
    DECLARE @c AS MONEY;

