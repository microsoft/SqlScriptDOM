//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.OptionValue.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;
namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(OnOffOptionValue node)
        {
            if (node.OptionState == OptionState.On)
                GenerateKeyword(TSqlTokenType.On);
            else if (node.OptionState == OptionState.Off)
                GenerateKeyword(TSqlTokenType.Off);
            else
                Debug.Assert(false, "Unknown OptionState!");
        }

        public override void ExplicitVisit(LiteralOptionValue node)
        {
            GenerateFragmentIfNotNull(node.Value);
        }
    }
}
