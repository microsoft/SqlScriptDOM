using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class IdentifierOrValueExpression
    {
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get
            {
                if (this.Identifier != null)
                {
                    return this.Identifier.Value;
                }
                else if (this.ValueExpression != null)
                {
                    Literal literal = this.ValueExpression as Literal;
                    if (literal != null)
                    {
                        return literal.Value;
                    }
                    VariableReference variableReference = this.ValueExpression as VariableReference;
                    if (variableReference != null)
                    {
                        return variableReference.Name;
                    }
                    GlobalVariableExpression globalVariableExpression = this.ValueExpression as GlobalVariableExpression;
                    if (globalVariableExpression != null)
                    {
                        return globalVariableExpression.Name;
                    }
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
