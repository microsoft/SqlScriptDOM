CREATE ENDPOINT e1
    AUTHORIZATION zzz 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR TSQL();

CREATE ENDPOINT e1 
    STATE = STARTED
    AS TCP (
            LISTENER_IP = ALL,
            LISTENER_PORT = 4022
           )
    FOR TSQL();

CREATE ENDPOINT e1 
    STATE = STOPPED
    AS TCP (
            LISTENER_IP = (1.2.3.4)
           )
    FOR TSQL();

CREATE ENDPOINT e1 
    STATE = STOPPED
    AS TCP (
            LISTENER_IP = (1.1.1.1:10.10.20.30)
           )
    FOR TSQL();

CREATE ENDPOINT e1
    AUTHORIZATION zzz 
    STATE = DISABLED
    AS TCP (
            LISTENER_IP = ('Some IpV6')
           )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS HTTP (
            PATH = 'url',
            AUTHENTICATION = (BASIC),
            PORTS = (CLEAR)
            )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS HTTP (
            AUTHENTICATION = (BASIC, DIGEST, INTEGRATED, KERBEROS, NTLM),
            PORTS = (SSL)
            )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS HTTP (
            PORTS = (CLEAR, SSL),
            SITE = '*',
            CLEAR_PORT = 10,
            SSL_PORT = 20
            )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS HTTP (
            AUTH_REALM = 'some realm',
            DEFAULT_LOGON_DOMAIN = 'redmond',
            COMPRESSION = DISABLED
            )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS HTTP (
            AUTH_REALM = NONE,
            DEFAULT_LOGON_DOMAIN = NONE,
            COMPRESSION = ENABLED
            )
    FOR TSQL();

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
             WEBMETHOD 'n1'.'m1'(NAME = 'd1.dbo.n1'),
             WEBMETHOD 'm2'(NAME = 'zzz', SCHEMA = NONE, FORMAT = NONE),
            BATCHES = ENABLED
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
             WEBMETHOD 'm1'(NAME = 'zzz', SCHEMA = STANDARD),
             WEBMETHOD 'm2'(NAME = 'zzz', SCHEMA = DEFAULT, FORMAT = ALL_RESULTS),
            BATCHES = DISABLED
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
             WEBMETHOD 'm1'(NAME = 'zzz', FORMAT = ROWSETS_ONLY),
            WSDL = NONE
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            WSDL = DEFAULT,
            SESSIONS = ENABLED,
            LOGIN_TYPE = MIXED
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            WSDL = 'sp_name',
            SESSIONS = DISABLED,
            LOGIN_TYPE = WINDOWS
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            SESSION_TIMEOUT = 10,
            DATABASE = 'db1',
            NAMESPACE = 'n1'
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            SESSION_TIMEOUT = NEVER,
            DATABASE = DEFAULT
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            NAMESPACE = DEFAULT,
            SCHEMA = NONE,
            CHARACTER_SET = SQL
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SOAP (
            SCHEMA = STANDARD,
            CHARACTER_SET = XML,
            HEADER_LIMIT = 1
             );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            AUTHENTICATION = WINDOWS
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            AUTHENTICATION = WINDOWS NTLM
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            AUTHENTICATION = WINDOWS KERBEROS CERTIFICATE c1
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            AUTHENTICATION = CERTIFICATE c1 WINDOWS
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            AUTHENTICATION = CERTIFICATE c1 WINDOWS NEGOTIATE
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            ENCRYPTION = DISABLED,
            MESSAGE_FORWARDING = DISABLED
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            ENCRYPTION = SUPPORTED ALGORITHM RC4
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            ENCRYPTION = SUPPORTED ALGORITHM RC4 AES
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            ENCRYPTION = REQUIRED ALGORITHM AES
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            ENCRYPTION = REQUIRED ALGORITHM AES RC4
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR SERVICE_BROKER (
            MESSAGE_FORWARDING = ENABLED,
            MESSAGE_FORWARD_SIZE = 1
                       );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR DATABASE_MIRRORING (
            AUTHENTICATION = WINDOWS,
            ROLE = WITNESS
                           );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR DATABASE_MIRRORING (
            ENCRYPTION = SUPPORTED,
            ROLE = PARTNER
                           );

CREATE ENDPOINT e1 
    AS TCP (
            LISTENER_PORT = 4022
           )
    FOR DATABASE_MIRRORING (
            ROLE = ALL
                           );