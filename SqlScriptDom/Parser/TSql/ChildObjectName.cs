//------------------------------------------------------------------------------
// <copyright file="ChildObjectName.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class ChildObjectName
    {
        // Here we have shifted (by 1) modifiers compared with SchemaObjecName
        private const int ServerModifierNumber = 5;
        private const int DatabaseModifierNumber = 4;
        private const int SchemaModifierNumber = 3;
        private const int BaseModifierNumber = 2;
        private const int ChildModifierNumber = 1;

        #region Redefined properties from base class

        /// <summary>
        /// Gets the base identifier.
        /// </summary>
        /// <value>The base identifier.</value>
        public override Identifier BaseIdentifier
        {
            get
            {
                return ChooseIdentifier(BaseModifierNumber);
            }
        }

        /// <summary>
        /// Gets the database identifier.
        /// </summary>
        /// <value>The database identifier.</value>
        public override Identifier DatabaseIdentifier
        {
            get
            {
                return ChooseIdentifier(DatabaseModifierNumber);
            }
        }

        /// <summary>
        /// Gets the schema identifier.
        /// </summary>
        /// <value>The schema identifier.</value>
        public override Identifier SchemaIdentifier
        {
            get
            {
                return ChooseIdentifier(SchemaModifierNumber);
            }
        }

        /// <summary>
        /// Gets the server identifier.
        /// </summary>
        /// <value>The server identifier.</value>
        public override Identifier ServerIdentifier
        {
            get
            {
                return ChooseIdentifier(ServerModifierNumber);
            }
        }
        #endregion

        /// <summary>
        /// Gets the child identifier.
        /// </summary>
        /// <value>The child identifier.</value>
        virtual public Identifier ChildIdentifier
        {
            get
            {
                return ChooseIdentifier(ChildModifierNumber);
            }
        }
    }
}
