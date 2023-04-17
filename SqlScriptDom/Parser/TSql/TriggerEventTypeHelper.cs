//------------------------------------------------------------------------------
// <copyright file="TriggerEventTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class TriggerEventTypeHelper : OptionsHelper<EventNotificationEventType>
    {
        // event types taken from sys.trigger_event_types
        //
        private TriggerEventTypeHelper()
        {
            // Yukon databased scoped trigger event types
            AddOptionMapping(EventNotificationEventType.AddRoleMember, CodeGenerationSupporter.AddRoleMember, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterApplicationRole, CodeGenerationSupporter.AlterApplicationRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterAssembly, CodeGenerationSupporter.AlterAssembly, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterAuthorizationDatabase, CodeGenerationSupporter.AlterAuthorizationDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterCertificate, CodeGenerationSupporter.AlterCertificate, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterFunction, CodeGenerationSupporter.AlterFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterIndex, CodeGenerationSupporter.AlterIndex, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterMessageType, CodeGenerationSupporter.AlterMessageType, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterPartitionFunction, CodeGenerationSupporter.AlterPartitionFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterPartitionScheme, CodeGenerationSupporter.AlterPartitionScheme, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterProcedure, CodeGenerationSupporter.AlterProcedure, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterQueue, CodeGenerationSupporter.AlterQueue, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterRemoteServiceBinding, CodeGenerationSupporter.AlterRemoteServiceBinding, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterRole, CodeGenerationSupporter.AlterRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterRoute, CodeGenerationSupporter.AlterRoute, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterSchema, CodeGenerationSupporter.AlterSchema, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterService, CodeGenerationSupporter.AlterService, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterTable, CodeGenerationSupporter.AlterTable, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterTrigger, CodeGenerationSupporter.AlterTrigger, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterUser, CodeGenerationSupporter.AlterUser, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterView, CodeGenerationSupporter.AlterView, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterXmlSchemaCollection, CodeGenerationSupporter.AlterXmlSchemaCollection, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateApplicationRole, CodeGenerationSupporter.CreateApplicationRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateAssembly, CodeGenerationSupporter.CreateAssembly, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateCertificate, CodeGenerationSupporter.CreateCertificate, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateContract, CodeGenerationSupporter.CreateContract, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateEventNotification, CodeGenerationSupporter.CreateEventNotification, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateFunction, CodeGenerationSupporter.CreateFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateIndex, CodeGenerationSupporter.CreateIndex, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateMessageType, CodeGenerationSupporter.CreateMessageType, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreatePartitionFunction, CodeGenerationSupporter.CreatePartitionFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreatePartitionScheme, CodeGenerationSupporter.CreatePartitionScheme, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateProcedure, CodeGenerationSupporter.CreateProcedure, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateQueue, CodeGenerationSupporter.CreateQueue, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateRemoteServiceBinding, CodeGenerationSupporter.CreateRemoteServiceBinding, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateRole, CodeGenerationSupporter.CreateRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateRoute, CodeGenerationSupporter.CreateRoute, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateSchema, CodeGenerationSupporter.CreateSchema, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateService, CodeGenerationSupporter.CreateService, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateStatistics, CodeGenerationSupporter.CreateStatistics, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateSynonym, CodeGenerationSupporter.CreateSynonym, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateTable, CodeGenerationSupporter.CreateTable, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateTrigger, CodeGenerationSupporter.CreateTrigger, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateType, CodeGenerationSupporter.CreateType, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateUser, CodeGenerationSupporter.CreateUser, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateView, CodeGenerationSupporter.CreateView, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateXmlIndex, CodeGenerationSupporter.CreateXmlIndex, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateXmlSchemaCollection, CodeGenerationSupporter.CreateXmlSchemaCollection, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DenyDatabase, CodeGenerationSupporter.DenyDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropApplicationRole, CodeGenerationSupporter.DropApplicationRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropAssembly, CodeGenerationSupporter.DropAssembly, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropCertificate, CodeGenerationSupporter.DropCertificate, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropContract, CodeGenerationSupporter.DropContract, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropEventNotification, CodeGenerationSupporter.DropEventNotification, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropFunction, CodeGenerationSupporter.DropFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropIndex, CodeGenerationSupporter.DropIndex, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropMessageType, CodeGenerationSupporter.DropMessageType, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropPartitionFunction, CodeGenerationSupporter.DropPartitionFunction, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropPartitionScheme, CodeGenerationSupporter.DropPartitionScheme, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropProcedure, CodeGenerationSupporter.DropProcedure, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropQueue, CodeGenerationSupporter.DropQueue, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRemoteServiceBinding, CodeGenerationSupporter.DropRemoteServiceBinding, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRole, CodeGenerationSupporter.DropRole, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRoleMember, CodeGenerationSupporter.DropRoleMember, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRoute, CodeGenerationSupporter.DropRoute, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSchema, CodeGenerationSupporter.DropSchema, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropService, CodeGenerationSupporter.DropService, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropStatistics, CodeGenerationSupporter.DropStatistics, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSynonym, CodeGenerationSupporter.DropSynonym, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropTable, CodeGenerationSupporter.DropTable, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropTrigger, CodeGenerationSupporter.DropTrigger, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropType, CodeGenerationSupporter.DropType, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropUser, CodeGenerationSupporter.DropUser, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropView, CodeGenerationSupporter.DropView, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropXmlSchemaCollection, CodeGenerationSupporter.DropXmlSchemaCollection, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.GrantDatabase, CodeGenerationSupporter.GrantDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.RevokeDatabase, CodeGenerationSupporter.RevokeDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.UpdateStatistics, CodeGenerationSupporter.UpdateStatistics, SqlVersionFlags.TSql90AndAbove);

            // Yukon server scoped trigger event types
            AddOptionMapping(EventNotificationEventType.AddServerRoleMember, CodeGenerationSupporter.AddServerRoleMember, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterAuthorizationServer, CodeGenerationSupporter.AlterAuthorizationServer, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterDatabase, CodeGenerationSupporter.AlterDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterEndpoint, CodeGenerationSupporter.AlterEndpoint, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterLogin, CodeGenerationSupporter.AlterLogin, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateDatabase, CodeGenerationSupporter.CreateDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateEndpoint, CodeGenerationSupporter.CreateEndpoint, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateLogin, CodeGenerationSupporter.CreateLogin, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DenyServer, CodeGenerationSupporter.DenyServer, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropDatabase, CodeGenerationSupporter.DropDatabase, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropEndpoint, CodeGenerationSupporter.DropEndpoint, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropLogin, CodeGenerationSupporter.DropLogin, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.DropServerRoleMember, CodeGenerationSupporter.DropServerRoleMember, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.GrantServer, CodeGenerationSupporter.GrantServer, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventType.RevokeServer, CodeGenerationSupporter.RevokeServer, SqlVersionFlags.TSql90AndAbove);

            // Katmai database scoped trigger event types
            AddOptionMapping(EventNotificationEventType.AddSignature, CodeGenerationSupporter.AddSignature, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AddSignatureSchemaObject, CodeGenerationSupporter.AddSignatureSchemaObject, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterAsymmetricKey, CodeGenerationSupporter.AlterAsymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterBrokerPriority, CodeGenerationSupporter.AlterBrokerPriority, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterDatabaseAuditSpecification, CodeGenerationSupporter.AlterDatabaseAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterDatabaseEncryptionKey, CodeGenerationSupporter.AlterDatabaseEncryptionKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterExtendedProperty, CodeGenerationSupporter.AlterExtendedProperty, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterFullTextCatalog, CodeGenerationSupporter.AlterFullTextCatalog, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterFullTextIndex, CodeGenerationSupporter.AlterFullTextIndex, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterFullTextStopList, CodeGenerationSupporter.AlterFullTextStopList, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterMasterKey, CodeGenerationSupporter.AlterMasterKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterPlanGuide, CodeGenerationSupporter.AlterPlanGuide, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterSymmetricKey, CodeGenerationSupporter.AlterSymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.BindDefault, CodeGenerationSupporter.BindDefault, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.BindRule, CodeGenerationSupporter.BindRule, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateAsymmetricKey, CodeGenerationSupporter.CreateAsymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateBrokerPriority, CodeGenerationSupporter.CreateBrokerPriority, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateDatabaseAuditSpecification, CodeGenerationSupporter.CreateDatabaseAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateDatabaseEncryptionKey, CodeGenerationSupporter.CreateDatabaseEncryptionKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateDefault, CodeGenerationSupporter.CreateDefault, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateExtendedProperty, CodeGenerationSupporter.CreateExtendedProperty, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateFullTextCatalog, CodeGenerationSupporter.CreateFullTextCatalog, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateFullTextIndex, CodeGenerationSupporter.CreateFullTextIndex, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateFullTextStopList, CodeGenerationSupporter.CreateFullTextStopList, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateMasterKey, CodeGenerationSupporter.CreateMasterKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreatePlanGuide, CodeGenerationSupporter.CreatePlanGuide, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateRule, CodeGenerationSupporter.CreateRule, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateSpatialIndex, CodeGenerationSupporter.CreateSpatialIndex, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateSymmetricKey, CodeGenerationSupporter.CreateSymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropAsymmetricKey, CodeGenerationSupporter.DropAsymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropBrokerPriority, CodeGenerationSupporter.DropBrokerPriority, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropDatabaseAuditSpecification, CodeGenerationSupporter.DropDatabaseAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropDatabaseEncryptionKey, CodeGenerationSupporter.DropDatabaseEncryptionKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropDefault, CodeGenerationSupporter.DropDefault, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropExtendedProperty, CodeGenerationSupporter.DropExtendedProperty, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropFullTextCatalog, CodeGenerationSupporter.DropFullTextCatalog, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropFullTextIndex, CodeGenerationSupporter.DropFullTextIndex, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropFullTextStopList, CodeGenerationSupporter.DropFullTextStopList, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropMasterKey, CodeGenerationSupporter.DropMasterKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropPlanGuide, CodeGenerationSupporter.DropPlanGuide, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRule, CodeGenerationSupporter.DropRule, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSignature, CodeGenerationSupporter.DropSignature, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSignatureSchemaObject, CodeGenerationSupporter.DropSignatureSchemaObject, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSymmetricKey, CodeGenerationSupporter.DropSymmetricKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.Rename, CodeGenerationSupporter.Rename, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.UnbindDefault, CodeGenerationSupporter.UnbindDefault, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.UnbindRule, CodeGenerationSupporter.UnbindRule, SqlVersionFlags.TSql100AndAbove);

            // Katmai server scoped trigger event types
            AddOptionMapping(EventNotificationEventType.AlterCredential, CodeGenerationSupporter.AlterCredential, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterCryptographicProvider, CodeGenerationSupporter.AlterCryptographicProvider, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterEventSession, CodeGenerationSupporter.AlterEventSession, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterInstance, CodeGenerationSupporter.AlterInstance, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterLinkedServer, CodeGenerationSupporter.AlterLinkedServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterMessage, CodeGenerationSupporter.AlterMessage, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterRemoteServer, CodeGenerationSupporter.AlterRemoteServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterResourceGovernorConfig, CodeGenerationSupporter.AlterResourceGovernorConfig, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterResourcePool, CodeGenerationSupporter.AlterResourcePool, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterServerAudit, CodeGenerationSupporter.AlterServerAudit, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterServerAuditSpecification, CodeGenerationSupporter.AlterServerAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterServiceMasterKey, CodeGenerationSupporter.AlterServiceMasterKey, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterWorkloadGroup, CodeGenerationSupporter.AlterWorkloadGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateCredential, CodeGenerationSupporter.CreateCredential, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateCryptographicProvider, CodeGenerationSupporter.CreateCryptographicProvider, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateEventSession, CodeGenerationSupporter.CreateEventSession, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateExtendedProcedure, CodeGenerationSupporter.CreateExtendedProcedure, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateLinkedServer, CodeGenerationSupporter.CreateLinkedServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateLinkedServerLogin, CodeGenerationSupporter.CreateLinkedServerLogin, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateMessage, CodeGenerationSupporter.CreateMessage, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateRemoteServer, CodeGenerationSupporter.CreateRemoteServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateResourcePool, CodeGenerationSupporter.CreateResourcePool, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateServerAudit, CodeGenerationSupporter.CreateServerAudit, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateServerAuditSpecification, CodeGenerationSupporter.CreateServerAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateWorkloadGroup, CodeGenerationSupporter.CreateWorkloadGroup, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropCredential, CodeGenerationSupporter.DropCredential, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropCryptographicProvider, CodeGenerationSupporter.DropCryptographicProvider, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropEventSession, CodeGenerationSupporter.DropEventSession, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropExtendedProcedure, CodeGenerationSupporter.DropExtendedProcedure, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropLinkedServer, CodeGenerationSupporter.DropLinkedServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropLinkedServerLogin, CodeGenerationSupporter.DropLinkedServerLogin, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropMessage, CodeGenerationSupporter.DropMessage, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropRemoteServer, CodeGenerationSupporter.DropRemoteServer, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropResourcePool, CodeGenerationSupporter.DropResourcePool, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropServerAudit, CodeGenerationSupporter.DropServerAudit, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropServerAuditSpecification, CodeGenerationSupporter.DropServerAuditSpecification, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventType.DropWorkloadGroup, CodeGenerationSupporter.DropWorkloadGroup, SqlVersionFlags.TSql100AndAbove);

            //KJ trigger event types
            AddOptionMapping(EventNotificationEventType.AlterServerConfiguration, CodeGenerationSupporter.AlterServerConfiguration, SqlVersionFlags.TSql100AndAbove);

            // Denali trigger event types
            AddOptionMapping(EventNotificationEventType.CreateSearchPropertyList, CodeGenerationSupporter.CreateSearchPropertyList, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterSearchPropertyList, CodeGenerationSupporter.AlterSearchPropertyList, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSearchPropertyList, CodeGenerationSupporter.DropSearchPropertyList, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateServerRole, CodeGenerationSupporter.CreateServerRole, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterServerRole, CodeGenerationSupporter.AlterServerRole, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.DropServerRole, CodeGenerationSupporter.DropServerRole, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateSequence, CodeGenerationSupporter.CreateSequence, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterSequence, CodeGenerationSupporter.AlterSequence, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSequence, CodeGenerationSupporter.DropSequence, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateAvailabilityGroup, CodeGenerationSupporter.CreateAvailabilityGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterAvailabilityGroup, CodeGenerationSupporter.AlterAvailabilityGroup, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventType.DropAvailabilityGroup, CodeGenerationSupporter.DropAvailabilityGroup, SqlVersionFlags.TSql110AndAbove);

            // SQL 12 trigger event types
            AddOptionMapping(EventNotificationEventType.CreateDatabaseAudit, CodeGenerationSupporter.CreateDatabaseAudit, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.DropDatabaseAudit, CodeGenerationSupporter.DropDatabaseAudit, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterDatabaseAudit, CodeGenerationSupporter.AlterDatabaseAudit, SqlVersionFlags.TSql120AndAbove);

            // SQL 15 trigger event types
            // TODO: Version bump change version to 13
            AddOptionMapping(EventNotificationEventType.CreateSecurityPolicy, CodeGenerationSupporter.CreateSecurityPolicy, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterSecurityPolicy, CodeGenerationSupporter.AlterSecurityPolicy, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.DropSecurityPolicy, CodeGenerationSupporter.DropSecurityPolicy, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateColumnMasterKey, CodeGenerationSupporter.CreateColumnMasterKey, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.DropColumnMasterKey, CodeGenerationSupporter.DropColumnMasterKey, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateColumnEncryptionKey, CodeGenerationSupporter.CreateColumnEncryptionKey, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterColumnEncryptionKey, CodeGenerationSupporter.AlterColumnEncryptionKey, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.DropColumnEncryptionKey, CodeGenerationSupporter.DropColumnEncryptionKey, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterDatabaseScopedConfiguration, CodeGenerationSupporter.AlterDatabaseScopedConfiguration, SqlVersionFlags.TSql120AndAbove);
            AddOptionMapping(EventNotificationEventType.CreateExternalResourcePool, CodeGenerationSupporter.CreateExternalResourcePool, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(EventNotificationEventType.AlterExternalResourcePool, CodeGenerationSupporter.AlterExternalResourcePool, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(EventNotificationEventType.DropExternalResourcePool, CodeGenerationSupporter.DropExternalResourcePool, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly TriggerEventTypeHelper Instance = new TriggerEventTypeHelper();
    }
}
