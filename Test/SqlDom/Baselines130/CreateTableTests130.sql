CREATE TABLE T1 (
    C1 INT NOT NULL,
    INDEX idx NONCLUSTERED (C1) WHERE C1 > 10
);


GO
CREATE TABLE T1 (
    C1 INT NOT NULL INDEX idx NONCLUSTERED WHERE C1 > 10,
    C2 INT NOT NULL
);


GO
CREATE TABLE T1 (
    C1 INT INDEX idx WHERE C1 > 10,
    C2 INT NOT NULL
);


GO
CREATE TABLE T1 (
    C1 INT NOT NULL,
    INDEX idx (C1) WHERE C1 > 10
);


GO
CREATE TABLE t (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
);

CREATE TABLE t (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON);


GO
CREATE TABLE t (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history, DATA_CONSISTENCY_CHECK=ON));


GO
CREATE TABLE t (
    COL0      INT                                         PRIMARY KEY,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history, DATA_CONSISTENCY_CHECK=OFF));


GO
CREATE TABLE t (
    COL0      INT                                        ,
    COL1      XML                                        ,
    COL2      FLOAT                                      ,
    COL3      NVARCHAR (64)                              ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END   NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = OFF);


GO
CREATE TABLE t2 (
    COL0      INT                                               ,
    COL1      XML                                               ,
    COL2      FLOAT                                             ,
    COL3      NVARCHAR (64)                                     ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
);


GO
CREATE TABLE t3 (
    COL0      INT                                               ,
    COL1      XML                                               ,
    COL2      FLOAT                                             ,
    COL3      NVARCHAR (64)                                     ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END          NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON);


GO
CREATE TABLE t4 (
    COL0      INT                                             ,
    COL1      XML                                             ,
    COL2      FLOAT                                           ,
    COL3      NVARCHAR (64)                                   ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START      NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history, DATA_CONSISTENCY_CHECK=OFF));


GO
CREATE TABLE t5 (
    COL0      INT                                               ,
    COL1      XML                                               ,
    COL2      FLOAT                                             ,
    COL3      NVARCHAR (64)                                     ,
    SYS_START DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END   DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history, DATA_CONSISTENCY_CHECK=ON));


GO
CREATE TABLE [t6 a] (
    COL0          INT                                               ,
    COL1          XML                                               ,
    COL2          FLOAT                                             ,
    COL3          NVARCHAR (64)                                     ,
    [SYS_START a] DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END       DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([SYS_START a], SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=[dbo 2].[t_history 2], DATA_CONSISTENCY_CHECK=ON));


GO
CREATE TABLE t (
    COL0         INT                                               ,
    COL1         XML                                               ,
    COL2         FLOAT                                             ,
    COL3         NVARCHAR (64)                                     ,
    USERID_START VARBINARY (85) GENERATED ALWAYS AS SUSER_SID START NOT NULL,
    SYS_START    DATETIME2 (7) GENERATED ALWAYS AS ROW START        NOT NULL,
    SYS_END      DATETIME2 (7) GENERATED ALWAYS AS ROW END          NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0       INT                                             ,
    COL1       XML                                             ,
    COL2       FLOAT                                           ,
    COL3       NVARCHAR (64)                                   ,
    USERID_END VARBINARY (85) GENERATED ALWAYS AS SUSER_SID END NOT NULL,
    SYS_START  DATETIME2 (7) GENERATED ALWAYS AS ROW START      NOT NULL,
    SYS_END    DATETIME2 (7) GENERATED ALWAYS AS ROW END        NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0           INT                                                 ,
    COL1           XML                                                 ,
    COL2           FLOAT                                               ,
    COL3           NVARCHAR (64)                                       ,
    USERNAME_START NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL,
    SYS_START      DATETIME2 (7) GENERATED ALWAYS AS ROW START          NOT NULL,
    SYS_END        DATETIME2 (7) GENERATED ALWAYS AS ROW END            NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0         INT                                               ,
    COL1         XML                                               ,
    COL2         FLOAT                                             ,
    COL3         NVARCHAR (64)                                     ,
    USERNAME_END NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME END NOT NULL,
    SYS_START    DATETIME2 (7) GENERATED ALWAYS AS ROW START        NOT NULL,
    SYS_END      DATETIME2 (7) GENERATED ALWAYS AS ROW END          NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0           INT                                                 ,
    COL1           XML                                                 ,
    COL2           FLOAT                                               ,
    COL3           NVARCHAR (64)                                       ,
    USERID_START   VARBINARY (85) GENERATED ALWAYS AS SUSER_SID START   NOT NULL,
    USERID_END     VARBINARY (85) GENERATED ALWAYS AS SUSER_SID END     NOT NULL,
    USERNAME_START NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL,
    USERNAME_END   NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME END   NOT NULL,
    SYS_START      DATETIME2 (7) GENERATED ALWAYS AS ROW START          NOT NULL,
    SYS_END        DATETIME2 (7) GENERATED ALWAYS AS ROW END            NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0              INT                                                 ,
    COL1              XML                                                 ,
    COL2              FLOAT                                               ,
    COL3              NVARCHAR (64)                                       ,
    [USER ID START]   VARBINARY (85) GENERATED ALWAYS AS SUSER_SID START   NOT NULL,
    [USER ID END]     VARBINARY (85) GENERATED ALWAYS AS SUSER_SID END     NOT NULL,
    [USER NAME START] NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL,
    [USER NAME END]   NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME END   NOT NULL,
    SYS_START         DATETIME2 (7) GENERATED ALWAYS AS ROW START          NOT NULL,
    SYS_END           DATETIME2 (7) GENERATED ALWAYS AS ROW END            NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0           INT                                                 ,
    COL1           XML                                                 ,
    COL2           FLOAT                                               ,
    COL3           NVARCHAR (64)                                       ,
    USERID_START   VARBINARY (85) GENERATED ALWAYS AS SUSER_SID START   NOT NULL,
    USERID_END     VARBINARY (85) GENERATED ALWAYS AS SUSER_SID END     NOT NULL,
    USERNAME_START NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME START NOT NULL,
    USERNAME_END   NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME END   NOT NULL,
    SYS_START      DATETIME2 (7) GENERATED ALWAYS AS ROW START          NOT NULL,
    SYS_END        DATETIME2 (7) GENERATED ALWAYS AS ROW END            NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
);


