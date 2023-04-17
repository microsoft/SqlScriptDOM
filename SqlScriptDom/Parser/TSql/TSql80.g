//------------------------------------------------------------------------------
// <copyright file="TSql80.g" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

// sacaglar: Handling position information for ASTs
// The properties and AddX (X is the type of the parameter, e.g., AddStatement)
// update the position information of the AST.  In the rare case when
// we are setting a bool, int, or string etc. we have to call UpdateTokenInfo
// with the token (because we are not passing the property a token, we are just 
// passing a bool, int etc.  
// Also for token that we don't track of like Comma, Semicolon etc. we have to
// call the same function.  Alternatively the properties(StartOffset, FragmentLength) 
// on Fragment.cs can be used for this purpose.

options {
    language = "CSharp";
    namespace = "Microsoft.SqlServer.TransactSql.ScriptDom";
}

{
    using System.Diagnostics;
    using System.Globalization;
    using System.Collections.Generic;
}

class TSql80ParserInternal extends Parser("TSql80ParserBaseInternal");
options {
    k = 2;
    defaultErrorHandler=false;
    classHeaderPrefix = "internal partial";
    importVocab = TSql;
}

{
    public TSql80ParserInternal(bool initialQuotedIdentifiersOn) 
        : base(initialQuotedIdentifiersOn)
    {
        initialize();
    }
}

// Figure out a way to refactor exception handling
entryPointChildObjectName returns [ChildObjectName vResult = null]
    : 
        vResult=childObjectNameWithThreePrefixes
        EOF 
    ;

entryPointSchemaObjectName returns [SchemaObjectName vResult = null]
    : 
        vResult=schemaObjectFourPartName
        EOF 
    ;

entryPointScalarDataType returns [DataTypeReference vResult = null]
    :
        vResult = scalarDataType
        EOF
    ;

entryPointExpression returns [ScalarExpression vResult = null]
    :
        vResult = expression
        EOF
    ;
    
entryPointBooleanExpression returns [BooleanExpression vResult = null]
    :
        vResult = booleanExpression
        EOF
    ;
    
entryPointStatementList returns [StatementList vResult = null]
{
    bool vParseErrorOccurred = false;
}
    :
        vResult = statementList[ref vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
                vResult = null;
        }
        EOF
    ;
    
entryPointSubqueryExpressionWithOptionalCTE returns [SelectStatement vResult = null]
    :
        vResult = subqueryExpressionAsStatement
        EOF
    ;
    
entryPointConstantOrIdentifier returns [TSqlFragment vResult = null]
    :
        vResult = possibleNegativeConstantOrIdentifier
        EOF
    ;

entryPointConstantOrIdentifierWithDefault returns [TSqlFragment vResult = null]
    :
        vResult = possibleNegativeConstantOrIdentifierWithDefault
        EOF
    ;
    
script returns [TSqlScript vResult = this.FragmentFactory.CreateFragment<TSqlScript>()]
{ 
    TSqlBatch vCurrentBatch;

    // Script always includes all of the tokens...
    if (vResult.ScriptTokenStream != null && vResult.ScriptTokenStream.Count > 0)
    {
        vResult.UpdateTokenInfo(0,vResult.ScriptTokenStream.Count-1);
    }
}
    : vCurrentBatch = batch 
        { 
            if (vCurrentBatch != null)
                AddAndUpdateTokenInfo(vResult, vResult.Batches, vCurrentBatch); 
        } 
        ( 
            Go
            {
                ResetQuotedIdentifiersSettingToInitial();
                ThrowPartialAstIfPhaseOne(null);
            }
            vCurrentBatch = batch
            {
                if (vCurrentBatch != null)
                    AddAndUpdateTokenInfo(vResult, vResult.Batches, vCurrentBatch); 
            }

        )* 
        tEof:EOF 
        {
            UpdateTokenInfo(vResult,tEof);
        }
    ;

