//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropDatabaseStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropDatabaseStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.Database); 
            
            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }
            
            GenerateSpace();
            GenerateCommaSeparatedList(node.Databases);
        }
    }
}
