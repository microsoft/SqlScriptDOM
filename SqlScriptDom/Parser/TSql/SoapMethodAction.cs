//------------------------------------------------------------------------------
// <copyright file="SoapMethodActions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of soap method actions
    /// </summary>            
    [Serializable]
    public enum SoapMethodAction
    {
        None    = 0,
        Add     = 1,
        Alter   = 2,
        Drop    = 3
    }

#pragma warning restore 1591
}
