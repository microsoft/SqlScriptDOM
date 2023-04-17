//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AlterResourceGovernorStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AlterResourceGovernorStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Alter);
            GenerateIdentifier(CodeGenerationSupporter.Resource);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Governor);

            switch (node.Command)
            {
                case AlterResourceGovernorCommandType.Disable:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Disable);
                    break;
                case AlterResourceGovernorCommandType.Reconfigure:
                    GenerateSpaceAndKeyword(TSqlTokenType.Reconfigure);
                    break;
                case AlterResourceGovernorCommandType.ClassifierFunction:
                    GenerateResourceGovernorClassifierFunction(node);
                    break;
                case AlterResourceGovernorCommandType.ResetStatistics:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Reset);
                    GenerateSpaceAndKeyword(TSqlTokenType.Statistics);
                    break;
                default:
                    Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public void GenerateResourceGovernorClassifierFunction(AlterResourceGovernorStatement node)
        {
            GenerateSpaceAndKeyword(TSqlTokenType.With);
            GenerateSpaceAndKeyword(TSqlTokenType.LeftParenthesis);
            GenerateIdentifier(CodeGenerationSupporter.ClassifierFunction);
            GenerateSpaceAndKeyword(TSqlTokenType.EqualsSign);
            if (node.ClassifierFunction != null)
                GenerateSpaceAndFragmentIfNotNull(node.ClassifierFunction);
            else 
                GenerateSpaceAndKeyword(TSqlTokenType.Null);
            GenerateKeyword(TSqlTokenType.RightParenthesis);
        }
    }
}
