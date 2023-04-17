//------------------------------------------------------------------------------
// <copyright file="SoapMethodSchemas.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of soap method schema
    /// </summary>            
    [Serializable]
    public enum SoapMethodSchemas
    {
        NotSpecified    = 0,
        None            = 1,
        Standard        = 2,
        Default         = 3
    }

#pragma warning restore 1591
}
