//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.LowPriorityLockWaitOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        internal void GenerateLowPriorityWaitOptions(IList<LowPriorityLockWaitOption> options)
        {
            if (options != null && options.Count > 0)
            {
                LowPriorityLockWaitMaxDurationOption maxDurationOption = null;
                LowPriorityLockWaitAbortAfterWaitOption abortAfterWaitOption = null;

                foreach (LowPriorityLockWaitOption option in options)
                {
                    if (option.OptionKind == LowPriorityLockWaitOptionKind.MaxDuration)
                    {
                        maxDurationOption = option as LowPriorityLockWaitMaxDurationOption;
                    }
                    else if (option.OptionKind == LowPriorityLockWaitOptionKind.AbortAfterWait)
                    {
                        abortAfterWaitOption = option as LowPriorityLockWaitAbortAfterWaitOption;
                    }
                }

                GenerateIdentifier(CodeGenerationSupporter.WaitAtLowPriority);
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);

                GenerateFragmentIfNotNull(maxDurationOption);
                if (maxDurationOption != null)
                {
                    GenerateSymbol(TSqlTokenType.Comma);
                    GenerateSpace();
                }

                GenerateFragmentIfNotNull(abortAfterWaitOption);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }

        public override void ExplicitVisit(LowPriorityLockWaitMaxDurationOption node)
        {
            GenerateTokenAndEqualSign(CodeGenerationSupporter.MaxDuration);
            GenerateSpaceAndFragmentIfNotNull(node.MaxDuration);
            if (node.Unit.HasValue)
            {
                GenerateSpace();
                LowPriorityLockWaitMaxDurationTimeUnitHelper.Instance.GenerateSourceForOption(_writer, node.Unit.Value);
            }
        }

        public override void ExplicitVisit(LowPriorityLockWaitAbortAfterWaitOption node)
        {
            GenerateTokenAndEqualSign(CodeGenerationSupporter.AbortAfterWait);
            GenerateSpace();
            AbortAfterWaitTypeHelper.Instance.GenerateSourceForOption(_writer, node.AbortAfterWait);
        }
    }
}
