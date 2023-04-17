//------------------------------------------------------------------------------
// <copyright file="InterfaceDescription.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Text;
using System;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Information about interface
    /// </summary>
    class InterfaceDescription : TypeDescription
    {
        public InterfaceDescription(String name)
            : base(name)
        {
        }

        public InterfaceDescription(String name, Int32 lineNo, Int32 colNo)
            : base(name, lineNo, colNo)
        {
        }


        /// <summary>
        /// Generates spec code
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
            sb.Append("\tinternal interface " + Name);
            if (!String.IsNullOrEmpty(BaseType))
                sb.Append(", " + BaseType);
            sb.AppendLine("\r\n\t{");

            sb.AppendLine();

            foreach (TypeMemberDescription member in members)
            {
                member.isInterfaceMember = true;
                sb.Append(member.GenerateSpec());
            }

            sb.AppendLine();
            sb.AppendLine("\t}");

            if (generateNamespace)
                sb.AppendLine("}");

            sb.AppendLine();
            return sb.ToString();
        }

        /// <summary>
        /// Generates real code... which in this case, same as spec :-)
        /// </summary>
        public override String GenerateCode(bool generateNamespace, bool generateTestDLL)
        {
            return GenerateSpec(generateNamespace);
        }
    }
}