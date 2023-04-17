CREATE TABLE [dbo].[DatabaseDefinitions] (
    [DatabaseDefinitionId] INT             IDENTITY (1, 1) NOT NULL,
    [Name]                 VARCHAR (50)    COLLATE Latin1_General_CI_AS NOT NULL,
    [Description]          VARCHAR (200)   COLLATE Latin1_General_CI_AS NULL,
    [PhysicalName]         DECIMAL (50, 3) COLLATE Latin1_General_CI_AS NULL,
    [dt_inserted]          DATETIME        NOT NULL,
    [dt_updated]           DATETIME        NOT NULL
) ON [PRIMARY];

