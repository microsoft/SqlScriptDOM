//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ScalarUDFInline.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(InlineFunctionOption node)
        {
            GenerateIdentifier(CodeGenerationSupporter.Inline);
            GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
            GenerateSpace();
            GenerateOptionStateOnOff(node.OptionState);
        }
    }
}