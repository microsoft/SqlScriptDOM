//------------------------------------------------------------------------------
// <copyright file="TypeDescription.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.ObjectModel;
using System.Collections.Generic;
using System;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Information about type (class or interface)
    /// </summary>
    abstract class TypeDescription : DefinitionDescription
    {
        protected String baseType;
        protected TypeDescription baseTypeDescription;

        public TypeDescription(String name)
            : base(name)
        {
        }

        public TypeDescription(String name, Int32 lineNo, Int32 colNo)
            : base(name, lineNo, colNo)
        {
        }

        public String BaseType
        {
            get { return baseType; }
            set { if (value != null) baseType = value; }
        }

        public TypeDescription BaseTypeDescription
        {
            get { return baseTypeDescription; }
            set { baseTypeDescription = value; }
        }

        public List<TypeMemberDescription> members = new List<TypeMemberDescription>();

        public IEnumerable<TypeMemberDescription> AllMembers
        {
            get
            {
                if (BaseTypeDescription != null)
                {
                    foreach (TypeMemberDescription member in BaseTypeDescription.AllMembers)
                    {
                        yield return member;
                    }
                }
                foreach (TypeMemberDescription member in members)
                {
                    if (!member.IsInheritedClass)
                    {
                        yield return member;
                    }
                }
            }
        }

        /// <summary>
        /// Generates spec code (only declarations, no implementation)
        /// </summary>
        abstract public String GenerateSpec(bool generateNamespace);

        /// <summary>
        /// Generates real code (declarations and implementation)
        /// </summary>
        abstract public String GenerateCode(bool generateNamespace, bool generateTestDLL);


    }
}