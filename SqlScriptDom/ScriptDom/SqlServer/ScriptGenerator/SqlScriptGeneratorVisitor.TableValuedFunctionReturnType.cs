//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.TableValuedFunctionReturnType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(TableValuedFunctionReturnType node)
        {
            NewLineAndIndent();
            AlignmentPoint declareBody = new AlignmentPoint();
            MarkAndPushAlignmentPoint(declareBody);
            GenerateFragmentIfNotNull(node.DeclareTableVariableBody);
            PopAlignmentPoint();
        }
    }
}
