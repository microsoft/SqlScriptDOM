//------------------------------------------------------------------------------
// <copyright file="RdaTableOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of RDA table option
    /// </summary>        
    public enum RdaTableOption
    {
        Disable                 = 0,
        Enable                  = 1,
        OffWithoutDataRecovery  = 2
    }

#pragma warning restore 1591
}
