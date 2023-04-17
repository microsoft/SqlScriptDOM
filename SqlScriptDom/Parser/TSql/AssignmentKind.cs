//------------------------------------------------------------------------------
// <copyright file="AssignmentKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of assignment
    /// </summary>   
    public enum AssignmentKind
    {
        Equals              = 0,
        AddEquals           = 1,
        SubtractEquals      = 2,
        MultiplyEquals      = 3,
        DivideEquals        = 4,
        ModEquals           = 5,
        BitwiseAndEquals    = 6,
        BitwiseOrEquals     = 7,
        BitwiseXorEquals    = 8,
        ConcatEquals        = 9
    }

#pragma warning restore 1591
}
