using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The various types of predicate SET STATISTICS options found in SQL.
    /// </summary>
    [Flags]
    [Serializable]
    public enum SetStatisticsOptions
    {
        None    = 0x0000,
        IO      = 0x0001,
        Profile = 0x0002,
        Time    = 0x0004,
        Xml     = 0x0008,
    }

#pragma warning restore 1591
}
