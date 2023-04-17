//------------------------------------------------------------------------------
// <copyright file="AstGenError.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System;

namespace Microsoft.VisualStudio.TeamSystem.Data.AstGen
{
    /// <summary>
    /// Error Information generated during AstGen 
    /// </summary>
    class AstGenError
    {
       #region Properties

        public Int32 LineNumber { get; private set; }
        public Int32 ColumnNumber { get; private set; }
        public String ErrorMessage { get; private set; }

        #endregion

        public AstGenError(Int32 lineNumber, Int32 columnNumber, String errorMessage)
        {
            LineNumber = lineNumber;
            ColumnNumber = columnNumber;
            ErrorMessage = errorMessage;
        }
    }
}
