//------------------------------------------------------------------------------
// <copyright file="TriggerEventGroupHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class TriggerEventGroupHelper : OptionsHelper<EventNotificationEventGroup>
    {
        private TriggerEventGroupHelper()
        {
            // Yukon database level event groups
            AddOptionMapping(EventNotificationEventGroup.DdlApplicationRoleEvents, CodeGenerationSupporter.DdlApplicationRoleEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlAssemblyEvents, CodeGenerationSupporter.DdlAssemblyEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlAuthorizationDatabaseEvents, CodeGenerationSupporter.DdlAuthorizationDatabaseEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlCertificateEvents, CodeGenerationSupporter.DdlCertificateEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlContractEvents, CodeGenerationSupporter.DdlContractEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseLevelEvents, CodeGenerationSupporter.DdlDatabaseLevelEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseSecurityEvents, CodeGenerationSupporter.DdlDatabaseSecurityEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlEventNotificationEvents, CodeGenerationSupporter.DdlEventNotificationEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlFunctionEvents, CodeGenerationSupporter.DdlFunctionEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlGdrDatabaseEvents, CodeGenerationSupporter.DdlGdrDatabaseEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlIndexEvents, CodeGenerationSupporter.DdlIndexEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlMessageTypeEvents, CodeGenerationSupporter.DdlMessageTypeEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlPartitionEvents, CodeGenerationSupporter.DdlPartitionEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlPartitionFunctionEvents, CodeGenerationSupporter.DdlPartitionFunctionEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlPartitionSchemeEvents, CodeGenerationSupporter.DdlPartitionSchemeEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlProcedureEvents, CodeGenerationSupporter.DdlProcedureEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlQueueEvents, CodeGenerationSupporter.DdlQueueEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlRemoteServiceBindingEvents, CodeGenerationSupporter.DdlRemoteServiceBindingEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlRoleEvents, CodeGenerationSupporter.DdlRoleEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlRouteEvents, CodeGenerationSupporter.DdlRouteEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlSchemaEvents, CodeGenerationSupporter.DdlSchemaEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServiceEvents, CodeGenerationSupporter.DdlServiceEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlSsbEvents, CodeGenerationSupporter.DdlSsbEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlStatisticsEvents, CodeGenerationSupporter.DdlStatisticsEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlSynonymEvents, CodeGenerationSupporter.DdlSynonymEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlTableEvents, CodeGenerationSupporter.DdlTableEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlTableViewEvents, CodeGenerationSupporter.DdlTableViewEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlTriggerEvents, CodeGenerationSupporter.DdlTriggerEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlTypeEvents, CodeGenerationSupporter.DdlTypeEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlUserEvents, CodeGenerationSupporter.DdlUserEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlViewEvents, CodeGenerationSupporter.DdlViewEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlXmlSchemaCollectionEvents, CodeGenerationSupporter.DdlXmlSchemaCollectionEvents, SqlVersionFlags.TSql90AndAbove);

            // Yukon server level event groups
            AddOptionMapping(EventNotificationEventGroup.DdlAuthorizationServerEvents, CodeGenerationSupporter.DdlAuthorizationServerEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlEndpointEvents, CodeGenerationSupporter.DdlEndpointEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlEvents, CodeGenerationSupporter.DdlEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlGdrServerEvents, CodeGenerationSupporter.DdlGdrServerEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlLoginEvents, CodeGenerationSupporter.DdlLoginEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServerLevelEvents, CodeGenerationSupporter.DdlServerLevelEvents, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServerSecurityEvents, CodeGenerationSupporter.DdlServerSecurityEvents, SqlVersionFlags.TSql90AndAbove);

            // Katmai database level event groups
            AddOptionMapping(EventNotificationEventGroup.DdlAsymmetricKeyEvents, CodeGenerationSupporter.DdlAsymmetricKeyEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlBrokerPriorityEvents, CodeGenerationSupporter.DdlBrokerPriorityEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlCryptoSignatureEvents, CodeGenerationSupporter.DdlCryptoSignatureEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseAuditSpecificationEvents, CodeGenerationSupporter.DdlDatabaseAuditSpecificationEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseEncryptionKeyEvents, CodeGenerationSupporter.DdlDatabaseEncryptionKeyEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDefaultEvents, CodeGenerationSupporter.DdlDefaultEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlExtendedPropertyEvents, CodeGenerationSupporter.DdlExtendedPropertyEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlFullTextCatalogEvents, CodeGenerationSupporter.DdlFullTextCatalogEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlFullTextStopListEvents, CodeGenerationSupporter.DdlFullTextStopListEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlMasterKeyEvents, CodeGenerationSupporter.DdlMasterKeyEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlPlanGuideEvents, CodeGenerationSupporter.DdlPlanGuideEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlRuleEvents, CodeGenerationSupporter.DdlRuleEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlSymmetricKeyEvents, CodeGenerationSupporter.DdlSymmetricKeyEvents, SqlVersionFlags.TSql100AndAbove);

            // Katmai server level event groups
            AddOptionMapping(EventNotificationEventGroup.DdlCredentialEvents, CodeGenerationSupporter.DdlCredentialEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseEvents, CodeGenerationSupporter.DdlDatabaseEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlCryptographicProviderEvents, CodeGenerationSupporter.DdlCryptographicProviderEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlEventSessionEvents, CodeGenerationSupporter.DdlEventSessionEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlExtendedProcedureEvents, CodeGenerationSupporter.DdlExtendedProcedureEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlLinkedServerEvents, CodeGenerationSupporter.DdlLinkedServerEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlLinkedServerLoginEvents, CodeGenerationSupporter.DdlLinkedServerLoginEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlMessageEvents, CodeGenerationSupporter.DdlMessageEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlRemoteServerEvents, CodeGenerationSupporter.DdlRemoteServerEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlResourceGovernorEvents, CodeGenerationSupporter.DdlResourceGovernorEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlResourcePool, CodeGenerationSupporter.DdlResourcePool, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServerAuditEvents, CodeGenerationSupporter.DdlServerAuditEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServerAuditSpecificationEvents, CodeGenerationSupporter.DdlServerAuditSpecificationEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlServiceMasterKeyEvents, CodeGenerationSupporter.DdlServiceMasterKeyEvents, SqlVersionFlags.TSql100AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlWorkloadGroup, CodeGenerationSupporter.DdlWorkloadGroup, SqlVersionFlags.TSql100AndAbove);

            //Denali event groups
            AddOptionMapping(EventNotificationEventGroup.DdlSearchPropertyListEvents, CodeGenerationSupporter.DdlSearchPropertyListEvents, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlSequenceEvents, CodeGenerationSupporter.DdlSequenceEvents, SqlVersionFlags.TSql110AndAbove);
            AddOptionMapping(EventNotificationEventGroup.DdlAvailabilityGroupEvents, CodeGenerationSupporter.DdlAvailabilityGroupEvents, SqlVersionFlags.TSql110AndAbove);

            // SQL 12 event groups
            AddOptionMapping(EventNotificationEventGroup.DdlDatabaseAuditEvents, CodeGenerationSupporter.DdlDatabaseAuditEvents, SqlVersionFlags.TSql120AndAbove);

            // SQL 15 event groups
            // TODO: Version bump change version to 13
            AddOptionMapping(EventNotificationEventGroup.DdlSecurityPolicyEvents, CodeGenerationSupporter.DdlSecurityPolicyEvents, SqlVersionFlags.TSql120AndAbove);
        }

        internal static readonly TriggerEventGroupHelper Instance = new TriggerEventGroupHelper();
    }
}