// TODO, sacaglar: Tracking issue, bug# 584772
batch returns [TSqlBatch vResult = null]
{
    TSqlStatement vStatement;
}
    : 
      (        
        (
           Create ( Proc | Procedure | Trigger | Default | Rule | View | Function | Schema )
         |
           Alter ( Proc | Procedure | Trigger | View | Function )
        )=>
        ( 
            vStatement=lastStatement
            {
                if (vStatement != null)
                {    
                    if (vResult == null)
                        vResult = FragmentFactory.CreateFragment<TSqlBatch>();
                        
                    AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
                }
            }
        )
        |
        (vStatement = optSimpleExecute
            {
                if (vStatement != null) // Can be empty
                {
                    ThrowPartialAstIfPhaseOne(vStatement);

                    if (vResult == null)
                        vResult = this.FragmentFactory.CreateFragment<TSqlBatch>();
                        
                    AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
                }
            }
            (vStatement=statementOptSemi 
                {
                    if (vStatement != null) // statement can be null if there was a parse error.
                    {
                        if (vResult == null)
                            vResult = this.FragmentFactory.CreateFragment<TSqlBatch>();
                            
                        AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
                    }
                }
            )*
        )
      )
    ;
    exception
    catch[TSqlParseErrorException exception]
    {
        if (!exception.DoNotLog)
        {
            AddParseError(exception.ParseError);
        }
        RecoverAtBatchLevel();
    }
    catch[antlr.NoViableAltException exception]
    {
        ParseError error = GetFaultTolerantUnexpectedTokenError(
            exception.token, exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtBatchLevel();
    }
    catch[antlr.MismatchedTokenException exception]
    {
        ParseError error = GetFaultTolerantUnexpectedTokenError(
            exception.token, exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtBatchLevel();
    }
    catch[antlr.RecognitionException]
    {
        ParseError error = GetUnexpectedTokenError();
        AddParseError(error);
        RecoverAtBatchLevel();
    }
    catch[antlr.TokenStreamRecognitionException exception]
    {
        // This exception should be handled when we are creating TSqlTokenStream...
        ParseError error = ProcessTokenStreamRecognitionException(exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtBatchLevel();
    }
    catch[antlr.ANTLRException exception]
    {
        CreateInternalError("batch", exception);
    }

statementOptSemi returns [TSqlStatement vResult = null]
    : vResult=statement optSingleSemicolon[vResult]
    ;
        
optSingleSemicolon[TSqlStatement vParent]
    : (
            // Greedy behavior is good enough, we ignore the semicolons
            options {greedy = true; } :
            tSemi:Semicolon
        {
            if (vParent != null) // vResult can be null if there was a parse error.
                UpdateTokenInfo(vParent,tSemi);
        }
      )?
    ;

optSimpleExecute returns [ExecuteStatement vResult = null]
{
    ExecutableProcedureReference vExecProc;
    ExecuteSpecification vExecuteSpecification;
}
    : (vExecProc = execProc 
        {
            vResult = FragmentFactory.CreateFragment<ExecuteStatement>();
            vExecuteSpecification = FragmentFactory.CreateFragment<ExecuteSpecification>();
            vExecuteSpecification.ExecutableEntity = vExecProc;
            vResult.ExecuteSpecification=vExecuteSpecification;
        }
        optSingleSemicolon[vResult]
        )
    | /* empty */
    ;

statement returns [TSqlStatement vResult = null]
{
    // The next tokens offset is cached to help error 
    // recovery, so when error occurs if the next token is 
    // Create or Alter, and its offset is the same as
    // vNextTokenOffset that means, this rule already 
    // tried to parsed and failed, so we should skip over.
    // The case where it works is:
    //    select * from    create table t1(c1 int)
    int nextTokenLine = LT(1).getLine();
    int nextTokenColumn = LT(1).getColumn();
}
    : vResult=createTableStatement
    | vResult=alterTableStatement
    | vResult=createIndexStatement
    | vResult=declareStatements
    | vResult=setStatements
    | vResult=beginStatements
    | vResult=breakStatement
    | vResult=continueStatement
    | vResult=ifStatement
    | vResult=whileStatement
    | vResult=labelStatement
    | vResult=backupStatement
    | vResult=restoreStatement
    | vResult=gotoStatement
    | vResult=saveTransactionStatement
    | vResult=rollbackTransactionStatement
    | vResult=commitTransactionStatement
    | vResult=createStatisticsStatement
    | vResult=updateStatisticsStatement
    | vResult=alterDatabaseStatements
    | vResult=executeStatement
    | vResult=select
    | vResult=deleteStatement
    | vResult=insertStatement
    | vResult=updateStatement
    | vResult=raiseErrorStatements
    | vResult=createDatabaseStatement
    | vResult=printStatement
    | vResult=waitForStatement
    | vResult=readTextStatement
    | vResult=updateTextStatement
    | vResult=writeTextStatement
    | vResult=lineNoStatement
    | vResult=useStatement
    | vResult=killStatement
    | vResult=bulkInsertStatement
    | vResult=insertBulkStatement
    | vResult=checkpointStatement
    | vResult=reconfigureStatement
    | vResult=shutdownStatement
    | vResult=setUserStatement
    | vResult=truncateTableStatement
    | vResult=grantStatement80
    | vResult=denyStatement80
    | vResult=revokeStatement80
    | vResult=returnStatement
    | vResult=openCursorStatement
    | vResult=closeCursorStatement
    | vResult=deallocateCursorStatement
    | vResult=fetchCursorStatement
    | vResult=dropStatements
    | vResult=dbccStatement
    | vResult=revertStatement
    | vResult=diskStatement
    ;
    exception
    catch[TSqlParseErrorException exception]
    {
        if (!exception.DoNotLog)
        {
            AddParseError(exception.ParseError);
        }
        RecoverAtStatementLevel(nextTokenLine, nextTokenColumn);
    }
    catch[antlr.NoViableAltException exception]
    {
        ParseError error = GetFaultTolerantUnexpectedTokenError(
            exception.token, exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtStatementLevel(nextTokenLine, nextTokenColumn);
    }
    catch[antlr.MismatchedTokenException exception]
    {
        ParseError error = GetFaultTolerantUnexpectedTokenError(
            exception.token, exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtStatementLevel(nextTokenLine, nextTokenColumn);
    }
    catch[antlr.RecognitionException]
    {
        ParseError error = GetUnexpectedTokenError();
        AddParseError(error);
        RecoverAtStatementLevel(nextTokenLine, nextTokenColumn);
    }
    catch[antlr.TokenStreamRecognitionException exception]
    {
        // This exception should be handled when we are creating TSqlTokenStream...
        ParseError error = ProcessTokenStreamRecognitionException(exception, _tokenSource.LastToken.Offset);
        AddParseError(error);
        RecoverAtStatementLevel(nextTokenLine, nextTokenColumn);
    }
    catch[antlr.ANTLRException exception]
    {
        CreateInternalError("statement", exception);
    }

lastStatement returns [TSqlStatement vResult = null]
    : vResult=createProcedureStatement 
    | vResult=alterProcedureStatement
    | vResult=createTriggerStatement
    | vResult=alterTriggerStatement
    | vResult=createDefaultStatement
    | vResult=createRuleStatement
    | vResult=createViewStatement
    | vResult=alterViewStatement
    | vResult=createFunctionStatement
    | vResult=alterFunctionStatement
    | vResult=createSchemaStatement
    ;


//////////////////////////////////////////////////////////////////////
// Alter Database
//////////////////////////////////////////////////////////////////////
alterDatabaseStatements returns [AlterDatabaseStatement vResult = null]
{
    Identifier vIdentifier = null;
}
    : tAlter:Alter Database 
        ( 
            vIdentifier=identifier 
        | 
            vIdentifier=sqlCommandIdentifier
        )
        ( vResult = alterDbAdd
        | {NextTokenMatches(CodeGenerationSupporter.Remove)}?
          vResult = alterDbRemove
        | {NextTokenMatches(CodeGenerationSupporter.Modify)}? 
          vResult = alterDbModify
        | vResult = alterDbSet
        | vResult = alterDbCollate
        )
        {
            vResult.DatabaseName = vIdentifier;
            UpdateTokenInfo(vResult,tAlter);
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;
    exception
    catch[PhaseOnePartialAstException exception]
    {
        UpdateTokenInfo(exception.Statement,tAlter);
        (exception.Statement as AlterDatabaseStatement).DatabaseName = vIdentifier;
        throw;
    }

alterDbCollate returns [AlterDatabaseCollateStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseCollateStatement>()]
    : collation[vResult]
    ;
    
alterDbAdd returns [AlterDatabaseStatement vResult = null]
    : Add 
        (
            vResult = alterDbAddFile
        | 
            vResult = alterDbAddFilegroup
        )
    ;
    
// Add File / Add LOG File
alterDbAddFile returns [AlterDatabaseAddFileStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseAddFileStatement>()]
{
    Identifier vIdentifier;
}
    :    (tLog:Identifier
            {
                Match(tLog,CodeGenerationSupporter.Log);
                vResult.IsLog = true;
            }
        )? 
        File 
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }
        fileDeclBodyList[vResult, vResult.FileDeclarations]
        (vIdentifier = toFilegroup
            {
                if (vResult.IsLog)
                    throw GetUnexpectedTokenErrorException(vIdentifier);

                vResult.FileGroup = vIdentifier;
            }
        )?
    ;
    
// Add FILEGROUP
alterDbAddFilegroup returns [AlterDatabaseAddFileGroupStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseAddFileGroupStatement>()]
{
    Identifier vIdentifier;
}
    : tFilegroup:Identifier vIdentifier=identifier
        {
            Match(tFilegroup, CodeGenerationSupporter.Filegroup);
            vResult.FileGroup = vIdentifier;
        }
    ;
        
alterDbRemove returns [AlterDatabaseStatement vResult = null]
{
    Identifier vIdentifier;
}
    : tRemove:Identifier 
        {
            Match(tRemove,CodeGenerationSupporter.Remove);
        }
        (File vIdentifier = identifier 
            {
                AlterDatabaseRemoveFileStatement removeFile = FragmentFactory.CreateFragment<AlterDatabaseRemoveFileStatement>();
                removeFile.File = vIdentifier;
                vResult = removeFile;
            }
        |
        tFileGroup:Identifier vIdentifier = identifier
            {
                // REMOVE FILEGROUP
                Match(tFileGroup,CodeGenerationSupporter.Filegroup);
                AlterDatabaseRemoveFileGroupStatement vRemoveFilegroup = FragmentFactory.CreateFragment<AlterDatabaseRemoveFileGroupStatement>();
                vRemoveFilegroup.FileGroup = vIdentifier;
                vResult = vRemoveFilegroup;
            }
        )
    ;
    
alterDbModify returns [AlterDatabaseStatement vResult = null]
{
    Identifier vIdentifier;
}
    : tModify:Identifier
        {
            Match(tModify,CodeGenerationSupporter.Modify);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Name)}?        
            (tName:Identifier EqualsSign vIdentifier = identifier
                {
                    // MODIFY NAME = <newname>
                    Match(tName,CodeGenerationSupporter.Name);
                    AlterDatabaseModifyNameStatement modifyDbName = FragmentFactory.CreateFragment<AlterDatabaseModifyNameStatement>();
                    modifyDbName.NewDatabaseName = vIdentifier;
                    vResult = modifyDbName;
                }
            )
        |
            (tFileGroup2:Identifier 
                {
                    Match(tFileGroup2,CodeGenerationSupporter.Filegroup);
                }
                vResult = alterDbModifyFilegroup
            )
        |    vResult = alterDbModifyFile
        )
    ;
    
// MODIFY File syntax
alterDbModifyFile returns [AlterDatabaseModifyFileStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseModifyFileStatement>()]
{
    FileDeclaration vFileDecl;
}
    : File 
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vFileDecl = fileDecl[true]
        {
            vResult.FileDeclaration = vFileDecl;
        }
    ;
    
alterDbModifyFilegroup returns [AlterDatabaseModifyFileGroupStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseModifyFileGroupStatement>()]
{
    Identifier vIdentifier, vIdentifier2;
}
    : vIdentifier = identifier
        {
            vResult.FileGroup = vIdentifier;
        }
        (
            (tName2:Identifier EqualsSign vIdentifier2 = identifier
                {
                    // MODIFY FILEGROUP <name> NAME = <newname>
                    Match(tName2,CodeGenerationSupporter.Name);
                    vResult.NewFileGroupName = vIdentifier2;
                    ThrowPartialAstIfPhaseOne(vResult);
                }
            )
        | tDefault:Default
            {
                // MODIFY FILEGROUP <name> Default
                vResult.MakeDefault = true;
                UpdateTokenInfo(vResult,tDefault);
            }
        | 
            (tUpdatabilityOption:Identifier 
                {
                    // MODIFY FILEGROUP <name> <option>
                    ThrowPartialAstIfPhaseOne(vResult);
                    
                    vResult.UpdatabilityOption = ModifyFilegroupOptionsHelper.Instance.ParseOption(tUpdatabilityOption, SqlVersionFlags.TSql80);
                    UpdateTokenInfo(vResult,tUpdatabilityOption);
                }
            )
        )
    ;
    
alterDbSet returns [AlterDatabaseSetStatement vResult]
{
    AlterDatabaseTermination vTermination;
}
    : Set vResult = dbOptionStateList 
        (
            vTermination = xactTermination
            {
                vResult.Termination = vTermination;
            }
        )?
    ;
    
toFilegroup returns [Identifier vResult]
    : To tFilegroup:Identifier vResult = identifier
        {
            Match(tFilegroup,CodeGenerationSupporter.Filegroup);
        }
    ;
    
xactTermination returns [AlterDatabaseTermination vResult = FragmentFactory.CreateFragment<AlterDatabaseTermination>()]
{
    Literal vInteger;
}
    : tWith:With 
        {
            UpdateTokenInfo(vResult,tWith);
        }
        ( Rollback 
            (
                (tAfter:Identifier vInteger = integer 
                    {
                        Match(tAfter,CodeGenerationSupporter.After);
                        vResult.RollbackAfter = vInteger;
                    }
                    (tSeconds:Identifier
                        {
                            Match(tSeconds,CodeGenerationSupporter.Seconds);
                            UpdateTokenInfo(vResult,tSeconds);
                        }
                    )?
                )
                |
                tImmediate:Identifier
                {
                    Match(tImmediate,CodeGenerationSupporter.Immediate);
                    UpdateTokenInfo(vResult,tImmediate);
                    vResult.ImmediateRollback = true;
                }
            )
        | tNoWait:Identifier
            {
                Match(tNoWait,CodeGenerationSupporter.NoWaitAlterDb);
                UpdateTokenInfo(vResult,tNoWait);
                vResult.NoWait = true;
            }
        )
    ;
    
dbOptionStateList returns [AlterDatabaseSetStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseSetStatement>()]
{
    DatabaseOption vOption;
}
    : vOption = dbOptionStateItem 
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
        (Comma vOption = dbOptionStateItem
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )*
    ;
    
dbOptionStateItem returns [DatabaseOption vResult = null]
    :    
        {NextTokenMatches(CodeGenerationSupporter.CursorDefault)}? 
        vResult = cursorDefaultDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.Recovery)}? 
        vResult = recoveryDbOption
    |    vResult = dbSingleIdentOption // <db_state_option>, <db_user_access_option>, <db_update_option>, <service_broker_option>
    |    vResult = alterDbOnOffOption
    ;
    
dbSingleIdentOption returns [DatabaseOption vResult = FragmentFactory.CreateFragment<DatabaseOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind = SimpleDbOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
            UpdateTokenInfo(vResult,tOption);
        }
    ;
    
cursorDefaultDbOption returns [CursorDefaultDatabaseOption vResult = FragmentFactory.CreateFragment<CursorDefaultDatabaseOption>()]
    : tCursorDefault:Identifier tLocalGlobal:Identifier
        {
            Match(tCursorDefault,CodeGenerationSupporter.CursorDefault);
            vResult.OptionKind = DatabaseOptionKind.CursorDefault;
            if (TryMatch(tLocalGlobal, CodeGenerationSupporter.Local))
                vResult.IsLocal = true;
            else
                {
                    Match(tLocalGlobal, CodeGenerationSupporter.Global);
                    vResult.IsLocal = false;
                }
            UpdateTokenInfo(vResult,tLocalGlobal);
        }
    ;
    
recoveryDbOption returns [RecoveryDatabaseOption vResult = FragmentFactory.CreateFragment<RecoveryDatabaseOption>()]
    : tRecovery:Identifier 
        {
            Match(tRecovery,CodeGenerationSupporter.Recovery);
            vResult.OptionKind = DatabaseOptionKind.Recovery;
        }
        (
            tFull:Full
                {
                    vResult.Value = RecoveryDatabaseOptionKind.Full;
                    UpdateTokenInfo(vResult,tFull);
                }
            |
            tBulkLoggedSimple:Identifier
                {
                    vResult.Value = RecoveryDbOptionsHelper.Instance.ParseOption(tBulkLoggedSimple);
                    UpdateTokenInfo(vResult,tBulkLoggedSimple);
                }
        )
    ;
            
alterDbOnOffOption returns [OnOffDatabaseOption vResult = FragmentFactory.CreateFragment<OnOffDatabaseOption>()]
{
    OptionState vOptionState;
}
    : tOption:Identifier vOptionState = optionOnOff[vResult]
        {
            vResult.OptionKind = OnOffSimpleDbOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
            vResult.OptionState = vOptionState;
        }
    ;
    
//////////////////////////////////////////////////////////////////////
// Alter Database End
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// Create Database
//////////////////////////////////////////////////////////////////////

createDatabaseStatement returns [CreateDatabaseStatement vResult = FragmentFactory.CreateFragment<CreateDatabaseStatement>()]
{
    Identifier vIdentifier;
}
    : tCreate:Create Database vIdentifier=identifier 
        {
            vResult.DatabaseName = vIdentifier;
            UpdateTokenInfo(vResult,tCreate);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        recoveryUnitList[vResult] 
        collationOpt[vResult]
        (
            dbAddendums[vResult]
        )? 
    ;
    
recoveryUnitList [CreateDatabaseStatement vParent]
    : (onDisk[vParent])? 
      (
        {NextTokenMatches(CodeGenerationSupporter.Log)}? // Conflicts with sendStatement (SEND On ...)
        tLog:Identifier On fileDeclBodyList[vParent, vParent.LogOn]
        {
            Match(tLog,CodeGenerationSupporter.Log);
        }
      )?
    ;
    
onDisk [CreateDatabaseStatement vParent]
{
    FileDeclaration vFileDecl;
    FileGroupDefinition vFilegroup;
    FileGroupDefinition currentFilegroup = FragmentFactory.CreateFragment<FileGroupDefinition>();
    vParent.FileGroups.Add(currentFilegroup);
}
    : On vFileDecl = fileDecl[false]
        {
            currentFilegroup.FileDeclarations.Add(vFileDecl);
            vParent.UpdateTokenInfo(vFileDecl);
        }
        (Comma 
            (vFileDecl = fileDecl[false]
                {
                    currentFilegroup.FileDeclarations.Add(vFileDecl);
                    vParent.UpdateTokenInfo(vFileDecl);
                }
            | vFilegroup = fileGroupDecl
                {
                    currentFilegroup = vFilegroup;
                    AddAndUpdateTokenInfo(vParent, vParent.FileGroups, currentFilegroup);
                }
            )
        )*
    ;
    
fileDeclBodyList [TSqlFragment vParent, IList<FileDeclaration> fileDeclarations]
{
    FileDeclaration vFileDecl;
}
    : vFileDecl = fileDeclBody[false]
        {
            AddAndUpdateTokenInfo(vParent, fileDeclarations, vFileDecl);
        }
        (Comma vFileDecl = fileDeclBody[false]
            {
                AddAndUpdateTokenInfo(vParent, fileDeclarations, vFileDecl);
            }
        )*        
    ;
    
fileDecl [bool isAlterDbModifyFileStatement] returns [FileDeclaration vResult]
    : tPrimary:Primary vResult = fileDeclBody[isAlterDbModifyFileStatement]
        {
            vResult.IsPrimary = true;
            UpdateTokenInfo(vResult,tPrimary);
        }
    | vResult = fileDeclBody[isAlterDbModifyFileStatement]
    ;
    
fileGroupDecl returns [FileGroupDefinition vResult = FragmentFactory.CreateFragment<FileGroupDefinition>()]
{
    FileDeclaration vFileDecl;
    Identifier vIdentifier;
}
    : tFileGroup:Identifier vIdentifier = identifier 
        {
            Match(tFileGroup,CodeGenerationSupporter.Filegroup);
            UpdateTokenInfo(vResult,tFileGroup);
            vResult.Name = vIdentifier;
        }
        (Default
            {
                vResult.IsDefault = true;
            }
        )? 
        vFileDecl = fileDeclBody[false]
        {
            AddAndUpdateTokenInfo(vResult, vResult.FileDeclarations, vFileDecl);
        }
    ;
    
fileDeclBody [bool isAlterDbModifyFileStatement] returns [FileDeclaration vResult = FragmentFactory.CreateFragment<FileDeclaration>()]
{
    FileDeclarationOption vOption;
    long encounteredOptions = 0;
}
    : tLParen:LeftParenthesis vOption = fileOption[isAlterDbModifyFileStatement]
        {
            UpdateTokenInfo(vResult,tLParen);
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
        (Comma vOption = fileOption[isAlterDbModifyFileStatement]
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
            if (!isAlterDbModifyFileStatement && (encounteredOptions & (1 << (int)FileDeclarationOptionKind.FileName)) == 0)
            {
                ThrowParseErrorException("SQL46065", 
                    vResult, TSqlParserResource.SQL46065Message);
            }
        }
    ;
    
fileOption [bool newNameAllowed] returns [FileDeclarationOption vResult = null]
    :    {NextTokenMatches(CodeGenerationSupporter.Name)}?
        vResult = nameFileOption
    |    {NextTokenMatches(CodeGenerationSupporter.FileName)}?
        vResult = fileNameFileOption
    |    {NextTokenMatches(CodeGenerationSupporter.Size)}?
        vResult = sizeFileOption
    |    {NextTokenMatches(CodeGenerationSupporter.MaxSize)}?
        vResult = maxSizeFileOption
    |    {NextTokenMatches(CodeGenerationSupporter.FileGrowth)}?
        vResult = fileGrowthFileOption
    |    {NextTokenMatches(CodeGenerationSupporter.NewName)}?
        vResult = newNameFileOption
        {
            if (!newNameAllowed)
            {
                ThrowParseErrorException("SQL46062", 
                    vResult, TSqlParserResource.SQL46062Message);
            }            
        }
    ;
    
nameFileOption returns [NameFileDeclarationOption vResult = FragmentFactory.CreateFragment<NameFileDeclarationOption>()]
{
    IdentifierOrValueExpression vValue;
}
    : tName:Identifier EqualsSign vValue = nonEmptyStringOrIdentifier
        {
            vResult.OptionKind=FileDeclarationOptionKind.Name;
            Match(tName,CodeGenerationSupporter.Name);
            UpdateTokenInfo(vResult, tName);
            vResult.LogicalFileName = vValue;
            vResult.IsNewName = false;
        }
    ;

newNameFileOption returns [NameFileDeclarationOption vResult = FragmentFactory.CreateFragment<NameFileDeclarationOption>()]
{
    IdentifierOrValueExpression vValue;
}
    : tNewName:Identifier EqualsSign vValue = nonEmptyStringOrIdentifier
        {
            Match(tNewName,CodeGenerationSupporter.NewName);
            vResult.OptionKind=FileDeclarationOptionKind.NewName;
            UpdateTokenInfo(vResult, tNewName);
            vResult.LogicalFileName = vValue;
            vResult.IsNewName = true;
        }
    ;
    
fileNameFileOption returns [FileNameFileDeclarationOption vResult = FragmentFactory.CreateFragment<FileNameFileDeclarationOption>()]
{
    Literal vValue;
}
    : tFileName:Identifier EqualsSign vValue = nonEmptyString
        {
            vResult.OptionKind=FileDeclarationOptionKind.FileName;
            Match(tFileName,CodeGenerationSupporter.FileName);
            UpdateTokenInfo(vResult, tFileName);
            vResult.OSFileName = vValue;
        }
    ;
    
sizeFileOption returns [SizeFileDeclarationOption vResult = FragmentFactory.CreateFragment<SizeFileDeclarationOption>()]
{
    Literal vValue;
    MemoryUnit vUnits;
}
    : tSize:Identifier EqualsSign vValue = integer
        {
            vResult.OptionKind=FileDeclarationOptionKind.Size;
            Match(tSize,CodeGenerationSupporter.Size);
            UpdateTokenInfo(vResult, tSize);
            vResult.Size = vValue;
        }
        (vUnits = memUnit
            {
                vResult.Units = vUnits;
            }
        )?
    ;

maxSizeFileOption returns [MaxSizeFileDeclarationOption vResult = FragmentFactory.CreateFragment<MaxSizeFileDeclarationOption>()]
{
    Literal vValue;
    MemoryUnit vUnits;
}
    : tMaxSize:Identifier EqualsSign 
        {
            Match(tMaxSize,CodeGenerationSupporter.MaxSize);
            vResult.OptionKind=FileDeclarationOptionKind.MaxSize;
            UpdateTokenInfo(vResult, tMaxSize);
        }
        (
            (
                vValue = integer
                {
                    vResult.MaxSize = vValue;
                }
                (vUnits = memUnit
                    {
                        vResult.Units = vUnits;
                    }
                )?
            )
            |
            tUnlimited:Identifier
            {
                Match(tUnlimited,CodeGenerationSupporter.Unlimited);
                vResult.Unlimited = true;
            }
        )
    ;

fileGrowthFileOption returns [FileGrowthFileDeclarationOption vResult = FragmentFactory.CreateFragment<FileGrowthFileDeclarationOption>()]
{
    Literal vValue;
    MemoryUnit vUnits;
}
    : tFileGrowth:Identifier EqualsSign 
        {
            Match(tFileGrowth,CodeGenerationSupporter.FileGrowth);
            vResult.OptionKind=FileDeclarationOptionKind.FileGrowth;
            UpdateTokenInfo(vResult, tFileGrowth);
        }
        (
            vValue = integer
            {
                vResult.GrowthIncrement = vValue;
            }
            (
                (
                    vUnits = memUnit
                    {
                        vResult.Units = vUnits;
                    }
                )
                |
                tPercent:PercentSign
                {
                    vResult.Units = MemoryUnit.Percent;
                    UpdateTokenInfo(vResult,tPercent);
                }
            )?
        )
    ;
        
memUnit returns [MemoryUnit vResult = MemoryUnit.Unspecified]
    : tUnit:Identifier
        {
            vResult = MemoryUnitsHelper.Instance.ParseOption(tUnit);
        }
    ;

dbAddendums [CreateDatabaseStatement vParent]
    : (For 
        (
            tAttach:Identifier 
            {
                Match(tAttach, CodeGenerationSupporter.Attach);
                vParent.AttachMode =AttachMode.Attach;
                UpdateTokenInfo(vParent,tAttach);
            }
        |
            tLoad:Load
            {
                vParent.AttachMode = AttachMode.Load;
                UpdateTokenInfo(vParent,tLoad);
            }
        )
        (Identifier)?
      )
    ;

optionOnOff [TSqlFragment vParent] returns [OptionState vResult = OptionState.NotSet]
    : tOn:On 
        {
            vResult = OptionState.On;
            UpdateTokenInfo(vParent,tOn);
        }
    | tOff:Off
        {
            vResult = OptionState.Off;
            UpdateTokenInfo(vParent,tOff);
        }
    ;
    
//////////////////////////////////////////////////////////////////////
// End Create Database
//////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////
// Backup / Restore (aka Dump / Load)
//////////////////////////////////////////////////////////////////////

backupStatement returns [BackupStatement vResult]
{
    IToken vStartToken;
}
    : vStartToken=backupStart vResult=backupMain 
        {
            UpdateTokenInfo(vResult,vStartToken);
        }
        (
            To devList[vResult, vResult.Devices]
        )? 
        (
            backupOptions[vResult]
        )?
    ;
    
backupStart returns [IToken vToken = null]
    : tBackup:Backup 
        {
            vToken = tBackup;
        }
    | tDump:Dump
        {
            vToken = tDump;
        }
    ;
    
restoreStatement returns [RestoreStatement vResult = FragmentFactory.CreateFragment<RestoreStatement>()]
{
    IToken vStartToken;
}
    :  vStartToken = restoreStart
        {
            UpdateTokenInfo(vResult,vStartToken);
        }
        (
            (
                restoreMain[vResult]
                (
                    From devList[vResult, vResult.Devices]
                )? 
            )
        |    
            (
                tOption:Identifier From devList[vResult, vResult.Devices]
                {
                    vResult.Kind = RestoreStatementKindsHelper.Instance.ParseOption(tOption);
                }
            )
        )
        (
            restoreOptions[vResult]
        )?
    ;
    
backupMain returns [BackupStatement vResult = null]
    : vResult = backupDatabase
    | vResult = backupTransactionLog
    ;
    
backupDatabase returns [BackupDatabaseStatement vResult = FragmentFactory.CreateFragment<BackupDatabaseStatement>()]
{
    IdentifierOrValueExpression vName;
}
    : Database vName = identifierOrVariable 
        {
            vResult.DatabaseName = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        backupFileListOpt[vResult]
    ;
    
backupTransactionLog returns [BackupTransactionLogStatement vResult = FragmentFactory.CreateFragment<BackupTransactionLogStatement>()]
{
    IdentifierOrValueExpression vName;
}
    : (
        Transaction 
      |
        Tran
      | 
        tLog:Identifier
        {
            Match(tLog,CodeGenerationSupporter.Log);
        }
      )
      vName = identifierOrVariable
      {
        vResult.DatabaseName = vName;
        ThrowPartialAstIfPhaseOne(vResult);
      }
    ;
    
backupFileListOpt [BackupDatabaseStatement vParent]
{
    BackupRestoreFileInfo vFile;
}
    : (vFile = backupRestoreFile
        {
            AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
        }
        (Comma vFile = backupRestoreFile
            {
                AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
            }
        )*)?
    ;

restoreFileListOpt [RestoreStatement vParent]
{
    BackupRestoreFileInfo vFile;
}
    : (vFile = backupRestoreFile 
        {
            AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
        }
        (Comma vFile = backupRestoreFile
            {
                AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
            }
        )*)?
    ;
    
backupRestoreFile returns [BackupRestoreFileInfo vResult = FragmentFactory.CreateFragment<BackupRestoreFileInfo>()]
{
    ValueExpression vFileOrFilegroup;
}
    : (tFile:File EqualsSign 
        {
            vResult.ItemKind = BackupRestoreItemKind.Files;
        }
        (
            vFileOrFilegroup = stringOrVariable
            {
                AddAndUpdateTokenInfo(vResult, vResult.Items, vFileOrFilegroup);
            }
        | backupRestoreFileNameList[vResult]
        )
      )
    | (tFilegroupPage:Identifier EqualsSign 
        (
            vFileOrFilegroup = stringOrVariable    
            {
                if (TryMatch(tFilegroupPage,CodeGenerationSupporter.Page))
                    vResult.ItemKind = BackupRestoreItemKind.Page;
                else
                {
                    Match(tFilegroupPage,CodeGenerationSupporter.Filegroup);
                    vResult.ItemKind = BackupRestoreItemKind.FileGroups;
                }
                UpdateTokenInfo(vResult,tFilegroupPage);
                AddAndUpdateTokenInfo(vResult, vResult.Items, vFileOrFilegroup);
            }
        | backupRestoreFileNameList[vResult]
            {
                Match(tFilegroupPage,CodeGenerationSupporter.Filegroup);
                vResult.ItemKind = BackupRestoreItemKind.FileGroups;
            }
        )
      )
    ;
    
backupRestoreFileNameList [BackupRestoreFileInfo vParent]
{
    ValueExpression vItem;
}
    : tLParen:LeftParenthesis vItem = stringOrVariable
        {
            AddAndUpdateTokenInfo(vParent, vParent.Items, vItem);
        }
        (Comma vItem = stringOrVariable
            {
                AddAndUpdateTokenInfo(vParent, vParent.Items, vItem);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
devList [TSqlFragment vParent, IList<DeviceInfo> deviceInfos]
{
    DeviceInfo vDeviceInfo;
}
    : vDeviceInfo = deviceInfo
        {
            AddAndUpdateTokenInfo(vParent, deviceInfos, vDeviceInfo);
        }
        (Comma vDeviceInfo = deviceInfo
            {
                AddAndUpdateTokenInfo(vParent, deviceInfos, vDeviceInfo);
            }
        )*
    ;
    
deviceInfo returns [DeviceInfo vResult = FragmentFactory.CreateFragment<DeviceInfo>()]
{
    IdentifierOrValueExpression vLogicalDevice;
    ValueExpression vPhysicalDevice;
}
    : vLogicalDevice = identifierOrVariable
        {
            vResult.LogicalDevice = vLogicalDevice;
        }
    | 
        (
            tDevType:Identifier 
            {
                vResult.DeviceType = DeviceTypesHelper.Instance.ParseOption(tDevType);                
            }
        |   Disk
            {
                vResult.DeviceType = DeviceType.Disk;
            }
        )
        EqualsSign vPhysicalDevice = stringOrVariable
        {
            vResult.PhysicalDevice = vPhysicalDevice;
        }
    ;
        
backupOptions [BackupStatement vParent]
{
    BackupOption vOption;
}
    : With vOption = backupOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = backupOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
    ;
    
backupOption returns [BackupOption vResult = FragmentFactory.CreateFragment<BackupOption>()]
{
    ScalarExpression vValue;
}
    : tSimpleOption:Identifier
        {
            vResult.OptionKind = BackupOptionsNoValueHelper.Instance.ParseOption(tSimpleOption, SqlVersionFlags.TSql80);
            UpdateTokenInfo(vResult,tSimpleOption);
        }
    | (tOption:Identifier EqualsSign 
        (
            vValue = signedIntegerOrVariable
        |
            vValue = stringLiteral
        )
        {
            vResult.OptionKind = BackupOptionsWithValueHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
            vResult.Value = vValue;
        }
      )
    ;
    
restoreStart returns [IToken vToken = null]
    : tRestore:Restore
        {
            vToken = tRestore;
        }
    | tLoad:Load
        {
            vToken = tLoad;
        }
    ;    
    
restoreMain [RestoreStatement vParent]
{
    IdentifierOrValueExpression vName;
}
    : (Database vName = identifierOrVariable 
            {
                vParent.DatabaseName = vName;
                vParent.Kind = RestoreStatementKind.Database;
                ThrowPartialAstIfPhaseOne(vParent);
            }
            restoreFileListOpt[vParent]
          )
    |    (Transaction | Tran) vName = identifierOrVariable
        {
            vParent.DatabaseName = vName;
            vParent.Kind = RestoreStatementKind.TransactionLog;
            ThrowPartialAstIfPhaseOne(vParent);
        }
    | (tLog:Identifier vName = identifierOrVariable 
        {
            Match(tLog,CodeGenerationSupporter.Log);
            vParent.DatabaseName = vName;
            vParent.Kind = RestoreStatementKind.TransactionLog;
            ThrowPartialAstIfPhaseOne(vParent);
        }
        restoreFileListOpt[vParent]
      )
    ;
        
restoreOptions [RestoreStatement vParent]
    : With restoreOptionsList[vParent]
    ;
    
restoreOptionsList [RestoreStatement vParent]
{
    RestoreOption vOption;
}
    : vOption = restoreOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = restoreOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
    ;
    
restoreOption returns [RestoreOption vResult = null]
{
    ValueExpression vValue, vAfter;
    ScalarExpression vExpression;
}
    : vResult = simpleRestoreOption
    | (tOption:Identifier EqualsSign 
        ( {IsStopAtBeforeMarkRestoreOption(tOption)}?
            (
                vValue = stringOrVariable vAfter = afterClause
                {
                    vResult = CreateStopRestoreOption(tOption,vValue, vAfter);
                }
            )
        | vExpression = signedInteger
            {
                vResult = CreateSimpleRestoreOptionWithValue(tOption,vExpression);
            }
        | vValue = stringOrVariable
            {
                if (IsStopAtBeforeMarkRestoreOption(tOption))
                    vResult = CreateStopRestoreOption(tOption,vValue, null);
                else
                    vResult = CreateSimpleRestoreOptionWithValue(tOption,vValue);
            }
        )
      )
    | vResult = moveRestoreOption
    | vResult = fileRestoreOption
    ;
    
simpleRestoreOption returns [RestoreOption vResult = FragmentFactory.CreateFragment<RestoreOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind = RestoreOptionNoValueHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
            UpdateTokenInfo(vResult,tOption);
        }
    ;

moveRestoreOption returns [MoveRestoreOption vResult = FragmentFactory.CreateFragment<MoveRestoreOption>()]
{
    ValueExpression vLogicalFile, vOSFile;
}
    : tMove:Identifier vLogicalFile = stringOrVariable To vOSFile = stringOrVariable
        {
            Match(tMove,CodeGenerationSupporter.Move);
            vResult.OptionKind=RestoreOptionKind.Move;
            vResult.LogicalFileName = vLogicalFile;
            vResult.OSFileName = vOSFile;            
        }
    ;
    
fileRestoreOption returns [ScalarExpressionRestoreOption vResult = FragmentFactory.CreateFragment<ScalarExpressionRestoreOption>()]
{
    ScalarExpression vValue;
}
    : File EqualsSign vValue = signedIntegerOrVariable
        {
            vResult.OptionKind = RestoreOptionKind.File;
            vResult.Value = vValue;
        }
    ;
    
afterClause returns [ValueExpression vResult = null]
    : tAfter:Identifier vResult = stringOrVariable
        {
            Match(tAfter,CodeGenerationSupporter.After);
        }
    ;    
    
//////////////////////////////////////////////////////////////////////
// End Backup / Restore (aka Dump / Load)
//////////////////////////////////////////////////////////////////////

// Bulk Insert & Insert Bulk    
bulkInsertStatement returns [BulkInsertStatement vResult = FragmentFactory.CreateFragment<BulkInsertStatement>()]
{
    SchemaObjectName vTo;
    IdentifierOrValueExpression vFrom;
}
    : tBulk:Bulk Insert vTo = schemaObjectThreePartName 
        {
            UpdateTokenInfo(vResult,tBulk);
            vResult.To = vTo;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        From vFrom = bulkInsertFrom 
        {
            vResult.From = vFrom;
        }
        (
            bulkInsertOptions[vResult]
        )?
    ;
    
bulkInsertFrom returns [IdentifierOrValueExpression vResult = null]
{
    Literal vLiteral;
}
    : vLiteral = integer
      {
        vResult=IdentifierOrValueExpression(vLiteral);
      }
    | vResult = stringOrIdentifier
    ;

bulkInsertOptions [BulkInsertStatement vParent]
{
    BulkInsertOption vOption;
    long encountered = 0;
}
    : With LeftParenthesis vOption = bulkInsertOption
        {
            CheckOptionDuplication(ref encountered,(int)vOption.OptionKind,vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = bulkInsertOption
            {
                CheckOptionDuplication(ref encountered,(int)vOption.OptionKind,vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
bulkInsertOption returns [BulkInsertOption vResult = null]
    : vResult = bulkInsertSortOrderOption
    | vResult = simpleBulkInsertOptionWithValue
    | vResult = simpleBulkInsertOptionNoValue
    ;
    
insertBulkOption returns [BulkInsertOption vResult = null]
    : vResult = bulkInsertSortOrderOption
    | vResult = simpleInsertBulkOption
    ;
    
simpleInsertBulkOption returns [BulkInsertOption vResult = null]
{
    Literal vValue;
    LiteralBulkInsertOption vLiteralBulkInsertOption;
}
    :   tOption:Identifier
        (
            (EqualsSign vValue = integerOrNumeric
                {
                    vLiteralBulkInsertOption=FragmentFactory.CreateFragment<LiteralBulkInsertOption>();
                    UpdateTokenInfo(vLiteralBulkInsertOption, tOption);
                    if (TryMatch(tOption, CodeGenerationSupporter.RowsPerBatch))
                        vLiteralBulkInsertOption.OptionKind = BulkInsertOptionKind.RowsPerBatch;
                    else
                    {
                        Match(tOption, CodeGenerationSupporter.KilobytesPerBatch);
                        vLiteralBulkInsertOption.OptionKind = BulkInsertOptionKind.KilobytesPerBatch;
                    }
                    vLiteralBulkInsertOption.Value = vValue;
                    vResult=vLiteralBulkInsertOption;
                }
            )
        | /* empty */
            {
                vResult=FragmentFactory.CreateFragment<BulkInsertOption>();
                vResult.OptionKind = BulkInsertFlagOptionsHelper.Instance.ParseOption(tOption);
                UpdateTokenInfo(vResult, tOption);
                if (vResult.OptionKind == BulkInsertOptionKind.KeepIdentity) // For some reason, KeepIdentity for Insert Bulk is not supported...
                    throw GetUnexpectedTokenErrorException(tOption);
            }
        )
    ; 
    
simpleBulkInsertOptionWithValue returns [LiteralBulkInsertOption vResult = FragmentFactory.CreateFragment<LiteralBulkInsertOption>()]
{
    Literal vValue;
}
    : tOption:Identifier EqualsSign 
        ( vValue = integerOrNumeric
            {
                vResult.OptionKind = BulkInsertIntOptionsHelper.Instance.ParseOption(tOption);
                UpdateTokenInfo(vResult, tOption);
                vResult.Value = vValue;
            }
        | vValue = stringLiteral
            {
                vResult.OptionKind = BulkInsertStringOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
                if (vResult.OptionKind == BulkInsertOptionKind.CodePage) // Check for code page string constants
                    MatchString(vValue, CodeGenerationSupporter.ACP, CodeGenerationSupporter.OEM, CodeGenerationSupporter.Raw);
                else if (vResult.OptionKind == BulkInsertOptionKind.DataFileType)
                    MatchString(vValue, CodeGenerationSupporter.Char, CodeGenerationSupporter.Native, 
                            CodeGenerationSupporter.WideChar, CodeGenerationSupporter.WideNative,
                            CodeGenerationSupporter.WideCharAnsi, CodeGenerationSupporter.DTSBuffers);
                UpdateTokenInfo(vResult, tOption);
                vResult.Value = vValue;
            }
        )
    ;
    
simpleBulkInsertOptionNoValue returns [BulkInsertOption vResult = FragmentFactory.CreateFragment<BulkInsertOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind = BulkInsertFlagOptionsHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    ;
        
insertBulkStatement returns [InsertBulkStatement vResult = FragmentFactory.CreateFragment<InsertBulkStatement>()]
{
    SchemaObjectName vTo;
}
    : tInsert:Insert Bulk vTo = schemaObjectThreePartName 
        {
            vResult.To = vTo;
            UpdateTokenInfo(vResult,tInsert);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        // Conflicts with Select, which can also start from '(', so making it greedy...
        (options { greedy = true; } : coldefList[vResult])? 
        (
            insertBulkOptions[vResult]
        )?
    ;
    
coldefList [InsertBulkStatement vParent]
{
    InsertBulkColumnDefinition vColumnDefinition;
}
    : LeftParenthesis vColumnDefinition = coldefItem 
        {
            AddAndUpdateTokenInfo(vParent, vParent.ColumnDefinitions, vColumnDefinition);
        }
        (Comma vColumnDefinition = coldefItem
            {
                AddAndUpdateTokenInfo(vParent, vParent.ColumnDefinitions, vColumnDefinition);
            }        
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
coldefItem returns [InsertBulkColumnDefinition vResult = FragmentFactory.CreateFragment<InsertBulkColumnDefinition>()]
{
    ColumnDefinitionBase vColumn;
    bool vIsNull;
}
    : vColumn = columnDefinitionEx
        {
            vResult.Column = vColumn;
        } 
        (vIsNull = nullNotNull[vResult]
            {
                vResult.NullNotNull = (vIsNull ? NullNotNull.Null : NullNotNull.NotNull);
            }
        )?
    ;
    
insertBulkOptions [InsertBulkStatement vParent]
{
    long encountered = 0;
    BulkInsertOption vOption;
}
    : With LeftParenthesis vOption = insertBulkOption
        {
            CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = insertBulkOption
            {
                CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
        
bulkInsertSortOrderOption returns [OrderBulkInsertOption vResult = FragmentFactory.CreateFragment<OrderBulkInsertOption>()]
{
    ColumnWithSortOrder vColumn;
}
    : tOrder:Order LeftParenthesis vColumn = columnWithSortOrder
        {
            vResult.OptionKind=BulkInsertOptionKind.Order;
            UpdateTokenInfo(vResult,tOrder);
            AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
        }
        (Comma vColumn = columnWithSortOrder
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;
    
// End Of Bulk Insert & Insert Bulk

// Dbcc statements
dbccStatement returns [DbccStatement vResult = FragmentFactory.CreateFragment<DbccStatement>()]
    : tDbcc:Dbcc tIdentifier:Identifier 
        {
            DbccCommand command;
            if (DbccCommandsHelper.Instance.TryParseOption(tIdentifier,out command))
                vResult.Command = command;
            else
            {
                vResult.Command = DbccCommand.Free;
                vResult.DllName = tIdentifier.getText();
            }
            
            UpdateTokenInfo(vResult,tDbcc);
            UpdateTokenInfo(vResult,tIdentifier);
        }
        (options { greedy = true; } : dbccNamedLiteralList[vResult])?  // Select can start from '(', so, making literal list greedy...
        (
            dbccOptions[vResult]
        )?
    ;
    
dbccOptions [DbccStatement vParent]
    : With dbccOptionsList[vParent]
    ;
    
dbccOptionsList [DbccStatement vParent]
    : dbccOptionsListItems[vParent] 
    | dbccOptionsJoin[vParent]
    ;
    
dbccOptionsListItems [DbccStatement vParent]
{
    DbccOption vOption;
}
    : vOption=dbccOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption=dbccOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
    ;

dbccOption returns [DbccOption vResult=FragmentFactory.CreateFragment<DbccOption>()]
    :     tOption:Identifier 
        {
            vResult.OptionKind= DbccOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
            UpdateTokenInfo(vResult, tOption);
        }
    ;
    
dbccOptionsJoin [DbccStatement vParent]
{
    DbccOption vOption;
}
    : vOption=dbccJoinOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Join vOption=dbccJoinOption
            {
                vParent.OptionsUseJoin=true;
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )+
    ;

dbccJoinOption returns [DbccOption vResult=FragmentFactory.CreateFragment<DbccOption>()]
    :     tOption:Identifier 
        {
            vResult.OptionKind= DbccJoinOptionsHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult, tOption);
        }
    ;
    
dbccNamedLiteralList [DbccStatement vParent]
{
    DbccNamedLiteral vLiteral;
}
    : tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vParent,tLParen);
        }
        (
            (vLiteral = dbccNamedLiteral
                {
                    AddAndUpdateTokenInfo(vParent, vParent.Literals, vLiteral);
                }
                (Comma vLiteral = dbccNamedLiteral
                    {
                        AddAndUpdateTokenInfo(vParent, vParent.Literals, vLiteral);
                    }
                )*
            )
        | /* empty */
            {
                vParent.ParenthesisRequired = true;
            }
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
dbccNamedLiteral returns [DbccNamedLiteral vResult = FragmentFactory.CreateFragment<DbccNamedLiteral>()]
{
    ScalarExpression vValue;
}
    : (tIdentifier:Identifier EqualsSign
        {
            vResult.Name = tIdentifier.getText();
            UpdateTokenInfo(vResult,tIdentifier);
        }
      )? 
        vValue = possibleNegativeConstantOrIdentifier
        {
            vResult.Value = vValue;
        }
    ;    
    
// End of Dbcc statements

createSchemaStatement returns [CreateSchemaStatement vResult = this.FragmentFactory.CreateFragment<CreateSchemaStatement>()]
{
    Identifier vIdentifier;
    StatementList vStatementList;
}
    : 
        tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        Schema
        (
            vIdentifier=identifier
            {
                vResult.Name = vIdentifier;
            }
            authorizationOpt[vResult]
        |
            authorization[vResult]
        )        
        {            
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vStatementList=createSchemaElementList
        {
            vResult.StatementList = vStatementList;
        }
    ;
    
createSchemaElementList returns [StatementList vResult = FragmentFactory.CreateFragment<StatementList>()]
{
    TSqlStatement vStatement;
}
    : 
    (vStatement=createSchemaElement
        {
            Debug.Assert(vStatement != null);
            AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
        }
    )*
    ;

createSchemaElement returns [TSqlStatement vResult = null]
    : 
        (
            vResult=createViewStatement
        | 
            vResult=createTableStatement
        | 
            vResult=grantStatement80
        )
    ;

createFunctionStatement returns [CreateFunctionStatement vResult = this.FragmentFactory.CreateFragment<CreateFunctionStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : 
        tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        functionStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

alterFunctionStatement returns [AlterFunctionStatement vResult = this.FragmentFactory.CreateFragment<AlterFunctionStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : 
        tAlter:Alter 
        {
            UpdateTokenInfo(vResult,tAlter);
        }
        functionStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

functionStatementBody[FunctionStatementBody vResult, out bool vParseErrorOccurred]
{
    SchemaObjectName vSchemaObjectName;
}
    : Function vSchemaObjectName=schemaObjectThreePartName 
        {
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Trigger);
            vResult.Name = vSchemaObjectName;
            CheckForTemporaryFunction(vSchemaObjectName);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        LeftParenthesis (functionParameterList[vResult])? RightParenthesis tReturns:Identifier 
            {
                Match(tReturns, CodeGenerationSupporter.Returns);
            }
        functionReturnTypeAndBody[vResult, out vParseErrorOccurred]
    ;

functionParameterList[FunctionStatementBody vResult]
{
    ProcedureParameter vParameter;
}
    :
        vParameter=functionParameter
        {
            AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
        }
        ( Comma vParameter=functionParameter
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
            }
        )*
    ;  

functionParameter returns[ProcedureParameter vResult = FragmentFactory.CreateFragment<ProcedureParameter>()]
{
    Identifier vIdentifier;
}
    : vIdentifier=identifierVariable (As)?
        {
            vResult.VariableName = vIdentifier;
        }
        scalarProcedureParameter[vResult, false]
    ;    
    
functionReturnTypeAndBody [FunctionStatementBody vParent, out bool vParseErrorOccurred]
{
    DataTypeReference vDataType;
    DeclareTableVariableBody vDeclareTableBody;
    SelectFunctionReturnType vSelectReturn;
    BeginEndBlockStatement vCompoundStatement;
    vParseErrorOccurred = false;
}
    :
        // Scalar functions
        vDataType = scalarDataType
        {
            ScalarFunctionReturnType vScalarResult = FragmentFactory.CreateFragment<ScalarFunctionReturnType>();
            vScalarResult.DataType = vDataType;
            vParent.ReturnType = vScalarResult;
        }
        (functionAttributes[vParent])? (As)? 
        (
            vCompoundStatement = beginEndBlockStatement
            {
                SetFunctionBodyStatement(vParent, vCompoundStatement);
                vParseErrorOccurred = vCompoundStatement == null;
            }
        )
    | 
        // Inline table-valued functions
        Table (functionAttributes[vParent])? (As)? Return vSelectReturn = functionReturnClauseRelational
        {
            vParent.ReturnType = vSelectReturn;
        }
    | 
        // Multistatement Table-valued Functions         
        vDeclareTableBody = declareTableBody[IndexAffectingStatement.CreateOrAlterFunction]
        {
            TableValuedFunctionReturnType vTableResult = FragmentFactory.CreateFragment<TableValuedFunctionReturnType>();
            vTableResult.DeclareTableVariableBody = vDeclareTableBody;
            vParent.ReturnType = vTableResult;
        }
        (functionAttributes[vParent])? (As)? vCompoundStatement = beginEndBlockStatement
        {
            SetFunctionBodyStatement(vParent, vCompoundStatement);
            vParseErrorOccurred = vCompoundStatement == null;
        }
    ;
    
functionReturnClauseRelational returns [SelectFunctionReturnType vResult = FragmentFactory.CreateFragment<SelectFunctionReturnType>()]
{
    SelectStatement vSelectStatement;
}
    : vSelectStatement = subqueryExpressionAsStatement
        {
            vResult.SelectStatement = vSelectStatement;
        }
    ;
         
functionAttributes [FunctionStatementBody vParent]
{
    FunctionOption vOption;
    long encounteredOptions=0;
}
    : With vOption=functionAttribute
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);        
        }
        (
            Comma vOption=functionAttribute
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);        
            }
        )*
    ;

functionAttribute returns [FunctionOption vResult = FragmentFactory.CreateFragment<FunctionOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind=ParseAlterCreateFunctionWithOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    | tReturns:Identifier Null On Null tInput:Identifier
        {
            Match(tReturns,CodeGenerationSupporter.Returns);
            Match(tInput,CodeGenerationSupporter.Input);
            vResult.OptionKind = FunctionOptionKind.ReturnsNullOnNullInput;
            UpdateTokenInfo(vResult,tInput);
        }
    | tCalled:Identifier On Null tInput2:Identifier
        {
            Match(tCalled,CodeGenerationSupporter.Called);
            Match(tInput2,CodeGenerationSupporter.Input);
            vResult.OptionKind = FunctionOptionKind.CalledOnNullInput;
            UpdateTokenInfo(vResult,tInput2);
        }
    ;
    
createStatisticsStatement returns [CreateStatisticsStatement vResult = this.FragmentFactory.CreateFragment<CreateStatisticsStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    StatisticsOption vStatisticsOption;
    bool isConflictingOption = false;
}
    :
        tCreate:Create tStatistics:Statistics vIdentifier=identifier
        {
            UpdateTokenInfo(vResult,tCreate);
            UpdateTokenInfo(vResult,tStatistics);
            vResult.Name = vIdentifier;            
        }
        On vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.OnName = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        identifierColumnList[vResult, vResult.Columns]
        (
            With vStatisticsOption=createStatisticsStatementWithOption[ref isConflictingOption]
            {
                AddAndUpdateTokenInfo(vResult, vResult.StatisticsOptions, vStatisticsOption);
            }
            (
                Comma vStatisticsOption=createStatisticsStatementWithOption[ref isConflictingOption]
                {
                    AddAndUpdateTokenInfo(vResult, vResult.StatisticsOptions, vStatisticsOption);
                }
            )*
        )?      
    ;

sampleStatisticsOption[ref bool isConflictingOption] returns [LiteralStatisticsOption vResult = this.FragmentFactory.CreateFragment<LiteralStatisticsOption>()]
{
    Literal vLiteral;
}
    :
        tSample:Identifier vLiteral=integer 
        {
            Match(tSample, CodeGenerationSupporter.Sample);
            if (isConflictingOption == true)
                ThrowParseErrorException("SQL46071", tSample, TSqlParserResource.SQL46071Message);
            else
                isConflictingOption = true;
            vResult.Literal = vLiteral;
        }
        (
            tRows:Identifier
            {
                UpdateTokenInfo(vResult,tRows);
                vResult.OptionKind = ParseSampleOptionsWithOption(tRows);
            }
        | tPercent:Percent
            {
                UpdateTokenInfo(vResult,tPercent);
                vResult.OptionKind = StatisticsOptionKind.SamplePercent;
            }
        )
    ;

simpleStatisticsOption[ref bool isConflictingOption] returns [StatisticsOption vResult = FragmentFactory.CreateFragment<StatisticsOption>()]
    :
        tOption:Identifier 
        {
            UpdateTokenInfo(vResult,tOption);
            if (TryMatch(tOption, CodeGenerationSupporter.Rows)) 
            {
                vResult.OptionKind = StatisticsOptionKind.Rows;            
                 if (isConflictingOption == true)
                    ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                else
                    isConflictingOption = true;
            }
            else
            {
                if (TryMatch(tOption, CodeGenerationSupporter.FullScan)) 
                {
                     if (isConflictingOption == true)
                        ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                    else
                        isConflictingOption = true;
                }
                vResult.OptionKind = ParseCreateStatisticsWithOption(tOption);            
            }
        }
    ;

createStatisticsStatementWithOption[ref bool isConflictingOption] returns [StatisticsOption vResult]
    :   
        vResult = sampleStatisticsOption[ref isConflictingOption]
    |
        vResult = simpleStatisticsOption[ref isConflictingOption]
    ;

updateStatisticsStatement returns [UpdateStatisticsStatement vResult = this.FragmentFactory.CreateFragment<UpdateStatisticsStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    StatisticsOption vStatisticsOption;
    bool isConflictingOption = false;     
}
    :
        tUpdate:Update tStatistics:Statistics
        {
            UpdateTokenInfo(vResult,tUpdate);
            UpdateTokenInfo(vResult,tStatistics);            
        }
        vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.SchemaObjectName = vSchemaObjectName;
        }
        (
            options {greedy=true;} : 
            (LeftParenthesis identifier)=> // necessary because select statement can start with LeftParenthesis
            columnNameList[vResult, vResult.SubElements]
        |
            vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.SubElements, vIdentifier);
            }
        )?
        (
            With vStatisticsOption=updateStatisticsStatementWithOption[ref isConflictingOption]
            {
                AddAndUpdateTokenInfo(vResult, vResult.StatisticsOptions, vStatisticsOption);
            }
            (
                Comma vStatisticsOption=updateStatisticsStatementWithOption[ref isConflictingOption]
                {
                    AddAndUpdateTokenInfo(vResult, vResult.StatisticsOptions, vStatisticsOption);
                }
            )*
        )?      
    ;

updateStatisticsStatementWithOption [ref bool isConflictingOption] returns [StatisticsOption vResult ]
    :   
        vResult = sampleStatisticsOption[ref isConflictingOption]
    |
        vResult = updateStatisticsLiteralOption
    | 
        vResult = updateStatisticsSimpleOption[ref isConflictingOption]
    ;

updateStatisticsLiteralOption returns [LiteralStatisticsOption vResult = FragmentFactory.CreateFragment<LiteralStatisticsOption>()]
{
    Literal vLiteral;
}
    :
        tPageCount:Identifier EqualsSign vLiteral=integerOrRealOrNumeric
        {
            Match(tPageCount, CodeGenerationSupporter.PageCount);
            vResult.OptionKind = StatisticsOptionKind.PageCount;
            UpdateTokenInfo(vResult,tPageCount);
            vResult.Literal = vLiteral;
        }
    |
        tRowCount:RowCount EqualsSign vLiteral=integerOrRealOrNumeric
        {
            UpdateTokenInfo(vResult,tRowCount);
            vResult.OptionKind = StatisticsOptionKind.RowCount;
            vResult.Literal = vLiteral;
        }
    ;

updateStatisticsSimpleOption [ref bool isConflictingOption] returns [StatisticsOption vResult = FragmentFactory.CreateFragment<StatisticsOption>()]
    :
        tOption:Identifier 
        {
           UpdateTokenInfo(vResult,tOption);
            if (TryMatch(tOption, CodeGenerationSupporter.Rows)) 
            {
                vResult.OptionKind = StatisticsOptionKind.Rows;            
                 if (isConflictingOption == true)
                    ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                else
                    isConflictingOption = true;
            }
            else
            {
                if (TryMatch(tOption, CodeGenerationSupporter.FullScan)) 
                {
                     if (isConflictingOption == true)
                        ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                    else
                        isConflictingOption = true;
                }
                vResult.OptionKind = StatisticsOptionHelper.Instance.ParseOption(tOption);
            }
        }
    | 
        tAll:All
        {
            UpdateTokenInfo(vResult,tAll);
            vResult.OptionKind = StatisticsOptionKind.All;
        }
    | 
        tIndex:Index
        {
            UpdateTokenInfo(vResult,tIndex);
            vResult.OptionKind = StatisticsOptionKind.Index;
        }
    ;    

ifStatement returns [IfStatement vResult = this.FragmentFactory.CreateFragment<IfStatement>()]
{
    BooleanExpression vExpression;
    TSqlStatement vStatement;

    bool vParseErrorHappened = false;
}

    :
        tIf:If 
        {
            UpdateTokenInfo(vResult,tIf);
        }
        vExpression=booleanExpression 
        {
            vResult.Predicate = vExpression;
        }
        ( vStatement=statementOptSemi )
        {
            if (null == vStatement) // if a parse error happens
                vParseErrorHappened = true;
            else
                vResult.ThenStatement = vStatement;
        }
        ( 
            // The closest if claims the else...
            options {greedy = true; } :
            Else vStatement=statementOptSemi
            {
                if (null == vStatement) // if a parse error happens
                    vParseErrorHappened = true;
                else
                    vResult.ElseStatement = vStatement;
            }
        )?
        {
            // Won't return the fragment with the error in it.
            // Because code generation will fail for this case.
            if (vParseErrorHappened)
                vResult = null;
        }
    ;

whileStatement returns [WhileStatement vResult = this.FragmentFactory.CreateFragment<WhileStatement>()]
{
    BooleanExpression vExpression;
    TSqlStatement vStatement;
}
    : 
        tWhile:While
        {
            UpdateTokenInfo(vResult,tWhile);
        }
        vExpression=booleanExpression 
        {
            vResult.Predicate = vExpression;
        }
        ( vStatement=statementOptSemi ) 
        {
            if (null == vStatement) // if a parse error happens
                {
                    // Won't return the fragment with the error in it.
                    // Because code generation will fail for this case.
                    vResult = null;
                }
            else
                {
                    vResult.Statement = vStatement;
                }
        }
    ;

lineNoStatement returns [LineNoStatement vResult = this.FragmentFactory.CreateFragment<LineNoStatement>()]
{
    IntegerLiteral vLiteral;
}
    :
        tLineNo:LineNo vLiteral=integer
        {
            UpdateTokenInfo(vResult,tLineNo);
            vResult.LineNo = vLiteral;
        }
    ;

useStatement returns [UseStatement vResult = this.FragmentFactory.CreateFragment<UseStatement>()]
{
    Identifier vIdentifier;
}
    :
        tUse:Use vIdentifier=identifier
        {
            UpdateTokenInfo(vResult,tUse);
            vResult.DatabaseName = vIdentifier;
        }
    ;

killStatement returns [KillStatement vResult = this.FragmentFactory.CreateFragment<KillStatement>()]
{
    ScalarExpression vExpression;
}
    :    tKill:Kill 
        {
            UpdateTokenInfo(vResult,tKill);
        }
        (
            vExpression=signedInteger
        |
            vExpression=stringLiteral
        )
        {
            vResult.Parameter = vExpression;
        }
        (
            With tStatusOnly:Identifier
            {
                Match(tStatusOnly, CodeGenerationSupporter.StatusOnly);
                vResult.WithStatusOnly = true;
                UpdateTokenInfo(vResult,tStatusOnly);
            }
        )?
    ;

checkpointStatement returns [CheckpointStatement vResult = this.FragmentFactory.CreateFragment<CheckpointStatement>()]
    :
        tCheckpoint:Checkpoint
        {
            UpdateTokenInfo(vResult,tCheckpoint);
        }
    ;

reconfigureStatement returns [ReconfigureStatement vResult = this.FragmentFactory.CreateFragment<ReconfigureStatement>()]
    :
        tReconfigure:Reconfigure
        {
            UpdateTokenInfo(vResult,tReconfigure);
        }
        (
            With tOverride:Identifier
            {
                Match(tOverride, CodeGenerationSupporter.Override);
                vResult.WithOverride = true;
                UpdateTokenInfo(vResult,tOverride);
            }
        )?
    ;

shutdownStatement returns [ShutdownStatement vResult = this.FragmentFactory.CreateFragment<ShutdownStatement >()]
    :
        tShutdown:Shutdown
        {
            UpdateTokenInfo(vResult,tShutdown);
        }
        (
            With tNoWait:Identifier
            {
                Match(tNoWait, CodeGenerationSupporter.NoWait);
                vResult.WithNoWait = true;
                UpdateTokenInfo(vResult,tNoWait);
            }
        )?
    ;

setUserStatement returns [SetUserStatement vResult = this.FragmentFactory.CreateFragment<SetUserStatement>()]
{
    ValueExpression vLiteral;
}
    :
        tSetUser:SetUser
        {
            UpdateTokenInfo(vResult,tSetUser);
        }
        (
            vLiteral=stringOrVariable
            {
                vResult.UserName = vLiteral;
            }
            (
                With tNoReset:Identifier
                {
                    Match(tNoReset, CodeGenerationSupporter.NoReset);
                    vResult.WithNoReset = true;
                    UpdateTokenInfo(vResult,tNoReset);
                }
            )?
        )?
    ;

truncateTableStatement returns [TruncateTableStatement vResult = this.FragmentFactory.CreateFragment<TruncateTableStatement>()]
{
    SchemaObjectName vSchemaObjectName;
}
    :
        tTruncate:Truncate Table vSchemaObjectName=schemaObjectThreePartName
        {
            UpdateTokenInfo(vResult,tTruncate);
            vResult.TableName = vSchemaObjectName;
        }
    ;

grantStatement80 returns [GrantStatement80 vResult = this.FragmentFactory.CreateFragment<GrantStatement80>()]
{
    SecurityElement80 vSecurityElement80;
    SecurityUserClause80 vSecurityUserClause80;
    Identifier vIdentifier;
}
    : tGrant:Grant 
        {
            UpdateTokenInfo(vResult,tGrant);
        }
        vSecurityElement80=securityElement80 
        {
            vResult.SecurityElement80 = vSecurityElement80;
        }
        To vSecurityUserClause80=securityUserClause80
        {
            vResult.SecurityUserClause80 = vSecurityUserClause80;
        }
        (
            With Grant tOption:Option
            {
                vResult.WithGrantOption = true;
                UpdateTokenInfo(vResult,tOption);
            }
        )?
        (
            As vIdentifier=identifier
            {
                vResult.AsClause = vIdentifier;
            }
        )?
    ;

denyStatement80 returns [DenyStatement80 vResult = this.FragmentFactory.CreateFragment<DenyStatement80>()]
{
    SecurityElement80 vSecurityElement80;
    SecurityUserClause80 vSecurityUserClause80;
}
    : tDeny:Deny 
        {
            UpdateTokenInfo(vResult,tDeny);
        }
        vSecurityElement80=securityElement80 
        {
            vResult.SecurityElement80 = vSecurityElement80;
        }
        To vSecurityUserClause80=securityUserClause80
        {
            vResult.SecurityUserClause80 = vSecurityUserClause80;
        }
        (
            tCascade:Cascade
            {
                vResult.CascadeOption = true;
                UpdateTokenInfo(vResult,tCascade);
            }
        )?
    ;

revokeStatement80 returns [RevokeStatement80 vResult = this.FragmentFactory.CreateFragment<RevokeStatement80>()]
{
    SecurityElement80 vSecurityElement80;
    SecurityUserClause80 vSecurityUserClause80;
    Identifier vIdentifier;
}
    : tRevoke:Revoke 
        (
            (Grant Option For)=>
            Grant Option For
            {
                vResult.GrantOptionFor = true;
            }
        |   /* empty */
        )
        {
            UpdateTokenInfo(vResult,tRevoke);
        }
        vSecurityElement80=securityElement80 
        {
            vResult.SecurityElement80 = vSecurityElement80;
        }
        ( To | From ) vSecurityUserClause80=securityUserClause80
        {
            vResult.SecurityUserClause80 = vSecurityUserClause80;
        }
        (
            tCascade:Cascade
            {
                vResult.CascadeOption = true;
                UpdateTokenInfo(vResult,tCascade);
            }
        )?
        (
            As vIdentifier=identifier
            {
                vResult.AsClause = vIdentifier;
            }
        )?
    ;

// Same as grant_revoke_deny body in Sql yacc.
securityElement80 returns [SecurityElement80 vResult = null]
    :
        vResult=commandSecurityElementAll80
    |
        // The All variation is refactored out of this to avoid linear approximation problem in Antlr.
        vResult=commandSecurityElement80
    |
        vResult=privilegeSecurityElement80
    ;

commandSecurityElementAll80 returns [CommandSecurityElement80 vResult = this.FragmentFactory.CreateFragment<CommandSecurityElement80>()]
    :
        tAll:All
        {
            vResult.All = true;
            UpdateTokenInfo(vResult,tAll);
        }
    ;

commandSecurityElement80 returns [CommandSecurityElement80 vResult = this.FragmentFactory.CreateFragment<CommandSecurityElement80>()]
    :
        command80[vResult]
        (
            Comma command80[vResult]
        )*
    ;

command80[CommandSecurityElement80 vParent]
    :
        tCreate:Create 
        {
            UpdateTokenInfo(vParent,tCreate);
        }
        (
            tDatabase1:Database
            {
                UpdateTokenInfo(vParent,tDatabase1);
                vParent.CommandOptions |= CommandOptions.CreateDatabase;
            }
        |
            tDefault:Default
            {
                UpdateTokenInfo(vParent,tDefault);
                vParent.CommandOptions |= CommandOptions.CreateDefault;
            }
        |
            tFunction:Function
            {
                UpdateTokenInfo(vParent,tFunction);
                vParent.CommandOptions |= CommandOptions.CreateFunction;
            }
        |
            tProcedure:Procedure
            {
                UpdateTokenInfo(vParent,tProcedure);
                vParent.CommandOptions |= CommandOptions.CreateProcedure;
            }
        |
            tProc:Proc
            {
                UpdateTokenInfo(vParent,tProc);
                vParent.CommandOptions |= CommandOptions.CreateProcedure;
            }
        |
            tRule:Rule
            {
                UpdateTokenInfo(vParent,tRule);
                vParent.CommandOptions |= CommandOptions.CreateRule;
            }
        |
            tTable:Table
            {
                UpdateTokenInfo(vParent,tTable);
                vParent.CommandOptions |= CommandOptions.CreateTable;
            }
        |
            tView:View
            {
                UpdateTokenInfo(vParent,tView);
                vParent.CommandOptions |= CommandOptions.CreateView;
            }
        )
    |
        tBackup:Backup
        {
            UpdateTokenInfo(vParent,tBackup);
        }
        (
            tDatabase2:Database
            {
                UpdateTokenInfo(vParent,tDatabase2);
                vParent.CommandOptions |= CommandOptions.BackupDatabase;
            }
        |
            tLog:Identifier
            {
                UpdateTokenInfo(vParent,tLog);
                Match(tLog,CodeGenerationSupporter.Log);
                vParent.CommandOptions |= CommandOptions.BackupLog;
            }
        )
    ;

privilegeSecurityElement80 returns [PrivilegeSecurityElement80 vResult = this.FragmentFactory.CreateFragment<PrivilegeSecurityElement80>()]
{
    Privilege80 vPrivilege80;
    SchemaObjectName vSchemaObjectName;
}
    :   vPrivilege80=privilege80
        {
            AddAndUpdateTokenInfo(vResult, vResult.Privileges, vPrivilege80);
        }
        (
            Comma vPrivilege80=privilege80
            {
                AddAndUpdateTokenInfo(vResult, vResult.Privileges, vPrivilege80);
            }
        )*
        On vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.SchemaObjectName = vSchemaObjectName;
        }
        (columnNameList[vResult, vResult.Columns])?
    ;

privilege80 returns [Privilege80 vResult = this.FragmentFactory.CreateFragment<Privilege80>()]
    :
        (
            tAll:All
            {
                UpdateTokenInfo(vResult,tAll);
                vResult.PrivilegeType80 = PrivilegeType80.All;
            }
            (
                tPrivileges:Identifier
                {
                    Match(tPrivileges, CodeGenerationSupporter.Privileges);
                    UpdateTokenInfo(vResult,tPrivileges);
                }
            )?
        |
            tSelect:Select
            {
                UpdateTokenInfo(vResult,tSelect);
                vResult.PrivilegeType80 = PrivilegeType80.Select;
            }
        |
            tInsert:Insert
            {
                UpdateTokenInfo(vResult,tInsert);
                vResult.PrivilegeType80 = PrivilegeType80.Insert;
            }
        |
            tDelete:Delete
            {
                UpdateTokenInfo(vResult,tDelete);
                vResult.PrivilegeType80 = PrivilegeType80.Delete;
            }
        |
            tUpdate:Update
            {
                UpdateTokenInfo(vResult,tUpdate);
                vResult.PrivilegeType80 = PrivilegeType80.Update;
            }
        |
            tExecute:Execute
            {
                UpdateTokenInfo(vResult,tExecute);
                vResult.PrivilegeType80 = PrivilegeType80.Execute;
            }
        |
            tExec:Exec
            {
                UpdateTokenInfo(vResult,tExec);
                vResult.PrivilegeType80 = PrivilegeType80.Execute;
            }
        |
            tReferences:References
            {
                UpdateTokenInfo(vResult,tReferences);
                vResult.PrivilegeType80 = PrivilegeType80.References;
            }
        )
        (columnNameList[vResult, vResult.Columns])?
    ;

// users rule in the yacc grammar
securityUserClause80 returns [SecurityUserClause80 vResult = this.FragmentFactory.CreateFragment<SecurityUserClause80>()]
{
    Identifier vIdentifier;
}
    :
        tPublic:Public
        {
            UpdateTokenInfo(vResult,tPublic);
            vResult.UserType80 = UserType80.Public;            
        }
    |
        tNull:Null
        {
            UpdateTokenInfo(vResult,tNull);
            vResult.UserType80 = UserType80.Null;            
        }
    |
        vIdentifier=identifier
        {
            vResult.UserType80 = UserType80.Users;
            AddAndUpdateTokenInfo(vResult, vResult.Users, vIdentifier);
        }
        (
            Comma vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Users, vIdentifier);
            }
        )*
    ;

printStatement returns [PrintStatement vResult = this.FragmentFactory.CreateFragment<PrintStatement>()]
{
    ScalarExpression vExpression;
}
    :
        tPrint:Print vExpression=expression
        {
            UpdateTokenInfo(vResult,tPrint);
            vResult.Expression = vExpression;
        }
    ;

waitForStatement returns [WaitForStatement vResult = FragmentFactory.CreateFragment<WaitForStatement>()]
{
    ValueExpression vLiteral;
}
    : tWaitFor:WaitFor
        {
            UpdateTokenInfo(vResult,tWaitFor);
        }
        (
            tId:Identifier vLiteral=stringOrVariable
            {
                vResult.WaitForOption = WaitForOptionHelper.Instance.ParseOption(tId);
                vResult.Parameter = vLiteral;
            }
        )
    ;

readTextStatement returns [ReadTextStatement vResult = this.FragmentFactory.CreateFragment<ReadTextStatement>()]
{
    ColumnReferenceExpression vColumn;
    ValueExpression vLiteral;
}
    :
        tReadText:ReadText
        {
            UpdateTokenInfo(vResult,tReadText);
        }
        vColumn=column
        {
            CheckTableNameExistsForColumn(vColumn, true);            
            vResult.Column = vColumn;
        }
        vLiteral=binaryOrVariable
        {
            vResult.TextPointer = vLiteral;
        }
        vLiteral=integerOrVariable
        {
            vResult.Offset = vLiteral;
        }
        vLiteral=integerOrVariable
        {
            vResult.Size = vLiteral;
        }
        (
            tHoldLock:HoldLock
            {
                UpdateTokenInfo(vResult,tHoldLock);
                vResult.HoldLock = true;
            }
        )?
    ;

updateTextStatement returns [UpdateTextStatement vResult = this.FragmentFactory.CreateFragment<UpdateTextStatement>()]
{
    ColumnReferenceExpression vColumn;
    ValueExpression vLiteral;
    ScalarExpression vExpression;
}
    :
        tUpdateText:UpdateText
        {
            UpdateTokenInfo(vResult,tUpdateText);
        }
        modificationTextStatement[vResult]
        vExpression=signedIntegerOrVariableOrNull
        {
            vResult.InsertOffset = vExpression;
        }
        vExpression=signedIntegerOrVariableOrNull
        {
            vResult.DeleteLength = vExpression;
        }
        (
            modificationTextStatementWithLog[vResult]
        )?
        (
            vColumn=column
            {
                CheckTableNameExistsForColumn(vColumn, true);            
                vResult.SourceColumn = vColumn;
            }
            vLiteral=binaryOrVariable
            {
                vResult.SourceParameter = vLiteral;
            }
        |
            vLiteral=writeString
            {
                vResult.SourceParameter = vLiteral;
            }
        )?
    ;

writeTextStatement returns [WriteTextStatement vResult = this.FragmentFactory.CreateFragment<WriteTextStatement>()]
{
    ValueExpression vLiteral;
}
    :
        tWriteText:WriteText
        {
            UpdateTokenInfo(vResult,tWriteText);
        }
        modificationTextStatement[vResult]
        (
            modificationTextStatementWithLog[vResult]
        )?
        vLiteral=writeString
        {
            vResult.SourceParameter = vLiteral;
        }
    ;

modificationTextStatementWithLog[TextModificationStatement vParent]
    :
        With tLog:Identifier
        {
            Match(tLog, CodeGenerationSupporter.Log);
            UpdateTokenInfo(vParent,tLog);
            vParent.WithLog = true;
        }
    ;

modificationTextStatement[TextModificationStatement vParent]
{
    ColumnReferenceExpression vColumn;
    ValueExpression vValueExpression;
    Literal vLiteral;
}
    :
        (
            Bulk
            {
                vParent.Bulk = true;
            }
        )?
        vColumn=column
        {
            CheckTableNameExistsForColumn(vColumn, true);            
            vParent.Column = vColumn;
        }
        (
            vValueExpression=binaryOrVariable
        | 
            vValueExpression=integer
        )
        {
            vParent.TextId = vValueExpression;
        }
        (
            tTimeStamp:Identifier
            {
                Match(tTimeStamp, CodeGenerationSupporter.TimeStamp);
            }
            EqualsSign vLiteral=binary
            {
                vParent.Timestamp = vLiteral;
            }
        )?
    ;

writeString returns [ValueExpression vResult]
    :
        vResult=nullLiteral
    |
        vResult=stringLiteral
    |
        vResult=binary
    |
        vResult=variable
    ;

returnStatement returns [ReturnStatement vResult = this.FragmentFactory.CreateFragment<ReturnStatement>()]
{
    ScalarExpression vExpression;
}
    : tReturn:Return 
        {
            UpdateTokenInfo(vResult,tReturn);            
        }
        // Conflicts with select (which can start with '(' )
        ((expression) => vExpression = expression
            {
                vResult.Expression = vExpression;
            }
        |
        /* empty */
        )
    ;
    
openCursorStatement returns [OpenCursorStatement vResult = FragmentFactory.CreateFragment<OpenCursorStatement>()]
{
    CursorId vCursorId;
}
    : tOpen:Open vCursorId = cursorId
        {
            UpdateTokenInfo(vResult,tOpen);
            vResult.Cursor = vCursorId;
        }
    ;

closeCursorStatement returns [CloseCursorStatement vResult = FragmentFactory.CreateFragment<CloseCursorStatement>()]
{
    CursorId vCursorId;
}
    : tClose:Close vCursorId = cursorId
        {
            UpdateTokenInfo(vResult,tClose);
            vResult.Cursor = vCursorId;
        }
    ;

deallocateCursorStatement returns [DeallocateCursorStatement vResult = FragmentFactory.CreateFragment<DeallocateCursorStatement>()]
{
    CursorId vCursorId;
}
    : tDeallocate:Deallocate vCursorId = cursorId
        {
            UpdateTokenInfo(vResult,tDeallocate);
            vResult.Cursor = vCursorId;
        }
    ;
    
fetchCursorStatement returns [FetchCursorStatement vResult]
{
    VariableReference vVariable;
}
    : tFetch:Fetch vResult = rowSelector 
        {
            UpdateTokenInfo(vResult,tFetch);
        }
        (Into vVariable = variable 
            {
                AddAndUpdateTokenInfo(vResult, vResult.IntoVariables, vVariable);
            }
            (Comma vVariable = variable
                {
                    AddAndUpdateTokenInfo(vResult, vResult.IntoVariables, vVariable);
                }
            )*
        )?
    ;

rowSelector returns [FetchCursorStatement vResult = FragmentFactory.CreateFragment<FetchCursorStatement>()]
{
    CursorId vCursorId;
    FetchType vFetchType;
}
    : (vCursorId = cursorId
            {
                vResult.Cursor = vCursorId;
            }
        )
    |    (vFetchType = fetchType From vCursorId = cursorId
            {
                vResult.Cursor = vCursorId;
                vResult.FetchType = vFetchType;
            }
        )
    |    (From vCursorId = cursorId
            {
                vResult.Cursor = vCursorId;
            }
        )
    ;
    
fetchType returns [FetchType vResult = FragmentFactory.CreateFragment<FetchType>()]
{
    ScalarExpression vOffset;
}
    :    tFetchOrientation:Identifier
        {
            vResult.Orientation = FetchOrientationHelper.Instance.ParseOption(tFetchOrientation);
        }
        (
            (vOffset = signedInteger
                {
                    if (vResult.Orientation != FetchOrientation.Relative && vResult.Orientation != FetchOrientation.Absolute)
                        throw GetUnexpectedTokenErrorException(tFetchOrientation);
                    vResult.RowOffset = vOffset;
                }
            )
            |
            (vOffset = variable
                {
                    if (vResult.Orientation != FetchOrientation.Relative && vResult.Orientation != FetchOrientation.Absolute)
                        throw GetUnexpectedTokenErrorException(tFetchOrientation);
                    vResult.RowOffset = vOffset;
                }
            )
            | /* empty */
                {
                    if (vResult.Orientation == FetchOrientation.Relative || vResult.Orientation == FetchOrientation.Absolute)
                        throw GetUnexpectedTokenErrorException(tFetchOrientation);
                }
        )
    ;

cursorId returns [CursorId vResult = FragmentFactory.CreateFragment<CursorId>()]
{
    Identifier vIdentifier;
    IdentifierOrValueExpression vName;
}
    :  {NextTokenMatches(CodeGenerationSupporter.Global)}?
        (tGlobal:Identifier vIdentifier = identifier
            {
                Match(tGlobal, CodeGenerationSupporter.Global);
                vResult.Name = IdentifierOrValueExpression(vIdentifier);
                vResult.IsGlobal = true;
            }
        )
    | vName = identifierOrVariable
        {
            vResult.Name = vName;
            vResult.IsGlobal = false;
        }
    ;

//////////////////////////////////////////////////////////////
// Drop statements
//////////////////////////////////////////////////////////////
dropStatements returns [TSqlStatement vResult]
    : tDrop:Drop
        (      vResult = dropDatabaseStatement
            | vResult = dropIndexStatement
            | vResult = dropStatisticsStatement
            | vResult = dropTableStatement 
            | vResult = dropProcedureStatement 
            | vResult = dropFunctionStatement 
            | vResult = dropViewStatement 
            | vResult = dropDefaultStatement 
            | vResult = dropRuleStatement 
            | vResult = dropTriggerStatement
        )
        {
            UpdateTokenInfo(vResult,tDrop);
        }
    ;

dropDatabaseStatement returns [DropDatabaseStatement vResult = FragmentFactory.CreateFragment<DropDatabaseStatement>()]
{
    Identifier vIdentifier;
}
    : Database vIdentifier = identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Databases, vIdentifier);
        }
        (Comma vIdentifier = identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Databases, vIdentifier);
            }
        )*
    ;

dropIndexStatement returns [DropIndexStatement vResult = FragmentFactory.CreateFragment<DropIndexStatement>()]
{
    DropIndexClauseBase vClause;
}
    : Index vClause = indexDropObject
        {
            AddAndUpdateTokenInfo(vResult, vResult.DropIndexClauses, vClause);
        }
        (Comma vClause = indexDropObject
            {
                AddAndUpdateTokenInfo(vResult, vResult.DropIndexClauses, vClause);
            }
        )*
    ;
        
dropStatisticsStatement returns [DropStatisticsStatement vResult = FragmentFactory.CreateFragment<DropStatisticsStatement>()]
{
    ChildObjectName vObjName;
}
    : Statistics vObjName = statisticsDropObject
        {
            AddAndUpdateTokenInfo(vResult, vResult.Objects, vObjName);
        }
        (Comma vObjName = statisticsDropObject
            {
                AddAndUpdateTokenInfo(vResult, vResult.Objects, vObjName);
            }
        )*
    ;
    
statisticsDropObject returns [ChildObjectName vResult]
    : vResult = childObjectNameWithThreePrefixes
        {
            if (vResult.BaseIdentifier == null)
                ThrowParseErrorException("SQL46038", vResult, TSqlParserResource.SQL46038Message);
        }
    ;
    
childObjectNameWithThreePrefixes returns [ChildObjectName vResult = FragmentFactory.CreateFragment<ChildObjectName>()]
{
    List<Identifier> vIdentifiers;
}
    : vIdentifiers = identifierList[4]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifiers);
        }
    ;

dropTableStatement returns [DropTableStatement vResult = FragmentFactory.CreateFragment<DropTableStatement>()]
    : Table dropObjectList[vResult,false]
    ;

dropProcedureStatement returns [DropProcedureStatement vResult = FragmentFactory.CreateFragment<DropProcedureStatement>()]
    : (Procedure | Proc) dropObjectList[vResult,true]
    ;

dropFunctionStatement returns [DropFunctionStatement vResult = FragmentFactory.CreateFragment<DropFunctionStatement>()]
    : Function dropObjectList[vResult,true]
    ;

dropViewStatement returns [DropViewStatement vResult = FragmentFactory.CreateFragment<DropViewStatement>()]
    : View dropObjectList[vResult,true]
    ;
    
dropDefaultStatement returns [DropDefaultStatement vResult = FragmentFactory.CreateFragment<DropDefaultStatement>()]
    : Default dropObjectList[vResult,true]
    ;
    
dropRuleStatement returns [DropRuleStatement vResult = FragmentFactory.CreateFragment<DropRuleStatement>()]
    : Rule dropObjectList[vResult,true]
    ;

dropTriggerStatement returns [DropTriggerStatement vResult = FragmentFactory.CreateFragment<DropTriggerStatement>()]
    : Trigger dropObjectList[vResult,true]
    ;

dropObjectList [DropObjectsStatement vParent, bool onlyTwoPartNames]
{
    SchemaObjectName vObject;
}
    : vObject = dropObject[onlyTwoPartNames]
        {
            AddAndUpdateTokenInfo(vParent, vParent.Objects, vObject);
        }
        (Comma vObject = dropObject[onlyTwoPartNames]
            {
                AddAndUpdateTokenInfo(vParent, vParent.Objects, vObject);
            }
        )*
    ;

dropObject [bool onlyTwoPartNames] returns [SchemaObjectName vResult]
    : vResult = schemaObjectThreePartName
        {
            if (onlyTwoPartNames)
                CheckTwoPartNameForSchemaObjectName(vResult, CodeGenerationSupporter.Drop);
        }
    ;

indexDropObject returns [BackwardsCompatibleDropIndexClause vResult = FragmentFactory.CreateFragment<BackwardsCompatibleDropIndexClause>()]
{
    ChildObjectName vObjName;
}
    : vObjName = childObjectNameWithThreePrefixes
        {
            if (vObjName.BaseIdentifier == null)
                ThrowParseErrorException("SQL46027", vObjName, TSqlParserResource.SQL46027Message);
            vResult.Index = vObjName;
        }
    ;
    
//////////////////////////////////////////////////////////////
// Drop statements end
//////////////////////////////////////////////////////////////
    
labelStatement returns [LabelStatement vResult = this.FragmentFactory.CreateFragment<LabelStatement>()]
    : tLabel:Label 
        {
            UpdateTokenInfo(vResult,tLabel);
            vResult.Value = tLabel.getText();
        }
    ;

gotoStatement returns [GoToStatement vResult = this.FragmentFactory.CreateFragment<GoToStatement>()]
{
    Identifier vIdentifier;
}
    :   tGoto:GoTo vIdentifier=nonQuotedIdentifier
        {
            UpdateTokenInfo(vResult,tGoto);
            vResult.LabelName = vIdentifier;
        }
    ;

beginStatements returns [TSqlStatement vResult = null]
    :
        ( Begin ( Distributed )? ( Tran | Transaction )  )=> // Syntactic predicate up to three lookaheads
        vResult=beginTransactionStatement
    |   
        vResult=beginEndBlockStatement
    ;

saveTransactionStatement returns [SaveTransactionStatement vResult = this.FragmentFactory.CreateFragment<SaveTransactionStatement>()]
    :
        tSave:Save
        {
            UpdateTokenInfo(vResult,tSave);
        }
        ( Tran | Transaction )
        transactionName[vResult]
    ;

rollbackTransactionStatement returns [RollbackTransactionStatement vResult = this.FragmentFactory.CreateFragment<RollbackTransactionStatement>()]
    :
        tRollback:Rollback
        {
            UpdateTokenInfo(vResult,tRollback);
        }
        (
            (Identifier)=>
            tWork:Identifier
            {
                Match(tWork, CodeGenerationSupporter.Work);
            }
        |
 
            (
            (

                    tTran:Tran 
                    {
                        UpdateTokenInfo(vResult,tTran);
                    }
                |   tTransaction:Transaction
                    {
                        UpdateTokenInfo(vResult,tTransaction);
                    }
                )
                (   
                    options {greedy=true;} : 
                    transactionName[vResult]
                )?
            )
        )?
    ;

commitTransactionStatement returns [CommitTransactionStatement vResult = this.FragmentFactory.CreateFragment<CommitTransactionStatement>()]
    :
        tCommit:Commit
        {
            UpdateTokenInfo(vResult,tCommit);
        }
        ( 
            (Identifier)=>
            tWork:Identifier
            {
                Match(tWork, CodeGenerationSupporter.Work);
            }
        |

            (
                (
                    tTran:Tran 
                    {
                        UpdateTokenInfo(vResult,tTran);
                    }
                |   tTransaction:Transaction
                    {
                        UpdateTokenInfo(vResult,tTransaction);
                    }
                )
                (
                    options {greedy=true;} : 
                    transactionName[vResult]
                )?
            )
        )?
    ;

beginTransactionStatement returns [BeginTransactionStatement vResult = this.FragmentFactory.CreateFragment<BeginTransactionStatement>()]
{
    ValueExpression vLiteral;
}
    :   tBegin:Begin
        {
            UpdateTokenInfo(vResult,tBegin);
        }
        ( 
            Distributed
            {
                vResult.Distributed = true;
            }

        )?
        ( 
            tTran:Tran 
            {
                UpdateTokenInfo(vResult,tTran);
            }
        |   tTransaction:Transaction
            {
                UpdateTokenInfo(vResult,tTransaction);
            }
        )
        (
            transactionName[vResult]
        )?
        (
            With tId:Identifier
            {
                Match(tId,CodeGenerationSupporter.Mark);
                UpdateTokenInfo(vResult,tId);
                vResult.MarkDefined = true;
            }
            (vLiteral = stringOrVariable
                {
                    vResult.MarkDescription = vLiteral;
                }
            )?
        )?
    ;

transactionName[TransactionStatement vParent]
{
    Identifier vIdentifier;
    IdentifierOrValueExpression vName;
}
    :   vName=identifierOrVariable
        {
            vParent.Name = vName;
        }
    |   vIdentifier = weirdTransactionName
        {
            vParent.Name = IdentifierOrValueExpression(vIdentifier);
        }
    ;
    
weirdTransactionName returns [Identifier vResult = FragmentFactory.CreateFragment<Identifier>()]
{
    System.Text.StringBuilder vStringBuilder = new System.Text.StringBuilder();
}
    :
        (
            tMinus:Minus
            {
                vStringBuilder.Append(tMinus.getText());
                UpdateTokenInfo(vResult, tMinus);
            }
        )?
        tInteger:Integer tColon:Colon
        {
            UpdateTokenInfo(vResult, tInteger);
            vStringBuilder.Append(tInteger.getText());
            vStringBuilder.Append(tColon.getText());
        }
        tranIdentifier[vStringBuilder, vResult]
        tDot:Dot
        {
            vStringBuilder.Append(tDot.getText());
        }
        tranIdentifier[vStringBuilder, vResult]
        {
            vResult.Value = vStringBuilder.ToString();
        }
    ;

tranIdentifier[System.Text.StringBuilder vStringBuilder, TSqlFragment vParent]
    :
        tId1:Identifier
        {
            vStringBuilder.Append(tId1.getText());
            UpdateTokenInfo(vParent,tId1);
        }
    |
        tId2:QuotedIdentifier
        {
            vStringBuilder.Append(tId2.getText());
            UpdateTokenInfo(vParent,tId2);
        }
    ;

beginEndBlockStatement returns [BeginEndBlockStatement vResult = this.FragmentFactory.CreateFragment<BeginEndBlockStatement>()]
{
    TSqlStatement vStatement;
    bool vParseErrorOccurred = false;
    StatementList vStatementList = FragmentFactory.CreateFragment<StatementList>();
}
    :
        tBegin:Begin 
        {
            UpdateTokenInfo(vResult,tBegin);
        }
        (
            vStatement=statementOptSemi
            {
                if (null != vStatement) // statement can be null if there was a parse error.
                    AddAndUpdateTokenInfo(vStatementList, vStatementList.Statements, vStatement);
                else 
                {
                    vParseErrorOccurred = true;
                    ThrowIfEndOfFileOrBatch();
                }
            }
        )+
        tEnd:End
        {
            vResult.StatementList = vStatementList;
            UpdateTokenInfo(vResult,tEnd);
            // Do not return a partial AST if there was a parse error.
            if (vParseErrorOccurred)
                vResult = null;
        }
    ;

breakStatement returns [BreakStatement vResult = this.FragmentFactory.CreateFragment<BreakStatement>()]
    :
        tBreak:Break
        {
            UpdateTokenInfo(vResult,tBreak);
        }
    ;

continueStatement returns [ContinueStatement vResult = this.FragmentFactory.CreateFragment<ContinueStatement>()]
    :
        tContinue:Continue
        {
            UpdateTokenInfo(vResult,tContinue);
        }
    ;

declareVariableElement returns [DeclareVariableElement vResult = this.FragmentFactory.CreateFragment<DeclareVariableElement>()]
{
    Identifier vIdentifier;
    DataTypeReference vDataType;
}
    : 
        vIdentifier=identifierVariable 
        (As)?
        (
            vDataType=scalarDataType
        |   
            vDataType=cursorDataType
        )
        {
            vResult.VariableName = vIdentifier;
            vResult.DataType = vDataType;
        }
    ;

declareStatements returns [TSqlStatement vResult = null]
{
    DeclareTableVariableBody vDeclareTableBody;
}
    : tDeclare:Declare
        ( (Variable (As)? Table)=>
            vDeclareTableBody=declareTableBody[IndexAffectingStatement.DeclareTableVariable]
            {
                DeclareTableVariableStatement vDeclare = FragmentFactory.CreateFragment<DeclareTableVariableStatement>();
                vDeclare.Body = vDeclareTableBody;
                vResult = vDeclare;                
            }
        |   vResult=declareVariableStatement
        |   vResult=declareCursorStatement
        )
        {
            UpdateTokenInfo(vResult,tDeclare);
        }
    ;
    
setStatements returns [TSqlStatement vResult = null ]
    : tSet:Set
        (
              {!NextTokenMatches(CodeGenerationSupporter.FipsFlagger)}? 
              vResult=predicateSetStatement
            | vResult=setVariableStatement 
            | vResult=setStatisticsStatement
            | vResult=setRowcountStatement
            | vResult=setOffsetsStatement
            | vResult=setCommandStatement
            | vResult=setTransactionIsolationLevelStatement
            | vResult=setTextSizeStatement
            | vResult=setIdentityInsertStatement
            | vResult=setErrorLevelStatement
        )
        {
            UpdateTokenInfo(vResult,tSet);
        }
   ;

setVariableStatement returns [SetVariableStatement vResult = this.FragmentFactory.CreateFragment<SetVariableStatement>()]
{
    VariableReference vLiteral;
    ScalarExpression vExpression;
    CursorDefinition vCursorDef;
}
    : vLiteral=variable
        {
            vResult.Variable = vLiteral;
        }
        EqualsSign 
        (
            vExpression=expression
            {
                vResult.Expression = vExpression;
            }
        |
            vCursorDef = cursorDefinition
            {
                vResult.CursorDefinition = vCursorDef;
            }
        )
    ;

// Looking for set statements like the following:
//
// set quoted_identifiers on
//
// or
//
// set ansi_nulls off

predicateSetStatement returns [PredicateSetStatement vResult = this.FragmentFactory.CreateFragment<PredicateSetStatement>()]
    : tOption:Identifier 
        {
            vResult.Options = PredicateSetOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql80);
        }
        (Comma tOption2:Identifier
            {
                vResult.Options |= PredicateSetOptionsHelper.Instance.ParseOption(tOption2, SqlVersionFlags.TSql80);
            }
        )* 
        setOnOff[vResult]
        {
            if ((vResult.Options & SetOptions.QuotedIdentifier) == SetOptions.QuotedIdentifier)
                _tokenSource.QuotedIdentifier = vResult.IsOn;
        }
    ;
    
setRowcountStatement returns [SetRowCountStatement vResult = FragmentFactory.CreateFragment<SetRowCountStatement>()]
{
    ValueExpression vLiteral;
}
    : RowCount vLiteral = integerOrVariable
        {
            vResult.NumberRows = vLiteral;
        }
    ;

setOffsetsStatement returns [SetOffsetsStatement vResult = FragmentFactory.CreateFragment<SetOffsetsStatement>()]
{
    SetOffsets vOffset;
}
    : Offsets vOffset = offsetItem 
        {
            vResult.Options = vOffset;
        }
        (Comma vOffset = offsetItem
            {
                vResult.Options |= vOffset;
            }
        )* 
        setOnOff[vResult]
    ;
    
offsetItem returns [SetOffsets vResult = SetOffsets.None]
    : Select    { vResult = SetOffsets.Select; }
    | From        { vResult = SetOffsets.From; }
    | Order        { vResult = SetOffsets.Order; }
    | Compute    { vResult = SetOffsets.Compute; }
    | Table        { vResult = SetOffsets.Table; }
    | ( Procedure | Proc ) { vResult = SetOffsets.Procedure; }
    | ( Execute | Exec ) { vResult = SetOffsets.Execute; }
    | tId:Identifier
        {
            if (TryMatch(tId,CodeGenerationSupporter.Statement))
                vResult = SetOffsets.Statement;
            else
            {
                Match(tId,CodeGenerationSupporter.Param);
                vResult = SetOffsets.Param;
            }
        }
    ;

setCommandStatement returns [SetCommandStatement vResult = FragmentFactory.CreateFragment<SetCommandStatement>()]
{
    SetCommand vCommand;
}
    : vCommand = setCommand
        {
            AddAndUpdateTokenInfo(vResult, vResult.Commands, vCommand);
        }
        (Comma vCommand = setCommand
            {
                AddAndUpdateTokenInfo(vResult, vResult.Commands, vCommand);
            }
        )*
    ;
    
setCommand returns [SetCommand vResult = null]
{
    ScalarExpression vValue;
}
    :    // Fips flagger - following SQL server parser behavior and checking for valid values here
        {NextTokenMatches(CodeGenerationSupporter.FipsFlagger)}? 
        (tFipsFlagger:Identifier vResult = fipsFlaggerLevel)
        |
        (tId:Identifier vValue = possibleNegativeConstantOrIdentifier
            {
                GeneralSetCommand vGeneralSetCommand = FragmentFactory.CreateFragment<GeneralSetCommand>();
                vGeneralSetCommand.CommandType = GeneralSetCommandTypeHelper.Instance.ParseOption(tId);
                vGeneralSetCommand.Parameter = vValue;
                vResult = vGeneralSetCommand;
            }
        )
    ;

fipsFlaggerLevel returns [SetFipsFlaggerCommand vResult = FragmentFactory.CreateFragment<SetFipsFlaggerCommand>()]
{
    StringLiteral vLevel;
}
    : tOff:Off
        {
            vResult.ComplianceLevel = FipsComplianceLevel.Off;
            UpdateTokenInfo(vResult,tOff);
        }
    | vLevel=stringLiteral
        {
            vResult.ComplianceLevel = FipsComplianceLevelHelper.Instance.ParseOption(GetFirstToken(vLevel));
            vResult.UpdateTokenInfo(vLevel);
        }
    ;
    
setTransactionIsolationLevelStatement returns [SetTransactionIsolationLevelStatement vResult = FragmentFactory.CreateFragment<SetTransactionIsolationLevelStatement>()]
    : (Transaction | Tran) tIsolation:Identifier tLevel:Identifier 
        {
            Match(tIsolation,CodeGenerationSupporter.Isolation);
            Match(tLevel,CodeGenerationSupporter.Level);
        }
        (
            (Read tCommittedUncommitted:Identifier
                {
                    if (TryMatch(tCommittedUncommitted,CodeGenerationSupporter.Committed))
                        vResult.Level = IsolationLevel.ReadCommitted;
                    else
                    {
                        Match(tCommittedUncommitted, CodeGenerationSupporter.Uncommitted);
                        vResult.Level = IsolationLevel.ReadUncommitted;
                    }
                    UpdateTokenInfo(vResult,tCommittedUncommitted);
                }
            )
            |
            (tRepeatable:Identifier tRead:Read
                {
                    Match(tRepeatable, CodeGenerationSupporter.Repeatable);
                    vResult.Level = IsolationLevel.RepeatableRead;
                    UpdateTokenInfo(vResult,tRead);
                }
            )
            |
            (tSerializable:Identifier
                {
                    Match(tSerializable, CodeGenerationSupporter.Serializable);
                    vResult.Level = IsolationLevel.Serializable;
                    UpdateTokenInfo(vResult,tSerializable);
                }
            )
        )
    ;
    
setTextSizeStatement returns [SetTextSizeStatement vResult = FragmentFactory.CreateFragment<SetTextSizeStatement>()]
{
    ScalarExpression vTextSize;
}
    : TextSize vTextSize = signedInteger
        {
            vResult.TextSize = vTextSize;
        }
    ;
    
setIdentityInsertStatement returns [SetIdentityInsertStatement vResult = FragmentFactory.CreateFragment<SetIdentityInsertStatement>()]
{
    SchemaObjectName vTable;
}
    : IdentityInsert vTable = schemaObjectThreePartName 
        {
            vResult.Table = vTable;
        }
        setOnOff[vResult]
    ;
  
setErrorLevelStatement returns [SetErrorLevelStatement vResult = FragmentFactory.CreateFragment<SetErrorLevelStatement>()]
{
    ScalarExpression vLevel;
}
    : Errlvl vLevel = signedInteger
        {
            vResult.Level = vLevel;
        }
    ;
    
setStatisticsStatement returns [SetStatisticsStatement vResult = FragmentFactory.CreateFragment<SetStatisticsStatement>()]
    : Statistics tStatOption:Identifier 
        {
            vResult.Options = SetStatisticsOptionsHelper.Instance.ParseOption(tStatOption, SqlVersionFlags.TSql80);
        }
        (Comma tStatOption2:Identifier
            {
                vResult.Options |= SetStatisticsOptionsHelper.Instance.ParseOption(tStatOption2, SqlVersionFlags.TSql80);
            }
        )* 
        setOnOff[vResult]
    ;
    
setOnOff [SetOnOffStatement vParent]
    : tOn:On
         {
            UpdateTokenInfo(vParent,tOn);
            vParent.IsOn = true;
         }
     | tOff:Off
         {
            UpdateTokenInfo(vParent,tOff);
            vParent.IsOn = false;
         }
    ;   
   
declareTableBody [IndexAffectingStatement statementType] returns [DeclareTableVariableBody vResult]
{
    Identifier vLiteral;
    bool vAsDefined = false;
}
    :    vLiteral=identifierVariable Table vResult=declareTableBodyMain[statementType]
        {
            vResult.VariableName = vLiteral;
            vResult.AsDefined = vAsDefined;
        }
    ;

declareTableBodyMain [IndexAffectingStatement statementType] returns [DeclareTableVariableBody vResult = this.FragmentFactory.CreateFragment<DeclareTableVariableBody>()]
{
    TableDefinition vTableDefinition;
}
    : 
        LeftParenthesis
        vTableDefinition = tableDefinition[statementType, null]
        {
            vResult.Definition = vTableDefinition;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

tableDefinition[IndexAffectingStatement statementType, AlterTableAddTableElementStatement vStatement] returns [TableDefinition vResult = this.FragmentFactory.CreateFragment<TableDefinition>()]
{
    if(PhaseOne && vStatement != null)
    {
        //In PhaseOne, need to make sure Definition is set on the statement
        vStatement.Definition = vResult;
    }
}
    :
        tableElement[statementType, vResult, vStatement] 
        ( Comma tableElement[statementType, vResult, vStatement] )* 
    ;


declareVariableStatement returns [DeclareVariableStatement vResult = this.FragmentFactory.CreateFragment<DeclareVariableStatement>()]
{
    DeclareVariableElement vDeclareVariableElement;
}
    : vDeclareVariableElement=declareVariableElement
        {
            AddAndUpdateTokenInfo(vResult, vResult.Declarations, vDeclareVariableElement);
        }
        ( Comma vDeclareVariableElement=declareVariableElement
            {
                AddAndUpdateTokenInfo(vResult, vResult.Declarations, vDeclareVariableElement);
            }
        )*
    ;

declareCursorStatement returns [DeclareCursorStatement vResult = FragmentFactory.CreateFragment<DeclareCursorStatement>()]
{
    Identifier vIdentifier;
    CursorDefinition vCursorDef;
    List<CursorOption> vOptions = new List<CursorOption>();
}
    : vIdentifier = identifier cursorOpts[true, vOptions] vCursorDef = cursorDefinitionOptions[vOptions]
        {
            vResult.Name = vIdentifier;
            vResult.CursorDefinition = vCursorDef;
        }
    ;
    
cursorDefinition returns [CursorDefinition vResult]
{
    List<CursorOption> vOptions = new List<CursorOption>();
}
    : vResult = cursorDefinitionOptions[vOptions]
    ;  

cursorDefinitionOptions[IList<CursorOption> vOptions] returns [CursorDefinition vResult = FragmentFactory.CreateFragment<CursorDefinition>()]
{
    SelectStatement vSelect;
}
    : Cursor cursorOpts[false, vOptions] For vSelect=selectStatement
        {
            vResult.Select = vSelect;
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOptions);
        }
    ;
    
cursorOpts [bool oldSyntax, IList<CursorOption> vOptions]
{
    CursorOption vOption;
}
    :    (vOption=cursorOption
            {
                // Only INSENSITIVE and SCROLL are allowed in old syntax, and INSENSITIVE is not allowed in new syntax
                if (oldSyntax)
                {
                    if (vOption.OptionKind != CursorOptionKind.Insensitive && vOption.OptionKind  != CursorOptionKind.Scroll)
                        ThrowIncorrectSyntaxErrorException(vOption);
                }
                else
                {
                    if (vOption.OptionKind == CursorOptionKind.Insensitive)
                        ThrowIncorrectSyntaxErrorException(vOption);
                }
                vOptions.Add(vOption);     
            }
        )*
    ;

cursorOption returns [CursorOption vResult = FragmentFactory.CreateFragment<CursorOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind=CursorOptionsHelper.Instance.ParseOption(tOption);
        }
    ;
    
createIndexStatement returns [CreateIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateIndexStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    ColumnWithSortOrder vColumnWithSortOrder;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
}
    : tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        ( Unique
            {
                vResult.Unique = true;
            }
        )?
        (
            Clustered
            {
                vResult.Clustered = true;
            }
        |   NonClustered
            {
                vResult.Clustered = false;
            }
        )?
        Index vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
        On vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.OnName = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        LeftParenthesis vColumnWithSortOrder=columnWithSortOrder
        {
            AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
        }
        ( Comma vColumnWithSortOrder=columnWithSortOrder
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        (
            With indexLegacyOptionList[vResult]
            {
                vResult.Translated80SyntaxTo90 = true;
            }
        )?
        (
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
    ;

filegroupOrPartitionScheme returns [FileGroupOrPartitionScheme vResult = this.FragmentFactory.CreateFragment<FileGroupOrPartitionScheme>()]
{
    IdentifierOrValueExpression vTSqlFragment;
}
    : vTSqlFragment=stringOrIdentifier
        {
            vResult.Name = vTSqlFragment;
        }
    ;

indexLegacyOptionList[CreateIndexStatement vParent]
{
    IndexOption vIndexOption;
}
    :   vIndexOption=indexLegacyOption
        {
            VerifyAllowedIndexOption(IndexAffectingStatement.CreateIndex, vIndexOption);
            AddAndUpdateTokenInfo(vParent, vParent.IndexOptions, vIndexOption);
        }
        (
            Comma vIndexOption=indexLegacyOption
            {
                VerifyAllowedIndexOption(IndexAffectingStatement.CreateIndex, vIndexOption);
                AddAndUpdateTokenInfo(vParent, vParent.IndexOptions, vIndexOption);
            }
        )*
    ;

fillFactorOption returns [IndexExpressionOption vResult = this.FragmentFactory.CreateFragment<IndexExpressionOption>()]
{
    Literal vValue;
}    
    :   tFillFactor:FillFactor EqualsSign vValue=integer
        {
            CheckFillFactorRange(vValue);
            vResult.OptionKind = IndexOptionKind.FillFactor;
            vResult.Expression = vValue;
            UpdateTokenInfo(vResult, tFillFactor);
        }
    ;

indexLegacyOption returns [IndexOption vResult = null]
    :
        vResult=fillFactorOption
    |   tId2:Identifier
        {
            IndexStateOption vOption = this.FragmentFactory.CreateFragment<IndexStateOption>();
            vResult = vOption;
            vOption.OptionKind = ParseIndexLegacyWithOption(tId2);
            UpdateTokenInfo(vOption, tId2);
            vOption.OptionState = OptionState.On;
        }
    ;
    
subqueryExpressionAsStatement returns [SelectStatement vResult = FragmentFactory.CreateFragment<SelectStatement>()]
{
    QueryExpression vQueryExpression;
}
    : vQueryExpression=subqueryExpression
        {
            vResult.QueryExpression = vQueryExpression;
        }
    ; 

selectStatement returns [SelectStatement vResult = null]
    : vResult=select
    ;

select returns [SelectStatement vResult = this.FragmentFactory.CreateFragment<SelectStatement>()]
{
    QueryExpression vQueryExpression;
    OrderByClause vOrderByClause;
    ComputeClause vComputeClause;
    ForClause vForClause;
}
    :   vQueryExpression=queryExpression[vResult]
        (
            vOrderByClause=orderByClause
            {
                vQueryExpression.OrderByClause = vOrderByClause;
            }
        )?
        (
            vComputeClause=computeClause
            {
                AddAndUpdateTokenInfo(vResult, vResult.ComputeClauses, vComputeClause);
            }
        )*
        (vForClause = forClause
            {
                vQueryExpression.ForClause = vForClause;
            }
        )?
        (optimizerHints[vResult, vResult.OptimizerHints])?
        {
            vResult.QueryExpression = vQueryExpression;
        }
    ;

derivedTable returns [QueryDerivedTable vResult = FragmentFactory.CreateFragment<QueryDerivedTable>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=subqueryExpression tRParen:RightParenthesis
        {
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tLParen);
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAlias[vResult]
        (
            options {greedy=true;} :
            columnNameList[vResult, vResult.Columns]
        )?
    ;

subquery [ExpressionFlags expressionFlags] returns [ScalarSubquery vResult = this.FragmentFactory.CreateFragment<ScalarSubquery>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=subqueryExpression tRParen:RightParenthesis
        {
	    if (ExpressionFlags.ScalarSubqueriesDisallowed == (expressionFlags & ExpressionFlags.ScalarSubqueriesDisallowed))
	    {
		ThrowParseErrorException("SQL46098", vQueryExpression, TSqlParserResource.SQL46098Message);
	    }	    
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tLParen);
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

subqueryExpression returns [QueryExpression vResult = null]
{
    BinaryQueryExpression vBinaryQueryExpression = null;
}
    :
        vResult=subqueryExpressionUnit
        (
            {
                vBinaryQueryExpression = this.FragmentFactory.CreateFragment<BinaryQueryExpression>();
                vBinaryQueryExpression.FirstQueryExpression = vResult;
            }
            (
                Union
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Union;
                }
            |
                Except
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Except;
                }
            |
                Intersect
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Intersect;
                }
            )
            (
                All
                {
                    vBinaryQueryExpression.All = true;
                }
            )?
            vResult=subqueryExpressionUnit
            {
                vBinaryQueryExpression.SecondQueryExpression = vResult;
                vResult = vBinaryQueryExpression;
            }
        )*
    ;

subqueryExpressionUnit returns [QueryExpression vResult = null]
    :
        vResult=subquerySpecification
    |   
        vResult=subqueryParenthesis
    ;

subqueryParenthesis returns [QueryParenthesisExpression vResult = this.FragmentFactory.CreateFragment<QueryParenthesisExpression>()]
{
    QueryExpression vQueryExpression;
}
    :
        tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
        }
        vQueryExpression=subqueryExpression
        {
            vResult.QueryExpression = vQueryExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;


queryExpression[SelectStatement vSelectStatement] returns [QueryExpression vResult = null]
{
    BinaryQueryExpression vBinaryQueryExpression = null;
}
    :
        vResult=queryExpressionUnit[vSelectStatement]
        (
            {
                vBinaryQueryExpression = this.FragmentFactory.CreateFragment<BinaryQueryExpression>();
                vBinaryQueryExpression.FirstQueryExpression = vResult;
            }
            (
                Union
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Union;
                }
            |
                Except
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Except;
                }
            |
                Intersect
                {
                    vBinaryQueryExpression.BinaryQueryExpressionType = BinaryQueryExpressionType.Intersect;
                }
            )
            (
                All
                {
                    vBinaryQueryExpression.All = true;
                }
            )?
            vResult=queryExpressionUnit[null]
            {
                vBinaryQueryExpression.SecondQueryExpression = vResult;
                vResult = vBinaryQueryExpression;
            }
        )*
    ;

