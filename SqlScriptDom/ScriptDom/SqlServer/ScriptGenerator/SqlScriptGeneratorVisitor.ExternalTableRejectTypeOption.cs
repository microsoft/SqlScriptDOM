//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalTableStatementRejectTypeOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalTableRejectTypeOption node)
        {
            string externalTableRejectTypeName = GetValueForEnumKey(_externalTableRejectTypeNames, node.Value);
            if (!string.IsNullOrEmpty(externalTableRejectTypeName))
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.RejectType, externalTableRejectTypeName);
            }
        }
        
        protected static Dictionary<ExternalTableRejectType, string> _externalTableRejectTypeNames = new Dictionary<ExternalTableRejectType, string>()
        {
            {ExternalTableRejectType.Value, CodeGenerationSupporter.Value},
            {ExternalTableRejectType.Percentage, CodeGenerationSupporter.Percentage}
        };
    }
}