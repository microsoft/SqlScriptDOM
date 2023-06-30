//------------------------------------------------------------------------------
// <copyright file="ExternalTableOptionHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    /// <summary>
    /// The external table optional property mapptings.
    /// </summary>
    internal class ExternalTableOptionHelper : OptionsHelper<ExternalTableOptionKind>
    {
        private ExternalTableOptionHelper()
        {
            AddOptionMapping(ExternalTableOptionKind.Distribution, CodeGenerationSupporter.Distribution);
            AddOptionMapping(ExternalTableOptionKind.FileFormat, CodeGenerationSupporter.FileFormat);
            AddOptionMapping(ExternalTableOptionKind.Location, CodeGenerationSupporter.Location);
            AddOptionMapping(ExternalTableOptionKind.RejectSampleValue, CodeGenerationSupporter.RejectSampleValue);
            AddOptionMapping(ExternalTableOptionKind.RejectType, CodeGenerationSupporter.RejectType);
            AddOptionMapping(ExternalTableOptionKind.RejectValue, CodeGenerationSupporter.RejectValue);
            AddOptionMapping(ExternalTableOptionKind.RejectedRowLocation, CodeGenerationSupporter.RejectedRowLocation);
            AddOptionMapping(ExternalTableOptionKind.SchemaName, CodeGenerationSupporter.SchemaName);
            AddOptionMapping(ExternalTableOptionKind.ObjectName, CodeGenerationSupporter.ObjectName);
            AddOptionMapping(ExternalTableOptionKind.TableOptions, CodeGenerationSupporter.TableOptions, SqlVersionFlags.TSql160AndAbove);
        }

        internal static readonly ExternalTableOptionHelper Instance = new ExternalTableOptionHelper();
    }
}