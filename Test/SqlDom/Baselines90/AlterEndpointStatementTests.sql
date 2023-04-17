ALTER ENDPOINT e1 
    STATE = STARTED, 
    AFFINITY = NONE;

ALTER ENDPOINT e1 
    STATE = DISABLED, 
    AFFINITY = ADMIN;

ALTER ENDPOINT e1 
    AFFINITY = 1000000
    AS TCP (
            LISTENER_PORT = 4022
           );

ALTER ENDPOINT e1 
    FOR SOAP (
            ADD WEBMETHOD 'm1'(NAME = 'n1'),
            ALTER WEBMETHOD 'm2'(NAME = 'n2')
             );

ALTER ENDPOINT e1 
    FOR SOAP (
            DROP WEBMETHOD 'm1'
             );

