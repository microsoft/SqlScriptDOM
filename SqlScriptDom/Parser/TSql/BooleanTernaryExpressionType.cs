//------------------------------------------------------------------------------
// <copyright file="TernaryExpressionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of expressions that have three expressions as children.
    /// </summary>
    [Serializable]
    public enum BooleanTernaryExpressionType
    {
        /// <summary>
        /// The between expression, example: expression BETWEEN expression AND expression
        /// </summary>
        Between = 0,
        /// <summary>
        /// The not between expression, example: expression NOT BETWEEN expression AND expression
        /// </summary>
        NotBetween = 1
    }
}
