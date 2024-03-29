//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DropIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(DropIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Drop);
            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            if(node.IsIfExists)
            {
                GenerateSpaceAndKeyword(TSqlTokenType.If);
                GenerateSpaceAndKeyword(TSqlTokenType.Exists);
            }

            GenerateSpace();
            // could be: BackwardsCompatibleDropIndexClause or DropIndexClause
            GenerateCommaSeparatedList(node.DropIndexClauses);
        }
    }
}
