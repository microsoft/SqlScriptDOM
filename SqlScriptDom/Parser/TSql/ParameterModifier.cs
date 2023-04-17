//------------------------------------------------------------------------------
// <copyright file="ParameterModifier.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of parameter modifier
    /// </summary>        
    public enum ParameterModifier
    {
        None        = 0,
        Output      = 1,
        ReadOnly    = 2,
    }

#pragma warning restore 1591
}
