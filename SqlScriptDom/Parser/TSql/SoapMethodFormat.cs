//------------------------------------------------------------------------------
// <copyright file="soapmethodformats.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible soap method formats
    /// </summary>            
    [Serializable]
    public enum SoapMethodFormat
    {
        NotSpecified    = 0,
        AllResults      = 1,
        RowsetsOnly     = 2,
        None            = 3
    }

#pragma warning restore 1591
}
