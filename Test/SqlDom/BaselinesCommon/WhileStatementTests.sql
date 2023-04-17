WHILE @a > 10
    CREATE TABLE t1 (
        c1 INT
    );

WHILE @a > 10
    BEGIN
        IF (@b > 10)
            BREAK;
        ELSE
            IF (@c = 20)
                CONTINUE;
    END

