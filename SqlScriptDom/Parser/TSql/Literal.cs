//------------------------------------------------------------------------------
// <copyright file="Literal.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class Literal
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public abstract LiteralType LiteralType
        { get; }
    }

    public partial class IntegerLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Integer; }
        }
    }

    public partial class NumericLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Numeric; }
        }
    }

    public partial class RealLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Real; }
        }
    }

    public partial class MoneyLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Money; }
        }
    }

    public partial class BinaryLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Binary; }
        }
    }

    public partial class StringLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.String; }
        }
    }

    public partial class NullLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Null; }
        }
    }

    public partial class DefaultLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Default; }
        }
    }

    public partial class MaxLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Max; }
        }
    }

    public partial class OdbcLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Odbc; }
        }
    }

    public partial class IdentifierLiteral
    {
        /// <summary>
        /// Represents the type of the literal.
        /// </summary>
        public override LiteralType LiteralType
        {
            get { return ScriptDom.LiteralType.Identifier; }
        }
    }
}
