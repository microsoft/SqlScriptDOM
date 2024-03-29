
-- SEARCH PROPERTY LIST option and options syntax
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH SEARCH PROPERTY LIST OFF
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH SEARCH PROPERTY LIST = OFF
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH SEARCH PROPERTY LIST foo
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH (CHANGE_TRACKING = AUTO, SEARCH PROPERTY LIST = foo)
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH STOPLIST = system
CREATE FULLTEXT INDEX ON t1 KEY INDEX [i1] WITH (SEARCH PROPERTY LIST foo, STOPLIST = system, CHANGE_TRACKING = OFF, NO POPULATION)


GO

-- SET SEARCH PROPERTY LIST option in ALTER FULLTEXT INDEX
ALTER FULLTEXT INDEX ON t1 SET SEARCH PROPERTY LIST = OFF
ALTER FULLTEXT INDEX ON t1 SET SEARCH PROPERTY LIST OFF WITH NO POPULATION
ALTER FULLTEXT INDEX ON t1 SET SEARCH PROPERTY LIST = FOO WITH NO POPULATION
ALTER FULLTEXT INDEX ON t1 SET SEARCH PROPERTY LIST foo
go
--statistical semantics
create fulltext index on t1(c1 type column typecol language 1033 statistical_semantics, c2 statistical_semantics) key index i1
go
alter fulltext index on t1 add (c1 statistical_semantics, c2 language 0 statistical_semantics) with no population
go
alter fulltext index on t1 alter column c1 add statistical_semantics with no population
go
alter fulltext index on t2 alter column c2 drop statistical_semantics
go