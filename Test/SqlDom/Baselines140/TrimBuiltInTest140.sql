SELECT trim('  TestString  ');

SELECT trim('[]' FROM '[abcdefgh]');


GO
DECLARE @str AS VARCHAR (10) = 'TestString';

DECLARE @chars AS VARCHAR (3) = 'Teg';

SELECT trim(@str);

SELECT trim(@chars FROM @str);


GO
SELECT trim(*)
FROM TestTable;

SELECT trim(col1 FROM col2)
FROM TestTable;


GO
SELECT trim(NULL);

SELECT trim('');


GO
SELECT trim(NULL FROM '[abcdefgh]');

SELECT trim('[]' FROM NULL);

SELECT trim(NULL FROM NULL);

SELECT trim('' FROM '[abcdefgh]');

SELECT trim('[]' FROM '');

SELECT trim('' FROM '');


GO
SELECT ltrim('  TestString');

SELECT substring('Test', 0, 2);


GO
SELECT trim(ltrim(' Test') COLLATE Latin1_General_100_BIN2 FROM N'TestString');