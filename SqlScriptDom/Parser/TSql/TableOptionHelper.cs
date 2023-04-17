//------------------------------------------------------------------------------
// <copyright file="TableOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The external table optional property mapptings.
    /// </summary>
    internal class TableOptionHelper : OptionsHelper<TableOptionKind>
    {
        private TableOptionHelper()
        {
            AddOptionMapping(TableOptionKind.Distribution, CodeGenerationSupporter.Distribution);
            AddOptionMapping(TableOptionKind.Partition, CodeGenerationSupporter.Partition);
        }

        internal static readonly TableOptionHelper Instance = new TableOptionHelper();
    }
}