queryExpressionUnit[SelectStatement vSelectStatement] returns [QueryExpression vResult = null]
    :
        vResult=querySpecification[vSelectStatement]
    |   
        vResult=queryParenthesis[vSelectStatement]
    ;

queryParenthesis[SelectStatement vSelectStatement] returns [QueryParenthesisExpression vResult = this.FragmentFactory.CreateFragment<QueryParenthesisExpression>()]
{
    QueryExpression vQueryExpression;
}
    :
        tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
        }
        vQueryExpression=queryExpression[vSelectStatement]
        {
            vResult.QueryExpression = vQueryExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

subquerySpecification returns [QuerySpecification vResult = this.FragmentFactory.CreateFragment<QuerySpecification>()]
{
    TopRowFilter vTopRowFilter;
    UniqueRowFilter vUniqueRowFilter;
    SelectElement vSelectColumn;
    FromClause vFromClause;
    WhereClause vWhereClause;
    GroupByClause vGroupByClause;
    HavingClause vHavingClause;
    BrowseForClause vBrowseForClause;
    OrderByClause vOrderByClause;
}
    :    tSelect:Select
        {
            UpdateTokenInfo(vResult,tSelect);            
        } 
        ( 
            vUniqueRowFilter=uniqueRowFilter
            {
                vResult.UniqueRowFilter = vUniqueRowFilter;
            }
        )?
        (
            vTopRowFilter=topRowFilter
            {
                vResult.TopRowFilter = vTopRowFilter;
            }
        )?
        vSelectColumn=selectColumnOrStarExpression
        {
            AddAndUpdateTokenInfo(vResult, vResult.SelectElements, vSelectColumn);
        }
        ( 
            Comma vSelectColumn=selectColumnOrStarExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.SelectElements, vSelectColumn);
            }
        )*
        vFromClause = fromClauseOpt
        {
                vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=whereClause
            {
                vResult.WhereClause = vWhereClause;
            }
        )?
        (
            vGroupByClause=groupByClause
            {
                vResult.GroupByClause = vGroupByClause;
            }
        )?
        (
            vHavingClause=havingClause
            {
                vResult.HavingClause = vHavingClause;
            }
        )?
        (
            vOrderByClause=orderByClause
            {
                vResult.OrderByClause = vOrderByClause;
            }
        )?
        (
           {LA(1) == For && LA(2) == Browse}?
            For vBrowseForClause = browseForClause
            {
                vResult.ForClause = vBrowseForClause;
            }
        | /* empty */
        )
        {
            if (vResult.OrderByClause != null && vResult.TopRowFilter == null)
            {
                ThrowParseErrorException("SQL46047", vResult, TSqlParserResource.SQL46047Message);        
            }
            
            if (vResult.TopRowFilter != null && 
                vResult.TopRowFilter.WithTies &&
                vResult.OrderByClause == null)
            {
                ThrowParseErrorException("SQL46048", vResult, TSqlParserResource.SQL46048Message);        
            }            
        }
    ;

