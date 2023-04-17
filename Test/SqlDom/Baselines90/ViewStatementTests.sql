CREATE VIEW schema1.view1 (
    a,
    b,
    c,
    d,
    jacl
)
WITH ENCRYPTION, SCHEMABINDING, VIEW_METADATA
AS
SELECT *
FROM schema1.table2
WITH CHECK OPTION;


GO
CREATE VIEW schema1.view1 (
    a,
    b,
    c,
    d,
    jacl
)
WITH ENCRYPTION, SCHEMABINDING, VIEW_METADATA
AS
SELECT *
FROM schema1.table2;


GO
CREATE VIEW view1 (
    a,
    b,
    c,
    d,
    jacl
)
WITH ENCRYPTION, SCHEMABINDING, VIEW_METADATA
AS
SELECT *
FROM schema1.table2;


GO
CREATE VIEW schema1.view1
WITH ENCRYPTION, SCHEMABINDING, VIEW_METADATA
AS
SELECT *
FROM schema1.table2;


GO
ALTER VIEW schema1.view1 (
    a,
    b,
    c,
    d,
    jacl
)
AS
SELECT *
FROM schema1.table2;


GO
ALTER VIEW View1
AS
SELECT *
FROM Table1;


GO
ALTER VIEW schema1.view1
WITH SCHEMABINDING
AS
SELECT c1,
       c2
FROM schema1.table2;


GO
CREATE VIEW v1
AS
SELECT *
FROM t1;


GO
CREATE VIEW v1
AS
SELECT *
FROM t1
WITH CHECK OPTION;


GO
CREATE VIEW "Category Sales for 1997"
AS
SELECT "Product Sales for 1997".CategoryName,
       Sum("Product Sales for 1997".ProductSales) AS CategorySales
FROM "Product Sales for 1997"
GROUP BY "Product Sales for 1997".CategoryName;

