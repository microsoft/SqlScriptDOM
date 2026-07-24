//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorOptions.Indentation.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class SqlScriptGeneratorOptions
    {
        /// <summary>
        /// Whether indentation should be emitted using tab characters for the current option set.
        /// This is true only when <see cref="IndentationMode"/> is <see cref="IndentationMode.Tabs"/>
        /// and <see cref="CommaPlacement"/> is not <see cref="CommaPlacement.Leading"/>.
        /// </summary>
        internal Boolean UseTabsForIndentation
        {
            get
            {
                return IndentationMode == IndentationMode.Tabs
                    && CommaPlacement != CommaPlacement.Leading;
            }
        }
    }
}
