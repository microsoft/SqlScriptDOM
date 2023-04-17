//------------------------------------------------------------------------------
// <copyright file="BulkInsertFlagOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class BulkInsertFlagOptionsHelper : OptionsHelper<BulkInsertOptionKind>
    {
        private BulkInsertFlagOptionsHelper()
        {
            AddOptionMapping(BulkInsertOptionKind.NoTriggers, CodeGenerationSupporter.NoTriggers);
            AddOptionMapping(BulkInsertOptionKind.KeepIdentity, CodeGenerationSupporter.KeepIdentity);
            AddOptionMapping(BulkInsertOptionKind.KeepNulls, CodeGenerationSupporter.KeepNulls);
            AddOptionMapping(BulkInsertOptionKind.TabLock, CodeGenerationSupporter.TabLock);
            AddOptionMapping(BulkInsertOptionKind.CheckConstraints, CodeGenerationSupporter.CheckConstraints);
            AddOptionMapping(BulkInsertOptionKind.FireTriggers, CodeGenerationSupporter.FireTriggers);
            AddOptionMapping(BulkInsertOptionKind.IncludeHidden, CodeGenerationSupporter.IncludeHidden);
        }

        internal static readonly BulkInsertFlagOptionsHelper Instance = new BulkInsertFlagOptionsHelper();
    }
}
