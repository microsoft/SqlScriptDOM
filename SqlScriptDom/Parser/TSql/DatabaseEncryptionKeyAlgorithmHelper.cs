//------------------------------------------------------------------------------
// <copyright file="DatabaseEncryptionKeyAlgorithmHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class DatabaseEncryptionKeyAlgorithmHelper : OptionsHelper<DatabaseEncryptionKeyAlgorithm>
    {
        private DatabaseEncryptionKeyAlgorithmHelper()
        {
            AddOptionMapping(DatabaseEncryptionKeyAlgorithm.Aes128, CodeGenerationSupporter.Aes128);
            AddOptionMapping(DatabaseEncryptionKeyAlgorithm.Aes192, CodeGenerationSupporter.Aes192);
            AddOptionMapping(DatabaseEncryptionKeyAlgorithm.Aes256, CodeGenerationSupporter.Aes256);
            AddOptionMapping(DatabaseEncryptionKeyAlgorithm.TripleDes3Key, CodeGenerationSupporter.TripleDes3Key);
        }

        internal static readonly DatabaseEncryptionKeyAlgorithmHelper Instance = new DatabaseEncryptionKeyAlgorithmHelper();
    }
}