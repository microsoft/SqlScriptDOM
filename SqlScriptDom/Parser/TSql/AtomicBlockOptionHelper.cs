//------------------------------------------------------------------------------
// <copyright file="AtomicBlockOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
	[Serializable]
	internal class AtomicBlockOptionHelper : OptionsHelper<AtomicBlockOptionKind>
	{
		private AtomicBlockOptionHelper()
		{
			AddOptionMapping(AtomicBlockOptionKind.DateFirst, CodeGenerationSupporter.DateFirst, SqlVersionFlags.TSql120AndAbove);
			AddOptionMapping(AtomicBlockOptionKind.DateFormat, CodeGenerationSupporter.DateFormat, SqlVersionFlags.TSql120AndAbove);
			AddOptionMapping(AtomicBlockOptionKind.DelayedDurability, CodeGenerationSupporter.DelayedDurability, SqlVersionFlags.TSql120AndAbove);
			AddOptionMapping(AtomicBlockOptionKind.IsolationLevel, CodeGenerationSupporter.TransactionIsolationLevel, SqlVersionFlags.TSql120AndAbove);
			AddOptionMapping(AtomicBlockOptionKind.Language, CodeGenerationSupporter.Language, SqlVersionFlags.TSql120AndAbove);
		}

		internal static readonly AtomicBlockOptionHelper Instance = new AtomicBlockOptionHelper();
	}
}
