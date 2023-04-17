//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.RemoteServiceBindingStatementBase.cs" company="Microsoft">
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
        protected void GenerateBindingOptions(IList<RemoteServiceBindingOption> options)
        {
            if (options != null && options.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.With);

                GenerateSpace();
                GenerateCommaSeparatedList(options);
            }
        }

        public override void ExplicitVisit(UserRemoteServiceBindingOption node)
        {
            if (node.User != null)
            {
                GenerateNameEqualsValue(TSqlTokenType.User, node.User);
            }
        }

        public override void ExplicitVisit(OnOffRemoteServiceBindingOption node)
        {
            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.Anonymous, node.OptionState);
        }
    }
}
