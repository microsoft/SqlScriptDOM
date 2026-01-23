SELECT (SELECT 1
        WHERE (IIF (1 > 0
                    AND 2 > 1, 1, 0)) = 1);


GO
SELECT 1
WHERE (IIF (1 > 0, 1, 0)) = 1;


GO
SELECT 1
WHERE (IIF (1 > 0
            AND 2 > 1
            OR 3 < 4, 1, 0)) = 1;


GO
SELECT 1
WHERE (IIF (1 >= 0
            AND 2 <= 1
            AND 3 <> 4, 1, 0)) = 1;


GO
SELECT 1
WHERE (IIF (IIF (1 > 0, 1, 0) > 0, 'yes', 'no')) = 'yes';


GO
SELECT (IIF (1 > 0, 1, 0)) + 1 AS result;


GO
SELECT 1
WHERE (IIF (1 > 0, 1, 0)) = 1
      AND (IIF (2 > 1, 1, 0)) = 1;


GO
SELECT 1
WHERE IIF (1 > 0
           AND 2 > 1, 1, 0) = 1;


GO
SELECT 1
WHERE ((((IIF (1 > 0
               AND 2 > 1, 1, 0))))) = 1;


GO
SELECT 1
WHERE (((((IIF (1 > 0
                AND 2 > 1
                OR 3 < 4, 1, 0)))))) = 1;


GO
SELECT 1
WHERE (((IIF (1 > 0, 1, 0)) = 1)
       AND ((IIF (2 > 1, 1, 0)) = 1));


GO
SELECT ((((IIF (1 > 0
                AND 2 > 1, 10, 20))))) + 5 AS result;


GO
SELECT 1
WHERE ((IIF ((IIF (1 > 0, 1, 0)) > 0, 'yes', 'no'))) = 'yes';


GO
SELECT 1
WHERE (IIF (((IIF (1 > 0
                   AND 2 > 1, 1, 0))) > 0, 'a', 'b')) = 'a';


GO
SELECT 1
WHERE ((IIF ((IIF ((IIF (1 > 0, 1, 0)) > 0, 2, 3)) > 1, 'x', 'y'))) = 'x';


GO
SELECT 1
WHERE (((IIF (((IIF (1 > 0
                     AND 2 > 1, 1, 0))) = 1
              AND 3 < 5, 'pass', 'fail')))) = 'pass';


GO
SELECT 1 AS IIF
FROM T1;


