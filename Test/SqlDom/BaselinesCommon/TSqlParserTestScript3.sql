CREATE TABLE A_Schema.MyTable (
    col1 INT             NOT NULL,
    col2 VARCHAR (10)   ,
    col3 DECIMAL (20, 5),
    col4 TIMESTAMP      ,
    CONSTRAINT MyPimaryKey PRIMARY KEY CLUSTERED (col1 ASC),
    CONSTRAINT MyForeignKey FOREIGN KEY (col2) REFERENCES A_Schema.A_TABLE (A3)
);