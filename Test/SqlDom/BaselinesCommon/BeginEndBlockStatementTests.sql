BEGIN
    CREATE TABLE t1 (
        c1 INT
    );
END

BEGIN
    CREATE TABLE t1 (
        c1 INT
    );
    SET ANSI_NULLS ON;
    CREATE TABLE t2 (
        c1 INT
    );
END

BEGIN
    BEGIN
        BEGIN
            CREATE TABLE t1 (
                c1 INT
            );
            SET ANSI_NULLS ON;
        END
    END
END

BREAK;

CONTINUE;

BEGIN
    CREATE TABLE t1 (
        c1 INT
    );
    BREAK;
    SET ANSI_NULLS ON;
    CREATE TABLE t2 (
        c1 INT
    );
    CONTINUE;
END

