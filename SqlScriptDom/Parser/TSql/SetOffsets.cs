//------------------------------------------------------------------------------
// <copyright file="SetOffsets.cs" company="Microsoft">
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
   /// The various types of keywords used in SET OFFSETS statement.
   /// </summary>
    [Flags]
    [Serializable]
    public enum SetOffsets
    {
        None                = 0x00000000,
        Select              = 0x00000001,
        From                = 0x00000002,
        Order               = 0x00000004,
        Compute             = 0x00000008,
        Table               = 0x00000010,
        Procedure           = 0x00000020,
        Execute             = 0x00000040,
        Statement           = 0x00000080,
        Param               = 0x00000100,
    }

#pragma warning restore 1591
}
