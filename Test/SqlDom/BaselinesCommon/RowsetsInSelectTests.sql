SELECT FT_TBL.Description,
       FT_TBL.CategoryName,
       KEY_TBL.RANK
FROM Categories AS FT_TBL
     INNER JOIN
     CONTAINSTABLE (Categories, *, '("sweet and savory" NEAR sauces) OR
      ("sweet and savory" NEAR candies)', 10)
     ON FT_TBL.CategoryID = KEY_TBL.[KEY];

SELECT FT_TBL.CategoryName,
       FT_TBL.Description,
       KEY_TBL.RANK
FROM Categories AS FT_TBL
     INNER JOIN
     FREETEXTTABLE (Categories, Description, 'sweetest candy bread and dry meat') AS KEY_TBL
     ON FT_TBL.CategoryID = KEY_TBL.[KEY];

SELECT a.*
FROM OPENROWSET ('SQLOLEDB', N'seattle1'; 'manager'; 'MyPass', 'SELECT * FROM pubs.dbo.authors ORDER BY au_lname, au_fname') AS a;

SELECT a.*
FROM OPENROWSET ('MSDASQL', 'DRIVER={SQL Server};SERVER=seattle1;UID=manager;PWD=PLACEHOLDER', [pubs].[dbo].[authors]);

SELECT *
FROM OPENQUERY (OracleSvr, 'SELECT name, id FROM joe.titles') AS a;

SELECT *
FROM OPENDATASOURCE ('SQLOLEDB', 'Data Source=ServerName;User ID=MyUID;Password=PLACEHOLDER').'Something' AS Z;

SELECT *
FROM OPENDATASOURCE ('Microsoft.Jet.OLEDB.4.0', 'Data Source="c:\Finance\account.xls";User ID=Admin;Password=;Extended properties=Excel 5.0')...xactions;

SELECT *
FROM OPENXML (@idoc, '/ROOT/Customer', 1) WITH (CustomerID VARCHAR (10) '../@CustomerID', ContactName VARCHAR (20));

SELECT *
FROM OPENXML (@idoc, '/ROOT/Customer/Order/OrderDetail') WITH t1;

SELECT *
FROM OPENXML (@idoc, '/ROOT/Customer/Order/OrderDetail') AS xxx;

SELECT *
INTO #TempEdge
FROM OPENXML (@xmldoc, @xpath);

SELECT *
FROM OPENXML (@idoc, '/root/Customer/Order', 1) WITH (oid CHAR (5), amount FLOAT, comment NTEXT 'text()');

EXECUTE sp_xml_removedocument @idoc;

SELECT *
FROM OPENROWSET (something, @var1);

SELECT *
FROM Categories AS FT_TBL
     INNER JOIN
     CONTAINSTABLE (Categories, (c1, c2), '("sweet and savory" NEAR sauces)', LANGUAGE 10, 10)
     ON FT_TBL.CategoryID = KEY_TBL.[KEY];

SELECT *
FROM Categories AS FT_TBL
     INNER JOIN
     CONTAINSTABLE (Categories, *, '("sweet and savory" NEAR sauces)', LANGUAGE 10, 3)
     ON FT_TBL.CategoryID = KEY_TBL.[KEY];

