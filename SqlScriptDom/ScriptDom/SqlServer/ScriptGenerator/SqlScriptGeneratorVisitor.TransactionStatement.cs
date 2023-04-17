//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TransactionStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        protected void GenerateTransactionName(Object node)
        {
            String stringName = node as String;
            if (stringName != null)
            {
                GenerateIdentifierWithoutCasing(stringName);
                return;
            }

            TSqlFragment fragmentName = node as TSqlFragment;
            if (fragmentName != null)
            {
                GenerateFragmentIfNotNull(fragmentName);
            }
        }
    }
}