browseForClause returns [BrowseForClause vResult = this.FragmentFactory.CreateFragment<BrowseForClause>()]
    :
        tBrowse:Browse
        {
            UpdateTokenInfo(vResult,tBrowse);
        }
    ;

querySpecification [SelectStatement vSelectStatement] returns [QuerySpecification vResult = this.FragmentFactory.CreateFragment<QuerySpecification>()]
{
    TopRowFilter vTopRowFilter;
    UniqueRowFilter vUniqueRowFilter;
    SchemaObjectName vSchemaObjectName;
    FromClause vFromClause;
    WhereClause vWhereClause;
    GroupByClause vGroupByClause;
    HavingClause vHavingClause;
}
    : 
        tSelect:Select
        {
            UpdateTokenInfo(vResult,tSelect);            
        } 
        ( 
            vUniqueRowFilter=uniqueRowFilter
            {
                vResult.UniqueRowFilter = vUniqueRowFilter;
            }
        )?
        (
            vTopRowFilter=topRowFilter
            {
                vResult.TopRowFilter = vTopRowFilter;
            }
        )?
        selectExpression[vResult] 
        ( 
            Comma selectExpression[vResult] 
        )*
        (
            tInto:Into
            vSchemaObjectName=schemaObjectThreePartName
            {
                if(vSelectStatement == null)
                {
                    ThrowIncorrectSyntaxErrorException(tInto);
                }
                vSelectStatement.Into = vSchemaObjectName;
            }
        )?
        vFromClause = fromClauseOpt
        {
            vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=whereClause
            {
                vResult.WhereClause = vWhereClause;
            }
        )?
        (
            vGroupByClause=groupByClause
            {
                vResult.GroupByClause = vGroupByClause;
            }
        )?
        (
            vHavingClause=havingClause
            {
                vResult.HavingClause = vHavingClause;
            }
        )?
    ;

uniqueRowFilter returns [UniqueRowFilter vResult = UniqueRowFilter.NotSpecified]
    :
        (
            All
            {
                vResult = UniqueRowFilter.All;
            }
        |
            Distinct
            {
                vResult = UniqueRowFilter.Distinct;
            }
        )
    ;

// This rule corresponds to topn in Sql Server grammar.
topRowFilter returns [TopRowFilter vResult = FragmentFactory.CreateFragment<TopRowFilter>()]
{
    ScalarExpression vExpression;
}
    : tTop:Top vExpression=integerOrRealOrNumeric
        {
            UpdateTokenInfo(vResult,tTop);
            vResult.Expression = vExpression;
        }
        (
            tPercent:Percent
            {
                ThrowIfPercentValueOutOfRange(vExpression);
                UpdateTokenInfo(vResult,tPercent);
                vResult.Percent = true;
            }
        )?
        (
            With tId:Identifier
            {
                Match(tId,CodeGenerationSupporter.Ties);
                UpdateTokenInfo(vResult,tId);
                vResult.WithTies = true;
            }
        )?
    ;

selectExpression[QuerySpecification vParent]
{
    SelectScalarExpression vSelectColumn;
    SelectSetVariable vSelectSetVariable;
    SelectStarExpression vSelectStarExpression;
}
    :
        (Variable EqualsSign)=>
        vSelectSetVariable=selectSetVariable
        {
            AddAndUpdateTokenInfo(vParent, vParent.SelectElements, vSelectSetVariable);
        }
    |
        (selectStarExpression)=>
        vSelectStarExpression=selectStarExpression
        {
            AddAndUpdateTokenInfo(vParent, vParent.SelectElements, vSelectStarExpression);        
        }
    |
        vSelectColumn=selectColumn
        {
            AddAndUpdateTokenInfo(vParent, vParent.SelectElements, vSelectColumn);
        }
    ;

selectColumnOrStarExpression returns [SelectElement vResult]
    :
        (selectStarExpression)=>
        vResult=selectStarExpression
        |
        vResult=selectColumn
    ;

selectColumn returns [SelectScalarExpression vResult = this.FragmentFactory.CreateFragment<SelectScalarExpression>()]
{
    ScalarExpression vExpression;
    IdentifierOrValueExpression vColumnName;
}   :
        vExpression=selectColumnExpression
        {
            vResult.Expression = vExpression;            
        } 
        (
            options {greedy=true;} : 
            (
                As
            )?
            (
                vColumnName=stringOrIdentifier
                {
                    vResult.ColumnName = vColumnName;
                }
            )
        )?
    |
        (
            vColumnName=stringOrIdentifier
            {
                vResult.ColumnName = vColumnName;
            }
        )
        EqualsSign
        (
            vExpression=selectColumnExpression
            {
                vResult.Expression = vExpression;            
            } 
        )
    ;

