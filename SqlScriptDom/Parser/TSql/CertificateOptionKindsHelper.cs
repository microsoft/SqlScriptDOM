//------------------------------------------------------------------------------
// <copyright file="CertificateOptionKindsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class CertificateOptionKindsHelper : OptionsHelper<CertificateOptionKinds>
    {
        private CertificateOptionKindsHelper()
        {
            AddOptionMapping(CertificateOptionKinds.Subject, CodeGenerationSupporter.Subject);
            AddOptionMapping(CertificateOptionKinds.StartDate, CodeGenerationSupporter.StartDate);
            AddOptionMapping(CertificateOptionKinds.ExpiryDate, CodeGenerationSupporter.ExpiryDate);
        }

        internal static readonly CertificateOptionKindsHelper Instance = new CertificateOptionKindsHelper();
    }
}
