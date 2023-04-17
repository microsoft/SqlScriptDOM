//------------------------------------------------------------------------------
// <copyright file="BinaryExpressionType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of scalar expressions that have two expressions as children.
    /// </summary>
    [Serializable]
    public enum BinaryExpressionType
    {
        /// <summary>
        /// The '+' character, addition.
        /// </summary>
        Add = 0,
        /// <summary>
        /// The '-' character, subtraction.
        /// </summary>
        Subtract = 1,
        /// <summary>
        /// The '*' character, multiplication.
        /// </summary>
        Multiply = 2,
        /// <summary>
        /// The '/' character, division.
        /// </summary>
        Divide = 3,
        /// <summary>
        /// The '%' character, returns the integer remainder of a division.
        /// </summary>
        Modulo = 4,
        /// <summary>
        /// The '&amp;' character, bitwise and.
        /// </summary>
        BitwiseAnd = 5,
        /// <summary>
        /// The '|' character, bitwise or.
        /// </summary>
        BitwiseOr = 6,
        /// <summary>
        /// The '^' character, bitwise exclusive or.
        /// </summary>
        BitwiseXor = 7,
        /// <summary>
        /// The '&lt;&lt;' character, left shift.
        /// </summary>
        LeftShift = 8,
        /// <summary>
        /// The '>>' character, right shift.
        /// </summary>
        RightShift = 9,
        /// <summary>
        /// The '||' character, concatenation.
        /// </summary>
        Concat = 10
    }
}
