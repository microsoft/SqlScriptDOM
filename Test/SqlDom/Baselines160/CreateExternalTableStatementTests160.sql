CREATE EXTERNAL TABLE t1 (
    c1 INT,
    c2 CHAR (16) COLLATE Latin1_General_BIN2 NOT NULL,
    c3 DATE,
    c4 DECIMAL (5, 2) NULL,
    c5 VARCHAR (50) COLLATE Latin1_General_BIN2 NULL
)
    WITH (
    DATA_SOURCE = eds1,
    LOCATION = '/test/test.txt',
    FILE_FORMAT = eff1,
    TABLE_OPTIONS = N'{"READ_OPTIONS":
["ALLOW_INCONSISTENT_READS"]
}'
    );

CREATE EXTERNAL TABLE t2 (
    c1 INT,
    c2 CHAR (16) COLLATE Latin1_General_BIN2 NOT NULL,
    c3 DATE,
    c4 DECIMAL (5, 2) NULL,
    c5 VARCHAR (50) COLLATE Latin1_General_BIN2 NULL
)
    WITH (
    DATA_SOURCE = eds1,
    LOCATION = '/test/test.txt',
    FILE_FORMAT = eff1,
    TABLE_OPTIONS = N'randomstring��?Ƕ�'
    );