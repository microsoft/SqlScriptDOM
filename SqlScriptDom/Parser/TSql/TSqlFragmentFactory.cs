//------------------------------------------------------------------------------
// <copyright file="TSqlFragmentFactory.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This class is a fragment factory capable of instantiating the fragments that
    /// are required by the TSql parser.
    /// </summary>

    
    [Serializable]
    internal class TSqlFragmentFactory
    {
        private IList<TSqlParserToken> _tokenStream = null;

        public void SetTokenStream(IList<TSqlParserToken> tokenStream)
        {
            _tokenStream = tokenStream;
        }

        public virtual FragmentType CreateFragment<FragmentType>() where FragmentType : TSqlFragment, new()
        {
            FragmentType fragment = new FragmentType();
            fragment.ScriptTokenStream = _tokenStream;
            return fragment;
        }
    }
}
