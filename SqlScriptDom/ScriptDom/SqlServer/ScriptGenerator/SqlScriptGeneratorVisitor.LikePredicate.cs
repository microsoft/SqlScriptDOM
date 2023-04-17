//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LikePredicate.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LikePredicate node)
        {
            GenerateFragmentIfNotNull(node.FirstExpression);

            if (node.NotDefined)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Not); 
            }

            GenerateSpaceAndKeyword(TSqlTokenType.Like); 

            GenerateSpaceAndFragmentIfNotNull(node.SecondExpression);

            if (node.EscapeExpression != null)
            {
                GenerateSpace();

                if (node.OdbcEscape)
                {
                    GenerateSymbol(TSqlTokenType.LeftCurly); 
                }
                
                GenerateKeyword(TSqlTokenType.Escape); 

                GenerateSpaceAndFragmentIfNotNull(node.EscapeExpression);

                if (node.OdbcEscape)
                {
                    GenerateSpaceAndSymbol(TSqlTokenType.RightCurly); 
                }
            }
        }
    }
}
