//------------------------------------------------------------------------------
// <copyright file="EncryptionAlgorithmsHelper.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------



namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
    
    internal class EncryptionAlgorithmsHelper : OptionsHelper<EncryptionAlgorithm>
    {
        private EncryptionAlgorithmsHelper()
        {
            AddOptionMapping(EncryptionAlgorithm.RC2, CodeGenerationSupporter.RC2);
            AddOptionMapping(EncryptionAlgorithm.RC4, CodeGenerationSupporter.RC4);
            AddOptionMapping(EncryptionAlgorithm.RC4_128, CodeGenerationSupporter.RC4_128);
            AddOptionMapping(EncryptionAlgorithm.Des, CodeGenerationSupporter.Des);
            AddOptionMapping(EncryptionAlgorithm.TripleDes, CodeGenerationSupporter.TripleDes);
            AddOptionMapping(EncryptionAlgorithm.DesX, CodeGenerationSupporter.DesX);
            AddOptionMapping(EncryptionAlgorithm.Aes128, CodeGenerationSupporter.Aes128);
            AddOptionMapping(EncryptionAlgorithm.Aes192, CodeGenerationSupporter.Aes192);
            AddOptionMapping(EncryptionAlgorithm.Aes256, CodeGenerationSupporter.Aes256);
            AddOptionMapping(EncryptionAlgorithm.Rsa512, CodeGenerationSupporter.Rsa512);
            AddOptionMapping(EncryptionAlgorithm.Rsa1024, CodeGenerationSupporter.Rsa1024);
            AddOptionMapping(EncryptionAlgorithm.Rsa2048, CodeGenerationSupporter.Rsa2048);
            AddOptionMapping(EncryptionAlgorithm.TripleDes3Key, CodeGenerationSupporter.TripleDes3Key);
            AddOptionMapping(EncryptionAlgorithm.Rsa3072, CodeGenerationSupporter.Rsa3072);
            AddOptionMapping(EncryptionAlgorithm.Rsa4096, CodeGenerationSupporter.Rsa4096);
        }

        internal static readonly EncryptionAlgorithmsHelper Instance = new EncryptionAlgorithmsHelper();
    }
}
