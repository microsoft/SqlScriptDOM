//------------------------------------------------------------------------------
// <copyright file="ScriptWriter.AlignmentPointInstance.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    internal partial class ScriptWriter
    {
        //[DebuggerDisplay("AlignmentPoint#{Id}(name:{Name}, offset:{Offset}, left:{_leftPoints.Count}, right:{_rightPoints.Count})")]
        internal class AlignmentPointData : ScriptWriterElement
        {
#if false
            private static Int32 _nextId = 0;
            internal Int32 Id { get; private set; }
#endif
            private Dictionary<AlignmentPointData, Int32> _leftPoints;
            private HashSet<AlignmentPointData> _rightPoints;

            public AlignmentPointData(String name)
            {
                this.ElementType = ScriptWriter.ScriptWriterElementType.AlignmentPoint;
                Name = name;
                _leftPoints = new Dictionary<AlignmentPointData, Int32>();
                _rightPoints = new HashSet<AlignmentPointData>();
#if false
                Id = _nextId++;
#endif
            }

            public Int32 Offset { get; set; }
            public String Name { get; private set; }

            public void AddLeftPoint(AlignmentPointData ap, Int32 width)
            {
                Int32 currentWidth;
                if (_leftPoints.TryGetValue(ap, out currentWidth) == false)
                {
                    // we don't have this point yet, add it
                    _leftPoints.Add(ap, width);
                }
                else if (currentWidth < width)
                {
                    // we already have this one, but its width is smaller, so we update it
                    _leftPoints[ap] = width;
                }
                // we do nothing if we already have this point and its width is larger
            }

            public Boolean HasNoLeftPoints
            {
                get
                {
                    return _leftPoints.Count == 0;
                }
            }

            public void AddRightPoint(AlignmentPointData ap)
            {
                _rightPoints.Add(ap);
            }

            public HashSet<AlignmentPointData> RightPoints
            {
                get
                {
                    return _rightPoints;
                }
            }

            // push the current point right when necessary based on the given left alignment point
            // after processing, remove the given point from the left point set of the current point
            public void AlignAndRemoveLeftPoint(AlignmentPointData ap)
            {
                Int32 width;
                if (_leftPoints.TryGetValue(ap, out width))
                {
                    Offset = Math.Max(ap.Offset + width, Offset);
                    _leftPoints.Remove(ap);
                }
                else
                {
                    Debug.Assert(false, "Cannot find the given AlignmentPointData");
                }
            }
        }
    }
}
