//------------------------------------------------------------------------------
// <copyright file="EventNotificationEventType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Possible event types
    /// </summary>    
    public enum EventNotificationEventType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// CREATE_TABLE.
        /// </summary>
        CreateTable = 21,

        /// <summary>
        /// ALTER_TABLE.
        /// </summary>
        AlterTable = 22,

        /// <summary>
        /// DROP_TABLE.
        /// </summary>
        DropTable = 23,

        /// <summary>
        /// CREATE_INDEX.
        /// </summary>
        CreateIndex = 24,

        /// <summary>
        /// ALTER_INDEX.
        /// </summary>
        AlterIndex = 25,

        /// <summary>
        /// DROP_INDEX.
        /// </summary>
        DropIndex = 26,

        /// <summary>
        /// CREATE_STATISTICS.
        /// </summary>
        CreateStatistics = 27,

        /// <summary>
        /// UPDATE_STATISTICS.
        /// </summary>
        UpdateStatistics = 28,

        /// <summary>
        /// DROP_STATISTICS.
        /// </summary>
        DropStatistics = 29,

        /// <summary>
        /// CREATE_SYNONYM.
        /// </summary>
        CreateSynonym = 34,

        /// <summary>
        /// DROP_SYNONYM.
        /// </summary>
        DropSynonym = 36,

        /// <summary>
        /// CREATE_VIEW.
        /// </summary>
        CreateView = 41,

        /// <summary>
        /// ALTER_VIEW.
        /// </summary>
        AlterView = 42,

        /// <summary>
        /// DROP_VIEW.
        /// </summary>
        DropView = 43,

        /// <summary>
        /// CREATE_PROCEDURE.
        /// </summary>
        CreateProcedure = 51,

        /// <summary>
        /// ALTER_PROCEDURE.
        /// </summary>
        AlterProcedure = 52,

        /// <summary>
        /// DROP_PROCEDURE.
        /// </summary>
        DropProcedure = 53,

        /// <summary>
        /// CREATE_FUNCTION.
        /// </summary>
        CreateFunction = 61,

        /// <summary>
        /// ALTER_FUNCTION.
        /// </summary>
        AlterFunction = 62,

        /// <summary>
        /// DROP_FUNCTION.
        /// </summary>
        DropFunction = 63,

        /// <summary>
        /// CREATE_TRIGGER.
        /// </summary>
        CreateTrigger = 71,

        /// <summary>
        /// ALTER_TRIGGER.
        /// </summary>
        AlterTrigger = 72,

        /// <summary>
        /// DROP_TRIGGER.
        /// </summary>
        DropTrigger = 73,

        /// <summary>
        /// CREATE_EVENT_NOTIFICATION.
        /// </summary>
        CreateEventNotification = 74,

        /// <summary>
        /// DROP_EVENT_NOTIFICATION.
        /// </summary>
        DropEventNotification = 76,

        /// <summary>
        /// CREATE_TYPE.
        /// </summary>
        CreateType = 91,

        /// <summary>
        /// DROP_TYPE.
        /// </summary>
        DropType = 93,

        /// <summary>
        /// CREATE_ASSEMBLY.
        /// </summary>
        CreateAssembly = 101,

        /// <summary>
        /// ALTER_ASSEMBLY.
        /// </summary>
        AlterAssembly = 102,

        /// <summary>
        /// DROP_ASSEMBLY.
        /// </summary>
        DropAssembly = 103,

        /// <summary>
        /// CREATE_USER.
        /// </summary>
        CreateUser = 131,

        /// <summary>
        /// ALTER_USER.
        /// </summary>
        AlterUser = 132,

        /// <summary>
        /// DROP_USER.
        /// </summary>
        DropUser = 133,

        /// <summary>
        /// CREATE_ROLE.
        /// </summary>
        CreateRole = 134,

        /// <summary>
        /// ALTER_ROLE.
        /// </summary>
        AlterRole = 135,

        /// <summary>
        /// DROP_ROLE.
        /// </summary>
        DropRole = 136,

        /// <summary>
        /// CREATE_APPLICATION_ROLE.
        /// </summary>
        CreateApplicationRole = 137,

        /// <summary>
        /// ALTER_APPLICATION_ROLE.
        /// </summary>
        AlterApplicationRole = 138,

        /// <summary>
        /// DROP_APPLICATION_ROLE.
        /// </summary>
        DropApplicationRole = 139,

        /// <summary>
        /// CREATE_SCHEMA.
        /// </summary>
        CreateSchema = 141,

        /// <summary>
        /// ALTER_SCHEMA.
        /// </summary>
        AlterSchema = 142,

        /// <summary>
        /// DROP_SCHEMA.
        /// </summary>
        DropSchema = 143,

        /// <summary>
        /// CREATE_LOGIN.
        /// </summary>
        CreateLogin = 144,

        /// <summary>
        /// ALTER_LOGIN.
        /// </summary>
        AlterLogin = 145,

        /// <summary>
        /// DROP_LOGIN.
        /// </summary>
        DropLogin = 146,

        /// <summary>
        /// CREATE_MESSAGE_TYPE.
        /// </summary>
        CreateMessageType = 151,

        /// <summary>
        /// ALTER_MESSAGE_TYPE.
        /// </summary>
        AlterMessageType = 152,

        /// <summary>
        /// DROP_MESSAGE_TYPE.
        /// </summary>
        DropMessageType = 153,

        /// <summary>
        /// CREATE_CONTRACT.
        /// </summary>
        CreateContract = 154,

        /// <summary>
        /// DROP_CONTRACT.
        /// </summary>
        DropContract = 156,

        /// <summary>
        /// CREATE_QUEUE.
        /// </summary>
        CreateQueue = 157,

        /// <summary>
        /// ALTER_QUEUE.
        /// </summary>
        AlterQueue = 158,

        /// <summary>
        /// DROP_QUEUE.
        /// </summary>
        DropQueue = 159,

        /// <summary>
        /// BROKER_QUEUE_DISABLED.
        /// </summary>
        BrokerQueueDisabled = 160,

        /// <summary>
        /// CREATE_SERVICE.
        /// </summary>
        CreateService = 161,

        /// <summary>
        /// ALTER_SERVICE.
        /// </summary>
        AlterService = 162,

        /// <summary>
        /// DROP_SERVICE.
        /// </summary>
        DropService = 163,

        /// <summary>
        /// CREATE_ROUTE.
        /// </summary>
        CreateRoute = 164,

        /// <summary>
        /// ALTER_ROUTE.
        /// </summary>
        AlterRoute = 165,

        /// <summary>
        /// DROP_ROUTE.
        /// </summary>
        DropRoute = 166,

        /// <summary>
        /// GRANT_SERVER.
        /// </summary>
        GrantServer = 167,

        /// <summary>
        /// DENY_SERVER.
        /// </summary>
        DenyServer = 168,

        /// <summary>
        /// REVOKE_SERVER.
        /// </summary>
        RevokeServer = 169,

        /// <summary>
        /// GRANT_DATABASE.
        /// </summary>
        GrantDatabase = 170,

        /// <summary>
        /// DENY_DATABASE.
        /// </summary>
        DenyDatabase = 171,

        /// <summary>
        /// REVOKE_DATABASE.
        /// </summary>
        RevokeDatabase = 172,

        /// <summary>
        /// QUEUE_ACTIVATION.
        /// </summary>
        QueueActivation = 173,

        /// <summary>
        /// CREATE_REMOTE_SERVICE_BINDING.
        /// </summary>
        CreateRemoteServiceBinding = 174,

        /// <summary>
        /// ALTER_REMOTE_SERVICE_BINDING.
        /// </summary>
        AlterRemoteServiceBinding = 175,

        /// <summary>
        /// DROP_REMOTE_SERVICE_BINDING.
        /// </summary>
        DropRemoteServiceBinding = 176,

        /// <summary>
        /// CREATE_XML_SCHEMA_COLLECTION.
        /// </summary>
        CreateXmlSchemaCollection = 177,

        /// <summary>
        /// ALTER_XML_SCHEMA_COLLECTION.
        /// </summary>
        AlterXmlSchemaCollection = 178,

        /// <summary>
        /// DROP_XML_SCHEMA_COLLECTION.
        /// </summary>
        DropXmlSchemaCollection = 179,

        /// <summary>
        /// CREATE_ENDPOINT.
        /// </summary>
        CreateEndpoint = 181,

        /// <summary>
        /// ALTER_ENDPOINT.
        /// </summary>
        AlterEndpoint = 182,

        /// <summary>
        /// DROP_ENDPOINT.
        /// </summary>
        DropEndpoint = 183,

        /// <summary>
        /// CREATE_PARTITION_FUNCTION.
        /// </summary>
        CreatePartitionFunction = 191,

        /// <summary>
        /// ALTER_PARTITION_FUNCTION.
        /// </summary>
        AlterPartitionFunction = 192,

        /// <summary>
        /// DROP_PARTITION_FUNCTION.
        /// </summary>
        DropPartitionFunction = 193,

        /// <summary>
        /// CREATE_PARTITION_SCHEME.
        /// </summary>
        CreatePartitionScheme = 194,

        /// <summary>
        /// ALTER_PARTITION_SCHEME.
        /// </summary>
        AlterPartitionScheme = 195,

        /// <summary>
        /// DROP_PARTITION_SCHEME.
        /// </summary>
        DropPartitionScheme = 196,

        /// <summary>
        /// CREATE_CERTIFICATE.
        /// </summary>
        CreateCertificate = 197,

        /// <summary>
        /// ALTER_CERTIFICATE.
        /// </summary>
        AlterCertificate = 198,

        /// <summary>
        /// DROP_CERTIFICATE.
        /// </summary>
        DropCertificate = 199,

        /// <summary>
        /// CREATE_DATABASE.
        /// </summary>
        CreateDatabase = 201,

        /// <summary>
        /// ALTER_DATABASE.
        /// </summary>
        AlterDatabase = 202,

        /// <summary>
        /// DROP_DATABASE.
        /// </summary>
        DropDatabase = 203,

        /// <summary>
        /// ALTER_AUTHORIZATION_SERVER.
        /// </summary>
        AlterAuthorizationServer = 204,

        /// <summary>
        /// ALTER_AUTHORIZATION_DATABASE.
        /// </summary>
        AlterAuthorizationDatabase = 205,

        /// <summary>
        /// CREATE_XML_INDEX.
        /// </summary>
        CreateXmlIndex = 206,

        /// <summary>
        /// ADD_ROLE_MEMBER.
        /// </summary>
        AddRoleMember = 207,

        /// <summary>
        /// DROP_ROLE_MEMBER.
        /// </summary>
        DropRoleMember = 208,

        /// <summary>
        /// ADD_SERVER_ROLE_MEMBER.
        /// </summary>
        AddServerRoleMember = 209,

        /// <summary>
        /// DROP_SERVER_ROLE_MEMBER.
        /// </summary>
        DropServerRoleMember = 210,

        /// <summary>
        /// ALTER_EXTENDED_PROPERTY.
        /// </summary>
        AlterExtendedProperty = 211,

        /// <summary>
        /// ALTER_FULLTEXT_CATALOG.
        /// </summary>
        AlterFullTextCatalog = 212,

        /// <summary>
        /// ALTER_FULLTEXT_INDEX.
        /// </summary>
        AlterFullTextIndex = 213,

        /// <summary>
        /// ALTER_INSTANCE.
        /// </summary>
        AlterInstance = 214,

        /// <summary>
        /// ALTER_MESSAGE.
        /// </summary>
        AlterMessage = 215,

        /// <summary>
        /// ALTER_PLAN_GUIDE.
        /// </summary>
        AlterPlanGuide = 216,

        /// <summary>
        /// ALTER_REMOTE_SERVER.
        /// </summary>
        AlterRemoteServer = 217,

        /// <summary>
        /// BIND_DEFAULT.
        /// </summary>
        BindDefault = 218,

        /// <summary>
        /// BIND_RULE.
        /// </summary>
        BindRule = 219,

        /// <summary>
        /// CREATE_DEFAULT.
        /// </summary>
        CreateDefault = 220,

        /// <summary>
        /// CREATE_EXTENDED_PROCEDURE.
        /// </summary>
        CreateExtendedProcedure = 221,

        /// <summary>
        /// CREATE_EXTENDED_PROPERTY.
        /// </summary>
        CreateExtendedProperty = 222,

        /// <summary>
        /// CREATE_FULLTEXT_CATALOG.
        /// </summary>
        CreateFullTextCatalog = 223,

        /// <summary>
        /// CREATE_FULLTEXT_INDEX.
        /// </summary>
        CreateFullTextIndex = 224,

        /// <summary>
        /// CREATE_LINKED_SERVER.
        /// </summary>
        CreateLinkedServer = 225,

        /// <summary>
        /// CREATE_LINKED_SERVER_LOGIN.
        /// </summary>
        CreateLinkedServerLogin = 226,

        /// <summary>
        /// CREATE_MESSAGE.
        /// </summary>
        CreateMessage = 227,

        /// <summary>
        /// CREATE_PLAN_GUIDE.
        /// </summary>
        CreatePlanGuide = 228,

        /// <summary>
        /// CREATE_RULE.
        /// </summary>
        CreateRule = 229,

        /// <summary>
        /// CREATE_REMOTE_SERVER.
        /// </summary>
        CreateRemoteServer = 230,

        /// <summary>
        /// DROP_DEFAULT.
        /// </summary>
        DropDefault = 231,

        /// <summary>
        /// DROP_EXTENDED_PROCEDURE.
        /// </summary>
        DropExtendedProcedure = 232,

        /// <summary>
        /// DROP_EXTENDED_PROPERTY.
        /// </summary>
        DropExtendedProperty = 233,

        /// <summary>
        /// DROP_FULLTEXT_CATALOG.
        /// </summary>
        DropFullTextCatalog = 234,

        /// <summary>
        /// DROP_FULLTEXT_INDEX.
        /// </summary>
        DropFullTextIndex = 235,

        /// <summary>
        /// DROP_LINKED_SERVER_LOGIN.
        /// </summary>
        DropLinkedServerLogin = 236,

        /// <summary>
        /// DROP_MESSAGE.
        /// </summary>
        DropMessage = 237,

        /// <summary>
        /// DROP_PLAN_GUIDE.
        /// </summary>
        DropPlanGuide = 238,

        /// <summary>
        /// DROP_RULE.
        /// </summary>
        DropRule = 239,

        /// <summary>
        /// DROP_REMOTE_SERVER.
        /// </summary>
        DropRemoteServer = 240,

        /// <summary>
        /// RENAME.
        /// </summary>
        Rename = 241,

        /// <summary>
        /// UNBIND_DEFAULT.
        /// </summary>
        UnbindDefault = 242,

        /// <summary>
        /// UNBIND_RULE.
        /// </summary>
        UnbindRule = 243,

        /// <summary>
        /// CREATE_SYMMETRIC_KEY.
        /// </summary>
        CreateSymmetricKey = 244,

        /// <summary>
        /// ALTER_SYMMETRIC_KEY.
        /// </summary>
        AlterSymmetricKey = 245,

        /// <summary>
        /// DROP_SYMMETRIC_KEY.
        /// </summary>
        DropSymmetricKey = 246,

        /// <summary>
        /// CREATE_ASYMMETRIC_KEY.
        /// </summary>
        CreateAsymmetricKey = 247,

        /// <summary>
        /// ALTER_ASYMMETRIC_KEY.
        /// </summary>
        AlterAsymmetricKey = 248,

        /// <summary>
        /// DROP_ASYMMETRIC_KEY.
        /// </summary>
        DropAsymmetricKey = 249,

        /// <summary>
        /// ALTER_SERVICE_MASTER_KEY.
        /// </summary>
        AlterServiceMasterKey = 251,

        /// <summary>
        /// CREATE_MASTER_KEY.
        /// </summary>
        CreateMasterKey = 252,

        /// <summary>
        /// ALTER_MASTER_KEY.
        /// </summary>
        AlterMasterKey = 253,

        /// <summary>
        /// DROP_MASTER_KEY.
        /// </summary>
        DropMasterKey = 254,

        /// <summary>
        /// ADD_SIGNATURE_SCHEMA_OBJECT.
        /// </summary>
        AddSignatureSchemaObject = 255,

        /// <summary>
        /// DROP_SIGNATURE_SCHEMA_OBJECT.
        /// </summary>
        DropSignatureSchemaObject = 256,

        /// <summary>
        /// ADD_SIGNATURE.
        /// </summary>
        AddSignature = 257,

        /// <summary>
        /// DROP_SIGNATURE.
        /// </summary>
        DropSignature = 258,

        /// <summary>
        /// CREATE_CREDENTIAL.
        /// </summary>
        CreateCredential = 259,

        /// <summary>
        /// ALTER_CREDENTIAL.
        /// </summary>
        AlterCredential = 260,

        /// <summary>
        /// DROP_CREDENTIAL.
        /// </summary>
        DropCredential = 261,

        /// <summary>
        /// DROP_LINKED_SERVER.
        /// </summary>
        DropLinkedServer = 262,

        /// <summary>
        /// ALTER_LINKED_SERVER.
        /// </summary>
        AlterLinkedServer = 263,

        /// <summary>
        /// CREATE_EVENT_SESSION.
        /// </summary>
        CreateEventSession = 264,

        /// <summary>
        /// ALTER_EVENT_SESSION.
        /// </summary>
        AlterEventSession = 265,

        /// <summary>
        /// DROP_EVENT_SESSION.
        /// </summary>
        DropEventSession = 266,

        /// <summary>
        /// CREATE_RESOURCE_POOL.
        /// </summary>
        CreateResourcePool = 267,

        /// <summary>
        /// ALTER_RESOURCE_POOL.
        /// </summary>
        AlterResourcePool = 268,

        /// <summary>
        /// DROP_RESOURCE_POOL.
        /// </summary>
        DropResourcePool = 269,

        /// <summary>
        /// CREATE_WORKLOAD_GROUP.
        /// </summary>
        CreateWorkloadGroup = 270,

        /// <summary>
        /// ALTER_WORKLOAD_GROUP.
        /// </summary>
        AlterWorkloadGroup = 271,

        /// <summary>
        /// DROP_WORKLOAD_GROUP.
        /// </summary>
        DropWorkloadGroup = 272,

        /// <summary>
        /// ALTER_RESOURCE_GOVERNOR_CONFIG.
        /// </summary>
        AlterResourceGovernorConfig = 273,

        /// <summary>
        /// CREATE_SPATIAL_INDEX.
        /// </summary>
        CreateSpatialIndex = 274,

        /// <summary>
        /// CREATE_CRYPTOGRAPHIC_PROVIDER.
        /// </summary>
        CreateCryptographicProvider = 275,

        /// <summary>
        /// ALTER_CRYPTOGRAPHIC_PROVIDER.
        /// </summary>
        AlterCryptographicProvider = 276,

        /// <summary>
        /// DROP_CRYPTOGRAPHIC_PROVIDER.
        /// </summary>
        DropCryptographicProvider = 277,

        /// <summary>
        /// CREATE_DATABASE_ENCRYPTION_KEY.
        /// </summary>
        CreateDatabaseEncryptionKey = 278,

        /// <summary>
        /// ALTER_DATABASE_ENCRYPTION_KEY.
        /// </summary>
        AlterDatabaseEncryptionKey = 279,

        /// <summary>
        /// DROP_DATABASE_ENCRYPTION_KEY.
        /// </summary>
        DropDatabaseEncryptionKey = 280,

        /// <summary>
        /// CREATE_BROKER_PRIORITY.
        /// </summary>
        CreateBrokerPriority = 281,

        /// <summary>
        /// ALTER_BROKER_PRIORITY.
        /// </summary>
        AlterBrokerPriority = 282,

        /// <summary>
        /// DROP_BROKER_PRIORITY.
        /// </summary>
        DropBrokerPriority = 283,

        /// <summary>
        /// CREATE_SERVER_AUDIT.
        /// </summary>
        CreateServerAudit = 284,

        /// <summary>
        /// ALTER_SERVER_AUDIT.
        /// </summary>
        AlterServerAudit = 285,

        /// <summary>
        /// DROP_SERVER_AUDIT.
        /// </summary>
        DropServerAudit = 286,

        /// <summary>
        /// CREATE_SERVER_AUDIT_SPECIFICATION.
        /// </summary>
        CreateServerAuditSpecification = 287,

        /// <summary>
        /// ALTER_SERVER_AUDIT_SPECIFICATION.
        /// </summary>
        AlterServerAuditSpecification = 288,

        /// <summary>
        /// DROP_SERVER_AUDIT_SPECIFICATION.
        /// </summary>
        DropServerAuditSpecification = 289,

        /// <summary>
        /// CREATE_DATABASE_AUDIT_SPECIFICATION.
        /// </summary>
        CreateDatabaseAuditSpecification = 290,

        /// <summary>
        /// ALTER_DATABASE_AUDIT_SPECIFICATION.
        /// </summary>
        AlterDatabaseAuditSpecification = 291,

        /// <summary>
        /// DROP_DATABASE_AUDIT_SPECIFICATION.
        /// </summary>
        DropDatabaseAuditSpecification = 292,

        /// <summary>
        /// CREATE_FULLTEXT_STOPLIST.
        /// </summary>
        CreateFullTextStopList = 293,

        /// <summary>
        /// ALTER_FULLTEXT_STOPLIST.
        /// </summary>
        AlterFullTextStopList = 294,

        /// <summary>
        /// DROP_FULLTEXT_STOPLIST.
        /// </summary>
        DropFullTextStopList = 295,

        /// <summary>
        /// ALTER_SERVER_CONFIGURATION.
        /// </summary>
        AlterServerConfiguration = 296,

        /// <summary>
        /// CREATE_SEARCH_PROPERTY_LIST.
        /// </summary>
        CreateSearchPropertyList = 297,

        /// <summary>
        /// ALTER_SEARCH_PROPERTY_LIST.
        /// </summary>
        AlterSearchPropertyList = 298,

        /// <summary>
        /// DROP_SEARCH_PROPERTY_LIST.
        /// </summary>
        DropSearchPropertyList = 299,

        /// <summary>
        /// CREATE_SERVER_ROLE.
        /// </summary>
        CreateServerRole = 300,

        /// <summary>
        /// ALTER_SERVER_ROLE.
        /// </summary>
        AlterServerRole = 301,

        /// <summary>
        /// DROP_SERVER_ROLE.
        /// </summary>
        DropServerRole = 302,

        /// <summary>
        /// CREATE_SEQUENCE.
        /// </summary>
        CreateSequence = 303,

        /// <summary>
        /// ALTER_SEQUENCE.
        /// </summary>
        AlterSequence = 304,

        /// <summary>
        /// DROP_SEQUENCE.
        /// </summary>
        DropSequence = 305,

        /// <summary>
        /// CREATE_AVAILABILITY_GROUP.
        /// </summary>
        CreateAvailabilityGroup = 306,

        /// <summary>
        /// ALTER_AVAILABILITY_GROUP.
        /// </summary>
        AlterAvailabilityGroup = 307,

        /// <summary>
        /// DROP_AVAILABILITY_GROUP.
        /// </summary>
        DropAvailabilityGroup = 308,

        /// <summary>
        /// CREATE_AUDIT.
        /// </summary>
        CreateDatabaseAudit = 309,

        /// <summary>
        /// DROP_AUDIT.
        /// </summary>
        DropDatabaseAudit = 310,

        /// <summary>
        /// ALTER_AUDIT.
        /// </summary>
        AlterDatabaseAudit = 311,

        /// <summary>
        /// CREATE SECURITY POLICY
        /// </summary>
        CreateSecurityPolicy = 312,

        /// <summary>
        /// ALTER SECURITY POLICY
        /// </summary>
        AlterSecurityPolicy = 313,

        /// <summary>
        /// DROP SECURITY POLICY
        /// </summary>
        DropSecurityPolicy = 314,

        /// <summary>
        /// CREATE_COLUMN_MASTER_KEY
        /// </summary>
        CreateColumnMasterKey = 315,

        /// <summary>
        /// DROP_COLUMN_MASTER_KEY
        /// </summary>
        DropColumnMasterKey = 316,

        /// <summary>
        /// CREATE_COLUMN_ENCRYPTION_KEY
        /// </summary>
        CreateColumnEncryptionKey = 317,

        /// <summary>
        /// ALTER_COLUMN_ENCRYPTION_KEY
        /// </summary>
        AlterColumnEncryptionKey = 318,

        /// <summary>
        /// DROP_COLUMN_ENCRYPTION_KEY
        /// </summary>
        DropColumnEncryptionKey = 319,

        /// <summary>
        /// ALTER_DATABASE_SCOPED_CONFIGURATION
        /// </summary>
        AlterDatabaseScopedConfiguration = 320,

        /// <summary>
        /// CREATE_EXTERNAL_RESOURCE_POOL
        /// </summary>
        CreateExternalResourcePool = 321,

        /// <summary>
        /// ALTER_EXTERNAL_RESOURCE_POOL
        /// </summary>
        AlterExternalResourcePool = 322,

        /// <summary>
        /// DROP_EXTERNAL_RESOURCE_POOL
        /// </summary>
        DropExternalResourcePool = 323,

        /// <summary>
        /// CREATE_EXTERNAL_LIBRARY
        /// </summary>
        CreateExternalLibrary = 324,

        /// <summary>
        /// ALTER_EXTERNAL_LIBRARY
        /// </summary>
        AlterExternalLibrary = 325,

        /// <summary>
        /// DROP_EXTERNAL_LIBRARY
        /// </summary>
        DropExternalLibrary = 326,

        /// <summary>
        /// ADD_SENSITIVITY_CLASSIFICATION
        /// </summary>
        AddSensitivityClassification = 327,

        /// <summary>
        /// DROP_SENSITIVITY_CLASSIFICATION
        /// </summary>
        DropSensitivityClassification = 328,

        /// <summary>
        /// CREATE_EXTERNAL_LANGUAGE
        /// </summary>
        CreateExternalLanguage = 329,

        /// <summary>
        /// ALTER_EXTERNAL_LANGUAGE
        /// </summary>
        AlterExternalLanguage = 330,

        /// <summary>
        /// DROP_EXTERNAL_LANGUAGE
        /// </summary>
        DropExternalLanguage = 331,

        /// <summary>
        /// CREATE_JSON_INDEX
        /// </summary>
        CreateJsonIndex = 343,

        /// <summary>
        /// CREATE_VECTOR_INDEX
        /// </summary>
        CreateVectorIndex = 344,

        /// <summary>
        /// ADD_INFORMATION_PROTECTION
        /// </summary>
        AddInformationProtection = 345,

        /// <summary>
        /// DROP_INFORMATION_PROTECTION
        /// </summary>
        DropInformationProtection = 346,

        /// <summary>
        /// AUDIT_LOGIN.
        /// </summary>
        AuditLogin = 1014,

        /// <summary>
        /// AUDIT_LOGOUT.
        /// </summary>
        AuditLogout = 1015,

        /// <summary>
        /// AUDIT_LOGIN_FAILED.
        /// </summary>
        AuditLoginFailed = 1020,

        /// <summary>
        /// EVENTLOG.
        /// </summary>
        EventLog = 1021,

        /// <summary>
        /// ERRORLOG.
        /// </summary>
        ErrorLog = 1022,

        /// <summary>
        /// LOCK_DEADLOCK.
        /// </summary>
        LockDeadlock = 1025,

        /// <summary>
        /// EXCEPTION.
        /// </summary>
        Exception = 1033,

        /// <summary>
        /// SP_CACHEMISS.
        /// </summary>
        SpCacheMiss = 1034,

        /// <summary>
        /// SP_CACHEINSERT.
        /// </summary>
        SpCacheInsert = 1035,

        /// <summary>
        /// SP_CACHEREMOVE.
        /// </summary>
        SpCacheRemove = 1036,

        /// <summary>
        /// SP_RECOMPILE.
        /// </summary>
        SpRecompile = 1037,

        /// <summary>
        /// OBJECT_CREATED.
        /// </summary>
        ObjectCreated = 1046,

        /// <summary>
        /// OBJECT_DELETED.
        /// </summary>
        ObjectDeleted = 1047,

        /// <summary>
        /// HASH_WARNING.
        /// </summary>
        HashWarning = 1055,

        /// <summary>
        /// LOCK_DEADLOCK_CHAIN.
        /// </summary>
        LockDeadlockChain = 1059,

        /// <summary>
        /// LOCK_ESCALATION.
        /// </summary>
        LockEscalation = 1060,

        /// <summary>
        /// OLEDB_ERRORS.
        /// </summary>
        OledbErrors = 1061,

        /// <summary>
        /// EXECUTION_WARNINGS.
        /// </summary>
        ExecutionWarnings = 1067,

        /// <summary>
        /// SORT_WARNINGS.
        /// </summary>
        SortWarnings = 1069,

        /// <summary>
        /// MISSING_COLUMN_STATISTICS.
        /// </summary>
        MissingColumnStatistics = 1079,

        /// <summary>
        /// MISSING_JOIN_PREDICATE.
        /// </summary>
        MissingJoinPredicate = 1080,

        /// <summary>
        /// SERVER_MEMORY_CHANGE.
        /// </summary>
        ServerMemoryChange = 1081,

        /// <summary>
        /// USERCONFIGURABLE_0.
        /// </summary>
        UserConfigurable0 = 1082,

        /// <summary>
        /// USERCONFIGURABLE_1.
        /// </summary>
        UserConfigurable1 = 1083,

        /// <summary>
        /// USERCONFIGURABLE_2.
        /// </summary>
        UserConfigurable2 = 1084,

        /// <summary>
        /// USERCONFIGURABLE_3.
        /// </summary>
        UserConfigurable3 = 1085,

        /// <summary>
        /// USERCONFIGURABLE_4.
        /// </summary>
        UserConfigurable4 = 1086,

        /// <summary>
        /// USERCONFIGURABLE_5.
        /// </summary>
        UserConfigurable5 = 1087,

        /// <summary>
        /// USERCONFIGURABLE_6.
        /// </summary>
        UserConfigurable6 = 1088,

        /// <summary>
        /// USERCONFIGURABLE_7.
        /// </summary>
        UserConfigurable7 = 1089,

        /// <summary>
        /// USERCONFIGURABLE_8.
        /// </summary>
        UserConfigurable8 = 1090,

        /// <summary>
        /// USERCONFIGURABLE_9.
        /// </summary>
        UserConfigurable9 = 1091,

        /// <summary>
        /// DATA_FILE_AUTO_GROW.
        /// </summary>
        DataFileAutoGrow = 1092,

        /// <summary>
        /// LOG_FILE_AUTO_GROW.
        /// </summary>
        LogFileAutoGrow = 1093,

        /// <summary>
        /// DATA_FILE_AUTO_SHRINK.
        /// </summary>
        DataFileAutoShrink = 1094,

        /// <summary>
        /// LOG_FILE_AUTO_SHRINK.
        /// </summary>
        LogFileAutoShrink = 1095,

        /// <summary>
        /// AUDIT_DATABASE_SCOPE_GDR_EVENT.
        /// </summary>
        AuditDatabaseScopeGdrEvent = 1102,

        /// <summary>
        /// AUDIT_SCHEMA_OBJECT_GDR_EVENT.
        /// </summary>
        AuditSchemaObjectGdrEvent = 1103,

        /// <summary>
        /// AUDIT_ADDLOGIN_EVENT.
        /// </summary>
        AuditAddLoginEvent = 1104,

        /// <summary>
        /// AUDIT_LOGIN_GDR_EVENT.
        /// </summary>
        AuditLoginGdrEvent = 1105,

        /// <summary>
        /// AUDIT_LOGIN_CHANGE_PROPERTY_EVENT.
        /// </summary>
        AuditLoginChangePropertyEvent = 1106,

        /// <summary>
        /// AUDIT_LOGIN_CHANGE_PASSWORD_EVENT.
        /// </summary>
        AuditLoginChangePasswordEvent = 1107,

        /// <summary>
        /// AUDIT_ADD_LOGIN_TO_SERVER_ROLE_EVENT.
        /// </summary>
        AuditAddLoginToServerRoleEvent = 1108,

        /// <summary>
        /// AUDIT_ADD_DB_USER_EVENT.
        /// </summary>
        AuditAddDBUserEvent = 1109,

        /// <summary>
        /// AUDIT_ADD_MEMBER_TO_DB_ROLE_EVENT.
        /// </summary>
        AuditAddMemberToDBRoleEvent = 1110,

        /// <summary>
        /// AUDIT_ADD_ROLE_EVENT.
        /// </summary>
        AuditAddRoleEvent = 1111,

        /// <summary>
        /// AUDIT_APP_ROLE_CHANGE_PASSWORD_EVENT.
        /// </summary>
        AuditAppRoleChangePasswordEvent = 1112,

        /// <summary>
        /// AUDIT_SCHEMA_OBJECT_ACCESS_EVENT.
        /// </summary>
        AuditSchemaObjectAccessEvent = 1114,

        /// <summary>
        /// AUDIT_BACKUP_RESTORE_EVENT.
        /// </summary>
        AuditBackupRestoreEvent = 1115,

        /// <summary>
        /// AUDIT_DBCC_EVENT.
        /// </summary>
        AuditDbccEvent = 1116,

        /// <summary>
        /// AUDIT_CHANGE_AUDIT_EVENT.
        /// </summary>
        AuditChangeAuditEvent = 1117,

        /// <summary>
        /// OLEDB_CALL_EVENT.
        /// </summary>
        OledbCallEvent = 1119,

        /// <summary>
        /// OLEDB_QUERYINTERFACE_EVENT.
        /// </summary>
        OledbQueryInterfaceEvent = 1120,

        /// <summary>
        /// OLEDB_DATAREAD_EVENT.
        /// </summary>
        OledbDataReadEvent = 1121,

        /// <summary>
        /// SHOWPLAN_XML.
        /// </summary>
        ShowPlanXml = 1122,

        /// <summary>
        /// DEPRECATION_ANNOUNCEMENT.
        /// </summary>
        DeprecationAnnouncement = 1125,

        /// <summary>
        /// DEPRECATION_FINAL_SUPPORT.
        /// </summary>
        DeprecationFinalSupport = 1126,

        /// <summary>
        /// EXCHANGE_SPILL_EVENT.
        /// </summary>
        ExchangeSpillEvent = 1127,

        /// <summary>
        /// AUDIT_DATABASE_MANAGEMENT_EVENT.
        /// </summary>
        AuditDatabaseManagementEvent = 1128,

        /// <summary>
        /// AUDIT_DATABASE_OBJECT_MANAGEMENT_EVENT.
        /// </summary>
        AuditDatabaseObjectManagementEvent = 1129,

        /// <summary>
        /// AUDIT_DATABASE_PRINCIPAL_MANAGEMENT_EVENT.
        /// </summary>
        AuditDatabasePrincipalManagementEvent = 1130,

        /// <summary>
        /// AUDIT_SCHEMA_OBJECT_MANAGEMENT_EVENT.
        /// </summary>
        AuditSchemaObjectManagementEvent = 1131,

        /// <summary>
        /// AUDIT_SERVER_PRINCIPAL_IMPERSONATION_EVENT.
        /// </summary>
        AuditServerPrincipalImpersonationEvent = 1132,

        /// <summary>
        /// AUDIT_DATABASE_PRINCIPAL_IMPERSONATION_EVENT.
        /// </summary>
        AuditDatabasePrincipalImpersonationEvent = 1133,

        /// <summary>
        /// AUDIT_SERVER_OBJECT_TAKE_OWNERSHIP_EVENT.
        /// </summary>
        AuditServerObjectTakeOwnershipEvent = 1134,

        /// <summary>
        /// AUDIT_DATABASE_OBJECT_TAKE_OWNERSHIP_EVENT.
        /// </summary>
        AuditDatabaseObjectTakeOwnershipEvent = 1135,

        /// <summary>
        /// BLOCKED_PROCESS_REPORT.
        /// </summary>
        BlockedProcessReport = 1137,

        /// <summary>
        /// SHOWPLAN_XML_STATISTICS_PROFILE.
        /// </summary>
        ShowPlanXmlStatisticsProfile = 1146,

        /// <summary>
        /// DEADLOCK_GRAPH.
        /// </summary>
        DeadlockGraph = 1148,

        /// <summary>
        /// TRACE_FILE_CLOSE.
        /// </summary>
        TraceFileClose = 1150,

        /// <summary>
        /// AUDIT_CHANGE_DATABASE_OWNER.
        /// </summary>
        AuditChangeDatabaseOwner = 1152,

        /// <summary>
        /// AUDIT_SCHEMA_OBJECT_TAKE_OWNERSHIP_EVENT.
        /// </summary>
        AuditSchemaObjectTakeOwnershipEvent = 1153,

        /// <summary>
        /// FT_CRAWL_STARTED.
        /// </summary>
        FtCrawlStarted = 1155,

        /// <summary>
        /// FT_CRAWL_STOPPED.
        /// </summary>
        FtCrawlStopped = 1156,

        /// <summary>
        /// FT_CRAWL_ABORTED.
        /// </summary>
        FtCrawlAborted = 1157,

        /// <summary>
        /// USER_ERROR_MESSAGE.
        /// </summary>
        UserErrorMessage = 1162,

        /// <summary>
        /// OBJECT_ALTERED.
        /// </summary>
        ObjectAltered = 1164,

        /// <summary>
        /// SQL_STMTRECOMPILE.
        /// </summary>
        SqlStmtRecompile = 1166,

        /// <summary>
        /// DATABASE_MIRRORING_STATE_CHANGE.
        /// </summary>
        DatabaseMirroringStateChange = 1167,

        /// <summary>
        /// SHOWPLAN_XML_FOR_QUERY_COMPILE.
        /// </summary>
        ShowPlanXmlForQueryCompile = 1168,

        /// <summary>
        /// SHOWPLAN_ALL_FOR_QUERY_COMPILE.
        /// </summary>
        ShowPlanAllForQueryCompile = 1169,

        /// <summary>
        /// AUDIT_SERVER_SCOPE_GDR_EVENT.
        /// </summary>
        AuditServerScopeGdrEvent = 1170,

        /// <summary>
        /// AUDIT_SERVER_OBJECT_GDR_EVENT.
        /// </summary>
        AuditServerObjectGdrEvent = 1171,

        /// <summary>
        /// AUDIT_DATABASE_OBJECT_GDR_EVENT.
        /// </summary>
        AuditDatabaseObjectGdrEvent = 1172,

        /// <summary>
        /// AUDIT_SERVER_OPERATION_EVENT.
        /// </summary>
        AuditServerOperationEvent = 1173,

        /// <summary>
        /// AUDIT_SERVER_ALTER_TRACE_EVENT.
        /// </summary>
        AuditServerAlterTraceEvent = 1175,

        /// <summary>
        /// AUDIT_SERVER_OBJECT_MANAGEMENT_EVENT.
        /// </summary>
        AuditServerObjectManagementEvent = 1176,

        /// <summary>
        /// AUDIT_SERVER_PRINCIPAL_MANAGEMENT_EVENT.
        /// </summary>
        AuditServerPrincipalManagementEvent = 1177,

        /// <summary>
        /// AUDIT_DATABASE_OPERATION_EVENT.
        /// </summary>
        AuditDatabaseOperationEvent = 1178,

        /// <summary>
        /// AUDIT_DATABASE_OBJECT_ACCESS_EVENT.
        /// </summary>
        AuditDatabaseObjectAccessEvent = 1180,

        /// <summary>
        /// OLEDB_PROVIDER_INFORMATION.
        /// </summary>
        OledbProviderInformation = 1194,

        /// <summary>
        /// MOUNT_TAPE.
        /// </summary>
        MountTape = 1195,

        /// <summary>
        /// ASSEMBLY_LOAD.
        /// </summary>
        AssemblyLoad = 1196,

        /// <summary>
        /// XQUERY_STATIC_TYPE.
        /// </summary>
        XQueryStaticType = 1198,

        /// <summary>
        /// QN__SUBSCRIPTION.
        /// </summary>
        QnSubscription = 1199,

        /// <summary>
        /// QN__PARAMETER_TABLE.
        /// </summary>
        QnParameterTable = 1200,

        /// <summary>
        /// QN__TEMPLATE.
        /// </summary>
        QnTemplate = 1201,

        /// <summary>
        /// QN__DYNAMICS.
        /// </summary>
        QnDynamics = 1202,

        /// <summary>
        /// BITMAP_WARNING.
        /// </summary>
        BitmapWarning = 1212,

        /// <summary>
        /// DATABASE_SUSPECT_DATA_PAGE.
        /// </summary>
        DatabaseSuspectDataPage = 1213,

        /// <summary>
        /// CPU_THRESHOLD_EXCEEDED.
        /// </summary>
        CpuThresholdExceeded = 1214,

        /// <summary>
        /// AUDIT_FULLTEXT.
        /// </summary>
        AuditFullText = 1235,
    }
}