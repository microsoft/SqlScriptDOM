//------------------------------------------------------------------------------
// <copyright file="ExternalModelTypeOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The enumeration specifies the external model type
    /// Currently, we support EMBEDDINGS only.
    /// </summary>
    public enum ExternalModelTypeOption
    {
        /// <summary>
        /// MODEL_TYPE = EMBEDDINGS
        /// </summary>
        EMBEDDINGS = 0,

    }

#pragma warning restore 1591
}
