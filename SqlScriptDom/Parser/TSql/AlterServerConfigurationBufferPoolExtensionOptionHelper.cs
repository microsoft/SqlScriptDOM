//------------------------------------------------------------------------------
// <copyright file="AlterServerConfigurationBufferPoolExtensionOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class AlterServerConfigurationBufferPoolExtensionOptionHelper : OptionsHelper<AlterServerConfigurationBufferPoolExtensionOptionKind>
    {
        private AlterServerConfigurationBufferPoolExtensionOptionHelper()
        {
            AddOptionMapping(AlterServerConfigurationBufferPoolExtensionOptionKind.FileName, CodeGenerationSupporter.FileName);
            AddOptionMapping(AlterServerConfigurationBufferPoolExtensionOptionKind.Size, CodeGenerationSupporter.Size);
        }

        internal static readonly AlterServerConfigurationBufferPoolExtensionOptionHelper Instance = new AlterServerConfigurationBufferPoolExtensionOptionHelper();
    }
}
