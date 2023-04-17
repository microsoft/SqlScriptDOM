using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(SelectInsertSource node)
        {
            AlignmentPoint clauseBody = new AlignmentPoint(ClauseBody);
            GenerateFragmentWithAlignmentPointIfNotNull(node.Select, clauseBody);
        }
    }
}
