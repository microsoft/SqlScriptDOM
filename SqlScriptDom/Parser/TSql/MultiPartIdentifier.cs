using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class MultiPartIdentifier
    {
        /// <summary>
        /// Gets or sets the <see cref="Microsoft.SqlServer.TransactSql.ScriptDom.Identifier"/> at the specified index.
        /// </summary>
        /// <value></value>
        public Identifier this[int index]
        {
            get
            {
                return Identifiers[index];
            }
            set
            {
                Identifiers[index] = value;
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public int Count
        {
            get { return Identifiers.Count; }
        }
    }
}
