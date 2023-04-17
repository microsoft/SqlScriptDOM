//------------------------------------------------------------------------------
// <copyright file="CursorOptionsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// Deals with DECLARE CURSOR options
    /// </summary>
    
    internal class CursorOptionsHelper : OptionsHelper<CursorOptionKind>
    {
        private CursorOptionsHelper()
        {
            AddOptionMapping(CursorOptionKind.Local, CodeGenerationSupporter.Local);
            AddOptionMapping(CursorOptionKind.Global, CodeGenerationSupporter.Global);
            AddOptionMapping(CursorOptionKind.Scroll, CodeGenerationSupporter.Scroll);
            AddOptionMapping(CursorOptionKind.ForwardOnly, CodeGenerationSupporter.ForwardOnly);
            AddOptionMapping(CursorOptionKind.Insensitive, CodeGenerationSupporter.Insensitive);
            AddOptionMapping(CursorOptionKind.Keyset, CodeGenerationSupporter.Keyset);
            AddOptionMapping(CursorOptionKind.Dynamic, CodeGenerationSupporter.Dynamic);
            AddOptionMapping(CursorOptionKind.FastForward, CodeGenerationSupporter.FastForward);
            AddOptionMapping(CursorOptionKind.ScrollLocks, CodeGenerationSupporter.ScrollLocks);
            AddOptionMapping(CursorOptionKind.Optimistic, CodeGenerationSupporter.Optimistic);
            AddOptionMapping(CursorOptionKind.ReadOnly, CodeGenerationSupporter.ReadOnly);
            AddOptionMapping(CursorOptionKind.Static, CodeGenerationSupporter.Static);
            AddOptionMapping(CursorOptionKind.TypeWarning, CodeGenerationSupporter.TypeWarning);
        }

        internal static readonly CursorOptionsHelper Instance = new CursorOptionsHelper();
    }
}
