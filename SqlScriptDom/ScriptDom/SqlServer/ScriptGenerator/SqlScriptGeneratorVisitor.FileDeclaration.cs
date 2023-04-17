//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileDeclaration.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        #region CreateAsymmetricKeyStatement

        public override void ExplicitVisit(FileDeclaration node)
        {
            if (node.IsPrimary)
            {
                GenerateKeyword(TSqlTokenType.Primary); 
            }

            GenerateParenthesisedCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(NameFileDeclarationOption node)
        {
            Debug.Assert(node.OptionKind == FileDeclarationOptionKind.Name || node.OptionKind == FileDeclarationOptionKind.NewName);
            String name = node.IsNewName ? CodeGenerationSupporter.NewName : CodeGenerationSupporter.Name;

            GenerateNameEqualsValue(name, node.LogicalFileName);
        }

        public override void ExplicitVisit(FileNameFileDeclarationOption node)
        {
            Debug.Assert(node.OptionKind == FileDeclarationOptionKind.FileName);

            if (!string.IsNullOrEmpty(node.OSFileName.Value))
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.FileName, node.OSFileName);
            }
        }

        public override void ExplicitVisit(SizeFileDeclarationOption node)
        {
            Debug.Assert(node.OptionKind == FileDeclarationOptionKind.Size);
            GenerateNameEqualsValue(CodeGenerationSupporter.Size, node.Size);
            GenerateSpaceAndMemoryUnit(node.Units);
        }

        public override void ExplicitVisit(MaxSizeFileDeclarationOption node)
        {
            Debug.Assert(node.OptionKind == FileDeclarationOptionKind.MaxSize);
            if (node.MaxSize != null)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.MaxSize, node.MaxSize);
                GenerateSpaceAndMemoryUnit(node.Units);
            }

            if (node.Unlimited)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.MaxSize, CodeGenerationSupporter.Unlimited); 
            }
        }

        public override void ExplicitVisit(FileGrowthFileDeclarationOption node)
        {
            Debug.Assert(node.OptionKind == FileDeclarationOptionKind.FileGrowth);
            GenerateNameEqualsValue(CodeGenerationSupporter.FileGrowth, node.GrowthIncrement);
            GenerateSpaceAndMemoryUnit(node.Units);
        }

        public override void ExplicitVisit(FileDeclarationOption node)
        {
            switch (node.OptionKind)
            {
                case FileDeclarationOptionKind.Offline:
                    GenerateIdentifier(CodeGenerationSupporter.Offline);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        #endregion
    }
}
