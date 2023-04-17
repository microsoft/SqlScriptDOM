//------------------------------------------------------------------------------
// <copyright file="SessionOptionUnitHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Handles conversion from/to MemoryUnits enum
    /// </summary>
    internal class SessionOptionUnitHelper : OptionsHelper<MemoryUnit>
    {
        private SessionOptionUnitHelper()
        {
            AddOptionMapping(MemoryUnit.KB, CodeGenerationSupporter.KB);
            AddOptionMapping(MemoryUnit.MB, CodeGenerationSupporter.MB);
        }

        internal static readonly SessionOptionUnitHelper Instance = new SessionOptionUnitHelper();
    }
}
