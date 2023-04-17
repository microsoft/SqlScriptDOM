//------------------------------------------------------------------------------
// <copyright file="GridParameterTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class GridParameterTypeHelper : OptionsHelper<GridParameterType>
    {
        private GridParameterTypeHelper()
        {
            AddOptionMapping(GridParameterType.Level1, CodeGenerationSupporter.Level1);
            AddOptionMapping(GridParameterType.Level2, CodeGenerationSupporter.Level2);
            AddOptionMapping(GridParameterType.Level3, CodeGenerationSupporter.Level3);
            AddOptionMapping(GridParameterType.Level4, CodeGenerationSupporter.Level4);
        }
        internal static readonly GridParameterTypeHelper Instance = new GridParameterTypeHelper();
    }
}
