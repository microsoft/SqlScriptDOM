ALTER INDEX sxi1
    ON t1 
FOR
(
     REMOVE  path1
);

ALTER INDEX sxi1
    ON t1 
FOR
(
     ADD path1 = '/a/b/c'
);

ALTER INDEX sxi1
    ON t1 
FOR
(
     ADD path1 = '/a/b/c' AS XQUERY 'xs:string'
);

ALTER INDEX sxi1
    ON t1 
FOR
(
     ADD path1 = '/a/b/c' AS XQUERY 'xs:string' SINGLETON
);

ALTER INDEX sxi1
    ON t1 
FOR
(
     ADD path1 = '/a/b/c' AS XQUERY 'xs:string' MAXLENGTH(50),
     REMOVE  path2,
     ADD path3 = '/a/b/f' AS XQUERY 'node()'
);

ALTER INDEX sxi1
    ON t1 
WITH XMLNAMESPACES ('www.microsoft.com' AS ns1, 'www.google.com' AS ns2)
FOR
(
     REMOVE  path1,
     REMOVE  path3,
     ADD path2 = '/a/b/c' AS XQUERY 'xs:string' MAXLENGTH(50),
     ADD path7 = '/a/b/c' AS XQUERY 'xs:double'
);