selectStarExpression returns [SelectStarExpression vResult = this.FragmentFactory.CreateFragment<SelectStarExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        (
            // Caveat: This would have a problem with ...IdentityColumn
            // however it works because table name always has to be 
            // written. 
            vMultiPartIdentifier=multiPartIdentifier[-1]
            {
                vResult.Qualifier=vMultiPartIdentifier;
            }
            Dot tStar1:Star
            {
                UpdateTokenInfo(vResult,tStar1);
            }
        | 
            tStar2:Star
            {
                UpdateTokenInfo(vResult,tStar2);
            }
        )
        { 
            CheckStarQualifier(vResult);
        }        
    ;

selectColumnExpression returns [ScalarExpression vResult = null]
    :
        vResult=expression
    |   
        vResult=identityFunction
    ;

identityFunction returns [IdentityFunctionCall vResult = this.FragmentFactory.CreateFragment<IdentityFunctionCall>()]
{
    DataTypeReference vDataType;
    ScalarExpression vExpression;
}
    :
        tIdentity:Identity
        {
            UpdateTokenInfo(vResult,tIdentity);
        }
        LeftParenthesis
        vDataType=scalarDataType
        {
            vResult.DataType = vDataType;
        }
        (
            Comma 
            vExpression=seedIncrement 
            {
                vResult.Seed = vExpression;
            }
            Comma 
            vExpression=seedIncrement 
            {
                vResult.Increment = vExpression;
            }            
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

selectSetVariable returns [SelectSetVariable vResult = this.FragmentFactory.CreateFragment<SelectSetVariable>()]
{
    VariableReference vLiteral;
    ScalarExpression vExpression;
}
    : vLiteral=variable EqualsSign vExpression=expression
        {
            vResult.Variable = vLiteral;
            vResult.Expression = vExpression;
        }
    ;

fromClauseOpt returns [FromClause vResult = null]
    :   
        (
            vResult = fromClause
        |
        )
    ;

fromClause returns [FromClause vResult = this.FragmentFactory.CreateFragment<FromClause>()]
{
    TableReference vTableReference;
}   
    :   
        tFrom:From vTableReference=selectTableReferenceWithOdbc
        {
            UpdateTokenInfo(vResult,tFrom);
            AddAndUpdateTokenInfo(vResult, vResult.TableReferences, vTableReference);
        }
        ( Comma vTableReference=selectTableReferenceWithOdbc
            {
                AddAndUpdateTokenInfo(vResult, vResult.TableReferences, vTableReference);
            }
        )*
    ;

selectTableReferenceWithOdbc returns [TableReference vResult = null]
    :
        vResult=selectTableReference
    |
        vResult=odbcQualifiedJoin
    ;

selectTableReference returns [TableReference vResult = null]
    :
        vResult=selectTableReferenceElement
        (
            joinElement[ref vResult]
        )*
    ;

odbcInitiator
    :    tOdbcInitiator:OdbcInitiator
        {
            ThrowParseErrorException("SQL46036", tOdbcInitiator, TSqlParserResource.SQL46036Message);
        }
    ;

odbcQualifiedJoin returns [OdbcQualifiedJoinTableReference vResult = this.FragmentFactory.CreateFragment<OdbcQualifiedJoinTableReference>()]
{
    TableReference vSelectTableReference;
}
    :
        (
            tLCurly:LeftCurly
            {
                UpdateTokenInfo(vResult,tLCurly);
            }
        |
            odbcInitiator
        )
        tOj:Identifier
        {
            Match(tOj, CodeGenerationSupporter.Oj);
        }
        (
            vSelectTableReference=odbcQualifiedJoin
        |
            vSelectTableReference=selectTableReference
            {
                if (!(vSelectTableReference is QualifiedJoin))
                {
                    ThrowParseErrorException("SQL46035", tLCurly, TSqlParserResource.SQL46035Message);
                }
            }
        )
        {
            vResult.TableReference = vSelectTableReference;
        }
        tRCurly:RightCurly
        {
            UpdateTokenInfo(vResult,tRCurly);
        }
    ;

odbcConvertSpecification returns [OdbcConvertSpecification vResult = this.FragmentFactory.CreateFragment<OdbcConvertSpecification>()]
{
    Identifier vIdentifier;
}
    :
        vIdentifier=nonQuotedIdentifier
        {
            vResult.Identifier = vIdentifier;
        }
    ;

extractFromExpression returns [ExtractFromExpression vResult = this.FragmentFactory.CreateFragment<ExtractFromExpression>()]
{
    ScalarExpression vExpression;
    Identifier vExtractedElement;
}
    :
        tExtract:Identifier LeftParenthesis vExtractedElement=identifier From vExpression=expression
        {
            Match(tExtract, CodeGenerationSupporter.Extract);
            vResult.Expression = vExpression;
            vResult.ExtractedElement = vExtractedElement;
        }
        RightParenthesis
    ;

odbcFunctionCall returns [OdbcFunctionCall vResult = this.FragmentFactory.CreateFragment<OdbcFunctionCall>()]
{
    ScalarExpression vExpression;
    Identifier vIdentifier = this.FragmentFactory.CreateFragment<Identifier>();
    vResult.ParametersUsed = true;
}
    :   
        tLCurly:LeftCurly
        {
            UpdateTokenInfo(vResult,tLCurly);
        }
        tOj:Identifier
        {
            Match(tOj, CodeGenerationSupporter.Fn);
        }
        {
            if (LA(1) != EOF) // EOF text is null.
            {
                // Getting the next token's text as an identifier.
                vIdentifier.SetUnquotedIdentifier(LT(1).getText());
                vResult.Name = vIdentifier;
            }
        }
        (
            Convert LeftParenthesis vExpression=expression
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            Comma vExpression=odbcConvertSpecification
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            RightParenthesis
        |
            Truncate LeftParenthesis vExpression=expression
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            Comma vExpression=expression
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            RightParenthesis
        |
            (
                Database 
            |
                User
            |
                CurrentDate
            )
                LeftParenthesis RightParenthesis
        |
            (
                Insert 
            |
                Left
            |
                Right
            )
            LeftParenthesis
            expressionList[vResult, vResult.Parameters]
            RightParenthesis
        |
            (
                CurrentTime
            |
                CurrentTimestamp
            )
            (
                LeftParenthesis
                (
                    expressionList[vResult, vResult.Parameters]
                )?
                RightParenthesis                
            |  /* empty */
                {
                    vResult.ParametersUsed = false;
                }
            )
        |
            {NextTokenMatches(CodeGenerationSupporter.Extract)}?
            vExpression=extractFromExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
        |
            Identifier LeftParenthesis
            (
                expressionList[vResult, vResult.Parameters]
            )?
            RightParenthesis
        )
        tRCurly:RightCurly
        {
            UpdateTokenInfo(vResult,tRCurly);
        }
    ;

expressionList[TSqlFragment vParent, IList<ScalarExpression> expressions]
{
    ScalarExpression vExpression;
}
    :   vExpression=expression
        {
            AddAndUpdateTokenInfo(vParent, expressions, vExpression);
        }
        (
            Comma vExpression=expression
            {
                AddAndUpdateTokenInfo(vParent, expressions, vExpression);
            }
        )*
    ;

joinTableReference returns [TableReference vResult = null]
{
    IToken tAfterJoinParenthesis = null;
}
    :        
        // Here we use save/skip optimization specific to nested lookaheads. That is because
        // rule "joinParenthesis" calls back to "joinTableReference" so the below lookahead 
        // has an exponential complexity. See method "TSql80ParserBaseInternal.SkipGuessing"
        // for details.
        ({ if (!SkipGuessing(tAfterJoinParenthesis)) }: 
            vResult=joinParenthesis ({ SaveGuessing(out tAfterJoinParenthesis); }:))=>
        ({ if (!SkipGuessing(tAfterJoinParenthesis)) }: 
            vResult=joinParenthesis)
        (  // If the first element is a joinParenthesis more join's are optional
            joinElement[ref vResult]
        )*
    |
        // If the first element is not a joinParenthesis at least one join is mandatory.
        vResult=selectTableReferenceElementWithOutJoinParenthesis
        (
            joinElement[ref vResult]
        )+
        
    ;

joinElement[ref TableReference vResult]
    :
        unqualifiedJoin[ref vResult]
    |   qualifiedJoin[ref vResult]
    ;

selectTableReferenceElement returns [TableReference vResult = null]
    :        
        (joinParenthesis)=>
        vResult=joinParenthesis
    |   vResult=selectTableReferenceElementWithOutJoinParenthesis
    ;

selectTableReferenceElementWithOutJoinParenthesis returns [TableReference vResult = null]
    :        
        vResult=schemaObjectOrFunctionTableReference
    |   vResult=builtInFunctionTableReference
    |   vResult=variableTableReference
    |   vResult=derivedTable
    |    vResult=openRowset
    |    vResult=fulltextTableReference
    |    vResult=openXmlTableReference
    ;

unqualifiedJoin[ref TableReference vResult]
{
    UnqualifiedJoin vUnqualifiedJoin = this.FragmentFactory.CreateFragment<UnqualifiedJoin>();
    TableReference vSelectTableReference;
}
    :
        (
            Cross 
            (
                Join
                {
                    vUnqualifiedJoin.UnqualifiedJoinType = UnqualifiedJoinType.CrossJoin;
                }
            |
                tApply:Identifier
                {
                    Match(tApply, CodeGenerationSupporter.Apply);
                    vUnqualifiedJoin.UnqualifiedJoinType = UnqualifiedJoinType.CrossApply;
                }
            )
        |
            Outer tApply1:Identifier
            {
                Match(tApply1, CodeGenerationSupporter.Apply);
                vUnqualifiedJoin.UnqualifiedJoinType = UnqualifiedJoinType.OuterApply;
            }
        )
        vSelectTableReference=selectTableReferenceElement
        {
            vUnqualifiedJoin.FirstTableReference = vResult;
            vUnqualifiedJoin.SecondTableReference = vSelectTableReference;
            vResult = vUnqualifiedJoin;
        }
    ;    

qualifiedJoin[ref TableReference vResult]
{
    QualifiedJoin vQualifiedJoin = this.FragmentFactory.CreateFragment<QualifiedJoin>();
    TableReference vSelectTableReference;
    BooleanExpression vExpression;
}
    :
        (
            Join
            {
                vQualifiedJoin.QualifiedJoinType = QualifiedJoinType.Inner;
            }
        |   
            (
                Inner        
                {
                    vQualifiedJoin.QualifiedJoinType = QualifiedJoinType.Inner;
                }
            |   Left ( Outer )?
                {
                    vQualifiedJoin.QualifiedJoinType = QualifiedJoinType.LeftOuter;
                }
            |   Right ( Outer )?
                {
                    vQualifiedJoin.QualifiedJoinType = QualifiedJoinType.RightOuter;
                }
            |   Full ( Outer )?
                {
                    vQualifiedJoin.QualifiedJoinType = QualifiedJoinType.FullOuter;
                }
            )
            (joinHint[vQualifiedJoin])?
            Join
        )     
        // Pay attention, this is recursive in the non-traditional LL(k) way
        // due to the On clause in qualifiedJoin
        vSelectTableReference=selectTableReferenceWithOdbc
        {
            vQualifiedJoin.FirstTableReference = vResult;
            vQualifiedJoin.SecondTableReference = vSelectTableReference;
        }
        On vExpression=booleanExpression
        {
            vQualifiedJoin.SearchCondition = vExpression;
            vResult = vQualifiedJoin;
        }
    ;    

joinHint[QualifiedJoin vParent]
    :
        tId1:Identifier
        (
            tId2:Identifier
            {
                Match(tId1,CodeGenerationSupporter.Local);
                vParent.JoinHint = JoinHintHelper.Instance.ParseOption(tId2);
                if (vParent.JoinHint == JoinHint.Remote)
                {
                    ThrowIncorrectSyntaxErrorException(tId2);
                }
            }
        |
            /* empty */
            {
                vParent.JoinHint = JoinHintHelper.Instance.ParseOption(tId1);
            }
        )
    ;

joinParenthesis returns [JoinParenthesisTableReference vResult = this.FragmentFactory.CreateFragment<JoinParenthesisTableReference>()]
{
    TableReference vSelectTableReference;
}
    :   tLParen:LeftParenthesis vSelectTableReference=joinTableReference tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Join = vSelectTableReference;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

builtInFunctionTableReference returns [BuiltInFunctionTableReference vResult = this.FragmentFactory.CreateFragment<BuiltInFunctionTableReference>()]
{
    Identifier vIdentifier;
    ScalarExpression vExpression;
}
    :
        tDoubleColon:DoubleColon
        {
            UpdateTokenInfo(vResult,tDoubleColon);
        }
        vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
        LeftParenthesis
        ( 
            vExpression=possibleNegativeConstantWithDefault
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            (
                Comma vExpression=possibleNegativeConstantWithDefault
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
                }
            )*
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAliasOpt[vResult]
    ;

variableTableReference returns [VariableTableReference vResult = this.FragmentFactory.CreateFragment<VariableTableReference>()]
{
    VariableReference vLiteral;
}
    :   vLiteral=variable
        {
            vResult.Variable = vLiteral;
        }
        simpleTableReferenceAliasOpt[vResult]
    ;

raiseErrorStatements returns [TSqlStatement vResult = null]
    :
        tRaiserror:Raiserror
        (
            vResult=raiseErrorStatement
        |
            vResult=raiseErrorLegacyStatement
        )
        {
            UpdateTokenInfo(vResult,tRaiserror);
        }
    ;

raiseErrorStatement returns [RaiseErrorStatement vResult = this.FragmentFactory.CreateFragment<RaiseErrorStatement>()]
{
    ScalarExpression vExpression;
}
    :
        tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
        }
        vExpression=signedIntegerOrStringOrVariable
        {
            vResult.FirstParameter = vExpression;
        }
        Comma vExpression=signedIntegerOrVariable
        {
            vResult.SecondParameter = vExpression;
        }
        Comma vExpression=signedIntegerOrVariable
        {
            vResult.ThirdParameter = vExpression;
        }
        (
            Comma vExpression=possibleNegativeConstant
            {
                AddAndUpdateTokenInfo(vResult, vResult.OptionalParameters, vExpression);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            // The option repeating is not an error condition, we can just bitwise or.
            With tOption1:Identifier
            {
                UpdateTokenInfo(vResult,tOption1);
                vResult.RaiseErrorOptions |= RaiseErrorOptionsHelper.Instance.ParseOption(tOption1);
            }
            (
                Comma tOption2:Identifier
                {
                    UpdateTokenInfo(vResult,tOption2);
                    vResult.RaiseErrorOptions |= RaiseErrorOptionsHelper.Instance.ParseOption(tOption2);
                }
            )*            
        )?
    ;

raiseErrorLegacyStatement returns [RaiseErrorLegacyStatement vResult = this.FragmentFactory.CreateFragment<RaiseErrorLegacyStatement>()]
{
    ScalarExpression vExpression;
    ValueExpression vLiteral;
}
    :
        vExpression=signedIntegerOrVariable
        {
            vResult.FirstParameter = vExpression;
        }
        vLiteral=stringOrVariable
        {
            vResult.SecondParameter = vLiteral;
        }
    ;

deleteStatement returns [DeleteStatement vResult = this.FragmentFactory.CreateFragment<DeleteStatement>()]
{
    DeleteSpecification vDeleteSpecification;
}
    :
        vDeleteSpecification = deleteSpecification
        {
            vResult.DeleteSpecification = vDeleteSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

deleteSpecification returns [DeleteSpecification vResult = this.FragmentFactory.CreateFragment<DeleteSpecification>()]
{        
    TableReference vDmlTarget;
    FromClause vFromClause;
    WhereClause vWhereClause;
}
    :   tDelete:Delete
        {
            UpdateTokenInfo(vResult,tDelete);
        }
        (
            From
        )?
        vDmlTarget=dmlTarget
        {
            vResult.Target = vDmlTarget;
        }
        vFromClause = fromClauseOpt
        {
            vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=dmlWhereClause
            {
                vResult.WhereClause = vWhereClause;
            }
        )?
    ;

insertStatement returns [InsertStatement vResult = this.FragmentFactory.CreateFragment<InsertStatement>()]
{
    InsertSpecification vInsertSpecification;
}
    :
        vInsertSpecification = insertSpecification
        {
            vResult.InsertSpecification = vInsertSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

insertSpecification returns [InsertSpecification vResult = this.FragmentFactory.CreateFragment<InsertSpecification>()]
{
    TableReference vDmlTarget;
    InsertSource vTSqlFragment = null;
    ColumnReferenceExpression vColumn;
}
    :   tInsert:Insert
        {
            UpdateTokenInfo(vResult,tInsert);
        }
        (
            Into
            {
                vResult.InsertOption = InsertOption.Into;
            }
        |
            Over
            {
                vResult.InsertOption = InsertOption.Over;
            }
        )?
        vDmlTarget=dmlTarget
        {
            vResult.Target = vDmlTarget;
        }
        (
            (LeftParenthesis (Dot | Identifier | QuotedIdentifier))=>  // Linear approximation conflicts with ( select ...)
            tLParen:LeftParenthesis vColumn=insertColumn
            {
                UpdateTokenInfo(vResult,tLParen);
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
            (
                Comma vColumn=insertColumn
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
        (
            vTSqlFragment=valuesInsertSource
        |
            vTSqlFragment=executeInsertSource
        |
            vTSqlFragment=selectInsertSource
        )
        {
            vResult.InsertSource = vTSqlFragment;
        }
    ;

updateStatement returns [UpdateStatement vResult = this.FragmentFactory.CreateFragment<UpdateStatement>()]
{
    UpdateSpecification vUpdateSpecification;
}
    :
        vUpdateSpecification = updateSpecification
        {
            vResult.UpdateSpecification = vUpdateSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

updateSpecification returns [UpdateSpecification vResult = this.FragmentFactory.CreateFragment<UpdateSpecification>()]
{
    TableReference vDmlTarget;
    FromClause vFromClause;
    WhereClause vWhereClause;
}
    :   tUpdate:Update
        {
            UpdateTokenInfo(vResult,tUpdate);
        }
        vDmlTarget=dmlTarget
        {
            vResult.Target = vDmlTarget;
        }
        setClausesList[vResult, vResult.SetClauses]
        vFromClause = fromClauseOpt
        {
            vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=dmlWhereClause
            {
                vResult.WhereClause = vWhereClause;
            }
        )?
    ;

setClausesList [TSqlFragment vParent, IList<SetClause> setClauses]
{
    SetClause vSetClause;
}
    :   tSet:Set vSetClause=setClause
        {
            UpdateTokenInfo(vParent,tSet);
            AddAndUpdateTokenInfo(vParent, setClauses, vSetClause);
        }
        (
            Comma vSetClause=setClause
            {
                AddAndUpdateTokenInfo(vParent, setClauses, vSetClause);
            }
        )*
    ;

setClause returns [AssignmentSetClause vResult = FragmentFactory.CreateFragment<AssignmentSetClause>()]
{
    VariableReference vLiteral;
    ScalarExpression vExpression;
}
    :   vLiteral=variable EqualsSign
        {
            vResult.Variable = vLiteral;
        }
        (
            (identifierList[-1] EqualsSign)=>
            setClauseSubItem[vResult]
        |
            vExpression=expression
            {
                vResult.NewValue = vExpression;
            }
        )
    |
        setClauseSubItem[vResult]
   ;

setClauseSubItem[AssignmentSetClause vParent]
{
    MultiPartIdentifier vMultiPartIdentifier;
    ScalarExpression vExpression;
}
    :   vMultiPartIdentifier=multiPartIdentifier[-1]
        {
            CreateSetClauseColumn(vParent, vMultiPartIdentifier);
        }      
        EqualsSign vExpression=expressionWithDefault
        {
            vParent.NewValue = vExpression;
        }
    ;

executeInsertSource returns [ExecuteInsertSource vResult = this.FragmentFactory.CreateFragment<ExecuteInsertSource>()]
{
    ExecuteSpecification vExecuteSpecification;
}
    :
        vExecuteSpecification = executeSpecification
        {
            vResult.Execute = vExecuteSpecification;
        }
    ;

selectInsertSource returns [SelectInsertSource vResult = this.FragmentFactory.CreateFragment<SelectInsertSource>()]
{
    QueryExpression vQueryExpression;
    OrderByClause vOrderByClause;
}
    :
        vQueryExpression = queryExpression[null]
        (
            vOrderByClause=orderByClause
            {
                vQueryExpression.OrderByClause = vOrderByClause;
            }
        )?
        {
            vResult.Select = vQueryExpression;
        }
    ;

valuesInsertSource returns [ValuesInsertSource vResult = this.FragmentFactory.CreateFragment<ValuesInsertSource>()]
{
    RowValue vRowValue;
}    
    :
        Default tValues1:Values
        {
            UpdateTokenInfo(vResult,tValues1);
            vResult.IsDefaultValues = true;
        }
    |
        tValues2:Values vRowValue = rowValueExpressionWithDefault
        {
            UpdateTokenInfo(vResult,tValues2);
            AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
        }
    ;

rowValueExpressionWithDefault returns [RowValue vResult = FragmentFactory.CreateFragment<RowValue>()]
    :   
        tLParen:LeftParenthesis 
        {
            UpdateTokenInfo(vResult,tLParen);
        }
        expressionWithDefaultList[vResult, vResult.ColumnValues] 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

insertColumn returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :   vMultiPartIdentifier=multiPartIdentifier[-1]
        {
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;

dmlTarget returns [TableReference vResult = null]
    :
          vResult=schemaObjectDmlTarget
        | vResult=openRowset
        | vResult=variableDmlTarget
    ;
        
variableDmlTarget returns [VariableTableReference vResult = FragmentFactory.CreateFragment<VariableTableReference>()]
{
    VariableReference vLiteral;
}
    :
        vLiteral=variable
        {
            vResult.Variable = vLiteral;
        }
    ;

schemaObjectDmlTarget returns [TableReferenceWithAlias vResult = null]
    :
    (schemaObjectFourPartName LeftParenthesis (possibleNegativeConstantWithDefault|RightParenthesis))=> // necessary because select statement can start with LeftParenthesis
            vResult=schemaObjectFunctionDmlTarget
        |   vResult=schemaObjectTableDmlTarget
    ;

schemaObjectFunctionDmlTarget returns [SchemaObjectFunctionTableReference vResult = FragmentFactory.CreateFragment<SchemaObjectFunctionTableReference>()]
{
    SchemaObjectName vSchemaObjectName;    
}
    :
    vSchemaObjectName = schemaObjectFourPartName
    {            
        vResult.SchemaObject = vSchemaObjectName;            
    } 
    parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
    ;


schemaObjectTableDmlTarget returns [NamedTableReference vResult = FragmentFactory.CreateFragment<NamedTableReference>()]
{
    SchemaObjectName vSchemaObjectName;
}   
    :   vSchemaObjectName = schemaObjectFourPartName
        {            
            vResult.SchemaObject = vSchemaObjectName;            
        } 
        (
            With tableHints[vResult, vResult.TableHints, false]
        )?
    ;

schemaObjectOrFunctionTableReference returns [TableReference vResult]
{
    SchemaObjectName vSchemaObjectName;
}
    : 
        vSchemaObjectName=schemaObjectFourPartName
        (
            {IsTableReference(true)}?
            vResult=schemaObjectTableReference[vSchemaObjectName]
            | vResult=schemaObjectFunctionTableReference[vSchemaObjectName]
        )
    ;

schemaObjectFunctionTableReference[SchemaObjectName vSchemaObjectName] returns [SchemaObjectFunctionTableReference vResult = this.FragmentFactory.CreateFragment<SchemaObjectFunctionTableReference>()]
{
    vResult.SchemaObject = vSchemaObjectName;
}
    :    (
            options {greedy=true;} :     
            parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
            simpleTableReferenceAliasOpt[vResult]
            (
                options {greedy=true;} :
                columnNameList[vResult, vResult.Columns]
            )?
        )
    ;

schemaObjectTableReference[SchemaObjectName vSchemaObjectName] returns [NamedTableReference vResult = this.FragmentFactory.CreateFragment<NamedTableReference>()]
{
    vResult.SchemaObject = vSchemaObjectName;  
    IndexTableHint vIndexTableHint;
	bool withSpecified = false;
}   
    :
        (
            options {greedy=true;} : 
            nonParameterTableHints[vResult, vResult.TableHints, ref withSpecified]
            (
                simpleTableReferenceAlias[vResult]
                {
                    if (vResult.Alias != null && withSpecified)
                    {
                        ThrowIncorrectSyntaxErrorException(vResult.Alias);
                    }
                }
            )?
        | 
            simpleTableReferenceAlias[vResult]
            (   
                (LeftParenthesis integer)=> // necessary because select statement can start with LeftParenthesis
                vIndexTableHint=oldForceIndex
                {
                    AddAndUpdateTokenInfo(vResult, vResult.TableHints, vIndexTableHint);
                }
            |   (With | HoldLock | LeftParenthesis (HoldLock|Index|identifier))=> // necessary because select statement can start with LeftParenthesis
                nonParameterTableHints[vResult, vResult.TableHints, ref withSpecified]
            )?
        )?
    ;

simpleTableReferenceAlias[TableReferenceWithAlias vParent]
{
    Identifier vIdentifier;
}
    :   (As)? vIdentifier = identifier
        {
            vParent.Alias = vIdentifier;            
        }
    ;

simpleTableReferenceAliasOpt[TableReferenceWithAlias vParent]
    :
        ( 
            options {greedy=true;} : 
            simpleTableReferenceAlias[vParent] 
        )?
    ;

fulltextTableReference returns [FullTextTableReference vResult = FragmentFactory.CreateFragment<FullTextTableReference>()]
{
    SchemaObjectName vTableName;
    ValueExpression vSearchCondition;
}
    :    fullTextTable[vResult] LeftParenthesis vTableName = schemaObjectFourPartName
        {
            vResult.TableName = vTableName;
        }
        Comma fulltextTableColumnList[vResult]
        Comma vSearchCondition = stringOrVariable 
        {
            vResult.SearchCondition = vSearchCondition;
        }
        (fulltextTableOptions[vResult])? 
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAliasOpt[vResult]
    ;

fullTextTable [FullTextTableReference vParent]
    : tContainsTable:ContainsTable
        {
            vParent.FullTextFunctionType = FullTextFunctionType.Contains;
            UpdateTokenInfo(vParent,tContainsTable);
        }
    | tFreetextTable:FreeTextTable
        {
            vParent.FullTextFunctionType = FullTextFunctionType.FreeText;
            UpdateTokenInfo(vParent,tFreetextTable);
        }
    ;
    
fulltextTableColumnList [FullTextTableReference vParent]
{
    ColumnReferenceExpression vColumn;
}
    :   vColumn = identifierColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
        }
    |   vColumn = starColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
        }
    |   LeftParenthesis vColumn = starColumnReferenceExpression RightParenthesis
        {
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
        }
    |   LeftParenthesis vColumn = identifierColumnReferenceExpression 
        {
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
        }
        (Comma vColumn = identifierColumnReferenceExpression 
            {
                AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
            }
        )* RightParenthesis
    ;

fulltextTableOptions [FullTextTableReference vParent]
{
    ValueExpression vTopN;
    ValueExpression vLanguage;
}
    : Comma vLanguage = languageExpression
        {
            vParent.Language = vLanguage;
        }
        (Comma vTopN = unsignedInteger
            {
                vParent.TopN = vTopN;
            }
        )?
    | Comma vTopN = unsignedInteger
        {
            vParent.TopN = vTopN;
        }
        (Comma vLanguage = languageExpression
            {
                vParent.Language = vLanguage;
            }
        )?
    ;
        
languageExpression returns [ValueExpression vResult = null]
{
    ValueExpression vLiteral;
}
    :
        tLanguage:Identifier
        {
            Match(tLanguage, CodeGenerationSupporter.Language);
        }
        vLiteral=binaryOrIntegerOrStringOrVariable
        {
            vResult = vLiteral;
        }
    ;    

openXmlTableReference returns [OpenXmlTableReference vResult]
    :    tOpenXml:OpenXml LeftParenthesis vResult = openXmlParams tRParen:RightParenthesis openXmlWithClauseOpt[vResult] simpleTableReferenceAliasOpt[vResult]
        {
            UpdateTokenInfo(vResult,tOpenXml);
            UpdateTokenInfo(vResult,tRParen);
        }
// In SQL 2005 parser, there is another clause: _OPENXML '(' column ',' _INTEGER ')'
// There is a comment that it is used to populate the XML indexed view
// Tried using it in SMS - gives error, so, probably internal stuff 
    ;

openXmlWithClauseOpt [OpenXmlTableReference vParent]
{
    SchemaObjectName vTableName;
}
    :    (With) => 
        (
            (With LeftParenthesis openXmlSchemaItemList[vParent] tRParen:RightParenthesis
                {
                    UpdateTokenInfo(vParent,tRParen);
                }
            )
            |
            (With vTableName = schemaObjectThreePartName
                {
                    vParent.TableName = vTableName;
                }
            )
        )
    | /* empty */
    ;
    
openXmlParams returns [OpenXmlTableReference vResult = FragmentFactory.CreateFragment<OpenXmlTableReference>()]
{
    VariableReference vVariable;
    ValueExpression vRowPattern;
    ValueExpression vFlags;
}
    : vVariable = variable Comma vRowPattern = stringOrVariable 
        {
            vResult.Variable = vVariable;
            vResult.RowPattern = vRowPattern;
        }
    (Comma vFlags = unsignedInteger // openxml_flags
        {
            vResult.Flags = vFlags;
        }
    )? 
    ;

openXmlSchemaItemList [OpenXmlTableReference vParent]
{
    SchemaDeclarationItem vItem;
}
    : vItem = openXmlSchemaItem 
        {
            AddAndUpdateTokenInfo(vParent, vParent.SchemaDeclarationItems, vItem);
        }
        (Comma vItem = openXmlSchemaItem
            {
                AddAndUpdateTokenInfo(vParent, vParent.SchemaDeclarationItems, vItem);
            }
        )*
    ;
    
openXmlSchemaItem returns [SchemaDeclarationItem vResult = FragmentFactory.CreateFragment<SchemaDeclarationItem>()]
{
    ValueExpression vMapping;
    ColumnDefinitionBase vColumn;
}
    : vColumn = columnDefinitionBasic 
        {
            vResult.ColumnDefinition = vColumn;
        }
        (vMapping = stringOrVariable
            {
                vResult.Mapping = vMapping;
            }
        )?
    ;
    
openRowset returns [TableReferenceWithAlias vResult]
    :    vResult = openRowsetRowset // alias is handled inside the rule...
    |    vResult = openQueryRowset simpleTableReferenceAliasOpt[vResult]
    |    vResult = adhocRowset simpleTableReferenceAliasOpt[vResult]

// TODO, olegr: find out, if we need this implemented (and when)    
//    looks like ODBC stuff, so, skipping for now...
//    |    vResult = binaryRowset
    ;
    
openRowsetRowset returns [TableReferenceWithAlias vResult]
    : tOpenRowset:OpenRowSet LeftParenthesis 
        ( vResult = openRowsetParams
        | vResult = internalOpenRowsetArgs
        )
        {
            UpdateTokenInfo(vResult,tOpenRowset);
        }
// There are few internal and undocumented rowsets in SQL Server: 
//        openrowsetbegin '(' _TABLE object ')' correlation_name
//        openrowsetbegin '(' _TABLE object ',' expression_list ')' correlation_name
    ;

internalOpenRowsetArgs returns [InternalOpenRowset vResult = FragmentFactory.CreateFragment<InternalOpenRowset>()]
{
    ScalarExpression vVarArg;
    Identifier vIdentifier;
}
    : vIdentifier = identifier
        {
            vResult.Identifier = vIdentifier;
        }
        (Comma vVarArg = possibleNegativeConstant
            {
                AddAndUpdateTokenInfo(vResult, vResult.VarArgs, vVarArg);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAliasOpt[vResult]
    ;
    
openQueryRowset returns [OpenQueryTableReference vResult = FragmentFactory.CreateFragment<OpenQueryTableReference>()]
{
    Identifier vLinkedServer;
    StringLiteral vQuery;
}
    :    tOpenQuery:OpenQuery LeftParenthesis vLinkedServer = identifier Comma vQuery = stringLiteral tRParen:RightParenthesis    
        {
            UpdateTokenInfo(vResult,tOpenQuery);
            vResult.LinkedServer = vLinkedServer;
            vResult.Query = vQuery;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;
    
adhocRowset returns [AdHocTableReference vResult = FragmentFactory.CreateFragment<AdHocTableReference>()]
{
     AdHocDataSource vDataSource;
     SchemaObjectNameOrValueExpression vObject;
}
    : vDataSource = adhocDataSource Dot 
        {
            vResult.DataSource = vDataSource;
        }
        vObject = objectOrString
        {
            // TODO, olegr: check, that we have exactly 3 part name (see corresponding rule in SQL parser)
            vResult.Object=vObject;
        }
    ;
    
unsignedInteger returns [ValueExpression vResult]
    :    vResult = integer
    |    vResult = variable
    ;

openRowsetParams returns [OpenRowsetTableReference vResult = FragmentFactory.CreateFragment<OpenRowsetTableReference>()]
{
    SchemaObjectName vSchemaObjectName;
    StringLiteral vProviderName, vProviderString, vDataSource, vUserId, vPassword, vQuery;
}
    : vProviderName = stringLiteral Comma
        {
            vResult.ProviderName = vProviderName;
        }
        (
            (vDataSource = stringLiteral 
                {
                    vResult.DataSource = vDataSource;
                }
                Semicolon 
                (vUserId = stringLiteral
                    {
                        vResult.UserId = vUserId;
                    }
                )? 
                Semicolon 
                (vPassword = stringLiteral
                    {
                        vResult.Password = vPassword;
                    }
                )?
            )
            |
            (vProviderString = stringLiteral
                {
                    vResult.ProviderString = vProviderString;
                }
            )
        )
        Comma 
        (vSchemaObjectName = schemaObjectThreePartName 
            {
                vResult.Object = vSchemaObjectName;
            }
        | vQuery = stringLiteral
            {
                vResult.Query = vQuery;
            }
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAliasOpt[vResult]        
    ;

oldForceIndex returns [IndexTableHint vResult = FragmentFactory.CreateFragment<IndexTableHint>()]
{
    Literal vLiteral;
}
    :   tLParen:LeftParenthesis vLiteral=integer tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.HintKind=TableHintKind.Index;
            AddAndUpdateTokenInfo(vResult, vResult.IndexValues, IdentifierOrValueExpression(vLiteral));
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

nonParameterTableHints [TSqlFragment vParent, IList<TableHint> hints, ref bool withSpecified]
{
    IndexTableHint vIndexHint;
}
    : 
        // can not be optional because of ambiguity with the ( x )
        // between oldForceIndex and function call.
        tHoldLock:HoldLock
        {
            TableHint vHoldLock = FragmentFactory.CreateFragment<TableHint>();
            UpdateTokenInfo(vHoldLock,tHoldLock);
            vHoldLock.HintKind = TableHintKind.HoldLock;
            AddAndUpdateTokenInfo(vParent, hints, vHoldLock);
        }
        (
            (LeftParenthesis integer)=> // necessary because select statement can start with LeftParenthesis
            vIndexHint = oldForceIndex
            {
                AddAndUpdateTokenInfo(vParent, hints, vIndexHint);
            }
        |   
            (LeftParenthesis (identifier|Index))=> // necessary because select statement can start with LeftParenthesis
            simpleTableHints[vParent, hints, ref withSpecified]
        )?
    |   simpleTableHints[vParent, hints, ref withSpecified]
    ;

simpleTableHints[TSqlFragment vParent, IList<TableHint> hints, ref bool withSpecified]
{
    IndexTableHint vOldForceIndex;
}
    :   tWith:With 
        {
            UpdateTokenInfo(vParent,tWith);
			withSpecified = true;
        }
        (
            vOldForceIndex = oldForceIndex
            {
                AddAndUpdateTokenInfo(vParent, hints, vOldForceIndex);
            }
        |
            tableHints[vParent, hints, true]
        )
    |   tableHints[vParent, hints, true]
    ;

tableHints[TSqlFragment vParent, IList<TableHint> hints, bool indexHintAllowed]
{
    TableHint vHint;
}
    :   tLParen:LeftParenthesis vHint = tableHint[indexHintAllowed]
        {
            UpdateTokenInfo(vParent,tLParen);
            AddAndUpdateTokenInfo(vParent, hints, vHint);
        }
        (
            ( Comma )? vHint = tableHint[indexHintAllowed]
            {
                AddAndUpdateTokenInfo(vParent, hints, vHint);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

tableHint [bool indexHintAllowed] returns [TableHint vResult]
    :
        vResult = simpleTableHint
    |
        vResult = indexTableHint[indexHintAllowed]
    ;
    
simpleTableHint returns [TableHint vResult = FragmentFactory.CreateFragment<TableHint>()]
    :
        tHoldLock:HoldLock
        {
            vResult.HintKind = TableHintKind.HoldLock;
            UpdateTokenInfo(vResult,tHoldLock);
        }
    |   tId:Identifier
        {
            vResult.HintKind = TableHintOptionsHelper.Instance.ParseOption(tId, SqlVersionFlags.TSql80);
            UpdateTokenInfo(vResult,tId);
        }
    ;
    
indexTableHint [bool indexHintAllowed] returns [IndexTableHint vResult = FragmentFactory.CreateFragment<IndexTableHint>()]
{
    IdentifierOrValueExpression vIndexValue;
}
    :   tIndex:Index
        {
            if (!indexHintAllowed)
                ThrowParseErrorException("SQL46074", tIndex, TSqlParserResource.SQL46074Message);
                
            UpdateTokenInfo(vResult,tIndex);
            vResult.HintKind=TableHintKind.Index;
        }
        (
            EqualsSign vIndexValue = identifierOrInteger
            {
                AddAndUpdateTokenInfo(vResult, vResult.IndexValues, vIndexValue);
            }
        |   
            LeftParenthesis vIndexValue = identifierOrInteger
            {
                AddAndUpdateTokenInfo(vResult, vResult.IndexValues, vIndexValue);
            }
            (
                Comma vIndexValue = identifierOrInteger
                {
                    AddAndUpdateTokenInfo(vResult, vResult.IndexValues, vIndexValue);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )
    ;

singleOldStyleTableHint [TSqlFragment vParent, IList<TableHint> hints]
{
    TableHint vHint;
}
    :   tLParen:LeftParenthesis
	    {
		    UpdateTokenInfo(vParent, tLParen);
		}
		vHint=tableHint[true] tRParen:RightParenthesis
        {
            AddAndUpdateTokenInfo(vParent, hints, vHint);
			UpdateTokenInfo(vParent, tRParen);
        }
    ;

whereClause returns [WhereClause vResult = FragmentFactory.CreateFragment<WhereClause>()]
{
    BooleanExpression vExpression;
}
    :    tWhere:Where vExpression=booleanExpression
        {
            UpdateTokenInfo(vResult,tWhere);
            vResult.SearchCondition = vExpression;
        }
    ;
    
dmlWhereClause returns [WhereClause vResult]
    :
        vResult = whereClause
    |
        vResult = whereCurrentOfCursorClause
    ;
    
whereCurrentOfCursorClause returns [WhereClause vResult = FragmentFactory.CreateFragment<WhereClause>()]
{
    CursorId vCursorId;
}
    :   tWhere:Where Current Of vCursorId = cursorId
        {
            UpdateTokenInfo(vResult,tWhere);
            vResult.Cursor = vCursorId;
        }
    ;

groupByClause returns [GroupByClause vResult = this.FragmentFactory.CreateFragment<GroupByClause>()]
{
    ExpressionGroupingSpecification vItem;
}
   :    tGroup:Group By
        {
            UpdateTokenInfo(vResult,tGroup);
        }
        (
            All
            {
                vResult.All = true;
            }
        )?
        vItem = simpleGroupByItem
        {
            AddAndUpdateTokenInfo(vResult, vResult.GroupingSpecifications, vItem);
        }
        (Comma vItem = simpleGroupByItem
            {
                AddAndUpdateTokenInfo(vResult, vResult.GroupingSpecifications, vItem);
            }
        )*
        (   
            // TODO, sacaglar: investigate why we need this syntactic predicate
            (With Identifier)=>
            With tOption:Identifier
            {
                if (vResult.All)
                    ThrowParseErrorException("SQL46084", tOption, TSqlParserResource.SQL46084Message);
                UpdateTokenInfo(vResult,tOption);
                vResult.GroupByOption = GroupByOptionHelper.Instance.ParseOption(tOption);
            }
        )?
    ;
    
simpleGroupByItem returns [ExpressionGroupingSpecification vResult = FragmentFactory.CreateFragment<ExpressionGroupingSpecification>()]
{
    ScalarExpression vExpression;
}
    :   vExpression = expression
        {
            vResult.Expression = vExpression;
        }
    ;    

havingClause returns [HavingClause vResult = this.FragmentFactory.CreateFragment<HavingClause>()]
{
    BooleanExpression vExpression;
}
    :   tHaving:Having vExpression=booleanExpression
        {
            UpdateTokenInfo(vResult,tHaving);
            vResult.SearchCondition = vExpression;
        }
    ;

orderByClause returns [OrderByClause vResult = this.FragmentFactory.CreateFragment<OrderByClause>()]
{
    ExpressionWithSortOrder vExpressionWithSortOrder;
}
    :
        tOrder:Order By
        {
            UpdateTokenInfo(vResult,tOrder);
        }
        vExpressionWithSortOrder=expressionWithSortOrder
        {
            AddAndUpdateTokenInfo(vResult, vResult.OrderByElements, vExpressionWithSortOrder);
        }
        (
            Comma vExpressionWithSortOrder=expressionWithSortOrder
            {
                AddAndUpdateTokenInfo(vResult, vResult.OrderByElements, vExpressionWithSortOrder);
            }
        )*
    ;

computeClause returns [ComputeClause vResult = this.FragmentFactory.CreateFragment<ComputeClause>()]
{
    ComputeFunction vComputeFunction;
}
    :   tCompute:Compute vComputeFunction=computeFunction
        {
            UpdateTokenInfo(vResult,tCompute);
            AddAndUpdateTokenInfo(vResult, vResult.ComputeFunctions, vComputeFunction);
        }
        (
            Comma vComputeFunction=computeFunction
            {
                AddAndUpdateTokenInfo(vResult, vResult.ComputeFunctions, vComputeFunction);
            }

        )*
        (
            By expressionList[vResult, vResult.ByExpressions]
        )?
    ;

computeFunction returns [ComputeFunction vResult = this.FragmentFactory.CreateFragment<ComputeFunction>()]
{
    ScalarExpression vExpression;
}
    :   tId:Identifier
        {           
            vResult.ComputeFunctionType = ComputeFunctionTypeHelper.Instance.ParseOption(tId);
        }
        LeftParenthesis
        vExpression=expression
        {
            vResult.Expression = vExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

expressionWithSortOrder returns [ExpressionWithSortOrder vResult = this.FragmentFactory.CreateFragment<ExpressionWithSortOrder>()]
{
    ScalarExpression vExpression;
    SortOrder vOrder;
}
    :
        vExpression=expression
        {
            vResult.Expression = vExpression;
        }
        (vOrder = orderByOption[vResult]
            {
                vResult.SortOrder = vOrder;
            }
        )?
    ;

forClause returns [ForClause vResult = null]
    : tFor:For 
        (vResult = browseForClause
            { 
                UpdateTokenInfo(vResult,tFor);
            }
        | (Read tOnly:Identifier
            {
                Match(tOnly, CodeGenerationSupporter.Only);
                vResult = FragmentFactory.CreateFragment<ReadOnlyForClause>();
                UpdateTokenInfo(vResult,tOnly);
            }
          )
        | vResult = xmlForClause
            {
                UpdateTokenInfo(vResult,tFor);
            }
        | vResult = updateForClause
            {
                UpdateTokenInfo(vResult,tFor);
            }
        )
    ;

xmlForClause returns [XmlForClause vResult = FragmentFactory.CreateFragment<XmlForClause>()]
{
    XmlForClauseOption vOption;
    XmlForClauseOptions encountered = XmlForClauseOptions.None;
}
    : tXml:Identifier vOption = xmlForClauseMode
        {
            Match(tXml, CodeGenerationSupporter.Xml);
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }            
        (Comma vOption = xmlParam[encountered]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                encountered |= vOption.OptionKind;
            }
        )*
    ;
    
xmlForClauseMode returns [XmlForClauseOption vResult = FragmentFactory.CreateFragment<XmlForClauseOption>()]
{
    Literal vElementName;
}
    : tMode:Identifier 
        {
            vResult.OptionKind = XmlForClauseModeHelper.Instance.ParseOption(tMode);
            UpdateTokenInfo(vResult,tMode);
        }
        (options { greedy = true; } : // Conflicts with Select, which can also start from '('
            tLParen:LeftParenthesis vElementName = stringLiteral tRParen:RightParenthesis
            {
                if (vResult.OptionKind == XmlForClauseOptions.Explicit || 
                    vResult.OptionKind == XmlForClauseOptions.Auto)
                    throw GetUnexpectedTokenErrorException(tLParen);
                
                vResult.Value = vElementName;                    
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;

xmlParam [XmlForClauseOptions encountered] returns [XmlForClauseOption vResult = FragmentFactory.CreateFragment<XmlForClauseOption>()]
{
    Literal vValue;
}
    : tOption:Identifier
        (    options { greedy = true; } : // Conflicts with Select, which can also start from '('. 
            tLParen:LeftParenthesis vValue = stringLiteral tRParen:RightParenthesis
            {
                if (TryMatch(tOption,CodeGenerationSupporter.XmlSchema) || 
                    TryMatch(tOption,CodeGenerationSupporter.Root))
                {
                    vResult.Value = vValue;
                    UpdateTokenInfo(vResult,tRParen);
                }
                else
                    throw GetUnexpectedTokenErrorException(tLParen);
            }
        |
            tOption2:Identifier
            {
                if (TryMatch(tOption,CodeGenerationSupporter.Binary))
                {
                    Match(tOption2,CodeGenerationSupporter.Base64);
                    vResult.OptionKind = XmlForClauseOptions.BinaryBase64;
                }
                else 
                {
                    Match(tOption,CodeGenerationSupporter.Elements);
                    if (TryMatch(tOption2,CodeGenerationSupporter.XsiNil))
                        vResult.OptionKind = XmlForClauseOptions.ElementsXsiNil;
                    else
                    {
                        Match(tOption2,CodeGenerationSupporter.Absent);
                        vResult.OptionKind = XmlForClauseOptions.ElementsAbsent;
                    }
                }
                UpdateTokenInfo(vResult,tOption2);
            }
        )?
        {
             // Means, that there was no tail, and we should parse tOption...
            if (vResult.OptionKind == XmlForClauseOptions.None)
            {
                vResult.OptionKind = XmlForClauseOptionsHelper.Instance.ParseOption(tOption);
                UpdateTokenInfo(vResult,tOption);
            }
            CheckXmlForClauseOptionDuplication(encountered,vResult.OptionKind,tOption);
        }
    ;    
    
updateForClause returns [UpdateForClause vResult = FragmentFactory.CreateFragment<UpdateForClause>()]
{
    ColumnReferenceExpression vColumn;
}
    :  tUpdate:Update 
        {
            UpdateTokenInfo(vResult,tUpdate);
        }
        (Of vColumn = column 
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
            (Comma vColumn = column
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
                }
            )*
        )?
    ;
    
optimizerHints [TSqlFragment vParent, IList<OptimizerHint> hintsCollection]
{
    OptimizerHint vHint;
}
    : tOption:Option LeftParenthesis vHint = hint
        {
            UpdateTokenInfo(vParent,tOption);
            AddAndUpdateTokenInfo(vParent, hintsCollection, vHint);
        }
        (Comma vHint = hint
            {
                AddAndUpdateTokenInfo(vParent, hintsCollection, vHint);
            }
        )* 
        tLastParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tLastParen);
        }
    ;
    
hint returns [OptimizerHint vResult]
    : vResult = simpleOptimizerHint
    | vResult = literalOptimizerHint
    ;
    
simpleOptimizerHint returns [OptimizerHint vResult = FragmentFactory.CreateFragment<OptimizerHint>()]
    : tMergeHashLoop:Identifier Join
        {
            vResult.HintKind = ParseJoinOptimizerHint(tMergeHashLoop);
        }
    | tConcatHashMergeKeep:Identifier Union 
        {
            vResult.HintKind = ParseUnionOptimizerHint(tConcatHashMergeKeep);
        }
    | tForce:Identifier Order
        {
            Match(tForce, CodeGenerationSupporter.Force);
            vResult.HintKind = OptimizerHintKind.ForceOrder;
        }
    | tHash:Identifier Group
        {
            Match(tHash, CodeGenerationSupporter.Hash);
            vResult.HintKind = OptimizerHintKind.HashGroup;
        }
    | tOrder:Order Group
        {
            vResult.HintKind = OptimizerHintKind.OrderGroup;
        }
    | tPlan:Identifier Plan
        {
            vResult.HintKind = PlanOptimizerHintHelper.Instance.ParseOption(tPlan, SqlVersionFlags.TSql80);
        }
    | tFirstWord:Identifier tSecondWord:Identifier
        {
            if (TryMatch(tFirstWord, CodeGenerationSupporter.Expand))
            {
                Match(tSecondWord, CodeGenerationSupporter.Views);
                vResult.HintKind = OptimizerHintKind.ExpandViews;
            }
            else
            {
                Match(tFirstWord, CodeGenerationSupporter.Bypass);
                Match(tSecondWord, CodeGenerationSupporter.OptimizerQueue);
                vResult.HintKind = OptimizerHintKind.BypassOptimizerQueue;
            }        
        }
    ;

literalOptimizerHint returns [LiteralOptimizerHint vResult = FragmentFactory.CreateFragment<LiteralOptimizerHint>()]
{
    Literal vValue;
}    
    : tIntegerHint:Identifier vValue = integer
        {
            vResult.HintKind = IntegerOptimizerHintHelper.Instance.ParseOption(tIntegerHint, SqlVersionFlags.TSql80);
            vResult.Value = vValue;
        }
    ;
    
createRuleStatement returns [CreateRuleStatement vResult = this.FragmentFactory.CreateFragment<CreateRuleStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    BooleanExpression vExpression;
}
    :
        tCreate:Create Rule 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        vSchemaObjectName=schemaObjectThreePartName
        {
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Rule);
            vResult.Name = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        As vExpression=booleanExpression
        {
            vResult.Expression = vExpression;
        }
    ;
        
createDefaultStatement returns [CreateDefaultStatement vResult = this.FragmentFactory.CreateFragment<CreateDefaultStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    ScalarExpression vExpression;
}
    :
        tCreate:Create Default
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        vSchemaObjectName=schemaObjectThreePartName
        {
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Default);
            vResult.Name = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        As vExpression=expression
        {
            vResult.Expression = vExpression;
        }
    ;

createViewStatement returns [CreateViewStatement vResult = this.FragmentFactory.CreateFragment<CreateViewStatement>()]
    : tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        viewStatementBody[vResult]
    ;

alterViewStatement returns [AlterViewStatement vResult = this.FragmentFactory.CreateFragment<AlterViewStatement>()]
    : tAlter:Alter 
        {
            UpdateTokenInfo(vResult,tAlter);
        }
        viewStatementBody[vResult]
    ;

viewStatementBody[ViewStatementBody vResult]
{
    SchemaObjectName vSchemaObjectName;
    SelectStatement vSelectStatement;
    ViewOption vViewOption;
    long encounteredOptions = 0;
}
    :   View vSchemaObjectName=schemaObjectTwoPartName
        {
            vResult.SchemaObjectName = vSchemaObjectName;
            CheckForTemporaryView(vSchemaObjectName);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (columnNameList[vResult, vResult.Columns])?
        ( 
            With vViewOption=viewOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vViewOption.OptionKind, vViewOption);
                AddAndUpdateTokenInfo(vResult, vResult.ViewOptions, vViewOption);
            }            
            (
                Comma vViewOption=viewOption
                {
                    CheckOptionDuplication(ref encounteredOptions, (int)vViewOption.OptionKind, vViewOption);
                    AddAndUpdateTokenInfo(vResult, vResult.ViewOptions, vViewOption);
                }
            )*
        )?
        tAs:As
        {
            UpdateTokenInfo(vResult,tAs);
        }
        vSelectStatement = subqueryExpressionAsStatement
        {
            vResult.SelectStatement = vSelectStatement;
        }
        (With Check 
            tOption:Option
            {
                UpdateTokenInfo(vResult,tOption);
                vResult.WithCheckOption = true;
            }
        )?
    ;

viewOption returns [ViewOption vResult = this.FragmentFactory.CreateFragment<ViewOption>()]
    :
        tOption:Identifier
        {
            vResult.OptionKind = ViewOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    ;
    
columnNameList [TSqlFragment vParent, IList<Identifier> columnNames]
{
    Identifier vIdentifier;
}
    :   LeftParenthesis vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vParent, columnNames, vIdentifier);
        }
        (Comma vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vParent, columnNames, vIdentifier);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

identifierColumnList [TSqlFragment vParent, IList<ColumnReferenceExpression> columns]
{
    ColumnReferenceExpression vColumn;
}
    : LeftParenthesis vColumn = identifierColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, columns, vColumn);
        }
        (Comma vColumn = identifierColumnReferenceExpression
            {
                AddAndUpdateTokenInfo(vParent, columns, vColumn);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

triggerOption returns [TriggerOption vResult = this.FragmentFactory.CreateFragment<TriggerOption>()]
    :
        tOption:Identifier
        {
            vResult.OptionKind = TriggerOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    ;

procedureOptions [ProcedureStatementBody vParent]
{
    ProcedureOption vOption;
    long encounteredOptions=0;
}
    : With vOption=procedureOption
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (
            Comma vOption=procedureOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
    ;

procedureOption returns [ProcedureOption vResult=FragmentFactory.CreateFragment<ProcedureOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind=ProcedureOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    ;
        
alterProcedureStatement returns [AlterProcedureStatement vResult = this.FragmentFactory.CreateFragment<AlterProcedureStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : tAlter:Alter 
        {
            UpdateTokenInfo(vResult,tAlter);
        }
        procedureStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

createProcedureStatement returns [CreateProcedureStatement vResult = this.FragmentFactory.CreateFragment<CreateProcedureStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        procedureStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

procedureStatementBody[ProcedureStatementBody vResult, out bool vParseErrorOccurred]
{
    ProcedureReference vProcRef;
    StatementList vStatementList;
    vParseErrorOccurred = false;
}
    : (Proc | Procedure)
        vProcRef = procedureReference
        {
            CheckTwoPartNameForSchemaObjectName(vProcRef.Name, CodeGenerationSupporter.Procedure);
            vResult.ProcedureReference = vProcRef;
        }
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }
        procedureParameterListOptionalParen[vResult]

        ( procedureOptions[vResult] )?
        ( For Replication
            {
                vResult.IsForReplication = true;
            }
        )?
        tAs:As
        {
            UpdateTokenInfo(vResult,tAs);
        }       
        (
            vStatementList = statementList[ref vParseErrorOccurred]
            {
                vResult.StatementList = vStatementList;
            }
        )?
    ;
    // The following is necessary due to the weird syntax with ; integer
    // after proc name.
    exception
    catch [antlr.NoViableAltException]
    {
        if (PhaseOne == true && vResult != null && vResult.ProcedureReference != null && vResult.ProcedureReference.Name != null)
            {
                ThrowPartialAstIfPhaseOne(vResult);
            }
        else
            {
                throw;
            }
    }
    
statementList [ref bool vParseErrorOccurred] returns [StatementList vResult = FragmentFactory.CreateFragment<StatementList>()]
{
    TSqlStatement vStatement;
}
    :( 
        vStatement=statementOptSemi 
        {
            if (null != vStatement) // If a parse error happens
                AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
            else 
                vParseErrorOccurred = true;
        }
    )+
    ;

procedureParameterListOptionalParen[ProcedureStatementBodyBase vResult]
    :
        LeftParenthesis 
        procedureParameterList[vResult] 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    | 
        ( 
            procedureParameterList[vResult] 
        )?
    ;

procedureParameterList[ProcedureStatementBodyBase vResult]
{
    ProcedureParameter vParameter;
}
    :
        vParameter=procedureParameter
        {
            AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
        }
        ( Comma vParameter=procedureParameter
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
            }
        )*
    ;

procedureParameter returns[ProcedureParameter vResult = FragmentFactory.CreateFragment<ProcedureParameter>()]
{
    Identifier vIdentifier;
}
    : vIdentifier=identifierVariable (As)?
        {
            vResult.VariableName = vIdentifier;
        }
        (
            cursorProcedureParameter[vResult]
        |
            scalarProcedureParameter[vResult, true]
        )
    ;    
    
cursorProcedureParameter [ProcedureParameter vParent]
{
    DataTypeReference vDataType;
}
    :    vDataType=cursorDataType
        {
            vParent.DataType = vDataType;
        }            
        ( tVarying:Varying
            {
                UpdateTokenInfo(vParent,tVarying);
                vParent.IsVarying = true;
            }
        )
        ( 
            tId2:Identifier 
            {
                Match(tId2,CodeGenerationSupporter.Output,CodeGenerationSupporter.Out);
                vParent.Modifier = ParameterModifier.Output;                                
                UpdateTokenInfo(vParent,tId2);
            }
        )   
    ;    

scalarProcedureParameter[ProcedureParameter vParent, bool outputAllowed]
{
    DataTypeReference vDataType;
    ScalarExpression vDefault;
}
    :    vDataType=scalarDataType
        {
            vParent.DataType = vDataType;
        }            
        (
            EqualsSign 
            (
                vDefault=possibleNegativeConstantOrIdentifierWithDefault
                {
                    vParent.Value = vDefault;
                }
            )
        )?
        ( 
            tId2:Identifier 
            {
                UpdateTokenInfo(vParent,tId2);
                Match(tId2,CodeGenerationSupporter.Output,CodeGenerationSupporter.Out);
                if (outputAllowed)
                    vParent.Modifier = ParameterModifier.Output;
                else
                    ThrowParseErrorException("SQL46039", tId2, TSqlParserResource.SQL46039Message);
            }
        )?
    ;    

possibleNegativeConstantOrIdentifier returns [ScalarExpression vResult]
    : vResult = possibleNegativeConstant
    | vResult = identifierLiteral
    ;

//This represents an Identifier when it is used in place of a Literal
identifierLiteral returns [IdentifierLiteral vResult = FragmentFactory.CreateFragment<IdentifierLiteral>()]
:
        tId:Identifier 
        {
            UpdateTokenInfo(vResult,tId);
            vResult.SetUnquotedIdentifier(tId.getText());
            CheckIdentifierLiteralLength(vResult);
        }
    |
        tId2:QuotedIdentifier 
        {
            UpdateTokenInfo(vResult,tId2);
            vResult.SetIdentifier(tId2.getText());
            CheckIdentifierLiteralLength(vResult);
        }
    ;

possibleNegativeConstantOrIdentifierWithDefault returns [ScalarExpression vResult]
    : vResult = possibleNegativeConstantOrIdentifier
    | vResult = defaultLiteral
    ;    
        
possibleNegativeConstant returns [ScalarExpression vResult]
    :   vResult = literal
    |   vResult = negativeConstant
    ;
    
negativeConstant returns [UnaryExpression vResult = FragmentFactory.CreateFragment<UnaryExpression>()]
{
    Literal vLiteral;
}
    :   tMinus:Minus vLiteral=subroutineParameterLiteral
        {
            UpdateTokenInfo(vResult, tMinus);
            vResult.UnaryExpressionType = UnaryExpressionType.Negative;
            vResult.Expression = vLiteral;
        }
    ;

possibleNegativeConstantWithDefault returns [ScalarExpression vResult = null]
    : 
        vResult=possibleNegativeConstant
    |
        vResult=defaultLiteral
    ;

alterTriggerStatement returns [AlterTriggerStatement vResult = this.FragmentFactory.CreateFragment<AlterTriggerStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : tAlter:Alter 
        {
            UpdateTokenInfo(vResult,tAlter);
        }
        triggerStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

createTriggerStatement returns [CreateTriggerStatement vResult = this.FragmentFactory.CreateFragment<CreateTriggerStatement>()]
{
    bool vParseErrorOccurred = false;
}
    : tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        triggerStatementBody[vResult, out vParseErrorOccurred]
        {
            if (vParseErrorOccurred)
            {
                vResult = null;
            }
        }
    ;

triggerStatementBody[TriggerStatementBody vResult, out bool vParseErrorOccurred]
{
    SchemaObjectName vSchemaObjectName;
    StatementList vStatementList;
    TriggerObject vTriggerObject;
    TriggerOption vTriggerOption;
    vParseErrorOccurred = false;
}
    : Trigger
        vSchemaObjectName=schemaObjectThreePartName
        {
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Trigger);
            vResult.Name = vSchemaObjectName;
        }
        On vTriggerObject = triggerObject
        {
            vResult.TriggerObject = vTriggerObject;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        ( With vTriggerOption=triggerOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vTriggerOption);
            } 
            (
                Comma vTriggerOption=triggerOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vTriggerOption);
                } 
            )*
        )?
        (
            dmlTriggerMidSection[vResult]
        )
        tAs:As
        {
            UpdateTokenInfo(vResult,tAs);
        }
        (vStatementList = statementList[ref vParseErrorOccurred]
            {
                vResult.StatementList = vStatementList;
            }
        )
    ;
    
dmlTriggerMidSection[TriggerStatementBody vParent]
{
    bool ofAppeared = false;
    TriggerAction vAction;
}
    :
        ( 
            For
            {
                vParent.TriggerType = TriggerType.For;
            }
        | 
            tId2:Identifier 
            ( 
                tOf:Of 
                {
                    ofAppeared = true;
                }
            )?
            {
                if (ofAppeared == true)
                    {
                        Match(tId2,CodeGenerationSupporter.Instead);
                        vParent.TriggerType = TriggerType.InsteadOf;
                    }
                else 
                    {
                        Match(tId2,CodeGenerationSupporter.After);
                        vParent.TriggerType = TriggerType.After;
                    }
            }   
        )
        vAction=dmlTriggerAction
        {
            AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
        }
        ( 
            Comma vAction=dmlTriggerAction
            {
                AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
            }
        )*
        (
            With tAppend:Identifier
            {
                Match(tAppend, CodeGenerationSupporter.Append);
                vParent.WithAppend = true;
            }
        )?
        (    Not For Replication 
            {
                vParent.IsNotForReplication = true;
            }
        )?
    ;

triggerObject returns [TriggerObject vResult = FragmentFactory.CreateFragment<TriggerObject>()]
{
    SchemaObjectName vSchemaObjectName;
}
    : vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.Name = vSchemaObjectName;
            vResult.TriggerScope = TriggerScope.Normal;
        }
    ;

dmlTriggerAction returns [TriggerAction vResult = FragmentFactory.CreateFragment<TriggerAction>()]
    : 
        tInsert:Insert
        {
            UpdateTokenInfo(vResult,tInsert);
            vResult.TriggerActionType = TriggerActionType.Insert;
        }
    | 
        tUpdate:Update
        {
            UpdateTokenInfo(vResult,tUpdate);
            vResult.TriggerActionType = TriggerActionType.Update;
        }
    | 
        tDelete:Delete
        {
            UpdateTokenInfo(vResult,tDelete);
            vResult.TriggerActionType = TriggerActionType.Delete;
        }
    ;

/////////////////////////////////////////////////
// Execute STATEMENT
/////////////////////////////////////////////////

executeStatement returns [ExecuteStatement vResult = FragmentFactory.CreateFragment<ExecuteStatement>()]
{
    ExecuteSpecification vExecuteSpecification;
    ExecuteOption vOption;
}
    : vExecuteSpecification = executeSpecification
       {
            vResult.ExecuteSpecification = vExecuteSpecification;
            ThrowPartialAstIfPhaseOne(vResult);
       }
          (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With vOption = executeOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )?
    ;

executeOption returns [ExecuteOption vResult = FragmentFactory.CreateFragment<ExecuteOption>()]
    : idRecompile:Identifier // RECOMPILE
        {
            Match(idRecompile, CodeGenerationSupporter.Recompile);
            vResult.OptionKind = ExecuteOptionKind.Recompile;
        }
    ;
    
execStart [TSqlFragment vParent]
    : tExec:Execute 
        {
            UpdateTokenInfo(vParent,tExec);
        }
    | tExec2:Exec 
        {
            UpdateTokenInfo(vParent,tExec2);
        }
    ;
       
executeSpecification returns [ExecuteSpecification vResult = FragmentFactory.CreateFragment<ExecuteSpecification>()]
    : 
        execStart[vResult] 
        execTypes[vResult]
    ;

execTypes [ExecuteSpecification vParent]
{
    VariableReference vVar;
    ExecutableEntity vExecutable;
}
    :    LeftParenthesis vExecutable = execStrTypes tRparen:RightParenthesis 
        {
            UpdateTokenInfo(vParent,tRparen);
            vParent.ExecutableEntity = vExecutable;
        }
    |    vExecutable = execProcEx
        {
            vParent.ExecutableEntity = vExecutable;
        }
    |    vVar = variable EqualsSign vExecutable = execProcEx
        {
            vParent.Variable = vVar;
            vParent.ExecutableEntity = vExecutable;
        }
    ;
    
execStrTypes returns [ExecutableEntity vResult]
    :    vResult = execSqlList (Comma setParamList[vResult])?
    ;
    
execProcEx returns [ExecutableProcedureReference vResult]
    :    vResult = execProc
    |   vResult = adhocDataSourceExecproc
    ;

adhocDataSourceExecproc returns [ExecutableProcedureReference vResult = FragmentFactory.CreateFragment<ExecutableProcedureReference>()]
{
    ProcedureReferenceName vProcRef;
    AdHocDataSource vDataSource;
}
    :    vDataSource = adhocDataSource Dot vProcRef = procObjectReference 
        {
            vResult.AdHocDataSource = vDataSource;
            
            // TODO, olegr: add check for number of name parts - should be exactly 3 (see corresonding rule in .cpp parser)
            
            vResult.ProcedureReference = vProcRef;
        }
        (options {greedy=true;} : setParamList[vResult])?
    ;

execProc returns [ExecutableProcedureReference vResult = FragmentFactory.CreateFragment<ExecutableProcedureReference>()]
{
    ProcedureReferenceName vProcRef;
}
    : 
        (vProcRef = procObjectReference | vProcRef = varObjectReference) 
        {
            vResult.ProcedureReference = vProcRef;
        }
        (
            options {greedy=true;} : setParamList[vResult]
        )?
    ;

procObjectReference returns [ProcedureReferenceName vResult = FragmentFactory.CreateFragment<ProcedureReferenceName>()]
{
    ProcedureReference vProcRefId;
}
    : vProcRefId = procedureReference
        {
            vResult.ProcedureReference=vProcRefId;
        }
    ;

varObjectReference  returns [ProcedureReferenceName vResult = FragmentFactory.CreateFragment<ProcedureReferenceName>()]
{
     VariableReference vProcRefVariable;
}    
    : vProcRefVariable = variable
        {
            vResult.ProcedureVariable=vProcRefVariable;
        }
    ;
    
procedureReference returns [ProcedureReference vResult = FragmentFactory.CreateFragment<ProcedureReference>()]
{
    Literal vProcNum;
    SchemaObjectName vName;
}
    : vName=schemaObjectFourPartName vProcNum = procNumOpt
        {
            vResult.Name = vName;
            vResult.Number = vProcNum;
        }
    ;

execSqlList returns [ExecutableStringList vResult = FragmentFactory.CreateFragment<ExecutableStringList>()]
{
    ValueExpression vFrag;
}
    : vFrag = stringOrGlobalVariableOrVariable 
        {
            AddAndUpdateTokenInfo(vResult, vResult.Strings, vFrag);
        }
     (Plus vFrag = stringOrGlobalVariableOrVariable
        {
            AddAndUpdateTokenInfo(vResult, vResult.Strings, vFrag);
        }
     )*
    ;

binaryOrVariable returns [ValueExpression vResult]
    :    vResult = binary
    |   vResult = variable
    ;

integerOrVariable returns [ValueExpression vResult]
    :    vResult = integer
    |   vResult = variable
    ;

integerOrRealOrNumeric returns [Literal vResult]
    :    vResult = integer
    |   vResult = real
    |    vResult = numeric
    ;

// TODO, olegr: Check with rwaymi if we need this and when...
//execEncryptedList returns [ExecutableEncryptedList vResult = FragmentFactory.CreateFragment<ExecutableEncryptedList>()]
//{
//    Literal vItem;
//}
//    :    vItem = execEncryptedItem 
//        {
//            vResult.AddItem(vItem);
//        }
//        (Plus vItem = execEncryptedItem
//            {
//                vResult.AddItem(vItem);
//            }
//        )*
//    ;
//
//execEncryptedItem returns [Literal vResult]
//    : Number 
//    ;

setParamList [ExecutableEntity vParent]
{
    ExecuteParameter vParam;
    bool nameEqualsValueWasUsed = false;
    int parameterNumber = 0;
}
    :    vParam = setParam[ref nameEqualsValueWasUsed, ref parameterNumber]
        {
            AddAndUpdateTokenInfo(vParent, vParent.Parameters, vParam);
        }
        (Comma vParam = setParam[ref nameEqualsValueWasUsed, ref parameterNumber]
            {
                AddAndUpdateTokenInfo(vParent, vParent.Parameters, vParam);
            }
        )*
    ;

setParam [ref bool nameEqualsValueWasUsed, ref int parameterNumber] returns [ExecuteParameter vResult = FragmentFactory.CreateFragment<ExecuteParameter>()]
{
    VariableReference vVariable;
    ScalarExpression vValue;
    Literal vLiteral;
    vResult.IsOutput = false;
    ++parameterNumber;
}
    :    (vVariable = variable EqualsSign 
            { 
                vResult.Variable = vVariable; 
            }
        )? 
        (
            (vValue = possibleNegativeConstantOrIdentifier
                {
                    vResult.ParameterValue = vValue;
                    
                    if (vResult.Variable != null)
                    {
                        nameEqualsValueWasUsed = true;
                    }
                    else if (nameEqualsValueWasUsed)
                    {
                        ThrowParseErrorException("SQL46089", vValue, TSqlParserResource.SQL46089Message, 
                            parameterNumber.ToString(CultureInfo.CurrentCulture));
                    }
                }
                (
                    {NextTokenMatches(CodeGenerationSupporter.Output) || NextTokenMatches(CodeGenerationSupporter.Out)}?
                    outId : Identifier // ID = OUTPUT / OUT
                    {
                        VariableReference parameterAsVariableReference = vValue as VariableReference;
                        GlobalVariableExpression parameterAsGlobalVarExpr = vValue as GlobalVariableExpression;
                        if (parameterAsVariableReference == null && parameterAsGlobalVarExpr == null)
                        {
                            ThrowParseErrorException("SQL46088", outId, TSqlParserResource.SQL46088Message);
                        }
                        
                        Match(outId, CodeGenerationSupporter.Output, CodeGenerationSupporter.Out);
                        vResult.IsOutput = true;                    
                        UpdateTokenInfo(vResult,outId);
                    }
                )?
            ) 
        | 
            vLiteral=defaultLiteral
            {
                vResult.ParameterValue = vLiteral;
            }
        )        
    ;

adhocDataSource returns [AdHocDataSource vResult = FragmentFactory.CreateFragment<AdHocDataSource>()]
{
    StringLiteral vProvider, vInit;
}
    : tOpenDataSource:OpenDataSource LeftParenthesis vProvider = stringLiteral Comma vInit = stringLiteral tEndParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tOpenDataSource);
            vResult.ProviderName = vProvider;
            vResult.InitString = vInit;
            UpdateTokenInfo(vResult,tEndParen);
        }
    ;

procNumOpt returns [Literal vResult = null]
    :  ProcNameSemicolon vResult = integer
    |
    ;

/////////////////////////////////////////////////
// Execute STATEMENT ENDS
/////////////////////////////////////////////////

createTableStatement returns [CreateTableStatement vResult = this.FragmentFactory.CreateFragment<CreateTableStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    TableDefinition vTableDefinition;
    IdentifierOrValueExpression vTextImageOnValue;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
}
    : tCreate:Create Table vSchemaObjectName=schemaObjectThreePartName 
        {
            UpdateTokenInfo(vResult,tCreate);
            vResult.SchemaObjectName = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        LeftParenthesis 
        vTableDefinition = tableDefinitionCreateTable
        {
            vResult.Definition = vTableDefinition;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        // Default is not used as a keyword after on or textimage_on (even though it is a keyword)
        // So we don't need to check for an optional Default keyword, default will always be quoted
        ( 
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        (    
            tTextImageOn:Identifier vTextImageOnValue = stringOrIdentifier
            {
                Match(tTextImageOn,CodeGenerationSupporter.TextImageOn);
                vResult.TextImageOn = vTextImageOnValue;
            }
        )?
    ;

tableDefinitionCreateTable returns [TableDefinition vResult = this.FragmentFactory.CreateFragment<TableDefinition>()]
    : 
        tableElement[IndexAffectingStatement.CreateTable, vResult, null] 
        ( 
            options {greedy=true;} :
            tComma:Comma 
            tableElement[IndexAffectingStatement.CreateTable, vResult, null]
        )* 
        // Dangling Comma at the end is valid 
        (Comma)?
    ;

alterTableStatement returns [AlterTableStatement vResult = null]
{
    SchemaObjectName vSchemaObjectName = null;
    ConstraintEnforcement vExistingRowsCheck = ConstraintEnforcement.NotSpecified;
}
    : tAlter:Alter Table vSchemaObjectName=schemaObjectThreePartName 
        ( 
            vResult=alterTableAlterColumnStatement
        |   vResult=alterTableTriggerModificationStatement
        |   vResult=alterTableDropTableElementStatement
        |   (
                (With vExistingRowsCheck = constraintEnforcement)?
                (
                    vResult=alterTableAddTableElementStatement[vExistingRowsCheck]
                |   
                    vResult=alterTableConstraintModificationStatement[vExistingRowsCheck]
                )
            )
        )
        {   // Update position later, because instantiation is lazy
            UpdateTokenInfo(vResult,tAlter);
            vResult.SchemaObjectName = vSchemaObjectName;
        }
    ;
    exception
    catch[PhaseOnePartialAstException exception]
    {
        AlterTableStatement vStatement = exception.Statement as AlterTableStatement;
        Debug.Assert(vStatement != null);
        UpdateTokenInfo(vStatement, tAlter);
        vStatement.SchemaObjectName = vSchemaObjectName;
        throw;
    }

constraintEnforcement returns [ConstraintEnforcement vResult = ConstraintEnforcement.NotSpecified]
    :
        Check
        {
            vResult=ConstraintEnforcement.Check;
        }
    |   NoCheck
        {
            vResult=ConstraintEnforcement.NoCheck;
        }
    ;

alterTableAlterColumnStatement returns [AlterTableAlterColumnStatement vResult = this.FragmentFactory.CreateFragment<AlterTableAlterColumnStatement>()]
{
    Identifier vIdentifier;
    DataTypeReference vDataType;
    bool vIsNull;
    bool vAddDefined = false;
}
    :    Alter Column vIdentifier=identifier 
        {
            vResult.ColumnIdentifier = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            vDataType=scalarDataType 
            {
                vResult.DataType = vDataType;
            }
            collationOpt[vResult]
            (vIsNull = nullNotNull[vResult]
                {
                    vResult.AlterTableAlterColumnOption = (vIsNull ? AlterTableAlterColumnOption.Null : AlterTableAlterColumnOption.NotNull);
                }
            )?
        |   
            ( 
                Add 
                {
                    vAddDefined = true;
                }
            |   Drop 
                {
                    vAddDefined = false;
                }
            )
            (
                tRowguidcol:RowGuidColumn
                {
                    UpdateTokenInfo(vResult,tRowguidcol);
                    if (vAddDefined)
                        vResult.AlterTableAlterColumnOption = AlterTableAlterColumnOption.AddRowGuidCol;
                    else
                        vResult.AlterTableAlterColumnOption = AlterTableAlterColumnOption.DropRowGuidCol;
                }
            |
                Not For tReplication:Replication 
                {
                    UpdateTokenInfo(vResult,tReplication);
                    if (vAddDefined)
                        vResult.AlterTableAlterColumnOption = AlterTableAlterColumnOption.AddNotForReplication;
                    else
                        vResult.AlterTableAlterColumnOption = AlterTableAlterColumnOption.DropNotForReplication;
                }
            )
        )
    ;

alterTableTriggerModificationStatement returns [AlterTableTriggerModificationStatement vResult = this.FragmentFactory.CreateFragment<AlterTableTriggerModificationStatement>()]
{
    Identifier vIdentifier;
}
    :    tId:Identifier
        {
            vResult.TriggerEnforcement = ParseTriggerEnforcement(tId);
        }
        Trigger
        (
            tAll:All
            {
                vResult.All = true;
                UpdateTokenInfo(vResult,tAll);
            }
        |   vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.TriggerNames, vIdentifier);
            }
            (
                Comma vIdentifier=identifier
                {
                    AddAndUpdateTokenInfo(vResult, vResult.TriggerNames, vIdentifier);
                }
            )*
        )
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;

alterTableDropTableElementStatement returns [AlterTableDropTableElementStatement vResult = this.FragmentFactory.CreateFragment<AlterTableDropTableElementStatement>()]
{
    AlterTableDropTableElement vAlterTableDropTableElement;   
}
    :    Drop vAlterTableDropTableElement=alterTableDropTableElement
        {
            AddAndUpdateTokenInfo(vResult, vResult.AlterTableDropTableElements, vAlterTableDropTableElement);
        }
        ( 
            Comma vAlterTableDropTableElement=alterTableDropTableElement
            {
                AddAndUpdateTokenInfo(vResult, vResult.AlterTableDropTableElements, vAlterTableDropTableElement);
            }
        )*
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }        
    ;

alterTableDropTableElement returns [AlterTableDropTableElement vResult = this.FragmentFactory.CreateFragment<AlterTableDropTableElement>()]
{
    Identifier vIdentifier;
}
    :
        ( 
            Constraint 
            {
                vResult.TableElementType = TableElementType.Constraint;
            }
        )?
        vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
    |
        Column vIdentifier=identifier
        {
            vResult.TableElementType = TableElementType.Column;
            vResult.Name = vIdentifier;
        }
    ;

alterTableAddTableElementStatement[ConstraintEnforcement vExistingRowsCheck] returns [AlterTableAddTableElementStatement vResult = FragmentFactory.CreateFragment<AlterTableAddTableElementStatement>()]
{
    vResult.ExistingRowsCheckEnforcement = vExistingRowsCheck;
    TableDefinition vTableDefinition;
}
    : 
        Add 
        vTableDefinition = tableDefinition[IndexAffectingStatement.AlterTableAddElement, vResult]
        {
            vResult.Definition=vTableDefinition;
        }
    ;


alterTableConstraintModificationStatement[ConstraintEnforcement vExistingRowsCheck] returns [AlterTableConstraintModificationStatement vResult = this.FragmentFactory.CreateFragment<AlterTableConstraintModificationStatement>()]
{
    Identifier vIdentifier;
    vResult.ExistingRowsCheckEnforcement = vExistingRowsCheck;
    ConstraintEnforcement vCheck;
}
    :    vCheck = constraintEnforcement Constraint
        {
            vResult.ConstraintEnforcement = vCheck;
        }
        (
            tAll:All
            {
                vResult.All = true;
                UpdateTokenInfo(vResult,tAll);
            }
        |   vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.ConstraintNames, vIdentifier);
            }
            (
                Comma vIdentifier=identifier
                {
                    AddAndUpdateTokenInfo(vResult, vResult.ConstraintNames, vIdentifier);
                }
            )*
        )
        {
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;

tableElement[IndexAffectingStatement statementType, TableDefinition vParent, AlterTableAddTableElementStatement vStatement]
{
    ColumnDefinition vColumnDefinition;
    ConstraintDefinition vConstraint;
}
    : vColumnDefinition=columnDefinition[statementType, vStatement]
        {
            AddAndUpdateTokenInfo(vParent, vParent.ColumnDefinitions, vColumnDefinition);
        }
    | vConstraint=tableConstraint[statementType, vStatement]
        {
            AddAndUpdateTokenInfo(vParent, vParent.TableConstraints, vConstraint);
        }
    ;

columnDefinition [IndexAffectingStatement statementType, AlterTableAddTableElementStatement vStatement] returns [ColumnDefinition vResult = FragmentFactory.CreateFragment<ColumnDefinition>()]
{
    Identifier vIdentifier;
    ConstraintDefinition vConstraint;
    Identifier vConstraintIdentifier = null;
    ScalarExpression vExpression;
}
    : vIdentifier=identifier
        {
            vResult.ColumnIdentifier = vIdentifier;
            
            // Phase one is handled a little different here than
            // other places, because we have to add the column definition prematurely.
            if (PhaseOne)
            {               
                // It is a P1 only at AlterTableAddTableElementStatement... 
                if (vStatement != null)
                {
                    AddAndUpdateTokenInfo(vStatement, vStatement.Definition.ColumnDefinitions, vResult);                  
                    ThrowPartialAstIfPhaseOne(vStatement);
                }
            }
        }
        (
            (
                As vExpression=expressionWithFlags[ExpressionFlags.ScalarSubqueriesDisallowed] // Computed column
                {
                    vResult.ComputedColumnExpression = vExpression;
                }
                (    // Only Unique/Primary Key constraints are allowed on computed columns
                    (
                        tConstraint:Constraint vConstraintIdentifier=identifier
                        {
                            ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tConstraint);
                        }
                    )?
                    vConstraint = uniqueColumnConstraint
                    {
                        if (vConstraintIdentifier != null)
                        {
                            UpdateTokenInfo(vConstraint,tConstraint);
                            vConstraint.ConstraintIdentifier = vConstraintIdentifier;
                        }
                        AddAndUpdateTokenInfo(vResult, vResult.Constraints, vConstraint);
                    }
                )?            
            )
        |    
            (    // Regular column
                regularColumnBody[vResult]
                columnConstraintListOpt[statementType, vResult]
            )
        )
    ;
    
regularColumnBody [ColumnDefinition vParent]
{
    DataTypeReference vDataType = null;
}
    :    ( options {greedy=true;} :                    
            vDataType=scalarDataType
            {
                vParent.DataType = vDataType;
            }
            collationOpt[vParent]
        )?
        {
            VerifyColumnDataType(vParent);
        }
    ;
    
columnConstraintListOpt [IndexAffectingStatement statementType, ColumnDefinition vResult]
{
    ConstraintDefinition vConstraint;
    IdentityOptions vIdentityOptions;
}
    :    ( rowguidcolConstraint[vResult]
        | vIdentityOptions=identityConstraint[statementType]
            {
                if (vResult.IdentityOptions != null)
                {
                    ThrowParseErrorException("SQL46043", vIdentityOptions, TSqlParserResource.SQL46043Message);
                }
                vResult.IdentityOptions=vIdentityOptions;
            } 
        | vConstraint=columnConstraint[statementType]
            {
                AddConstraintToColumn(vConstraint, vResult);
            }
        )*
    ;

rowguidcolConstraint [ColumnDefinition vParent]
    : tRowguidcol:RowGuidColumn
        {
            if (vParent.IsRowGuidCol)
                ThrowParseErrorException("SQL46042", tRowguidcol, TSqlParserResource.SQL46042Message);
            
            UpdateTokenInfo(vParent,tRowguidcol);
            vParent.IsRowGuidCol = true;
        }
    ;
    
identityConstraint [IndexAffectingStatement statementType] returns [IdentityOptions vResult = FragmentFactory.CreateFragment<IdentityOptions>()]
{
    ScalarExpression vExpression;
    bool vNotForReplication;
}
    : tIdentity:Identity
        {
            UpdateTokenInfo(vResult,tIdentity);
        } 
        (   
            (LeftParenthesis seedIncrement)=> // necessary because select statement can start with LeftParenthesis
            LeftParenthesis vExpression=seedIncrement 
            {
                vResult.IdentitySeed = vExpression;
            }
            Comma vExpression=seedIncrement 
            {
                vResult.IdentityIncrement = vExpression;
            }
            tRParen:RightParenthesis 
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
        vNotForReplication = replicationClauseOpt[statementType, vResult]
        {
            vResult.IsIdentityNotForReplication = vNotForReplication;
        }
    ;
    
// Same as column_definition in SQL YACC grammar
columnDefinitionBasic returns [ColumnDefinitionBase vResult = FragmentFactory.CreateFragment<ColumnDefinitionBase>()]
{
    Identifier vIdentifier;
    DataTypeReference vDataType;
}
    : vIdentifier=identifier vDataType=scalarDataType 
        {
            vResult.ColumnIdentifier = vIdentifier;
            vResult.DataType = vDataType;
        }
        collationOpt[vResult]
    ;
    
// Same as column_definition_ex in SQL YACC grammar
columnDefinitionEx returns [ColumnDefinitionBase vResult = null]
    : vResult = columnDefinitionBasic
    | tTimeStamp:Identifier
        {
            Match(tTimeStamp, CodeGenerationSupporter.TimeStamp);
            vResult = FragmentFactory.CreateFragment<ColumnDefinitionBase>();
            
            Identifier vIdent = this.FragmentFactory.CreateFragment<Identifier>();
            UpdateTokenInfo(vIdent, tTimeStamp);
            vIdent.SetUnquotedIdentifier(CodeGenerationSupporter.TimeStamp);
            
            vResult.ColumnIdentifier = vIdent;
        } 
    ;

columnConstraint[IndexAffectingStatement statementType] returns [ConstraintDefinition vResult = null]
{
    Identifier vConstraintIdentifier = null;
}
    : 
        ( 
            tConstraint:Constraint vConstraintIdentifier=identifier
            {
                ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tConstraint);
            }
        )?
        ( vResult=nullableConstraint
        | vResult=defaultColumnConstraint[statementType]
        | vResult=uniqueColumnConstraint
        | vResult=foreignKeyColumnConstraint[statementType]
        | vResult=checkConstraint[statementType]
        ) 
        {
            // null check is necessary for tConstraint
            if (vConstraintIdentifier != null) 
            {
                UpdateTokenInfo(vResult,tConstraint);
                vResult.ConstraintIdentifier = vConstraintIdentifier;
            }
        }
    ;
    exception
    catch[PhaseOneConstraintException]
    {
        // This can happen because of check constraint, however
        // it should never happen, because CreateTable or the 
        // Alter table add column should throw before this could happen.
        Debug.Assert(false);
        // TODO, sacaglar: in retail what should we do, throw or swallow?
    }

tableConstraint[IndexAffectingStatement statementType, AlterTableAddTableElementStatement vStatement] returns [ConstraintDefinition vResult = null]
{
    Identifier vConstraintIdentifier = null;
}
    : 
        ( 
            tConstraint:Constraint vConstraintIdentifier=identifier
            {
                ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tConstraint);
            }
        )?
        ( vResult=uniqueTableConstraint
        | vResult=defaultTableConstraint[statementType]
        | vResult=foreignKeyTableConstraint[statementType]
        | vResult=checkConstraint[statementType]
        ) 
        {
            // null check is necessary for tConstraint
            if (vConstraintIdentifier != null) 
            {
                UpdateTokenInfo(vResult,tConstraint);
                vResult.ConstraintIdentifier = vConstraintIdentifier;
            }
        }
    ;
    exception
    catch[PhaseOneConstraintException exception]
    {
        Debug.Assert(PhaseOne == true);
        if (vConstraintIdentifier != null)
        {
            UpdateTokenInfo(exception.Constraint, tConstraint);
            exception.Constraint.ConstraintIdentifier = vConstraintIdentifier;
        }
        Debug.Assert(vStatement != null);
        AddAndUpdateTokenInfo(vStatement, vStatement.Definition.TableConstraints, exception.Constraint);
        
        ThrowPartialAstIfPhaseOne(vStatement);
    }

nullableConstraint returns [NullableConstraintDefinition vResult = this.FragmentFactory.CreateFragment<NullableConstraintDefinition>()]
{
    bool vIsNull;
}
    : vIsNull = nullNotNull[vResult]
        {
            vResult.Nullable = vIsNull;
        }
    ;

// Highly similar code to defaultTableConstraint
defaultColumnConstraint[IndexAffectingStatement statementType] returns [DefaultConstraintDefinition vResult = this.FragmentFactory.CreateFragment<DefaultConstraintDefinition>()]
{
    ScalarExpression vExpression;
}
    : tDefault:Default vExpression=expressionWithFlags[ExpressionFlags.ScalarSubqueriesDisallowed]
        {
            UpdateTokenInfo(vResult,tDefault);
            vResult.Expression = vExpression;
        }
        (
            With tValues:Values
            {
                if (statementType != IndexAffectingStatement.AlterTableAddElement)
                    ThrowParseErrorException("SQL46013", tDefault, TSqlParserResource.SQL46013Message);
                    
                UpdateTokenInfo(vResult,tValues);
                vResult.WithValues = true;
            }
        )?
    ;

// Highly similar code to defaultColumnConstraint
defaultTableConstraint [IndexAffectingStatement statementType] returns [DefaultConstraintDefinition vResult = FragmentFactory.CreateFragment<DefaultConstraintDefinition>()]
{
    ScalarExpression vExpression;
    Identifier vIdentifier;
}
    :    tDefault:Default 
        {
            if (statementType != IndexAffectingStatement.AlterTableAddElement)
                ThrowParseErrorException("SQL46014", tDefault, TSqlParserResource.SQL46014Message);
                
            UpdateTokenInfo(vResult,tDefault);
            ThrowConstraintIfPhaseOne(vResult);
        }
        vExpression=expressionWithFlags[ExpressionFlags.ScalarSubqueriesDisallowed] For vIdentifier=identifier
        {
            vResult.Expression = vExpression;
            vResult.Column = vIdentifier;
        }
        (
            With tValues:Values
            {
                UpdateTokenInfo(vResult,tValues);
                vResult.WithValues = true;
            }
        )?
    ;

// Warning: highly similar code with uniqueTableConstraint
uniqueColumnConstraint returns [UniqueConstraintDefinition vResult = this.FragmentFactory.CreateFragment<UniqueConstraintDefinition>()]
{
    ColumnWithSortOrder vColumnWithSortOrder;
}
    :
        uniqueConstraintHeader[vResult, false]
        (
            (LeftParenthesis columnWithSortOrder)=>
            LeftParenthesis vColumnWithSortOrder=columnWithSortOrder
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
            }
            ( Comma vColumnWithSortOrder=columnWithSortOrder
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
                }
            )*                
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )? 
        uniqueConstraintTailOpt[vResult]
    ;

// Warning: highly similar code with uniqueColumnConstraint
uniqueTableConstraint returns [UniqueConstraintDefinition vResult = FragmentFactory.CreateFragment<UniqueConstraintDefinition>()]
{
    ColumnWithSortOrder vColumnWithSortOrder;
}
    :
        uniqueConstraintHeader[vResult, true]
        LeftParenthesis vColumnWithSortOrder=columnWithSortOrder
        {
            AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
        }
        ( Comma vColumnWithSortOrder=columnWithSortOrder
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumnWithSortOrder);
            }
        )*                
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        uniqueConstraintTailOpt[vResult]
    ;

uniqueConstraintHeader [UniqueConstraintDefinition vParent, bool throwInPhaseOne]
    :
        ( tUnique:Unique 
            {
                UpdateTokenInfo(vParent,tUnique);
                vParent.IsPrimaryKey = false;
            }
        | tPrimary:Primary tKey:Key
            {
                UpdateTokenInfo(vParent,tPrimary); // Important to set the beginning.
                UpdateTokenInfo(vParent,tKey);
                vParent.IsPrimaryKey = true;
            }
        ) 
        {
            if (throwInPhaseOne)
                ThrowConstraintIfPhaseOne(vParent);
        }
        ( tClustered:Clustered
            {
                UpdateTokenInfo(vParent,tClustered);
                vParent.Clustered = true;
            }
        | tNonclustered:NonClustered
            {
                UpdateTokenInfo(vParent,tNonclustered);
                vParent.Clustered = false;
            } 
        )?
    ;

uniqueConstraintTailOpt [UniqueConstraintDefinition vParent]
{
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
}
    :
        uniqueConstraintIndexOptionsOpt[vParent]
        (tOn:On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vParent.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?        
    ;

uniqueConstraintIndexOptionsOpt[UniqueConstraintDefinition vParent]
{
    IndexOption vIndexOption;
}
    :    (
            With
            (
                vIndexOption=fillFactorOption
                {
                    AddAndUpdateTokenInfo(vParent, vParent.IndexOptions, vIndexOption);
                }
                (
                    {NextTokenMatchesOneOf(CodeGenerationSupporter.SortedData, CodeGenerationSupporter.SortedDataReorg)}?
                    sortedDataOptions
                )?
            |            
                sortedDataOptions
                (
                    vIndexOption=fillFactorOption
                    {
                        AddAndUpdateTokenInfo(vParent, vParent.IndexOptions, vIndexOption);
                    }
                )?
            )
        )?
    ;

sortedDataOptions
    :
        tOption:Identifier
        {
            Match(tOption, CodeGenerationSupporter.SortedData, CodeGenerationSupporter.SortedDataReorg);
        }
    ;

columnWithSortOrder returns [ColumnWithSortOrder vResult = this.FragmentFactory.CreateFragment<ColumnWithSortOrder>()]
{
    ColumnReferenceExpression vColumn;
    SortOrder vOrder;
}
    : vColumn=identifierColumnReferenceExpression
        {
            vResult.Column = vColumn;
        }
        (vOrder = orderByOption[vResult]
            {
                vResult.SortOrder = vOrder;
            }
        )?
    ;

identifierColumnReferenceExpression returns [ColumnReferenceExpression vResult = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        vMultiPartIdentifier=multiPartIdentifier[1]
        {
            vResult.ColumnType = ColumnType.Regular;
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;

deleteUpdateAction[TSqlFragment vParent] returns [DeleteUpdateAction vResult = DeleteUpdateAction.NoAction]
    :
        tNo:Identifier 
        {
            Match(tNo, CodeGenerationSupporter.No);
        }
        tAction:Identifier 
        {
            UpdateTokenInfo(vParent,tAction);
            Match(tAction, CodeGenerationSupporter.Action);
        }
    |  
        tCascade:Cascade
        {
            UpdateTokenInfo(vParent,tCascade);
            vResult = DeleteUpdateAction.Cascade;
        }
    |
        Set
        (
            tNull:Null
            {
                UpdateTokenInfo(vParent,tNull);
                vResult = DeleteUpdateAction.SetNull;
            }
        |
            tDefault:Default
            {
                UpdateTokenInfo(vParent,tDefault);
                vResult = DeleteUpdateAction.SetDefault;
            }
        )
    ;

foreignKeyConstraintCommonEnd[IndexAffectingStatement statementType, ForeignKeyConstraintDefinition vParent]
{
    DeleteUpdateAction vDeleteUpdateAction;
    bool vOnDeleteParsed = false;
    SchemaObjectName vSchemaObjectName;
    bool vNotForReplication;
}
    : tReferences:References vSchemaObjectName=schemaObjectThreePartName 
        {
            ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tReferences);
            UpdateTokenInfo(vParent,tReferences);
            vParent.ReferenceTableName = vSchemaObjectName;
        }
        ( 
            (LeftParenthesis identifier)=> // necessary because select statement can start with LeftParenthesis
            columnNameList[vParent, vParent.ReferencedTableColumns]
        )?
        (   
            (On Delete)=>
            On Delete vDeleteUpdateAction=deleteUpdateAction[vParent]
            {
                vParent.DeleteAction = vDeleteUpdateAction;
                vOnDeleteParsed = true;
            }
        )?
        (
            On Update vDeleteUpdateAction=deleteUpdateAction[vParent]
            {
                vParent.UpdateAction = vDeleteUpdateAction;
            }
            (   
                tOn:On Delete 
                vDeleteUpdateAction=deleteUpdateAction[vParent]
                {
                    if (vOnDeleteParsed)
                    {
                        throw GetUnexpectedTokenErrorException(tOn); 
                    }
                    vParent.DeleteAction = vDeleteUpdateAction;
                }
            )?
        )?
        vNotForReplication = replicationClauseOpt[statementType, vParent]
        {
            vParent.NotForReplication = vNotForReplication;
        }
    ;
    
replicationClauseOpt [IndexAffectingStatement statementType, TSqlFragment vParent] returns [bool vResult = false]
    :
        ( 
            (Not For)=>
            tNot:Not For tReplication:Replication
            {
                ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tNot);
            
                UpdateTokenInfo(vParent,tReplication);
                vResult = true;
            }
        )?
    ;

// Warning: highly similar code with foreignKeyTableConstraint
foreignKeyColumnConstraint [IndexAffectingStatement statementType] returns [ForeignKeyConstraintDefinition vResult = FragmentFactory.CreateFragment<ForeignKeyConstraintDefinition>()]
    :
        (
            tForeign:Foreign Key
            {
                ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tForeign);
                UpdateTokenInfo(vResult,tForeign);
            }
            foreignConstraintColumnsOpt[vResult]
        )?
        foreignKeyConstraintCommonEnd[statementType, vResult]
    ; 

// Warning: highly similar code with foreignKeyColumnConstraint
foreignKeyTableConstraint [IndexAffectingStatement statementType] returns [ForeignKeyConstraintDefinition vResult = FragmentFactory.CreateFragment<ForeignKeyConstraintDefinition>()]
    :   tForeign:Foreign Key
        {
            ThrowSyntaxErrorIfNotCreateAlterTable(statementType, tForeign);
            
            UpdateTokenInfo(vResult,tForeign);
            ThrowConstraintIfPhaseOne(vResult);
        }
        foreignConstraintColumnsOpt[vResult]
        foreignKeyConstraintCommonEnd[statementType, vResult]
    ;
    
foreignConstraintColumnsOpt [ForeignKeyConstraintDefinition vParent]
    : 
        ( 
            (LeftParenthesis identifier)=> // necessary because select statement can start with LeftParenthesis
            columnNameList[vParent, vParent.Columns]
        )?
    ;

checkConstraint [IndexAffectingStatement statementType] returns [CheckConstraintDefinition vResult = FragmentFactory.CreateFragment<CheckConstraintDefinition>()]
{
    BooleanExpression vExpression;
    bool vNotForReplication;
}
    :    tCheck:Check 
        {
            UpdateTokenInfo(vResult,tCheck);
            ThrowConstraintIfPhaseOne(vResult);
        }
        vNotForReplication = replicationClauseOpt[statementType, vResult]
        {
            vResult.NotForReplication = vNotForReplication;
        }
        LeftParenthesis vExpression=booleanExpressionWithFlags[ExpressionFlags.ScalarSubqueriesDisallowed]
        {
            vResult.CheckCondition = vExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

cursorDataType returns [SqlDataTypeReference vResult = this.FragmentFactory.CreateFragment<SqlDataTypeReference>()]
    : tCursor:Cursor
        {
            UpdateTokenInfo(vResult,tCursor);
            vResult.SqlDataTypeOption = SqlDataTypeOption.Cursor;
        }
    ;

scalarDataType returns [DataTypeReference vResult = null]
{
    SchemaObjectName vDataTypeName = null;
    SqlDataTypeOption typeOption = SqlDataTypeOption.None;
    Identifier vIdentifier;
}
    : 
        (
            vIdentifier=identifier        
            {
                vDataTypeName = FragmentFactory.CreateFragment<SchemaObjectName>();
                AddAndUpdateTokenInfo(vDataTypeName, vDataTypeName.Identifiers, vIdentifier);
                
                typeOption = ParseDataType(vIdentifier.Value);
            }
            (
                {typeOption != SqlDataTypeOption.None}?
                vResult = sqlDataTypeWithoutNational[vDataTypeName, typeOption]
            |
                vResult = userDataType[vDataTypeName]
            )
        )
    | 
        vResult = doubleDataType
    |
        vResult = sqlDataTypeWithNational
    ; 

sqlDataTypeWithNational returns [SqlDataTypeReference vResult = FragmentFactory.CreateFragment<SqlDataTypeReference>()]
{
    Identifier vIdentifier;
    bool vVarying = false;
}
   :    tNational:National vIdentifier=identifier
        {
            vResult.SqlDataTypeOption = ParseDataType(vIdentifier.Value);
            
            if (vResult.SqlDataTypeOption == SqlDataTypeOption.None)
            {
                ThrowParseErrorException("SQL46003", tNational, 
                    TSqlParserResource.SQL46003Message, TSqlParserResource.UserDefined);
            }

            vResult.Name = FragmentFactory.CreateFragment<SchemaObjectName>();
            AddAndUpdateTokenInfo(vResult.Name, vResult.Name.Identifiers, vIdentifier);
            UpdateTokenInfo(vResult, tNational);
            vResult.UpdateTokenInfo(vIdentifier);
        }
        (tVarying:Varying
            {
                UpdateTokenInfo(vResult,tVarying);
                vVarying = true;
            }  
        )?
        dataTypeParametersOpt[vResult]
        {                    
            ProcessNationalAndVarying(vResult, tNational, vVarying);
            CheckSqlDataTypeParameters(vResult);
        }
    ;

sqlDataTypeWithoutNational [SchemaObjectName vName, SqlDataTypeOption vType] returns [SqlDataTypeReference vResult = FragmentFactory.CreateFragment<SqlDataTypeReference>()]
{
    vResult.Name = vName;
    vResult.SqlDataTypeOption = vType;
    vResult.UpdateTokenInfo(vName);
    
    bool vVarying = false;
}
   :    (tVarying:Varying
            {
                UpdateTokenInfo(vResult,tVarying);
                vVarying = true;
            }  
        )?
        dataTypeParametersOpt[vResult]
        {                    
            ProcessNationalAndVarying(vResult, null, vVarying);
            CheckSqlDataTypeParameters(vResult);
        }
    ;
    
userDataType [SchemaObjectName vName] returns [UserDataTypeReference vResult = FragmentFactory.CreateFragment<UserDataTypeReference>()]
{
    vResult.Name = vName;
    vResult.UpdateTokenInfo(vName);
}
    :   dataTypeParametersOpt[vResult]
    ;
    
dataTypeParametersOpt [ParameterizedDataTypeReference vParent]
{
    Literal vLiteral;
}
    :   (   (LeftParenthesis integer)=> // necessary because select statement can start with LeftParenthesis
            LeftParenthesis 
            (
                vLiteral=integer
                {
                    AddAndUpdateTokenInfo(vParent, vParent.Parameters, vLiteral);
                }
                ( Comma vLiteral=integer
                    {
                        AddAndUpdateTokenInfo(vParent, vParent.Parameters, vLiteral);
                    }
                )?
            )
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent,tRParen);
            }
        )?
    ;    

doubleDataType returns [SqlDataTypeReference vResult = FragmentFactory.CreateFragment<SqlDataTypeReference>()]
{
    Literal vLiteral;
}
    :   tDouble:Double tPrecision:Precision
        {
            SetNameForDoublePrecisionType(vResult, tDouble, tPrecision);
            vResult.SqlDataTypeOption = SqlDataTypeOption.Float;
        }
        (   (LeftParenthesis (integer | Identifier))=> // necessary because select statement can start with LeftParenthesis
            LeftParenthesis 
            (
                vLiteral=integer
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parameters, vLiteral);
                }
            )
            tRParen2:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen2);
            }
        )?
    ;

