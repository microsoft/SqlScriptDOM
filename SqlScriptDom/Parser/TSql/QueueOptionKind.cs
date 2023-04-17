//------------------------------------------------------------------------------
// <copyright file="QueueOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The possible queue options.
    /// </summary>
    [Serializable]
    public enum QueueOptionKind
    {
        Status = 0,
        Retention = 1,
        ActivationStatus = 2,
        ActivationProcedureName = 3,
        ActivationMaxQueueReaders = 4,
        ActivationExecuteAs = 5,
        ActivationDrop = 6,
        PoisonMessageHandlingStatus = 7,
    }

#pragma warning restore 1591
}
