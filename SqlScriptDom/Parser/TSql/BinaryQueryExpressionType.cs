//------------------------------------------------------------------------------
// <copyright file="BinaryQueryExpressionType.cs" company="Microsoft">
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
    /// The types of query expressions that have two query expressions as children.
    /// </summary>
    public enum BinaryQueryExpressionType
    {
        Union = 0,
        Except = 1,
        Intersect = 2,
    }


#pragma warning restore 1591
}
