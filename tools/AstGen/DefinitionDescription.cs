//------------------------------------------------------------------------------
// <copyright file="DefinitionDescription.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Text;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Information about some definition
    /// </summary>
    abstract class DefinitionDescription
    {
        public DefinitionDescription(String name)
        {
            this.name = name;
            this.LineNumber = 0;
            this.ColumnNumber = 0;
        }

        public DefinitionDescription(String name, Int32 lineNo, Int32 colNo)
        {
            this.name = name;
            this.LineNumber = lineNo;
            this.ColumnNumber = colNo;
        }

        // Fields
        String name;
        String summary = "";

        public Int32 LineNumber { get; private set; }
        public Int32 ColumnNumber { get; private set; }

        public const String NamespaceName = "namespace Microsoft.VisualStudio.TeamSystem.Data.Parser.TSql";
        public const String ScriptGenNamespaceName = "namespace Microsoft.SqlServer.TransactSql.ScriptDom";

        // Properties
        public String Name
        {
            get { return name; }
        }

        public String Summary
        {
            get { return summary; }
            set { if (value != null) summary = value; }
        }

        /// <summary>
        /// Helper to set a boolean property from String input
        /// </summary>
        protected static void SetBooleanPropertyUsingString(String str, ref bool propValue)
        {
            if (str != null)
            {
                if (String.Equals(str, "true", StringComparison.OrdinalIgnoreCase))
                    propValue = true;
                else if (String.Equals(str, "false", StringComparison.OrdinalIgnoreCase))
                    propValue = false;
            }
        }

        /// <summary>
        /// Writes summary section to StringBuilder
        /// </summary>
        public static void GenerateSummary(StringBuilder sb, String summary, String tabs)
        {
            if (!String.IsNullOrEmpty(summary))
            {
                sb.AppendLine(tabs + "/// <summary>");
                foreach (String summaryLine in summary.Split('\n'))
                    sb.AppendFormat(tabs + "/// {0}\r\n", summaryLine.Trim());
                sb.AppendLine(tabs + "/// </summary>");
            }
        }

        /// <summary>
        /// Writes summary section to StringBuilder
        /// </summary>
        /// <param name="sb">The string builder.</param>
        /// <param name="summary">The summary.</param>
        /// <param name="tabs">The tabs.</param>
        /// <param name="parameterName">Name of the parameter.</param>
        /// <param name="parameterDesc">The parameter description.</param>
        public static void GenerateSummary(StringBuilder sb, string summary, string tabs, string parameterName, string parameterDesc)
        {
            sb.AppendLine(tabs + "/// <summary>");
            foreach (string summaryLine in summary.Split('\n'))
                sb.AppendFormat(tabs + "/// {0}\r\n", summaryLine.Trim());
            sb.AppendLine(tabs + "/// </summary>");
            sb.AppendFormat("{0}/// <param name=\"{1}\">{2}</param>\r\n", tabs, parameterName, parameterDesc);
        }
    }
}