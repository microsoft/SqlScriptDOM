//------------------------------------------------------------------------------
// <copyright file="RdaTableOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class RdaTableOptionHelper : OptionsHelper<RdaTableOption>
    {
        private RdaTableOptionHelper()
        {
            AddOptionMapping(RdaTableOption.Enable, CodeGenerationSupporter.On);
            AddOptionMapping(RdaTableOption.Disable, CodeGenerationSupporter.Off);
            AddOptionMapping(RdaTableOption.OffWithoutDataRecovery, CodeGenerationSupporter.OffWithoutDataRecovery);
        }

        public static RdaTableOptionHelper Instance = new RdaTableOptionHelper();
    }
}
