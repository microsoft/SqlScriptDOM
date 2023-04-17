//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.IdentityFunction.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(IdentityFunctionCall node)
        {
            GenerateKeyword(TSqlTokenType.Identity);

            GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
            GenerateFragmentIfNotNull(node.DataType);

            if (node.Seed != null)
            {
                GenerateSymbol(TSqlTokenType.Comma);  

                GenerateSpaceAndFragmentIfNotNull(node.Seed);

                GenerateSymbol(TSqlTokenType.Comma);

                GenerateSpaceAndFragmentIfNotNull(node.Increment);
            }

            GenerateSymbol(TSqlTokenType.RightParenthesis); 
        }
    }
}
