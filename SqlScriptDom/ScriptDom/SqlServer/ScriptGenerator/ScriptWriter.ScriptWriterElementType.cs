//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class ScriptWriter
    {
        internal enum ScriptWriterElementType
        {
            AlignmentPoint,
            Token,
            NewLine,
        }
    }
}
