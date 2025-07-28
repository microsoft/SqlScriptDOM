SELECT id,
       json_col
FROM tab1
WHERE ISJSON(json_col) = 1;

SELECT id,
       json_col
FROM tab1
WHERE ISJSON(json_col, SCALAR) = 1;

SELECT ISJSON('true', VALUE);

DECLARE @jsonInfo AS NVARCHAR (MAX);

SET @jsonInfo = N'{"info":{"address":[{"town":"Paris"},{"town":"London"}]}}';

SELECT JSON_PATH_EXISTS(@jsonInfo, '$.info.address');

SELECT JSON_OBJECT('name':'value');

SELECT JSON_OBJECT('name':'value', 'type':1 NULL ON NULL);

SELECT JSON_OBJECT(NULL ON NULL);

SELECT JSON_OBJECT('name':'value', 'type':NULL ABSENT ON NULL);

SELECT JSON_OBJECT('name':'value', 'type':JSON_OBJECT('type_id':1, 'name':'a'));

SELECT JSON_OBJECT();

SELECT JSON_OBJECT('name':'value', 'type':1);

SELECT JSON_OBJECT('name':'value', 'type':JSON_ARRAY(1, 2));

DECLARE @id_key AS NVARCHAR (10) = N'id', @id_value AS NVARCHAR (64) = NEWID();

SELECT JSON_OBJECT('user_name':USER_NAME(), @id_key:@id_value, 'sid':(SELECT @@SPID));

SELECT s.session_id,
       JSON_OBJECT('security_id':s.security_id, 'login':s.login_name, 'status':s.status) AS info
FROM sys.dm_exec_sessions AS s
WHERE s.is_user_process = 1;

SELECT JSON_OBJECT('name':'b' RETURNING JSON);

SELECT JSON_OBJECT('name':'b' NULL ON NULL RETURNING JSON);

SELECT JSON_OBJECT('name':'b' ABSENT ON NULL RETURNING JSON);

SELECT JSON_ARRAY('a', JSON_OBJECT('name':'value', 'type':1 NULL ON NULL) NULL ON NULL);

SELECT JSON_ARRAY();

SELECT JSON_ARRAY('name');

SELECT JSON_ARRAY('a', 1, 'b', 2);

SELECT JSON_ARRAY('a', 1, NULL, 2 NULL ON NULL);

SELECT JSON_ARRAY('a', 1, NULL, 2 ABSENT ON NULL);

SELECT JSON_ARRAY(NULL ON NULL);

SELECT JSON_ARRAY(ABSENT ON NULL);

DECLARE @id_value AS NVARCHAR (64) = NEWID();

SELECT JSON_ARRAY(1, @id_value, (SELECT @@SPID));

SELECT s.session_id,
       JSON_ARRAY(s.host_name, s.program_name, s.client_interface_name)
FROM sys.dm_exec_sessions AS s
WHERE s.is_user_process = 1;

SELECT JSON_ARRAY('a', 1, NULL, 2 RETURNING JSON);

SELECT JSON_ARRAY('a', 1, NULL, 2 NULL ON NULL RETURNING JSON);

SELECT JSON_ARRAY('a', 1, NULL, 2 ABSENT ON NULL RETURNING JSON);

SELECT JSON_OBJECTAGG('name':'value');

SELECT JSON_OBJECTAGG('name':'value' NULL ON NULL);

SELECT JSON_OBJECTAGG(NULL ON NULL);

SELECT JSON_OBJECTAGG('name':NULL ABSENT ON NULL);

SELECT JSON_OBJECTAGG('name':JSON_OBJECT('type_id':1, 'name':'a'));

SELECT JSON_OBJECTAGG();

SELECT JSON_OBJECTAGG('name':1);

SELECT JSON_OBJECTAGG('name':JSON_ARRAY(1, 2));
SELECT JSON_OBJECTAGG('name':'b' NULL ON NULL RETURNING JSON);

SELECT JSON_OBJECTAGG('name':'b' ABSENT ON NULL RETURNING JSON);

SELECT JSON_OBJECTAGG('name':'b' RETURNING JSON);

