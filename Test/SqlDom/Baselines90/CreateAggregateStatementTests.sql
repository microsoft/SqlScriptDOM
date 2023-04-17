CREATE AGGREGATE Concatenate(@input NVARCHAR (4000))
    RETURNS NVARCHAR (4000)
    EXTERNAL NAME [StringUtilities].[Microsoft.Samples.SqlServer.Concatenate];

CREATE AGGREGATE dbo.a1(@input INT)
    RETURNS INT
    EXTERNAL NAME IntUtilities;

CREATE AGGREGATE a1(@input XML( zzz))
    RETURNS XML( zzz)
    EXTERNAL NAME ag1;