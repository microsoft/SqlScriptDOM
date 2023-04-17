//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.FormatableToken.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class ScriptWriter
    {
        /// <summary>
        /// the wrapper class for a TSql Token
        /// </summary>
        [DebuggerDisplay("Token({_token.Text})")]
        internal class TokenWrapper : ScriptWriterElement
        {
            private TSqlParserToken _token;

            public TokenWrapper(TSqlParserToken token)
            {
                _token = token;
                this.ElementType = ScriptWriterElementType.Token;
            }

            public TSqlParserToken Token
            {
                get { return _token; }
            }
        }
    }
}
