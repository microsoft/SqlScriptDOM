//------------------------------------------------------------------------------
// <copyright file="BoundingBoxParameterTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

    internal class BoundingBoxParameterTypeHelper : OptionsHelper<BoundingBoxParameterType>
    {
        private BoundingBoxParameterTypeHelper()
        {
            AddOptionMapping(BoundingBoxParameterType.XMin, CodeGenerationSupporter.XMin);
            AddOptionMapping(BoundingBoxParameterType.YMin, CodeGenerationSupporter.YMin);
            AddOptionMapping(BoundingBoxParameterType.XMax, CodeGenerationSupporter.XMax);
            AddOptionMapping(BoundingBoxParameterType.YMax, CodeGenerationSupporter.YMax);
        }
        internal static readonly BoundingBoxParameterTypeHelper Instance = new BoundingBoxParameterTypeHelper();
    }
}
