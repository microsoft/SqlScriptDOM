//------------------------------------------------------------------------------
// <copyright file="SchemaObjectName.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System.Diagnostics;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    public partial class SchemaObjectName
    {
        private const int MaxIdentifiers = 5;
        private const int ServerModifier = 4;
        private const int DatabaseModifier = 3;
        private const int SchemaModifier = 2;
        private const int BaseModifier = 1;

        /// <summary>
        /// The identifier that represents the name of the server.
        /// </summary>       

        virtual public Identifier ServerIdentifier
        {
            get
            {
                Debug.Assert(Identifiers.Count <= MaxIdentifiers);

                return ChooseIdentifier(ServerModifier);
            }
        }

        /// <summary>
        /// The identifier that represents the name of the database.
        /// </summary>
        virtual public Identifier DatabaseIdentifier
        {
            get
            {
                Debug.Assert(Identifiers.Count <= MaxIdentifiers);

                return ChooseIdentifier(DatabaseModifier);
            }
        }

        /// <summary>
        /// The identifier that represents the schema of the table.
        /// </summary>
        virtual public Identifier SchemaIdentifier
        {
            get
            {
                Debug.Assert(Identifiers.Count <= MaxIdentifiers);

                return ChooseIdentifier(SchemaModifier);
            }
        }

        /// <summary>
        /// The identifier that represents the table name.
        /// </summary>
        virtual public Identifier BaseIdentifier
        {
            get
            {
                Debug.Assert(Identifiers.Count <= MaxIdentifiers);

                return ChooseIdentifier(BaseModifier);
            }
        }

        /// <summary>
        /// Figures out which index is really meant for the given property defined by the modifier.
        /// </summary>
        /// <param name="modifier">The modifier that is used to figure out the identifier.</param>
        /// <returns>The wanted identifier, null if it isn't there.</returns>
        protected Identifier ChooseIdentifier(int modifier)
        {
            int index = Identifiers.Count - modifier;

            // If index < 0 then the modifier wasn't parsed.
            if (index < 0)
            {
                return null;
            }
            else
            {
                return Identifiers[index];
            }
        }
    }
}
