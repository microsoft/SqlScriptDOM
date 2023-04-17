//------------------------------------------------------------------------------
// <copyright file="AvailabilityReplicaOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class AvailabilityReplicaOptionsHelper : OptionsHelper<AvailabilityReplicaOptionKind>
    {
        private AvailabilityReplicaOptionsHelper()
        {
            AddOptionMapping(AvailabilityReplicaOptionKind.ApplyDelay, CodeGenerationSupporter.ApplyDelay);
            AddOptionMapping(AvailabilityReplicaOptionKind.AvailabilityMode, CodeGenerationSupporter.AvailabilityMode);
            AddOptionMapping(AvailabilityReplicaOptionKind.EndpointUrl, CodeGenerationSupporter.EndpointUrl);
            AddOptionMapping(AvailabilityReplicaOptionKind.SecondaryRole, CodeGenerationSupporter.SecondaryRole);
            AddOptionMapping(AvailabilityReplicaOptionKind.SessionTimeout, CodeGenerationSupporter.SessionTimeout);
            
        }

        public static readonly AvailabilityReplicaOptionsHelper Instance = new AvailabilityReplicaOptionsHelper();
    }
}
