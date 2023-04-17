CREATE TABLE [dbo].[DatabaseDefinitions] (
    [DatabaseDefinitionId] INT IDENTITY (1, 1) NOT NULL
) ON [PRIMARY];

CREATE TABLE t2 (
    int INT ,
    c2  TEXT
) ON [default] TEXTIMAGE_ON [default];

CREATE TABLE t3 (
    int INT ,
    c2  TEXT
) TEXTIMAGE_ON [Secondary];

