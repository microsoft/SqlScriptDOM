-- Test JSON_ARRAYAGG with ORDER BY clause

-- Basic ORDER BY
SELECT JSON_ARRAYAGG(value ORDER BY value) FROM mytable;

-- ORDER BY with ASC
SELECT JSON_ARRAYAGG(name ORDER BY name ASC) FROM users;

-- ORDER BY with DESC
SELECT JSON_ARRAYAGG(score ORDER BY score DESC) FROM scores;

-- ORDER BY with multiple columns
SELECT JSON_ARRAYAGG(data ORDER BY priority DESC, created_at ASC) FROM records;

-- ORDER BY with NULL CLAUSE
SELECT JSON_ARRAYAGG(value ORDER BY value NULL ON NULL) FROM data;

-- Real-world example with GROUP BY and system tables
SELECT TOP(5) c.object_id, JSON_ARRAYAGG(c.name ORDER BY c.column_id) AS column_list
FROM sys.columns AS c
GROUP BY c.object_id;