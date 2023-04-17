//------------------------------------------------------------------------------
// <copyright file="AstXmlReader.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using System;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Reads XML from disk into in-memory structures
    /// </summary>
    class AstXMLReader
    {
        /// <summary>
        /// Entry point - reads data into provided collections
        /// </summary>
        public static String ReadAstXML(String fileName, List<ClassDescription> classes,
            Dictionary<String, InterfaceDescription> interfaces,
            List<String> usedNamespaces,
            bool enableStreamAnalyticsExtensions)
        {
            AstXMLReader astReader = new AstXMLReader(classes, interfaces, usedNamespaces, enableStreamAnalyticsExtensions);
            return astReader.ReadAstXMLImpl(fileName);
        }

        private AstXMLReader(List<ClassDescription> classes,
            Dictionary<String, InterfaceDescription> interfaces,
            List<String> usedNamespaces,
            bool enableStreamAnalyticsExtensions)
        {
            _classes = classes;
            _interfaces = interfaces;
            _usedNamespaces = usedNamespaces;
            _enableStreamAnalyticsExtensions = enableStreamAnalyticsExtensions;
        }

        TypeDescription curType = null;
        TypeMemberDescription curMember = null;
        DefinitionDescription curDef = null;
        List<ClassDescription> _classes;
        Dictionary<String, InterfaceDescription> _interfaces;
        List<String> _usedNamespaces;
        private bool _enableStreamAnalyticsExtensions;

        String ReadAstXMLImpl(String fileName)
        {
            String namespaceToGenerate = String.Empty;
            XmlTextReader reader = new XmlTextReader(fileName);
            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.LocalName == "Types")
                    {
                        namespaceToGenerate = reader.GetAttribute("Namespace");
                    }
                    else if (reader.LocalName == "Use")
                    {
                        _usedNamespaces.Add(reader.GetAttribute("Namespace"));
                    }
                    else if (reader.LocalName == "Class")
                    {
                        TypeEnd();

                        ClassDescription newClass = new ClassDescription(reader.GetAttribute("Name"));
                        newClass.SetIsAbstract(reader.GetAttribute("Abstract"));
                        curType = newClass;
                        curType.Summary = reader.GetAttribute("Summary");
                        curType.BaseType = reader.GetAttribute("Base");
                    }
                    else if (reader.LocalName == "Member")
                    {
                        MemberEnd();

                        if (_enableStreamAnalyticsExtensions || !Convert.ToBoolean(reader.GetAttribute("IsStreamAnalyticsExtension") == null ? "false" : reader.GetAttribute("IsStreamAnalyticsExtension")))
                        { 
                            curMember = new TypeMemberDescription(reader.GetAttribute("Name"), reader.GetAttribute("Type"));
                            curMember.Summary = reader.GetAttribute("Summary");
                            curMember.IsCollection = Convert.ToBoolean(reader.GetAttribute("Collection") == null ? "false" : reader.GetAttribute("Collection"));
                            curMember.SetGenerateUpdatePositionInfoCall(reader.GetAttribute("GenerateUpdatePositionInfoCall"));
                            curMember.SetCustomImplementation(reader.GetAttribute("CustomImplementation"));
                        }
                    }
                    else if (reader.LocalName == "Summary")
                    {
                        if (curMember != null)
                            curDef = curMember;
                        else if (curType != null)
                            curDef = curType;
                        else
                            Debug.Assert(false);
                    }
                    else if (reader.LocalName == "Interface")
                    {
                        TypeEnd();

                        InterfaceDescription newInterface = new InterfaceDescription(reader.GetAttribute("Name"));
                        curType = newInterface;

                        curType.Summary = reader.GetAttribute("Summary");
                        curType.BaseType = reader.GetAttribute("Base");
                    }
                    else if (reader.LocalName == "Implements")
                    {
                        ClassDescription curClass = curType as ClassDescription;
                        if (curClass != null)
                            curClass.implements.Add(reader.GetAttribute("Interface"));
                        else
                            Debug.Assert(false);
                    }
                }
                else if (reader.NodeType == XmlNodeType.EndElement)
                {
                    if (reader.LocalName == "Class" || reader.LocalName == "Interface")
                        TypeEnd();
                    else if (reader.LocalName == "Member")
                        MemberEnd();
                    else if (reader.LocalName == "Summary")
                        curDef = null;
                    else if (reader.LocalName == "Types")
                        TypeEnd();
                }
                else if (reader.NodeType == XmlNodeType.Text)
                {
                    Debug.Assert(curDef != null);
                    curDef.Summary += reader.Value.Trim();
                }
            }
            return namespaceToGenerate;
        }

        private void MemberEnd()
        {
            if (curMember != null)
            {
                Debug.Assert(curType != null);
                curType.members.Add(curMember);
                curMember = null;
            }
        }

        private void TypeEnd()
        {
            if (curType == null)
                return;
            if (curMember != null)
            {
                curType.members.Add(curMember);
                curMember = null;
            }

            ClassDescription cd = curType as ClassDescription;
            if (cd != null)
                _classes.Add(cd);

            InterfaceDescription id = curType as InterfaceDescription;
            if (id != null)
                _interfaces[id.Name] = id;

            if (id == null && cd == null)
                Debug.Assert(false); // Some unknown type...

            curType = null;
        }
    }
}
