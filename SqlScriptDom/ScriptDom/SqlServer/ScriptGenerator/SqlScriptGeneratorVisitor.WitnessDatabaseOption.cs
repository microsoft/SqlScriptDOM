//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.WitnessDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(WitnessDatabaseOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == DatabaseOptionKind.Witness);
            GenerateIdentifier(CodeGenerationSupporter.Witness);
            GenerateSpace();

            if (node.WitnessServer != null)
            {
                GenerateSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.WitnessServer);
            }
            else if (node.IsOff)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Off); 
            }
        }
    }
}
