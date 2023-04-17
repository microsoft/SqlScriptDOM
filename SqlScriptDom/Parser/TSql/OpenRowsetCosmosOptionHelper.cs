//------------------------------------------------------------------------------
// <copyright file="OpenRowsetCosmosOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class OpenRowsetCosmosOptionsHelper : OptionsHelper<OpenRowsetCosmosOptionKind>
    {
        private OpenRowsetCosmosOptionsHelper()
        {
            AddOptionMapping(OpenRowsetCosmosOptionKind.Connection, CodeGenerationSupporter.Connection, SqlVersionFlags.TSql160AndAbove);
            AddOptionMapping(OpenRowsetCosmosOptionKind.Credential, CodeGenerationSupporter.Credential, SqlVersionFlags.TSql160AndAbove);
            AddOptionMapping(OpenRowsetCosmosOptionKind.Object, CodeGenerationSupporter.Object, SqlVersionFlags.TSql160AndAbove);
            AddOptionMapping(OpenRowsetCosmosOptionKind.Provider, CodeGenerationSupporter.Provider, SqlVersionFlags.TSql160AndAbove);
            AddOptionMapping(OpenRowsetCosmosOptionKind.Server_Credential, CodeGenerationSupporter.ServerCredential, SqlVersionFlags.TSql160AndAbove);
        }

        internal static readonly OpenRowsetCosmosOptionsHelper Instance = new OpenRowsetCosmosOptionsHelper();
    }
}
