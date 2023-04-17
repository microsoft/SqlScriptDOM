using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
   /// <summary>
   /// The various types of predicate SET options found in SQL.
   /// </summary>
    [Flags]
    [Serializable]
    public enum SetOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None                    = 0x00000000,
      /// <summary>
      /// QUOTED_IDENTIFIER
      /// </summary>
        QuotedIdentifier        = 0x00000001,
      /// <summary>
      /// CONCAT_NULL_YIELDS_NULL
      /// </summary>
        ConcatNullYieldsNull    = 0x00000002,
      /// <summary>
      /// CURSOR_CLOSE_ON_COMMIT
      /// </summary>
        CursorCloseOnCommit     = 0x00000004,
      /// <summary>
      /// ARITHABORT
      /// </summary>
        ArithAbort              = 0x00000008,
      /// <summary>
      /// ARITHIGNORE
      /// </summary>
        ArithIgnore             = 0x00000010,
      /// <summary>
      /// FMTONLY
      /// </summary>
        FmtOnly                 = 0x00000020,
      /// <summary>
      /// NOCOUNT
      /// </summary>
        NoCount                 = 0x00000040,
      /// <summary>
      /// NOEXEC
      /// </summary>
        NoExec                  = 0x00000080,
      /// <summary>
      /// NUMERIC_ROUNDABORT
      /// </summary>
        NumericRoundAbort       = 0x00000100,
      /// <summary>
      /// PARSEONLY
      /// </summary>
        ParseOnly               = 0x00000200,
      /// <summary>
      /// ANSI_DEFAULTS
      /// </summary>
        AnsiDefaults            = 0x00000400,
      /// <summary>
      /// ANSI_NULL_DFLT_OFF
      /// </summary>
        AnsiNullDfltOff         = 0x00000800,
      /// <summary>
      /// ANSI_NULL_DFLT_ON
      /// </summary>
        AnsiNullDfltOn          = 0x00001000,
      /// <summary>
      /// ANSI_NULLS
      /// </summary>
        AnsiNulls               = 0x00002000,
      /// <summary>
      /// ANSI_PADDING
      /// </summary>
        AnsiPadding             = 0x00004000,
      /// <summary>
      /// ANSI_WARNINGS
      /// </summary>
        AnsiWarnings            = 0x00008000,
      /// <summary>
      /// FORCEPLAN
      /// </summary>
        ForcePlan               = 0x00010000,
      /// <summary>
      /// SHOWPLAN_ALL
      /// </summary>
        ShowPlanAll             = 0x00020000,
      /// <summary>
      /// SHOWPLAN_TEXT
      /// </summary>
        ShowPlanText            = 0x00040000,
      /// <summary>
      /// IMPLICIT_TRANSACTIONS
      /// </summary>
        ImplicitTransactions    = 0x00080000,
      /// <summary>
      /// REMOTE_PROC_TRANSACTIONS
      /// </summary>
        RemoteProcTransactions  = 0x00100000,
      /// <summary>
      /// XACT_ABORT
      /// </summary>
        XactAbort               = 0x00200000,
      /// <summary>
      /// DISABLE_DEF_CNST_CHK 
      /// </summary>
        DisableDefCnstChk       = 0x00400000,
      /// <summary>
      /// SHOWPLAN_XML
      /// </summary>
        ShowPlanXml             = 0x00800000,
      /// <summary>
      /// NO_BROWSETABLE
      /// </summary>
        NoBrowsetable           = 0x01000000,
    }
}
