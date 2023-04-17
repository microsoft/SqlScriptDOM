//------------------------------------------------------------------------------
// <copyright file="TSqlTokenTypes.g" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// This file is a single input for token id generation for all TSql versions.
// Only token id table is used (generated scanner is NOT used in any way)

header{
#pragma warning disable 1591
}

options {
    language = "CSharp"; 
    namespace = "Microsoft.SqlServer.TransactSql.ScriptDom";
}

{
}

class TSql extends Lexer;

options {
	k = 2;
	charVocabulary = '\u0000'..'\uFFFF';
	testLiterals = false;
	caseSensitive = false;
	caseSensitiveLiterals = false;
}

tokens {
	// If keywords list is changed please maintain the parser rule: securityStatementPermission
	
	// Common keywords
    Add = "add";
    All = "all";
    Alter = "alter";
    And = "and";
    Any = "any";
    As = "as";
    Asc = "asc";
    Authorization = "authorization";
    Backup = "backup";
    Begin = "begin";
    Between = "between";
    Break = "break";
    Browse = "browse";
    Bulk = "bulk";
    By = "by";
    Cascade = "cascade";
    Case = "case";
    Check = "check";
    Checkpoint = "checkpoint";
    Close = "close";
    Clustered = "clustered";
    Coalesce = "coalesce";
    Collate = "collate";
    Column = "column";
    Commit = "commit";
    Compute = "compute";
    Constraint = "constraint";
    Contains = "contains";
    ContainsTable = "containstable";
    Continue = "continue";
    Convert = "convert";
    Create = "create" ;
    Cross = "cross";
    Current = "current";
    CurrentDate = "current_date";
    CurrentTime = "current_time";
    CurrentTimestamp = "current_timestamp";
    CurrentUser = "current_user";
    Cursor = "cursor";
    Database = "database";
    Dbcc = "dbcc";
    Deallocate = "deallocate";
    Declare = "declare";
    Default = "default";
    Delete = "delete";
    Deny = "deny";
    Desc = "desc";
    Distinct = "distinct";
    Distributed = "distributed";
    Double = "double";
    Drop = "drop";
    Else = "else";
    End = "end";
    Errlvl = "errlvl";
    Escape = "escape";
    Except = "except";
    Exec = "exec";
    Execute = "execute";
    Exists = "exists";
    Exit = "exit";
    Fetch = "fetch";
    File = "file";
    FillFactor = "fillfactor";
    For = "for";
    Foreign = "foreign";
    FreeText = "freetext";
    FreeTextTable = "freetexttable";
    From = "from";
    Full = "full";
    Function = "function";
    GoTo = "goto";
    Grant = "grant";
    Group = "group";
    Having = "having";
    HoldLock = "holdlock";
    Identity = "identity";
    IdentityInsert = "identity_insert";
    IdentityColumn = "identitycol";
    If = "if";
    In = "in";
    Index = "index";
    Inner = "inner";
    Insert = "insert";
    Intersect = "intersect";
    Into = "into";
    Is = "is";
    Join = "join";
    Key = "key";
    Kill = "kill";
    Left = "left";
    Like = "like";
    LineNo = "lineno";
    National = "national";
    NoCheck = "nocheck";
    NonClustered = "nonclustered";
    Not = "not";
    Null = "null";
    NullIf = "nullif";
    Of = "of";
    Off = "off";
    Offsets = "offsets";
    On = "on";
    Open = "open";
    OpenDataSource = "opendatasource";
    OpenQuery = "openquery";
    OpenRowSet = "openrowset";
    OpenXml = "openxml";
    Option = "option";
    Or = "or";
    Order = "order";
    Outer = "outer";
    Over = "over";
    Percent = "percent";
    Plan = "plan";
    Primary = "primary";
    Print = "print";
    Proc = "proc";
    Procedure = "procedure";
    Public = "public";
    Raiserror = "raiserror";
    Read = "read";
    ReadText = "readtext";
    Reconfigure = "reconfigure";
    References = "references";
    Replication = "replication";
    Restore = "restore";
    Restrict = "restrict";
    Return = "return";
    Revoke = "revoke";
    Right = "right";
    Rollback = "rollback";
    RowCount = "rowcount";
    RowGuidColumn = "rowguidcol";
    Rule = "rule";
    Save = "save";
    Schema = "schema";
    Select = "select" ;
    SessionUser = "session_user";
    Set = "set";
    SetUser = "setuser";
    Shutdown = "shutdown";
    Some = "some";
    Statistics = "statistics";
    SystemUser = "system_user";
    Table = "table";
    TextSize = "textsize";
    Then = "then";
    To = "to";
    Top = "top";
    Tran = "tran";
    Transaction = "transaction";
    Trigger = "trigger";
    Truncate = "truncate";
    TSEqual = "tsequal";
    Union = "union";
    Unique = "unique";
    Update = "update";
    UpdateText = "updatetext";
    Use = "use";
    User = "user";
    Values = "values";
    Varying = "varying";
    View = "view";
    WaitFor = "waitfor";
    When = "when";
    Where = "where";
    While = "while";
    With = "with";
    WriteText = "writetext";
    
		// Version-specific keywords
    // Only T-SQL 80:
    Disk;
    Precision;
    
    // Only T-SQL 90:
    External;
    Revert;
    Pivot;
    Unpivot;
    TableSample;
    
    // 80 and 90
    Dump;
    Load;
    
    // Only T-SQL 100
    Merge;
    StopList;

	//Only T-SQL 110
    SemanticKeyPhraseTable;
    SemanticSimilarityTable;
    SemanticSimilarityDetailsTable;
    TryConvert;

    //Only T-SQL 120

    // Punctuations
	Bang;
	PercentSign;
	Ampersand;
	LeftParenthesis;
	RightParenthesis;
	LeftCurly;
	RightCurly;
	Star;
	MultiplyEquals;
	Plus;
	Comma;
	Minus;
	Dot;
	Divide;
	Colon;
	DoubleColon;
	Semicolon;
	LessThan;
	EqualsSign;
	RightOuterJoin;
	GreaterThan;
	Circumflex;
	VerticalLine;
	Tilde;
	// Katmai assignment operators, except *=, which is LOJ above
	// LOJ changed to MultiplyEquals after fxcop fixes
	AddEquals;
	SubtractEquals;
	DivideEquals;
	ModEquals;
	BitwiseAndEquals;
	BitwiseOrEquals;
	BitwiseXorEquals;
    LeftShift;
    RightShift;
    Concat;
    ConcatEquals;
	
	// Complex tokens
	Go;
	Label;
	Integer;
	Numeric;
	Real;
	HexLiteral;
	Money;
	SqlCommandIdentifier;
	PseudoColumn;
	DollarPartition;
	AsciiStringOrQuotedIdentifier;
	AsciiStringLiteral;
	UnicodeStringLiteral;
	Identifier;
	QuotedIdentifier;
	Variable;
	OdbcInitiator;
	ProcNameSemicolon;

	// Comments
	SingleLineComment;
	MultilineComment;
}

	// Whitespace is here to make this file ANTLR-processable; no real lexer is generated
protected
WhiteSpace : ;


