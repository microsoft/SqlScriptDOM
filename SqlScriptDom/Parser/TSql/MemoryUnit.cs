//------------------------------------------------------------------------------
// <copyright file="memoryunits.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Memory units for file declaration options in CREATE DATABASE statement
    /// </summary>
    
    public enum MemoryUnit
    {
        Unspecified = 0,
        Percent     = 1,
        Bytes       = 2,
        KB          = 3,
        MB          = 4,
        GB          = 5,
        TB          = 6,  
        PB          = 7,      
        EB          = 8,
    }

#pragma warning restore 1591
}
