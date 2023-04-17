//------------------------------------------------------------------------------
// <copyright file="ExternalDataSourceOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ExternalDataSourceOptionHelper : OptionsHelper<ExternalDataSourceOptionKind>
    {
        /// <summary>
        /// The external data source optional property mapptings.
        /// </summary>
        private ExternalDataSourceOptionHelper()
        {
            AddOptionMapping(ExternalDataSourceOptionKind.ResourceManagerLocation, CodeGenerationSupporter.ResourceManagerLocation);
            AddOptionMapping(ExternalDataSourceOptionKind.Credential, CodeGenerationSupporter.Credential);
            AddOptionMapping(ExternalDataSourceOptionKind.DatabaseName, CodeGenerationSupporter.DatabaseName);
            AddOptionMapping(ExternalDataSourceOptionKind.ShardMapName, CodeGenerationSupporter.ShardMapName);
            AddOptionMapping(ExternalDataSourceOptionKind.ConnectionOptions, CodeGenerationSupporter.ConnectionOptions);
        }

        internal static readonly ExternalDataSourceOptionHelper Instance = new ExternalDataSourceOptionHelper();
    }
}
