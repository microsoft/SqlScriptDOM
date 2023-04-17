//------------------------------------------------------------------------------
// <copyright file="EventNotificationEventGroup.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of event group 
    /// </summary>    
    public enum EventNotificationEventGroup
    {
        /// <summary>
        /// Unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// DDL_EVENTS.
        /// </summary>
        DdlEvents = 10001,

        /// <summary>
        /// DDL_SERVER_LEVEL_EVENTS.
        /// </summary>
        DdlServerLevelEvents = 10002,

        /// <summary>
        /// DDL_ENDPOINT_EVENTS.
        /// </summary>
        DdlEndpointEvents = 10003,

        /// <summary>
        /// DDL_DATABASE_EVENTS.
        /// </summary>
        DdlDatabaseEvents = 10004,

        /// <summary>
        /// DDL_SERVER_SECURITY_EVENTS.
        /// </summary>
        DdlServerSecurityEvents = 10005,

        /// <summary>
        /// DDL_LOGIN_EVENTS.
        /// </summary>
        DdlLoginEvents = 10006,

        /// <summary>
        /// DDL_GDR_SERVER_EVENTS.
        /// </summary>
        DdlGdrServerEvents = 10007,

        /// <summary>
        /// DDL_AUTHORIZATION_SERVER_EVENTS.
        /// </summary>
        DdlAuthorizationServerEvents = 10008,

        /// <summary>
        /// DDL_CREDENTIAL_EVENTS.
        /// </summary>
        DdlCredentialEvents = 10009,

        /// <summary>
        /// DDL_SERVICE_MASTER_KEY_EVENTS.
        /// </summary>
        DdlServiceMasterKeyEvents = 10010,

        /// <summary>
        /// DDL_EXTENDED_PROCEDURE_EVENTS.
        /// </summary>
        DdlExtendedProcedureEvents = 10011,

        /// <summary>
        /// DDL_LINKED_SERVER_EVENTS.
        /// </summary>
        DdlLinkedServerEvents = 10012,

        /// <summary>
        /// DDL_LINKED_SERVER_LOGIN_EVENTS.
        /// </summary>
        DdlLinkedServerLoginEvents = 10013,

        /// <summary>
        /// DDL_MESSAGE_EVENTS.
        /// </summary>
        DdlMessageEvents = 10014,

        /// <summary>
        /// DDL_REMOTE_SERVER_EVENTS.
        /// </summary>
        DdlRemoteServerEvents = 10015,

        /// <summary>
        /// DDL_DATABASE_LEVEL_EVENTS.
        /// </summary>
        DdlDatabaseLevelEvents = 10016,

        /// <summary>
        /// DDL_TABLE_VIEW_EVENTS.
        /// </summary>
        DdlTableViewEvents = 10017,

        /// <summary>
        /// DDL_TABLE_EVENTS.
        /// </summary>
        DdlTableEvents = 10018,

        /// <summary>
        /// DDL_VIEW_EVENTS.
        /// </summary>
        DdlViewEvents = 10019,

        /// <summary>
        /// DDL_INDEX_EVENTS.
        /// </summary>
        DdlIndexEvents = 10020,

        /// <summary>
        /// DDL_STATISTICS_EVENTS.
        /// </summary>
        DdlStatisticsEvents = 10021,

        /// <summary>
        /// DDL_SYNONYM_EVENTS.
        /// </summary>
        DdlSynonymEvents = 10022,

        /// <summary>
        /// DDL_FUNCTION_EVENTS.
        /// </summary>
        DdlFunctionEvents = 10023,

        /// <summary>
        /// DDL_PROCEDURE_EVENTS.
        /// </summary>
        DdlProcedureEvents = 10024,

        /// <summary>
        /// DDL_TRIGGER_EVENTS.
        /// </summary>
        DdlTriggerEvents = 10025,

        /// <summary>
        /// DDL_EVENT_NOTIFICATION_EVENTS.
        /// </summary>
        DdlEventNotificationEvents = 10026,

        /// <summary>
        /// DDL_ASSEMBLY_EVENTS.
        /// </summary>
        DdlAssemblyEvents = 10027,

        /// <summary>
        /// DDL_TYPE_EVENTS.
        /// </summary>
        DdlTypeEvents = 10028,

        /// <summary>
        /// DDL_DATABASE_SECURITY_EVENTS.
        /// </summary>
        DdlDatabaseSecurityEvents = 10029,

        /// <summary>
        /// DDL_CERTIFICATE_EVENTS.
        /// </summary>
        DdlCertificateEvents = 10030,

        /// <summary>
        /// DDL_USER_EVENTS.
        /// </summary>
        DdlUserEvents = 10031,

        /// <summary>
        /// DDL_ROLE_EVENTS.
        /// </summary>
        DdlRoleEvents = 10032,

        /// <summary>
        /// DDL_APPLICATION_ROLE_EVENTS.
        /// </summary>
        DdlApplicationRoleEvents = 10033,

        /// <summary>
        /// DDL_SCHEMA_EVENTS.
        /// </summary>
        DdlSchemaEvents = 10034,

        /// <summary>
        /// DDL_GDR_DATABASE_EVENTS.
        /// </summary>
        DdlGdrDatabaseEvents = 10035,

        /// <summary>
        /// DDL_AUTHORIZATION_DATABASE_EVENTS.
        /// </summary>
        DdlAuthorizationDatabaseEvents = 10036,

        /// <summary>
        /// DDL_SYMMETRIC_KEY_EVENTS.
        /// </summary>
        DdlSymmetricKeyEvents = 10037,

        /// <summary>
        /// DDL_ASYMMETRIC_KEY_EVENTS.
        /// </summary>
        DdlAsymmetricKeyEvents = 10038,

        /// <summary>
        /// DDL_CRYPTO_SIGNATURE_EVENTS.
        /// </summary>
        DdlCryptoSignatureEvents = 10039,

        /// <summary>
        /// DDL_MASTER_KEY_EVENTS.
        /// </summary>
        DdlMasterKeyEvents = 10040,

        /// <summary>
        /// DDL_SSB_EVENTS.
        /// </summary>
        DdlSsbEvents = 10041,

        /// <summary>
        /// DDL_MESSAGE_TYPE_EVENTS.
        /// </summary>
        DdlMessageTypeEvents = 10042,

        /// <summary>
        /// DDL_CONTRACT_EVENTS.
        /// </summary>
        DdlContractEvents = 10043,

        /// <summary>
        /// DDL_QUEUE_EVENTS.
        /// </summary>
        DdlQueueEvents = 10044,

        /// <summary>
        /// DDL_SERVICE_EVENTS.
        /// </summary>
        DdlServiceEvents = 10045,

        /// <summary>
        /// DDL_ROUTE_EVENTS.
        /// </summary>
        DdlRouteEvents = 10046,

        /// <summary>
        /// DDL_REMOTE_SERVICE_BINDING_EVENTS.
        /// </summary>
        DdlRemoteServiceBindingEvents = 10047,

        /// <summary>
        /// DDL_XML_SCHEMA_COLLECTION_EVENTS.
        /// </summary>
        DdlXmlSchemaCollectionEvents = 10048,

        /// <summary>
        /// DDL_PARTITION_EVENTS.
        /// </summary>
        DdlPartitionEvents = 10049,

        /// <summary>
        /// DDL_PARTITION_FUNCTION_EVENTS.
        /// </summary>
        DdlPartitionFunctionEvents = 10050,

        /// <summary>
        /// DDL_PARTITION_SCHEME_EVENTS.
        /// </summary>
        DdlPartitionSchemeEvents = 10051,

        /// <summary>
        /// DDL_DEFAULT_EVENTS.
        /// </summary>
        DdlDefaultEvents = 10052,

        /// <summary>
        /// DDL_EXTENDED_PROPERTY_EVENTS.
        /// </summary>
        DdlExtendedPropertyEvents = 10053,

        /// <summary>
        /// DDL_FULLTEXT_CATALOG_EVENTS.
        /// </summary>
        DdlFullTextCatalogEvents = 10054,

        /// <summary>
        /// DDL_PLAN_GUIDE_EVENTS.
        /// </summary>
        DdlPlanGuideEvents = 10055,

        /// <summary>
        /// DDL_RULE_EVENTS.
        /// </summary>
        DdlRuleEvents = 10056,

        /// <summary>
        /// DDL_EVENT_SESSION_EVENTS.
        /// </summary>
        DdlEventSessionEvents = 10057,

        /// <summary>
        /// DDL_RESOURCE_GOVERNOR_EVENTS.
        /// </summary>
        DdlResourceGovernorEvents = 10058,

        /// <summary>
        /// DDL_RESOURCE_POOL.
        /// </summary>
        DdlResourcePool = 10059,

        /// <summary>
        /// DDL_WORKLOAD_GROUP.
        /// </summary>
        DdlWorkloadGroup = 10060,

        /// <summary>
        /// DDL_CRYPTOGRAPHIC_PROVIDER_EVENTS.
        /// </summary>
        DdlCryptographicProviderEvents = 10061,

        /// <summary>
        /// DDL_DATABASE_ENCRYPTION_KEY_EVENTS.
        /// </summary>
        DdlDatabaseEncryptionKeyEvents = 10062,

        /// <summary>
        /// DDL_BROKER_PRIORITY_EVENTS.
        /// </summary>
        DdlBrokerPriorityEvents = 10063,

        /// <summary>
        /// DDL_SERVER_AUDIT_EVENTS.
        /// </summary>
        DdlServerAuditEvents = 10064,

        /// <summary>
        /// DDL_SERVER_AUDIT_SPECIFICATION_EVENTS.
        /// </summary>
        DdlServerAuditSpecificationEvents = 10065,

        /// <summary>
        /// DDL_DATABASE_AUDIT_SPECIFICATION_EVENTS.
        /// </summary>
        DdlDatabaseAuditSpecificationEvents = 10066,

        /// <summary>
        /// DDL_FULLTEXT_STOPLIST_EVENTS.
        /// </summary>
        DdlFullTextStopListEvents = 10067,

        /// <summary>
        /// DDL_SEARCH_PROPERTY_LIST_EVENTS.
        /// </summary>
        DdlSearchPropertyListEvents = 10069,

        /// <summary>
        /// DDL_SEQUENCE_EVENTS.
        /// </summary>
        DdlSequenceEvents = 10070,

        /// <summary>
        /// DDL_AVAILABILITY_GROUP_EVENTS.
        /// </summary>
        DdlAvailabilityGroupEvents = 10071,

        /// <summary>
        /// DDL_SEQUENCE_EVENTS.
        /// </summary>
        DdlDatabaseAuditEvents = 10072,

        /// <summary>
        /// DDL_SECURITY_POLICY_EVENTS
        /// </summary>
        DdlSecurityPolicyEvents = 10073,

        /// <summary>
        /// DDL_COLUMN_MASTER_KEY_EVENTS.
        /// </summary>
        DdlColumnMasterKeyEvents = 10074,

        /// <summary>
        /// DDL_COLUMN_ENCRYPTION_KEY_EVENTS.
        /// </summary>
        DdlColumnEncryptionKeyEvents = 10075,

        /// <summary>
        /// DDL_EXTERNAL_RESOURCE_POOL_EVENTS.
        /// </summary>
        DdlExternalResourcePoolEvents = 10076,

        /// <summary>
        /// DDL_EXTERNAL_LIBRARY_EVENTS.
        /// </summary>
        DdlExternalLibraryEvents = 10077,

        /// <summary>
        /// DDL_SENSITIVITY_EVENTS.
        /// </summary>
        DdlSensitivityEvents = 10078,

        /// <summary>
        /// DDL_EXTERNAL_LANGUAGE_EVENTS.
        /// </summary>
        DdlExternalLanguageEvents = 10079,

        /// <summary>
        /// TRC_ALL_EVENTS.
        /// </summary>
        TrcAllEvents = 11000,

        /// <summary>
        /// TRC_DATABASE.
        /// </summary>
        TrcDatabase = 11002,

        /// <summary>
        /// TRC_ERRORS_AND_WARNINGS.
        /// </summary>
        TrcErrorsAndWarnings = 11003,

        /// <summary>
        /// TRC_LOCKS.
        /// </summary>
        TrcLocks = 11004,

        /// <summary>
        /// TRC_OBJECTS.
        /// </summary>
        TrcObjects = 11005,

        /// <summary>
        /// TRC_PERFORMANCE.
        /// </summary>
        TrcPerformance = 11006,

        /// <summary>
        /// TRC_SECURITY_AUDIT.
        /// </summary>
        TrcSecurityAudit = 11008,

        /// <summary>
        /// TRC_SERVER.
        /// </summary>
        TrcServer = 11009,

        /// <summary>
        /// TRC_STORED_PROCEDURES.
        /// </summary>
        TrcStoredProcedures = 11011,

        /// <summary>
        /// TRC_TSQL.
        /// </summary>
        TrcTSql = 11013,

        /// <summary>
        /// TRC_USER_CONFIGURABLE.
        /// </summary>
        TrcUserConfigurable = 11014,

        /// <summary>
        /// TRC_OLEDB.
        /// </summary>
        TrcOledb = 11015,

        /// <summary>
        /// TRC_FULL_TEXT.
        /// </summary>
        TrcFullText = 11017,

        /// <summary>
        /// TRC_DEPRECATION.
        /// </summary>
        TrcDeprecation = 11018,

        /// <summary>
        /// TRC_CLR.
        /// </summary>
        TrcClr = 11020,

        /// <summary>
        /// TRC_QUERY_NOTIFICATIONS.
        /// </summary>
        TrcQueryNotifications = 11021,
    }
}