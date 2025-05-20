-- FOR XML in subquery tests
SELECT *
FROM dbo.foo
FOR XML PATH(''), ROOT ('x'), TYPE;

SET @x = (SELECT *
    FROM dbo.foo
    FOR XML PATH(''), ROOT ('x'), TYPE);

-- FOR JSON in subquery tests
SELECT *
FROM dbo.foo
FOR JSON PATH, ROOT ('x');

SET @x = (SELECT *
    FROM dbo.foo
    FOR JSON PATH, ROOT ('x'));