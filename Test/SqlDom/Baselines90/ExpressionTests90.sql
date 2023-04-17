SELECT t::a,
       t2::f(),
       dbo.[type 1]::[Property],
       [funcType]::f(DEFAULT, 1, @3 + t::Pi);


GO
SELECT a.b.c.f.e.g.h,
       k.l.m.n.o.func();


GO
SELECT @a.[f1](c1, c2),
       a.b.c.d.f().g.h.k(1, 2, DEFAULT).l;


GO
SELECT (a.b()).A,
       dbo.t1.c1.Prop.Func(DEFAULT, @a.A),
       t::a.f().G,
       [dbo].[type]::func().func2(@a.f());


GO
SELECT [db].$PARTITION.f1 (@a),
       db.$PARTITION.[f2] (1, 2 + 12),
       $PARTITION.f1 (12 + 12),
       $PARTITION.[part func] (a.b::f());


GO
SELECT t1.count(ALL c1) OVER (PARTITION BY c1),
       count(ALL c1) OVER (PARTITION BY c1, c2 + 10)
FROM t1;

SELECT time() OVER (),
       time(1) OVER (PARTITION BY c1, c2 + 3 ORDER BY c1),
       sum(*) OVER (PARTITION BY c1),
       [dbo].f1() OVER (PARTITION BY c1),
       .f2(DEFAULT, 'def') OVER (PARTITION BY c1, c2);

SELECT ROW_NUMBER() OVER (ORDER BY column_1 ASC) AS column_1
FROM Table1;