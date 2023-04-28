//------------------------------------------------------------------------------
// <copyright file="AstGen.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    class Program
    {
        /// <summary>
        /// Generates file header for given file
        /// </summary>
        public static string GenerateFileHeader(string fileName, List<string> usedNamespaces)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"//------------------------------------------------------------------------------");
            sb.AppendFormat("// <copyright file=\"{0}\" company=\"Microsoft\">\r\n", Path.GetFileName(fileName));
            sb.AppendLine(@"//         Copyright (c) Microsoft Corporation.  All rights reserved.");
            sb.AppendLine(@"// </copyright>");
            sb.AppendLine(@"//------------------------------------------------------------------------------");
            sb.AppendLine();
            sb.AppendLine(@"using System;");
            sb.AppendLine(@"using System.Collections.Generic;");
            sb.AppendLine(@"using System.Collections.ObjectModel;");
            foreach (string namespaceName in usedNamespaces)
            {
                sb.AppendLine("using " + namespaceName + ";");
            }
            sb.AppendLine();
            return sb.ToString();
        }

        const string usage = "TS Data AST generator\r\nUsage: AstGen <input file> <generic visitor output file> <concrete visitor output file> <AST output file> [/spec] [/test] [/sgenbase <Script Generator Base Visitor output file>] [/sgenfallback <Script Generator Fallback Visitor output file]";

        static int Main(string[] args)
        {
            if (args.Length < 4 || args[0] == "/?" ||
                args[0] == "-?" || string.Equals(args[0], "-h", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine(usage);
                return 0;
            }

            string visitorFileName = args[1];
            string concreteVisitorFileName = args[2];
            string outputFileName = args[3];

            bool generateSpec = false;
            bool generateTestDll = false;
            bool enableStreamAnalyticsExtensions = false;

            for (int i = 4; i < args.Length; i++)
            {
                if ((string.Equals(args[4], "/spec", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(args[4], "-spec", StringComparison.OrdinalIgnoreCase)))
                {
                    generateSpec = true;
                }
                else if ((string.Equals(args[4], "/test", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(args[4], "-test", StringComparison.OrdinalIgnoreCase)))
                {
                    generateTestDll = true;
                }
                else if ((string.Equals(args[4], "/enableASAExtensions", StringComparison.OrdinalIgnoreCase) ||
                          string.Equals(args[4], "-enableASAExtensions", StringComparison.OrdinalIgnoreCase)))
                {
                    enableStreamAnalyticsExtensions = true;
                }
            }

            string overrideScriptGeneratorFileName = null;
            int index = generateSpec ? 6 : 5;
            if (generateTestDll &&
                    args.Length > index)
                overrideScriptGeneratorFileName = args[index];

            // These are used to collect class and member information ignoring InheritedMember and InheritedClass
            List<ClassDescription> originalClasses = new List<ClassDescription>();
            Dictionary<string, InterfaceDescription> originalInterfaces = new Dictionary<string, InterfaceDescription>();
            List<string> originalUsedNamespaces = new List<string>();
            string originalNamespaceForGeneratedCode;


            List<ClassDescription> classes = new List<ClassDescription>();
            Dictionary<string, InterfaceDescription> interfaces = new Dictionary<string, InterfaceDescription>();
            List<string> usedNamespaces = new List<string>();
            string namespaceForGeneratedCode;
            List<AstGenError> astGenErrors = null;

            try
            {
                // Read Xml which takes into account InheritedMember and InheritedClass
                namespaceForGeneratedCode = AstXmlReaderWithOrdering.ReadAstXml(args[0], classes, interfaces, usedNamespaces, generateTestDll, enableStreamAnalyticsExtensions);
                AddInterfaceMembersToClasses(classes, interfaces);
                ComputeClassHierarchy(classes);

                // Validate the Xml 
                astGenErrors = AstGenValidation.ValidateAstXmlSpecification(classes);

                if (astGenErrors != null && astGenErrors.Count > 0)
                {
                    WriteErrors(astGenErrors);
                    // We stop right here -- Do not generate the visitors and Ast.cs
                    return 2; // 2 in case of validation errors
                }
                else
                {
                    // Read Xml using the previous Xml read logic (ignoring InheritedMember and InheritedClass)
                    originalNamespaceForGeneratedCode = AstXMLReader.ReadAstXML(args[0], originalClasses, originalInterfaces, originalUsedNamespaces, enableStreamAnalyticsExtensions);
                    AddInterfaceMembersToClasses(originalClasses, originalInterfaces);
                    ComputeClassHierarchy(originalClasses);

                    // We validate that we have read the Xml properly (with InhreitedMembers and InheritedClass)
                    // We validate the result with the data generated by previous xml read logic  which ignores InheritedMembers and InheritedClass
                    astGenErrors = AstGenValidation.ValidateAstXmlReaderWithOrdering(originalClasses, originalInterfaces, classes, interfaces);

                    if (astGenErrors != null && astGenErrors.Count > 0)
                    {
                        WriteErrors(astGenErrors);
                        // We stop right here -- Do not generate the visitors and Ast.cs
                        return 2; // 2 in case of validation errors
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Failed to read input file: " + e.Message);
                return 1;
            }
            catch (IOException e)
            {
                Console.WriteLine("Failed to read input file: " + e.Message);
                return 1;
            }

            try
            {
                // Populate the inherited members so that they reflect the type and other properties of origninal members
                CompleteInheritedMembers(classes);

                GenerateGenericVisitor(visitorFileName, namespaceForGeneratedCode, classes, usedNamespaces);
                GenerateConcreteVisitor(concreteVisitorFileName, namespaceForGeneratedCode, classes, usedNamespaces);

                if (generateTestDll && !String.IsNullOrEmpty(overrideScriptGeneratorFileName))
                {
                    GenerateOverrideVisitor(overrideScriptGeneratorFileName, namespaceForGeneratedCode, classes, usedNamespaces);
                }

                if (generateSpec)
                {
                    using (StreamWriter sw = new StreamWriter(outputFileName))
                    {
                        sw.Write(GenerateFileHeader(outputFileName, usedNamespaces));
                        sw.Write("namespace " + namespaceForGeneratedCode + "\r\n{\r\n");
                        foreach (InterfaceDescription i in interfaces.Values)
                            sw.Write(i.GenerateSpec(false));
                        foreach (ClassDescription c in classes)
                            sw.Write(c.GenerateSpec(false));
                        sw.Write("\r\n}\r\n");
                        sw.Close();
                    }
                }
                else
                {
                    using (StreamWriter sw = new StreamWriter(outputFileName))
                    {
                        sw.Write(GenerateFileHeader(outputFileName, usedNamespaces));
                        sw.Write("namespace " + namespaceForGeneratedCode + "\r\n{\r\n");
                        foreach (InterfaceDescription i in interfaces.Values)
                            sw.Write(i.GenerateCode(false, generateTestDll));
                        foreach (ClassDescription c in classes)
                            sw.Write(c.GenerateCode(false, generateTestDll));
                        sw.Write("\r\n}\r\n");
                        sw.Close();
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine("Failed to write output file: " + e.Message);
                return 1;
            }
            catch (IOException e)
            {
                Console.WriteLine("Failed to write output file: " + e.Message);
                return 1;
            }
            return 0;
        }

        private static void CompleteInheritedMembers(List<ClassDescription> classes)
        {
            foreach (ClassDescription classDesc in classes)
            {
                foreach (TypeMemberDescription member in classDesc.members)
                {
                    if (member.IsInheritedMember == true)
                    {
                        foreach (var ancestorClassDesc in classDesc.GetAllAncestors())
                        {
                            if (String.Compare(member.containerClass, ancestorClassDesc.Name, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                foreach (TypeMemberDescription m in ancestorClassDesc.members)
                                {
                                    if (String.Compare(member.Name, m.Name, StringComparison.OrdinalIgnoreCase) == 0)
                                    {
                                        CloneMemberProperties(member, m);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void CloneMemberProperties(TypeMemberDescription member, TypeMemberDescription m)
        {
            member.type = m.type;
            member.IsCollection = m.IsCollection;
            member.IsCollectionFirstItem = m.IsCollectionFirstItem;
            member.SetCustomImplementation(m.CustomImplementation == true ? "true" : "false");
            member.SetGenerateUpdatePositionInfoCall(m.GenerateUpdatePositionInfoCall == true ? "true" : "false");
        }

        /// <summary>
        /// Flattens inheritance before code generation (copies all members from interfaces to classes)
        /// </summary>
        private static void AddInterfaceMembersToClasses(List<ClassDescription> classes, Dictionary<string, InterfaceDescription> interfaces)
        {
            foreach (ClassDescription cd in classes)
            {
                foreach (string interfaceName in cd.implements)
                {
                    InterfaceDescription id = null;
                    if (interfaces.TryGetValue(interfaceName, out id))
                    {
                        foreach (TypeMemberDescription m in id.members)
                        {
                            TypeMemberDescription copy = m.GetCopy();
                            copy.isInterfaceMember = false;
                            cd.members.Add(copy);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Generates Visitor class for all the classes defined
        /// </summary>
        private static void GenerateGenericVisitor(string fileName, string namespaceToGenerate,
            List<ClassDescription> classes, List<string> usedNamespaces)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace " + namespaceToGenerate);
            sb.AppendLine("{");
            DefinitionDescription.GenerateSummary(sb, "Visitor for AST nodes", "\t");
            sb.AppendFormat("\tpartial class {0}", ClassDescription.FragmentVisitorTypeName);
            sb.AppendLine();
            sb.AppendLine("\t{");

            foreach (ClassDescription c in classes)
            {
                c.GenerateVisitMethod(sb);
                c.GenerateExplicitVisitMethod(sb);
                // c.GenerateOnVisitMethod(sb);
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(GenerateFileHeader(fileName, usedNamespaces));
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Generates ConcreteVisitor class for all the classes defined
        /// </summary>
        private static void GenerateConcreteVisitor(
            string fileName,
            string namespaceToGenerate,
            List<ClassDescription> classes,
            List<string> usedNamespaces)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace " + namespaceToGenerate);
            sb.AppendLine("{");
            DefinitionDescription.GenerateSummary(sb, "Visitor for concrete AST nodes", "\t");
            sb.AppendFormat("\tpartial class {0}", ClassDescription.ConcreteFragmentVisitorTypeName);
            sb.AppendLine();
            sb.AppendLine("\t{");

            foreach (ClassDescription c in classes)
            {
                c.GenerateVisitMethodsForConcreteVisitor(sb);
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(GenerateFileHeader(fileName, usedNamespaces));
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Generates Visitor class for all the classes defined
        /// </summary>
        private static void GenerateOverrideVisitor(string fileName, string namespaceToGenerate,
            List<ClassDescription> classes, List<string> usedNamespaces)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("namespace " + namespaceToGenerate);
            sb.AppendLine("{");
            DefinitionDescription.GenerateSummary(sb, "Visitor for AST nodes", "\t");
            sb.AppendFormat("\tpartial class {0}", ClassDescription.OverrideScriptGeneratorVisitorTypeName);
            sb.AppendLine();
            sb.AppendLine("\t{");

            foreach (ClassDescription c in classes)
            {
                c.GenerateOverrideExplicitVisitMethod(sb);
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.Write(GenerateFileHeader(fileName, usedNamespaces));
                sw.WriteLine(sb.ToString());
                sw.Close();
            }
        }

        /// <summary>
        /// Resolves references from derived to base classes
        /// </summary>
        private static void ComputeClassHierarchy(List<ClassDescription> classes)
        {
            // Synthesize known base types
            ClassDescription tsqlFragment = new ClassDescription("TSqlFragment");

            for (int i = 0; i < classes.Count; i++)
            {
                ClassDescription currentClass = classes[i];
                if (!String.IsNullOrEmpty(currentClass.BaseType) && (currentClass.BaseTypeDescription == null))
                {
                    if (currentClass.BaseType == tsqlFragment.Name)
                    {
                        currentClass.BaseTypeDescription = tsqlFragment;
                    }
                    else
                    {
                        for (int j = 0; j < classes.Count; j++)
                        {
                            ClassDescription candidateBaseClass = classes[j];

                            if (currentClass.BaseType == candidateBaseClass.Name)
                            {
                                currentClass.BaseTypeDescription = candidateBaseClass;
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dumps the errors to the console
        /// </summary>
        private static void WriteErrors(List<AstGenError> astGenErrors)
        {
            foreach (AstGenError error in astGenErrors)
            {
                Console.WriteLine("Ast.Xml (" + error.LineNumber + "," + error.ColumnNumber + ") : " + error.ErrorMessage);
            }
        }
    }
}