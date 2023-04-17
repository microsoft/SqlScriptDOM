using Microsoft.SqlServer.TransactSql.ScriptDom;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SqlStudio.Tests.UTSqlScriptDom
{
    internal class GenericFragmentVisitor<T> : TSqlFragmentVisitor where T : TSqlFragment
    {
        public delegate bool FragmentHandler(T tSqlFragment);

        private readonly FragmentHandler _handler;

        public GenericFragmentVisitor(FragmentHandler handler)
        {
            if(handler == null)
            {
                Assert.Fail("GenericFragmentVisitor handler should have a value.");
            }
            _handler = handler;
        }

        public override void Visit(TSqlFragment node)
        {
            base.Visit(node);

            T nodeOfActualType = node as T;
            if (nodeOfActualType != null)
            {
                Assert.IsTrue(_handler(nodeOfActualType), "GenericFragmentVisitor validation failed.");
            }
        }
    }
}
