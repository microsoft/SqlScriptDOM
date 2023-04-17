//------------------------------------------------------------------------------
// <copyright file="FunctionOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// Possible values for function options
    /// </summary>    
    public enum FunctionOptionKind
    {
        Encryption              = 0,
        SchemaBinding           = 1,
        ReturnsNullOnNullInput  = 2,
        CalledOnNullInput       = 3,
        ExecuteAs               = 4,
        NativeCompilation       = 5,
        Inline                  = 6,
    }

#pragma warning restore 1591
}
