//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WsdlPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        // WSDL = { NONE | DEFAULT | 'sp_name' } 
        public override void ExplicitVisit(WsdlPayloadOption node)
        {
            if (node.IsNone)
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.Wsdl, CodeGenerationSupporter.None);
            }
            else
            {
                GenerateNameEqualsValue(CodeGenerationSupporter.Wsdl, node.Value);
            }
        }
    }
}
