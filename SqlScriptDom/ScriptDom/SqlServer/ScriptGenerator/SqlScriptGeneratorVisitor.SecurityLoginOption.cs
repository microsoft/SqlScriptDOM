//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OnOffPrincipalOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OnOffPrincipalOption node)
        {
            String optionName = String.Empty;
            switch (node.OptionKind)
            {
                case PrincipalOptionKind.CheckExpiration:
                    optionName = CodeGenerationSupporter.CheckExpiration;
                    break;
                case PrincipalOptionKind.CheckPolicy:
                    optionName = CodeGenerationSupporter.CheckPolicy;
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }

            GenerateOptionStateWithEqualSign(optionName, node.OptionState);
        }
    }
}
