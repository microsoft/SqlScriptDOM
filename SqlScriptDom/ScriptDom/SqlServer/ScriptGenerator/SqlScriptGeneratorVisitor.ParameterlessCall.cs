//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ParameterlessCall.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<ParameterlessCallType, TokenGenerator> _parameterlessCallTypeGenerators = 
            new Dictionary<ParameterlessCallType, TokenGenerator>()
        {
            {ParameterlessCallType.CurrentTimestamp, new KeywordGenerator(TSqlTokenType.CurrentTimestamp)},
            {ParameterlessCallType.CurrentUser, new KeywordGenerator(TSqlTokenType.CurrentUser)},
            {ParameterlessCallType.SessionUser, new KeywordGenerator(TSqlTokenType.SessionUser)},
            {ParameterlessCallType.SystemUser, new KeywordGenerator(TSqlTokenType.SystemUser)},
            {ParameterlessCallType.User, new KeywordGenerator(TSqlTokenType.User)},
        };
  
        public override void ExplicitVisit(ParameterlessCall node)
        {
            TokenGenerator generator = GetValueForEnumKey(_parameterlessCallTypeGenerators, node.ParameterlessCallType);
            if (generator != null)
            {
                GenerateToken(generator);
            }

			GenerateSpaceAndCollation(node.Collation);
        }
    }
}
