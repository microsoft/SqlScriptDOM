//------------------------------------------------------------------------------
// <copyright file="MessageValidationMethods.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of message validation methods
    /// </summary>            
    public enum MessageValidationMethod
    {
        NotSpecified    = 0,
        None            = 1,
        Empty           = 2,
        WellFormedXml   = 3,
        ValidXml        = 4
    }

#pragma warning restore 1591
}