GO
CREATE TABLE t (
    COL0           INT                                                        ,
    COL1           XML                                                        ,
    COL2           FLOAT                                                      ,
    COL3           NVARCHAR (64)                                              ,
    USERID_START   VARBINARY (85) GENERATED ALWAYS AS SUSER_SID START HIDDEN   NOT NULL,
    USERID_END     VARBINARY (85) GENERATED ALWAYS AS SUSER_SID END HIDDEN     NOT NULL,
    USERNAME_START NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME START HIDDEN NOT NULL,
    USERNAME_END   NVARCHAR (128) GENERATED ALWAYS AS SUSER_SNAME END HIDDEN   NOT NULL,
    SYS_START      DATETIME2 (7) GENERATED ALWAYS AS ROW START                 NOT NULL,
    SYS_END        DATETIME2 (7) GENERATED ALWAYS AS ROW END                   NOT NULL,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history));


GO
CREATE TABLE t (
    COL0 INT MASKED WITH (FUNCTION = 'default()')
);


GO
CREATE TABLE t (
    COL0 INT MASKED WITH (FUNCTION = 'default()')                     ,
    COL1 VARCHAR (32) MASKED WITH (FUNCTION = 'partial(3, "XXXX", 4)') NOT NULL
);


GO
CREATE TABLE t (
    COL0 INT MASKED WITH (FUNCTION = 'default()')                     ,
    COL1 INT                                                          ,
    COL2 VARCHAR (32) MASKED WITH (FUNCTION = 'partial(3, "XXXX", 4)') NOT NULL,
    COL3 VARCHAR (32)                                                  NOT NULL
);


