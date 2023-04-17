//------------------------------------------------------------------------------
// <copyright file="AstGenValidation.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Reads XML from disk into in-memory structures
    /// </summary>
    /// 

    class AstGenValidation
    {
        /// <summary>
        /// Entry point - validates the input xml 
        /// </summary>

        public static List<AstGenError> ValidateAstXmlSpecification(List<ClassDescription> classes)
        {
            Boolean isError = false;
            List<AstGenError> errors = new List<AstGenError>();

            foreach (var currentClass in classes)
            {
                var validationDelegate = new[] {
                                         // You can only have one Inherited Class element in a Class description
                                         ValidateForOnlyOneInheritedClass(currentClass),
                                         // InheritedClass should be an ancestor of current class (need not be immediate parent)
                                         CheckForValidInheritedClass(currentClass),
                                         // Cannot specify InheritedMember and InheritedClass on the same class 
                                        CheckForInheritedMemberAndInheritedClassOnSameClassDefinition(currentClass),
                                        // If user specifies Inherited Class and Inherited Member, InheritedClass attribute should be the ancestor of everything 
                                        // in Inherited Member
                                        CheckIfInheritedClassIsTopmostAncestor(currentClass, classes),
                                        // If user specifies Inherited Class and Inherited Member, everything under InheritedClass should be specified
                                        // as Inherited Member
                                        CheckForCompletenessUnderInheritedClass(currentClass, classes),
                                        // We don't allow IC beyond the immediate parent
                                        // this is a limitation of our implementation
                                        CheckForNoICBeyondImmediateParent(currentClass),
                                        // If user specifies InheritedMember, all the members of the ContainerClass must be specified as InheritedMember
                                        CheckCompletenessForContainerClass(currentClass, classes),
                                        // We need atleast 1 IC for the class unless everything has been specified as IM
                                        CheckForMinimumICSpecification(currentClass, classes),
                                        // We should not use the same name for two different members in the inheritance hierachy
                                        CheckForConflictingMemberNamesAlongInheritanceHierarchy(currentClass),
                                        };
                
                foreach (var v in validationDelegate)
                {
                    var errorsReturned = v;
                    if (errorsReturned != null && errorsReturned.Count > 0)
                    {
                        errors = errorsReturned;
                        isError = true;
                        break;
                    }
                }
                if (isError == true)
                {
                    break;
                }
            }

            return errors;
        }

        private static List<AstGenError> CheckForMinimumICSpecification(ClassDescription classDesc, List<ClassDescription> classes)
        {
            if (CheckMembers(classes, classDesc, classes, classDesc) == false)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, "Minimum IC not satisfied : " + classDesc.Name));
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// Checks for complete specification of Inherited Members down from the Inherited Class specification.
        /// </summary>
        private static List<AstGenError> CheckForCompletenessUnderInheritedClass(ClassDescription classDesc, List<ClassDescription> allClasses)
        {
            List<AstGenError> errorList = new List<AstGenError>();
            List<String> classesUnderInheritedClass = ExtractAllClassesUnderInheritedClass(classDesc);
            CheckForCompleteness(classesUnderInheritedClass, classDesc, errorList);
            return errorList;
        }

        /// <summary>
        /// Extracts all the classnames falling under the Inherited Class of the current class
        /// </summary>
        private static List<String> ExtractAllClassesUnderInheritedClass(ClassDescription classDesc)
        {
            TypeMemberDescription inheritedClass = classDesc.GetInheritedClassMember();
            List<String> classesUnderInheritedClass = new List<String>();
            if (inheritedClass != null)
            {

                TypeDescription typeDesc = classDesc;
                while (typeDesc.BaseTypeDescription != null)
                {
                    if (inheritedClass.Name.Equals(typeDesc.BaseTypeDescription.Name, StringComparison.Ordinal) == true)
                    {
                        break;
                    }
                    classesUnderInheritedClass.Add(typeDesc.BaseTypeDescription.Name);
                    typeDesc = typeDesc.BaseTypeDescription;
                }
                return classesUnderInheritedClass;
            }

            return classesUnderInheritedClass;
        }

        /// <summary>
        /// Checks for complete specification of Inherited Members down from the Inherited Class specification.
        /// </summary>
        private static void CheckForCompleteness(List<String> classNamesUnderInheritedClass, ClassDescription classDesc, List<AstGenError> errors)
        {
            HashSet<String> containerClasses = classDesc.GetAllContainerClasses();

            foreach (String name in classNamesUnderInheritedClass)
            {
                if (containerClasses.Contains(name) == false)
                {
                    errors.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber,
                        "All members belonging to the classes falling under the InheritedClass" + classDesc.GetInheritedClassMember().Name + " must be specified as Inherited Members"));
                }
            }
        }



        /// <summary>
        /// Checks if the class specified in Inherited Class is the topmost ancestor among all the IC and Inherited Member specification.
        /// </summary>
        private static List<AstGenError> CheckIfInheritedClassIsTopmostAncestor(ClassDescription classDesc, List<ClassDescription> allClasses)
        {
            TypeMemberDescription inheritedClass = classDesc.GetInheritedClassMember();
            // Do the validation only if we have inherited Class specified
            if (inheritedClass != null)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                HashSet<String> containerClasses = classDesc.GetAllContainerClasses();
                TypeDescription typeDesc = classDesc;
                foreach (String item in containerClasses)
                {
                    // Check if each inherited Member's ancestor is Inherited Class
                    if (CheckForAncestor(item, inheritedClass, allClasses) == false)
                    {
                        errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber,
                            "InheritedClass " + classDesc.GetInheritedClassMember().Name + "is not the topmost in the hierarchy"));
                    }
                }
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// Checks if a given class is an ancestor of the current class.
        /// </summary>
        private static Boolean CheckForAncestor(String currentClassName, TypeMemberDescription potentialAncestorClass, List<ClassDescription> allClasses)
        {
            foreach (ClassDescription classDesc in allClasses)
            {
                if (classDesc.Name.Equals(currentClassName, StringComparison.Ordinal) == true)
                {
                    TypeDescription typeDesc = classDesc;
                    while (typeDesc != null)
                    {
                        if (potentialAncestorClass != null &&
                            potentialAncestorClass.Name.Equals(typeDesc.BaseTypeDescription.Name) == false) // If ancestorClass is not the immediate parent
                        {
                            // Check if it is one of the ancestors
                            typeDesc = typeDesc.BaseTypeDescription;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }
            return false;
        }

        /// <summary>
        /// If one member of a class is specified as an inherited member, all the other members of the class must also be
        /// specified as Inherited Members. This method checks that rule.
        /// </summary>
        private static List<AstGenError> CheckCompletenessForContainerClass(ClassDescription classDesc, List<ClassDescription> allClasses)
        {
            Dictionary<String /*container class*/, HashSet<TypeMemberDescription>> inheritedMembers = classDesc.GetAllInheritedMembers();

            List<AstGenError> errorList = new List<AstGenError>();
            foreach (var item in inheritedMembers)
            {
                foreach (ClassDescription c in allClasses)
                {
                    if (c.Name.Equals(item.Key, StringComparison.Ordinal) == true)
                    {
                        HashSet<TypeMemberDescription> memberHash = new HashSet<TypeMemberDescription>(GetRegularMembers(c.members));
                        if (CheckMemberEquality(item.Value, memberHash) == false)
                        {
                            errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber,
                                "All the members of the Container Class " + item.Key + " must be specified as Inherited Members"));
                        }
                    }
                }
            }
            return errorList;
        }

        /// <summary>
        /// Gets all members of the class that are not InheritedClass or InheritedMember.
        /// </summary>
        private static List<TypeMemberDescription> GetRegularMembers(List<TypeMemberDescription> memberList)
        {
            List<TypeMemberDescription> members = new List<TypeMemberDescription>();
            foreach (TypeMemberDescription m in memberList)
            {
                if (m.IsInheritedClass == false && m.IsInheritedMember == false)
                {
                    members.Add(m);
                }
            }
            return members;
        }

        /// <summary>
        /// Checks if inherited class and inherited member point to the same class
        /// InheritedClass Name and containerClass Name is the same
        /// </summary>
        private static List<AstGenError> CheckForInheritedMemberAndInheritedClassOnSameClassDefinition(ClassDescription classDesc)
        {
            TypeMemberDescription inheritedClassMember = classDesc.GetInheritedClassMember();
            // Do the validation only if we have inherited Class specified
            if (inheritedClassMember != null)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                foreach (TypeMemberDescription member in classDesc.members)
                {
                    // If the inherited Member's container class is the same as the Inherited Class Name, output error message
                    if (member.IsInheritedMember == true &&
                        member.containerClass.Equals(inheritedClassMember.Name, StringComparison.Ordinal) == true)
                    {
                        String errorMessage = "InheritedMember and InheritedClass cannot point to the same class : " + member.containerClass;
                        errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, errorMessage));
                    }
                }
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// Checks if the inherited class specified is an ancestor of current class
        /// </summary>
        private static List<AstGenError> CheckForValidInheritedClass(ClassDescription classDesc)
        {
            Boolean isValidInheritedClass = false;
            TypeMemberDescription member = classDesc.GetInheritedClassMember();
            TypeDescription typeDesc = classDesc;
            if (member != null)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                while (typeDesc.BaseTypeDescription != null)
                {
                    if (member.Name.Equals(typeDesc.BaseTypeDescription.Name, StringComparison.Ordinal) == false)
                    // If InheritedClass is not the immediate parent
                    {
                        // Check if it is one of the ancestors
                        typeDesc = typeDesc.BaseTypeDescription;
                    }
                    else
                    {
                        isValidInheritedClass = true;
                        break;
                    }
                }
                if (isValidInheritedClass == false)
                {
                    StringBuilder errorMessage = new StringBuilder();
                    errorMessage.Append("You have to provide the ancestor name as an attribute for Inherited Class. ");
                    errorMessage.Append(member.Name + " is not an ancestor of " + classDesc.Name);
                    errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, errorMessage.ToString()));
                }
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// We read the xml using old logic which ignores InheritedClass and InheritedMember specification
        /// We read the xml using the new logic which takes into account InheritedClass and InheritedMember specification 
        /// We compare the generated data in both cases and make sure they are the same
        /// </summary>        
        public static List<AstGenError> ValidateAstXmlReaderWithOrdering(List<ClassDescription> originalClasses, Dictionary<string, InterfaceDescription> originalInterfaces, List<ClassDescription> classes, Dictionary<string, InterfaceDescription> interfaces)
        {
            String errorMessage = null;
            List<AstGenError> errors = new List<AstGenError>();
            if (originalClasses.Count != classes.Count)
            {
                errorMessage = "Class node counts do not match. \n Baseline Class Node Count : " + originalClasses.Count + "\n Current Class Node Count : " + classes.Count;
                errors.Add(new AstGenError(0, 0, errorMessage));
            }

            if (originalInterfaces.Count != interfaces.Count)
            {
                errorMessage = "Interface node counts do not match. \n Baseline Interface Node Count : " + originalInterfaces.Count + "\n Current Interface Node Count : " + interfaces.Count;
                errors.Add(new AstGenError(0, 0, errorMessage));
            }

            Int32 i, j;

            // Drill through the classes hierarchy and validate that everything is same
            for (i = 0; i < originalClasses.Count; i++)
            {
                for (j = 0; j < originalClasses.Count; j++)
                {
                    if (originalClasses[i].Name.Equals(classes[j].Name, StringComparison.Ordinal) == true)
                    {
                        if (CheckMembers(originalClasses, originalClasses[i], classes, classes[j]) == false)
                        {
                            errorMessage = "Class Description for " + classes[j].Name + " does not follow all rules";
                            errors.Add(new AstGenError(classes[j].LineNumber, classes[j].ColumnNumber, errorMessage));
                        }
                    }
                }
            }

            // Drill through the interfaces hierarchy and validate that everything is same
            foreach (var item in originalInterfaces)
            {
                if (interfaces.ContainsKey(item.Key) == false)
                {
                    errorMessage = "Interface Description for " + item.Key + " does not follow all rules";
                    errors.Add(new AstGenError(item.Value.LineNumber, item.Value.ColumnNumber, errorMessage));
                }
                else
                {
                    InterfaceDescription id = null;
                    if (interfaces.TryGetValue(item.Key, out id))
                    {
                        if (CheckInterfaceMembers(item.Value.members, id.members) == false)
                        {
                            errorMessage = "Interface Description for " + item.Key + " does not follow all rules";
                            errors.Add(new AstGenError(item.Value.LineNumber, item.Value.ColumnNumber, errorMessage));
                        }
                    }
                }
            }

            return errors;
        }

        /// <summary>
        /// Check whether the interface members match        
        /// </summary> 
        private static Boolean CheckInterfaceMembers(List<TypeMemberDescription> memberList1, List<TypeMemberDescription> memberList2)
        {
            HashSet<TypeMemberDescription> members1 = new HashSet<TypeMemberDescription>(memberList1);
            HashSet<TypeMemberDescription> members2 = new HashSet<TypeMemberDescription>(memberList2);
            return CheckMemberEquality(members1, members2);
        }

        /// <summary>
        /// Check whether the class members match
        /// </summary>
        private static Boolean CheckMembers(List<ClassDescription> originalClasses, ClassDescription originalClassDescription, List<ClassDescription> classes, ClassDescription classDescription)
        {
            HashSet<TypeMemberDescription> originalMembers = FlattenMembersBasedOnClassHierarchy(originalClasses, originalClassDescription);

            HashSet<TypeMemberDescription> members = new HashSet<TypeMemberDescription>(FlattenMembersBasedOnICAndIMDefinitions(classes, classDescription));

            return CheckMemberEquality(originalMembers, members);
        }

        /// <summary>
        /// Check whether the 2 member hashsets are equal
        /// </summary>
        private static Boolean CheckMemberEquality(HashSet<TypeMemberDescription> originalMembers, HashSet<TypeMemberDescription> members)
        {
            if (originalMembers.Count != members.Count)
            {
                return false;
            }

            foreach (TypeMemberDescription m in originalMembers)
            {
                if (members.Contains(m) == false)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Given a class, we flaten the members specified in the class based on the class hierarchy 
        /// (ignoring InheritedClass and InheritedMember specification)       
        /// </summary>
        private static HashSet<TypeMemberDescription> FlattenMembersBasedOnClassHierarchy(List<ClassDescription> classes, ClassDescription classDesc)
        {
            HashSet<TypeMemberDescription> members = new HashSet<TypeMemberDescription>();
            List<TypeDescription> ancestors = classDesc.GetAllAncestors();

            foreach (TypeDescription td in ancestors)
            {
                foreach (TypeMemberDescription m in GetRegularMembers(td.members))
                {
                    members.Add(m);
                }
            }
            foreach (TypeMemberDescription m in GetRegularMembers(classDesc.members))
            {
                members.Add(m);
            }

            return members;
        }

        /// <summary>
        /// Given a class, we flaten the members specified in the class based on InheritedClass and InheritedMember specification
        /// </summary>
        private static List<TypeMemberDescription> FlattenMembersBasedOnICAndIMDefinitions(List<ClassDescription> classes, ClassDescription classDesc)
        {
            List<TypeMemberDescription> members = new List<TypeMemberDescription>();
            foreach (TypeMemberDescription m in classDesc.members)
            {
                if (m.IsInheritedMember == true)
                {
                    foreach (ClassDescription classDescription in classes)
                    {
                        if (classDescription.Name.Equals(m.containerClass, StringComparison.Ordinal) == true)
                        {
                            WalkUpAndAddAllMembersToHashSet(members, classes, classDescription);
                        }
                    }
                }
                if (m.IsInheritedClass == true)
                {
                    foreach (ClassDescription classDescription in classes)
                    {
                        if (classDescription.Name.Equals(m.Name, StringComparison.Ordinal) == true)
                        {
                            // We need to go till the root to find out all memebers along that hierarchy
                            WalkUpAndAddAllMembersToHashSet(members, classes, classDescription);
                        }
                    }
                }
                if (m.IsInheritedClass == false && m.IsInheritedMember == false)
                {
                    // Regular member
                    members.Add(m);
                }
            }
            return members;
        }

        /// <summary>
        /// Given a list of members, we walk up the classes hierarchy and flatten the list based on InheritedClass and InheritedMember specification
        /// </summary>
        private static void WalkUpAndAddAllMembersToHashSet(List<TypeMemberDescription> members, List<ClassDescription> classes, ClassDescription classDesc)
        {
            foreach (TypeMemberDescription tm in FlattenMembersBasedOnICAndIMDefinitions(classes, classDesc))
            {
                members.Add(tm);
            }
        }

        /// <summary>
        /// Checks for only one inherited class in a class description
        /// </summary>
        private static List<AstGenError> ValidateForOnlyOneInheritedClass(ClassDescription classDesc)
        {
            Boolean foundInheritedClass = false;
            if (classDesc != null)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                // You can only have one Inherited Class element in a Class description
                foreach (TypeMemberDescription m in classDesc.members)
                {
                    if (m.IsInheritedClass == true)
                    {
                        if (foundInheritedClass == false)
                        {
                            foundInheritedClass = true;
                        }
                        else
                        {
                            String errorMessage = classDesc.Name + " has more than 1 inherited class definition which is not allowed";
                            errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, errorMessage));
                        }
                    }
                }
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// Checks that a class does not have an InheritedClass beyond its immediate parent
        /// This is a limitation of our implementation
        /// </summary>
        private static List<AstGenError> CheckForNoICBeyondImmediateParent(ClassDescription classDesc)
        {
            TypeMemberDescription inheritedClass = classDesc.GetInheritedClassMember();
            if (inheritedClass != null &&
                inheritedClass.Name.Equals(classDesc.BaseType, StringComparison.Ordinal) == false)
            {
                List<AstGenError> errorList = new List<AstGenError>();
                String errorMessage = classDesc.Name + " has Inherited Class definition that goes beyond its immediate parent";
                errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, errorMessage));
                return errorList;
            }
            return null;
        }

        /// <summary>
        /// Checks that we do not have the same name used for two different members in the inheritance hierachy 
        /// </summary>
        private static List<AstGenError> CheckForConflictingMemberNamesAlongInheritanceHierarchy(ClassDescription classDesc)
        {
            List<TypeDescription> ancestors = classDesc.GetAllAncestors();
            // Add the current class to the list so that we can process its members
            ancestors.Add(classDesc);

            List<AstGenError> errorList = new List<AstGenError>();
            HashSet<TypeMemberDescription> membersHash = new HashSet<TypeMemberDescription>();

            foreach (TypeDescription t in ancestors)
            {
                // We only check the regular members. 
                // Because obviously, there will be duplicates in case of InheritedMembers
                foreach (TypeMemberDescription m in GetRegularMembers(t.members))
                {
                    if (membersHash.Add(m) == false)
                    {
                        String errorMessage = classDesc.Name + " has duplicate member " + m.Name + " in its inheritance hierarchy";
                        errorList.Add(new AstGenError(classDesc.LineNumber, classDesc.ColumnNumber, errorMessage));
                    }
                }
            }
            return errorList;
        }        
    }
}
