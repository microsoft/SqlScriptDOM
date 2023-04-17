//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal abstract partial class SqlScriptGeneratorVisitor : TSqlConcreteFragmentVisitor
    {
        #region consts

        protected const String ClauseBody = "ClauseBody";
        protected const String SetClauseItemFirstEqualSign = "SetClauseItemFirstEqualSign";
        protected const String SetClauseItemSecondEqualSign = "SetClauseItemSecondEqualSign";
        protected const String InsertColumns = "InsertColumns";

        #endregion

        #region fields

        protected SqlScriptGeneratorOptions _options;
        protected ScriptWriter _writer;
        private Dictionary<TSqlFragment, Dictionary<String, AlignmentPoint>> _alignmentPointsForFragments;

        #endregion

        #region constructor

        public SqlScriptGeneratorVisitor(SqlScriptGeneratorOptions options, ScriptWriter writer) // visit no base types
        {
            _options = options;
            _writer = writer;
            _alignmentPointsForFragments = new Dictionary<TSqlFragment, Dictionary<String, AlignmentPoint>>();
        }

        #endregion

        #region formatting

        protected void NewLineAndIndent()
        {
            NewLine();
            Indent();
        }

        protected void Indent()
        {
            Indent(_options.IndentationSize);
        }

        protected void NewLineAndIndent(Int32 indentSize)
        {
            NewLine();
            Indent(indentSize);
        }

        protected void Indent(Int32 indentSize)
        {
            _writer.Indent(indentSize);
        }

        protected void Mark(AlignmentPoint ap)
        {
            _writer.Mark(ap);
        }

        protected void NewLine()
        {
            _writer.NewLine();
        }

        protected void PushAlignmentPoint(AlignmentPoint ap)
        {
            _writer.PushNewLineAlignmentPoint(ap);
        }

        protected void PopAlignmentPoint()
        {
            _writer.PopNewLineAlignmentPoint();
        }

        protected void MarkAndPushAlignmentPoint(AlignmentPoint ap)
        {
            Mark(ap);
            PushAlignmentPoint(ap);
        }

        protected AlignmentPoint FindOrCreateAlignmentPointByName(String apName)
        {
            return _writer.FindOrCreateAlignmentPoint(apName);
        }

        protected void AddAlignmentPointForFragment(TSqlFragment node, AlignmentPoint ap)
        {
            Debug.Assert(node != null, "TSqlFragment should not be null");
#if !PIMODLANGUAGE
            Debug.Assert(ap != null, "Alignment point should not be null");
            Debug.Assert(String.IsNullOrEmpty(ap.Name) == false, "Alignment point should have a name");
#endif

            if (node != null && ap != null && String.IsNullOrEmpty(ap.Name) == false)
            {
                Dictionary<String, AlignmentPoint> alignmentPointsForThisNode;
                if (_alignmentPointsForFragments.TryGetValue(node, out alignmentPointsForThisNode) == false)
                {
                    alignmentPointsForThisNode = new Dictionary<String, AlignmentPoint>();
                    _alignmentPointsForFragments.Add(node, alignmentPointsForThisNode);
                }

                if (String.IsNullOrEmpty(ap.Name) == false)
                {
                    Debug.Assert(alignmentPointsForThisNode.ContainsKey(ap.Name) == false, "An alignment point with same name exists");
                    if (alignmentPointsForThisNode.ContainsKey(ap.Name) == false)
                    {
                        alignmentPointsForThisNode.Add(ap.Name, ap);
                    }
                }
            }
        }

        protected AlignmentPoint GetAlignmentPointForFragment(TSqlFragment node, String name)
        {
            Debug.Assert(node != null, "TSqlFragment should not be null");
            Debug.Assert(String.IsNullOrEmpty(name) == false, "Alignment point name should not be null or empty");

            AlignmentPoint ap = null;

            if (node != null && String.IsNullOrEmpty(name) == false)
            {
                Dictionary<String, AlignmentPoint> alignmentPointsForThisNode;
                if (_alignmentPointsForFragments.TryGetValue(node, out alignmentPointsForThisNode) == true)
                {
                    if (alignmentPointsForThisNode.TryGetValue(name, out ap) == false)
                    {
                        ap = null;
                    }
                }
            }

            return ap;
        }

        protected void ClearAlignmentPointsForFragment(TSqlFragment node)
        {
            Debug.Assert(node != null, "TSqlFragment should not be null");
            if (node != null)
            {
                _alignmentPointsForFragments.Remove(node);
            }
        }

        #endregion
    }
}
