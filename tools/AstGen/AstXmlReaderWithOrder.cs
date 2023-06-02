//------------------------------------------------------------------------------
// <copyright file="AstXMLReaderWithOrder.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;


namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Reads XML from disk into in-memory structures taking into account InheritedMember and InheritedClass
    /// </summary>
    internal sealed class AstXmlReaderWithOrdering
    {
        /// <summary>
        /// Entry point - reads data into provided collections
        /// </summary>
        public static string ReadAstXml(
            string fileName,
            List<ClassDescription> classes,
            Dictionary<string /*interface name*/, InterfaceDescription> interfaces,
            List<string> usedNamespaces,
            bool genTestDll, bool enableStreamAnalyticsExtensions)
        {
            return AstXmlReaderWithOrdering.ReadAstXmlImpl(fileName, classes, interfaces, usedNamespaces, genTestDll, enableStreamAnalyticsExtensions);
        }

        public static string ReadAstXmlImpl(string fileName, List<ClassDescription> classes,
            Dictionary<string, InterfaceDescription> interfaces,
            List<string> usedNamespaces, bool generateTestDll, bool enableStreamAnalyticsExtensions)
        {
            XDocument doc = XDocument.Load(fileName, LoadOptions.SetLineInfo);

            // Get Class and member information
            var classesInfo = new List<ClassDescription>(
                from t in doc.Descendants("Class")
                select CreateClassDescription(t, generateTestDll, enableStreamAnalyticsExtensions));

            foreach (ClassDescription c in classesInfo)
            {
                classes.Add(c);
            }

            // Get Interfaces
            var id = (from t in doc.Descendants("Interface")
                      select CreateInterfaceDescription(t));

            foreach (InterfaceDescription i in id)
            {
                interfaces[i.Name] = i;
            }

            // Get namespace
            string namespaceToGenerate = (
                from t in doc.Elements("Types")
                select t.Attribute("Namespace").Value).Single<String>();

            // Get usedNameSpaces
            var q = (from t in doc.Elements("Use")
                     select t.Attribute("Namespace").Value).ToList();

            if (q.Count > 0)
            {
                usedNamespaces.Add(q.Single<String>());
            }

            return namespaceToGenerate;

        }

        // Creates class description, populates properties and returns class
        private static ClassDescription CreateClassDescription(XElement t, bool generateTestDll, bool enableStreamAnalyticsExtensions)
        {
            ClassDescription cd;
            IXmlLineInfo lineInfo = (IXmlLineInfo)t;

            if (lineInfo != null && lineInfo.HasLineInfo() == true)
            {
                cd = new ClassDescription(t.Attribute("Name").Value, lineInfo.LineNumber, lineInfo.LinePosition);
            }
            else
            {
                cd = new ClassDescription(t.Attribute("Name").Value);
            }

            if (cd != null)
            {
                // Populate properties
                cd.SetIsAbstract(t.Attribute("Abstract") == null ? "false" : t.Attribute("Abstract").Value);

                // Get members
                cd.members = new List<TypeMemberDescription>(
                        from m in t.Elements()
                        where 
                            (m.Name == "Member" || m.Name == "InheritedMember" || m.Name == "InheritedClass")
                            && (enableStreamAnalyticsExtensions || !Convert.ToBoolean(m.Attribute("IsStreamAnalyticsExtension") == null ? "false" : m.Attribute("IsStreamAnalyticsExtension").Value))
                        select CreateTypeMemberDescription(m));

                cd.implements = new List<String>(
                        from m in t.Elements("Implements")
                        select m.Attribute("Interface").Value);

                if ((cd.Name == "TSqlBatch" || cd.Name == "StatementList") && generateTestDll)
                {
                    //For Pimod.Language binding, we need TSqlBatch and StatementList base type to be TSqlStatement
                    //
                    cd.BaseType = "TSqlStatement";
                }
                else
                {
                    cd.BaseType = t.Attribute("Base") == null ? "TSqlFragment" : t.Attribute("Base").Value;
                }

                cd.Summary = GetSummary(t);
            }
            else
            {
                Debug.Assert(false, "Class Description cannot be null");
            }

            return cd;
        }

        // Creates Member description, populates properties and returns member
        private static TypeMemberDescription CreateTypeMemberDescription(XElement xm)
        {
            TypeMemberDescription member = null;

            if (xm.Name == "Member")
            {
                member = new TypeMemberDescription(xm.Attribute("Name").Value, xm.Attribute("Type").Value);
                member.IsInheritedClass = false;
                member.IsInheritedMember = false;
                member.IsCollection = Convert.ToBoolean(xm.Attribute("Collection") == null ? "false" : xm.Attribute("Collection").Value);
                member.IsCollectionFirstItem = Convert.ToBoolean(xm.Attribute("CollectionFirstItem") == null ? "false" : xm.Attribute("CollectionFirstItem").Value);
            }
            else if (xm.Name == "InheritedMember")
            {
                member = new TypeMemberDescription(xm.Attribute("Name").Value);
                member.IsInheritedClass = false;
                member.IsInheritedMember = true;
                member.containerClass = xm.Attribute("ContainerClass").Value;
                member.IsCollection = false;
            }
            else if (xm.Name == "InheritedClass")
            {
                member = new TypeMemberDescription(xm.Attribute("Name").Value);
                member.IsInheritedClass = true;
                member.IsInheritedMember = false;
                member.IsCollection = false;
            }

            Debug.Assert(member != null, "Member cannot be null");

            if (member != null)
            {
                member.Summary = GetSummary(xm);
                member.SetGenerateUpdatePositionInfoCall(xm.Attribute("GenerateUpdatePositionInfoCall") == null ? "true" : xm.Attribute("GenerateUpdatePositionInfoCall").Value);
                member.SetCustomImplementation(xm.Attribute("CustomImplementation") == null ? "false" : xm.Attribute("CustomImplementation").Value);
                member.isInterfaceMember = false;
            }

            return member;
        }

        private static InterfaceDescription CreateInterfaceDescription(XElement t)
        {
            InterfaceDescription id = null;
            IXmlLineInfo lineInfo = (IXmlLineInfo)t;

            if (lineInfo != null && lineInfo.HasLineInfo() == true)
            {
                id = new InterfaceDescription(t.Attribute("Name").Value, lineInfo.LineNumber, lineInfo.LinePosition);
            }
            else
            {
                id = new InterfaceDescription(t.Attribute("Name").Value);
            }

            Debug.Assert(id != null, "Interface Description cannot be null");
            if (id != null)
            {
                id.BaseType = t.Attribute("Base") == null ? "" : t.Attribute("Base").Value;
                id.Summary = GetSummary(t);

                // Add members
                id.members = new List<TypeMemberDescription>(
                        from m in t.Elements("Member")
                        select CreateTypeMemberDescription(m));
            }

            return id;
        }

        //Gets summary
        private static String GetSummary(XElement t)
        {
            var summary = t.Element("Summary");
            if (summary != null)
            {
                return summary.Value;
            }
            else
            {
                // No summary as a child element
                // Maybe,it has been specified as an attribute
                return t.Attribute("Summary") == null ? null : t.Attribute("Summary").Value;
            }
        }
    }
}