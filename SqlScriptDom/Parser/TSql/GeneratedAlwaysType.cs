//------------------------------------------------------------------------------
// <copyright file="GeneratedAlwaysType.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Possible values for 'GENERATED ALWAYS' clause
    /// </summary>
    public enum GeneratedAlwaysType
    {
        /// <summary>
        /// GENERATED ALWAYS AS ROW START
        /// </summary>
        RowStart,

        /// <summary>
        /// GENERATED ALWAYS AS ROW END
        /// </summary>
        RowEnd,

        /// <summary>
        /// GENERATED ALWAYS AS SUSER_SID START
        /// </summary>
        UserIdStart,

        /// <summary>
        /// GENERATED ALWAYS AS SUSER_SID END
        /// </summary>
        UserIdEnd,

        /// <summary>
        /// GENERATED ALWAYS AS SUSER_SNAME START
        /// </summary>
        UserNameStart,

        /// <summary>
        /// GENERATED ALWAYS AS SUSER_SNAME END
        /// </summary>
        UserNameEnd,

        /// <summary>
        /// GENERATED ALWAYS AS TRANSACTION_Id START
        /// </summary>
        TransactionIdStart,

        /// <summary>
        /// GENERATED ALWAYS AS TRANSACTION_Id END
        /// </summary>
        TransactionIdEnd,

        /// <summary>
        /// GENERATED ALWAYS AS SEQUENCE_NUMBER START
        /// </summary>
        SequenceNumberStart,

        /// <summary>
        /// GENERATED ALWAYS AS SEQUENCE_NUMBER END
        /// </summary>
        SequenceNumberEnd,


    }
}