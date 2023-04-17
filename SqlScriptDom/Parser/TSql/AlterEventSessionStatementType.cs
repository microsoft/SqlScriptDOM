//------------------------------------------------------------------------------
// <copyright file="AlterEventSessionStatementType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of alter event session statement
    /// </summary>   
    [Serializable]
    public enum AlterEventSessionStatementType
    {
        Unknown = 0,
        AddEventDeclarationOptionalSessionOptions = 1,
        DropEventSpecificationOptionalSessionOptions = 2,
        AddTargetDeclarationOptionalSessionOptions = 3,
        DropTargetSpecificationOptionalSessionOptions = 4,
        RequiredSessionOptions = 5,
        AlterStateIsStart = 6,
        AlterStateIsStop = 7,
    }

#pragma warning restore 1591
}
