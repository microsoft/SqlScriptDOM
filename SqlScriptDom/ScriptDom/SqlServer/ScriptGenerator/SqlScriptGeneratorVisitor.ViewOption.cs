//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ViewOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected static Dictionary<ViewOptionKind, String> _viewOptionTypeNames = new Dictionary<ViewOptionKind, String>()
        {
            { ViewOptionKind.Encryption, CodeGenerationSupporter.Encryption },
            { ViewOptionKind.SchemaBinding, CodeGenerationSupporter.SchemaBinding },
            { ViewOptionKind.ViewMetadata, CodeGenerationSupporter.ViewMetadata },
        };
  
        public override void ExplicitVisit(ViewOption node)
        {
            String optionName = GetValueForEnumKey(_viewOptionTypeNames, node.OptionKind);
            if (optionName != null)
            {
                GenerateIdentifier(optionName); 
            }
        }
    }
}
