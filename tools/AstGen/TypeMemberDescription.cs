//------------------------------------------------------------------------------
// <copyright file="TypeMemberDescription.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Globalization;
using System.Text;
using System;
using System.Diagnostics;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Information about type member
    /// </summary>
    class TypeMemberDescription : DefinitionDescription
    {
        public TypeMemberDescription(String name, String type)
            : base(name)
        {
            this.type = type;
        }

        public TypeMemberDescription(String name)
            : base(name)
        {
        }


        public bool isInterfaceMember = false;

        public String containerClass;

        public String type;

        public bool IsInheritedMember { get; set; }

        public bool IsInheritedClass { get; set; }

        public bool IsCollection { get; set; }

        private bool customImplementation = false;

        public bool CustomImplementation
        {
            get { return customImplementation; }
        }

        public void SetCustomImplementation(String str)
        {
            SetBooleanPropertyUsingString(str, ref customImplementation);
        }

        private bool generateUpdatePositionInfoCall = true;

        public bool GenerateUpdatePositionInfoCall
        {
            get { return generateUpdatePositionInfoCall; }
        }

        public void SetGenerateUpdatePositionInfoCall(String str)
        {
            SetBooleanPropertyUsingString(str, ref generateUpdatePositionInfoCall);
            if (GenerateUpdatePositionInfoCall && IsPrimitiveType(type))
                generateUpdatePositionInfoCall = false;
        }

        static string[] _primitiveTypes = new string[] { "bool", "bool?", "int", "int?", "string" };

        public bool IsPrimitiveType(string typeName)
        {
            return (Array.IndexOf<string>(_primitiveTypes, typeName) != -1);
        }

        /// <summary>
        /// Generates spec code (only declaration, no implementation)
        /// </summary>
        public String GenerateSpec()
        {
            StringBuilder sb = new StringBuilder();
            GenerateSummary(sb, Summary, "\t\t");

            GenerateAccessModifier(sb);

            if (IsCollection)
            {
                sb.AppendFormat("IList<{0}> {1}\r\n", type, Name);
                sb.AppendLine("\t\t{");
                if (isInterfaceMember)
                    sb.AppendLine("\t\t\tget;");
                else
                    sb.AppendLine("\t\t\tget { throw new NotImplementedException(); }");
            }
            else
            {
                sb.AppendFormat("{0} {1}\r\n", type, Name);
                sb.AppendLine("\t\t{");
                if (isInterfaceMember)
                    sb.AppendLine("\t\t\tget; set;");
                else
                {
                    sb.AppendLine("\t\t\tget { throw new NotImplementedException(); }");
                    sb.AppendLine("\t\t\tset { throw new NotImplementedException(); }");
                }
            }
            sb.AppendLine("\t\t}");
            sb.AppendLine();
            return sb.ToString();
        }

        private void GenerateAccessModifier(StringBuilder sb)
        {
            sb.Append("\t\t");
            if (!isInterfaceMember)
                sb.Append("public ");
        }

        public string LowerCaseName
        {
            get { return char.ToLower(Name[0], CultureInfo.InvariantCulture) + Name.Substring(1, Name.Length - 1); }
        }

        /// <summary>
        /// Generates real code (both declaration and implementation) for this member
        /// </summary>
        public String GenerateCode(bool generateTestDLL)
        {
            if (CustomImplementation)
                return ""; // Would be manually implemented, so, do not generate anything...

            if (IsInheritedClass || IsInheritedMember)
            {
                return "";
            }

            StringBuilder sb = new StringBuilder();
            String lowCaseName = LowerCaseName;

            // Generate member information only if it is a regular member            
            if (IsCollection)
            {
                // Backing field
                if (!isInterfaceMember && !generateTestDLL)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\t\tprivate List<{0}> _{1} = new List<{0}>();\r\n\r\n", type, lowCaseName);
                else if (!isInterfaceMember && generateTestDLL)
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\t\tprivate NodeList<{0}> _{1};\r\n\r\n", type, lowCaseName);

                // Property
                GenerateSummary(sb, Summary, "\t\t");
                GenerateAccessModifier(sb);
                sb.AppendFormat(CultureInfo.InvariantCulture, "IList<{0}> {1}\r\n", type, Name);
                sb.AppendLine("\t\t{");
                if (isInterfaceMember)
                    sb.Append("\t\t\tget; }\r\n");
                else
                    sb.AppendFormat(CultureInfo.InvariantCulture, "\t\t\tget {{ return _{0}; }}\r\n", lowCaseName);
                sb.AppendLine("\t\t}");
                sb.AppendLine();
            }
            else
            {
                // Backing field
                if (!isInterfaceMember)
                    sb.AppendFormat("\t\tprivate {0} _{1};\r\n\r\n", type, lowCaseName);

                // Property
                GenerateSummary(sb, Summary, "\t\t");
                GenerateAccessModifier(sb);
                sb.AppendFormat("{0} {1}\r\n", type, Name);
                sb.AppendLine("\t\t{");
                if (isInterfaceMember)
                {
                    sb.AppendFormat("\t\t\tget; set; \r\n", lowCaseName);
                }
                else if (generateTestDLL)
                {
                    sb.AppendFormat("\t\t\tget {{ return _{0}; }}\r\n", lowCaseName);
                    if (!IsPrimitiveType(type) && GenerateUpdatePositionInfoCall)
                    {
                        sb.AppendLine("\t\t\tset");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine();
                        sb.AppendLine("\t\t\t\tUpdateTokenInfo(value);");
                        sb.AppendLine();
                        sb.AppendFormat("\t\t\t\tif (_{0} != null)", LowerCaseName);
                        sb.AppendLine();
                        sb.AppendLine("\t\t\t\t{");
                        sb.AppendFormat("\t\t\t\t\t_{0}.Parent = null;", lowCaseName);
                        sb.AppendLine();
                        sb.AppendLine("\t\t\t\t}");
                        sb.AppendLine();
                        sb.AppendFormat("\t\t\t\t_{0} = value;", lowCaseName);
                        sb.AppendLine();
                        sb.AppendFormat("\t\t\t\tif (_{0} != null)", LowerCaseName);
                        sb.AppendLine();
                        sb.AppendLine("\t\t\t\t{");
                        sb.AppendFormat("\t\t\t\t\t_{0}.Parent = this;", lowCaseName);
                        sb.AppendLine();
                        sb.AppendLine("\t\t\t\t}");
                        sb.AppendLine("\t\t\t}");
                    }
                    else
                    {
                        //We don't update Parent pointers for Ints, Bools, and Enums
                        //
                        sb.AppendFormat("\t\t\tset {{ _{0} = value; }}", lowCaseName);
                        sb.AppendLine();
                    }
                }
                else
                {
                    sb.AppendFormat("\t\t\tget {{ return _{0}; }}\r\n", lowCaseName);
                    if (type != "bool" && type != "Int32" && GenerateUpdatePositionInfoCall)
                        sb.AppendFormat("\t\t\tset {{ UpdateTokenInfo(value); _{0} = value; }}\r\n", lowCaseName);
                    else
                        sb.AppendFormat("\t\t\tset {{ _{0} = value; }}\r\n", lowCaseName);
                }
                sb.AppendLine("\t\t}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Generate code for visiting this member (part of Visitor pattern)
        /// </summary>
        public void GenerateVisitorTraversal(StringBuilder sb, String baseClassName)
        {
            if (GenerateUpdatePositionInfoCall) // Not a member, typed by enum
            {
                if (IsInheritedClass == true)
                {
                    Debug.Assert(IsInheritedMember == false);
                    Debug.Assert(this.Name.Equals(baseClassName, StringComparison.Ordinal) == true,
                                    "InheritedClass " + this.Name + " should not go beyond Base class " + baseClassName);
                    if (String.Equals(this.Name, baseClassName, StringComparison.OrdinalIgnoreCase) == true)
                    {
                        Debug.Assert(String.IsNullOrEmpty(baseClassName) == false, "Base class should be specified");
                        if (String.IsNullOrEmpty(baseClassName) == false)
                        {
                            sb.AppendLine("\t\t\tbase.AcceptChildren(visitor);");
                        }
                    }
                }
                else if (IsInheritedMember == true)
                {
                    Debug.Assert(IsInheritedClass == false);
                    if (IsCollection)
                    {
                        sb.AppendLine("\t\t\tfor (Int32 i = 0, count = ((" + containerClass + ")this)." + Name + ".Count; i < count; ++i)");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine("\t\t\t\t ((" + containerClass + ")this)." + Name + "[i].Accept(visitor);");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine();
                    }
                    else
                    {
                        sb.AppendLine("\t\t\tif (((" + containerClass + ")this)." + Name + " != null)");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine("\t\t\t\t ((" + containerClass + ")this)." + Name + ".Accept(visitor);");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine();
                    }
                }
                else
                {
                    if (IsCollection)
                    {
                        sb.AppendLine("\t\t\tfor (Int32 i = 0, count = this." + Name + ".Count; i < count; ++i)");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine("\t\t\t\tthis." + Name + "[i].Accept(visitor);");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine();
                    }

                    else
                    {
                        sb.AppendLine("\t\t\tif (this." + Name + " != null)");
                        sb.AppendLine("\t\t\t{");
                        sb.AppendLine("\t\t\t\tthis." + Name + ".Accept(visitor);");
                        sb.AppendLine("\t\t\t}");
                        sb.AppendLine();
                    }
                }
            }
        }

        /// <summary>
        /// Copies this member information to be used somewhere else...
        /// </summary>
        public TypeMemberDescription GetCopy()
        {
            TypeMemberDescription retVal = new TypeMemberDescription(Name, type);
            retVal.IsCollection = IsCollection;
            retVal.isInterfaceMember = isInterfaceMember;
            retVal.IsInheritedClass = IsInheritedClass;
            retVal.IsInheritedMember = IsInheritedMember;
            retVal.generateUpdatePositionInfoCall = GenerateUpdatePositionInfoCall;
            retVal.Summary = Summary;
            return retVal;
        }

        #region Overridden methods

        public override Int32 GetHashCode()
        {
            return Name.GetHashCode();
        }

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var member = obj as TypeMemberDescription;
            if (member == null)
            {
                return false;
            }

            if (this == member)
            {
                return true;
            }

            if (GetHashCode() != member.GetHashCode())
            {
                return false;
            }

            return Name.Equals(member.Name, StringComparison.Ordinal);
        }

        # endregion
    }
}