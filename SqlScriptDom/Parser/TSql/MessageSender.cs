//------------------------------------------------------------------------------
// <copyright file="MessageSenders.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of message senders 
    /// </summary>            
    public enum MessageSender
    {
        None        = 0,
        Initiator   = 1,
        Target      = 2,
        Any         = 3
    }

#pragma warning restore 1591
}
