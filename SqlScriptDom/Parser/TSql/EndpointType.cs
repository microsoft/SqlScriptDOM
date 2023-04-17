//------------------------------------------------------------------------------
// <copyright file="EndpointTypes.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of endpoint
    /// </summary>        
    public enum EndpointType
    {
        NotSpecified        = 0,
        Soap                = 1,
        TSql                = 2,
        ServiceBroker       = 3,
        DatabaseMirroring   = 4,
    }

#pragma warning restore 1591
}
