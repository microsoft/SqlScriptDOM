//------------------------------------------------------------------------------
// <copyright file="ClassDescription.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Information about class
    /// </summary>
    class ClassDescription : TypeDescription
    {
        public ClassDescription(String name)
            : base(name)
        {
            baseType = TSqlFragment;
        }

        public ClassDescription(String name, Int32 lineNo, Int32 colNo)
            : base(name, lineNo, colNo)
        {
            baseType = TSqlFragment;
        }


        public const String TSqlFragment = "TSqlFragment";
        public const String FragmentVisitorTypeName = "TSqlFragmentVisitor";
        public const String ConcreteFragmentVisitorTypeName = "TSqlConcreteFragmentVisitor";
        public const String ScriptGenBaseVisitorTypeName = "SqlScriptGeneratorVisitorBase";
        public const String ScriptGenFallbackVisitorTypeName = "FallbackSqlScriptGeneratorVisitor";
        public const String OverrideScriptGeneratorVisitorTypeName = "OverrideSqlScriptGeneratorVisitor";

        public List<String> implements = new List<String>();

        private bool isAbstract = false;

        private bool? hasInheritedClass = null;

        public bool IsAbstract
        {
            get { return isAbstract; }
        }

        public void SetIsAbstract(String str)
        {
            SetBooleanPropertyUsingString(str, ref isAbstract);
        }

        public bool HasInheritedClass
        {
            get
            {
                if (hasInheritedClass == null)
                {
                    hasInheritedClass = GetInheritedClassMember() != null;
                }

                return hasInheritedClass.Value;
            }
        }

        public void SetHasInheritedClass(Boolean value)
        {
            hasInheritedClass = value;
        }

        public TypeMemberDescription GetInheritedClassMember()
        {
            foreach (TypeMemberDescription m in members)
            {
                if (m.IsInheritedClass == true)
                {
                    return m;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets all the inherited members of the current class
        /// </summary>
        public Dictionary<String, HashSet<TypeMemberDescription>> GetAllInheritedMembers()
        {
            Dictionary<String, HashSet<TypeMemberDescription>> result = new Dictionary<String, HashSet<TypeMemberDescription>>();
            HashSet<TypeMemberDescription> memberList;
            foreach (TypeMemberDescription m in members)
            {
                memberList = null;
                if (m.IsInheritedMember == true)
                {
                    if (result.TryGetValue(m.containerClass, out memberList) == false)
                    {
                        memberList = new HashSet<TypeMemberDescription>();
                        memberList.Add(m);
                        result.Add(m.containerClass, memberList);
                    }
                    else
                    {
                        memberList.Add(m);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all the container classes of the current class
        /// </summary>
        public HashSet<String> GetAllContainerClasses()
        {
            HashSet<String> containerClassHashset = new HashSet<String>();
            foreach (TypeMemberDescription m in members)
            {
                if (m.IsInheritedMember == true)
                {
                    containerClassHashset.Add(m.containerClass);
                }
            }
            return containerClassHashset;
        }

        /// <summary>
        /// Gets all the ancestors of the current class
        /// </summary>
        public List<TypeDescription> GetAllAncestors()
        {
            List<TypeDescription> ancestors = new List<TypeDescription>();
            TypeDescription typeDesc = this;
            while (typeDesc.BaseTypeDescription != null)
            {
                ancestors.Add(typeDesc.BaseTypeDescription);
                typeDesc = typeDesc.BaseTypeDescription;
            }

            return ancestors;
        }

        /// <summary>
        /// Generates spec
        /// </summary>
        public override String GenerateSpec(bool generateNamespace)
        {
            StringBuilder sb = new StringBuilder();
            if (generateNamespace)
            {
                sb.AppendLine(NamespaceName);
                sb.AppendLine("{");
            }

            GenerateSummary(sb, Summary, "\t");
            GenerateClassHeader(sb);

            sb.AppendLine();

            foreach (TypeMemberDescription member in members)
                sb.Append(member.GenerateSpec());

            sb.AppendLine();
            if (!IsAbstract)
            {
                GenerateSummary(sb, "Generates the TSql script that is defined by this fragment.", "\t\t");
                sb.AppendLine("\t\tpublic override void GenerateSource(System.IO.TextWriter writer)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tthrow new NotImplementedException();");
                sb.AppendLine("\t\t}");

                // Visitor pattern..
                sb.AppendLine();
                GenerateSummary(sb, "Accepts visitor", "\t\t");
                sb.AppendFormat("\t\tpublic override void Accept({0} visitor)", FragmentVisitorTypeName);
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tthrow new NotImplementedException();");
                sb.AppendLine("\t\t}");

                GenerateSummary(sb, "Accepts visitor for Children", "\t\t");
                sb.AppendFormat("\t\tpublic override void AcceptChildren({0} visitor)", FragmentVisitorTypeName);
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tthrow new NotImplementedException();");
                sb.AppendLine("\t\t}");
            }

            sb.AppendLine("\t}");

            if (generateNamespace)
                sb.AppendLine("}");

            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Generates real code for this class
        /// </summary>
        public override String GenerateCode(bool generateNamespace, bool generateTestDLL)
        {
            StringBuilder sb = new StringBuilder();
            if (generateNamespace)
            {
                sb.AppendLine(NamespaceName);
                sb.AppendLine("{");
            }
            GenerateSummary(sb, Summary, "\t");
            GenerateClassHeader(sb);

            sb.AppendLine();
            if (generateTestDLL)
            {
                this.GenerateConstructor(sb);
            }
            sb.AppendLine();

            foreach (TypeMemberDescription member in members)
                sb.Append(member.GenerateCode(generateTestDLL));

            sb.AppendLine();

            this.GenerateAcceptMethod(sb);
            this.GenerateAcceptChildrenMethod(sb);

            if (generateTestDLL)
            {
                this.GenerateChildrenProperty(sb);
                this.GenerateClone(sb);
            }

            //sb.AppendLine("\t\tpublic override void GenerateSource(System.IO.TextWriter writer)");
            //sb.AppendLine("\t\t{");
            //sb.AppendLine("\t\t\tthrow new NotImplementedException();");
            //sb.AppendLine("\t\t}");
            sb.AppendLine("\t}");

            if (generateNamespace)
                sb.AppendLine("}");

            sb.AppendLine();
            return sb.ToString();
        }

        public void GenerateChildrenProperty(StringBuilder sb)
        {
            if (members.Count > 0)
            {
                sb.AppendLine();
                sb.AppendLine("\t\tinternal override IEnumerable<TSqlFragment> Children");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tget");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tList<TSqlFragment> children = new List<TSqlFragment>();");

                // Now, visit all children...
                foreach (TypeMemberDescription member in members)
                {
                    if (member.IsCollection && member.GenerateUpdatePositionInfoCall)
                    {
                        sb.AppendFormat("\t\t\t\tforeach ({0} member in {1})\r\n", member.type, member.Name);
                        sb.AppendLine("\t\t\t\t\tchildren.Add(member);");
                    }
                    else if (!member.IsInheritedClass && member.GenerateUpdatePositionInfoCall)
                    {
                        sb.AppendFormat("\t\t\t\tif ({0} != null)\r\n", member.Name);
                        sb.AppendFormat("\t\t\t\t\tchildren.Add({0});\r\n", member.Name);
                    }
                    else if (member.IsInheritedClass)
                    {
                        sb.AppendLine("\t\t\t\tchildren.AddRange(base.Children);");
                    }
                }
                sb.AppendLine("\t\t\t\treturn children;");
                sb.AppendLine("\t\t\t}");

                sb.AppendLine("\t\t}");
            }
        }

        //////////////////////////////////////////////////////////////////
        // Visitor pattern parts - calls generated in this class
        #region Generated on the Fragment
        public void GenerateAcceptMethod(StringBuilder sb)
        {
            if (!IsAbstract)
            {
                // Visitor pattern..
                GenerateSummary(sb, "Accepts visitor", "\t\t");
                sb.AppendLine("\t\tpublic override void Accept(" + FragmentVisitorTypeName + " visitor)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tif (visitor != null)");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tvisitor.ExplicitVisit(this);");
                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t}");
            }
        }

        public void GenerateAcceptChildrenMethod(StringBuilder sb)
        {
            sb.AppendLine();
            GenerateSummary(sb, "Accepts visitor for Children", "\t\t");
            sb.AppendLine("\t\tpublic override void AcceptChildren" + "(" + FragmentVisitorTypeName + " visitor)");
            sb.AppendLine("\t\t{");
            // Now, visit all children...
            foreach (TypeMemberDescription member in members)
                member.GenerateVisitorTraversal(sb, this.BaseType);

            // Visit base...
            // If this class has an inherited class, no need to visit its base as part of the default action
            // Same should be done for inherited member also. Because, if we have have one inherited member, we would have all members as inherited member
            // equivalent to inherited class --> only that order will be different
            // Visit base only when there are no inherited members or inherited class
            if (GetAllInheritedMembers().Count == 0 &&
                HasInheritedClass == false)
            {
                if (!String.IsNullOrEmpty(BaseType))
                {
                    sb.AppendLine("\t\t\tbase.AcceptChildren(visitor);");
                }
            }

            sb.AppendLine("\t\t}");
        }
        #endregion

        //////////////////////////////////////////////////////////////////
        // Visitor pattern parts - calls generated in the Visitor class
        #region Generated on the Visitor
        public void GenerateVisitMethod(StringBuilder sb)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendFormat("\t\t/// Visitor for {0}", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine("\t\tpublic virtual void Visit" + "(" + Name + " node)");
            sb.AppendLine("\t\t{");
            sb.AppendLine("\t\t\tif (!this.VisitBaseType)");
            sb.AppendLine("\t\t\t{");
            sb.AppendLine("\t\t\t\tthis.Visit((TSqlFragment) node);");
            sb.AppendLine("\t\t\t}");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
        }

        public void GenerateExplicitVisitMethod(StringBuilder sb)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendFormat("\t\t/// Explicit Visitor for {0}", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendLine("\t\tpublic virtual void ExplicitVisit" + "(" + Name + " node)");
            sb.AppendLine("\t\t{");

            if (!String.IsNullOrEmpty(BaseType))
            {
                sb.AppendLine("\t\t\tif (this.VisitBaseType)");
                sb.AppendLine("\t\t\t{");
                GenerateVisitBaseTypes(this, sb);
                sb.AppendLine("\t\t\t}");
                sb.AppendLine();
            }

            sb.AppendLine("\t\t\tthis.Visit(node);");
            sb.AppendLine("\t\t\tnode.AcceptChildren(this);");
            sb.AppendLine("\t\t}");
            sb.AppendLine();
        }

        public void GenerateVisitMethodsForConcreteVisitor(StringBuilder sb)
        {
            if (this.IsAbstract)
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendFormat("\t\t/// Visitor for {0}", Name);
                sb.AppendLine();
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine("\t\tpublic override sealed void Visit(" + Name + " node)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tbase.Visit(node);");
                sb.AppendLine("\t\t}");
                sb.AppendLine();

                sb.AppendLine("\t\t/// <summary>");
                sb.AppendFormat("\t\t/// Explicit Visitor for {0}", Name);
                sb.AppendLine();
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine("\t\tpublic override sealed void ExplicitVisit(" + Name + " node)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tbase.ExplicitVisit(node);");
                sb.AppendLine("\t\t}");
                sb.AppendLine();
            }
        }

        public void GenerateOverrideExplicitVisitMethod(StringBuilder sb)
        {
            if (!this.IsAbstract)
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendFormat("\t\t/// Explicit Visitor for {0}", Name);
                sb.AppendLine();
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine("\t\tpublic override sealed void ExplicitVisit(" + Name + " node)");
                sb.AppendLine("\t\t{");
                sb.AppendLine("\t\t\tif(node.OverrideScript != null)");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tGenerateIdentifierWithoutCasing(node.OverrideScript);");
                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t\telse");
                sb.AppendLine("\t\t\t{");
                sb.AppendLine("\t\t\t\tbase.ExplicitVisit(node);");
                sb.AppendLine("\t\t\t}");
                sb.AppendLine("\t\t}");
                sb.AppendLine();
            }
        }

        private static void GenerateVisitBaseTypes(TypeDescription currentClass, StringBuilder sb)
        {
            while (currentClass.BaseTypeDescription != null)
            {
                sb.AppendFormat("\t\t\t\tthis.Visit(({0}) node);", currentClass.BaseType);
                sb.AppendLine();
                currentClass = currentClass.BaseTypeDescription;
            }
        }

        #endregion

        /// <summary>
        /// Generates class header for this class
        /// </summary>
        private void GenerateClassHeader(StringBuilder sb)
        {
            sb.AppendLine("\t[System.Serializable]");
            sb.AppendFormat("\tpublic {0}partial class {1} : {2}", IsAbstract ? "abstract " : "", Name, BaseType);
            foreach (String implementedInterface in implements)
                sb.Append(", " + implementedInterface);
            sb.AppendLine("\r\n\t{");
        }

        private void GenerateConstructor(StringBuilder sb)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendFormat("\t\t/// Constructor for {0}", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t/// </summary>");
            sb.AppendFormat("\t\t{0} {1}()", IsAbstract ? "protected" : "public", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t{");

            foreach (TypeMemberDescription member in members)
            {
                if (member.IsCollection && !member.CustomImplementation && !member.IsInheritedMember)
                {
                    sb.AppendFormat("\t\t\t_{0}= new NodeList<{1}>(this);", member.LowerCaseName, member.type);
                    sb.AppendLine();
                }
            }
            sb.AppendLine("\t\t}");
            sb.AppendLine();
        }

        private void GenerateConstructorParameter(StringBuilder sb, TypeMemberDescription member)
        {
            //We output the parameter name as pName instead of LowerCaseName because some of the member names are String or Object, and string and object are reserved word.
            //
            if (member.IsCollection)
            {
                sb.AppendFormat("IEnumerable<{0}> parameter{1}, ", member.type, member.Name);
            }
            else
            {
                sb.AppendFormat("{0} parameter{1}, ", member.type, member.Name);
            }
        }

        private void GenerateConstructorBody(StringBuilder sb, TypeMemberDescription member)
        {
            if (member.IsCollection)
            {
                sb.AppendFormat("\t\t\tif(parameter{0} == null)", member.Name);
                sb.AppendLine();
                sb.AppendFormat("\t\t\t\t_{0}= new NodeList<{1}>(this);", member.LowerCaseName, member.type);
                sb.AppendLine();
                sb.AppendLine("\t\t\telse");
                sb.AppendFormat("\t\t\t\t_{0} = new NodeList<{1}>(this, parameter{2});", member.LowerCaseName, member.type, member.Name);
            }
            else
            {
                sb.AppendFormat("\t\t\t{0} = parameter{0};", member.Name);
                sb.AppendLine();
                if (!member.IsPrimitiveType(member.type) && member.GenerateUpdatePositionInfoCall)
                {
                    sb.AppendFormat("\t\t\tif ({0} != null)", member.Name);
                    sb.AppendLine();
                    sb.AppendFormat("\t\t\t\t{0}.Parent = this;", member.Name);
                    sb.AppendLine();
                }
            }
            sb.AppendLine();
        }

        private void GenerateClone(StringBuilder sb)
        {
            sb.AppendLine("\t\t/// <summary>");
            sb.AppendFormat("\t\t/// Clone for {0}", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t/// </summary>");

            //If we're abstract, just need to generate a Clone method that casts CloneInternal to our type
            //
            sb.AppendFormat("\t\tpublic new {0}  Clone()", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t{");
            sb.AppendFormat("\t\t\treturn ({0})CloneInternal();", Name);
            sb.AppendLine();
            sb.AppendLine("\t\t}");
            sb.AppendLine();

            if (!IsAbstract)
            {
                sb.AppendLine("\t\t/// <summary>");
                sb.AppendFormat("\t\t/// Clone for {0}", Name);
                sb.AppendLine();
                sb.AppendLine("\t\t/// </summary>");
                sb.AppendLine("\t\tprotected override TSqlFragment CloneInternal()");
                sb.AppendLine("\t\t{");
                sb.AppendFormat("\t\t\t{0} clone = new {0}();", Name);
                sb.AppendLine();
                List<TypeMemberDescription> baseMembers = new List<TypeMemberDescription>(BaseTypeDescription.AllMembers);
                foreach (TypeMemberDescription member in baseMembers)
                {
                    GenerateCloneMember(sb, member);
                }
                foreach (TypeMemberDescription member in members)
                {
                    GenerateCloneMember(sb, member);
                }

                sb.AppendLine("\t\t\treturn clone;");
                sb.AppendLine("\t\t}");
                sb.AppendLine();
            }
        }

        private void GenerateCloneMember(StringBuilder sb, TypeMemberDescription member)
        {
            if (member.IsCollection)
            {
                sb.AppendFormat("\t\t\tforeach({0} item in this.{1})", member.type, member.Name);
                sb.AppendLine();
                sb.AppendFormat("\t\t\tclone.{0}.Add(item.Clone());", member.Name);
                sb.AppendLine();
            }
            else if (!member.IsInheritedClass)
            {
                if (!member.IsPrimitiveType(member.type) && member.GenerateUpdatePositionInfoCall)
                {
                    sb.AppendFormat("\t\t\tif(this.{0} != null)", member.Name);
                    sb.AppendLine();
                    sb.AppendFormat("\t\t\t\tclone.{0} = this.{0}.Clone();", member.Name);
                    sb.AppendLine();
                }
                else
                {
                    sb.AppendFormat("\t\t\tclone.{0} = this.{0};", member.Name);
                    sb.AppendLine();
                }
            }
        }
    }
}