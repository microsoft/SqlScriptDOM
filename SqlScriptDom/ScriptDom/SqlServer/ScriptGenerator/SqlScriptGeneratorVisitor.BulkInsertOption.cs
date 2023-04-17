//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.BulkInsertOption.cs" company="Microsoft">
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
        public override void ExplicitVisit(OrderBulkInsertOption node)
        {
            System.Diagnostics.Debug.Assert(node.OptionKind == BulkInsertOptionKind.Order);
            GenerateKeywordAndSpace(TSqlTokenType.Order);
            GenerateParenthesisedCommaSeparatedList(node.Columns);

            if (node.IsUnique)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.Unique);
            }
        }

        public override void ExplicitVisit(BulkInsertOption node)
        {
            if (OpenRowsetBulkHintOptionsHelper.Instance.TryGenerateSourceForOption(_writer, node.OptionKind))
            {
                // nothing else to do
            }
            else
            {
                BulkInsertFlagOptionsHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);
            }
        }

        public override void ExplicitVisit(LiteralBulkInsertOption node)
        {
            if (BulkInsertIntOptionsHelper.Instance.TryGenerateSourceForOption(_writer, node.OptionKind))
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Value);
            }
            else if (BulkInsertStringOptionsHelper.Instance.TryGenerateSourceForOption(_writer, node.OptionKind))
            {
                GenerateSpaceAndSymbol(TSqlTokenType.EqualsSign);
                GenerateSpaceAndFragmentIfNotNull(node.Value);
            }
        }
    }
}
