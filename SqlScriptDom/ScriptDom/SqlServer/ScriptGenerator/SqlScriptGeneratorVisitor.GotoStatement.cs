//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.GotoStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(GoToStatement node)
        {
            GenerateKeyword(TSqlTokenType.GoTo);

            // The label reference must not be bracketed or recased: labels cannot be delimited
            // (GOTO [label] is invalid T-SQL) and the reference must keep matching the label
            // declaration, which is emitted verbatim by LabelStatement. No effect under default options.
            GenerateWithoutIdentifierFormatting(() => GenerateSpaceAndFragmentIfNotNull(node.LabelName));
        }
    }
}
