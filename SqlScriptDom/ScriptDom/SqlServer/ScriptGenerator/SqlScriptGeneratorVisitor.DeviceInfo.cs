//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.DeviceInfo.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {

        private static Dictionary<DeviceType, TokenGenerator> _deviceTypeGenerators = new Dictionary<DeviceType, TokenGenerator>()
        {
            {DeviceType.DatabaseSnapshot, new IdentifierGenerator(CodeGenerationSupporter.DatabaseSnapshot)},
            {DeviceType.Disk,  new KeywordGenerator(TSqlTokenType.Disk)},
            {DeviceType.Tape, new IdentifierGenerator(CodeGenerationSupporter.Tape)},
            {DeviceType.VirtualDevice, new IdentifierGenerator(CodeGenerationSupporter.VirtualDevice)},
        };

        public override void ExplicitVisit(DeviceInfo node)
        {
            if (node.LogicalDevice != null)
            {
                GenerateFragmentIfNotNull(node.LogicalDevice);
            }
            else
            {
                TokenGenerator generator = GetValueForEnumKey(_deviceTypeGenerators, node.DeviceType);
                if (generator != null)
                {
                    GenerateNameEqualsValue(generator, node.PhysicalDevice);
                }
            }
        }
    }
}
