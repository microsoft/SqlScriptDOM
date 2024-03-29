//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropAssemblyStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropAssemblyStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Assembly);

            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }
            
            GenerateSpace();
            GenerateCommaSeparatedList(node.Objects);

            if (node.WithNoDependents)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.No);
                GenerateSpaceAndIdentifier(CodeGenerationSupporter.Dependents); 
            }
        }
    }
}
