COPY INTO FactOnlineSales FROM 'https://contosoretaildw.blob.core.windows.net/contosoretaildw-tables/FactOnlineSales/'
WITH (
  FieldTerminator = '|'
, DateFormat = 'YMD'
, RowTerminator = '0X0A'
);

COPY INTO dbo.[FactOnlineSalesv1] FROM 'https://unsecureaccount.blob.core.windows.net/customerdatasets/linitem.csv';

COPY INTO test_1 FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_1.txt'
WITH (
  File_Type = 'CSV'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'YMD'
, MaxErrors = 10
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
, FirstRow = 2
);

COPY INTO test_2 FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_2.txt'
WITH (
  File_Type = 'CSV'
, Identity_Insert = 'ON'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'YMD'
, MaxErrors = 10
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
);

COPY INTO test_3( Col_one DEFAULT 'stringdefault' 1,  Col_two DEFAULT 1 3) FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_3.txt'
WITH (
  File_Type = 'CSV'
, Identity_Insert = 'OFF'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'YMD'
, MaxErrors = 10
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
);

COPY INTO test_4 FROM 'https://polybasewasb.blob.core.windows.net/customerdatasets/parquet/test.parquet'
WITH (
  File_Type = 'PARQUET'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
);

COPY INTO test_5 FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_5.txt'
WITH (
  File_Type = 'PARQUET'
, File_Format = FILEFORMAT
, Identity_Insert = 'ON'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'YMD'
, MaxErrors = 10
, Compression = 'GZIP'
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
);

COPY INTO test_6 FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_6.txt'
WITH (
  File_Type = 'ORC'
, Identity_Insert = 'ON'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'YMD'
, MaxErrors = 10
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
, Compression = 'DEFAULTCODEC'
, ERRORFILE_CREDENTIAL = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
);

COPY INTO FactOnlineSales FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_3.txt', 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_4.txt'
WITH (
  File_Type = 'CSV'
, Identity_Insert = 'ON'
, Credential = (Identity = 'SHARED ACCESS SIGNATURE',Secret = '<YOUR_SAS_TOKEN>')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'DMY'
, MaxErrors = 10
, Compression = 'SNAPPY'
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
);

COPY INTO FactOnlineSales FROM 'https://loadingtest.blob.core.windows.net/customerdatasets/sastest/test_3.txt'
WITH (
  File_Type = 'CSV'
, Identity_Insert = 'ON'
, Credential = (Identity = 'MANAGED IDENTITY')
, FieldQuote = '"'
, FirstRow = 2
, FieldTerminator = ';'
, RowTerminator = '0X0A'
, Encoding = 'UTF8'
, DateFormat = 'DMY'
, MaxErrors = 10
, Compression = 'GZIP'
, ErrorFile = '/SASTEST/ERRORSFOLDER/'
);

