//#define VISIBLE_WHITESPACE
//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    /// <summary>
    /// Writes tokens to a token stream which can then be outputted to text
    /// </summary>
    internal partial class ScriptWriter
    {
        #region private static members

        private static NewLineElement _newLine = new NewLineElement();
        private static TSqlParserToken _newLineToken = new TSqlParserToken(TSqlTokenType.WhiteSpace, Environment.NewLine);

        #endregion

        #region Private Fields

        //private Int32 _indentation; // number of white space characters to be inserted for indentation
        private SqlScriptGeneratorOptions _options;
        private Dictionary<AlignmentPoint, AlignmentPointData> _alignmentPointDataMap; // AlignmentPoints to their AlignmentPointData for all alignment points
        private Dictionary<String, AlignmentPoint> _alignmentPointNameMapForCurrentScope; // Name to AlignmentPoints for all pushes
        private Stack<Dictionary<String, AlignmentPoint>> _alignmentPointNameMaps; // Name to AlignmentPoints for the alignment points found after the nearest push
        private List<ScriptWriterElement> _scriptWriterElements;
        private Stack<AlignmentPoint> _newLineAlignmentPoints;

        #endregion

        #region Public Constructors

        public ScriptWriter(SqlScriptGeneratorOptions options)
        {
            _options = options;
            _alignmentPointDataMap = new Dictionary<AlignmentPoint, AlignmentPointData>();
            _alignmentPointNameMapForCurrentScope = new Dictionary<String, AlignmentPoint>();
            _alignmentPointNameMaps = new Stack<Dictionary<String, AlignmentPoint>>();
            _scriptWriterElements = new List<ScriptWriterElement>();
            _newLineAlignmentPoints = new Stack<AlignmentPoint>();
        }

        #endregion

        #region public methods

        public void AddKeyword(TSqlTokenType keywordId)
        {
            String text = ScriptGeneratorSupporter.GetTokenString(keywordId, _options.KeywordCasing);
            TSqlParserToken token = new TSqlParserToken(keywordId, text);
            AddToken(token);
        }

        public void AddIdentifierWithCasing(String text)
        {
            ScriptGeneratorSupporter.CheckForNullReference(text, "text");
            AddIdentifier(text, true);
        }

        public void AddIdentifierWithoutCasing(String text)
        {
            ScriptGeneratorSupporter.CheckForNullReference(text, "text");
            AddIdentifier(text, false);
        }

        public void AddToken(TSqlParserToken token)
        {
            ScriptGeneratorSupporter.CheckForNullReference(token, "token");
            AddTokenWrapper(new TokenWrapper(token));
        }

        public void NewLine()
        {
            AddNewLine();

            // if we have some AlignmentPoints on stack, we set to the top one
            if (_newLineAlignmentPoints.Count > 0)
            {
                Mark(_newLineAlignmentPoints.Peek());
            }
        }

        public void Indent(Int32 size)
        {
            AddSpace(size);
        }

        public void Mark(AlignmentPoint ap)
        {
            if (String.IsNullOrEmpty(ap.Name) == false &&
                _alignmentPointNameMapForCurrentScope.ContainsKey(ap.Name) == false)
            {
                _alignmentPointNameMapForCurrentScope.Add(ap.Name, ap);
            }
            AddAlignmentPoint(ap);
        }

        public void PushNewLineAlignmentPoint(AlignmentPoint ap)
        {
            _newLineAlignmentPoints.Push(ap);
            _alignmentPointNameMaps.Push(_alignmentPointNameMapForCurrentScope);
            _alignmentPointNameMapForCurrentScope = new Dictionary<String, AlignmentPoint>();
        }

        public void PopNewLineAlignmentPoint()
        {
            _newLineAlignmentPoints.Pop();
            _alignmentPointNameMapForCurrentScope = _alignmentPointNameMaps.Pop();
        }

        public AlignmentPoint FindOrCreateAlignmentPoint(String name)
        {
            AlignmentPoint ap = null;

            if (_alignmentPointNameMapForCurrentScope.TryGetValue(name, out ap) == false)
            {
                // may not be necessary, just want to make it explicit
                ap = null;
            }

            if (ap == null)
            {
                ap = new AlignmentPoint(name);
            }

            return ap;
        }

        /// <summary>
        /// Writes the textual contents of this script writer to the specified text writer
        /// </summary>
        /// <remarks>This method calls Dispose after completing to dispose of the script writer</remarks>
        /// <param name="writer">The text writer to write contents to</param>
        public void WriteTo(TextWriter writer)
        {
            List<TSqlParserToken> alignedTokens = TryGetAlignedTokens();
            foreach (TSqlParserToken token in alignedTokens)
            {
                writer.Write(token.Text);
            }
            writer.Flush();
        }

        /// <summary>
        /// Writes the tokens in this script writer to the specified list
        /// </summary>
        /// <remarks>This method calls Dispose after completing to dispose of the script writer</remarks>
        /// <param name="tokens">A list of tokens to write the contents of this writer to</param>
        public void WriteTo(IList<TSqlParserToken> tokens)
        {
            List<TSqlParserToken> alignedTokens = TryGetAlignedTokens();
            foreach (TSqlParserToken token in alignedTokens)
            {
                tokens.Add(token);
            }
        }

        #endregion

        #region supporting methods

        private void AddIdentifier(String text, Boolean applyCasing)
        {
            if (applyCasing)
            {
                // Apply keyword casing (for identifiers that are actually keywords)
                text = ScriptGeneratorSupporter.GetCasedString(text, _options.KeywordCasing);
            }
            else
            {
                // Apply identifier casing (for actual identifiers)
                switch (_options.IdentifierCasing)
                {
                    case IdentifierCasing.Lowercase:
                        text = text.ToLowerInvariant();
                        break;
                    case IdentifierCasing.Uppercase:
                        text = text.ToUpperInvariant();
                        break;
                    case IdentifierCasing.PascalCase:
                        text = ScriptGeneratorSupporter.GetPascalCase(text);
                        break;
                    case IdentifierCasing.PreserveOriginal:
                    default:
                        // No transformation - preserve original casing
                        break;
                }
            }

            TSqlParserToken token = new TSqlParserToken(TSqlTokenType.Identifier, text);
            AddToken(token);
        }

        private void AddSpace(int count)
        {
            AddToken(ScriptGeneratorSupporter.CreateWhitespaceToken(count));
        }

        private void AddTokenWrapper(TokenWrapper token)
        {
            _scriptWriterElements.Add(token);
        }

#if DEBUG
        HashSet<AlignmentPoint> _alignmentPointsForCurrentLine = new HashSet<AlignmentPoint>();
#endif

        private void AddAlignmentPoint(AlignmentPoint ap)
        {
#if DEBUG
            if (_alignmentPointsForCurrentLine.Contains(ap))
            {
                Debug.Assert(false, "Duplicated alignment points found in the same line");
            }
            _alignmentPointsForCurrentLine.Add(ap);
#endif
            _scriptWriterElements.Add(FindOrCreateAlignmentPointData(ap));
        }

        private void AddNewLine()
        {
#if DEBUG
            _alignmentPointsForCurrentLine.Clear();
#endif
            _scriptWriterElements.Add(_newLine);
        }

        private ScriptWriterElement FindOrCreateAlignmentPointData(AlignmentPoint ap)
        {
            AlignmentPointData apd;
            if (_alignmentPointDataMap.TryGetValue(ap, out apd) == false)
            {
                apd = new AlignmentPointData(ap.Name);
                _alignmentPointDataMap.Add(ap, apd);
            }

            return apd;
        }

        // try to return the aligned tokens
        // return unaligned tokens if failing aligning the tokens
        private List<TSqlParserToken> TryGetAlignedTokens()
        {
            List<TSqlParserToken> result = Align();

            if (result == null)
                result = GetAllTokens();

            return result;
        }

        private List<TSqlParserToken> Align()
        {
            // keep all alignment points 
            HashSet<AlignmentPointData> allPoints = new HashSet<AlignmentPointData>();

            // find out the width for each alignment point and populate relationship among alignment points 

            Int32 width = 0; // keep the width between two alignment points
            AlignmentPointData previousPoint = null;
            for (Int32 index = 0; index < _scriptWriterElements.Count; ++index)
            {
                ScriptWriterElement element = _scriptWriterElements[index];

                switch (element.ElementType)
                {
                    case ScriptWriterElementType.AlignmentPoint:
                        AlignmentPointData ap = element as AlignmentPointData;
#if !PIMODLANGUAGE
                        Debug.Assert(ap != null, "AlignmentPointData is expected");
#endif
                        allPoints.Add(ap);

                        if (previousPoint != null)
                        {
                            // this is not the first alignment point of the current line, so establish the relationships
                            ap.AddLeftPoint(previousPoint, width);
                            previousPoint.AddRightPoint(ap);
                        }
                        else
                        {
                            // this is the first alignment point of the current line, so the width is also its offset
                            ap.Offset = Math.Max(ap.Offset, width);
                        }

                        width = 0;
                        previousPoint = ap;
                        break;
                    case ScriptWriterElementType.Token:
                        TokenWrapper tokenWrapper = element as TokenWrapper;
                        Debug.Assert(tokenWrapper != null, "TokenWrapper is expected");
                        Debug.Assert(tokenWrapper.Token.Text != null, "TokenWrapper.Token.Text should not be null");
                        if (tokenWrapper != null && tokenWrapper.Token != null && tokenWrapper.Token.Text != null)
                        {
                            width += tokenWrapper.Token.Text.Length;
                        }
                        break;
                    case ScriptWriterElementType.NewLine:
                        Debug.Assert(element is NewLineElement, "NewLineElement is expected");
                        width = 0;
                        previousPoint = null;
                        break;
                    default:
                        Debug.Assert(false, "Unknown ScriptWriterElement type");
                        break;
                }
            }

            // we have established previous-next relationships among all alignmnet points
            // now, we perform the alignment
            while (true)
            {
                if (allPoints.Count == 0)
                {
                    // all the alignment points have been aligned, so we are done
                    break;
                }

                AlignmentPointData ap = FindOneAlignmentPointWithOutDependent(allPoints);
                if (ap == null)
                {
                    // if we can't find any, we have a circle among alignment points
                    return null;
                }

                // let's align ap up
                HashSet<AlignmentPointData> rightPoints = ap.RightPoints;
                foreach (AlignmentPointData rightPoint in rightPoints)
                {
                    rightPoint.AlignAndRemoveLeftPoint(ap);
                }

                // ap is done; let's remove it; 
                allPoints.Remove(ap);
            }

            // generate aligned token stream
            List<TSqlParserToken> tokens = new List<TSqlParserToken>();

            Int32 offset = 0;
            for (Int32 index = 0; index < _scriptWriterElements.Count; ++index)
            {
                ScriptWriterElement element = _scriptWriterElements[index];
                switch (element.ElementType)
                {
                    case ScriptWriterElementType.AlignmentPoint:
                        AlignmentPointData ap = element as AlignmentPointData;
#if !PIMODLANGUAGE
                        Debug.Assert(ap != null, "AlignmentPointData is expected");
#endif
                        Debug.Assert(ap.Offset >= offset, "Incorrect offset");
                        if (ap.Offset > offset)
                        {
                            tokens.Add(ScriptGeneratorSupporter.CreateWhitespaceToken(ap.Offset - offset));
                        }
                        offset = ap.Offset;
                        break;
                    case ScriptWriterElementType.Token:
                        TokenWrapper tokenWrapper = element as TokenWrapper;
                        Debug.Assert(tokenWrapper != null, "TokenWrapper is expected");
                        Debug.Assert(tokenWrapper.Token.Text != null, "TokenWrapper.Token.Text should not be null");
                        if (tokenWrapper != null && tokenWrapper.Token != null && tokenWrapper.Token.Text != null)
                        {
                            tokens.Add(tokenWrapper.Token);
                            offset += tokenWrapper.Token.Text.Length;
                        }
                        break;
                    case ScriptWriterElementType.NewLine:
                        Debug.Assert(element is NewLineElement, "NewLineElement is expected");
                        tokens.Add(_newLineToken);
                        offset = 0;
                        break;
                    default:
                        Debug.Assert(false, "Unknown ScriptWriterElement type");
                        break;
                }
            }

            return tokens;
        }

        // get all tokens without alignment
        private List<TSqlParserToken> GetAllTokens()
        {
            List<TSqlParserToken> tokens = new List<TSqlParserToken>();

            for (Int32 index = 0; index < _scriptWriterElements.Count; ++index)
            {
                ScriptWriterElement element = _scriptWriterElements[index];
                switch (element.ElementType)
                {
                    case ScriptWriterElementType.Token:
                        TokenWrapper tokenWrapper = element as TokenWrapper;
                        Debug.Assert(tokenWrapper != null, "TokenWrapper is expected");
                        tokens.Add(tokenWrapper.Token);
                        break;
                    case ScriptWriterElementType.NewLine:
                        Debug.Assert(element is NewLineElement, "NewLineElement is expected");
                        tokens.Add(_newLineToken);
                        break;
                    case ScriptWriterElementType.AlignmentPoint:
                        // we don't do anything for the alignement points
                        break;
                    default:
                        Debug.Assert(false, "Unknown ScriptWriterElement type");
                        break;
                }
            }

            return tokens;
        }

        private static AlignmentPointData FindOneAlignmentPointWithOutDependent(HashSet<AlignmentPointData> points)
        {
            AlignmentPointData value = null;

            foreach (var item in points)
            {
                if (item.HasNoLeftPoints)
                {
                    value = item;
                    break;
                }
            }

            return value;
        }

        #endregion
    }
}
