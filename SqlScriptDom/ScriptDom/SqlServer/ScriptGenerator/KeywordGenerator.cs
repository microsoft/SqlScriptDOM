//------------------------------------------------------------------------------
// <copyright file="IGenerateToken.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal sealed class KeywordGenerator : TokenGenerator
    {
        #region fields

        private TSqlTokenType _keywordId;

        #endregion

        #region ctor

        public KeywordGenerator(TSqlTokenType keywordId)
            : this(keywordId, false)
        {
        }

        public KeywordGenerator(TSqlTokenType keywordId, Boolean appendSpace)
            : base(appendSpace)
        {
            _keywordId = keywordId;
        }

        #endregion

        #region overrides

        public override void Generate(ScriptWriter writer)
        {
            writer.AddKeyword(_keywordId);
            AppendSpaceIfRequired(writer);
        }

        #endregion
    }
}
