SELECT *
FROM OPENROWSET (
     PROVIDER = 'CosmosDB', CONNECTION = 'Account=synapselink-cosmosdb-sqlsample;Database=db1', OBJECT = 'a', CREDENTIAL = 'MyCredential')
     WITH (date_rep VARCHAR (20), cases BIGINT, geo_id VARCHAR (6)) AS rows;

SELECT *
FROM OPENROWSET (
     PROVIDER = 'CosmosDB', CONNECTION = 'Account=synapselink-cosmosdb-sqlsample;Database=db2', OBJECT = 'b', CREDENTIAL = 'MyCosmosDbAccountCredential') AS rows;

SELECT *
FROM OPENROWSET (
     PROVIDER = 'CosmosDB', CONNECTION = 'Account=synapselink-cosmosdb-sqlsample;Database=db3', OBJECT = 'c d', SERVER_CREDENTIAL = 'My Server Credential')
     WITH (date_rep VARCHAR (20), cases BIGINT) AS rows;

SELECT *
FROM OPENROWSET (
     PROVIDER = 'CosmosDB', CONNECTION = 'Account=synapselink-cosmosdb-sqlsample;Database=mydb', OBJECT = 'object')
     WITH (date_rep VARCHAR (20), cases BIGINT) AS rows;

SELECT *
FROM OPENROWSET (
     PROVIDER = 'CosmosDB', CONNECTION = 'Account=synapselink-cosmosdb-sqlsample;Database=mydb', OBJECT = 'object');

SELECT TOP 100 UserID
FROM OPENROWSET (BULK 'path', FORMAT = 'SStream', PARSER_VERSION = '2.0') AS a;

SELECT TOP 10 *
FROM OPENROWSET ('CosmosDB', 'Accnt', myTable)
     WITH (date_rep VARCHAR (20), cases BIGINT, geo_id VARCHAR (6)) AS rows;

SELECT TOP 10 *
FROM OPENROWSET ('CosmosDB', 'x', t)
     WITH (a VARCHAR (20), b BIGINT, c VARCHAR (6));