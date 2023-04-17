using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectStarExpression node)
        {
            GenerateFragmentIfNotNull(node.Qualifier);
            if (node.Qualifier != null && node.Qualifier.Count > 0)
            {
                GenerateSymbol(TSqlTokenType.Dot);
            }
            GenerateSymbol(TSqlTokenType.Star);
        }
    }

}
