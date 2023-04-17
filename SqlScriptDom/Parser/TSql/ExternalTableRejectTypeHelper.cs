//------------------------------------------------------------------------------
// <copyright file="ExternalTableRejectTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The external table reject type mapptings.
    /// </summary>
    internal class ExternalTableRejectTypeHelper : OptionsHelper<ExternalTableRejectType>
    {
        private ExternalTableRejectTypeHelper()
        {
            AddOptionMapping(ExternalTableRejectType.Value, CodeGenerationSupporter.Value);
            AddOptionMapping(ExternalTableRejectType.Percentage, CodeGenerationSupporter.Percentage);
        }

        internal static readonly ExternalTableRejectTypeHelper Instance = new ExternalTableRejectTypeHelper();
    }
}
