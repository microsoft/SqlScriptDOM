//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.AddFileSpec.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(AddFileSpec node)
        {
            GenerateFragmentIfNotNull(node.File);
            if (node.FileName != null)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.As);
                GenerateSpaceAndFragmentIfNotNull(node.FileName);
            }
        }
    }
}
