//------------------------------------------------------------------------------
// <copyright file="IdentifierGenerator.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal sealed class IdentifierGenerator : TokenGenerator
    {
        #region fields

        private String _identifier;

        #endregion

        #region ctor

        public IdentifierGenerator(String identifier)
            : this(identifier, false)
        {
        }

        public IdentifierGenerator(String identifier, Boolean appendSpace)
            : base(appendSpace)
        {
            _identifier = identifier;
        }

        #endregion

        #region overrides

        public override void Generate(ScriptWriter writer)
        {
            writer.AddIdentifierWithCasing(_identifier);
            AppendSpaceIfRequired(writer);
        }

        #endregion
    }
}
