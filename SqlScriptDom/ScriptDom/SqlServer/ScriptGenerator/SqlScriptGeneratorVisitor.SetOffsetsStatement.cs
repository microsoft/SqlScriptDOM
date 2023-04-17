//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SetOffsetsStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        
        protected static Dictionary<SetOffsets , TokenGenerator> _setOffsetsGenerators = new Dictionary<SetOffsets , TokenGenerator>()
        {
            { SetOffsets.Compute, new KeywordGenerator(TSqlTokenType.Compute) },
            { SetOffsets.Execute, new KeywordGenerator(TSqlTokenType.Execute) },
            { SetOffsets.From, new KeywordGenerator(TSqlTokenType.From) },
            { SetOffsets.Order, new KeywordGenerator(TSqlTokenType.Order) },
            { SetOffsets.Param, new IdentifierGenerator(CodeGenerationSupporter.Param) },
            { SetOffsets.Procedure, new KeywordGenerator(TSqlTokenType.Procedure) },
            { SetOffsets.Select, new KeywordGenerator(TSqlTokenType.Select) },
            { SetOffsets.Statement, new IdentifierGenerator(CodeGenerationSupporter.Statement) },
            { SetOffsets.Table, new KeywordGenerator(TSqlTokenType.Table) },
            // we don't have to handle None: we don't do anything if it's None
            //{ SetOffsets.None, new KeywordGenerator(TSqlTokenType.TABLE) },
        };
  

        public override void ExplicitVisit(SetOffsetsStatement node)
        {
            GenerateKeyword(TSqlTokenType.Set);
            GenerateSpaceAndKeyword(TSqlTokenType.Offsets);

            GenerateSpace();
            GenerateCommaSeparatedFlagOpitons(_setOffsetsGenerators, node.Options);

            GenerateSpaceAndKeyword(node.IsOn ? TSqlTokenType.On : TSqlTokenType.Off); 
        }
    }
}
