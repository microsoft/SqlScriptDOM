//------------------------------------------------------------------------------
// <copyright file="PayloadOptionKinds.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------


using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    /// <summary>
    /// The types of payload options
    /// </summary>        
    [Flags]
    public enum PayloadOptionKinds
    {
        None                = 0x0000,
        WebMethod           = 0x0001,
        Batches             = 0x0002,
        Wsdl                = 0x0004,
        Sessions            = 0x0008,
        LoginType           = 0x0010,
        SessionTimeout      = 0x0020,
        Database            = 0x0040,
        Namespace           = 0x0080,
        Schema              = 0x0100,
        CharacterSet        = 0x0200,
        HeaderLimit         = 0x0400,
        Authentication      = 0x0800,
        Encryption          = 0x1000,
        MessageForwarding   = 0x2000,
        MessageForwardSize  = 0x4000,
        Role                = 0x8000,

        SoapOptions = WebMethod | Batches | Wsdl | Sessions | LoginType | SessionTimeout |
                                Database | Namespace | Schema | CharacterSet | HeaderLimit,
        ServiceBrokerOptions = Authentication | Encryption | MessageForwarding | MessageForwardSize,
        DatabaseMirroringOptions = Authentication | Encryption | Role
    }

#pragma warning restore 1591
}
