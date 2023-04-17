//------------------------------------------------------------------------------
// <copyright file="SetStatisticsOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Helps SET STATISTICS statement options
    /// </summary>
    
    [Serializable]
    internal class SetStatisticsOptionsHelper : OptionsHelper<SetStatisticsOptions>
    {
        private SetStatisticsOptionsHelper()
        {
            AddOptionMapping(SetStatisticsOptions.IO, CodeGenerationSupporter.IO, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetStatisticsOptions.Profile, CodeGenerationSupporter.Profile, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetStatisticsOptions.Time, CodeGenerationSupporter.Time, SqlVersionFlags.TSqlAll);
            AddOptionMapping(SetStatisticsOptions.Xml, CodeGenerationSupporter.Xml, SqlVersionFlags.TSql90AndAbove);
        }

        internal static readonly SetStatisticsOptionsHelper Instance = new SetStatisticsOptionsHelper();
    }
}
