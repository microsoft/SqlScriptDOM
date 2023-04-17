//------------------------------------------------------------------------------
// <copyright file="antlrSuppressions.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
 
using System.Diagnostics.CodeAnalysis;

[module: SuppressMessage("Microsoft.Design", "CA1064:ExceptionsShouldBePublic", Scope = "type", Target = "antlr.ANTLRException")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.CharQueue.#.ctor(System.Int32)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.CharScanner.#.ctor()")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.CommonToken.#.ctor(System.Int32,System.String)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.LLkParser.#.ctor(antlr.TokenBuffer,System.Int32)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.LLkParser.#.ctor(antlr.TokenStream,System.Int32)")]
[module: SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors", Scope = "member", Target = "antlr.Token.#.ctor(System.Int32,System.String)")]
