GRANT view definition control create alter (c1, c2, c3)
    ON APPLICATION ROLE::a.b..d TO PUBLIC, NULL, [user1], user2
    WITH GRANT OPTION
    AS c1;

GRANT all privileges
    ON ASSEMBLY::a.b..d (c1, c2) TO PUBLIC;

GRANT control
    ON ASYMMETRIC KEY::a.b..d TO NULL
    AS c1;

GRANT create control alter
    ON CERTIFICATE::a.b..d TO PUBLIC, NULL, [user1], user2;

GRANT create control alter
    ON [a] TO NULL, user2
    AS [all];

GRANT create
    ON CONTRACT::[c1] TO NULL
    WITH GRANT OPTION;

GRANT create TO NULL
    WITH GRANT OPTION
    AS [p1];

GRANT control (c1) TO PUBLIC
    AS [p1];

GRANT control
    ON t1 (c1) TO PUBLIC
    AS [p1];

GRANT all privileges TO PUBLIC
    AS [p1];

DENY view definition control create alter (c1, c2, c3)
    ON DATABASE::a.b..d TO PUBLIC, NULL, [user1], user2 CASCADE
    AS c1;

DENY all privileges
    ON ENDPOINT::a.b..d (c1, c2) TO PUBLIC;

DENY control
    ON FULLTEXT CATALOG::a.b..d TO NULL
    AS c1;

DENY create control alter
    ON LOGIN::a.b..d TO PUBLIC, NULL, [user1], user2;

DENY create control alter
    ON [a] TO NULL, user2
    AS [all];

DENY create
    ON MESSAGE TYPE::m1 TO NULL CASCADE;

DENY create TO NULL CASCADE
    AS [p1];

DENY control (c1) TO PUBLIC
    AS [p1];

DENY control
    ON t1 (c1) TO PUBLIC
    AS [p1];

DENY all privileges TO PUBLIC
    AS [p1];

REVOKE GRANT OPTION FOR view definition control create alter (c1, c2, c3)
    ON OBJECT::a.b..d TO PUBLIC, NULL, [user1], user2 CASCADE
    AS c1;

REVOKE view definition control create alter (c1, c2, c3)
    ON REMOTE SERVICE BINDING::a.b..d TO PUBLIC, NULL, [user1], user2 CASCADE
    AS c1;

REVOKE all privileges
    ON ROLE::a.b..d (c1, c2) TO PUBLIC;

REVOKE control
    ON ROUTE::a.b..d TO NULL
    AS c1;

REVOKE create control alter
    ON SCHEMA::a.b..d TO PUBLIC, NULL, [user1], user2;

REVOKE create control alter
    ON [a] TO NULL, user2
    AS [all];

REVOKE create
    ON SERVER::s1 TO NULL CASCADE;

REVOKE create TO NULL CASCADE
    AS [p1];

REVOKE control (c1) TO PUBLIC
    AS [p1];

REVOKE GRANT OPTION FOR control
    ON t1 (c1) TO PUBLIC
    AS [p1];

REVOKE all privileges TO PUBLIC
    AS [p1];

ALTER AUTHORIZATION
    ON OBJECT::Parts.Sprockets
    TO MichikoOsada;

ALTER AUTHORIZATION
    ON Parts.Sprockets
    TO [MichikoOsada];

ALTER AUTHORIZATION
    ON OBJECT::ProductionView06
    TO SCHEMA OWNER;

ALTER AUTHORIZATION
    ON SERVICE::..ProductionView06
    TO SCHEMA OWNER;

ALTER AUTHORIZATION
    ON SYMMETRIC KEY::Parts.Sprockets
    TO [c1];

ALTER AUTHORIZATION
    ON TYPE::Parts.Sprockets
    TO [c1];

ALTER AUTHORIZATION
    ON USER::Parts.Sprockets
    TO [c1];

ALTER AUTHORIZATION
    ON XML SCHEMA COLLECTION::Parts.Sprockets
    TO [c1];

ALTER AUTHORIZATION
    ON SEARCH PROPERTY LIST::list1
    TO [c1];

ALTER AUTHORIZATION
    ON AVAILABILITY GROUP::ag1
    TO [c1];

GRANT EXECUTE
    ON XML SCHEMA COLLECTION::[dm].[OrderCombined_v1r1m1] TO [db_spexecute];

GRANT control
    ON APPLICATION ROLE::app1 TO PUBLIC;

GRANT control
    ON ASYMMETRIC KEY::key1 TO PUBLIC;

GRANT control
    ON FULLTEXT CATALOG::cat1 TO PUBLIC;

GRANT control
    ON FULLTEXT STOPLIST::stoplist1 TO PUBLIC;

GRANT control
    ON MESSAGE TYPE::message1 TO PUBLIC;

GRANT control
    ON SYMMETRIC KEY::key1 TO PUBLIC;

GRANT control
    ON XML SCHEMA COLLECTION::xsc1 TO PUBLIC;

GRANT control
    ON REMOTE SERVICE BINDING::rsb1 TO PUBLIC;

GRANT control
    ON SEARCH PROPERTY LIST::list1 TO PUBLIC;

GRANT control
    ON AVAILABILITY GROUP::ag1 TO PUBLIC;