//------------------------------------------------------------------------------
// <copyright file="FipsComplianceLevelHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with different FIPS compliance levels
    /// </summary>
    
    internal class FipsComplianceLevelHelper : OptionsHelper<FipsComplianceLevel>
    {
        private FipsComplianceLevelHelper()
        {
            AddOptionMapping(FipsComplianceLevel.Off, TSqlTokenType.Off);
            AddOptionMapping(FipsComplianceLevel.Entry, "'" + CodeGenerationSupporter.Entry + "'");
            AddOptionMapping(FipsComplianceLevel.Intermediate, "'" + CodeGenerationSupporter.Intermediate + "'");
            AddOptionMapping(FipsComplianceLevel.Full, "'" + CodeGenerationSupporter.Full + "'");
        }

        internal static readonly FipsComplianceLevelHelper Instance = new FipsComplianceLevelHelper();
    }
}
