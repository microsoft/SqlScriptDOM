CREATE TYPE SSN
    FROM VARCHAR (11) NOT NULL;

CREATE TYPE [schema].SSN
    FROM INT NULL;

CREATE TYPE [schema].SSN
    FROM DECIMAL (MAX);

CREATE TYPE SSN
     EXTERNAL NAME [SomeAssembly];

CREATE TYPE Utf8String
     EXTERNAL NAME utf8string.[Microsoft.Samples.SqlServer.utf8string];

