CREATE TABLE table1 (
    a                    INT                                                    ,
    b                    INT                                                    ,
    TRANSACTIONID_START  BIGINT GENERATED ALWAYS AS TRANSACTION_ID START         NOT NULL,
    TRANSACTIONID_END    BIGINT GENERATED ALWAYS AS TRANSACTION_ID END          ,
    SEQUENCENUMBER_START BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER START HIDDEN NOT NULL,
    SEQUENCENUMBER_END   BIGINT GENERATED ALWAYS AS SEQUENCE_NUMBER END HIDDEN   NULL
);

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (TRANSACTION_ID_COLUMN_NAME=transactionId,SEQUENCE_NUMBER_COLUMN_NAME=SequenceNumber,OPERATION_TYPE_COLUMN_NAME=OperationId,OPERATION_TYPE_DESC_COLUMN_NAME=OperationTypeDescription)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (TRANSACTION_ID_COLUMN_NAME=transactionId)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (TRANSACTION_ID_COLUMN_NAME=transactionId,SEQUENCE_NUMBER_COLUMN_NAME=SequenceNumber)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (TRANSACTION_ID_COLUMN_NAME=transactionId,SEQUENCE_NUMBER_COLUMN_NAME=SequenceNumber,OPERATION_TYPE_COLUMN_NAME=OperationId)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON);

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = OFF);

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (SEQUENCE_NUMBER_COLUMN_NAME=SequenceNumber,OPERATION_TYPE_COLUMN_NAME=OperationId,OPERATION_TYPE_DESC_COLUMN_NAME=OperationTypeDescription)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger (TRANSACTION_ID_COLUMN_NAME=transactionId,SEQUENCE_NUMBER_COLUMN_NAME=SequenceNumber,OPERATION_TYPE_COLUMN_NAME=OperationId,OPERATION_TYPE_DESC_COLUMN_NAME=OperationTypeDescription)));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger,APPEND_ONLY= ON));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger,APPEND_ONLY= OFF));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (LEDGER_VIEW=dbo.t_ledger,APPEND_ONLY= ON));

CREATE TABLE t (
    COL0                INT                                            ,
    COL1                XML                                            ,
    COL2                FLOAT                                          ,
    COL3                NVARCHAR (64)                                  ,
    SYS_START           DATETIME2 (7) GENERATED ALWAYS AS ROW START     NOT NULL,
    SYS_END             DATETIME2 (7) GENERATED ALWAYS AS ROW END       NOT NULL,
    TRANSACTIONID_START BIGINT GENERATED ALWAYS AS TRANSACTION_ID START NOT NULL,
    TRANSACTIONID_END   BIGINT GENERATED ALWAYS AS TRANSACTION_ID END  ,
    CONSTRAINT PK_MY_PRIMARY_KEY PRIMARY KEY CLUSTERED (COL0),
    PERIOD FOR SYSTEM_TIME (SYS_START, SYS_END)
)
WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE=dbo.t_history), LEDGER = ON (APPEND_ONLY= ON));

