//------------------------------------------------------------------------------
// <copyright file="memoryunitshelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Handles conversion from/to MemoryUnits enum
    /// </summary>
    internal class MemoryUnitsHelper : OptionsHelper<MemoryUnit>
    {
        private MemoryUnitsHelper()
        {
            AddOptionMapping(MemoryUnit.KB, CodeGenerationSupporter.KB);
            AddOptionMapping(MemoryUnit.MB, CodeGenerationSupporter.MB);
            AddOptionMapping(MemoryUnit.GB, CodeGenerationSupporter.GB);
            AddOptionMapping(MemoryUnit.TB, CodeGenerationSupporter.TB);
            AddOptionMapping(MemoryUnit.Percent, TSqlTokenType.PercentSign);
        }

        internal static readonly MemoryUnitsHelper Instance = new MemoryUnitsHelper();
 
    }
}
