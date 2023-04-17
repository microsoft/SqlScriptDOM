//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ExternalFileFormatContainerOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ExternalFileFormatContainerOption node)
        {
            ExternalFileFormatOptionHelper.Instance.GenerateSourceForOption(_writer, node.OptionKind);

            if (node.Suboptions.Count > 0)
            {
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.Suboptions);
            }
        }
    }
}