multiPartIdentifier[int vMaxNumber] returns [MultiPartIdentifier vResult = FragmentFactory.CreateFragment<MultiPartIdentifier>()]
{
    List<Identifier> vIdentifierList;
}
    :
        vIdentifierList = identifierList[vMaxNumber]
       {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifierList);
        }
    ;
    
identifierList[int vMaxNumber] returns [List<Identifier> vResult = new List<Identifier>()]
{
    Identifier vIdentifier = null;
}
    :
        vIdentifier=identifier
        {
            AddIdentifierToListWithCheck(vResult, vIdentifier, vMaxNumber);
        }       
        (
            options {greedy=true;} :
            identifierListElement[vResult, vMaxNumber, false]
        )*
    |   
        identifierListElement[vResult, vMaxNumber, true]
        (
            options {greedy=true;} : 
            identifierListElement[vResult, vMaxNumber, false]
        )*
    ;

identifierListElement[List<Identifier> vParent, int vMaxNumber, bool first]
{
    Identifier vIdentifier;
}
    :
        tDot1:Dot
        {
            if (true == first)
                {
                    vIdentifier = GetEmptyIdentifier(tDot1);
                    AddIdentifierToListWithCheck(vParent, vIdentifier, vMaxNumber);
                }
        }
        (
            tDot2:Dot
            {
                vIdentifier = GetEmptyIdentifier(tDot2);
                AddIdentifierToListWithCheck(vParent, vIdentifier, vMaxNumber);
            }
        )*        
        vIdentifier=identifier
        {
            AddIdentifierToListWithCheck(vParent, vIdentifier, vMaxNumber);
        }       
    ;

