//------------------------------------------------------------------------------
// <copyright file="OpenRowsetBulkHintOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal partial class OpenRowsetBulkHintOptionsHelper : OptionsHelper<BulkInsertOptionKind>
    {
        private OpenRowsetBulkHintOptionsHelper()
        {
            AddOptionMapping(BulkInsertOptionKind.SingleBlob, CodeGenerationSupporter.SingleBlob);
            AddOptionMapping(BulkInsertOptionKind.SingleClob, CodeGenerationSupporter.SingleClob);
            AddOptionMapping(BulkInsertOptionKind.SingleNClob, CodeGenerationSupporter.SingleNClob);
        }

        internal static readonly OpenRowsetBulkHintOptionsHelper Instance = new OpenRowsetBulkHintOptionsHelper();
    }
}
