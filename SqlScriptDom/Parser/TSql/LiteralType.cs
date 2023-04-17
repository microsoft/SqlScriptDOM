//------------------------------------------------------------------------------
// <copyright file="LiteralType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The literal types.
    /// </summary>
    public enum LiteralType
    {
        /// <summary>
        /// The integer type is a sequence of digits without '.' or 'e'
        /// </summary>
        Integer = 0,
        /// <summary>
        /// A sequence of digits that has an e
        /// </summary>
        Real = 1,
        /// <summary>
        /// Represented as string of digits with an optional decimal point.
        /// Has to start with a currency symbol.
        /// </summary>
        Money = 2,
        /// <summary>
        /// Hexadecimal numbers, they have to be prefixed with 0x.
        /// </summary>
        Binary = 3,
        /// <summary>
        /// A string of characters that are delimited with the quote (') character
        /// If the QUOTED_IDENTIFIER is off the delimiter (") is also valid.
        /// </summary>
        String = 4,
        /// <summary>
        /// The reserved word null.
        /// </summary>
        Null = 5,
        /// <summary>
        /// DEFAULT keyword is used.
        /// </summary>
        Default = 6,
        /// <summary>
        /// MAX context sensitive keyword is used.
        /// </summary>
        Max = 7,
        /// <summary>
        /// Odbc format literals in curly braces { }.
        /// </summary>
        Odbc = 8,
        /// <summary>
        /// An Identifier that is treated like a Literal, such as a parameter default
        /// </summary>
        Identifier = 9,
        /// <summary>
        /// A sequence of numbers with a '.'
        /// </summary>
        Numeric = 10
    }
}
