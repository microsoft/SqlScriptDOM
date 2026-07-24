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
                // The top entry may be null when a named-alignment scope was pushed without its own
                // newline-restoration point (see PushNamedAlignmentScope); nothing to restore then.
                AlignmentPoint newLineAlignmentPoint = _newLineAlignmentPoints.Peek();
                if (newLineAlignmentPoint != null)
                {
                    Mark(newLineAlignmentPoint);
                }
            }
        }

        public void Indent(Int32 size)
        {
            AddSpace(size);
        }

        // In Tabs indentation mode, rewrite the leading whitespace of every line so that it uses
        // tab characters only. The script is first generated (and aligned) entirely with spaces;
        // this pass then replaces the leading run of spaces on each line with the number of tab
        // stops needed to reach (rounding up) that column: ceil(leadingSpaces / IndentationSize)
        // tabs, with no trailing spaces. Whitespace that appears after content on a line (mid-line
        // alignment padding) is left untouched. This pass is a no-op unless UseTabsForIndentation
        // is set (never when CommaPlacement is Leading).
        private List<TSqlParserToken> ConvertLeadingWhitespaceToTabs(List<TSqlParserToken> tokens)
        {
            if (_options.UseTabsForIndentation == false || _options.IndentationSize <= 0)
            {
                return tokens;
            }

            Int32 size = _options.IndentationSize;
            List<TSqlParserToken> result = new List<TSqlParserToken>(tokens.Count);
            Boolean atLineStart = true;
            Int32 leadingSpaces = 0;

            for (Int32 index = 0; index < tokens.Count; ++index)
            {
                TSqlParserToken token = tokens[index];
                // Whether the NEXT token starts a new line is determined by whether this token ENDS
                // with a newline, not merely contains one: a multi-line block comment or string
                // literal contains newlines but ends with content (e.g. "*/" or "'"), so tokens
                // that follow it on the same line must not be treated as leading indentation.
                Boolean endsWithNewLine = TokenEndsWithNewLine(token);

                if (atLineStart && !endsWithNewLine && IsAllSpaces(token.Text))
                {
                    // Accumulate the leading run of spaces (which may span multiple tokens).
                    leadingSpaces += token.Text.Length;
                    continue;
                }

                if (atLineStart)
                {
                    // Reached the end of the leading whitespace run: emit it as tab characters,
                    // rounding up to the next tab stop so no trailing spaces remain.
                    if (leadingSpaces > 0)
                    {
                        Int32 tabs = (leadingSpaces + size - 1) / size;
                        result.Add(ScriptGeneratorSupporter.CreateTabToken(tabs));
                        leadingSpaces = 0;
                    }
                }

                result.Add(token);
                atLineStart = endsWithNewLine;
            }

            return result;
        }

        private static Boolean TokenEndsWithNewLine(TSqlParserToken token)
        {
            String text = token.Text;
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            Char last = text[text.Length - 1];
            return last == '\n' || last == '\r';
        }

        private static Boolean IsAllSpaces(String text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return false;
            }

            foreach (Char c in text)
            {
                if (c != ' ')
                {
                    return false;
                }
            }
            return true;
        }

        // Tabs mode only. True when the element right after 'index' is a single-space separator
        // token. Such an alignment point is a "field separator" (for example the gap between a
        // clause keyword and its body, or between column-definition fields) whose gap is rendered
        // with tabs and whose trailing space is then swallowed.
        private Boolean IsFollowedBySeparatorSpace(Int32 index)
        {
            if (index + 1 >= _scriptWriterElements.Count)
            {
                return false;
            }

            TokenWrapper nextToken = _scriptWriterElements[index + 1] as TokenWrapper;
            return nextToken != null && nextToken.Token != null && nextToken.Token.Text == " ";
        }

        // Tabs mode only. Computes where this alignment point snaps to a tab stop, once its space
        // offset and its left neighbours' shifts are final. A field separator snaps to the first
        // tab stop strictly past its (shifted) column; every other point simply inherits the shift
        // of its left neighbours. TabShift records how far the snap moved the point versus the
        // space layout, and is propagated rightward so later columns on the line stay aligned.
        private void ComputeTabSnappedLayout(AlignmentPointData ap)
        {
            Int32 size = _options.IndentationSize;
            if (size <= 0)
            {
                return;
            }

            if (ap.AbsorbsSeparator)
            {
                Int32 shiftedColumn = ap.Offset + ap.MaxLeftTabShift;
                ap.TabTarget = ((shiftedColumn / size) + 1) * size;
                ap.TabShift = ap.TabTarget - ap.Offset - 1;
            }
            else
            {
                ap.TabShift = ap.MaxLeftTabShift;
            }
        }

        // Tabs mode only. When 'ap' is a field separator, emits its aligned gap as tab characters
        // (advancing 'offset' to the precomputed tab-stop target) and asks the caller to swallow
        // the single separator space that follows. Returns false in the default Spaces mode, or for
        // points that are not tab-snapped, so the caller falls back to the normal space padding.
        private Boolean TryEmitTabSnappedGap(AlignmentPointData ap, List<TSqlParserToken> tokens, ref Int32 offset, ref Boolean absorbSeparatorSpace)
        {
            if (_options.UseTabsForIndentation == false
                || _options.IndentationSize <= 0
                || ap.AbsorbsSeparator == false)
            {
                return false;
            }

            // The target already accounts for the cumulative shift introduced by any earlier
            // tab-snapped points on this line, so columns stay aligned.
            Int32 size = _options.IndentationSize;

            // If the precomputed target is not ahead of the current offset (which can happen when
            // earlier tab-snapped points on this line advanced 'offset' past this point's target),
            // advance to the next tab stop from the current offset so 'offset' never moves backward
            // and the tabs emitted actually land on the position we record.
            Int32 target = ap.TabTarget;
            if (target <= offset)
            {
                target = ((offset / size) + 1) * size;
            }

            Int32 tabCount = target / size - offset / size;
            if (tabCount < 1)
            {
                tabCount = 1;
            }

            tokens.Add(ScriptGeneratorSupporter.CreateTabToken(tabCount));
            offset = target;
            absorbSeparatorSpace = true;
            return true;
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

        // Add a comma-and-space separator that is right-aligned so that it ends exactly at the
        // offset of the alignment point that immediately follows it. Used for leading comma
        // placement in keyword-aligned lists (e.g. the SELECT column list).
        public void AddRightAlignedCommaSeparator()
        {
            List<TSqlParserToken> tokens = new List<TSqlParserToken>(2)
            {
                new TSqlParserToken(TSqlTokenType.Comma, ScriptGeneratorSupporter.GetTokenString(TSqlTokenType.Comma, _options.KeywordCasing))
            };
            if (_options.LeadingCommaSpaceCount > 0)
            {
                tokens.Add(ScriptGeneratorSupporter.CreateWhitespaceToken(_options.LeadingCommaSpaceCount));
            }
            _scriptWriterElements.Add(new RightAlignedSeparatorElement(tokens));
        }

        public void PushNewLineAlignmentPoint(AlignmentPoint ap)
        {
            PushNewLineAlignmentPoint(ap, resetNameScope: true);
        }

        // When resetNameScope is true (the default) the current named-alignment-point scope is
        // replaced with a fresh one, so named alignment points inside the pushed scope are
        // independent of the outer scope (used e.g. for nested constructs). When false, the same
        // named-alignment-point scope is kept, so named alignment points (e.g. column-definition
        // field alignment) remain shared across the pushed scopes; this is needed when the push
        // exists only to provide a newline-restoration point (comment preservation) and must not
        // defeat cross-line alignment.
        public void PushNewLineAlignmentPoint(AlignmentPoint ap, Boolean resetNameScope)
        {
            _newLineAlignmentPoints.Push(ap);
            _alignmentPointNameMaps.Push(_alignmentPointNameMapForCurrentScope);
            if (resetNameScope)
            {
                _alignmentPointNameMapForCurrentScope = new Dictionary<String, AlignmentPoint>();
            }
        }

        public void PopNewLineAlignmentPoint()
        {
            _newLineAlignmentPoints.Pop();
            _alignmentPointNameMapForCurrentScope = _alignmentPointNameMaps.Pop();
        }

        // Isolates the named-alignment-point scope for a list: named alignment points created after
        // this call are shared among the list's items but do not leak into the enclosing scope, so
        // separate lists rendered later in the same parent scope do not align against each other.
        // Unlike PushNewLineAlignmentPoint(ap), this neither adds an alignment point at the current
        // position nor changes newline restoration: the current newline-restoration point (if any)
        // is reused so NewLine() behavior inside the scope is unchanged.
        public void PushNamedAlignmentScope()
        {
            AlignmentPoint currentNewLineAlignmentPoint = _newLineAlignmentPoints.Count > 0 ? _newLineAlignmentPoints.Peek() : null;
            PushNewLineAlignmentPoint(currentNewLineAlignmentPoint, resetNameScope: true);
        }

        public void PopNamedAlignmentScope()
        {
            PopNewLineAlignmentPoint();
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
                text = ScriptGeneratorSupporter.GetCasedString(text, _options.KeywordCasing);
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

            // Tabs mode only post-pass; returns 'result' unchanged in the default Spaces mode.
            return ConvertLeadingWhitespaceToTabs(result);
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

                        // Tabs mode only: record whether this point is a field separator so its
                        // gap can later be rendered as tabs. In the default Spaces mode this is
                        // skipped and AbsorbsSeparator stays false, so nothing downstream changes.
                        if (_options.UseTabsForIndentation)
                        {
                            // AlignmentPointData is shared across every occurrence of the same
                            // alignment point, so OR the flag instead of overwriting it: the point
                            // is snapped to a tab stop if it is a field separator on ANY line. The
                            // per-occurrence decision to actually emit the tab gap is made during
                            // emission below.
                            ap.AbsorbsSeparator |= IsFollowedBySeparatorSpace(index);
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
                    case ScriptWriterElementType.RightAlignedSeparator:
                        // The separator occupies its own width between the previous alignment
                        // point and the following one (which it is right-aligned against).
                        RightAlignedSeparatorElement separatorWidthElement = element as RightAlignedSeparatorElement;
                        Debug.Assert(separatorWidthElement != null, "RightAlignedSeparatorElement is expected");
                        if (separatorWidthElement != null)
                        {
                            width += separatorWidthElement.Width;
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

                // Tabs mode only: compute where this point snaps to a tab stop, now that its
                // offset and its left neighbours' shifts are final. Skipped in the default Spaces
                // mode, where the offsets computed above are used exactly as-is.
                if (_options.UseTabsForIndentation)
                {
                    ComputeTabSnappedLayout(ap);
                }

                // let's align ap up
                HashSet<AlignmentPointData> rightPoints = ap.RightPoints;
                foreach (AlignmentPointData rightPoint in rightPoints)
                {
                    rightPoint.AlignAndRemoveLeftPoint(ap);

                    // Tabs mode only: carry this point's tab shift to its right neighbours so they
                    // stay aligned once earlier gaps have been snapped to tab stops.
                    if (_options.UseTabsForIndentation)
                    {
                        rightPoint.MaxLeftTabShift = Math.Max(rightPoint.MaxLeftTabShift, ap.TabShift);
                    }
                }

                // ap is done; let's remove it; 
                allPoints.Remove(ap);
            }

            // generate aligned token stream
            List<TSqlParserToken> tokens = new List<TSqlParserToken>(_scriptWriterElements.Count);

            Int32 offset = 0;
            RightAlignedSeparatorElement pendingSeparator = null;
            // When a mid-line alignment point has just been snapped to a tab stop in Tabs mode, the
            // single separator space the generator emits after it is redundant and is swallowed so
            // the following content follows the tab run directly.
            Boolean absorbSeparatorSpace = false;
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
                        // In Tabs mode a clause body can be snapped forward onto a tab stop, which
                        // can leave 'offset' ahead of a later alignment point's precomputed offset.
                        Debug.Assert(_options.UseTabsForIndentation || ap.Offset >= offset, "Incorrect offset");
                        if (pendingSeparator != null)
                        {
                            // Right-align the buffered separator so that it ends exactly at this
                            // alignment point's offset: pad up to (offset - separatorWidth), then
                            // emit the separator so the following item starts at the aligned column.
                            Int32 padBeforeSeparator = ap.Offset - offset - pendingSeparator.Width;
                            if (padBeforeSeparator > 0)
                            {
                                tokens.Add(ScriptGeneratorSupporter.CreateWhitespaceToken(padBeforeSeparator));
                                offset += padBeforeSeparator;
                            }

                            foreach (TSqlParserToken separatorToken in pendingSeparator.Tokens)
                            {
                                tokens.Add(separatorToken);
                                offset += separatorToken.Text.Length;
                            }

                            pendingSeparator = null;
                        }

                        // Tabs mode only: if THIS occurrence is followed by the single separator
                        // space token, emit its aligned gap as tabs and skip the default space
                        // padding below. The per-occurrence IsFollowedBySeparatorSpace check makes
                        // occurrences of a shared alignment point that are not separators fall
                        // through to normal padding, even though the shared AbsorbsSeparator flag is
                        // true. Skipped in the default Spaces mode, so that path runs unchanged.
                        if (_options.UseTabsForIndentation
                            && IsFollowedBySeparatorSpace(index)
                            && TryEmitTabSnappedGap(ap, tokens, ref offset, ref absorbSeparatorSpace))
                        {
                            break;
                        }

                        if (ap.Offset > offset)
                        {
                            tokens.Add(ScriptGeneratorSupporter.CreateWhitespaceToken(ap.Offset - offset));
                        }
                        // Use Math.Max because emitting a right-aligned separator above can advance
                        // 'offset' to exactly ap.Offset; never move the offset backwards here.
                        offset = Math.Max(offset, ap.Offset);
                        break;
                    case ScriptWriterElementType.Token:
                        FlushPendingSeparator(tokens, ref pendingSeparator, ref offset);
                        TokenWrapper tokenWrapper = element as TokenWrapper;
                        Debug.Assert(tokenWrapper != null, "TokenWrapper is expected");
                        Debug.Assert(tokenWrapper.Token.Text != null, "TokenWrapper.Token.Text should not be null");
                        if (tokenWrapper != null && tokenWrapper.Token != null && tokenWrapper.Token.Text != null)
                        {
                            // Swallow the single separator space that follows a tab-snapped alignment point.
                            if (absorbSeparatorSpace && tokenWrapper.Token.Text == " ")
                            {
                                absorbSeparatorSpace = false;
                                break;
                            }
                            absorbSeparatorSpace = false;
                            tokens.Add(tokenWrapper.Token);
                            offset += tokenWrapper.Token.Text.Length;
                        }
                        break;
                    case ScriptWriterElementType.RightAlignedSeparator:
                        // Defer emission until the following alignment point is known so the
                        // separator can be right-aligned against it.
                        FlushPendingSeparator(tokens, ref pendingSeparator, ref offset);
                        pendingSeparator = element as RightAlignedSeparatorElement;
                        break;
                    case ScriptWriterElementType.NewLine:
                        FlushPendingSeparator(tokens, ref pendingSeparator, ref offset);
                        Debug.Assert(element is NewLineElement, "NewLineElement is expected");
                        tokens.Add(_newLineToken);
                        offset = 0;
                        absorbSeparatorSpace = false;
                        break;
                    default:
                        Debug.Assert(false, "Unknown ScriptWriterElement type");
                        break;
                }
            }

            FlushPendingSeparator(tokens, ref pendingSeparator, ref offset);

            return tokens;
        }

        // Emit a buffered right-aligned separator inline (used when it is not immediately
        // followed by an alignment point to right-align against).
        private static void FlushPendingSeparator(List<TSqlParserToken> tokens, ref RightAlignedSeparatorElement pendingSeparator, ref Int32 offset)
        {
            if (pendingSeparator == null)
            {
                return;
            }

            foreach (TSqlParserToken separatorToken in pendingSeparator.Tokens)
            {
                tokens.Add(separatorToken);
                offset += separatorToken.Text.Length;
            }

            pendingSeparator = null;
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
                    case ScriptWriterElementType.RightAlignedSeparator:
                        RightAlignedSeparatorElement separatorElement = element as RightAlignedSeparatorElement;
                        Debug.Assert(separatorElement != null, "RightAlignedSeparatorElement is expected");
                        if (separatorElement != null)
                        {
                            foreach (TSqlParserToken separatorToken in separatorElement.Tokens)
                            {
                                tokens.Add(separatorToken);
                            }
                        }
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
