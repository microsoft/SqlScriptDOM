//------------------------------------------------------------------------------
// <copyright file="attachmode.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of attach mode
    /// </summary>   
    public enum AttachMode
    {
        None = 0,
        Attach = 1,
        AttachRebuildLog = 2,
        AttachForceRebuildLog = 3,
        Load = 4,
    }


#pragma warning restore 1591
}
