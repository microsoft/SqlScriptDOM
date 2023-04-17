//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TriggerAction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TriggerAction node)
        {
            switch (node.TriggerActionType)
            {
                case TriggerActionType.Delete:
                    GenerateKeyword(TSqlTokenType.Delete);
                    break;
                case TriggerActionType.Insert:
                    GenerateKeyword(TSqlTokenType.Insert);
                    break;
                case TriggerActionType.Update:
                    GenerateKeyword(TSqlTokenType.Update);
                    break;
                case TriggerActionType.Event:
                    GenerateFragmentIfNotNull(node.EventTypeGroup);
                    break;
                case TriggerActionType.LogOn:
                    GenerateIdentifier(CodeGenerationSupporter.Logon);
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
