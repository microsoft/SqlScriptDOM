//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropViewStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropViewStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.View); 

            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }
            
            GenerateSpace();
            GenerateCommaSeparatedList(node.Objects);
        }
    }
}
