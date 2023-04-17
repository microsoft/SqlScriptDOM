//------------------------------------------------------------------------------
// <copyright file="BulkInsertIntOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class BulkInsertIntOptionsHelper : OptionsHelper<BulkInsertOptionKind>
    {
        private BulkInsertIntOptionsHelper()
        {
            AddOptionMapping(BulkInsertOptionKind.MaxErrors, CodeGenerationSupporter.MaxErrors);
            AddOptionMapping(BulkInsertOptionKind.FirstRow, CodeGenerationSupporter.FirstRow);
            AddOptionMapping(BulkInsertOptionKind.LastRow, CodeGenerationSupporter.LastRow);
            AddOptionMapping(BulkInsertOptionKind.BatchSize, CodeGenerationSupporter.BatchSize);
            AddOptionMapping(BulkInsertOptionKind.CodePage, CodeGenerationSupporter.CodePage);
            AddOptionMapping(BulkInsertOptionKind.RowsPerBatch, CodeGenerationSupporter.RowsPerBatch);
            AddOptionMapping(BulkInsertOptionKind.KilobytesPerBatch, CodeGenerationSupporter.KilobytesPerBatch);
        }

        internal static readonly BulkInsertIntOptionsHelper Instance = new BulkInsertIntOptionsHelper();
    }
}
