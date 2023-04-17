//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LiteralPrincipalOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(LiteralPrincipalOption node)
        {
            String optionName = GetValueForEnumKey(_loginOptionsNames, node.OptionKind);
            GenerateNameEqualsValue(optionName, node.Value);
        }
    }
}
