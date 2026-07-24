//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.RightAlignedSeparatorElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class ScriptWriter
    {
        /// <summary>
        /// A run of tokens (e.g. a leading comma and a space) that must be right-aligned so
        /// that it ends exactly at the offset of the alignment point that immediately follows
        /// it. This is used for leading comma placement, where the comma should sit just before
        /// the aligned item column rather than at the start of the line.
        /// </summary>
        internal class RightAlignedSeparatorElement : ScriptWriterElement
        {
            private readonly List<TSqlParserToken> _tokens;
            private readonly int _width;

            public RightAlignedSeparatorElement(List<TSqlParserToken> tokens)
            {
                _tokens = tokens;
                _width = 0;
                foreach (TSqlParserToken token in tokens)
                {
                    if (token != null && token.Text != null)
                    {
                        _width += token.Text.Length;
                    }
                }

                this.ElementType = ScriptWriterElementType.RightAlignedSeparator;
            }

            public List<TSqlParserToken> Tokens
            {
                get { return _tokens; }
            }

            public int Width
            {
                get { return _width; }
            }
        }
    }
}
