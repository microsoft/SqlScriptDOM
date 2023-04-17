//------------------------------------------------------------------------------
// <copyright company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    internal class ExternalStreamOptionHelper : OptionsHelper<ExternalStreamOptionKind>
    {
        /// <summary>
        /// The external data source optional property mapptings.
        /// </summary>
        private ExternalStreamOptionHelper()
        {
            AddOptionMapping(ExternalStreamOptionKind.FileFormat, CodeGenerationSupporter.FileFormat);
            AddOptionMapping(ExternalStreamOptionKind.DataSource, CodeGenerationSupporter.DataSource);
        }

        internal static readonly ExternalStreamOptionHelper Instance = new ExternalStreamOptionHelper();
    }
}
