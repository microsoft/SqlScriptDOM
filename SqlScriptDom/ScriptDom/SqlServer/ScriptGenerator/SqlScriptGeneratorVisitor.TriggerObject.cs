//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TriggerObject.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TriggerObject node)
        {
            switch (node.TriggerScope)
            {
                case TriggerScope.Normal:
                    GenerateFragmentIfNotNull(node.Name);
                    break;
                case TriggerScope.Database:
                    GenerateKeyword(TSqlTokenType.Database);
                    break;
                case TriggerScope.AllServer:
                    GenerateKeyword(TSqlTokenType.All);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
