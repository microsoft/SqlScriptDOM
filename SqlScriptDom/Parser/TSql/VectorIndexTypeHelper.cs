//------------------------------------------------------------------------------
// <copyright file="VectorIndexTypeHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class VectorIndexTypeHelper : OptionsHelper<VectorIndexType>
    {
        private VectorIndexTypeHelper()
        {
            AddOptionMapping(VectorIndexType.DiskANN, "'" + CodeGenerationSupporter.DiskANN + "'");
        }

        public static readonly VectorIndexTypeHelper Instance = new VectorIndexTypeHelper();
    }
}