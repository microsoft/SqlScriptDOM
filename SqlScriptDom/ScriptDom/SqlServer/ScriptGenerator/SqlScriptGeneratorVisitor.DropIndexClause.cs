//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropIndexClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropIndexClause node)
        {
            GenerateFragmentIfNotNull(node.Index);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);

            GenerateSpaceAndFragmentIfNotNull(node.Object);

            GenerateCommaSeparatedWithClause(node.Options, true, true);
        }

        public override void ExplicitVisit(MoveToDropIndexOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == IndexOptionKind.MoveTo);
            GenerateIdentifier(CodeGenerationSupporter.Move);
            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpaceAndFragmentIfNotNull(node.MoveTo);
        }

        public override void ExplicitVisit(FileStreamOnDropIndexOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == IndexOptionKind.FileStreamOn);
            GenerateIdentifier(CodeGenerationSupporter.FileStreamOn);
            GenerateSpaceAndFragmentIfNotNull(node.FileStreamOn);
        }
    }
}
