//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.ScriptWriterElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// A mark interface
    /// </summary>
    internal partial class ScriptWriter
    {
        internal abstract class ScriptWriterElement
        {
            public ScriptWriterElementType ElementType { protected set; get; }
        }
    }
}
