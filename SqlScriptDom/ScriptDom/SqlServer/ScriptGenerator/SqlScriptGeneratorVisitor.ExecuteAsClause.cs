//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecuteAsClause.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecuteAsClause node)
        {
            GenerateKeyword(TSqlTokenType.Execute);
            GenerateSpaceAndKeyword(TSqlTokenType.As);

            switch (node.ExecuteAsOption)
            {
                case ExecuteAsOption.Caller:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Caller); 
                    break;
                case ExecuteAsOption.Self:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Self); 
                    break;
                case ExecuteAsOption.Owner:
                    GenerateSpaceAndIdentifier(CodeGenerationSupporter.Owner); 
                    break;
                case ExecuteAsOption.Login:
                case ExecuteAsOption.String:
                case ExecuteAsOption.User:
                    GenerateSpaceAndFragmentIfNotNull(node.Literal);
                    break;
            }
        }
    }
}
