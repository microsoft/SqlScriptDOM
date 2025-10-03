SELECT JSON_ARRAYAGG(value ORDER BY value)
FROM mytable;

SELECT JSON_ARRAYAGG(name ORDER BY name ASC)
FROM users;

SELECT JSON_ARRAYAGG(score ORDER BY score DESC)
FROM scores;

SELECT JSON_ARRAYAGG(data ORDER BY priority DESC, created_at ASC)
FROM records;

SELECT JSON_ARRAYAGG(value ORDER BY value NULL ON NULL)
FROM data;

SELECT TOP (5) c.object_id,
               JSON_ARRAYAGG(c.name ORDER BY c.column_id) AS column_list
FROM sys.columns AS c
GROUP BY c.object_id;