//------------------------------------------------------------------------------
// <copyright file="modifyfilegroupoptionshelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class ModifyFilegroupOptionsHelper : OptionsHelper<ModifyFileGroupOption>
    {
        private ModifyFilegroupOptionsHelper()
        {
            AddOptionMapping(ModifyFileGroupOption.ReadOnly, CodeGenerationSupporter.ReadOnly);
            AddOptionMapping(ModifyFileGroupOption.ReadOnlyOld, CodeGenerationSupporter.ReadOnlyOld);
            AddOptionMapping(ModifyFileGroupOption.ReadWrite, CodeGenerationSupporter.ReadWrite);
            AddOptionMapping(ModifyFileGroupOption.ReadWriteOld, CodeGenerationSupporter.ReadWriteOld);
            AddOptionMapping(ModifyFileGroupOption.AutogrowAllFiles, CodeGenerationSupporter.AutogrowAllFiles, SqlVersionFlags.TSql130AndAbove);
            AddOptionMapping(ModifyFileGroupOption.AutogrowSingleFile, CodeGenerationSupporter.AutogrowSingleFile, SqlVersionFlags.TSql130AndAbove);
        }

        internal static readonly ModifyFilegroupOptionsHelper Instance = new ModifyFilegroupOptionsHelper();
    }
}
