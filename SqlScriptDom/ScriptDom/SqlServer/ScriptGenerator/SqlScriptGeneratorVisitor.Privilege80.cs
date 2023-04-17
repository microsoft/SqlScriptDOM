//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.Privilege80.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<PrivilegeType80, TokenGenerator> _privilegeType80Generators = new Dictionary<PrivilegeType80, TokenGenerator>()
        {
            {PrivilegeType80.All, new KeywordGenerator(TSqlTokenType.All)},
            {PrivilegeType80.Delete, new KeywordGenerator(TSqlTokenType.Delete)},
            {PrivilegeType80.Execute, new KeywordGenerator(TSqlTokenType.Execute)},
            {PrivilegeType80.Insert, new KeywordGenerator(TSqlTokenType.Insert)},
            {PrivilegeType80.References, new KeywordGenerator(TSqlTokenType.References)},
            {PrivilegeType80.Select, new KeywordGenerator(TSqlTokenType.Select)},
            {PrivilegeType80.Update, new KeywordGenerator(TSqlTokenType.Update)},
        };
  
        public override void ExplicitVisit(Privilege80 node)
        {
            TokenGenerator gen = GetValueForEnumKey(_privilegeType80Generators, node.PrivilegeType80);
            if (gen != null)
            {
                GenerateToken(gen);
            }

            if (node.Columns != null && node.Columns.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Columns);
            }
        }
    }
}
