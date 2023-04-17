//------------------------------------------------------------------------------
// <copyright file="RetentionUnitsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    [Serializable]
    internal class RetentionUnitHelper : OptionsHelper<TimeUnit>
    {
        private RetentionUnitHelper()
        {
            AddOptionMapping(TimeUnit.Days, CodeGenerationSupporter.Days);
            AddOptionMapping(TimeUnit.Hours, CodeGenerationSupporter.Hours);
            AddOptionMapping(TimeUnit.Minutes, CodeGenerationSupporter.Minutes);
        }

        internal static readonly RetentionUnitHelper Instance = new RetentionUnitHelper();
    }
}