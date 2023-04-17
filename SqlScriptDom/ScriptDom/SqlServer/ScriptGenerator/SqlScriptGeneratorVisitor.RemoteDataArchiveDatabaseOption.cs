//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.FileStreamDatabaseOption.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        /// <summary>
        /// Script out the Alter Database statement with RemotDataArchive.
        /// </summary>
        /// <remarks>
        /// We are generating the following syntax:
        ///
        ///   ALTER DATABASE database_name
        ///   SET REMOTE_DATA_ARCHIVE = { ON | OFF }
        ///   [ ( SERVER = server_name, CREDENTIAL = { 'credential_name' | MANAGED_SERVICE_ACCOUNT }) ]
        ///
        /// OR
        ///
        ///   ALTER DATABASE database_name
        ///   SET REMOTE_DATA_ARCHIVE ( SERVER = server_name, CREDENTIAL = { 'credential_name' | MANAGED_SERVICE_ACCOUNT })
        ///
        /// </remarks>
        public override void ExplicitVisit(RemoteDataArchiveDatabaseOption node)
        {
            Debug.Assert(node.OptionKind == DatabaseOptionKind.RemoteDataArchive, "DatabaseOptionKind does not match");

            // If there was no ON / OFF explicitly set then it's the option modification clause.
            //
            if (node.OptionState == OptionState.NotSet)
            {
                GenerateIdentifier(CodeGenerationSupporter.RemoteDataArchive);
            }
            else
            {
                GenerateOptionStateWithEqualSign(CodeGenerationSupporter.RemoteDataArchive, node.OptionState);
            }

            if (node.Settings.Count > 0)
            {
                GenerateSpaceAndSymbol(TSqlTokenType.LeftParenthesis);
                bool firstOption = true;
                foreach (RemoteDataArchiveDatabaseSetting rdaSetting in node.Settings)
                {
                    if (!firstOption)
                    {
                        GenerateSymbolAndSpace(TSqlTokenType.Comma);
                    }
                    else
                    {
                        firstOption = false;
                    }

                    switch (rdaSetting.SettingKind)
                    {
                        case RemoteDataArchiveDatabaseSettingKind.Server:
                            RemoteDataArchiveDbServerSetting serverSetting =
                                rdaSetting as RemoteDataArchiveDbServerSetting;
                            GenerateNameEqualsValue(CodeGenerationSupporter.Server, serverSetting.Server);
                            break;
                        case RemoteDataArchiveDatabaseSettingKind.Credential:
                            RemoteDataArchiveDbCredentialSetting credentialSetting =
                                rdaSetting as RemoteDataArchiveDbCredentialSetting;
                            GenerateIdentifier(CodeGenerationSupporter.Credential);
                            GenerateSpace();
                            GenerateSymbol(TSqlTokenType.EqualsSign);
                            GenerateSpaceAndFragmentIfNotNull(credentialSetting.Credential);
                            break;
                        case RemoteDataArchiveDatabaseSettingKind.FederatedServiceAccount:
                            RemoteDataArchiveDbFederatedServiceAccountSetting federatedServiceAccountSetting =
                                rdaSetting as RemoteDataArchiveDbFederatedServiceAccountSetting;
                            GenerateOptionStateWithEqualSign(CodeGenerationSupporter.FederatedServiceAccount,
                                federatedServiceAccountSetting.IsOn ? OptionState.On : OptionState.Off);
                            break;
                        default:
                            System.Diagnostics.Debug.Assert(false, "An unhandled option is encountered.",
                                "Invalid RemoteDataArchiveDatabaseSettingKind: {0}", rdaSetting.SettingKind);
                            break;
                    }
                }
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }
        }
    }
}
