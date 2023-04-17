//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EndpointAffinity.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EndpointAffinity node)
        {
            switch (node.Kind)
            {
                case AffinityKind.Admin:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Affinity, CodeGenerationSupporter.Admin);
                    break;
                case AffinityKind.None:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Affinity, CodeGenerationSupporter.None);
                    break;
                case AffinityKind.Integer:
                    GenerateNameEqualsValue(CodeGenerationSupporter.Affinity, node.Value);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
