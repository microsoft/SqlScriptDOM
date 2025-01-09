//------------------------------------------------------------------------------
// <copyright file="SqlVersion.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// This enum lists the versions for Sql.
    /// </summary>
    public enum SqlVersion
    {
        /// <summary>
        /// Sql 9.0 mode, this is the default.
        /// </summary>
        Sql90 = 0,

        /// <summary>
        /// Sql 8.0 mode.
        /// </summary>
        Sql80 = 1,

        /// <summary>
        /// Sql 10.0 mode.
        /// </summary>
        Sql100 = 2,

        /// <summary>
        /// Sql 11.0 mode.
        /// </summary>
        Sql110 = 3,

        /// <summary>
        /// Sql 12.0 mode.
        /// </summary>
        Sql120 = 4,

        /// <summary>
        /// Sql 13.0 mode.
        /// </summary>
        Sql130 = 5,

        /// <summary>
        /// Sql 14.0 mode.
        /// </summary>
        Sql140 = 6,

        /// <summary>
        /// Sql 15.0 mode.
        /// </summary>
        Sql150 = 7,

        /// <summary>
        /// Sql 16.0 mode.
        /// </summary>
        Sql160 = 8,

        /// <summary>
        /// Sql 17.0 mode.
        /// </summary>
        Sql170 = 9,
    }
}