schemaObjectTwoPartName returns [SchemaObjectName vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>()]
{
    List<Identifier> vIdentifierList;
}
    :
       vIdentifierList=identifierList[2]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifierList);
        }
    ;

schemaObjectThreePartName returns [SchemaObjectName vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>()]
{
    List<Identifier> vIdentifierList;
}
    :   vIdentifierList=identifierList[3]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifierList);
        }
    ;

schemaObjectFourPartName returns [SchemaObjectName vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>()]
{
    List<Identifier> vIdentifierList;
}
    :   vIdentifierList=identifierList[4]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifierList);
        }
    ;

booleanExpression returns [BooleanExpression vResult=null]
    : vResult=booleanExpressionWithFlags[ExpressionFlags.None]
    ;

booleanExpressionWithFlags [ExpressionFlags expressionFlags] returns [BooleanExpression vResult=null]
    : vResult=booleanExpressionOr[expressionFlags]
    ;

booleanExpressionOr [ExpressionFlags expressionFlags] returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;
}
    : vResult=booleanExpressionAnd[expressionFlags]
        ( 
            Or vExpression=booleanExpressionAnd[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BooleanBinaryExpressionType.Or);
            }
        )*
    ;

booleanExpressionAnd [ExpressionFlags expressionFlags] returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;
}
    : vResult=booleanExpressionUnary[expressionFlags]
        ( 
            And vExpression=booleanExpressionUnary[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BooleanBinaryExpressionType.And);
            }
        )*
    ;

booleanExpressionUnary [ExpressionFlags expressionFlags] returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;
}
    : tNot:Not vExpression=booleanExpressionUnary[expressionFlags]
        {
            BooleanNotExpression vUnaryExpression = this.FragmentFactory.CreateFragment<BooleanNotExpression>();
            vResult = vUnaryExpression;
            UpdateTokenInfo(vUnaryExpression, tNot);
            vUnaryExpression.Expression = vExpression;
        }
    | vResult=booleanExpressionPrimary[expressionFlags]
    ;

booleanExpressionPrimary [ExpressionFlags expressionFlags] returns [BooleanExpression vResult = null]
{
    ScalarExpression vExpressionFirst;
    bool vNotDefined = false;
    // These will always change however the compiler still compares, 
    // so default values are chosen for types.
    BooleanComparisonType vType = BooleanComparisonType.Equals;
}
    : 
        {IsNextRuleBooleanParenthesis()}?
        vResult=booleanExpressionParenthesis
    |   
        vExpressionFirst=expressionWithFlags[expressionFlags]
        (   
            vType=comparisonOperator
            (
                vResult=comparisonPredicate[vExpressionFirst, vType, expressionFlags]
            |   
                vResult=subqueryComparisonPredicate[vExpressionFirst, vType, expressionFlags]
            )
        |   
            vResult=joinPredicate[vExpressionFirst, vType, expressionFlags]
        | 
            vResult=isPredicate[vExpressionFirst]
        |
            ( 
                tNot:Not 
                {
                    vNotDefined = true;
                }
            )?
            (            
                vResult=inPredicate[vExpressionFirst, vNotDefined, expressionFlags]
            |   
                vResult=betweenPredicate[vExpressionFirst, vNotDefined, expressionFlags]
            |   
                vResult=likePredicate[vExpressionFirst, vNotDefined, expressionFlags]
            )
            {
                if (tNot != null)
                {
                    UpdateTokenInfo(vResult,tNot);
                }
            }
        )
    |
        vResult=fulltextPredicate
    |
        vResult=existsPredicate[expressionFlags]
    |
        vResult=tsEqualCall
    |
        vResult=updateCall
    ;      

tsEqualCall returns [TSEqualCall vResult = this.FragmentFactory.CreateFragment<TSEqualCall>()]
{
    ScalarExpression vExpression;
}
    :   tTSEqual:TSEqual LeftParenthesis vExpression=expression
        {
            UpdateTokenInfo(vResult,tTSEqual);
            vResult.FirstExpression = vExpression;
        }
        Comma vExpression=expression tRParen:RightParenthesis
        {
            vResult.SecondExpression = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

updateCall returns [UpdateCall vResult = this.FragmentFactory.CreateFragment<UpdateCall>()]
{
    Identifier vIdentifier;
}
    :
        tUpdate:Update LeftParenthesis vIdentifier=identifier tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tUpdate);
            vResult.Identifier = vIdentifier;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;


fulltextPredicate returns [FullTextPredicate vResult = this.FragmentFactory.CreateFragment<FullTextPredicate>()]
{
    ColumnReferenceExpression vColumn;
    ValueExpression vLiteral;
    ValueExpression vLanguageTerm;
}
    :
        (
            tContains:Contains
            {
                UpdateTokenInfo(vResult,tContains);
                vResult.FullTextFunctionType = FullTextFunctionType.Contains;
            }
        |
            tFreetext:FreeText
            {
                UpdateTokenInfo(vResult,tFreetext);
                vResult.FullTextFunctionType = FullTextFunctionType.FreeText;
            }
        )
        LeftParenthesis 
        (
            vColumn=fulltextColumn 
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
        |
            LeftParenthesis
            (
                (starColumn)=>
                vColumn=starColumn
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
                }
            |
                vColumn=column 
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
                }
                (
                    Comma vColumn=column 
                    {
                        AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
                    }
                )*
            )    
            RightParenthesis
        )
        Comma vLiteral=stringOrVariable
        {
            vResult.Value = vLiteral;
        }
        (    
            Comma vLanguageTerm=languageExpression
            {
                vResult.LanguageTerm = vLanguageTerm;
            }
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

fulltextColumn returns [ColumnReferenceExpression vResult = null]
    :
        (starColumn)=>
        vResult=starColumn
    |
        vResult=column
    ;

booleanExpressionParenthesis returns [BooleanParenthesisExpression vResult = this.FragmentFactory.CreateFragment<BooleanParenthesisExpression>()]
{
    BooleanExpression vExpression;
}
    :
        tLParen:LeftParenthesis vExpression=booleanExpression tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Expression = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

existsPredicate [ExpressionFlags expressionFlags] returns [ExistsPredicate vResult = this.FragmentFactory.CreateFragment<ExistsPredicate>()]
{
    ScalarSubquery vSubquery;
}    
    :
        Exists vSubquery=subquery[expressionFlags]
        {
            vResult.Subquery = vSubquery;
        }
    ;

joinPredicate[ScalarExpression vExpressionFirst, BooleanComparisonType vType, ExpressionFlags expressionFlags] returns [BooleanComparisonExpression vResult = this.FragmentFactory.CreateFragment<BooleanComparisonExpression>()]
{
    ScalarExpression vExpressionSecond;
}
    :
        vType=joinOperator vExpressionSecond=expression
        {
            vResult.ComparisonType = vType;
            vResult.FirstExpression = vExpressionFirst;
            vResult.SecondExpression = vExpressionSecond;
        }
    ;

comparisonPredicate[ScalarExpression vExpressionFirst, BooleanComparisonType vType, ExpressionFlags expressionFlags] returns [BooleanComparisonExpression vResult = this.FragmentFactory.CreateFragment<BooleanComparisonExpression>()]
{
    ScalarExpression vExpressionSecond;
}
    :
        vExpressionSecond=expressionWithFlags[expressionFlags]
        {
            vResult.ComparisonType = vType;
            vResult.FirstExpression = vExpressionFirst;
            vResult.SecondExpression = vExpressionSecond;
        }
    ;

subqueryComparisonPredicate[ScalarExpression vExpressionFirst, BooleanComparisonType vType, ExpressionFlags expressionFlags] returns [SubqueryComparisonPredicate vResult = this.FragmentFactory.CreateFragment<SubqueryComparisonPredicate>();]
{
    ScalarSubquery vSubquery;
    SubqueryComparisonPredicateType vSubqueryComparisonPredicateType = SubqueryComparisonPredicateType.None;
}
    :
        vSubqueryComparisonPredicateType=subqueryComparisonPredicateType
        vSubquery=subquery[expressionFlags]
        {
            vResult.ComparisonType = vType;
            vResult.Expression = vExpressionFirst;
            vResult.SubqueryComparisonPredicateType = vSubqueryComparisonPredicateType;
            vResult.Subquery = vSubquery;
        }
    ;

isPredicate[ScalarExpression vExpressionFirst] returns [BooleanIsNullExpression vResult = this.FragmentFactory.CreateFragment<BooleanIsNullExpression>()]
{
    bool vIsNull;
}
    : Is vIsNull = nullNotNull[vResult]
        {
            vResult.Expression = vExpressionFirst;
            vResult.IsNot = !vIsNull;
        }
    ;

inPredicate[ScalarExpression vExpressionFirst, bool vNotDefined, ExpressionFlags expressionFlags] returns [InPredicate vResult = this.FragmentFactory.CreateFragment<InPredicate>()]
{
    ScalarSubquery vSubquery;
}
    :   tIn:In 
        {
            if (vNotDefined)
                vResult.NotDefined = true;
            UpdateTokenInfo(vResult,tIn);
            vResult.Expression = vExpressionFirst;
        }
        (
            {IsNextRuleSelectParenthesis()}?
            vSubquery=subquery[expressionFlags]
            {
                vResult.Subquery = vSubquery;
            }
        |
            LeftParenthesis 
            expressionList[vResult, vResult.Values]
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )
    ;

betweenPredicate[ScalarExpression vExpressionFirst, bool vNotDefined, ExpressionFlags expressionFlags] returns [BooleanTernaryExpression vResult = this.FragmentFactory.CreateFragment<BooleanTernaryExpression>()]
{
    ScalarExpression vExpression;
}
    :
        tBetween:Between vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.SecondExpression = vExpression;
        }
        And vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.ThirdExpression = vExpression;

            if (vNotDefined)
            {
                vResult.TernaryExpressionType = BooleanTernaryExpressionType.NotBetween;
            } else
            {
                vResult.TernaryExpressionType = BooleanTernaryExpressionType.Between;
            }
            UpdateTokenInfo(vResult,tBetween);
            vResult.FirstExpression = vExpressionFirst;
        }
    ;

likePredicate[ScalarExpression vExpressionFirst, bool vNotDefined, ExpressionFlags expressionFlags] returns [LikePredicate vResult = this.FragmentFactory.CreateFragment<LikePredicate>()]
{
    ScalarExpression vExpression;
}
    :
        tLike:Like 
        {
            if (vNotDefined == true)
                {
                    vResult.NotDefined = true;
                }                
            UpdateTokenInfo(vResult,tLike);
            vResult.FirstExpression = vExpressionFirst;
        }
        vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.SecondExpression = vExpression;
        }
        (
            escapeExpression[vResult, expressionFlags]
        |
            LeftCurly
            {
                vResult.OdbcEscape = true;
            }
            escapeExpression[vResult, expressionFlags]
            tRCurly:RightCurly
            {
                UpdateTokenInfo(vResult,tRCurly);
            }
        )?
    ;

escapeExpression[LikePredicate vParent, ExpressionFlags expressionFlags]
{
    ScalarExpression vExpression;
}
    :
        Escape vExpression=expressionWithFlags[expressionFlags]
        {
            vParent.EscapeExpression = vExpression;
        }
    ;

comparisonOperator returns [BooleanComparisonType vResult = BooleanComparisonType.Equals]
    :
        EqualsSign
        {
            vResult = BooleanComparisonType.Equals;
        }
    |   GreaterThan 
        {
            vResult = BooleanComparisonType.GreaterThan;
        }
        ( 
            EqualsSign
            {
                vResult = BooleanComparisonType.GreaterThanOrEqualTo;
            }
        )?
    |   LessThan 
        {
            vResult = BooleanComparisonType.LessThan;
        }
        (
            EqualsSign
            {
                vResult = BooleanComparisonType.LessThanOrEqualTo;
            }
        |   GreaterThan
            {
                vResult = BooleanComparisonType.NotEqualToBrackets;
            }
        )?
    |   Bang
        (
            EqualsSign
            {
                vResult = BooleanComparisonType.NotEqualToExclamation;
            }
        |   LessThan
            {
                vResult = BooleanComparisonType.NotLessThan;
            }
        |   GreaterThan
            {
                vResult = BooleanComparisonType.NotGreaterThan;
            }
        )
    ;

joinOperator returns [BooleanComparisonType vResult = BooleanComparisonType.LeftOuterJoin]
    :
        MultiplyEquals
        {
            vResult = BooleanComparisonType.LeftOuterJoin;
        }
    |   RightOuterJoin
        {
            vResult = BooleanComparisonType.RightOuterJoin;
        }
    ;

subqueryComparisonPredicateType returns [SubqueryComparisonPredicateType vResult = SubqueryComparisonPredicateType.None]
    :
        All 
        {
            vResult = SubqueryComparisonPredicateType.All;
        }
    | 
        Any 
        {
            vResult = SubqueryComparisonPredicateType.Any;
        }
    | Some
        {
            vResult = SubqueryComparisonPredicateType.Any;
        }
    ;

expression returns [ScalarExpression vResult = null]
    : vResult=expressionWithFlags[ExpressionFlags.None]
    ;

expressionWithFlags [ExpressionFlags expressionFlags] returns [ScalarExpression vResult = null]
    : vResult=expressionBinaryPri2[expressionFlags]
    ;

expressionBinaryPri2 [ExpressionFlags expressionFlags] returns [ScalarExpression vResult = null]
{
    ScalarExpression vExpression;
}
    : vResult=expressionBinaryPri1[expressionFlags]
        ( 
            Plus vExpression=expressionBinaryPri1[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.Add);
            }
        |   Minus vExpression=expressionBinaryPri1[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.Subtract);
            }
        |   Ampersand vExpression=expressionBinaryPri1[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.BitwiseAnd);
            }
        |   VerticalLine vExpression=expressionBinaryPri1[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.BitwiseOr);
            }
        |   Circumflex vExpression=expressionBinaryPri1[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.BitwiseXor);
            }
        )*
    ;

expressionBinaryPri1 [ExpressionFlags expressionFlags] returns [ScalarExpression vResult = null]
{
    ScalarExpression vExpression;
}
    : vResult=expressionUnary[expressionFlags]
        ( 
            Star vExpression=expressionUnary[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.Multiply);
            }
        |   Divide vExpression=expressionUnary[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.Divide);
            }
        |   PercentSign vExpression=expressionUnary[expressionFlags]
            {
                AddBinaryExpression(ref vResult, vExpression, BinaryExpressionType.Modulo);
            }
        )*
    ;

expressionUnary [ExpressionFlags expressionFlags] returns [ScalarExpression vResult = null]
{
    ScalarExpression vExpression = null;
    UnaryExpression vUnaryExpression = null;
}
    :  
        (
            tPlus:Plus
            {
                vUnaryExpression = this.FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpression, tPlus);
                vUnaryExpression.UnaryExpressionType = UnaryExpressionType.Positive;
            }
        |   tMinus:Minus
            {
                vUnaryExpression = this.FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpression, tMinus);
                vUnaryExpression.UnaryExpressionType = UnaryExpressionType.Negative;
            }
        |   tTilde:Tilde
            {
                vUnaryExpression = this.FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpression, tTilde);
                vUnaryExpression.UnaryExpressionType = UnaryExpressionType.BitwiseNot;
            }
        )
        vExpression=expressionUnary[expressionFlags]
        {            
            vResult = vUnaryExpression;
            vUnaryExpression.Expression = vExpression;
        }
    |
        vResult=expressionPrimary[expressionFlags]
    ;

expressionPrimary [ExpressionFlags expressionFlags] returns [PrimaryExpression vResult = null]
    : 
        (
            odbcInitiator
        |
            {LA(1) == LeftCurly && NextTokenMatches(CodeGenerationSupporter.Fn, 2)}?
            vResult=odbcFunctionCall
        |
            vResult=literal
        | 
            {NextTokenMatches(CodeGenerationSupporter.Cast) && (LA(2) == LeftParenthesis)}?        
            vResult=castCall
        |
            (Identifier LeftParenthesis)=>
            vResult=identifierBuiltInFunctionCall
        |
            vResult = leftFunctionCall
        |
            vResult = rightFunctionCall
        |
            // Syntactic predicate does not give the required k==3 behavior so using semantic instead
            {((LA(1) == Identifier || LA(1) == QuotedIdentifier) && LA(2) == Dot && LA(3) == DollarPartition) || LA(1) == DollarPartition}?
            vResult=partitionFunctionCall
        |
            vResult=columnOrFunctionCall
        |
            vResult=nullIfExpression[expressionFlags]
        |
            vResult=coalesceExpression[expressionFlags]
        |
            vResult=caseExpression[expressionFlags]
        |   
            vResult=convertCall
        |
            vResult=parameterlessCall
        |
            vResult=paranthesisDisambiguatorForExpressions[expressionFlags]
        )
        collationOpt[vResult]
    ;

paranthesisDisambiguatorForExpressions [ExpressionFlags expressionFlags] returns [PrimaryExpression vResult = null]
    :
        {IsNextRuleSelectParenthesis()}?
        vResult=subquery[expressionFlags]
    | 
        vResult=expressionParenthesis[expressionFlags]
    ;

basicFunctionCall returns [FunctionCall vResult = this.FragmentFactory.CreateFragment<FunctionCall>()]
{
    Identifier vIdentifier;
}
    :   vIdentifier=identifier 
        {
            vResult.FunctionName = vIdentifier;
        }
        parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
    ;

expressionParenthesis [ExpressionFlags expressionFlags] returns [ParenthesisExpression vResult = this.FragmentFactory.CreateFragment<ParenthesisExpression>()]
{
    ScalarExpression vExpression;
}
    :    tLParen:LeftParenthesis vExpression=expressionWithFlags[expressionFlags] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Expression = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

