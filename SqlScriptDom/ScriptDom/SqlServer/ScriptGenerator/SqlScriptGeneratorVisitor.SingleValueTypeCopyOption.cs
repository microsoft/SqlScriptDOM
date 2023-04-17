//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.SingleValueTypeCopyOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SingleValueTypeCopyOption node)
        {
            GenerateSpaceAndFragmentIfNotNull(node.SingleValue);
        }
    }
}
