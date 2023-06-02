//------------------------------------------------------------------------------
// <copyright file="DeviceTypesHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class DeviceTypesHelper : OptionsHelper<DeviceType>
    {
        private DeviceTypesHelper()
        {
            AddOptionMapping(DeviceType.Disk, CodeGenerationSupporter.Disk);
            AddOptionMapping(DeviceType.Tape, CodeGenerationSupporter.Tape);
            AddOptionMapping(DeviceType.VirtualDevice, CodeGenerationSupporter.VirtualDevice, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DeviceType.DatabaseSnapshot, CodeGenerationSupporter.DatabaseSnapshot, SqlVersionFlags.TSql90AndAbove);
            AddOptionMapping(DeviceType.Url, CodeGenerationSupporter.Url, SqlVersionFlags.TSql110AndAbove);
        }

        internal static readonly DeviceTypesHelper Instance = new DeviceTypesHelper();
    }
}
