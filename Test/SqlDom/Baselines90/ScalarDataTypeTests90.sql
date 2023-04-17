CREATE TABLE t1 (
    c1 XML( myCollection) NOT NULL
);


GO
CREATE TABLE t1 (
    c1 DECIMAL (MAX)
);


GO
CREATE TABLE [dbo].[DatabaseDefinitions] (
    c6f  DECIMAL (MAX)                   NOT NULL,
    c7c  NUMERIC (MAX)                   NOT NULL,
    c17a VARCHAR (MAX)                   NOT NULL,
    c39a FLOAT (MAX)                     NOT NULL,
    c47  [mytype] (MAX)                 ,
    c48  dbo.mytype (MAX)                NOT NULL,
    c51  XML( [dbo].myCollection)       ,
    c52  XML(CONTENT [dbo].myCollection) NULL,
    c53  XML(DOCUMENT [myCollection])    NOT NULL,
    c55  XML( myCollection)             ,
    c56  XML( [dbo].myCollection)       ,
    c57  XML(CONTENT [dbo].myCollection) NULL,
    c58  XML(DOCUMENT [myCollection])    NOT NULL
);


GO
CREATE TABLE t1 (
    c44 dbo.[mytype]          NOT NULL,
    c45 dbo.mytype (10)      ,
    c46 [dbo].mytype (10, 20) NOT NULL
);