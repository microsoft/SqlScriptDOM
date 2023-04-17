//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CertificateOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

using Microsoft.SqlServer.TransactSql.ScriptDom;
using System.Collections.Generic;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        private static Dictionary<CertificateOptionKinds, String> _certificateOptionNames = new Dictionary<CertificateOptionKinds, String>()
        {
            {CertificateOptionKinds.Subject, CodeGenerationSupporter.Subject},
            {CertificateOptionKinds.StartDate, CodeGenerationSupporter.StartDate},
            {CertificateOptionKinds.ExpiryDate, CodeGenerationSupporter.ExpiryDate},
        };

        public override void ExplicitVisit(CertificateOption node)
        {
            String optionName = GetValueForEnumKey(_certificateOptionNames, node.Kind);
            if (optionName != null)
            {
                GenerateNameEqualsValue(optionName, node.Value);
            }
        }
    }
}
