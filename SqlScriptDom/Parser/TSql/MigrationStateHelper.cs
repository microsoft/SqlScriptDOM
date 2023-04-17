//------------------------------------------------------------------------------
// <copyright file="MigrationStateHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class MigrationStateHelper : OptionsHelper<MigrationState>
    {
        private MigrationStateHelper()
        {
            AddOptionMapping(MigrationState.Paused, CodeGenerationSupporter.Paused);
            AddOptionMapping(MigrationState.Outbound, CodeGenerationSupporter.Outbound);
            AddOptionMapping(MigrationState.Inbound, CodeGenerationSupporter.Inbound);
        }

        public static MigrationStateHelper Instance = new MigrationStateHelper();
    }
}
