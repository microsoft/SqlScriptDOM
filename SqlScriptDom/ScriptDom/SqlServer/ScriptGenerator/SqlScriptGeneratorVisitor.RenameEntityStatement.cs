//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RenameEntityStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(RenameEntityStatement node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Rename);

            if(node.RenameEntityType == SecurityObjectKind.Object)
            {
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Object);
            }

            if(node.SeparatorType != null)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.DoubleColon);
            }

            GenerateSpaceAndFragmentIfNotNull(node.OldName);
            GenerateSpaceAndKeyword(TSqlTokenType.To);
            GenerateSpaceAndFragmentIfNotNull(node.NewName);
        }
    }
}
