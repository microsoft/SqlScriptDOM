//------------------------------------------------------------------------------
// <copyright file="AlterAvailabilityGroupStatementType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The types of alter availability group statements.
    /// </summary>       
    public enum AlterAvailabilityGroupStatementType
    {
        /// <summary>
        /// Add database
        /// </summary>
        AddDatabase                 = 0,
        /// <summary>
        /// Remove database
        /// </summary>
        RemoveDatabase              = 1,
        /// <summary>
        /// Add replica
        /// </summary>
        AddReplica                  = 2,
        /// <summary>
        /// Modify replica
        /// </summary>
        ModifyReplica               = 3,
        /// <summary>
        /// Remove replica
        /// </summary>
        RemoveReplica               = 4,
        /// <summary>
        /// Set options
        /// </summary>
        Set                         = 5,
        /// <summary>
        /// Take an action
        /// </summary>
        Action                      = 6,
    }
}