GO
CREATE TABLE t (
    COL0 INT                                                          ,
    COL1 INT MASKED WITH (FUNCTION = 'default()')                     ,
    COL2 VARCHAR (32)                                                  NOT NULL,
    COL3 VARCHAR (32) MASKED WITH (FUNCTION = 'partial(3, "XXXX", 4)') NOT NULL
);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (DISTRIBUTION = HASH(COL0));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (DISTRIBUTION = ROUND_ROBIN);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (DISTRIBUTION = REPLICATE);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED INDEX(COL0 ASC, COL1 DESC));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED COLUMNSTORE INDEX);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (HEAP);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(COL0 RANGE LEFT FOR VALUES ()));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(COL0 RANGE LEFT FOR VALUES ()), CLUSTERED COLUMNSTORE INDEX);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (DISTRIBUTION = HASH(COL0), PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)), CLUSTERED COLUMNSTORE INDEX);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(COL0 RANGE FOR VALUES ()));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED COLUMNSTORE INDEX, DISTRIBUTION = HASH(COL0), PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED INDEX(COL0 ASC, COL1 DESC), DISTRIBUTION = HASH(COL0), PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED INDEX(COL0 ASC, COL1 DESC), PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)), DISTRIBUTION = REPLICATE);


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (CLUSTERED INDEX(COL0 ASC, COL1 DESC), DISTRIBUTION = ROUND_ROBIN, PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    COL0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(Col0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    Col0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(COL0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE t (
    Col0 INT          NOT NULL,
    COL1 VARCHAR (20),
    COL2 VARCHAR (6) 
)
WITH (PARTITION(Col0 RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE [t6 a] (
    COL0          INT                                               ,
    COL1          XML                                               ,
    COL2          FLOAT                                             ,
    COL3          NVARCHAR (64)                                     ,
    [SYS_START a] DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END       DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([SYS_START a], SYS_END)
)
WITH (PARTITION([SYS_START a] RANGE RIGHT FOR VALUES ()));


GO
CREATE TABLE [t6 a] (
    COL0          INT                                               ,
    COL1          XML                                               ,
    COL2          FLOAT                                             ,
    COL3          NVARCHAR (64)                                     ,
    [SYS_START a] DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END       DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([SYS_START a], SYS_END)
)
WITH (DISTRIBUTION = HASH([SYS_START a]));


GO
CREATE TABLE [t6 a] (
    COL0          INT                                               ,
    COL1          XML                                               ,
    COL2          FLOAT                                             ,
    COL3          NVARCHAR (64)                                     ,
    [SYS_START a] DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END       DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([SYS_START a], SYS_END)
)
WITH (CLUSTERED INDEX([SYS_START a] ASC));


GO
CREATE TABLE [t6 a] (
    COL0          INT                                               ,
    COL1          XML                                               ,
    COL2          FLOAT                                             ,
    COL3          NVARCHAR (64)                                     ,
    [SYS_START a] DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    SYS_END       DATETIME2 (7) GENERATED ALWAYS AS ROW END HIDDEN   NOT NULL,
    PERIOD FOR SYSTEM_TIME ([SYS_START a], SYS_END)
)
WITH (CLUSTERED INDEX([SYS_START a] ASC), DISTRIBUTION = ROUND_ROBIN, PARTITION([SYS_START a] RANGE RIGHT FOR VALUES (10, 20)));


GO
CREATE TABLE dbo.Table_OrderCCI (
    [column_1] INT          NOT NULL,
    [column_2] NCHAR (10)   NULL,
    [column_3] INT         ,
    [column_4] NUMERIC (18) NULL,
    [zipCode]  VARCHAR (6) 
)
WITH (CLUSTERED COLUMNSTORE INDEX ORDER(column_1, column_3));


GO
CREATE TABLE [dbo].[table1] (
    [col1] INT NOT NULL CONSTRAINT [SystemGeneratedPkName] PRIMARY KEY NONCLUSTERED ([col1] ASC) NOT ENFORCED,
    [col2] INT NOT NULL
);


GO
CREATE TABLE [dbo].[table2] (
    [col1] INT NOT NULL PRIMARY KEY NONCLUSTERED ([col1] ASC) NOT ENFORCED,
    [col2] INT NOT NULL
);


GO
CREATE TABLE [dbo].[table3] (
    [col1] INT NOT NULL PRIMARY KEY NONCLUSTERED NOT ENFORCED,
    [col2] INT NOT NULL
);


GO
CREATE TABLE [dbo].[table4] (
    [col1] INT NOT NULL,
    [col2] INT NOT NULL,
    CONSTRAINT [SystemGeneratedPkName1] PRIMARY KEY NONCLUSTERED ([col1] ASC) NOT ENFORCED
);


GO
CREATE TABLE [dbo].[table5] (
    [col1] INT,
    [col2] INT UNIQUE NOT ENFORCED
);


GO
CREATE TABLE [dbo].[table6] (
    [col1] INT PRIMARY KEY NONCLUSTERED NOT ENFORCED,
    [col2] INT UNIQUE NOT ENFORCED
);


GO
CREATE TABLE [dbo].[table7] (
    [col1] INT NOT NULL,
    [col2] INT NOT NULL,
    CONSTRAINT [SystemGeneratedPkName1] UNIQUE ([col2] ASC) NOT ENFORCED
);


GO
CREATE TABLE [dbo].[table8] (
    [col1] INT NOT NULL UNIQUE ([col1] ASC) NOT ENFORCED,
    [col2] INT NOT NULL
);


GO
CREATE TABLE myTable1_HASH_MCD (
    id       INT          NOT NULL,
    lastName VARCHAR (20),
    zipCode  VARCHAR (6) 
)
WITH (DISTRIBUTION = HASH(id, lastName));


GO
CREATE TABLE myTable2_HEAP_HASH_MCD (
    id       INT          NOT NULL,
    lastName VARCHAR (20),
    zipCode  VARCHAR (6) 
)
WITH (DISTRIBUTION = HASH(id, lastName, zipCode), HEAP);


GO
CREATE TABLE myTable3_CI_HASH_MCD (
    id       INT          NOT NULL,
    lastName VARCHAR (20),
    zipCode  VARCHAR (6) 
)
WITH (DISTRIBUTION = HASH(id, lastName), CLUSTERED INDEX(zipCode));


GO
CREATE TABLE myTable4_CCI_HASH_MCD (
    [id]       INT          NOT NULL,
    [lastName] VARCHAR (20) NULL,
    [zipCode]  VARCHAR (6)  NULL
)
WITH (CLUSTERED COLUMNSTORE INDEX, DISTRIBUTION = HASH([id], [lastName], [zipCode]));