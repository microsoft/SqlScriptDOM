//------------------------------------------------------------------------------
// <copyright file="GenerateAlwaysTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
   internal class GenerateAlwaysTypeHelper : OptionsHelper<GeneratedAlwaysType>
    {
        private GenerateAlwaysTypeHelper()
        {
            AddOptionMapping(GeneratedAlwaysType.RowStart, CodeGenerationSupporter.Start);
            AddOptionMapping(GeneratedAlwaysType.RowEnd, CodeGenerationSupporter.End);
        }

        internal static readonly GenerateAlwaysTypeHelper Instance = new GenerateAlwaysTypeHelper();
    }
}
