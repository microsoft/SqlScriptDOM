//------------------------------------------------------------------------------
// <copyright file="DataCompressionLevelHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class DataCompressionLevelHelper : OptionsHelper<DataCompressionLevel>
    {
        private DataCompressionLevelHelper()
        {
            AddOptionMapping(DataCompressionLevel.None, CodeGenerationSupporter.None);
            AddOptionMapping(DataCompressionLevel.Page, CodeGenerationSupporter.Page);
            AddOptionMapping(DataCompressionLevel.Row, CodeGenerationSupporter.Row);			
            AddOptionMapping(DataCompressionLevel.ColumnStore, CodeGenerationSupporter.ColumnStore);
            AddOptionMapping(DataCompressionLevel.ColumnStoreArchive, CodeGenerationSupporter.ColumnStoreArchive);
        }

        public static readonly DataCompressionLevelHelper Instance = new DataCompressionLevelHelper();
    }
}
