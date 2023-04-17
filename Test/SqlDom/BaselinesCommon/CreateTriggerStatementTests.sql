CREATE TRIGGER trig1
    ON employees
    FOR INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER dbo.trig1
    ON dbo.employees
    FOR INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON dbo.employees
    WITH ENCRYPTION
    FOR INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON employees
    AFTER INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON employees
    INSTEAD OF INSERT
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON employees
    INSTEAD OF INSERT, UPDATE
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON employees
    AFTER DELETE, INSERT, UPDATE
    AS CREATE TABLE t1 (
           c1 INT
       );


GO
CREATE TRIGGER trig1
    ON employees
    AFTER DELETE, INSERT, UPDATE
    AS CREATE TABLE t1 (
           c1 INT
       );
       CREATE TABLE t2 (
           c1 INT
       );
       CREATE TABLE t3 (
           c1 INT
       );


GO
CREATE TRIGGER reminder
    ON [Northwind].[Schema1].titles
    FOR INSERT, UPDATE, DELETE
    AS CREATE TABLE t1 (
           Col1 INT NOT NULL
       );

