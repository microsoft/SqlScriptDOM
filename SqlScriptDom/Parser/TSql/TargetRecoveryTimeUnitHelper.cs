//------------------------------------------------------------------------------
// <copyright file="RecoveryDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class TargetRecoveryTimeUnitHelper : OptionsHelper<TimeUnit>
    {
        private TargetRecoveryTimeUnitHelper()
        {
            AddOptionMapping(TimeUnit.Minutes, CodeGenerationSupporter.Minutes);
            AddOptionMapping(TimeUnit.Seconds, CodeGenerationSupporter.Seconds);
        }

        internal static readonly TargetRecoveryTimeUnitHelper Instance = new TargetRecoveryTimeUnitHelper();
    }
}

