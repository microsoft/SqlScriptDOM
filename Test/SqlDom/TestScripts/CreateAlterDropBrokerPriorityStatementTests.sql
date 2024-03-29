CREATE BROKER PRIORITY bp1 FOR CONVERSATION;

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = 5);

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = 5, CONTRACT_NAME = con_name);

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = 5, CONTRACT_NAME = con_name, REMOTE_SERVICE_NAME = 'ab');

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = 5, CONTRACT_NAME = con_name, REMOTE_SERVICE_NAME = 'ab', LOCAL_SERVICE_NAME = local_name);

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = DEFAULT);

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (REMOTE_SERVICE_NAME = ANY);

CREATE BROKER PRIORITY bp1 FOR CONVERSATION SET (CONTRACT_NAME = ANY);
GO

ALTER BROKER PRIORITY bp1 FOR CONVERSATION SET (PRIORITY_LEVEL = 5, CONTRACT_NAME = con_name, REMOTE_SERVICE_NAME = 'ab', LOCAL_SERVICE_NAME = local_name);
GO

DROP BROKER PRIORITY bp1;
