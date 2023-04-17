ALTER TRIGGER trig1
    ON employees
    FOR INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );

