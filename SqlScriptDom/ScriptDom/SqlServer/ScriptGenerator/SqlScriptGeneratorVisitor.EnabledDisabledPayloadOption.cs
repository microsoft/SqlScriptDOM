//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.EnabledDisabledPayloadOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(EnabledDisabledPayloadOption node)
        {
            TokenGenerator generator = GetValueForEnumKey(_payloadOptionKindsGenerators, node.Kind);
            if (generator != null)
            {
                GenerateNameEqualsValue(
                        generator,
                        node.IsEnabled ? CodeGenerationSupporter.Enabled : CodeGenerationSupporter.Disabled);
            }
        }
    }
}
