CREATE PROCEDURE [sp_hk]
WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER
AS
BEGIN ATOMIC 
WITH (TRANSACTION ISOLATION LEVEL = READ COMMITTED, LANGUAGE = N'us_english', datefirst = 1, dateformat = 'mdy')
	SELECT 1
END	
go

CREATE PROCEDURE [sp_hk]
WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER
AS
BEGIN ATOMIC 
WITH (DELAYED_DURABILITY = on, TRANSACTION ISOLATION LEVEL = READ UNCOMMITTED, LANGUAGE = N'us_english', datefirst = 1, dateformat = 'mdy')
	SELECT 1
END	
go

CREATE PROCEDURE [sp_hk]
WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER
AS
BEGIN ATOMIC 
WITH (TRANSACTION ISOLATION LEVEL = REPEATABLE READ, delayed_durability = off, LANGUAGE = N'us_english', datefirst = 1, dateformat = 'mdy')
	SELECT 1
END	
go

CREATE PROCEDURE [sp_hk]
WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER
AS
BEGIN ATOMIC 
WITH (TRANSACTION ISOLATION LEVEL = SERIALIZABLE, LANGUAGE = N'us_english', datefirst = 1, dateformat = 'mdy')
	SELECT 1
END	
go

CREATE PROCEDURE [sp_hk]
WITH NATIVE_COMPILATION, SCHEMABINDING, EXECUTE AS OWNER
AS
BEGIN ATOMIC 
WITH (TRANSACTION ISOLATION LEVEL = SNAPSHOT, LANGUAGE = N'us_english', datefirst = 1, dateformat = 'mdy', delayed_durability = on)
	SELECT 1
END	
go

-- Make sure setting ISOLATION LEVEL works as before (code was refectoried)
SET TRANSACTION ISOLATION LEVEL READ COMMITTED;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
SET TRANSACTION ISOLATION LEVEL REPEATABLE READ;
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;
SET TRANSACTION ISOLATION LEVEL SNAPSHOT;