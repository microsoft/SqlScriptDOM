//------------------------------------------------------------------------------
// <copyright file="DialogOptionKind.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;

namespace Microsoft.SqlServer.TransactSql.ScriptDom
{
#pragma warning disable 1591

    public enum DialogOptionKind
    {
        RelatedConversation         = 0,
        RelatedConversationGroup    = 1,
        Lifetime                    = 2,
        Encryption                  = 3,
    }

#pragma warning restore 1591
}
