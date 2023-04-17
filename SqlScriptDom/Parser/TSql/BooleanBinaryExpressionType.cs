//------------------------------------------------------------------------------
// <copyright file="BooleanBinaryExpressionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of boolean expressions that have two expressions as children.
    /// </summary>
    [Serializable]
    public enum BooleanBinaryExpressionType
    {
        /// <summary>
        /// The AND keyword, boolean and operation
        /// </summary>
        And = 0,
        /// <summary>
        /// The OR keyword, boolean or operation
        /// </summary>
        Or = 1
    }
}
