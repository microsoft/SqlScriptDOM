EXECUTE ('SELECT ProductID, Name 
    FROM AdventureWorks.Production.Product
    WHERE ProductID = ? ', 952) AS USER = 'User1' AT SeattleSales;


GO
EXECUTE ('select') AS LOGIN;

EXECUTE ('select' + ' * from t1', 5, @a) AS LOGIN;

