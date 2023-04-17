SELECT t::a COLLATE Albanian_BIN;


GO
SELECT (c1).SomeProperty COLLATE Albanian_BIN;


GO
SELECT c1.f1().SomeProperty COLLATE Albanian_BIN.AnotherProperty COLLATE Albanian_BIN;


GO
SELECT dbo.MyAgg(ALL c1, c2, 10) OVER ();


GO
SELECT dbo.MyAgg(DISTINCT c1, c2, 10);