//------------------------------------------------------------------------------
// <copyright file="servicebrokeroptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible service broker options for CREATE DATABASE statement
    /// </summary>
    [Serializable]
    public enum ServiceBrokerOption
    {
        None = 0,
        EnableBroker = 1,
        NewBroker = 2,
        ErrorBrokerConversations = 3
    }

#pragma warning restore 1591
}
