CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c'
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY MAXLENGTH(50)
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'xs:string'
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'xs:string' SINGLETON
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'xs:string' MAXLENGTH(50)
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'xs:string' MAXLENGTH(50) SINGLETON
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'node()'
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS XQUERY 'node()' SINGLETON
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c',
        path3 = '/a/b/e' AS XQUERY 'xs:string' MAXLENGTH(50) SINGLETON,
        path1 = '/a/b/f' AS XQUERY 'node()'
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS SQL NVARCHAR (50)
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS SQL NVARCHAR (50) SINGLETON
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c',
        path3 = '/a/b/e' AS XQUERY 'xs:string' MAXLENGTH(50) SINGLETON,
        path4 = '/a/b/f' AS XQUERY 'node()',
        path6 = '/a/b/n' AS SQL NVARCHAR (50),
        path5 = '/a/b/m' AS SQL NVARCHAR (50) SINGLETON
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
WITH XMLNAMESPACES ('www.microsoft.com' AS ns1, 'www.google.com' AS ns2)
FOR(    path1 = '/a/b/c'
);

CREATE XML INDEX sxisecondary ON t1(c1)
USING XML INDEX sxi1
FOR( path1
);

CREATE SELECTIVE XML INDEX sxi1 ON t1(c1)
FOR(    path1 = '/a/b/c' AS SQL NVARCHAR (50)
)
WITH (DROP_EXISTING = ON, FILLFACTOR = 2);
