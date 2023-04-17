SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS NOT DISTINCT FROM 1;

SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS DISTINCT FROM 1;

SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS NULL;

SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS NOT NULL;

SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS NOT NULL;

SELECT Tab1.name,
       Tab2.id
FROM Tab1, Tab2
WHERE Tab1.id IS NULL;

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS DISTINCT FROM ANY (SELECT Tab2.name
                                      FROM Tab2);

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS DISTINCT FROM ANY (SELECT Tab2.name
                                      FROM Tab2);

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS DISTINCT FROM ALL (SELECT Tab2.name
                                      FROM Tab2);

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS NOT DISTINCT FROM ANY (SELECT Tab2.name
                                          FROM Tab2);

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS NOT DISTINCT FROM ANY (SELECT Tab2.name
                                          FROM Tab2);

SELECT Tab1.name
FROM Tab1
WHERE Tab1.name IS NOT DISTINCT FROM ALL (SELECT Tab2.name
                                          FROM Tab2);
