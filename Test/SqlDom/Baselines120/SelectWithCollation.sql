SELECT MyColumn.MyFunctionWhichReturnsSomething(param).MyOtherFunction() COLLATE SQL_Latin1_General_CP1_CI_AS;


GO
SELECT [MyColumn].MyFunctionWhichReturnsSomething(param).MyOtherFunction() COLLATE SQL_Latin1_General_CP1_CI_AS;


GO
SELECT a().b(param).c([column]) COLLATE SQL_Latin1_General_CP1_CI_AS;


GO
SELECT a() COLLATE SQL_Latin1_General_CP1_CI_AS;


GO
SELECT [a] COLLATE some_collation,
       [b] COLLATE some_other_collation AS ColumnA;


GO
SELECT func([a]).func([b]).func(1, 2, 3, g(4)) COLLATE some_collation AS FunctionCalls,
       [b] COLLATE some_other_collation AS ColumnB
WHERE g([a]) > 0;