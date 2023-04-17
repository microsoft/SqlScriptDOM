//------------------------------------------------------------------------------
// <copyright file="EventSessionEventRetentionModeType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of event retention modes
    /// </summary>        
    [Serializable]
    public enum EventSessionEventRetentionModeType
    {
        Unknown = 0,
        AllowSingleEventLoss = 1,
        AllowMultipleEventLoss = 2,
        NoEventLoss = 3,
    }

#pragma warning restore 1591
}
