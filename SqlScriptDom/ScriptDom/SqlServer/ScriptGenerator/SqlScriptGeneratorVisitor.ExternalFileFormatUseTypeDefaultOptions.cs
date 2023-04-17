//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalFileFormatUseTypeDefaultOptions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalFileFormatUseDefaultTypeOption node)
        {
            string externalFileFormatUseDefaultTypeName = GetValueForEnumKey(_externalFileFormatUseDefaultTypeNames, node.ExternalFileFormatUseDefaultType);
            if (!string.IsNullOrEmpty(externalFileFormatUseDefaultTypeName))
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.UseTypeDefault, externalFileFormatUseDefaultTypeName);
            }
        }

        protected static Dictionary<ExternalFileFormatUseDefaultType, string> _externalFileFormatUseDefaultTypeNames = new Dictionary<ExternalFileFormatUseDefaultType, string>()
        {
            {ExternalFileFormatUseDefaultType.False, CodeGenerationSupporter.False},
            {ExternalFileFormatUseDefaultType.True, CodeGenerationSupporter.True}
        };
    }
}