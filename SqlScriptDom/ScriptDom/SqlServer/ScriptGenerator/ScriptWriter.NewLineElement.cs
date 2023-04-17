//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class ScriptWriter
    {
        /// <summary>
        /// Represent a new line in token streams
        /// </summary>

        [DebuggerDisplay("NewLine==========")]
        internal class NewLineElement : ScriptWriterElement
        {
            public NewLineElement()
            {
                this.ElementType = ScriptWriterElementType.NewLine;
            }
        }
    }
}
