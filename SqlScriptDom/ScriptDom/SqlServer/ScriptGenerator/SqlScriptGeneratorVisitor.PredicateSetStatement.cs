//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.PredicateSetStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        protected static Dictionary<SetOptions, TokenGenerator> _setOptionsGenerators = new Dictionary<SetOptions, TokenGenerator>()
        {
            { SetOptions.AnsiDefaults, new IdentifierGenerator(CodeGenerationSupporter.AnsiDefaults) },
            { SetOptions.AnsiNullDfltOff, new IdentifierGenerator(CodeGenerationSupporter.AnsiNullDfltOff) },
            { SetOptions.AnsiNullDfltOn, new IdentifierGenerator(CodeGenerationSupporter.AnsiNullDfltOn) },
            { SetOptions.AnsiNulls, new IdentifierGenerator(CodeGenerationSupporter.AnsiNulls) },
            { SetOptions.AnsiPadding, new IdentifierGenerator(CodeGenerationSupporter.AnsiPadding) },
            { SetOptions.AnsiWarnings, new IdentifierGenerator(CodeGenerationSupporter.AnsiWarnings) },
            { SetOptions.ArithAbort, new IdentifierGenerator(CodeGenerationSupporter.ArithAbort) },
            { SetOptions.ArithIgnore, new IdentifierGenerator(CodeGenerationSupporter.ArithIgnore) },
            { SetOptions.ConcatNullYieldsNull, new IdentifierGenerator(CodeGenerationSupporter.ConcatNullYieldsNull) },
            { SetOptions.CursorCloseOnCommit, new IdentifierGenerator(CodeGenerationSupporter.CursorCloseOnCommit) },
            { SetOptions.DisableDefCnstChk, new IdentifierGenerator(CodeGenerationSupporter.DisableDefCnstChk) },
            { SetOptions.FmtOnly, new IdentifierGenerator(CodeGenerationSupporter.FmtOnly) },
            { SetOptions.ForcePlan, new IdentifierGenerator(CodeGenerationSupporter.ForcePlan) },
            { SetOptions.ImplicitTransactions, new IdentifierGenerator(CodeGenerationSupporter.ImplicitTransactions) },
            { SetOptions.NoCount, new IdentifierGenerator(CodeGenerationSupporter.NoCount) },
            { SetOptions.NoExec, new IdentifierGenerator(CodeGenerationSupporter.NoExec) },
            { SetOptions.NumericRoundAbort, new IdentifierGenerator(CodeGenerationSupporter.NumericRoundAbort) },
            { SetOptions.ParseOnly, new IdentifierGenerator(CodeGenerationSupporter.ParseOnly) },
            { SetOptions.QuotedIdentifier, new IdentifierGenerator(CodeGenerationSupporter.QuotedIdentifier) },
            { SetOptions.RemoteProcTransactions, new IdentifierGenerator(CodeGenerationSupporter.RemoteProcTransactions) },
            { SetOptions.ShowPlanAll, new IdentifierGenerator(CodeGenerationSupporter.ShowPlanAll) },
            { SetOptions.ShowPlanText, new IdentifierGenerator(CodeGenerationSupporter.ShowPlanText) },
            { SetOptions.ShowPlanXml, new IdentifierGenerator(CodeGenerationSupporter.ShowPlanXml) },
            { SetOptions.XactAbort, new IdentifierGenerator(CodeGenerationSupporter.XactAbort) },
            { SetOptions.NoBrowsetable, new IdentifierGenerator(CodeGenerationSupporter.NoBrowsetable) },
        };
  
        public override void ExplicitVisit(PredicateSetStatement node)
        {
            GenerateKeywordAndSpace(TSqlTokenType.Set);
            GenerateCommaSeparatedFlagOpitons(_setOptionsGenerators, node.Options);
            GenerateSpaceAndKeyword(node.IsOn ? TSqlTokenType.On : TSqlTokenType.Off); 
        }
    }
}
