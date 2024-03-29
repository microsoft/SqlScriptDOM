//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropTriggerStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropTriggerStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.Trigger);

            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }
            
            GenerateSpace();
            GenerateCommaSeparatedList(node.Objects);

            switch (node.TriggerScope)
            {
                case TriggerScope.Normal:
                    break;
                case TriggerScope.Database:
                    NewLineAndIndent();
                    GenerateKeyword(TSqlTokenType.On);
                    GenerateSpaceAndKeyword(TSqlTokenType.Database); 
                    break;
                case TriggerScope.AllServer:
                    NewLineAndIndent();
                    GenerateKeyword(TSqlTokenType.On);
                    GenerateSpaceAndKeyword(TSqlTokenType.All);
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Server); 
                    break;
                default:
                    System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.");
                    break;
            }
        }
    }
}
