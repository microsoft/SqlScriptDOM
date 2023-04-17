//------------------------------------------------------------------------------
// <copyright file="PartnerDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class PartnerDbOptionsHelper : OptionsHelper<PartnerDatabaseOptionKind>
    {
        private PartnerDbOptionsHelper()
        {
            AddOptionMapping(PartnerDatabaseOptionKind.Failover, CodeGenerationSupporter.Failover);
            AddOptionMapping(PartnerDatabaseOptionKind.ForceServiceAllowDataLoss, CodeGenerationSupporter.ForceServiceAllowDataLoss);
            AddOptionMapping(PartnerDatabaseOptionKind.Off, TSqlTokenType.Off);
            AddOptionMapping(PartnerDatabaseOptionKind.Resume, CodeGenerationSupporter.Resume);
            AddOptionMapping(PartnerDatabaseOptionKind.Suspend, CodeGenerationSupporter.Suspend);
            AddOptionMapping(PartnerDatabaseOptionKind.Timeout, CodeGenerationSupporter.Timeout);
        }

        public static readonly PartnerDbOptionsHelper Instance = new PartnerDbOptionsHelper();
    }
}
