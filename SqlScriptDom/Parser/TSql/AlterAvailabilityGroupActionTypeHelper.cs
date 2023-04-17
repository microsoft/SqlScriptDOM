//------------------------------------------------------------------------------
// <copyright file="AllowReadOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class AlterAvailabilityGroupActionTypeHelper : OptionsHelper<AlterAvailabilityGroupActionType>
    {
        private AlterAvailabilityGroupActionTypeHelper()
        {
            AddOptionMapping(AlterAvailabilityGroupActionType.Failover, CodeGenerationSupporter.Failover);
            AddOptionMapping(AlterAvailabilityGroupActionType.ForceFailoverAllowDataLoss, CodeGenerationSupporter.ForceFailoverAllowDataLoss);
            AddOptionMapping(AlterAvailabilityGroupActionType.Join, TSqlTokenType.Join );
            AddOptionMapping(AlterAvailabilityGroupActionType.Offline, CodeGenerationSupporter.Offline);
            AddOptionMapping(AlterAvailabilityGroupActionType.Online, CodeGenerationSupporter.Online);
            
        }

        public static readonly AlterAvailabilityGroupActionTypeHelper Instance = new AlterAvailabilityGroupActionTypeHelper();
    }
}
