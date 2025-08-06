//------------------------------------------------------------------------------
// <copyright file="SecurityObjectKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;
namespace Microsoft.SqlServer.TransactSql.ScriptDom
{

#pragma warning disable 1591
    /// <summary>
    /// The types of security objects
    /// </summary>        
    [Serializable]
    public enum SecurityObjectKind
    {
        NotSpecified = 0,
        ApplicationRole = 1,
        Assembly = 2,
        AsymmetricKey = 3,
        Certificate = 4,
        Contract = 5,
        Database = 6,
        Endpoint = 7,
        FullTextCatalog = 8,
        Login = 9,
        MessageType = 10,
        Object = 11,
        RemoteServiceBinding = 12,
        Role = 13,
        Route = 14,
        Schema = 15,
        Server = 16,
        Service = 17,
        SymmetricKey = 18,
        Type = 19,
        User = 20,
        XmlSchemaCollection = 21,
        FullTextStopList = 22,
        SearchPropertyList = 23,
        ServerRole = 24,
        AvailabilityGroup = 25,
        ExternalModel = 26,
    }

#pragma warning restore 1591
}
