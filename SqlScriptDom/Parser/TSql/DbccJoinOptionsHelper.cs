//------------------------------------------------------------------------------
// <copyright file="DbccJoinOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class DbccJoinOptionsHelper : OptionsHelper<DbccOptionKind>
    {
        private DbccJoinOptionsHelper()
        {
            AddOptionMapping(DbccOptionKind.StatHeader, CodeGenerationSupporter.StatHeader);
            AddOptionMapping(DbccOptionKind.DensityVector, CodeGenerationSupporter.DensityVector);
        }

        internal static readonly DbccJoinOptionsHelper Instance = new DbccJoinOptionsHelper();
    }
}
