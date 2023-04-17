//------------------------------------------------------------------------------
// <copyright file="attachmodehelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class AttachModeHelper : OptionsHelper<AttachMode>
    {
        private AttachModeHelper()
        {
            AddOptionMapping(AttachMode.Attach,CodeGenerationSupporter.Attach);
            AddOptionMapping(AttachMode.AttachRebuildLog, CodeGenerationSupporter.AttachRebuildLog);
            AddOptionMapping(AttachMode.AttachForceRebuildLog, CodeGenerationSupporter.AttachForceRebuildLog);
            AddOptionMapping(AttachMode.Load, CodeGenerationSupporter.Load);
        }

        internal static readonly AttachModeHelper Instance = new AttachModeHelper();
    }
}
