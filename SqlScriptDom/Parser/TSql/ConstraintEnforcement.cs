//------------------------------------------------------------------------------
// <copyright file="ConstraintEnforcement.cs" company="Microsoft">
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
    /// Defines how constraints can be enforced.
    /// </summary>
    public enum ConstraintEnforcement
    {
        NotSpecified = 0,
        NoCheck = 1,
        Check = 2,
    }

#pragma warning restore 1591
}
