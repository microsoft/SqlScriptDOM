//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateAggregateStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        #region CreateAggregateStatement

        public override void ExplicitVisit(CreateAggregateStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Aggregate);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            GenerateParenthesisedCommaSeparatedList(node.Parameters);

            NewLineAndIndent();
            GenerateIdentifier(CodeGenerationSupporter.Returns); 

            // return type
            GenerateSpaceAndFragmentIfNotNull(node.ReturnType);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.External); 
            GenerateSpaceAndIdentifier(CodeGenerationSupporter.Name); 

            // assembly name
            GenerateSpaceAndFragmentIfNotNull(node.AssemblyName);
        }

        #endregion
    }
}
