//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.ChildObjectName.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(ChildObjectName node)
        {
            if (node.ServerIdentifier != null)
            {
                GenerateFragmentIfNotNull(node.ServerIdentifier);
                GenerateSymbol(TSqlTokenType.Dot);
            }

            if (node.DatabaseIdentifier != null)
            {
                GenerateFragmentIfNotNull(node.DatabaseIdentifier);
                GenerateSymbol(TSqlTokenType.Dot);
            }

            if (node.SchemaIdentifier != null)
            {
                GenerateFragmentIfNotNull(node.SchemaIdentifier);
                GenerateSymbol(TSqlTokenType.Dot); 
            }

            if (node.BaseIdentifier != null)
            {
                GenerateFragmentIfNotNull(node.BaseIdentifier);
                GenerateSymbol(TSqlTokenType.Dot); 
            }

            GenerateFragmentIfNotNull(node.ChildIdentifier);
        }
    }
}
