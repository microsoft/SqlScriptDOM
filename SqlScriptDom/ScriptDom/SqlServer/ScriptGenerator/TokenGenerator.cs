//------------------------------------------------------------------------------
// <copyright file="TokenGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal abstract class TokenGenerator
    {
        private Boolean _appendSpace;

        public TokenGenerator(Boolean appendSpace)
        {
            _appendSpace = appendSpace;
        }

        protected void AppendSpaceIfRequired(ScriptWriter writer)
        {
            if (this._appendSpace)
            {
                writer.AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(1));
            }
        }

        abstract public void Generate(ScriptWriter writer);
    }
}
