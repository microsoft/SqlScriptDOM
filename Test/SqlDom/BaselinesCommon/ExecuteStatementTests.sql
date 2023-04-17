EXECUTE @returnstatus = dbo.ufnGetSalesOrderStatusText @Status = 2;

EXECUTE dbo.ProcTestDefaults;1 DEFAULT, 'I', @p3 = DEFAULT;

EXECUTE dbo.Proc_Test_Defaults @p2 = @p10 OUTPUT
    WITH RECOMPILE;

EXECUTE ('ALTER INDEX ALL ON ' + @schemaname + '.' + @tablename + ' REBUILD;');

EXECUTE ('SELECT ' + @@ERROR);

EXECUTE @varName ;

EXECUTE OPENDATASOURCE ('SQLNCLI', 'Data Source=London\Payroll;Integrated Security=SSPI').AdventureWorks.HumanResources.SomeProcedure ;


GO
EXECUTE sp_grantdbaccess 'redmond\eftqa1';


GO
EXECUTE sp_grantdbaccess 'redmond\eftqa1';

SELECT *
FROM t1;

EXECUTE sp_addtype birthday, datetime, 'NULL';

EXECUTE a.b.c.d ;

