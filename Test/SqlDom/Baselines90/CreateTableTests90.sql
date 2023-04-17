CREATE TABLE t0 (
    A31 AS -A1 PERSISTED,
    A32 AS -A1 PERSISTED NOT NULL,
    A33 AS -A1 PERSISTED CHECK (A33 < 10),
    A34 AS -A1 PERSISTED FOREIGN KEY REFERENCES t1 (c1),
    A51 AS A0 PERSISTED CONSTRAINT PK_KEY PRIMARY KEY,
    cz  AS -A1 PERSISTED NOT NULL FOREIGN KEY REFERENCES t1 (c1) CHECK (1 > 1)
) ON partitionScheme (someColumn);


GO
CREATE TABLE foo (
    id INT NOT NULL
) ON partitionScheme (someColumn) TEXTIMAGE_ON [default];


GO
CREATE TABLE t1 (
    c1 INT                  ,
    c2 NTEXT                ,
    c3 NVARCHAR             ,
    c4 VARBINARY            ,
    c5 VARCHAR              ,
    c6 XML(CONTENT dbo.xsd1)
);