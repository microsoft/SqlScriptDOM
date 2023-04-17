//------------------------------------------------------------------------------
// <copyright file="PageVerifyDbOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class PageVerifyDbOptionsHelper : OptionsHelper<PageVerifyDatabaseOptionKind>
    {
        private PageVerifyDbOptionsHelper()
        {
            AddOptionMapping(PageVerifyDatabaseOptionKind.None, CodeGenerationSupporter.None);
            AddOptionMapping(PageVerifyDatabaseOptionKind.Checksum, CodeGenerationSupporter.Checksum);
            AddOptionMapping(PageVerifyDatabaseOptionKind.TornPageDetection, CodeGenerationSupporter.TornPageDetection);
        }

        internal static readonly PageVerifyDbOptionsHelper Instance = new PageVerifyDbOptionsHelper();
    }
}
