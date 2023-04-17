//------------------------------------------------------------------------------
// <copyright file="AlignmentPoint.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    //[DebuggerDisplay("AlignmentPoint(id:{Id}, name:{Name})")]
    internal class AlignmentPoint
    {
        private String _name;
#if false
        private static Int32 _nextId = 0;
        internal Int32 Id { get; private set; }
#endif
        public AlignmentPoint()
            : this(null)
        {
#if false
            Id = _nextId++;
#endif
        }

        public AlignmentPoint(String name)
        {
            _name = name;
        }

        public String Name
        {
            get { return _name; }
        }
    }
}