convertCall returns [ConvertCall vResult = this.FragmentFactory.CreateFragment<ConvertCall>()]
{
    DataTypeReference vDataType;
    ScalarExpression vExpression;
}
    :
        tConvert:Convert LeftParenthesis vDataType=scalarDataType Comma vExpression=expression 
        {
            UpdateTokenInfo(vResult,tConvert);
            vResult.DataType = vDataType;
            vResult.Parameter = vExpression;
        }
        (
            Comma vExpression=expression
            {
                vResult.Style = vExpression;
            }
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

castCall returns [CastCall vResult = this.FragmentFactory.CreateFragment<CastCall>()]
{
    DataTypeReference vDataType;
    ScalarExpression vExpression;
}
    :
        tCast:Identifier LeftParenthesis vExpression=expression As vDataType=scalarDataType tRParen:RightParenthesis
        {
            Match(tCast, CodeGenerationSupporter.Cast);
            UpdateTokenInfo(vResult,tCast);
            vResult.DataType = vDataType;
            vResult.Parameter = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

parameterlessCall returns [ParameterlessCall vResult = this.FragmentFactory.CreateFragment<ParameterlessCall>()]
    :
        tUser:User
        {
            UpdateTokenInfo(vResult,tUser);
            vResult.ParameterlessCallType = ParameterlessCallType.User;
        }
    |
        tCurrentUser:CurrentUser
        {
            UpdateTokenInfo(vResult,tCurrentUser);
            vResult.ParameterlessCallType = ParameterlessCallType.CurrentUser;
        }
    |
        tSessionUser:SessionUser
        {
            UpdateTokenInfo(vResult,tSessionUser);
            vResult.ParameterlessCallType = ParameterlessCallType.SessionUser;
        }
    |
        tSystemUser:SystemUser
        {
            UpdateTokenInfo(vResult,tSystemUser);
            vResult.ParameterlessCallType = ParameterlessCallType.SystemUser;
        }
    |
        tCurrentTimestamp:CurrentTimestamp
        {
            UpdateTokenInfo(vResult,tCurrentTimestamp);
            vResult.ParameterlessCallType = ParameterlessCallType.CurrentTimestamp;
        }
    ;

identifierBuiltInFunctionCall returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
{
    Identifier vIdentifier;
}
    :    vIdentifier=nonQuotedIdentifier
        {
            vResult.FunctionName=vIdentifier;
        }
        LeftParenthesis 
        (
            identifierBuiltInFunctionCallDefaultParams[vResult]
        |
            identifierBuiltInFunctionCallUniqueRowFilter[vResult]
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

identifierBuiltInFunctionCallDefaultParams [FunctionCall vParent]
{
    ColumnReferenceExpression vColumn;
}
    :
        expressionList[vParent, vParent.Parameters]
    |
        vColumn=starColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vParent.Parameters, vColumn);
        }
    |   /* empty */
    ;

starColumnReferenceExpression returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
    : tStar:Star
      {
        vResult.ColumnType = ColumnType.Wildcard;
        UpdateTokenInfo(vResult, tStar);
      }
    ;

identifierBuiltInFunctionCallUniqueRowFilter [FunctionCall vParent]
{
    UniqueRowFilter vUniqueRowFilter;
    ScalarExpression vExpression;
}
    :    vUniqueRowFilter=uniqueRowFilter vExpression=expression
        {
            vParent.UniqueRowFilter = vUniqueRowFilter;
            AddAndUpdateTokenInfo(vParent, vParent.Parameters, vExpression);
        }
    ;

leftFunctionCall returns [LeftFunctionCall vResult = FragmentFactory.CreateFragment<LeftFunctionCall>()]
    :
        tLeft:Left
        {
            UpdateTokenInfo(vResult,tLeft);
        }
        reservedBuiltInFunctionCallParameters[vResult, vResult.Parameters]
    ;

rightFunctionCall returns [RightFunctionCall vResult = FragmentFactory.CreateFragment<RightFunctionCall>()]
    :
        tRight:Right
        {
            UpdateTokenInfo(vResult,tRight);
        }
        reservedBuiltInFunctionCallParameters[vResult, vResult.Parameters]
    ;

reservedBuiltInFunctionCallParameters [TSqlFragment vParent, IList<ScalarExpression> parameters]
    :
        LeftParenthesis
        (expressionList[vParent, parameters])?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

expressionWithDefault returns [ScalarExpression vResult]
    :   vResult=expression
    |   vResult=defaultLiteral
    ;

expressionWithDefaultList [TSqlFragment vParent, IList<ScalarExpression> expressions]
{
    ScalarExpression vExpression;
}
    :   vExpression=expressionWithDefault
        {
            AddAndUpdateTokenInfo(vParent, expressions, vExpression);
        }
        (
            Comma vExpression=expressionWithDefault
            {
                AddAndUpdateTokenInfo(vParent, expressions, vExpression);
            }
        )*
    ;

parenthesizedOptExpressionWithDefaultList [TSqlFragment vParent, IList<ScalarExpression> expressions]
    :
        LeftParenthesis
        (expressionWithDefaultList[vParent, expressions])?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

nullIfExpression [ExpressionFlags expressionFlags] returns [NullIfExpression vResult = this.FragmentFactory.CreateFragment<NullIfExpression>()]
{
    ScalarExpression vExpression;
}
    :
        tNullIf:NullIf
        {
            UpdateTokenInfo(vResult,tNullIf);
        }
        LeftParenthesis vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.FirstExpression = vExpression;
        }
        Comma vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.SecondExpression = vExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

coalesceExpression [ExpressionFlags expressionFlags] returns [CoalesceExpression vResult = this.FragmentFactory.CreateFragment<CoalesceExpression>()]
{
    ScalarExpression vExpression;
}
    :
        tCoalesce:Coalesce
        {
            UpdateTokenInfo(vResult,tCoalesce);
        }
        LeftParenthesis vExpression=expressionWithFlags[expressionFlags]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Expressions, vExpression);
        }
        (
            Comma
            vExpression=expressionWithFlags[expressionFlags]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Expressions, vExpression);
            }
        )+
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

simpleWhenClause [ExpressionFlags expressionFlags] returns [SimpleWhenClause vResult = this.FragmentFactory.CreateFragment<SimpleWhenClause>()]
{
    ScalarExpression vExpression = null;
}
    :
        tWhen:When
        {
            UpdateTokenInfo(vResult,tWhen);
        }
        vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.WhenExpression = vExpression;
        }
        Then vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.ThenExpression  = vExpression;
        }
    ;

searchedWhenClause [ExpressionFlags expressionFlags] returns [SearchedWhenClause vResult = this.FragmentFactory.CreateFragment<SearchedWhenClause>()]
{
    BooleanExpression vExpression = null;
       ScalarExpression vThenExpression = null;
}
    :
        tWhen:When
        {
            UpdateTokenInfo(vResult,tWhen);
        }
        vExpression=booleanExpressionWithFlags[expressionFlags]
        {
            vResult.WhenExpression = vExpression;
        }
        Then vThenExpression=expressionWithFlags[expressionFlags]
        {
            vResult.ThenExpression  = vThenExpression;
        }
    ;

caseExpression [ExpressionFlags expressionFlags] returns [CaseExpression vResult ]
{
    ScalarExpression vExpression = null;
}
    :
        tCase:Case
        (
            vExpression=expressionWithFlags[expressionFlags]
            vResult = simpleCaseExpression[vExpression, expressionFlags]
        |
             vResult = searchedCaseExpression[expressionFlags]
        )
        (
            Else vExpression=expressionWithFlags[expressionFlags]
            {
                vResult.ElseExpression = vExpression;
            }
        )?
        tEnd:End
        {
            UpdateTokenInfo(vResult,tCase);
            UpdateTokenInfo(vResult,tEnd);
        }
    ;

simpleCaseExpression[ScalarExpression inputExpression, ExpressionFlags expressionFlags] returns [SimpleCaseExpression vResult = this.FragmentFactory.CreateFragment<SimpleCaseExpression>()]
{
    SimpleWhenClause vWhenClause;
    vResult.InputExpression = inputExpression;
}
    :
        ( 
            vWhenClause=simpleWhenClause[expressionFlags]
            {
                AddAndUpdateTokenInfo(vResult, vResult.WhenClauses, vWhenClause);
            }
        )+
    ;

searchedCaseExpression [ExpressionFlags expressionFlags] returns [SearchedCaseExpression vResult = this.FragmentFactory.CreateFragment<SearchedCaseExpression>()]
{
    SearchedWhenClause vWhenClause;
}
    :
        ( 
            vWhenClause=searchedWhenClause[expressionFlags]
            {
                AddAndUpdateTokenInfo(vResult, vResult.WhenClauses, vWhenClause);
            }
        )+
    ;

specialColumn[ColumnReferenceExpression vResult]
    : 
        tIdentityCol:IdentityColumn
        {
            UpdateTokenInfo(vResult,tIdentityCol);
            vResult.ColumnType = ColumnType.IdentityCol;
        }
    | 
        tRowguidCol:RowGuidColumn
        {
            UpdateTokenInfo(vResult,tRowguidCol);
            vResult.ColumnType = ColumnType.RowGuidCol;
        }
    |
        tPseudoColumn:PseudoColumn
        {
            UpdateTokenInfo(vResult,tPseudoColumn);
            vResult.ColumnType = PseudoColumnHelper.Instance.ParseOption(tPseudoColumn, SqlVersionFlags.TSql80);
        }
    ;

// This rule is not removed because it is used in fulltextColumn and readTextStatement.
column returns [ColumnReferenceExpression vResult = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
    vResult.ColumnType = ColumnType.Regular;
}
    :
        (
            // Caveat: This would have a problem with ...IdentityColumn
            // however it works because table name always has to be 
            // written. 
            vMultiPartIdentifier=multiPartIdentifier[-1]
            {
                vResult.MultiPartIdentifier=vMultiPartIdentifier;
            }
            (
                Dot specialColumn[vResult]
            )?
        | 
            specialColumn[vResult]
        )
        { 
            CheckSpecialColumn(vResult);
            CheckTableNameExistsForColumn(vResult, false);
        }        
    ;

partitionFunctionCall returns [PartitionFunctionCall vResult = this.FragmentFactory.CreateFragment<PartitionFunctionCall>()]
{
    Identifier vIdentifier;
}
    :
        (
            vIdentifier=identifier
            {
                vResult.DatabaseName = vIdentifier;
            }
            Dot
        )?
        tDollarPartition:DollarPartition
        {
            UpdateTokenInfo(vResult,tDollarPartition);
        }
        Dot vIdentifier=identifier
        {
            vResult.FunctionName = vIdentifier;
        }
        LeftParenthesis 
        expressionList[vResult, vResult.Parameters]
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

userFunctionCall[MultiPartIdentifier vIdentifiers] returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
    :
        LeftParenthesis
        (
            (expressionWithDefaultList[vResult, vResult.Parameters])?
        |
            identifierBuiltInFunctionCallUniqueRowFilter[vResult]
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
            PutIdentifiersIntoFunctionCall(vResult, vIdentifiers);
        }      
    ;

columnOrFunctionCall returns [PrimaryExpression vResult = null]
{
    MultiPartIdentifier vMultiPartIdentifier = null;
    ColumnReferenceExpression vColumn = null;
}
    :
        (
            // Caveat: This would have a problem with ...IdentityColumn
            // however it works because table name always has to be 
            // written. 
            vMultiPartIdentifier=multiPartIdentifier[-1] 
            (
                // This is ambiguous because of the select that is a statement
                // starter with a LeftParenthesis. For example ( select * from t1) is a statement.
                // Actually this greedy is false, it should be the opposite however,
                // a column without a from clause is a semantic error anyways!  So we should
                // be ok, in order to fix this in syntactic time, we have to write a C# function 
                // that looks at the tokens ahead and return false if it identifies a select 
                // statement.
                // Greedy is for the second alternative not the first one.
                options {greedy=true;} : 
                {
                    vColumn = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>();
                }
                Dot specialColumn[vColumn]
            |
                vResult=userFunctionCall[vMultiPartIdentifier]
            |
                /* empty */
            )
        | 
            {
                vColumn = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>();
            }
            specialColumn[vColumn]
        )
        { 
            if (vResult == null || vResult is ColumnReferenceExpression)
            {
                // In the empty case above vColumn might be null. I try to put any code there, 
                // there will be a ambiguity warning.                
                if (vColumn == null)
                {
                    vColumn = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>();
                }                
                vColumn.MultiPartIdentifier=vMultiPartIdentifier;
                CheckSpecialColumn(vColumn);
                CheckTableNameExistsForColumn(vColumn, false);
                vResult = vColumn;
            }
        }        
    ;

starColumn returns [ColumnReferenceExpression vResult = this.FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    vResult.ColumnType = ColumnType.Wildcard;
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        (
            // Caveat: This would have a problem with ...IdentityColumn
            // however it works because table name always has to be 
            // written. 
            vMultiPartIdentifier=multiPartIdentifier[-1]
            {
                vResult.MultiPartIdentifier = vMultiPartIdentifier;
            }
            Dot tStar1:Star
            {
                UpdateTokenInfo(vResult,tStar1);
                vResult.ColumnType = ColumnType.Wildcard;
            }
        | 
            tStar2:Star
            {
                UpdateTokenInfo(vResult,tStar2);
                vResult.ColumnType = ColumnType.Wildcard;
            }
        )
        { 
            CheckSpecialColumn(vResult);
            CheckTableNameExistsForColumn(vResult, false);
        }        
    ;
    
revertStatement returns [RevertStatement vResult = FragmentFactory.CreateFragment<RevertStatement>()]
{
    ScalarExpression vExpression;
}
    : tRevert:Revert 
        {
            UpdateTokenInfo(vResult,tRevert);
        }
        (options { greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            With tCookie:Identifier EqualsSign vExpression = expression
            {
                Match(tCookie,CodeGenerationSupporter.Cookie);
                vResult.Cookie = vExpression;
            }
        )?
    ;

diskStatement returns [DiskStatement vResult = FragmentFactory.CreateFragment<DiskStatement>()]
{
    DiskStatementOption vOption;
}
    : tDisk:Disk tIdentifier:Identifier
        {
            if(TryMatch(tIdentifier, CodeGenerationSupporter.Init))
            {
                vResult.DiskStatementType=DiskStatementType.Init;
            }
            else
            {
                Match(tIdentifier, CodeGenerationSupporter.Resize);
                vResult.DiskStatementType=DiskStatementType.Resize;
            }
            UpdateTokenInfo(vResult, tDisk);
        }
        vOption=diskStatementOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
        (
            Comma vOption=diskStatementOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )*
    ;

diskStatementOption returns[DiskStatementOption vResult = FragmentFactory.CreateFragment<DiskStatementOption>()]
{
    IdentifierOrValueExpression vValue;
}
    : tIdentifier:Identifier EqualsSign vValue=identifierOrValueExpression
      {
        vResult.OptionKind=DiskStatementOptionsHelper.Instance.ParseOption(tIdentifier);
        vResult.Value=vValue;
      }
    ;

///////////////////////////////////////////////////////////////////////////////////////////
// Simple utility stuff
///////////////////////////////////////////////////////////////////////////////////////////
nullNotNull [TSqlFragment vParent] returns [bool vIsNull = true]
    :   (tNot:Not 
            {
                vIsNull = false;
                UpdateTokenInfo(vParent, tNot);
            }
        )? 
        tNull:Null
          {
            UpdateTokenInfo(vParent,tNull);
        }
    ;

orderByOption [TSqlFragment vParent] returns [SortOrder vResult = SortOrder.NotSpecified]
    : tAsc:Asc 
        {
            UpdateTokenInfo(vParent,tAsc);
            vResult = SortOrder.Ascending;
        }
    | tDesc:Desc 
        {
            UpdateTokenInfo(vParent,tDesc);
            vResult = SortOrder.Descending;
        }
    ;

signedIntegerOrVariable returns [ScalarExpression vResult]
    : vResult=signedInteger
    | vResult=variable
    ;

signedIntegerOrStringOrVariable returns [ScalarExpression vResult]
    : vResult=signedInteger
    | vResult=stringOrVariable
    ;

signedIntegerOrVariableOrNull returns [ScalarExpression vResult]
    : vResult=signedIntegerOrVariable
    | vResult=nullLiteral
    ;

signedInteger returns [ScalarExpression vResult = null]
{
    Literal vLiteral;
    UnaryExpression vUnaryExpr = null;
}
    : 
        (
            tMinus:Minus 
            {
                vUnaryExpr = this.FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpr, tMinus);
                vUnaryExpr.UnaryExpressionType = UnaryExpressionType.Negative;
            }
        )?
            
        vLiteral=integer
        {            
            if (vUnaryExpr == null)
                vResult = vLiteral;
            else
            {
                vUnaryExpr.Expression = vLiteral;
                vResult = vUnaryExpr;
            }
        }
    ;
    
identifierOrVariable returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]    
{
    ValueExpression vVariable;
    Identifier vIdentifier;
}
    : vVariable = variable
      {
        vResult.ValueExpression=vVariable;
      }
    | vIdentifier = identifier
      {
        vResult.Identifier = vIdentifier;
      }
    ;

stringOrVariable returns [ValueExpression vResult = null]
    : vResult = stringLiteral
    | vResult = variable
    ;

stringOrGlobalVariableOrVariable returns [ValueExpression vResult = null]
    : vResult = stringLiteral
    | vResult = globalVariableOrVariableReference
    ;

authorizationOpt [IAuthorization vParent]
    : (authorization[vParent])?
    ;
    
authorization [IAuthorization vParent]
{
    Identifier vOwner;
}
    : Authorization vOwner = identifier
        {
            vParent.Owner = vOwner;
        }
    ;

collationOpt [ICollationSetter vParent]
    : /* empty */
    | collation[vParent]
    ;
    
collation [ICollationSetter vParent]
{
    Identifier vIdentifier;
}
    : Collate vIdentifier = nonQuotedIdentifier
        {
            vParent.Collation = vIdentifier;
        }
    ;

objectOrString returns [SchemaObjectNameOrValueExpression vResult = this.FragmentFactory.CreateFragment<SchemaObjectNameOrValueExpression>()]
{
    SchemaObjectName vSchemaObjectName;
    Literal vString;
}
    : vSchemaObjectName = schemaObjectThreePartName 
      {
        vResult.SchemaObjectName = vSchemaObjectName;
      }
    | vString = stringLiteral
      {
        vResult.ValueExpression=vString;
      }
    ;    

// Regularly the parser variable rule binds to a Literal AST
// However an identifier is more appropriate in some places, so we
// directly use the Variable token not the variable rule and
// create the Identifier AST from the token.
identifierVariable returns [Identifier vResult = this.FragmentFactory.CreateFragment<Identifier>()]
    : tId:Variable 
        {
            UpdateTokenInfo(vResult,tId);
            vResult.SetIdentifier(tId.getText());
        }
    ;

identifierOrInteger returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]
{
    Identifier vIdentifier;
    Literal vLiteral;
}
    :   vIdentifier=identifier
        {
            vResult.Identifier=vIdentifier;
        }
    |   vLiteral=integer
        {
            vResult.ValueExpression = vLiteral;
        }
    ;

identifierOrValueExpression returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]
{
    Identifier vIdentifier;
    ValueExpression vLiteral;
}
    :   vIdentifier=identifier
        {
            vResult.Identifier=vIdentifier;
        }
    |   vLiteral=literal
        {
            vResult.ValueExpression = vLiteral;
        }
    ;

stringOrIdentifier returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]
{
    Identifier vIdentifier;
    Literal vLiteral;
}
    :    vLiteral=stringLiteral
        {
            vResult.ValueExpression = vLiteral;
        }
    |    vIdentifier=identifier
        {
            vResult.Identifier=vIdentifier;
        }
    ;
        
nonEmptyStringOrIdentifier returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]
{
    Identifier vIdentifier;
    Literal vLiteral;
}
    :   vLiteral=nonEmptyString
        {
            vResult.ValueExpression = vLiteral;
        }
    |   vIdentifier=identifier
        {
            vResult.Identifier=vIdentifier;
        }
    ;

nonQuotedIdentifier returns [Identifier vResult = this.FragmentFactory.CreateFragment<Identifier>()]
    : 
        tId:Identifier 
        {
            UpdateTokenInfo(vResult,tId);
            vResult.SetUnquotedIdentifier(tId.getText());
        }
    ;

sqlCommandIdentifier returns [SqlCommandIdentifier vResult = this.FragmentFactory.CreateFragment<SqlCommandIdentifier>()]
    : 
        tId:SqlCommandIdentifier 
        {
            UpdateTokenInfo(vResult,tId);
            vResult.SetUnquotedIdentifier(tId.getText());
        }
    ;

identifier returns [Identifier vResult = this.FragmentFactory.CreateFragment<Identifier>()]
    : 
        tId:Identifier 
        {
            UpdateTokenInfo(vResult,tId);
            vResult.SetUnquotedIdentifier(tId.getText());
            CheckIdentifierLength(vResult);
        }
    |
        tId2:QuotedIdentifier 
        {
            UpdateTokenInfo(vResult,tId2);
            vResult.SetIdentifier(tId2.getText());
            CheckIdentifierLength(vResult);
        }
    ;

seedIncrement returns [ScalarExpression vResult = null]
{
    Literal vLiteral;
    UnaryExpression vUnaryExpression = null;
}
    : 
        (
            tMinus:Minus 
            {
                vUnaryExpression = FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpression,tMinus);
                vUnaryExpression.UnaryExpressionType = UnaryExpressionType.Negative;
            }
        |
            tPlus:Plus
            {
                vUnaryExpression = FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(vUnaryExpression,tPlus);
                vUnaryExpression.UnaryExpressionType = UnaryExpressionType.Positive;
           }
        )?
        vLiteral=integerOrNumeric
        {            
            if (vUnaryExpression != null)
            {
                vUnaryExpression.Expression = vLiteral;
                vResult = vUnaryExpression;
            }
            else
                vResult = vLiteral;
        }
    ;

subroutineParameterLiteral returns [Literal vResult]
    : vResult = integer
    | vResult = real
    | vResult = numeric
    | vResult = moneyLiteral
    ;

binaryOrIntegerOrStringOrVariable returns [ValueExpression vResult]
    :   
        vResult=binary
    |
        vResult=stringLiteral
    |
        vResult=integer
    |
        vResult=variable
    ;

integerOrNumeric returns [Literal vResult]
    : 
        vResult=integer
    |    vResult=numeric
    ;

literal returns [ValueExpression vResult]
    : vResult = integer
    | vResult = real
    | vResult = numeric
    | vResult = moneyLiteral
    | vResult = binary
    | vResult = stringLiteral
    | vResult = nullLiteral
    | vResult = globalVariableOrVariableReference
    | vResult = odbcLiteral
    ;

globalVariableOrVariableReference returns [ValueExpression vResult = null]
{
    VariableReference vVariable;
    GlobalVariableExpression vGlobalVariableExpression;
}
    : tVariable:Variable
        {
            if(tVariable.getText().StartsWith("@@", StringComparison.Ordinal))
            {
                vGlobalVariableExpression = FragmentFactory.CreateFragment<GlobalVariableExpression>();
                vGlobalVariableExpression.Name = tVariable.getText();
                vResult = vGlobalVariableExpression;
            }    
            else
            {
                vVariable = FragmentFactory.CreateFragment<VariableReference>();
                vVariable.Name = tVariable.getText();
                vResult = vVariable;
            }
            UpdateTokenInfo(vResult,tVariable);
        }
    ;

variable returns [VariableReference vResult = this.FragmentFactory.CreateFragment<VariableReference>()]
    : tVariable:Variable
        {
            UpdateTokenInfo(vResult,tVariable);
            vResult.Name = tVariable.getText();
        }
    ;

integer returns [IntegerLiteral vResult = this.FragmentFactory.CreateFragment<IntegerLiteral>()]
    : tInt:Integer
        {
            UpdateTokenInfo(vResult,tInt);
            vResult.Value = tInt.getText();
        }
    ;

numeric returns [NumericLiteral vResult = this.FragmentFactory.CreateFragment<NumericLiteral>()]
    : tNumeric:Numeric
        {
            UpdateTokenInfo(vResult, tNumeric);
            vResult.Value = tNumeric.getText();
        }
    ;

real returns [RealLiteral vResult = this.FragmentFactory.CreateFragment<RealLiteral>()]
    : tReal:Real
        {
            UpdateTokenInfo(vResult,tReal);
            vResult.Value = tReal.getText();
        }
    ;

nonEmptyString returns [StringLiteral vResult]
    : vResult = stringLiteral
        {
            if (vResult.Value == null || vResult.Value.Length == 0)
            {
                ThrowParseErrorException("SQL46063", vResult, 
                    TSqlParserResource.SQL46063Message);
            }
        }
    ;

stringLiteral returns [StringLiteral vResult = this.FragmentFactory.CreateFragment<StringLiteral>()]
    : tAsciiStringLiteral:AsciiStringLiteral
        {
            UpdateTokenInfo(vResult,tAsciiStringLiteral);
            vResult.Value = DecodeAsciiStringLiteral(tAsciiStringLiteral.getText());
            vResult.IsLargeObject=IsAsciiStringLob(vResult.Value);
        }
    | tUnicodeStringLiteral:UnicodeStringLiteral
        {
            UpdateTokenInfo(vResult,tUnicodeStringLiteral);
            vResult.IsNational=true;
            vResult.Value = DecodeUnicodeStringLiteral(tUnicodeStringLiteral.getText());
            vResult.IsLargeObject=IsUnicodeStringLob(vResult.Value);
        }
    ;

binary returns [BinaryLiteral vResult = this.FragmentFactory.CreateFragment<BinaryLiteral>()]
    :   
        tHexLiteral:HexLiteral
        {
            UpdateTokenInfo(vResult,tHexLiteral);
            vResult.Value = tHexLiteral.getText();
            vResult.IsLargeObject=IsBinaryLiteralLob(vResult.Value);
        }
    ;

nullLiteral returns [NullLiteral vResult = this.FragmentFactory.CreateFragment<NullLiteral>()]
    : tNull:Null
        {
            UpdateTokenInfo(vResult,tNull);
            vResult.Value = tNull.getText();
        }
    ;

moneyLiteral returns [MoneyLiteral vResult = this.FragmentFactory.CreateFragment<MoneyLiteral>()]
    : tMoney:Money
        {
            UpdateTokenInfo(vResult,tMoney);
            vResult.Value = tMoney.getText();
        }
    ;
    
odbcLiteral returns [OdbcLiteral vResult = this.FragmentFactory.CreateFragment<OdbcLiteral>()]
    : tLCurly:LeftCurly
        {
            UpdateTokenInfo(vResult,tLCurly);
        }
        tIdentifier:Identifier
        (
            tAsciiStringLiteral2:AsciiStringLiteral
            {
                vResult.OdbcLiteralType = ParseOdbcLiteralType(tIdentifier);
                UpdateTokenInfo(vResult,tAsciiStringLiteral2);
                vResult.Value = DecodeAsciiStringLiteral(tAsciiStringLiteral2.getText());
            }
        |
            tUnicodeStringLiteral2:UnicodeStringLiteral
            {
                vResult.OdbcLiteralType = ParseOdbcLiteralType(tIdentifier);
                vResult.IsNational=true;
                UpdateTokenInfo(vResult,tUnicodeStringLiteral2);
                vResult.Value = DecodeUnicodeStringLiteral(tUnicodeStringLiteral2.getText());
            }
        )
        tRCurly:RightCurly
        {
            UpdateTokenInfo(vResult,tRCurly);
        }
    ;

defaultLiteral returns [DefaultLiteral vResult = this.FragmentFactory.CreateFragment<DefaultLiteral>()]
    : tDefault:Default
        {
            UpdateTokenInfo(vResult,tDefault);
            vResult.Value = tDefault.getText();
        }
    ;

///////////////////////////////////////////////////////////////////////////////////////////
// End of simple utility stuff
///////////////////////////////////////////////////////////////////////////////////////////

{
    #pragma warning disable 618, 219
}

class TSql80LexerInternal extends Lexer("TSqlLexerBaseInternal");

options {
    k = 2;
    charVocabulary = '\u0000'..'\uFFFF';
    testLiterals = false;
    caseSensitive = false;
    caseSensitiveLiterals = false;
    classHeaderPrefix = "internal partial";
    importVocab = TSql;
}

tokens {
        // Version-specific keywords
    // TSql 80
    Disk = "disk";
    Precision = "precision";

    Dump = "dump";
    Load = "load";
}

{
    public TSql80LexerInternal() 
        : this(new System.IO.StringReader(String.Empty))
    {
    }
}

Bang            : '!' ;
PercentSign        : '%' ;
Ampersand        : '&' ;
LeftParenthesis           : '(' ;
RightParenthesis           : ')' ;
LeftCurly           : '{' ;
RightCurly           : '}' ;
Star             : '*' ;
MultiplyEquals                : "*=";
Plus            : '+' ;
Comma            : ',' ;
Minus            : '-' ;
protected // see Number productions
Dot                :     ;
Divide            : '/' ;
Colon            : ':' ;
DoubleColon        : "::";
LessThan        : '<' ;
EqualsSign        : '=' ;
RightOuterJoin    : "=*";
GreaterThan        : '>' ;
Circumflex        : '^' ;
VerticalLine    : '|' ;
Tilde            : '~' ;

protected
Semicolon        : ';' ;

protected
Digit
    :    '0'..'9'
    ;

protected
FirstLetter
    :    'a'..'z' | '_' | '#' | '\u0080'..'\ufffe' 
    ;

protected
Letter
    :    'a'..'z' | '_' | '#' | '@' | '$' | '\u0080'..'\ufffe'
    ;

protected
MoneySign
    :    '\u0024' // Dollar sign
    |    '\u00A3' // Pound sign
    |    '\u00A4' // Currency sign
    |    '\u00A5' // Yen sign
    |    '\u09F2' // Bengali Rupee mark
    |    '\u09F3' // Bengali Rupee sign
    |    '\u0E3F' // Thai Baht symbol
    |    '\u20AC' // Euro-currency Sign
    |    '\u20A1' // Colon sign
    |    '\u20A2' // Cruzeiro sign
    |    '\u20A3' // French Franc sign
    |    '\u20A4' // Lira sign
    |    '\u20A6' // Naira sign
    |    '\u20A7' // Peseta sign
    |    '\u20A8' // Rupee sign
    |    '\u20A9' // Won sign
    |    '\u20AA' // New Sheqel sign
    |    '\u20AB' // Dong sign
    ;

ProcNameSemicolon
    :
        (Semicolon (WS_CHAR_WO_NEWLINE)* Number)=>
        Semicolon
    |
        Semicolon
        {
            $setType(Semicolon); 
        }
    ;
    
protected
WS_CHAR_WO_NEWLINE
    :
        (    
            ' '
        |    '\u0001'
        |    '\u0002'
        |    '\u0003'
        |    '\u0004'    // end of transmission (yes, some ... use this?)
        |    '\u0005'
        |    '\u0006'
        |    '\u0007'
        |    '\u0008'
        |    '\u0009'
                        // 0x0a LF
        |    '\u000b'     // vertical tab
        |    '\u000c'
                        // 0x0d CR
        |    '\u000e'
        |    '\u000f'    // for people who want to use \n, but do not know the difference between 15 and 015 !!!
        |    '\u0010'
        |    '\u0011'
        |    '\u0012'     // device control, but also a common mistake for \n in hex :)
        |    '\u0013'
        |    '\u0014'
        |    '\u0015'
        |    '\u0016'
        |    '\u0017'
        |    '\u0018'
        |    '\u0019'
        |    '\u001a'
        |    '\u001b'
        |    '\u001c'
        |    '\u001d'
        |    '\u001e'
        |    '\u001f'
        )
    ;

// If there is only white space since the last newline, then update the _acceptableGoOffset 
// so that Go is parsed when it is not only the first column, but also if there is only 
// white space between the first column and "go".
WhiteSpace
{ 
    bool resetAcceptable = (CurrentOffset == _acceptableGoOffset);
}
    :    
        (
            WS_CHAR_WO_NEWLINE
            { 
                // Propagate the _acceptableGoOffset so that we can ignore the ws after newline for Go
                if (resetAcceptable) 
                    _acceptableGoOffset = CurrentOffset;
            }
        )+
    |
        EndOfLine
        {
            _acceptableGoOffset = CurrentOffset;
        }
    ;

Go
options {testLiterals=true;}
    :    { CurrentOffset==_acceptableGoOffset }? 
        "go"    
        (
            (
                Letter 
                { $setType(Identifier); }
            | 
                Digit
                { $setType(Identifier); }
            )*            
            ( 
                (Colon ~':')=>
                Colon
                {
                    $setType(Label); 
                }
            )?
        )
    ;

protected
Label :;

protected
Integer :;

protected
Numeric :;

protected
Real :;

protected
HexLiteral
    :;

Number
    : (Digit)+ 
        { 
            if(IsValueTooLargeForTokenInteger(text.ToString()))
            {
                $setType(Numeric);
            }
            else
            {
                $setType(Integer); 
            }

        }
           ('.' (Digit)*
            { 
                $setType(Numeric); 
            }
        )?
        (Exponent 
            { 
                $setType(Real); 
            }
        )?
    |    '.' 
        { 
            $setType(Dot); 
        }
           ( (Digit)+
            { 
                $setType(Numeric); 
            } 
            (Exponent
                {
                    $setType(Real);
                }
            )?
        )?
    |    "0x"    
        ( Digit 
        | 'a'..'f' 
        | '\\' EndOfLine    
        )* 
        { 
            $setType(HexLiteral); 
        } // "0x" is valid hex literal
    ;

protected
Exponent
    :    'e' ( '+' | '-' )? (Digit)*
    ;
    
protected
EndOfLine
    /*    '\r' '\n' can be matched in one alternative or by matching
        '\r' in one iteration and '\n' in another.  I am trying to
        handle any flavor of newline that comes in, but the language
        that allows both "\r\n" and "\r" and "\n" to all be valid
        newline is ambiguous.  Consequently, the resulting grammar
        must be ambiguous.  I'm shutting this warning off.
     */
    :   
        (options { generateAmbigWarnings=false; } :
            "\r\n"    // DOS
        |    '\r'    // Unix    
        |    '\n'    // Macintosh    
        )
        {
            newline();
        } 
    ;

protected
Money :;

protected
SqlCommandIdentifier :;

protected
PseudoColumn :;

protected
DollarPartition :;

protected
AsciiStringOrQuotedIdentifier :;

AsciiStringLiteral
    :    { beginComplexToken(); } '\'' 
        (
            { checkEOF(TokenKind.String); } 
            ~('\'' | '\n' | '\r')
        | 
            EndOfLine
        |
            '\'' '\''
        )*
        '\''
    ;

UnicodeStringLiteral
    :    { beginComplexToken(); } 'n' '\''
        (
            { checkEOF(TokenKind.String); } 
            ~('\'' | '\n' | '\r') 
        | 
            EndOfLine
        | 
            '\'' '\''
        )*
        '\''
    ;

Identifier
options {testLiterals=true;} // For some reason space ' ' after money sign is ok...
    :    
        ('$' LeftParenthesis) => 
        { beginComplexToken(); } '$' LeftParenthesis
        (
            { checkEOF(TokenKind.SqlCommandIdentifier); }
            ~(')' | '\n' | '\r')
        |
            EndOfLine
        )+ 
        RightParenthesis 
        { 
            $setType(SqlCommandIdentifier); 
        }
    |    
        ('$' ('@' | FirstLetter)) => 
        '$' ('@' | FirstLetter) (Letter | Digit)*
        {
            if (String.Equals(text.ToString(), CodeGenerationSupporter.DollarPartition, 
                              StringComparison.OrdinalIgnoreCase))
            {
                $setType(DollarPartition);
            }
            else
            {
                $setType(PseudoColumn);
            }
        }
    |    
        (MoneySign) =>  MoneySign (' ')* (Minus | Plus)? (Digit)+ ( '.' (Digit)* (Exponent)? | Exponent)?
        { 
            $setType(Money); 
        }
    |    
        FirstLetter (Letter | Digit)* 
        ( 
            (Colon ~':')=>
            Colon
            {
                $setType(Label);
            }
        |
            /* empty */
        )
    ;

QuotedIdentifier
    :    { beginComplexToken(); } '['
        (
            { checkEOF(TokenKind.QuotedIdentifier); } 
            ~(']' | '\n' | '\r')
        |
            EndOfLine            
        | 
            ']' ']'
        )+
        ']'
    |    { beginComplexToken(); } '\"'
        (
            (   
                { checkEOF(TokenKind.QuotedIdentifier); } 
                ~ ('\"' | '\n' | '\r')
            |
                EndOfLine
            | 
                '\"' '\"'
            )*
            {
                if (text.Length > 1) // To account for the opening "
                {
                    $setType(AsciiStringOrQuotedIdentifier);
                }
                else
                {
                    $setType(AsciiStringLiteral);
                }
            }
        )
        '\"'
    ;

Variable
    :    '@' (Letter | Digit)*
    ;

protected
OdbcInitiator :;

SingleLineComment
    :    "--"
        (
            ('(' '*')=>
            '(' '*'
            {
                $setType(OdbcInitiator);
            }
        |
            (
                { LA(1) != EOF_CHAR }?
                ~('\r' | '\n')               
            )*
            { 
                _acceptableGoOffset = CurrentOffset;
            }
        )
    ;

MultilineComment
{ 
    bool resetAcceptable = (CurrentOffset == _acceptableGoOffset);
}
    :    { beginComplexToken(); } 
        "/*"
        (
            // For some reason, ANTLR complains about ambiguity between rules with '*', even with predicate...
            options { generateAmbigWarnings=false; } :
            EndOfLine
        |    { LA(2)!='/' }? '*'
        |   { LA(2)!='*' }? '/'    
        |    
            { checkEOF(TokenKind.MultiLineComment); }
            ~('*' | '\n' | '\r' | '/')
        |   MultilineComment // embedded comments are allowed.
        )*
        "*/"
        {
            if (resetAcceptable) 
                _acceptableGoOffset = CurrentOffset;
        }
    ;
