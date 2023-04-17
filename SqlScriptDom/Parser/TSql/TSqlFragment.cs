//------------------------------------------------------------------------------
// <copyright file="TSqlFragment.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This class is a real internal class to parser, 
    /// therefore it is not in the spec tree.
    /// </summary>
    
    [Serializable]
    public abstract partial class TSqlFragment
    {
        /// <summary>
        /// Updates token offsets information; if the offsets are uninitialized, 
        /// it sets them, otherwise it updates them.
        /// </summary> 
        /// <param name="fragment">The fragment that is used to set this fragment.</param>
        internal void UpdateTokenInfo(TSqlFragment fragment)
        {
            // It is possible that the fragment is null.
            if (fragment == null)
                return;

            UpdateTokenInfo(fragment.FirstTokenIndex, fragment.LastTokenIndex);
            if (fragment.ScriptTokenStream != null)
            {
                ScriptTokenStream = fragment.ScriptTokenStream;
            }
        }

        /// <summary>
        /// The function that updates token indicies
        /// </summary>
        /// <param name="firstIndex">The new first token index.</param>
        /// <param name="lastIndex">The new last token index.</param>
        internal void UpdateTokenInfo(int firstIndex, int lastIndex)
        {
            // Disregard invalid values
            if (firstIndex < 0 || lastIndex < 0)
                return;

            if (firstIndex > lastIndex)
            {
                int tmp = firstIndex;
                firstIndex = lastIndex;
                lastIndex = tmp;
            }

            if (firstIndex < _firstTokenIndex || _firstTokenIndex == Uninitialized)
                _firstTokenIndex = firstIndex;

            if (lastIndex > _lastTokenIndex || _lastTokenIndex == Uninitialized)
                _lastTokenIndex = lastIndex;
        }

        /// <summary>
        /// Accepts the specified visitor.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void Accept(TSqlFragmentVisitor visitor)
        {
            // Intentionally left empty.
        }

        /// <summary>
        /// Accepts specified visitor on the children.
        /// </summary>
        /// <param name="visitor">The visitor.</param>
        public virtual void AcceptChildren(TSqlFragmentVisitor visitor)
        {
            // Intentionally left empty.
        }


        /// <summary>
        /// Defines the character offset of fragments starting location in the script it was parsed.
        /// </summary>
        /// <value></value>
        public int StartOffset
        {
            get
            {
                // Should we also check if _firstTokenIndex is less than token stream length?
                if (_firstTokenIndex == Uninitialized || _scriptTokenStream == null)
                    return Uninitialized;

                return _scriptTokenStream[_firstTokenIndex].Offset;
            }
        }

        /// <summary>
        /// Defines the number of characters the fragment takes up in the script it was parsed.
        /// </summary>
        /// <value></value>
        public int FragmentLength
        {
            get
            {
                // Should we also check if _firstTokenIndex/_lastTokenIndex are less than token stream length?
                if (_firstTokenIndex == Uninitialized || _lastTokenIndex == Uninitialized || _scriptTokenStream == null)
                    return Uninitialized;

                TSqlParserToken lastToken = _scriptTokenStream[_lastTokenIndex];
                int lastTokenLength = lastToken.Text == null ? 0 : lastToken.Text.Length;
                return lastToken.Offset - _scriptTokenStream[_firstTokenIndex].Offset + lastTokenLength;
            }
        }

        /// <summary>
        /// Gets the start line.
        /// </summary>
        /// <value>The start line.</value>
        public int StartLine
        {
            get
            {
                // Should we also check if _firstTokenIndex is less than token stream length?
                if (_firstTokenIndex == Uninitialized || _scriptTokenStream == null)
                    return Uninitialized;

                return _scriptTokenStream[_firstTokenIndex].Line;
            }
        }

        /// <summary>
        /// Gets the start column.
        /// </summary>
        /// <value>The start column.</value>
        public int StartColumn
        {
            get
            {
                // Should we also check if _firstTokenIndex is less than token stream length?
                if (_firstTokenIndex == Uninitialized || _scriptTokenStream == null)
                    return Uninitialized;

                return _scriptTokenStream[_firstTokenIndex].Column;
            }
        }

        /// <summary>
        /// Gets or sets the first index of the token.
        /// </summary>
        /// <value>The first index of the token.</value>
        public int FirstTokenIndex
        {
            get { return _firstTokenIndex; }
            set { _firstTokenIndex = value; }
        }

        /// <summary>
        /// Gets or sets the last index of the token.
        /// </summary>
        /// <value>The last index of the token.</value>
        public int LastTokenIndex
        {
            get { return _lastTokenIndex; }
            set { _lastTokenIndex = value; }
        }

        /// <summary>
        /// Gets or sets the script token stream.
        /// </summary>
        /// <value>The script token stream.</value>
        public IList<TSqlParserToken> ScriptTokenStream
        {
            get { return _scriptTokenStream; }
            set { _scriptTokenStream = value; }
        }

        /// <summary>
        /// Constant to indicate and uninitialized token.
        /// </summary>
        public const int Uninitialized = -1;

        private int _firstTokenIndex = Uninitialized;
        private int _lastTokenIndex = Uninitialized;
        IList<TSqlParserToken> _scriptTokenStream = null;
    }
}
