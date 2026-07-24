//------------------------------------------------------------------------------
// <copyright file="ColumnAliasStyle.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Represents the possible ways of rendering column aliases in SELECT projections
    /// </summary>
    public enum ColumnAliasStyle
    {
        /// <summary>
        /// Render column aliases using the AS keyword: expression AS alias
        /// </summary>
        AsKeyword,

        /// <summary>
        /// Render column aliases using the equals sign: alias = expression
        /// </summary>
        EqualsSign,

        /// <summary>
        /// Preserve the column alias style from the original script without converting between styles
        /// </summary>
        Preserve
    }
}
