//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DiskStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Diagnostics;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DiskStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Disk);
            switch(node.DiskStatementType)
            {
                case DiskStatementType.Init:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Init);
                    break;
                case DiskStatementType.Resize:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Resize);
                    break;
                default:
                    Debug.Assert(false, "Unexpected value encountered");
                    break;
            }
            GenerateSpace();
            GenerateCommaSeparatedList(node.Options);
        }

        public override void ExplicitVisit(DiskStatementOption node)
        {
            DiskStatementOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            GenerateSpace();
            GenerateSymbol(TSqlTokenType.EqualsSign);
            GenerateSpaceAndFragmentIfNotNull(node.Value);
        }
    }
}
