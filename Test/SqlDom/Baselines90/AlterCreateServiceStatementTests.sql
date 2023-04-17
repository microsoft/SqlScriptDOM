ALTER SERVICE s1 ON QUEUE dbo.q1;

ALTER SERVICE s1 (ADD CONTRACT c1, DROP CONTRACT c2, ADD CONTRACT c3);

ALTER SERVICE [//Adventure-Works.com/Expenses] (ADD CONTRACT [//Adventure-Works.com/Expenses/ExpenseProcessing]);


GO
CREATE SERVICE s1
    AUTHORIZATION zzz
    ON QUEUE q1;

CREATE SERVICE s1
    ON QUEUE dbo.q1
    (c1, c2);

CREATE SERVICE s1
    ON QUEUE q1
    (c1);

