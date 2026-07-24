//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorOptions.LeadingComma.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class SqlScriptGeneratorOptions
    {
        /// <summary>
        /// The number of whitespace characters written after a leading comma when
        /// <see cref="CommaPlacement"/> is <see cref="CommaPlacement.Leading"/>.
        /// The total width reserved for a leading comma is one column for the comma itself
        /// plus this many columns of trailing whitespace (so the default of 1 reserves 2 columns).
        /// </summary>
        /// <remarks>
        /// This is currently an internal knob so the leading-comma spacing is defined in a single
        /// place rather than hard-coded at every call site. It can later be promoted to a public
        /// script generation option.
        /// </remarks>
        internal Int32 LeadingCommaSpaceCount { get; set; } = 1;
    }
}
