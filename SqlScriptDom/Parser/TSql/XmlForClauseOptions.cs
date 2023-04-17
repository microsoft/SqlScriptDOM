//------------------------------------------------------------------------------
// <copyright file="XmlForClauseOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591

    /// <summary>
    /// Enum to store different XML for clause options
    /// </summary>
    [Flags]
    
    [Serializable]
    public enum XmlForClauseOptions
    {
        None            = 0x0000,
           // Modes
        Raw             = 0x0001,
        Auto            = 0x0002,
        Explicit        = 0x0004,
        Path            = 0x0008,

           // All other options
        XmlData         = 0x0010,
        XmlSchema       = 0x0020,
        Elements        = 0x0040,
        ElementsXsiNil  = 0x0080,
        ElementsAbsent  = 0x0100,
        BinaryBase64    = 0x0200,
        Type            = 0x0400,
        Root            = 0x0800,

        ElementsAll     = Elements | ElementsXsiNil | ElementsAbsent,
    }

#pragma warning restore 1591

}
