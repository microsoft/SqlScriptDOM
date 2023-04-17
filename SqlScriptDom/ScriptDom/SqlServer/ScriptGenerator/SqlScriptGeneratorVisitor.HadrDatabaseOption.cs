//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.HadrDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(HadrDatabaseOption node)
        {
            Debug.Assert(node.OptionKind == DatabaseOptionKind.Hadr);
            GenerateIdentifier(CodeGenerationSupporter.Hadr);
            switch (node.HadrOption)
            {
                case HadrDatabaseOptionKind.Suspend:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Suspend);
                    break;
                case HadrDatabaseOptionKind.Resume:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Resume);
                    break;
                case HadrDatabaseOptionKind.Off:
                    GenerateSpaceAndKeyword(TSqlTokenType.Off);
                    break;
                default:
                    Debug.Assert(false, "Unexpected option encountered");
                    break;
            }
        }

        public override void ExplicitVisit(HadrAvailabilityGroupDatabaseOption node)
        {
            Debug.Assert(node.HadrOption == HadrDatabaseOptionKind.AvailabilityGroup);
            Debug.Assert(node.OptionKind == DatabaseOptionKind.Hadr);
            GenerateIdentifier(CodeGenerationSupporter.Hadr);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Availability);
            GenerateSpaceAndKeyword(TSqlTokenType.Group);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.GroupName);
        }
    }
}
