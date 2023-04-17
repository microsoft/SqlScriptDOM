//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TriggerOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TriggerOption node)
        {
            switch (node.OptionKind)
            {
                case TriggerOptionKind.Encryption:
                    GenerateIdentifier(CodeGenerationSupporter.Encryption);
                    break;
                case TriggerOptionKind.NativeCompile:
                    GenerateIdentifier(CodeGenerationSupporter.NativeCompilation);
                    break;
                case TriggerOptionKind.SchemaBinding:
                    GenerateIdentifier(CodeGenerationSupporter.SchemaBinding);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }

        public override void ExplicitVisit(ExecuteAsTriggerOption node)
        {
            GenerateFragmentIfNotNull(node.ExecuteAsClause);
        }
    }
}
