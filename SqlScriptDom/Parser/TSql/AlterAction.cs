//------------------------------------------------------------------------------
// <copyright file="AlterAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter action
    /// </summary>           
    public enum AlterAction
    {
        None    = 0,
        Add     = 1,
        Drop    = 2
    }

#pragma warning restore 1591
}
