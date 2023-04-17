//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExecutableStringList.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExecutableStringList node)
        {
            GenerateSymbol(TSqlTokenType.LeftParenthesis); 
	
            for (int i = 0; i < node.Strings.Count; ++i)
            {
                if (i > 0)
                {
                    GenerateSpaceAndSymbol(TSqlTokenType.Plus);
                    GenerateSpace();
                }
                GenerateFragmentIfNotNull(node.Strings[i]);
            }

            if (node.Parameters.Count > 0)
            {
                GenerateSymbolAndSpace(TSqlTokenType.Comma);
                GenerateParameters(node);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
