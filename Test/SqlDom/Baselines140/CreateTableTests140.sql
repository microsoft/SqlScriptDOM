CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_pk (
    COL0 INT PRIMARY KEY WITH (IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON))
);

CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_uk (
    COL0 INT UNIQUE WITH (IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON))
);

CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_pk (
    COL0 INT PRIMARY KEY WITH (IGNORE_DUP_KEY = ON)
);

CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_uk (
    COL0 INT UNIQUE WITH (IGNORE_DUP_KEY = ON)
);

CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_pk (
    COL0 INT PRIMARY KEY WITH (ALLOW_PAGE_LOCKS = ON, IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON), ALLOW_ROW_LOCKS = ON)
);

CREATE TABLE tab_with_ignoredupkey_suppressmessages_on_uk (
    COL0 INT UNIQUE WITH (ALLOW_PAGE_LOCKS = ON, IGNORE_DUP_KEY = ON (SUPPRESS_MESSAGES = ON), ALLOW_ROW_LOCKS = ON)
);

CREATE TABLE tab_with_hidden_on_all_column_types (
    pk    INT                     PRIMARY KEY,
    col1  BIGINT HIDDEN          ,
    col2  NUMERIC (10, 5) HIDDEN ,
    col3  BIT HIDDEN             ,
    col4  SMALLINT HIDDEN        ,
    col5  DECIMAL (5, 2) HIDDEN  ,
    col6  SMALLMONEY HIDDEN      ,
    col7  INT HIDDEN             ,
    col8  TINYINT HIDDEN         ,
    col9  MONEY HIDDEN           ,
    col10 FLOAT HIDDEN           ,
    col11 REAL HIDDEN            ,
    col12 DATE HIDDEN            ,
    col13 DATETIMEOFFSET HIDDEN  ,
    col14 DATETIME2 HIDDEN       ,
    col15 DATETIME2 (5) HIDDEN   ,
    col16 SMALLDATETIME HIDDEN   ,
    col17 DATETIME HIDDEN        ,
    col18 TIME HIDDEN            ,
    col19 CHAR HIDDEN            ,
    col20 CHAR (20) HIDDEN       ,
    col21 VARCHAR HIDDEN         ,
    col22 VARCHAR (5) HIDDEN     ,
    col23 VARCHAR (MAX) HIDDEN   ,
    col24 TEXT HIDDEN            ,
    col25 NCHAR HIDDEN           ,
    col26 NCHAR (32) HIDDEN      ,
    col27 NVARCHAR HIDDEN        ,
    col28 NVARCHAR (256) HIDDEN  ,
    col29 NVARCHAR (MAX) HIDDEN  ,
    col30 NTEXT HIDDEN           ,
    col31 BINARY HIDDEN          ,
    col32 BINARY (8) HIDDEN      ,
    col33 VARBINARY HIDDEN       ,
    col34 VARBINARY (32) HIDDEN  ,
    col35 IMAGE HIDDEN           ,
    col36 TIMESTAMP HIDDEN       ,
    col37 hierarchyid HIDDEN     ,
    col38 UNIQUEIDENTIFIER HIDDEN,
    col39 UNIQUEIDENTIFIER HIDDEN ROWGUIDCOL UNIQUE,
    col40 SQL_VARIANT HIDDEN     ,
    col41 XML HIDDEN             ,
    col42 geometry HIDDEN        ,
    col43 geography HIDDEN       
);

CREATE TABLE tab_with_hidden_cols (
    [A] INT                    ,
    [B] CHAR (32) HIDDEN        NULL,
    [C] FLOAT HIDDEN            NOT NULL,
    [D] INT HIDDEN              DEFAULT 1,
    [E] INT HIDDEN              DEFAULT 1 PRIMARY KEY,
    [F] DATETIME2 (5) HIDDEN    CONSTRAINT [constr1] DEFAULT '2010/01/01' NOT NULL,
    [G] NCHAR (64) HIDDEN       CONSTRAINT [constr1] DEFAULT N'SQL' NULL,
    [H] INT HIDDEN              IDENTITY,
    [I] INT HIDDEN              IDENTITY NOT FOR REPLICATION,
    [J] NCHAR (10)              COLLATE Traditional_Spanish_ci_ai HIDDEN NOT NULL,
    [K] INT HIDDEN              UNIQUE,
    [L] UNIQUEIDENTIFIER HIDDEN DEFAULT '123' ROWGUIDCOL NOT NULL UNIQUE
);

CREATE TABLE tab_with_hidden_and_temporal (
    [sys_start]    DATETIME2 (7) GENERATED ALWAYS AS ROW START HIDDEN NOT NULL,
    [sys_end]      DATETIME2 (7) GENERATED ALWAYS AS ROW END          NOT NULL,
    [product_code] INT                                                NOT NULL,
    [value]        INT HIDDEN                                        ,
    [row_id]       INT                                                IDENTITY,
    CONSTRAINT [constr1] PRIMARY KEY NONCLUSTERED ([product_code]),
    PERIOD FOR SYSTEM_TIME (sys_start, sys_end)
)
WITH (MEMORY_OPTIMIZED = OFF, SYSTEM_VERSIONING = ON (HISTORY_TABLE=tab_history));

CREATE TABLE tab_with_hidden_and_sparse (
    [A]        INT                                         ,
    [B]        CHAR (32) SPARSE HIDDEN                      NULL,
    [C]        FLOAT SPARSE                                 NULL,
    [D]        NCHAR (64) SPARSE HIDDEN                     NULL,
    [SpColset] XML COLUMN_SET FOR ALL_SPARSE_COLUMNS HIDDEN
);

CREATE TABLE tab_with_hidden_and_encrypted (
    CustName NVARCHAR (60) COLLATE Latin1_General_BIN2  ENCRYPTED WITH (
     COLUMN_ENCRYPTION_KEY = key1,
     ENCRYPTION_TYPE = RANDOMIZED,
     ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
    ) HIDDEN,
    SSN      VARCHAR (11)  COLLATE Latin1_General_BIN2  ENCRYPTED WITH (
     COLUMN_ENCRYPTION_KEY = key1,
     ENCRYPTION_TYPE = DETERMINISTIC,
     ALGORITHM = 'AEAD_AES_256_CBC_HMAC_SHA_256'
    ),
    Age      INT           NULL
);

CREATE TABLE tab_with_hidden_and_masked (
    [PK]    INT                                                       NOT NULL,
    [Data1] NVARCHAR (32) MASKED WITH (FUNCTION = 'default()') HIDDEN DEFAULT 'a' NOT NULL,
    [Data2] NVARCHAR (32) MASKED WITH (FUNCTION = 'default()')        DEFAULT 'b' NOT NULL,
    CONSTRAINT [constr1] PRIMARY KEY ([PK])
);

CREATE TABLE tab_with_hidden_and_filestream (
    [Id]           UNIQUEIDENTIFIER                  ROWGUIDCOL NOT NULL UNIQUE,
    [SerialNumber] INT                               UNIQUE,
    [Chart]        VARBINARY (MAX) FILESTREAM HIDDEN NULL
);