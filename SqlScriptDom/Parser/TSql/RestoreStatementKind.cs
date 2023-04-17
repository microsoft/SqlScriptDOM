//------------------------------------------------------------------------------
// <copyright file="RestoreStatementKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of restore statements
    /// </summary>            
    [Serializable]
    public enum RestoreStatementKind
    {
        None            = 0,
        Database        = 1,
        TransactionLog  = 2,
        FileListOnly    = 3,
        VerifyOnly      = 4,
        LabelOnly       = 5,
        RewindOnly      = 6,
        HeaderOnly      = 7
    }

#pragma warning restore 1591
}
