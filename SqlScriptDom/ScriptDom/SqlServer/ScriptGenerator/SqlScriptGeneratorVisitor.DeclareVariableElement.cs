//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeclareVariableElement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DeclareVariableElement node)
        {
            GenerateFragmentIfNotNull(node.VariableName);
            GenerateSpaceAndKeyword(TSqlTokenType.As);
            GenerateSpaceAndFragmentIfNotNull(node.DataType);

            GenerateSpaceAndFragmentIfNotNull(node.Nullable);

            if (node.Value != null)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Value);
            }
        }
    }
}
