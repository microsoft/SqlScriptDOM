CREATE RULE dbo.r1
    AS @a1 > 10;


GO
CREATE RULE r1
    AS @a1 > 10
       AND @a2 BETWEEN 20 AND 39;

