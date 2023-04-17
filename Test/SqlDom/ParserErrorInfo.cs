//------------------------------------------------------------------------------
// <copyright file="ErrorTestElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    /// <summary>
    /// Used for error testing.
    /// </summary>
    class ParserErrorInfo
    {
        public ParserErrorInfo(int offset, string errorId)
        {
            _expectedOffset = offset;
            _errorId = errorId;
            _errorMessage = TSqlParserResource.ResourceManager.GetString(errorId + "Message");
        }

        public ParserErrorInfo(int offset, string errorId, params string []messageTokens)
        {
            _expectedOffset = offset;
            _errorId = errorId;
            _errorMessage = TSqlParserResource.ResourceManager.GetString(errorId + "Message");
            if (messageTokens.Length > 0)
                _errorMessage = string.Format(_errorMessage,messageTokens);
        }

        private int _expectedOffset;
        private string _errorId;
        private string _errorMessage;

        public void VerifyError(ParseError error)
        {
            if (!_errorId.Equals("SQL" + error.Number) || !_errorMessage.Equals(error.Message) || _expectedOffset != error.Offset)
                ParserTestUtils.LogError(error);

            Assert.AreEqual<string>(_errorId, "SQL" + error.Number);
            Assert.AreEqual<int>(_expectedOffset, error.Offset);
            Assert.AreEqual<string>(_errorMessage, error.Message);
        }
    }
}
