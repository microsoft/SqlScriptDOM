SET @a = 45;

SET @b = 'Hello';

SET @c = (@a + @c - 23) / @a;

SET @CursorVar = CURSOR SCROLL DYNAMIC
    FOR SELECT LastName,
               FirstName
        FROM Northwind.dbo.Employees
        WHERE LastName LIKE 'B%';

SET @var1 = @var2;

