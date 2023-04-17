//------------------------------------------------------------------------------
// <copyright file="XmlDataTypeOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// These are the possible modifiers to the xml data type.
    /// </summary>
    
    [Serializable]
    public enum XmlDataTypeOption
    {
        /// <summary>
        /// Nothing was defined.
        /// </summary>
        None = 0,

        /// <summary>
        /// CONTENT was used.
        /// </summary>
        Content = 1,

        /// <summary>
        /// DOCUMENT was used.
        /// </summary>
        Document = 2
    }
}
