//------------------------------------------------------------------------------
// <copyright file="TSql120.g" company="Microsoft">
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

class TSql120ParserInternal extends Parser("TSql120ParserBaseInternal");
options {
    k = 2;
    defaultErrorHandler=false;
    classHeaderPrefix = "internal partial";
    importVocab = TSql;
}

{
    public TSql120ParserInternal(bool initialQuotedIdentifiersOn) 
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
{
    SelectFunctionReturnType vRetType;
}
    :
        vRetType = functionReturnClauseRelational
        {
            vResult = vRetType.SelectStatement;
        }
        EOF
    ;
    
entryPointIPv4Address returns [IPv4 vResult = null]
    :
        vResult = ipAddressV4
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
    : (tSemi:Semicolon)*
      (        
        (
           Create ( Proc | Procedure | Trigger | Default | Rule | View | Function | Schema | {NextTokenMatches(CodeGenerationSupporter.Federation)}? )
         |
           Alter ( Proc | Procedure | Trigger | View | Function | {NextTokenMatches(CodeGenerationSupporter.Federation)}? )
         | 
           Use {NextTokenMatches(CodeGenerationSupporter.Federation) && LA(2) == Identifier}?
        )=>
        ( 
            vStatement=lastStatementOptSemi
            {
                if (vStatement != null)
                {    
                    if (vResult == null)
                    {
                        vResult = this.FragmentFactory.CreateFragment<TSqlBatch>();
                    }
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
    : vResult=statement optSemicolons[vResult]
    ;
    
lastStatementOptSemi returns [TSqlStatement vResult = null]
    : vResult=lastStatement optSemicolons[vResult]
    ;
    
optSemicolons[TSqlStatement vParent]
{
    int nSemicolons = 0;
}
    : (
            // Greedy behavior is good enough, we ignore the semicolons
            options {greedy = true; } :
            tSemi:Semicolon
        {
            ++nSemicolons;
            if (vParent != null) // vResult can be null if there was a parse error.
                UpdateTokenInfo(vParent,tSemi);
        }
      )*
    ;

// This rule conflicts with identifierStatements (both can start with Identifier)
// We should update predicates here and in identifierStatements at the same time
optSimpleExecute returns [ExecuteStatement vResult = null]
{
    ExecutableProcedureReference vExecProc;
    ExecuteSpecification vExecuteSpecification;
}
    : {!NextTokenMatches(CodeGenerationSupporter.Disable) && !NextTokenMatches(CodeGenerationSupporter.Enable) && 
       !NextTokenMatches(CodeGenerationSupporter.Move) && !NextTokenMatches(CodeGenerationSupporter.Get) &&
       !NextTokenMatches(CodeGenerationSupporter.Receive) && !NextTokenMatches(CodeGenerationSupporter.Send) && 
       !NextTokenMatches(CodeGenerationSupporter.Throw)}? 
        (vExecProc = execProc 
        {
            vResult = FragmentFactory.CreateFragment<ExecuteStatement>();
            vExecuteSpecification = FragmentFactory.CreateFragment<ExecuteSpecification>();
            vExecuteSpecification.ExecutableEntity = vExecProc;
            vResult.ExecuteSpecification=vExecuteSpecification;
        }
        optSemicolons[vResult]
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
    | vResult=backupStatements
    | vResult=restoreStatements
    | vResult=gotoStatement
    | vResult=saveTransactionStatement
    | vResult=rollbackTransactionStatement
    | vResult=commitTransactionStatement
    | vResult=createStatisticsStatement
    | vResult=updateStatisticsStatement
    | vResult=alterDatabaseStatements
    | vResult=executeStatement
    | vResult=withCommonTableExpressionsAndXmlNamespacesStatements
    | vResult=raiseErrorStatement
    | vResult=alter2005Statements
    | vResult=create2005Statements
    | vResult=createDatabaseStatements
    | vResult=addSignatureStatement
    | vResult=identifierStatements
    | vResult=printStatement
    | vResult=waitForStatement
    | vResult=readTextStatement
    | vResult=updateTextStatement
    | vResult=writeTextStatement
    | vResult=lineNoStatement
    | vResult=useStatement
    | vResult=killStatements
    | vResult=bulkInsertStatement
    | vResult=insertBulkStatement
    | vResult=checkpointStatement
    | vResult=reconfigureStatement
    | vResult=shutdownStatement
    | vResult=setUserStatement
    | vResult=truncateTableStatement
    | vResult=grantStatement90
    | vResult=denyStatement90
    | vResult=revokeStatement90
    | vResult=returnStatement
    | vResult=openStatements
    | vResult=closeStatements
    | vResult=deallocateCursorStatement
    | vResult=fetchCursorStatement
    | vResult=dropStatements
    | vResult=dbccStatement
    | vResult=revertStatement
    | vResult=executeAsStatement
    | vResult=endConversationStatement
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

withCommonTableExpressionsAndXmlNamespacesStatements returns [StatementWithCtesAndXmlNamespaces vResult = null]
{
    WithCtesAndXmlNamespaces vWithCommonTableExpressionsAndXmlNamespaces = null;
}
    :
        (
            vWithCommonTableExpressionsAndXmlNamespaces=withCommonTableExpressionsAndXmlNamespaces
        )?
        (
            vResult=select[SubDmlFlags.SelectNotForInsert]
            {
                // check for invalid combination of CHANGE_TRACKING_CONTEXT and Select statement
                if ((vWithCommonTableExpressionsAndXmlNamespaces != null) && (vWithCommonTableExpressionsAndXmlNamespaces.ChangeTrackingContext != null))
                    ThrowParseErrorException("SQL46072", vWithCommonTableExpressionsAndXmlNamespaces.ChangeTrackingContext, TSqlParserResource.SQL46072Message);
            }                
        | 
            vResult=deleteStatement[SubDmlFlags.None]
        | 
            vResult=insertStatement[SubDmlFlags.None]
        | 
            vResult=updateStatement[SubDmlFlags.None]
        |
            vResult=mergeStatement[SubDmlFlags.None]
        )
        {
            vResult.WithCtesAndXmlNamespaces = vWithCommonTableExpressionsAndXmlNamespaces;
        }
    ;

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
    | vResult=createFederationStatement
    | vResult=alterFederationStatement
    | vResult=useFederationStatement
    ;

// This rule conflicts with optSimpleExecute (both can start with Identifier)
// We should update predicates here and in optSimpleExecute at the same time
identifierStatements returns [TSqlStatement vResult]
    :    {NextTokenMatches(CodeGenerationSupporter.Disable)}?
        vResult=disableTriggerStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Enable)}?
        vResult=enableTriggerStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Move)}?
        vResult = moveConversationStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Get)}?
        vResult = getConversationGroupStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Receive)}?
        vResult = receiveStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Send)}?
        vResult = sendStatement
    |      {NextTokenMatches(CodeGenerationSupporter.Throw)}?
        vResult = throwStatement
    ;


disableTriggerStatement returns [EnableDisableTriggerStatement vResult = this.FragmentFactory.CreateFragment<EnableDisableTriggerStatement>()]
    :
        tDisable:Identifier
        {
            Match(tDisable, CodeGenerationSupporter.Disable);
            UpdateTokenInfo(vResult,tDisable);
            vResult.TriggerEnforcement = TriggerEnforcement.Disable;
        }
        enableDisableTriggerBody[vResult]
    ;

enableTriggerStatement returns [EnableDisableTriggerStatement vResult = this.FragmentFactory.CreateFragment<EnableDisableTriggerStatement>()]
    :
        tEnable:Identifier
        {
            Match(tEnable, CodeGenerationSupporter.Enable);
            UpdateTokenInfo(vResult,tEnable);
            vResult.TriggerEnforcement = TriggerEnforcement.Enable;
        }
        enableDisableTriggerBody[vResult]
    ;

enableDisableTriggerBody[EnableDisableTriggerStatement vParent]
{
    SchemaObjectName vSchemaObjectName;
    TriggerObject vTriggerObject;
}
    :
        Trigger
        (
            vSchemaObjectName=schemaObjectThreePartName
            {
                AddAndUpdateTokenInfo(vParent, vParent.TriggerNames,vSchemaObjectName);
            }
            (
                Comma vSchemaObjectName=schemaObjectThreePartName
                {
                    AddAndUpdateTokenInfo(vParent, vParent.TriggerNames,vSchemaObjectName);
                }
            )*
        |   
            All
            {
                vParent.All = true;
            }
        )
        On vTriggerObject=triggerObject
        {
            vParent.TriggerObject = vTriggerObject;
        }
    ;

create2005Statements returns [TSqlStatement vResult = null]
    : tCreate:Create
        (
            {NextTokenMatches(CodeGenerationSupporter.Aggregate)}?
            vResult=createAggregateStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Application)}?
            vResult=createApplicationRoleStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Assembly)}?
            vResult=createAssemblyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Asymmetric)}?
            vResult=createAsymmetricKeyStatement            
        |
            {NextTokenMatches(CodeGenerationSupporter.Availability)}?
            vResult=createAvailabilityGroupStatement            
        |
            {NextTokenMatches(CodeGenerationSupporter.Broker)}?
            vResult=createBrokerPriorityStatement            
        |
            {NextTokenMatches(CodeGenerationSupporter.Certificate)}?
            vResult=createCertificateStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.ColumnStore)}?
            vResult=createColumnStoreIndexStatement[null, null]
        |
            {NextTokenMatches(CodeGenerationSupporter.Contract)}?
            vResult=createContractStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Credential)}?
            vResult=createCredentialStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Cryptographic)}?
            vResult=createCryptographicProviderStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Endpoint)}?
            vResult=createEndpointStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Event)}?
            vResult=createEventStatement // NOTIFICATION or SESSION
        |
            {NextTokenMatches(CodeGenerationSupporter.Fulltext)}?
            vResult=createFulltextStatement // Index or CATALOG
        |
            vResult=createPrimaryXmlIndexStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Selective)}?
            vResult=createSelectiveXmlIndexStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Xml)}?
            vResult=createXmlStatements // Index or Schema
        |
            {NextTokenMatches(CodeGenerationSupporter.Login)}?
            vResult=createLoginStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Message)}?
            vResult=createMessageTypeStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Master)}?
            vResult=createMasterKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Partition)}?
            vResult=createPartitionStatement // SCHEME or Function
        |
            {NextTokenMatches(CodeGenerationSupporter.Queue)}?
            vResult=createQueueStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Remote)}?
            vResult=createRemoteServiceBindingStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Resource)}?
            vResult=createResourcePoolStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Role)}?
            vResult=createRoleStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Route)}?
            vResult=createRouteStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Search)}?
            vResult=createSearchPropertyListStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Service)}?
            vResult=createServiceStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Spatial)}?
            vResult=createSpatialIndexStatement            
        |
            {NextTokenMatches(CodeGenerationSupporter.Symmetric)}?
            vResult=createSymmetricKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Synonym)}?
            vResult=createSynonymStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Type)}?
            vResult=createTypeStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Server)}?
            vResult=createServerStatements //AUDIT or ROLE
        |
            {NextTokenMatches(CodeGenerationSupporter.Workload)}?
            vResult=createWorkloadGroupStatement            
        |    
            {NextTokenMatches(CodeGenerationSupporter.Sequence)}?
            vResult=createSequenceStatement
        |
            vResult=createUserStatement
        )
        {
            UpdateTokenInfo(vResult,tCreate);
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;
    exception
    catch[PhaseOnePartialAstException exception]
    {
        UpdateTokenInfo(exception.Statement, tCreate);
        throw;
    }

createAggregateStatement returns [CreateAggregateStatement vResult = FragmentFactory.CreateFragment<CreateAggregateStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    ProcedureParameter vParameter;
    AssemblyName vAssemblyName;
    DataTypeReference vDataType;
}
    : tAggregate:Identifier vSchemaObjectName=schemaObjectThreePartName
        {
            Match(tAggregate, CodeGenerationSupporter.Aggregate);
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Aggregate);
            vResult.Name = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        LeftParenthesis vParameter = aggregateParameter 
        {
            AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
        }
        (Comma vParameter = aggregateParameter 
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vParameter);
            }
        )*
        RightParenthesis
        tReturns:Identifier vDataType = scalarDataType
        {
            Match(tReturns,CodeGenerationSupporter.Returns);
            vResult.ReturnType = vDataType;
        }
        External vAssemblyName = assemblyName
        {
            vResult.AssemblyName = vAssemblyName;
        }
    ;
    
aggregateParameter returns [ProcedureParameter vResult = FragmentFactory.CreateFragment<ProcedureParameter>()]
{
    Identifier vParamName;
    DataTypeReference vDataType;
    NullableConstraintDefinition vNullableConstraintDefinition;
}
    :   vParamName = identifierVariable (As)? vDataType = scalarDataType
        {
            vResult.VariableName = vParamName;
            vResult.DataType = vDataType;
        }
       (
            vNullableConstraintDefinition = nullableConstraint
            {
                vResult.Nullable=vNullableConstraintDefinition;
            }
        )?
    ;
    
createApplicationRoleStatement returns [CreateApplicationRoleStatement vResult = this.FragmentFactory.CreateFragment<CreateApplicationRoleStatement>()]
    : 
        applicationRoleStatement[vResult, true]
    ;

createAssemblyStatement returns [CreateAssemblyStatement vResult = this.FragmentFactory.CreateFragment<CreateAssemblyStatement>()]
{
    Identifier vIdentifier;
    AssemblyOption vOption;
}
    :   tAssembly:Identifier vIdentifier=identifier
        {
            Match(tAssembly, CodeGenerationSupporter.Assembly);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        From expressionList[vResult, vResult.Parameters]
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With tPermissionSet:Identifier EqualsSign
            vOption=assemblyPermissionSetOption[tPermissionSet]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )?
    ;

createAsymmetricKeyStatement returns [CreateAsymmetricKeyStatement vResult = FragmentFactory.CreateFragment<CreateAsymmetricKeyStatement>()]
{
    Identifier vIdentifier;
    Literal vPassword;
}
    : tAsymmetric:Identifier Key vIdentifier=identifier 
        {
            Match(tAsymmetric, CodeGenerationSupporter.Asymmetric);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        createAsymmetricKeyParams[vResult]
        (
            // Greedy due to linear approximation introduced after the rule securityStatementPermission
            options {greedy = true; } :
            vPassword = encryptClause
            {
                vResult.Password = vPassword;
            }
        )?
    ;

createAsymmetricKeyParams[CreateAsymmetricKeyStatement vParent]
{
    EncryptionSource vSource;
}
    : From vSource = asymKeySource
        {
            vParent.KeySource=vSource;
        }
    | With asymKeySpec[vParent]
    ;
    
asymKeySource returns [EncryptionSource vResult]
    : 
        vResult = fileEncryptionSource
    |   {NextTokenMatches(CodeGenerationSupporter.Assembly)}?
        vResult = assemblyEncryptionSource
    |   vResult = providerEncryptionSource
    ;
    
assemblyEncryptionSource returns [AssemblyEncryptionSource vResult=FragmentFactory.CreateFragment<AssemblyEncryptionSource>()]
{
    Identifier vAssembly;
}
    : tAssembly:Identifier vAssembly = identifier
        {
            Match(tAssembly, CodeGenerationSupporter.Assembly);
            vResult.Assembly = vAssembly;
        }
    ;
    
providerEncryptionSource returns [ProviderEncryptionSource vResult = FragmentFactory.CreateFragment<ProviderEncryptionSource>()]
{
    Identifier vProviderName;
}
    : tProvider:Identifier vProviderName = identifier 
        {
            Match(tProvider, CodeGenerationSupporter.Provider);
            vResult.Name = vProviderName;
        }
        providerKeySourceOptions[vResult.KeyOptions, vResult]
    ;
    
fileEncryptionSource returns [FileEncryptionSource vResult = FragmentFactory.CreateFragment<FileEncryptionSource>()]
{
    Literal vFile;
}
    :  (tExecutable:Identifier
            {
                Match(tExecutable, CodeGenerationSupporter.Executable);
                vResult.IsExecutable = true;            
            }
        )? 
        File EqualsSign vFile = stringLiteral
        {
            vResult.File = vFile;
        }
    ;
    
asymKeySpec [CreateAsymmetricKeyStatement vParent]
    : tAlgorithm:Identifier EqualsSign tRealAlg:Identifier
        {
            Match(tAlgorithm,CodeGenerationSupporter.Algorithm);
            vParent.EncryptionAlgorithm = EncryptionAlgorithmsHelper.Instance.ParseOption(tRealAlg);
            UpdateTokenInfo(vParent,tRealAlg);
        }
    ;
        
createCertificateStatement returns [CreateCertificateStatement vResult = FragmentFactory.CreateFragment<CreateCertificateStatement>()]
{
    Identifier vIdentifier;
}
    : tCertificate:Identifier vIdentifier=identifier
        {
            Match(tCertificate, CodeGenerationSupporter.Certificate);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        createCertificateParams[vResult] 
        (
            // Greedy due to linear approximation introduced after the rule securityStatementPermission
            options {greedy = true; } :
            createCertificateActivityFlag[vResult]
        )?
    ;
    
createCertificateParams [CreateCertificateStatement vParent]
{
    Literal vPassword;
    CertificateOption vOption;
    CertificateOptionKinds encounteredOptions = CertificateOptionKinds.None;
}
    : From certificateSource[vParent]
    | (
        (vPassword = encryptClause
            {
                vParent.EncryptionPassword = vPassword;
            }
        )? 
        With vOption = certificateOption[encounteredOptions]
            {
                encounteredOptions = encounteredOptions | vOption.Kind;
                AddAndUpdateTokenInfo(vParent, vParent.CertificateOptions,vOption);
            }
        (Comma vOption = certificateOption[encounteredOptions]
            {
                encounteredOptions = encounteredOptions | vOption.Kind;
                AddAndUpdateTokenInfo(vParent, vParent.CertificateOptions,vOption);
            }
        )*
      )
    ;
    
createCertificateActivityFlag [CertificateStatementBase vParent]
{
    OptionState vOptionState;
}
    : tActive:Identifier For tBeginDialog:Identifier EqualsSign vOptionState = optionOnOff[vParent]
        {
            Match(tActive,CodeGenerationSupporter.Active);
            Match(tBeginDialog,CodeGenerationSupporter.BeginDialog);
            vParent.ActiveForBeginDialog = vOptionState;
        }
    ;

certificateOption [CertificateOptionKinds encountered]returns [CertificateOption vResult = FragmentFactory.CreateFragment<CertificateOption>()]
{
    Literal vValue;
}
    : tOption:Identifier EqualsSign vValue = stringLiteral
        {
            vResult.Kind = CertificateOptionKindsHelper.Instance.ParseOption(tOption);
            vResult.Value = vValue;
            CheckCertificateOptionDupication(encountered,vResult.Kind,tOption);
        }
    ;
    
certificateSource [CreateCertificateStatement vParent]
{
    EncryptionSource vCertificateSource;
}
    :   
        (
            vCertificateSource=fileEncryptionSource 
            (
                // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } :
                With privateKeySpec[vParent]
            )?
        |
            vCertificateSource = assemblyEncryptionSource
        )
        {
            vParent.CertificateSource = vCertificateSource;
        }
    ;
    
encryptClause returns [Literal vResult]
    : tEncryption:Identifier By tPassword:Identifier EqualsSign vResult = stringLiteral
        {
            Match(tEncryption,CodeGenerationSupporter.Encryption);
            Match(tPassword,CodeGenerationSupporter.Password);
        }
    ;
    
privateKeySpec [CertificateStatementBase vParent]
    : tPrivate:Identifier Key LeftParenthesis certificatePrivateKeySpec[vParent] (Comma certificatePrivateKeySpec[vParent])* tRParen:RightParenthesis
        {
            Match(tPrivate,CodeGenerationSupporter.Private);
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
certificatePrivateKeySpec [CertificateStatementBase vParent]
{
    Literal vFilePath;
}
    : passwordChangeOption[vParent]
    | tFile:File EqualsSign vFilePath = stringLiteral
        {
            if (vParent.PrivateKeyPath != null)
                throw GetUnexpectedTokenErrorException(tFile);
            else
                vParent.PrivateKeyPath = vFilePath;
        }
    ;
    
passwordChangeOption [IPasswordChangeOption vParent]
{
    Literal vPassword;
}
    : tEncryptionDecryption:Identifier By tPassword:Identifier EqualsSign vPassword = stringLiteral
        {
            if (TryMatch(tEncryptionDecryption,CodeGenerationSupporter.Encryption))
            {
                if (vParent.EncryptionPassword != null)
                    throw GetUnexpectedTokenErrorException(tEncryptionDecryption);
                else
                    vParent.EncryptionPassword = vPassword;
            }
            else
            {
                Match(tEncryptionDecryption,CodeGenerationSupporter.Decryption);
                if (vParent.DecryptionPassword != null)
                    throw GetUnexpectedTokenErrorException(tEncryptionDecryption);
                else
                    vParent.DecryptionPassword = vPassword;
            }
        }
    ;
    
    
createContractStatement returns [CreateContractStatement vResult = FragmentFactory.CreateFragment<CreateContractStatement>()]
{
    Identifier vIdentifier;
    ContractMessage vMessage;
}
    : tContract:Identifier vIdentifier=identifier
        {
            Match(tContract, CodeGenerationSupporter.Contract);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        LeftParenthesis vMessage = contractMessage
            {
                AddAndUpdateTokenInfo(vResult, vResult.Messages,vMessage);
            }
        (Comma vMessage = contractMessage
            {
                AddAndUpdateTokenInfo(vResult, vResult.Messages,vMessage);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;
    
contractMessage returns [ContractMessage vResult = FragmentFactory.CreateFragment<ContractMessage>()]
{
    Identifier vMessageName;
}
    : vMessageName = identifier tSent:Identifier By 
        {
            Match(tSent,CodeGenerationSupporter.Sent);
            vResult.Name = vMessageName;
        }
        ( tAny:Any 
            {
                vResult.SentBy = MessageSender.Any;
                UpdateTokenInfo(vResult,tAny);
            }
        | tInitiatorTarget:Identifier
            {
                if (TryMatch(tInitiatorTarget,CodeGenerationSupporter.Initiator))
                    vResult.SentBy = MessageSender.Initiator;
                else
                {
                    Match(tInitiatorTarget,CodeGenerationSupporter.Target);
                    vResult.SentBy = MessageSender.Target;
                }
                UpdateTokenInfo(vResult,tInitiatorTarget);
            }
        )
    ;

createCredentialStatement returns [CreateCredentialStatement vResult = FragmentFactory.CreateFragment<CreateCredentialStatement>()]
{
    Identifier vCryptographicProviderName;    
}
    : credentialStatementBody[vResult]
        (
            For tCryptographic:Identifier tProvider:Identifier vCryptographicProviderName=identifier
            {
                Match(tCryptographic, CodeGenerationSupporter.Cryptographic);
                Match(tProvider, CodeGenerationSupporter.Provider);
                vResult.CryptographicProviderName = vCryptographicProviderName;
            }
        )?
    ;
    
credentialStatementBody [CredentialStatement vParent]
{
    Identifier vIdentifier;
    Literal vLiteral;
}
    : tCredential:Identifier vIdentifier=identifier
        {
            Match(tCredential, CodeGenerationSupporter.Credential);
            vParent.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vParent);
        }
      With Identity EqualsSign vLiteral = stringLiteral
      {
        vParent.Identity = vLiteral;
      }
      (Comma tSecret:Identifier EqualsSign vLiteral = stringLiteral
        {
            Match(tSecret,CodeGenerationSupporter.Secret);
            vParent.Secret = vLiteral;
        }
      )?
    ;

createServerStatements returns [TSqlStatement vResult]
    : tServer:Identifier
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Audit)}?
            vResult=createServerAuditStatements
          | 
            {NextTokenMatches(CodeGenerationSupporter.Role)}?
            vResult=createServerRoleStatement
        )
    ;

createServerAuditStatements returns [TSqlStatement vResult]
    :    tAudit:Identifier
        {
            Match(tAudit, CodeGenerationSupporter.Audit);
        }
        (
            vResult = createServerAuditSpecificationStatement
        |
            vResult = createServerAuditStatement
        )
    ;
    
createServerAuditStatement returns [CreateServerAuditStatement vResult = FragmentFactory.CreateFragment<CreateServerAuditStatement>()]
{
    Identifier vAuditName;
    AuditTarget vTarget;
    BooleanExpression vFilterPredicate;
}
    :    vAuditName = identifier
        {
            vResult.AuditName = vAuditName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vTarget = auditTargetClause[true]
        {
            vResult.AuditTarget = vTarget;
        }
        (    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            auditCreateWithClause[vResult]
        )?
        (
            Where vFilterPredicate=eventBooleanExpression
            {
                vResult.PredicateExpression = vFilterPredicate;
            }    
        )?
    ;
    
auditTargetClause [bool filePathRequired] returns [AuditTarget vResult = FragmentFactory.CreateFragment<AuditTarget>()]
{
    AuditTargetOption vOption;
    bool filePathOptionEncountered = false;
}
    :    tTo:To 
        {
            UpdateTokenInfo(vResult,tTo);
        }
        (
            tFile:File LeftParenthesis vOption = auditFileOption
            {
                vResult.TargetKind = AuditTargetKind.File;
                AddAndUpdateTokenInfo(vResult, vResult.TargetOptions, vOption);
                
                filePathOptionEncountered |= (vOption.OptionKind==AuditTargetOptionKind.FilePath);
            }
            (Comma vOption = auditFileOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.TargetOptions, vOption);
                    
                    filePathOptionEncountered |= (vOption.OptionKind==AuditTargetOptionKind.FilePath);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
                if (filePathRequired && !filePathOptionEncountered)
                {
                    ThrowParseErrorException("SQL46056", tFile, 
                        TSqlParserResource.SQL46056Message);
                }
            }
        |
            tApplicationLogSecurityLog:Identifier
            {
                if (TryMatch(tApplicationLogSecurityLog, CodeGenerationSupporter.ApplicationLog))
                    vResult.TargetKind = AuditTargetKind.ApplicationLog;
                else
                {
                    Match(tApplicationLogSecurityLog, CodeGenerationSupporter.SecurityLog);
                    vResult.TargetKind = AuditTargetKind.SecurityLog;
                }
                UpdateTokenInfo(vResult,tApplicationLogSecurityLog);
            }
        )
    ;
    
// Corresponds to audit_file_option_element in SQL yacc grammar
auditFileOption returns [AuditTargetOption vResult = null]
    :
            {NextTokenMatches(CodeGenerationSupporter.MaxSize)}?
            vResult = maxSizeAuditFileOption
        |    
            {NextTokenMatches(CodeGenerationSupporter.MaxRolloverFiles)}?
            vResult = maxRolloverFilesAuditFileOption
        |
            {NextTokenMatches(CodeGenerationSupporter.ReserveDiskSpace)}?
            vResult = reserveDiskSpaceAuditFileOption
        |
            {NextTokenMatches(CodeGenerationSupporter.MaxFiles)}?
            vResult = maxFilesAuditFileOption
        |
            vResult = filePathAuditFileOption
    ;
    
maxSizeAuditFileOption returns [MaxSizeAuditTargetOption vResult = FragmentFactory.CreateFragment<MaxSizeAuditTargetOption>()]
{
    Literal vSize;
}
    :    tOption:Identifier EqualsSign
        {
            Match(tOption, CodeGenerationSupporter.MaxSize);
            vResult.OptionKind=AuditTargetOptionKind.MaxSize;
            UpdateTokenInfo(vResult, tOption);
        }
        (
            vSize = integer tUnit:Identifier
            {
                vResult.Size = vSize;
                
                if (TryMatch(tUnit, CodeGenerationSupporter.GB))
                {
                    vResult.Unit = MemoryUnit.GB;
                    ThrowIfTooLargeAuditFileSize(vSize, 10);
                }
                else if (TryMatch(tUnit, CodeGenerationSupporter.TB))
                {
                    vResult.Unit = MemoryUnit.TB;
                    ThrowIfTooLargeAuditFileSize(vSize, 20);
                }
                else
                {
                    Match(tUnit, CodeGenerationSupporter.MB);
                    vResult.Unit = MemoryUnit.MB;
                    ThrowIfTooLargeAuditFileSize(vSize, 0);
                }
                
                UpdateTokenInfo(vResult, tUnit);
            }
        |
            tUnlimited:Identifier
            {
                Match(tUnlimited, CodeGenerationSupporter.Unlimited);
                vResult.IsUnlimited = true;
                vResult.Size = null;
                vResult.Unit = MemoryUnit.Unspecified;
            }
        )
    ;

maxRolloverFilesAuditFileOption returns [MaxRolloverFilesAuditTargetOption vResult = FragmentFactory.CreateFragment<MaxRolloverFilesAuditTargetOption>()]
{
    Literal vValue;
}
    :    tOption:Identifier EqualsSign 
        {
            Match(tOption, CodeGenerationSupporter.MaxRolloverFiles);
            vResult.OptionKind=AuditTargetOptionKind.MaxRolloverFiles;
            UpdateTokenInfo(vResult, tOption);
        }
        (
            vValue = integer
            {
                vResult.Value = vValue;
            }
        |
            tUnlimited:Identifier
            {
                Match(tUnlimited, CodeGenerationSupporter.Unlimited);
                vResult.IsUnlimited = true;
                UpdateTokenInfo(vResult, tUnlimited);
            }
        )
    ;

maxFilesAuditFileOption returns [LiteralAuditTargetOption vResult = FragmentFactory.CreateFragment<LiteralAuditTargetOption>()]
{
    Literal vValue;
}
    :    tOption:Identifier EqualsSign vValue = integer
        {
            Match(tOption, CodeGenerationSupporter.MaxFiles);
            vResult.OptionKind=AuditTargetOptionKind.MaxFiles;
            UpdateTokenInfo(vResult, tOption);
            vResult.Value = vValue;
        }
    ;

reserveDiskSpaceAuditFileOption returns [OnOffAuditTargetOption vResult = FragmentFactory.CreateFragment<OnOffAuditTargetOption>()] 
{
    OptionState vValue;
}
    :    tOption:Identifier EqualsSign vValue = optionOnOff[vResult]
        {
            Match(tOption, CodeGenerationSupporter.ReserveDiskSpace);
            vResult.OptionKind=AuditTargetOptionKind.ReserveDiskSpace;
            UpdateTokenInfo(vResult, tOption);
            vResult.Value = vValue;
        }
    ;
    
filePathAuditFileOption returns [LiteralAuditTargetOption vResult = FragmentFactory.CreateFragment<LiteralAuditTargetOption>()]
{
    Literal vValue;
}
    :    tOption:Identifier EqualsSign vValue = stringLiteral
        {
            Match(tOption, CodeGenerationSupporter.FilePath);
            vResult.OptionKind=AuditTargetOptionKind.FilePath;
            UpdateTokenInfo(vResult, tOption);
            vResult.Value = vValue;
        }
    ;

auditCreateWithClause [ServerAuditStatement vParent]
{
    AuditOption vOption;
}
    :    With LeftParenthesis vOption = auditCreateOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = auditCreateOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

auditWithClause [ServerAuditStatement vParent]
{
    AuditOption vOption;
}
    :    With LeftParenthesis vOption = auditOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = auditOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

// Corresponds to audit_create_option_element in SQL yacc
auditCreateOption returns [AuditOption vResult]
    :
        tOption:Identifier EqualsSign
        (
            vResult = queueDelayAuditOption[tOption]
        |
            vResult = onFailureAuditOption[tOption]
        |
            vResult = auditGuidAuditOption[tOption]
        )
    ;
    
// Corresponds to audit_option_element in SQL yacc
auditOption returns [AuditOption vResult]
    :
        tOption:Identifier EqualsSign
        (
            vResult = queueDelayAuditOption[tOption]
        |
            vResult = onFailureAuditOption[tOption]
        |
            vResult = stateAuditOption[tOption]
        )
    ;
    
queueDelayAuditOption [IToken tOption] returns [QueueDelayAuditOption vResult = FragmentFactory.CreateFragment<QueueDelayAuditOption>()]
{
    Literal vValue;
}
    :    vValue = integer
        {
            Match(tOption, CodeGenerationSupporter.QueueDelay);
            vResult.OptionKind=AuditOptionKind.QueueDelay;
            UpdateTokenInfo(vResult,tOption);
            vResult.Delay = vValue;
        }
    ;
    
onFailureAuditOption [IToken tOption] returns [OnFailureAuditOption vResult = FragmentFactory.CreateFragment<OnFailureAuditOption>()]
    :
        {
            Match(tOption, CodeGenerationSupporter.OnFailure);
            UpdateTokenInfo(vResult,tOption);
            vResult.OptionKind=AuditOptionKind.OnFailure;
        }
        (
            tContinue:Continue
            {
                UpdateTokenInfo(vResult,tContinue);
                vResult.OnFailureAction = AuditFailureActionType.Continue;
            }
        |
            tShutdown:Shutdown
            {
                UpdateTokenInfo(vResult,tShutdown);
                vResult.OnFailureAction = AuditFailureActionType.Shutdown;
            }
        |
            tIdentifier:Identifier
            {
                Match(tIdentifier, CodeGenerationSupporter.FailOperation);
                UpdateTokenInfo(vResult, tIdentifier);
                vResult.OnFailureAction = AuditFailureActionType.FailOperation;
            }
        )
    ;    
    
auditGuidAuditOption [IToken tOption] returns [AuditGuidAuditOption vResult = FragmentFactory.CreateFragment<AuditGuidAuditOption>()]
{
    Literal vValue;
}
    :    vValue = stringLiteral
        {
            Match(tOption, CodeGenerationSupporter.AuditGuid);
            ThrowIfWrongGuidFormat(vValue);
            vResult.OptionKind=AuditOptionKind.AuditGuid;
            UpdateTokenInfo(vResult,tOption);
            vResult.Guid = vValue;
        }
    ;

stateAuditOption [IToken tOption] returns [StateAuditOption vResult = FragmentFactory.CreateFragment<StateAuditOption>()]
{
    OptionState vValue;
}
    :    vValue = optionOnOff[vResult]
        {
            Match(tOption, CodeGenerationSupporter.State);
            vResult.OptionKind=AuditOptionKind.State;
            UpdateTokenInfo(vResult,tOption);
            vResult.Value = vValue;
        }
    ;
    
createServerAuditSpecificationStatement returns [CreateServerAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<CreateServerAuditSpecificationStatement>()]
{
    Identifier vAuditSpecName;
    AuditSpecificationPart vPart;
}
    : tSpecification:Identifier vAuditSpecName = identifier 
        {
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.SpecificationName = vAuditSpecName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        auditSpecificationForClause[vResult]
        (    // Conflicts with Add SIGNATURE (but it actually shouldn't, k=2 should be enough)
            (Add LeftParenthesis) => 
            vPart = createAuditSpecificationDetail
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
            }
            (Comma vPart = createAuditSpecificationDetail
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
                }
            )*
        )?
        auditSpecificationStateOpt[vResult]
    ;

alterServerStatements returns [TSqlStatement vResult]
    :
        tServer:Identifier
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Audit)}?
            vResult = alterServerAuditStatements
        |
            {NextTokenMatches(CodeGenerationSupporter.Configuration)}?
            vResult = alterServerConfigurationStatement
        |   
            {NextTokenMatches(CodeGenerationSupporter.Role)}?
            vResult = alterServerRoleStatement
        )
    ;

alterServerAuditStatements returns [TSqlStatement vResult]
    :  tAudit:Identifier 
        {
            Match(tAudit, CodeGenerationSupporter.Audit);
        }
        (    
            {NextTokenMatches(CodeGenerationSupporter.Specification)}?
            vResult = alterServerAuditSpecificationStatement
        |
            vResult = alterServerAuditStatement
        )
    ;
    
alterServerAuditSpecificationStatement returns [AlterServerAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<AlterServerAuditSpecificationStatement>()]
{
    Identifier vAuditSpecName;
    AuditSpecificationPart vPart;
}
    : tSpecification:Identifier vAuditSpecName = identifier 
        {
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.SpecificationName = vAuditSpecName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (auditSpecificationForClause[vResult])?
        (    // Conflicts with Add SIGNATURE and Drop statements
            ((Add|Drop) LeftParenthesis) => 
            vPart = auditSpecificationDetail
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
            }
            (Comma vPart = auditSpecificationDetail
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
                }
            )*
        )?
        auditSpecificationStateOpt[vResult]
    ;
    
alterServerAuditStatement returns [AlterServerAuditStatement vResult = FragmentFactory.CreateFragment<AlterServerAuditStatement>()]
{
    Identifier vAuditName;
    Identifier vNewName;
    AuditTarget vTarget = null;
    BooleanExpression vFilterPredicate = null;
}
    : vAuditName = identifier 
        {
            vResult.AuditName = vAuditName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Modify)}?
            tModify:Identifier tName:Identifier EqualsSign vNewName = identifier
            {
                Match(tModify, CodeGenerationSupporter.Modify);
                Match(tName, CodeGenerationSupporter.Name);
                vResult.NewName = vNewName;
            }
        |
            (
                vTarget = auditTargetClause[false]
                {
                    vResult.AuditTarget = vTarget;
                }
            )?
            (    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } :
                auditWithClause[vResult]
            )?
            (
                Where vFilterPredicate=eventBooleanExpression
                {
                    vResult.PredicateExpression = vFilterPredicate;
                }    
            )?
            {
                if(vTarget == null && (vResult.Options == null || vResult.Options.Count == 0) && vFilterPredicate == null)
                {
                    ThrowIncorrectSyntaxErrorException(vAuditName);
                }
            }
        |
            tRemove:Identifier tWhere:Where
            {
                Match(tRemove, CodeGenerationSupporter.Remove);
                UpdateTokenInfo(vResult, tWhere);
                vResult.RemoveWhere=true;
            }
        )
    ;

alterServerConfigurationStatement returns [TSqlStatement vResult]
    :
        tConfiguration:Identifier Set
        {
            Match(tConfiguration, CodeGenerationSupporter.Configuration);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Process)}?
            vResult = alterServerConfigurationSetProcessAffinityStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Buffer)}?
            vResult = alterServerConfigurationSetBufferPoolExtensionStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Diagnostics)}?
            vResult = alterServerConfigurationSetDiagnosticsLogStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Failover)}?
            vResult = alterServerConfigurationSetFailoverClusterPropertyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Hadr)}?
            vResult = alterServerConfigurationSetHadrClusterStatement
        )
    ;

alterServerConfigurationSetBufferPoolExtensionStatement returns [AlterServerConfigurationSetBufferPoolExtensionStatement vResult = FragmentFactory.CreateFragment<AlterServerConfigurationSetBufferPoolExtensionStatement>()]
{
    AlterServerConfigurationBufferPoolExtensionOption vOption;
}
    :   tBuffer:Identifier tPool:Identifier tExtension:Identifier
        {
            Match(tBuffer, CodeGenerationSupporter.Buffer);
            Match(tPool, CodeGenerationSupporter.Pool);
            Match(tExtension, CodeGenerationSupporter.Extension);
        }
        vOption=alterServerConfigurationBufferPoolExtensionContainerOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
    ;

alterServerConfigurationBufferPoolExtensionContainerOption returns [AlterServerConfigurationBufferPoolExtensionContainerOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationBufferPoolExtensionContainerOption>()]
{
    OnOffOptionValue vOptionValue;
    AlterServerConfigurationBufferPoolExtensionOption vFileNameSuboption;
    AlterServerConfigurationBufferPoolExtensionOption vSizeSuboption;
}
    :   vOptionValue=onOffOptionValue
        {
            vResult.OptionValue = vOptionValue;
            vResult.OptionKind = AlterServerConfigurationBufferPoolExtensionOptionKind.OnOff;
        }
        (
            {
                // Additional options are only allowed when buffer pool extension is set to ON
                if (vOptionValue.OptionState != OptionState.On)
                    ThrowIncorrectSyntaxErrorException(vOptionValue);
            }
            tLParen:LeftParenthesis vFileNameSuboption=alterServerConfigurationBufferPoolExtensionFileNameOption
            {
                UpdateTokenInfo(vResult, tLParen);
                AddAndUpdateTokenInfo(vResult, vResult.Suboptions, vFileNameSuboption);
            }
            tComma:Comma vSizeSuboption=alterServerConfigurationBufferPoolExtensionSizeOption tRParen:RightParenthesis
            {
                AddAndUpdateTokenInfo(vResult, vResult.Suboptions, vSizeSuboption);
                UpdateTokenInfo(vResult, tRParen);
            }
        |
            {
                // Empty rule: setting buffer pool extension to OFF is the only allowed
                if (vOptionValue.OptionState != OptionState.Off)
                    ThrowIncorrectSyntaxErrorException(vOptionValue);
            }
        )
    ;

alterServerConfigurationBufferPoolExtensionFileNameOption returns [AlterServerConfigurationBufferPoolExtensionOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationBufferPoolExtensionOption>()]
{
    LiteralOptionValue vFileName;
}
    :   tFileName:Identifier EqualsSign vFileName=stringLiteralOptionValue
        {
            Match(tFileName, CodeGenerationSupporter.FileName);
            vResult.OptionKind = AlterServerConfigurationBufferPoolExtensionOptionHelper.Instance.ParseOption(tFileName);
            vResult.OptionValue = vFileName;
        }
    ;

alterServerConfigurationBufferPoolExtensionSizeOption returns [AlterServerConfigurationBufferPoolExtensionSizeOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationBufferPoolExtensionSizeOption>()]
{
    LiteralOptionValue vSize;
    MemoryUnit vMemUnit;
}
    :   tSize:Identifier EqualsSign vSize=integerLiteralOptionValue vMemUnit=memUnit[vResult]
        {
            Match(tSize, CodeGenerationSupporter.Size);
            
            if (vMemUnit != MemoryUnit.KB && vMemUnit != MemoryUnit.MB && vMemUnit != MemoryUnit.GB)
                ThrowIncorrectSyntaxErrorException(vSize);

            vResult.OptionKind = AlterServerConfigurationBufferPoolExtensionOptionHelper.Instance.ParseOption(tSize);
            vResult.OptionValue = vSize;
            vResult.SizeUnit = vMemUnit;
        }
    ;

alterServerConfigurationSetDiagnosticsLogStatement returns [AlterServerConfigurationSetDiagnosticsLogStatement vResult = FragmentFactory.CreateFragment<AlterServerConfigurationSetDiagnosticsLogStatement>()]
{
    AlterServerConfigurationDiagnosticsLogOption vOption;
}
    :   tDiagnostics:Identifier tLog:Identifier
        {
            Match(tDiagnostics, CodeGenerationSupporter.Diagnostics);
            Match(tLog, CodeGenerationSupporter.Log);
        }
        vOption=alterServerConfigurationDiagnosticsLogOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
    ;

alterServerConfigurationDiagnosticsLogOption returns [AlterServerConfigurationDiagnosticsLogOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationDiagnosticsLogOption>()]
{
    OptionValue vOptionValue;
}
    :   vOptionValue=onOffOptionValue
        {
            vResult.OptionKind = AlterServerConfigurationDiagnosticsLogOptionKind.OnOff;
            vResult.OptionValue = vOptionValue;
        }
    |
        {NextTokenMatches(CodeGenerationSupporter.MaxUnderscoreSize)}?
        vResult=alterServerConfigurationDiagnosticsLogMaxSizeOption
    |
        tLogOption:Identifier EqualsSign
        {
            vResult.OptionKind = AlterServerConfigurationDiagnosticsLogOptionHelper.Instance.ParseOption(tLogOption);
        }
        (
            {vResult.OptionKind == AlterServerConfigurationDiagnosticsLogOptionKind.Path}?
            vOptionValue=stringOrDefaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        |
            {vResult.OptionKind == AlterServerConfigurationDiagnosticsLogOptionKind.MaxFiles}?
            vOptionValue=integerOrDefaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        )
    ;

alterServerConfigurationDiagnosticsLogMaxSizeOption returns [AlterServerConfigurationDiagnosticsLogMaxSizeOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationDiagnosticsLogMaxSizeOption>()]
{
    OptionValue vOptionValue;
}
    :   tMaxSize:Identifier EqualsSign
        {
            vResult.OptionKind = AlterServerConfigurationDiagnosticsLogOptionHelper.Instance.ParseOption(tMaxSize);
            if (vResult.OptionKind != AlterServerConfigurationDiagnosticsLogOptionKind.MaxSize)
                ThrowIncorrectSyntaxErrorException(tMaxSize);
        }
        (
            vOptionValue=integerLiteralOptionValue tMB:Identifier
            {
                Match(tMB, CodeGenerationSupporter.MB);
                vResult.OptionValue = vOptionValue;
                vResult.SizeUnit = MemoryUnit.MB;
                UpdateTokenInfo(vResult, tMB);
            }
        |
            vOptionValue=defaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        )
    ;

alterServerConfigurationSetFailoverClusterPropertyStatement returns [AlterServerConfigurationSetFailoverClusterPropertyStatement vResult = FragmentFactory.CreateFragment<AlterServerConfigurationSetFailoverClusterPropertyStatement>()]
{
    AlterServerConfigurationFailoverClusterPropertyOption vOption;
}
    :   tFailover:Identifier tCluster:Identifier tProperty:Identifier
        {
            Match(tFailover, CodeGenerationSupporter.Failover);
            Match(tCluster, CodeGenerationSupporter.Cluster);
            Match(tProperty, CodeGenerationSupporter.Property);
        }
        vOption=alterServerConfigurationFailoverClusterPropertyOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
    ;

alterServerConfigurationFailoverClusterPropertyOption returns [AlterServerConfigurationFailoverClusterPropertyOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationFailoverClusterPropertyOption>()]
{
    OptionValue vOptionValue;
}
    :   tProperty:Identifier EqualsSign
        {
            vResult.OptionKind = AlterServerConfigurationFailoverClusterPropertyOptionHelper.Instance.ParseOption(tProperty);
        }
        (
            {vResult.OptionKind == AlterServerConfigurationFailoverClusterPropertyOptionKind.SqlDumperDumpFlags}?
            vOptionValue=binaryOrDefaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        |
            {vResult.OptionKind == AlterServerConfigurationFailoverClusterPropertyOptionKind.SqlDumperDumpPath}?
            vOptionValue=stringOrDefaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        |
            vOptionValue=integerOrDefaultLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        )
    ;

alterServerConfigurationSetHadrClusterStatement returns [AlterServerConfigurationSetHadrClusterStatement vResult = FragmentFactory.CreateFragment<AlterServerConfigurationSetHadrClusterStatement>()]
{
    AlterServerConfigurationHadrClusterOption vOption;
}
    :    tHadr:Identifier tCluster:Identifier
        {
            Match(tHadr, CodeGenerationSupporter.Hadr);
            Match(tCluster, CodeGenerationSupporter.Cluster);
        }
        vOption=alterServerConfigurationHadrClusterOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
    ;

alterServerConfigurationHadrClusterOption returns [AlterServerConfigurationHadrClusterOption vResult = FragmentFactory.CreateFragment<AlterServerConfigurationHadrClusterOption>()]
{
    OptionValue vOptionValue;
}
    :    tOptionKind:Identifier EqualsSign
        {
            vResult.OptionKind = AlterServerConfigurationHadrClusterOptionHelper.Instance.ParseOption(tOptionKind);
        }
        (
            vOptionValue=stringLiteralOptionValue
            {
                vResult.OptionValue = vOptionValue;
            }
        |
            tLocal:Identifier
            {
                Match(tLocal, CodeGenerationSupporter.Local);
                vResult.IsLocal = true;
                UpdateTokenInfo(vResult, tLocal);
            }
        )
    ;

alterServerConfigurationSetProcessAffinityStatement returns [AlterServerConfigurationStatement vResult = FragmentFactory.CreateFragment<AlterServerConfigurationStatement>()]
    :   tProcess:Identifier tAffinity:Identifier
        {
            Match(tProcess, CodeGenerationSupporter.Process);
            Match(tAffinity, CodeGenerationSupporter.Affinity);
        }
        tCpuOrNumanode:Identifier EqualsSign
        (
            affinityRangeList[vResult]
            {
                if (TryMatch(tCpuOrNumanode, CodeGenerationSupporter.Cpu))
                {
                    vResult.ProcessAffinity = ProcessAffinityType.Cpu;
                }
                else
                {
                    Match(tCpuOrNumanode, CodeGenerationSupporter.NumaNode);
                    vResult.ProcessAffinity = ProcessAffinityType.NumaNode;
                }                
            }
        |
            tAuto:Identifier
            {
                // AUTO implies CPU affinity
                Match(tCpuOrNumanode, CodeGenerationSupporter.Cpu);
                Match(tAuto, CodeGenerationSupporter.Auto);
                
                vResult.ProcessAffinity = ProcessAffinityType.CpuAuto;
                
                UpdateTokenInfo(vResult, tAuto);            
            }
        )
    ;
    
affinityRangeList [AlterServerConfigurationStatement vParent]
{
    ProcessAffinityRange vAffinityRange;
}
    :   vAffinityRange = affinityRange
        {
            AddAndUpdateTokenInfo(vParent, vParent.ProcessAffinityRanges, vAffinityRange);
        }
        (Comma vAffinityRange = affinityRange
            {
                AddAndUpdateTokenInfo(vParent, vParent.ProcessAffinityRanges, vAffinityRange);
            }
        )*
    ;
    
affinityRange returns [ProcessAffinityRange vResult = FragmentFactory.CreateFragment<ProcessAffinityRange>()]
{
    Literal vBoundary;
}
    :   vBoundary = integer
        {
            vResult.From = vBoundary;
        }
        (To vBoundary = integer
            {
                vResult.To = vBoundary;
            }
        )?    
    ;
    
//////////////////////////////////////////////////////////////////////
// Alter Database
//////////////////////////////////////////////////////////////////////
alterDatabaseStatements returns [TSqlStatement vResult = null]
    : tAlter:Alter Database 
        (
            // Conflicts with alterDatabase alternative below
            {NextTokenMatches(CodeGenerationSupporter.Audit) && NextTokenMatches(CodeGenerationSupporter.Specification, 2)}? 
            vResult = alterDatabaseAuditSpecification[tAlter]
        |
            vResult = alterDatabase[tAlter]
        |
            vResult = alterDatabaseEncryptionKey[tAlter]
        )
    ;
    
alterDatabase [IToken tAlter] returns [AlterDatabaseStatement vResult = null]
{
    Identifier vIdentifier = null;
    bool vUseCurrent = false;
}
    :    ( 
            vIdentifier=identifier 
        | 
            vIdentifier=sqlCommandIdentifier
        |
            tCurrent:Current
            {
                vUseCurrent=true;
            }
        )
        ( vResult = alterDbAdd
        | {NextTokenMatches(CodeGenerationSupporter.Remove)}?
          vResult = alterDbRemove
        | {NextTokenMatches(CodeGenerationSupporter.Modify)}? 
          vResult = alterDbModify
        | vResult = alterDbSet
        | vResult = alterDbCollate
        | vResult = alterDbRebuild // Undocumented - for PSS only
        )
        {
            if(vUseCurrent)
            {
                vResult.UseCurrent = true;
                UpdateTokenInfo(vResult,tCurrent);
            }
            else
            {
                vResult.DatabaseName = vIdentifier;
            }
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
    
alterDbRebuild returns [AlterDatabaseRebuildLogStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseRebuildLogStatement>()]
{
    FileDeclaration vFileDeclaration;
}
    : tRebuild:Identifier tLog:Identifier 
        {
            Match(tRebuild, CodeGenerationSupporter.Rebuild);
            Match(tLog, CodeGenerationSupporter.Log);
            UpdateTokenInfo(vResult,tLog);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (On vFileDeclaration = fileDecl[false]
            {
                vResult.FileDeclaration = vFileDeclaration;
            }
        )?
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
        (Contains tFileStreamOrMemoryOptimizedData:Identifier
            {
                if (TryMatch(tFileStreamOrMemoryOptimizedData, CodeGenerationSupporter.FileStream))
                {                    
                    vResult.ContainsFileStream = true;;
                }
                else
                {
                    Match(tFileStreamOrMemoryOptimizedData, CodeGenerationSupporter.MemoryOptimizedData);                
                    vResult.ContainsMemoryOptimizedData = true;
                }
                UpdateTokenInfo(vResult, tFileStreamOrMemoryOptimizedData);
            }
        )? 
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
        |    vResult = alterDbModifyAzureOptions
        )
    ;

alterDbModifyAzureOptions returns [AlterDatabaseSetStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseSetStatement>()]
    :
        azureOptions[vResult, vResult.Options]
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
    AlterDatabaseTermination vTermination;
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
                    
                    vResult.UpdatabilityOption = ModifyFilegroupOptionsHelper.Instance.ParseOption(tUpdatabilityOption, SqlVersionFlags.TSql120);
                    UpdateTokenInfo(vResult,tUpdatabilityOption);
                }
                (
                    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                    options {greedy = true; } :
                    vTermination=xactTermination
                    {
                        vResult.Termination = vTermination;
                    }
                )?
            )
        )
    ;
    
alterDbSet returns [AlterDatabaseSetStatement vResult]
{
    AlterDatabaseTermination vTermination;
}
    : Set vResult = dbOptionStateList 
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
                    // Greedy due to conflict with identifierStatements
                    (options {greedy=true;} : tSeconds:Identifier
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
    ulong encounteredOptions = 0;
}
    : vOption = dbOptionStateItem[ref encounteredOptions]
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
        }
        (Comma vOption = dbOptionStateItem[ref encounteredOptions]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )*
    ;
    
dbOptionStateItem[ref ulong encounteredOptions] returns [DatabaseOption vResult = null]
    :    
    {NextTokenMatches(CodeGenerationSupporter.CursorDefault)}? 
        vResult = cursorDefaultDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.Recovery)}? 
        vResult = recoveryDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.TargetRecoveryTime)}?
        vResult = targetRecoveryTimeDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.PageVerify)}? 
        vResult = pageVerifyDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.Partner)}? 
        vResult = partnerOption 
    |    {NextTokenMatches(CodeGenerationSupporter.Witness)}? 
        vResult = witnessOption
    |    {NextTokenMatches(CodeGenerationSupporter.Parameterization)}? 
        vResult = parameterizationOption
    |    {NextTokenMatches(CodeGenerationSupporter.CompatibilityLevel)}?
        vResult = compatibilityLevelDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.ChangeTracking)}?
        vResult = changeTrackingDbOption
    |   {NextTokenMatches(CodeGenerationSupporter.Containment)}?
        vResult = dbContainmentOption
    |    {NextTokenMatches(CodeGenerationSupporter.Hadr)}?
        vResult = hadrDbOption
    |    {NextTokenMatches(CodeGenerationSupporter.DelayedDurability)}?
        vResult = dbDelayedDurabilityOption
    |     {NextTokenMatches(CodeGenerationSupporter.AutoCreateStatistics)}? 
        vResult = autoCreateStatisticsDbOption
    |    {NextTokenMatchesOneOf(OptionValidForCreateDatabase())}?
        vResult = createAlterDbOption[ref encounteredOptions]
    |    vResult = dbSingleIdentOption // <db_state_option>, <db_user_access_option>, <db_update_option>, <service_broker_option>
    |    vResult = alterDbOnOffOption
    ;
    
dbSingleIdentOption returns [DatabaseOption vResult = FragmentFactory.CreateFragment<DatabaseOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind = SimpleDbOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
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

targetRecoveryTimeDbOption returns [TargetRecoveryTimeDatabaseOption vResult = FragmentFactory.CreateFragment<TargetRecoveryTimeDatabaseOption>()]
{
    Literal vRecoveryTime;
}
    : tTargetRecoveryTime:Identifier EqualsSign vRecoveryTime = integer tUnit:Identifier
    {
        Match(tTargetRecoveryTime,CodeGenerationSupporter.TargetRecoveryTime);
        vResult.OptionKind = DatabaseOptionKind.TargetRecoveryTime;
        vResult.RecoveryTime = vRecoveryTime;
        vResult.Unit = TargetRecoveryTimeUnitHelper.Instance.ParseOption(tUnit);
        UpdateTokenInfo(vResult,tUnit);
    }
    ;

pageVerifyDbOption returns [PageVerifyDatabaseOption vResult = FragmentFactory.CreateFragment<PageVerifyDatabaseOption>()]
    : tPageVerify:Identifier tCheckshumTornPageDetectionNone:Identifier
        {
            Match(tPageVerify,CodeGenerationSupporter.PageVerify);
            vResult.OptionKind = DatabaseOptionKind.PageVerify;
            vResult.Value = PageVerifyDbOptionsHelper.Instance.ParseOption(tCheckshumTornPageDetectionNone);
            UpdateTokenInfo(vResult,tCheckshumTornPageDetectionNone);
        }
    ;
 
autoCreateStatisticsDbOption returns [AutoCreateStatisticsDatabaseOption vResult = FragmentFactory.CreateFragment<AutoCreateStatisticsDatabaseOption>()]
{
    OptionState vOptionState;
    OptionState vOptionIncremental;
}
    : tAutoCreateStatistics:Identifier
        {
            Match(tAutoCreateStatistics, CodeGenerationSupporter.AutoCreateStatistics);
            vResult.OptionKind = DatabaseOptionKind.AutoCreateStatistics;
            UpdateTokenInfo(vResult,tAutoCreateStatistics);
            vResult.HasIncremental = false;
        }
        vOptionState = optionOnOff[vResult]
            {
                vResult.OptionState = vOptionState;
            }
        (   
            LeftParenthesis tIncremental:Identifier EqualsSign vOptionIncremental = optionOnOff[vResult]
            {
                if(vOptionState != OptionState.On)
                {
                    ThrowIncorrectSyntaxErrorException(tIncremental);
                }
                Match(tIncremental, CodeGenerationSupporter.Incremental);
                vResult.IncrementalState = vOptionIncremental;
                vResult.HasIncremental = true;
            }
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen);
            }
        )?
    ;

alterDbOnOffOption returns [OnOffDatabaseOption vResult = FragmentFactory.CreateFragment<OnOffDatabaseOption>()]
{
    OptionState vOptionState;
    bool vHasEquals = false;
}
    : tOption:Identifier
        {
            vResult.OptionKind = OnOffSimpleDbOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
        }
        (
            EqualsSign
            {
                vHasEquals = true;
            }
        )?
        vOptionState = optionOnOff[vResult]
        {            
            bool requiresEquals = OnOffSimpleDbOptionsHelper.Instance.RequiresEqualsSign(vResult.OptionKind);
            if((vHasEquals && !requiresEquals) || (!vHasEquals && requiresEquals))
            {
                ThrowIncorrectSyntaxErrorException(tOption);
            }
            vResult.OptionState = vOptionState;
        }
    ;

partnerOption returns [PartnerDatabaseOption vResult = FragmentFactory.CreateFragment<PartnerDatabaseOption>()]
{
    Literal vPartnerServer, vTimeout;
}
    : tPartner:Identifier
        {
            Match(tPartner,CodeGenerationSupporter.Partner);
            vResult.OptionKind = DatabaseOptionKind.Partner;
        }
        ( 
            ( {NextTokenMatches(CodeGenerationSupporter.Timeout)}?
              (tTimeout:Identifier vTimeout = integer
                {
                    Match(tTimeout,CodeGenerationSupporter.Timeout);
                    vResult.Timeout = vTimeout;                    
                    vResult.PartnerOption = PartnerDatabaseOptionKind.Timeout;
                }
              )
            | {NextTokenMatches(CodeGenerationSupporter.Safety)}?
              (tSafety:Identifier 
                {
                    Match(tSafety,CodeGenerationSupporter.Safety);
                }
                    (tFull:Full 
                        {
                            vResult.PartnerOption = PartnerDatabaseOptionKind.SafetyFull;
                            UpdateTokenInfo(vResult,tFull);
                        }
                    | 
                    tOff:Off
                        {
                            vResult.PartnerOption = PartnerDatabaseOptionKind.SafetyOff;
                            UpdateTokenInfo(vResult,tOff);
                        }
                    )
               )
            |
                tPartnerOption:Identifier
                {
                    vResult.PartnerOption = PartnerDbOptionsHelper.Instance.ParseOption(tPartnerOption);
                    UpdateTokenInfo(vResult,tPartnerOption);
                }
            )
        | tOff2:Off
            {
                vResult.PartnerOption = PartnerDatabaseOptionKind.Off;
                UpdateTokenInfo(vResult,tOff2);
            }
        | EqualsSign vPartnerServer = stringLiteral
            {
                vResult.PartnerServer = vPartnerServer;
                vResult.PartnerOption = PartnerDatabaseOptionKind.PartnerServer;
            }
        )
    ;
    
witnessOption returns [WitnessDatabaseOption vResult = FragmentFactory.CreateFragment<WitnessDatabaseOption>()]
{
    Literal vWitnessServer;
}
    : tWitness:Identifier
        {
            Match(tWitness,CodeGenerationSupporter.Witness);
            vResult.OptionKind = DatabaseOptionKind.Witness;
        }
        (
            tOff:Off
            {
                vResult.IsOff = true;
                UpdateTokenInfo(vResult,tOff);
            }
        | 
            EqualsSign vWitnessServer = stringLiteral
            {
                vResult.WitnessServer = vWitnessServer;
            }
        )
    ;

parameterizationOption returns [ParameterizationDatabaseOption vResult = FragmentFactory.CreateFragment<ParameterizationDatabaseOption>()]
    : tParameterization:Identifier tSimpleForced:Identifier
        {
            Match(tParameterization,CodeGenerationSupporter.Parameterization);
            vResult.OptionKind = DatabaseOptionKind.Parameterization;
            if (TryMatch(tSimpleForced, CodeGenerationSupporter.Simple))
                vResult.IsSimple = true;
            else
            {
                Match(tSimpleForced, CodeGenerationSupporter.Forced);
                vResult.IsSimple = false;
            }
            UpdateTokenInfo(vResult,tSimpleForced);
        }
    ;

compatibilityLevelDbOption returns [LiteralDatabaseOption vResult = FragmentFactory.CreateFragment<LiteralDatabaseOption>()]
{
    Literal vCompatLevel;
}
    : tCompatibilityLevel:Identifier EqualsSign vCompatLevel = integer
        {
            Match(tCompatibilityLevel, CodeGenerationSupporter.CompatibilityLevel);
            vResult.OptionKind = DatabaseOptionKind.CompatibilityLevel;
            UpdateTokenInfo(vResult, tCompatibilityLevel);
            vResult.Value = vCompatLevel;
        }
    ;
    
changeTrackingDbOption returns [ChangeTrackingDatabaseOption vResult = FragmentFactory.CreateFragment<ChangeTrackingDatabaseOption>()]
    : tChangeTracking:Identifier
        {
            Match(tChangeTracking, CodeGenerationSupporter.ChangeTracking);
            vResult.OptionKind = DatabaseOptionKind.ChangeTracking;
            UpdateTokenInfo(vResult, tChangeTracking);
        }
        (
            (EqualsSign tOff:Off
                {
                    vResult.OptionState = OptionState.Off;
                    UpdateTokenInfo(vResult, tOff);
                }
            )
        |
            (EqualsSign tOn:On 
                {
                    vResult.OptionState = OptionState.On;
                    UpdateTokenInfo(vResult, tOn);
                }
                (
                    // Conflicts with Select, which can also start from '(', so making it greedy...
                    options { greedy = true; } :  
                    changeTrackingOnOptions[vResult]
                )?
            )
        |
            changeTrackingOnOptions[vResult]
        )
    ;
    
changeTrackingOnOptions [ChangeTrackingDatabaseOption vParent]
{
    bool autoCleanupEncountered = false;
    bool changeRetentionEncountered = false;
    ChangeTrackingOptionDetail vDetail;
}
    :
        LeftParenthesis vDetail = changeTrackingOneOption[ref autoCleanupEncountered, ref changeRetentionEncountered]
            {
                AddAndUpdateTokenInfo(vParent, vParent.Details, vDetail);
            }
            (Comma vDetail = changeTrackingOneOption[ref autoCleanupEncountered, ref changeRetentionEncountered]
                {
                    AddAndUpdateTokenInfo(vParent, vParent.Details, vDetail);
                }
            )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;
    
changeTrackingOneOption [ref bool autoCleanupEncountered, ref bool changeRetentionEncountered] returns [ChangeTrackingOptionDetail vResult = null]
{
    Literal vRetentionPeriod;
}
    :    tAutoCleanupChangeRetention:Identifier EqualsSign
        (
            tOn:On
            {
                vResult = CreateAutoCleanupDetail(tAutoCleanupChangeRetention, tOn, ref autoCleanupEncountered);
            }
        |
            tOff:Off
            {
                vResult = CreateAutoCleanupDetail(tAutoCleanupChangeRetention, tOff, ref autoCleanupEncountered);
            }
        |
            vRetentionPeriod = integer tUnits:Identifier
            {
                Match(tAutoCleanupChangeRetention, CodeGenerationSupporter.ChangeRetention);
                if (changeRetentionEncountered)
                {
                    ThrowParseErrorException("SQL46050", tAutoCleanupChangeRetention,
                        TSqlParserResource.SQL46050Message, tAutoCleanupChangeRetention.getText());
                }                
                changeRetentionEncountered = true;
                ChangeRetentionChangeTrackingOptionDetail changeRetention = FragmentFactory.CreateFragment<ChangeRetentionChangeTrackingOptionDetail>();
                changeRetention.Unit = RetentionUnitHelper.Instance.ParseOption(tUnits);
                changeRetention.RetentionPeriod = vRetentionPeriod;
                
                UpdateTokenInfo(changeRetention, tAutoCleanupChangeRetention);
                UpdateTokenInfo(changeRetention, tUnits);
                vResult = changeRetention;
            }
        )
    ;

dbContainmentOption returns [ContainmentDatabaseOption vResult = FragmentFactory.CreateFragment<ContainmentDatabaseOption>()]
    : tContainment:Identifier EqualsSign
        {
            Match(tContainment, CodeGenerationSupporter.Containment);
            UpdateTokenInfo(vResult, tContainment);
            vResult.OptionKind=DatabaseOptionKind.Containment;
        }
        tContainmentKind:Identifier
        {
            vResult.Value=ContainmentOptionKindHelper.Instance.ParseOption(tContainmentKind);
            UpdateTokenInfo(vResult,tContainmentKind);
        }
    ;

hadrDbOption returns [DatabaseOption vResult = null]
    : tHadr:Identifier 
        (
            {NextTokenMatches(CodeGenerationSupporter.Availability)}?
            vResult = hadrAvailabilityDbOption
        |
            vResult = simpleHadrDbOption
        )
        {
            Match(tHadr, CodeGenerationSupporter.Hadr);
            vResult.OptionKind = DatabaseOptionKind.Hadr;
            UpdateTokenInfo(vResult, tHadr);
        }
    ;

hadrAvailabilityDbOption returns [HadrAvailabilityGroupDatabaseOption vResult = FragmentFactory.CreateFragment<HadrAvailabilityGroupDatabaseOption>()]
{
    Identifier vGroupName;
}    
    : tAvailability:Identifier Group EqualsSign vGroupName = identifier
        {
            Match(tAvailability, CodeGenerationSupporter.Availability);
            vResult.GroupName=vGroupName;
            vResult.HadrOption = HadrDatabaseOptionKind.AvailabilityGroup;
        }
    ;
  
simpleHadrDbOption returns [HadrDatabaseOption vResult = FragmentFactory.CreateFragment<HadrDatabaseOption>()]
    : 
        tOption:Identifier
        {
            if(TryMatch(tOption, CodeGenerationSupporter.Resume))
            {
                vResult.HadrOption = HadrDatabaseOptionKind.Resume;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.Suspend);
                vResult.HadrOption = HadrDatabaseOptionKind.Suspend;
            }
            UpdateTokenInfo(vResult, tOption);
        }
    | 
        tOff:Off
        {
            vResult.HadrOption = HadrDatabaseOptionKind.Off;
            UpdateTokenInfo(vResult, tOff);
        }
    ;

dbDelayedDurabilityOption returns [DelayedDurabilityDatabaseOption vResult = FragmentFactory.CreateFragment<DelayedDurabilityDatabaseOption>()]
    : tDelayedDurability:Identifier EqualsSign tDelayedDurabilityKind:Identifier
        {
            Match(tDelayedDurability, CodeGenerationSupporter.DelayedDurability);
            vResult.OptionKind=DatabaseOptionKind.DelayedDurability;
            vResult.Value=DelayedDurabilityOptionKindHelper.Instance.ParseOption(tDelayedDurabilityKind);
            UpdateTokenInfo(vResult, tDelayedDurabilityKind);
        }
    ;

//////////////////////////////////////////////////////////////////////
// Alter Database End
//////////////////////////////////////////////////////////////////////

alterDatabaseAuditSpecification [IToken tAlter] returns [AlterDatabaseAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseAuditSpecificationStatement>()]
{
    Identifier vAuditSpecName;
    AuditSpecificationPart vPart;
}
    :    tAudit:Identifier tSpecification:Identifier vAuditSpecName = identifier
        {
            UpdateTokenInfo(vResult, tAlter);
            Match(tAudit, CodeGenerationSupporter.Audit);
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.SpecificationName = vAuditSpecName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (auditSpecificationForClause[vResult])?
        ( // Conflicts with Add SIGNATURE and Drop statements
            ((Add|Drop) LeftParenthesis) => 
            vPart = auditSpecificationDetailDb
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
            }
            (Comma vPart = auditSpecificationDetailDb
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
                }
            )*
        )?
        auditSpecificationStateOpt[vResult]
    ;
    
alterDatabaseEncryptionKey [IToken tAlter] returns [AlterDatabaseEncryptionKeyStatement vResult = FragmentFactory.CreateFragment<AlterDatabaseEncryptionKeyStatement>()]
{
    CryptoMechanism vEncryptor;
}
    :    tEncryption:Identifier Key
        {
            UpdateTokenInfo(vResult, tAlter);
            Match(tEncryption, CodeGenerationSupporter.Encryption);
        }
        (
            (tRegenerate:Identifier With tAlgorithm:Identifier EqualsSign tAlgorithmKind:Identifier 
                {
                    Match(tRegenerate, CodeGenerationSupporter.Regenerate);
                    Match(tAlgorithm, CodeGenerationSupporter.Algorithm);
                    
                    vResult.Regenerate = true;
                    vResult.Algorithm = DatabaseEncryptionKeyAlgorithmHelper.Instance.ParseOption(tAlgorithmKind);
                    UpdateTokenInfo(vResult, tAlgorithmKind);
                }
                ( {NextTokenMatches(CodeGenerationSupporter.Encryption)&& LA(2) == By}?
                    tEncryption2:Identifier By vEncryptor = dekEncryptor
                    {
                        Match(tEncryption2, CodeGenerationSupporter.Encryption);
                        vResult.Encryptor = vEncryptor;
                    }
                )?
            )
            |
            (tEncryption3:Identifier By vEncryptor = dekEncryptor
                {
                    Match(tEncryption3, CodeGenerationSupporter.Encryption);
                    vResult.Encryptor = vEncryptor;
                }
            )
        )
    ;

//////////////////////////////////////////////////////////////////////
// Create Database
//////////////////////////////////////////////////////////////////////

createDatabaseStatements returns [TSqlStatement vResult = null]
    :
        tCreate:Create Database
        (
            // Conflicts with createDatabase alternative below
            {NextTokenMatches(CodeGenerationSupporter.Audit) && NextTokenMatches(CodeGenerationSupporter.Specification, 2)}? 
            vResult = createDatabaseAuditSpecification
        |
            vResult = createDatabase
        |    
            vResult = createDatabaseEncryptionKey
        )
        {
            if (vResult != null)
                UpdateTokenInfo(vResult,tCreate);
        }
    ;

createDatabase returns [CreateDatabaseStatement vResult = FragmentFactory.CreateFragment<CreateDatabaseStatement>()]
{
    Identifier vIdentifier;
    ContainmentDatabaseOption vContainment;
}
    : vIdentifier=identifier 
        {
            vResult.DatabaseName = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
      (
        (
            {NextTokenMatches(CodeGenerationSupporter.Copy, 2)}?
            azureAsCopyOf[vResult]
        )?
        (
            {NextTokenMatches(CodeGenerationSupporter.MaxSize,2) || NextTokenMatches(CodeGenerationSupporter.Edition,2) || NextTokenMatches(CodeGenerationSupporter.ServiceObjective,2)}?
            azureOptions[vResult, vResult.Options]
        )?
         (
            vContainment = dbContainmentOption
            {
                  vResult.Containment = vContainment;
              }
          )?
        recoveryUnitList[vResult] 
        collationOpt[vResult]
        (
            dbAddendums[vResult]
        )? 
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            withCreateDbOptions[vResult]
        )?
      )
    ;

azureOptions[TSqlFragment vParent, IList<DatabaseOption> vOptions]
{
    DatabaseOption vOption;
    long encounteredOptions = 0;
}
    : LeftParenthesis vOption=azureOption
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vOptions, vOption);
        }
        (
            Comma vOption=azureOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vOptions, vOption);
            }
        )*
        tRightParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRightParen);
        }
    ;

azureOption returns [DatabaseOption vResult]
    :  
       {NextTokenMatches(CodeGenerationSupporter.MaxSize)}?
       vResult=azureMaxSizeDatabaseOption
     | {NextTokenMatches(CodeGenerationSupporter.Edition)}?
       vResult=azureEditionDatabaseOption
     | {LA(3) == Identifier}?
       vResult=azureElasticPoolServiceObjectiveDatabaseOption
     | vResult=azureServiceObjectiveDatabaseOption
    ;

azureMaxSizeDatabaseOption returns [MaxSizeDatabaseOption vResult = FragmentFactory.CreateFragment<MaxSizeDatabaseOption>()]
{
    Literal vSize;
} 
    : tMaxSize:Identifier EqualsSign vSize=integer tUnit:Identifier
        {
            Match(tMaxSize, CodeGenerationSupporter.MaxSize);
            vResult.OptionKind = DatabaseOptionKind.MaxSize;
            UpdateTokenInfo(vResult, tMaxSize);
            vResult.MaxSize=vSize;
            Match(tUnit, CodeGenerationSupporter.GB, CodeGenerationSupporter.MB);
            if (TryMatch(tUnit, CodeGenerationSupporter.GB))
            {
                vResult.Units=MemoryUnit.GB;
            }
            else
            {
                Match(tUnit, CodeGenerationSupporter.MB);
                vResult.Units=MemoryUnit.MB;
            }           
            UpdateTokenInfo(vResult, tUnit);
        }
    ;

azureEditionDatabaseOption returns [LiteralDatabaseOption vResult=FragmentFactory.CreateFragment<LiteralDatabaseOption>()]
{
    Literal vValue;
}
    : tEdition:Identifier EqualsSign vValue=stringLiteral
        {
            Match(tEdition, CodeGenerationSupporter.Edition);
            UpdateTokenInfo(vResult, tEdition);
            vResult.OptionKind = DatabaseOptionKind.Edition;
            vResult.Value = vValue;
        }
    ;

azureServiceObjectiveDatabaseOption returns [LiteralDatabaseOption vResult=FragmentFactory.CreateFragment<LiteralDatabaseOption>()]
{
    Literal vValue;
}
    : tServiceObjective:Identifier EqualsSign vValue=stringLiteral
        {
            Match(tServiceObjective, CodeGenerationSupporter.ServiceObjective);
            UpdateTokenInfo(vResult, tServiceObjective);
            vResult.OptionKind = DatabaseOptionKind.ServiceObjective;
            vResult.Value = vValue;
        }
    ;

azureElasticPoolServiceObjectiveDatabaseOption returns [ElasticPoolSpecification vResult=FragmentFactory.CreateFragment<ElasticPoolSpecification>()]
{
    Identifier vIdentifier;
}
    : tServiceObjective:Identifier EqualsSign tElasticPool:Identifier LeftParenthesis tName:Identifier EqualsSign vIdentifier=identifier RightParenthesis
        {
            Match(tServiceObjective, CodeGenerationSupporter.ServiceObjective);
            Match(tElasticPool, CodeGenerationSupporter.ElasticPool);
            Match(tName, CodeGenerationSupporter.Name);
            vResult.ElasticPoolName = vIdentifier;
            vResult.OptionKind = DatabaseOptionKind.ServiceObjective;
            UpdateTokenInfo(vResult, tServiceObjective);
        }
    ;

azureAsCopyOf[CreateDatabaseStatement vParent]
{
    MultiPartIdentifier vSourceDatabase;
}    
    : As tCopy:Identifier Of vSourceDatabase=multiPartIdentifier[2]
        { 
            Match(tCopy, CodeGenerationSupporter.Copy);
            vParent.CopyOf=vSourceDatabase;
        }
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
    FileGroupDefinition vFileGroup;
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
            | vFileGroup = fileGroupDecl
                {
                    currentFilegroup = vFileGroup;
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
        (Contains tFileStreamOrMemoryOptimizedData:Identifier
            {
                if (TryMatch(tFileStreamOrMemoryOptimizedData, CodeGenerationSupporter.FileStream))
                {
                    vResult.ContainsFileStream = true;;
                }
                else
                {
                    Match(tFileStreamOrMemoryOptimizedData, CodeGenerationSupporter.MemoryOptimizedData);                                    
                    vResult.ContainsMemoryOptimizedData = true;
                }
                UpdateTokenInfo(vResult, tFileStreamOrMemoryOptimizedData);
            }
        )?  
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
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            UpdateTokenInfo(vResult,tLParen);
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
    |    {NextTokenMatches(CodeGenerationSupporter.Offline)}?
        vResult = offlineFileOption
    ;
    
nameFileOption returns [NameFileDeclarationOption vResult = FragmentFactory.CreateFragment<NameFileDeclarationOption>()]
{
    IdentifierOrValueExpression vValue;
}
    : tName:Identifier EqualsSign vValue = nonEmptyStringOrIdentifier
        {
            Match(tName,CodeGenerationSupporter.Name);
            vResult.OptionKind=FileDeclarationOptionKind.Name;
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
            Match(tFileName,CodeGenerationSupporter.FileName);
            vResult.OptionKind=FileDeclarationOptionKind.FileName;
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
            Match(tSize,CodeGenerationSupporter.Size);
            vResult.OptionKind=FileDeclarationOptionKind.Size;
            UpdateTokenInfo(vResult, tSize);
            vResult.Size = vValue;
        }
        (vUnits = memUnit[vResult]
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
            UpdateTokenInfo(vResult, tMaxSize);
            vResult.OptionKind=FileDeclarationOptionKind.MaxSize;
        }
        (
            (
                vValue = integer
                {
                    vResult.MaxSize = vValue;
                }
                (vUnits = memUnit[vResult]
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
            UpdateTokenInfo(vResult, tFileGrowth);
            vResult.OptionKind=FileDeclarationOptionKind.FileGrowth;
        }
        (
            vValue = integer
            {
                vResult.GrowthIncrement = vValue;
            }
            (
                (
                    vUnits = memUnit[vResult]
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
    
offlineFileOption returns [FileDeclarationOption vResult = FragmentFactory.CreateFragment<FileDeclarationOption>()]
    : tOffline:Identifier
        {
            Match(tOffline,CodeGenerationSupporter.Offline);
            vResult.OptionKind=FileDeclarationOptionKind.Offline;
            UpdateTokenInfo(vResult,tOffline);
        }
    ;
    
memUnit [TSqlFragment vParent] returns [MemoryUnit vResult = MemoryUnit.Unspecified]
    : tUnit:Identifier
        {
            vResult = MemoryUnitsHelper.Instance.ParseOption(tUnit);
            UpdateTokenInfo(vParent, tUnit);
        }
    ;

dbAddendums [CreateDatabaseStatement vParent]
{
    Identifier vIdentifier;
}
    : (For tAttach:Identifier 
        {
            vParent.AttachMode = AttachModeHelper.Instance.ParseOption(tAttach);
            if (vParent.AttachMode == AttachMode.Load)
                ThrowIncorrectSyntaxErrorException(tAttach);
                
            UpdateTokenInfo(vParent,tAttach);
        }
      )
    | As tSnapshot:Identifier Of vIdentifier = identifier
        {
            Match(tSnapshot,CodeGenerationSupporter.Snapshot);
            vParent.DatabaseSnapshot = vIdentifier;
        }
    ;
    
withCreateDbOptions [CreateDatabaseStatement vParent]
{
    DatabaseOption vOption;
    ulong encounteredOptions = 0;
}
    : 
    With vOption = createAlterDbOption[ref encounteredOptions] 
        {
            AddAndUpdateTokenInfo<DatabaseOption>(vParent, vParent.Options, vOption);
        }
        (Comma vOption = createAlterDbOption[ref encounteredOptions] 
            {
                AddAndUpdateTokenInfo<DatabaseOption>(vParent, vParent.Options, vOption);
            }
        )*
    ;

createDbServiceBrokerOption returns [DatabaseOption vResult = FragmentFactory.CreateFragment<DatabaseOption>()]
{
    ServiceBrokerOption vServiceBrokerOption;
}
    :
        tServiceBrokerOption:Identifier
        {
            vServiceBrokerOption = ServiceBrokerOptionsHelper.Instance.ParseOption(tServiceBrokerOption);
            switch(vServiceBrokerOption)
            {
                case ServiceBrokerOption.EnableBroker:
                    vResult.OptionKind = DatabaseOptionKind.EnableBroker;
                    break;
                case ServiceBrokerOption.NewBroker:
                    vResult.OptionKind = DatabaseOptionKind.NewBroker;
                    break;
                case ServiceBrokerOption.ErrorBrokerConversations:
                    vResult.OptionKind = DatabaseOptionKind.ErrorBrokerConversations;
                    break;
            }
            UpdateTokenInfo(vResult,tServiceBrokerOption);
        }
    ;
    
createAlterDbOption [ref ulong encounteredOptions] returns [DatabaseOption vResult]
    :
        (
          {NextTokenMatches(CodeGenerationSupporter.RestrictedUser)}?
          vResult = restrictedUserCreateDbOption
        |
          {NextTokenMatches(CodeGenerationSupporter.DbChaining) || NextTokenMatches(CodeGenerationSupporter.Trustworthy)}?
          vResult = createDbOnOffOption
        | 
            {NextTokenMatches(CodeGenerationSupporter.FileStream)}?
          vResult=fileStreamCreateAlterDbOption    
        |
          vResult = createDbServiceBrokerOption
        | 
          vResult = createAlterDbEqualsSignOption
        )
        {
          CheckOptionDuplication(ref encounteredOptions, (int)vResult.OptionKind, vResult);
        }
    ;

createDbOnOffOption returns [OnOffDatabaseOption vResult = FragmentFactory.CreateFragment<OnOffDatabaseOption>()]
{
    OptionState vOptionState;
}
    : tOption:Identifier vOptionState = optionOnOff[vResult]
        {
            if (TryMatch(tOption, CodeGenerationSupporter.DbChaining))
            {
                   vResult.OptionKind = DatabaseOptionKind.DBChaining;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.Trustworthy);
                vResult.OptionKind=DatabaseOptionKind.Trustworthy;
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.OptionState = vOptionState;
        }
    ;

createAlterDbEqualsSignOption returns [DatabaseOption vResult]
    : tOption:Identifier EqualsSign
    (
        vResult = createAlterDbLiteralOption[tOption]
        | vResult = createAlterDbIdentifierOption[tOption]
        | vResult = createAlterDbOnOffOption[tOption]
    )
    ;    
    
createAlterDbOnOffOption [IToken tOption] returns [OnOffDatabaseOption vResult = FragmentFactory.CreateFragment<OnOffDatabaseOption>()]
{
    OptionState vOptionState;
}
    : vOptionState = optionOnOff[vResult]
        {
            if(TryMatch(tOption, CodeGenerationSupporter.NestedTriggers))
            {
                vResult.OptionKind = DatabaseOptionKind.NestedTriggers;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.TransformNoiseWords);
                vResult.OptionKind = DatabaseOptionKind.TransformNoiseWords;
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.OptionState = vOptionState;
        }
    ;
    
createAlterDbLiteralOption [IToken tOption] returns [LiteralDatabaseOption vResult = FragmentFactory.CreateFragment<LiteralDatabaseOption>()]
{
    IntegerLiteral vIntLiteral;
}
    : vIntLiteral = integer
        {
            if(TryMatch(tOption, CodeGenerationSupporter.DefaultLanguage))
            {
                vResult.OptionKind = DatabaseOptionKind.DefaultLanguage;
                CheckIfValidLanguageInteger(vIntLiteral);
            }
            else if(TryMatch(tOption, CodeGenerationSupporter.DefaultFullTextLanguage))
            {
                vResult.OptionKind = DatabaseOptionKind.DefaultFullTextLanguage;
                CheckIfValidLanguageInteger(vIntLiteral);
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.TwoDigitYearCutoff);
                vResult.OptionKind = DatabaseOptionKind.TwoDigitYearCutoff;
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.Value = vIntLiteral;
        }
    ;

createAlterDbIdentifierOption [IToken tOption] returns [IdentifierDatabaseOption vResult = FragmentFactory.CreateFragment<IdentifierDatabaseOption>()]
{
    Identifier vIdentifier;
}
    : vIdentifier = identifier
        {
            if(TryMatch(tOption, CodeGenerationSupporter.DefaultLanguage))
            {
                vResult.OptionKind=DatabaseOptionKind.DefaultLanguage;
                CheckIfValidLanguageIdentifier(vIdentifier);
            }
            else 
            {
                Match(tOption, CodeGenerationSupporter.DefaultFullTextLanguage);
                vResult.OptionKind=DatabaseOptionKind.DefaultFullTextLanguage;
                CheckIfValidLanguageIdentifier(vIdentifier);
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.Value = vIdentifier;
        }
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

restrictedUserCreateDbOption returns [DatabaseOption vResult = FragmentFactory.CreateFragment<DatabaseOption>()]
    : tOption:Identifier
        {
            Match(tOption, CodeGenerationSupporter.RestrictedUser);
            vResult.OptionKind = DatabaseOptionKind.RestrictedUser;
            UpdateTokenInfo(vResult,tOption);
        }
    ;
    
fileStreamCreateAlterDbOption returns [FileStreamDatabaseOption vResult = FragmentFactory.CreateFragment<FileStreamDatabaseOption>()]
    : tFileStream:Identifier LeftParenthesis
        {
            Match(tFileStream, CodeGenerationSupporter.FileStream);
            vResult.OptionKind=DatabaseOptionKind.FileStream;
            UpdateTokenInfo(vResult, tFileStream);
        }
        (
            fileStreamOption[vResult]
        )
        (
            Comma fileStreamOption[vResult]
        )*
        tRightParenthesis:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
    ;

fileStreamOption[FileStreamDatabaseOption vParent]
{
    Literal vDirectoryName;
    NonTransactedFileStreamAccess? vNonTransactedFileStreamAccess = null;
}    : tOption:Identifier EqualsSign
        (
            (
                Off
                {
                    vNonTransactedFileStreamAccess = NonTransactedFileStreamAccess.Off;
                }
            |    
                Full
                {
                    vNonTransactedFileStreamAccess = NonTransactedFileStreamAccess.Full;
                }
            |
                tIdentifier:Identifier
                {
                    Match(tIdentifier, CodeGenerationSupporter.ReadOnly);
                    vNonTransactedFileStreamAccess = NonTransactedFileStreamAccess.ReadOnly;
                }
            )
            {
                Match(tOption, CodeGenerationSupporter.NonTransactedAccess);
                if(vParent.NonTransactedAccess != null)
                {
                    ThrowParseErrorException("SQL46049", tOption, TSqlParserResource.SQL46049Message, tOption.getText());
                }
                vParent.NonTransactedAccess=vNonTransactedFileStreamAccess.Value;
            }
        |
            (
                vDirectoryName=stringLiteralOrNull
                {
                    Match(tOption, CodeGenerationSupporter.DirectoryName);
                    if(vParent.DirectoryName != null)
                    {
                        ThrowParseErrorException("SQL46049", tOption, TSqlParserResource.SQL46049Message, tOption.getText());
                    }
                    vParent.DirectoryName=vDirectoryName;
                }
            )
        )
    ;

//////////////////////////////////////////////////////////////////////
// End Create Database
//////////////////////////////////////////////////////////////////////

createDatabaseAuditSpecification returns [CreateDatabaseAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<CreateDatabaseAuditSpecificationStatement>()]
{
    Identifier vAuditSpecName;
    AuditSpecificationPart vPart;
}
    : tAudit:Identifier tSpecification:Identifier vAuditSpecName = identifier 
        {
            Match(tAudit, CodeGenerationSupporter.Audit);
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.SpecificationName = vAuditSpecName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        auditSpecificationForClause[vResult]
        (    // Conflicts with Add SIGNATURE (but it actually shouldn't, k=2 should be enough)
            (Add LeftParenthesis) => 
            vPart = createAuditSpecificationDetailDb
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
            }
            (Comma vPart = createAuditSpecificationDetailDb
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parts, vPart);
                }
            )*
        )?
        auditSpecificationStateOpt[vResult]
    ;

auditSpecificationForClause [AuditSpecificationStatement vParent]
{
    Identifier vAuditName;
}    
    : For tServer:Identifier tAudit:Identifier vAuditName = identifier
        {
            Match(tServer, CodeGenerationSupporter.Server);
            Match(tAudit, CodeGenerationSupporter.Audit);
            vParent.AuditName = vAuditName;
        }
    ;
    
createAuditSpecificationDetailDb returns [AuditSpecificationPart vResult = FragmentFactory.CreateFragment<AuditSpecificationPart>()]
{
    AuditSpecificationDetail vDetail;
}
    : tAdd:Add LeftParenthesis 
        (
            vDetail = auditActionSpecification
        |
            vDetail = databaseAuditActionGroup
        )
        tRParen:RightParenthesis
        {
            vResult.Details = vDetail;
            vResult.IsDrop = false;
            UpdateTokenInfo(vResult, tAdd);
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

createAuditSpecificationDetail returns [AuditSpecificationPart vResult = FragmentFactory.CreateFragment<AuditSpecificationPart>()]
{
    AuditSpecificationDetail vDetail;
}
    : tAdd:Add LeftParenthesis vDetail = serverAuditActionGroup tRParen:RightParenthesis
        {
            vResult.Details = vDetail;
            vResult.IsDrop = false;
            UpdateTokenInfo(vResult, tAdd);
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
auditSpecificationDetailDb returns [AuditSpecificationPart vResult = FragmentFactory.CreateFragment<AuditSpecificationPart>()]
{
    AuditSpecificationDetail vDetail;
}
    :   (
            tAdd:Add 
            {
                UpdateTokenInfo(vResult, tAdd);
                vResult.IsDrop = false;
            }
        |
            tDrop:Drop
            {
                UpdateTokenInfo(vResult, tDrop);
                vResult.IsDrop = true;
            }
        )
        LeftParenthesis 
        (
            vDetail = auditActionSpecification
        |
            vDetail = databaseAuditActionGroup
        )
        tRParen:RightParenthesis
        {
            vResult.Details = vDetail;
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

auditSpecificationDetail returns [AuditSpecificationPart vResult = FragmentFactory.CreateFragment<AuditSpecificationPart>()]
{
    AuditSpecificationDetail vDetail;
}
    :   (
            tAdd:Add 
            {
                UpdateTokenInfo(vResult, tAdd);
                vResult.IsDrop = false;
            }
        |
            tDrop:Drop
            {
                UpdateTokenInfo(vResult, tDrop);
                vResult.IsDrop = true;
            }
        )
        LeftParenthesis vDetail = serverAuditActionGroup tRParen:RightParenthesis
        {
            vResult.Details = vDetail;
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
        
databaseAuditActionGroup returns [AuditActionGroupReference vResult = FragmentFactory.CreateFragment<AuditActionGroupReference>()]
    :
        tDatabaseAuditActionGroup:Identifier
        {
            vResult.Group = DatabaseAuditActionGroupHelper.Instance.ParseOption(tDatabaseAuditActionGroup, SqlVersionFlags.TSql120);
            UpdateTokenInfo(vResult, tDatabaseAuditActionGroup);
        }
    ;
    
serverAuditActionGroup returns [AuditActionGroupReference vResult = FragmentFactory.CreateFragment<AuditActionGroupReference>()]
    :
        tServerAuditActionGroup:Identifier
        {
            vResult.Group = ServerAuditActionGroupHelper.Instance.ParseOption(tServerAuditActionGroup, SqlVersionFlags.TSql120);
            UpdateTokenInfo(vResult, tServerAuditActionGroup);
        }
    ;

auditActionSpecification returns [AuditActionSpecification vResult = FragmentFactory.CreateFragment<AuditActionSpecification>()]
{
    SecurityPrincipal vPrincipal;
    SecurityTargetObject vTargetObject;
    DatabaseAuditAction vAction;
}
    :    vAction = actionWithQual 
        {
            AddAndUpdateTokenInfo(vResult, vResult.Actions, vAction);
        }
        (Comma vAction =actionWithQual
            {
                AddAndUpdateTokenInfo(vResult, vResult.Actions, vAction);
            }
        )* 
        vTargetObject = securityTargetObject[true]
        {
            vResult.TargetObject = vTargetObject;
        }
        By vPrincipal=principal
        {
            AddAndUpdateTokenInfo(vResult, vResult.Principals, vPrincipal);
        }
        (
            Comma vPrincipal=principal
            {
                AddAndUpdateTokenInfo(vResult, vResult.Principals, vPrincipal);
            }
        )*
    ;

// Note, olegr: 
// In SQL Server rule 'action_with_qual' 'security_qual_list_with_paren' list 
// must be empty, so it is not copied here...
actionWithQual returns [DatabaseAuditAction vResult = FragmentFactory.CreateFragment<DatabaseAuditAction>()]
    :
        tSelect:Select
        {
            vResult.ActionKind = DatabaseAuditActionKind.Select;
            UpdateTokenInfo(vResult, tSelect);
        }
    |
        tUpdate:Update
        {
            vResult.ActionKind = DatabaseAuditActionKind.Update;
            UpdateTokenInfo(vResult, tUpdate);
        }
    |
        tInsert:Insert
        {
            vResult.ActionKind = DatabaseAuditActionKind.Insert;
            UpdateTokenInfo(vResult, tInsert);
        }
    |
        tDelete:Delete
        {
            vResult.ActionKind = DatabaseAuditActionKind.Delete;
            UpdateTokenInfo(vResult, tDelete);
        }
    |
        tExecute:Execute
        {
            vResult.ActionKind = DatabaseAuditActionKind.Execute;
            UpdateTokenInfo(vResult, tExecute);
        }
    |
        tReceive:Identifier
        {
            Match(tReceive, CodeGenerationSupporter.Receive);
            vResult.ActionKind = DatabaseAuditActionKind.Receive;
            UpdateTokenInfo(vResult, tReceive);
        }
    |
        tReferences:References
        {
            vResult.ActionKind = DatabaseAuditActionKind.References;
            UpdateTokenInfo(vResult, tReferences);
        }
    ;
       
auditSpecificationStateOpt [AuditSpecificationStatement vParent]
{
    OptionState vState;
}
    :    (    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :    
            With LeftParenthesis tState:Identifier EqualsSign vState = optionOnOff[vParent] tRParen:RightParenthesis
            {
                Match(tState, CodeGenerationSupporter.State);
                vParent.AuditState = vState;
                UpdateTokenInfo(vParent, tRParen);
            }
        )?
    ;
    
createDatabaseEncryptionKey returns [CreateDatabaseEncryptionKeyStatement vResult = FragmentFactory.CreateFragment<CreateDatabaseEncryptionKeyStatement>()]
{
    CryptoMechanism vEncryptor;
}
    : tEncryption:Identifier Key With tAlgorithm:Identifier EqualsSign 
        tAlgorithmKind:Identifier tEncryption2:Identifier By vEncryptor = dekEncryptor
        {
            Match(tEncryption, CodeGenerationSupporter.Encryption);
            Match(tEncryption2, CodeGenerationSupporter.Encryption);
            Match(tAlgorithm, CodeGenerationSupporter.Algorithm);
            
            vResult.Algorithm = DatabaseEncryptionKeyAlgorithmHelper.Instance.ParseOption(tAlgorithmKind);
            vResult.Encryptor = vEncryptor;
        }
    ;

dekEncryptor returns [CryptoMechanism vResult = FragmentFactory.CreateFragment<CryptoMechanism>()]
{
    Identifier vIdentifier;
}
    :   dekEncryptorType[vResult] vIdentifier = identifier
        {
            vResult.Identifier = vIdentifier;
        }
    ;

dekEncryptorType [CryptoMechanism vParent]
    :   tServer:Identifier
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        (
            (tCertificate:Identifier 
                {
                    Match(tCertificate, CodeGenerationSupporter.Certificate);
                    vParent.CryptoMechanismType = CryptoMechanismType.Certificate;
                }
            )
            |
            (tAsymmetric:Identifier Key
                {
                    Match(tAsymmetric, CodeGenerationSupporter.Asymmetric);
                    vParent.CryptoMechanismType = CryptoMechanismType.AsymmetricKey;
                }
            )
        )
    ;

//////////////////////////////////////////////////////////////////////
// Backup / Restore
//////////////////////////////////////////////////////////////////////
backupStatements returns [TSqlStatement vResult]
    : tBackup:Backup
        (
            {NextTokenMatches(CodeGenerationSupporter.Certificate)}?
            vResult = backupCertificateStatement
        |    {NextTokenMatches(CodeGenerationSupporter.Service)}?
            vResult = backupServiceMasterKeyStatement
        |    
            vResult = backupMasterKeyStatement
        |
            vResult = backupStatement
        )
        {
            UpdateTokenInfo(vResult,tBackup);
        }
    ;

backupStatement returns [BackupStatement vResult]
    : vResult=backupMain 
        (
            backupDevices[vResult]
        )? 
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            backupOptions[vResult]
        )?
    ;
    
restoreStatements returns [TSqlStatement vResult]
    : tRestore:Restore
        (    {NextTokenMatches(CodeGenerationSupporter.Service)}?
            vResult=restoreServiceMasterKeyStatement
        |
            vResult = restoreMasterKeyStatement
        | 
            vResult=restoreStatement
        )
        {
            UpdateTokenInfo(vResult,tRestore);
        }
    ;

restoreStatement returns [RestoreStatement vResult = FragmentFactory.CreateFragment<RestoreStatement>()]
    :  (
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            restoreOptions[vResult]
        )?
    ;
    
backupMain returns [BackupStatement vResult = null]
    : vResult = backupDatabase
    | vResult = backupTransactionLog
//    | Table objectOrVariable // Not yet implemented in SQL Server (although passes parser)
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
    : tLog:Identifier vName = identifierOrVariable
      {
        Match(tLog,CodeGenerationSupporter.Log);
        vResult.DatabaseName = vName;
        ThrowPartialAstIfPhaseOne(vResult);
      }
    ;
    
// Can end with Identifier - and that conflicts with identifierStatements, so making it greedy...
backupFileListOpt [BackupDatabaseStatement vParent]
{
    BackupRestoreFileInfo vFile;
}
    : (options {greedy=true;} : vFile = backupRestoreFile
        {
            AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
        }
        (Comma vFile = backupRestoreFile
            {
                AddAndUpdateTokenInfo(vParent, vParent.Files, vFile);
            }
        )*)?
    ;

// Can end with Identifier - and that conflicts with identifierStatements, so making it greedy...
restoreFileListOpt [RestoreStatement vParent]
{
    BackupRestoreFileInfo vFile;
}
    : (options {greedy=true;} : vFile = backupRestoreFile 
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
    | tReadWriteFilegroups:Identifier
        {
            Match(tReadWriteFilegroups,CodeGenerationSupporter.ReadWriteFilegroups);
            vResult.ItemKind = BackupRestoreItemKind.ReadWriteFileGroups;
            UpdateTokenInfo(vResult,tReadWriteFilegroups);
        }
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
    
backupDevices [BackupStatement vParent]
{
    MirrorToClause vMirrorTo;
}
    : To devList[vParent, vParent.Devices]
        (vMirrorTo = mirrorTo
            {
                AddAndUpdateTokenInfo(vParent, vParent.MirrorToClauses, vMirrorTo);
            }
        )*
    ;
    
mirrorTo returns [MirrorToClause vResult = FragmentFactory.CreateFragment<MirrorToClause>()]
    : 
        tMirror:Identifier To devList[vResult, vResult.Devices]
        {
            Match(tMirror, CodeGenerationSupporter.Mirror);
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
                vResult.DeviceType = DeviceTypesHelper.Instance.ParseOption(tDevType, SqlVersionFlags.TSql120);
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
    
backupOption returns [BackupOption vResult = null]
{
    ScalarExpression vValue;
}
    : 
      {NextTokenMatches(CodeGenerationSupporter.Encryption)}?
      vResult = backupEncryptionOption
    | tSimpleOption:Identifier
        {
            // NO_LOG and TRUNCATE_ONLY are not supported in Katmai, but due to upgrade issues we still support them
            vResult = FragmentFactory.CreateFragment<BackupOption>();
            vResult.OptionKind = BackupOptionsNoValueHelper.Instance.ParseOption(tSimpleOption, SqlVersionFlags.TSql120);
            UpdateTokenInfo(vResult,tSimpleOption);
        }
    | (tOption:Identifier EqualsSign 
        (
            vValue = signedIntegerOrVariable
        |
            vValue = stringLiteral
        )
        {
            vResult = FragmentFactory.CreateFragment<BackupOption>();
            vResult.OptionKind = BackupOptionsWithValueHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
            vResult.Value = vValue;
        }
      )
    ;

backupEncryptionOption returns [BackupEncryptionOption vResult = FragmentFactory.CreateFragment<BackupEncryptionOption>()]
{
    CryptoMechanism vEncryptor;
}
    :   tEncryption:Identifier LeftParenthesis tAlgorithm:Identifier EqualsSign tAlgorithmKind:Identifier
        {
            Match(tEncryption, CodeGenerationSupporter.Encryption);
            Match(tAlgorithm, CodeGenerationSupporter.Algorithm);

            vResult.Algorithm = EncryptionAlgorithmsHelper.Instance.ParseOption(tAlgorithmKind);
        }
        Comma vEncryptor = backupEncrytor tRParen:RightParenthesis
        {
            vResult.Encryptor = vEncryptor;
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

backupEncrytor returns [CryptoMechanism vResult = FragmentFactory.CreateFragment<CryptoMechanism>()]
{
    Identifier vIdentifier;
}
    :   dekEncryptorType[vResult] EqualsSign vIdentifier = identifier
        {
            vResult.Identifier = vIdentifier;
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
    | (tLog:Identifier vName = identifierOrVariable 
        {
            Match(tLog,CodeGenerationSupporter.Log);
            vParent.DatabaseName = vName;
            vParent.Kind = RestoreStatementKind.TransactionLog;
            ThrowPartialAstIfPhaseOne(vParent);
        }
        restoreFileListOpt[vParent]
      )
//    | Table objectOrVariable  // Not yet implemented in SQL Server
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
    ValueExpression vValue,vAfter;
    ScalarExpression vExpression;
}
    : 
      {NextTokenMatches(CodeGenerationSupporter.FileStream)}?
      vResult = fileStreamRestoreOption
    | vResult = simpleRestoreOption
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
            vResult.OptionKind = RestoreOptionNoValueHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
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

fileStreamRestoreOption returns [FileStreamRestoreOption vResult = FragmentFactory.CreateFragment<FileStreamRestoreOption>()]
{
    FileStreamDatabaseOption vFileStreamDatabaseOption;
} 
    : vFileStreamDatabaseOption=fileStreamCreateAlterDbOption
        {
            vResult.OptionKind=RestoreOptionKind.FileStream;
            vResult.FileStreamOption = vFileStreamDatabaseOption;
        }
    ;
    
afterClause returns [ValueExpression vResult = null]
    : tAfter:Identifier vResult = stringOrVariable
        {
            Match(tAfter,CodeGenerationSupporter.After);
        }
    ;    

// Backup CERTIFICATE
backupCertificateStatement returns [BackupCertificateStatement vResult = FragmentFactory.CreateFragment<BackupCertificateStatement>()]
{
    Identifier vName;
    Literal vFile;
}
    : tCertificate:Identifier vName = identifier To File EqualsSign vFile = stringLiteral
        {
            Match(tCertificate,CodeGenerationSupporter.Certificate);
            vResult.Name = vName;
            vResult.File = vFile;
        }
        (    options {greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            With privateKeySpec[vResult]
        )?
    ;
    
// Backup and Restore SERVICE MASTER Key statements
backupServiceMasterKeyStatement returns [BackupServiceMasterKeyStatement vResult = FragmentFactory.CreateFragment<BackupServiceMasterKeyStatement>()]
    : tService:Identifier 
        {
            Match(tService,CodeGenerationSupporter.Service);
        }
        backupMasterKeyBody[vResult]
    ;

restoreServiceMasterKeyStatement returns [RestoreServiceMasterKeyStatement vResult = FragmentFactory.CreateFragment<RestoreServiceMasterKeyStatement>()]
    : tService:Identifier 
        {
            Match(tService,CodeGenerationSupporter.Service);
        }
        restoreMasterKeyBody[vResult]
        (    options {greedy=true;} : // Greedy due to conflict with identifierStatements
            tForce:Identifier
            {
                Match(tForce,CodeGenerationSupporter.Force);
                vResult.IsForce = true;
                UpdateTokenInfo(vResult,tForce);
            }
        )?
    ;
    
restoreMasterKeyBody [BackupRestoreMasterKeyStatementBase vParent]
{
    Literal vFile, vPassword;
}
    : tMaster:Identifier Key From File EqualsSign vFile = stringLiteral tDecryption:Identifier By tPassword:Identifier EqualsSign vPassword = stringLiteral
        {
            Match(tMaster,CodeGenerationSupporter.Master);
            vParent.File = vFile;
            Match(tDecryption,CodeGenerationSupporter.Decryption);
            Match(tPassword,CodeGenerationSupporter.Password);
            vParent.Password= vPassword;            
        }    
    ;
    
// Backup and Restore MASTER Key statements
backupMasterKeyStatement returns [BackupMasterKeyStatement vResult = FragmentFactory.CreateFragment<BackupMasterKeyStatement>()]
    : backupMasterKeyBody[vResult]
    ;
    
backupMasterKeyBody [BackupRestoreMasterKeyStatementBase vParent]
{
    Literal vFile, vPassword;
}
    : tMaster:Identifier Key To File EqualsSign vFile = stringLiteral tEncryption:Identifier By tPassword:Identifier EqualsSign vPassword = stringLiteral
        {
            Match(tMaster,CodeGenerationSupporter.Master);
            vParent.File = vFile;
            Match(tEncryption,CodeGenerationSupporter.Encryption);
            Match(tPassword,CodeGenerationSupporter.Password);
            vParent.Password= vPassword;            
        }
    ;
    
restoreMasterKeyStatement returns [RestoreMasterKeyStatement vResult = FragmentFactory.CreateFragment<RestoreMasterKeyStatement>()]
{
    Literal vPassword;
}
    : restoreMasterKeyBody[vResult] tEncryption:Identifier By tPassword:Identifier EqualsSign vPassword = stringLiteral
        {
            Match(tEncryption,CodeGenerationSupporter.Encryption);
            Match(tPassword,CodeGenerationSupporter.Password);
            vResult.EncryptionPassword = vPassword;            
        }
        (    options {greedy=true;} : // Greedy due to conflict with identifierStatements
            tForce:Identifier
            {
                Match(tForce,CodeGenerationSupporter.Force);
                vResult.IsForce = true;
                UpdateTokenInfo(vResult,tForce);
            }
        )?
    ;
    
//////////////////////////////////////////////////////////////////////
// End Backup / Restore
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
            CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (Comma vOption = bulkInsertOption
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
                UpdateTokenInfo(vResult, tOption);
                vResult.OptionKind = BulkInsertFlagOptionsHelper.Instance.ParseOption(tOption);
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
                vResult.OptionKind = BulkInsertStringOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
                UpdateTokenInfo(vResult, tOption);
                if (vResult.OptionKind == BulkInsertOptionKind.CodePage) // Check for code page string constants
                    MatchString(vValue, CodeGenerationSupporter.ACP, CodeGenerationSupporter.OEM, CodeGenerationSupporter.Raw);
                else if (vResult.OptionKind == BulkInsertOptionKind.DataFileType)
                    MatchString(vValue, CodeGenerationSupporter.Char, CodeGenerationSupporter.Native, 
                            CodeGenerationSupporter.WideChar, CodeGenerationSupporter.WideNative,
                            CodeGenerationSupporter.WideCharAnsi, CodeGenerationSupporter.DTSBuffers);
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
    
openRowsetBulkOrderHint returns [OrderBulkInsertOption vResult]
    :
        vResult = bulkInsertSortOrderOption
        (tUnique:Unique
            {
                vResult.IsUnique = true;
            }
        )?
    ;
    
// End Of Bulk Insert & Insert Bulk

// Dbcc statements
dbccStatement returns [DbccStatement vResult = FragmentFactory.CreateFragment<DbccStatement>()]
    : tDbcc:Dbcc tIdentifier:Identifier 
        {
            DbccCommand command;
            if (DbccCommandsHelper.Instance.TryParseOption(tIdentifier,out command))
            {
                if (command == DbccCommand.ConcurrencyViolation ||
                    command == DbccCommand.MemObjList ||
                    command == DbccCommand.MemoryMap)
                {
                    ThrowIncorrectSyntaxErrorException(tIdentifier);
                }
                    
                vResult.Command = command;
            }
            else
            {
                vResult.Command = DbccCommand.Free;
                vResult.DllName = tIdentifier.getText();
            }
            
            UpdateTokenInfo(vResult,tDbcc);
            UpdateTokenInfo(vResult,tIdentifier);
        }
        (   // Select can start from '(', so, making literal list greedy...
            options { greedy = true; } : 
            dbccNamedLiteralList[vResult]
        )?  
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
            vResult.OptionKind= DbccOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
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

createEndpointStatement returns [CreateEndpointStatement vResult = FragmentFactory.CreateFragment<CreateEndpointStatement>()]
{
    Identifier vIdentifier;
}
    : tEndpoint:Identifier vIdentifier=identifier
        {
            Match(tEndpoint, CodeGenerationSupporter.Endpoint);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult] 
        (endpointOptions[vResult])? protocolInfo[vResult] payloadInfo[vResult]
    ;

endpointOptions [AlterCreateEndpointStatementBase vParent]
    : endpointOption[vParent] (Comma endpointOption[vParent])*
    ;
    
endpointOption [AlterCreateEndpointStatementBase vParent]
    : {NextTokenMatches(CodeGenerationSupporter.State)}?
      endpointState[vParent]
    | endpointAffinity[vParent]
    ;
            
endpointState [AlterCreateEndpointStatementBase vParent]
    : tState:Identifier EqualsSign tValue:Identifier
        {
            if (vParent.State != EndpointState.NotSpecified)
                throw GetUnexpectedTokenErrorException(tState);
            vParent.State = EndpointStateHelper.Instance.ParseOption(tValue);
            UpdateTokenInfo(vParent,tValue);
        }
    ;
    
endpointAffinity [AlterCreateEndpointStatementBase vParent]
{
    Literal vInteger;
    EndpointAffinity vAffinity = FragmentFactory.CreateFragment<EndpointAffinity>();
}
    : tAffinity:Identifier EqualsSign 
        {
            Match(tAffinity,CodeGenerationSupporter.Affinity);
            if (vParent.Affinity != null)
                throw GetUnexpectedTokenErrorException(tAffinity);
            UpdateTokenInfo(vAffinity, tAffinity);
        }
        (tNoneAdmin:Identifier
            {
                if (TryMatch(tNoneAdmin,CodeGenerationSupporter.None))
                    vAffinity.Kind = AffinityKind.None;
                else
                {
                    Match(tNoneAdmin,CodeGenerationSupporter.Admin);
                    vAffinity.Kind = AffinityKind.Admin;
                }
                UpdateTokenInfo(vAffinity, tNoneAdmin);
            }
        | vInteger = integer
            {
                vAffinity.Kind = AffinityKind.Integer;
                vAffinity.Value = vInteger;
            }
        )
        {
            vParent.Affinity = vAffinity;
        }
    ;

protocolInfo [AlterCreateEndpointStatementBase vParent]
{
    EndpointProtocolOption vOption;
    EndpointProtocolOptions encountered = EndpointProtocolOptions.None;
}
    : As tProtocol:Identifier 
        {
            vParent.Protocol = EndpointProtocolsHelper.Instance.ParseOption(tProtocol);
        }
        LeftParenthesis vOption = protocolOptionsItem[vParent.Protocol, encountered]
        {
            AddAndUpdateTokenInfo(vParent, vParent.ProtocolOptions,vOption);
            encountered = vOption.Kind;
        }
        (Comma vOption = protocolOptionsItem[vParent.Protocol, encountered]
            {
                AddAndUpdateTokenInfo(vParent, vParent.ProtocolOptions,vOption);
                encountered |= vOption.Kind;
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
protocolOptionsItem [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [EndpointProtocolOption vResult = null]
{
}
    : {NextTokenMatches(CodeGenerationSupporter.ListenerIP)}?
      vResult = listenerIpProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.ListenerPort)}?
      vResult = listenerPortProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Path)}?
      vResult = pathProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Site)}?
      vResult = siteProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.ClearPort)}?
      vResult = clearPortProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.SslPort)}?
      vResult = sslPortProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Authentication)}?
      vResult = authenticationProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Ports)}?
      vResult = portsProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.AuthRealm)}?
      vResult = authenticationRealmProtocolOption[protocol, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.DefaultLogonDomain)}?
      vResult = defaultLogonDomainProtocolOption[protocol, encountered]
    | vResult = compressionProtocolOption[protocol, encountered]
    ;
    
pathProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tPath:Identifier EqualsSign vValue = stringLiteral
        {
            vResult.Kind = EndpointProtocolOptions.HttpPath;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tPath);
            vResult.Value = vValue;
        }
    ;

siteProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tSite:Identifier EqualsSign vValue = stringLiteral
        {
            vResult.Kind = EndpointProtocolOptions.HttpSite;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tSite);
            vResult.Value = vValue;
        }
    ;

clearPortProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tClearPort:Identifier EqualsSign vValue = integer
        {
            vResult.Kind = EndpointProtocolOptions.HttpClearPort;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tClearPort);
            vResult.Value = vValue;
        }
    ;
    
sslPortProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tSslPort:Identifier EqualsSign vValue = integer
        {
            vResult.Kind = EndpointProtocolOptions.HttpSslPort;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tSslPort);
            vResult.Value = vValue;
        }
    ;

listenerPortProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tListenerPort:Identifier EqualsSign vValue = integer
        {
            vResult.Kind = EndpointProtocolOptions.TcpListenerPort;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tListenerPort);
            ThrowIfInvalidListenerPortValue(vValue);            
            vResult.Value = vValue;
        }
    ;

authenticationProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [AuthenticationEndpointProtocolOption vResult = FragmentFactory.CreateFragment<AuthenticationEndpointProtocolOption>()]
    : tAuthentication:Identifier EqualsSign 
        {
            vResult.Kind = EndpointProtocolOptions.HttpAuthentication;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tAuthentication);
        }
        LeftParenthesis tOption:Identifier 
        {
            vResult.AuthenticationTypes = AuthenticationTypesHelper.Instance.ParseOption(tOption);
        }
        (Comma tOption2:Identifier
            {
                vResult.AuthenticationTypes = AggregateAuthenticationType(vResult.AuthenticationTypes,
                    AuthenticationTypesHelper.Instance.ParseOption(tOption2),tOption2);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

portsProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [PortsEndpointProtocolOption vResult = FragmentFactory.CreateFragment<PortsEndpointProtocolOption>()]
    : tPorts:Identifier EqualsSign 
        {
            vResult.Kind = EndpointProtocolOptions.HttpPorts;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tPorts);
        }
        LeftParenthesis tOption:Identifier 
        {
            vResult.PortTypes = PortTypesHelper.Instance.ParseOption(tOption);
        }
        (Comma tOption2:Identifier
            {
                vResult.PortTypes = AggregatePortType(vResult.PortTypes,
                    PortTypesHelper.Instance.ParseOption(tOption2),tOption2);
            }
        )* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

authenticationRealmProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tAuthRealm:Identifier EqualsSign 
        {
            vResult.Kind = EndpointProtocolOptions.HttpAuthenticationRealm;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tAuthRealm);
        }
        (vValue = nonEmptyString 
            {
                vResult.Value = vValue;
            }
        | tNone:Identifier
            {
                Match(tNone, CodeGenerationSupporter.None);
            }
        )
    ;

defaultLogonDomainProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [LiteralEndpointProtocolOption vResult = FragmentFactory.CreateFragment<LiteralEndpointProtocolOption>()]
{
    Literal vValue;
}
    : tDefaultLogonDomain:Identifier EqualsSign 
        {
            vResult.Kind = EndpointProtocolOptions.HttpDefaultLogonDomain;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tDefaultLogonDomain);
        }
        (vValue = nonEmptyString
            {
                vResult.Value = vValue;
            }
        | tNone:Identifier
            {
                Match(tNone, CodeGenerationSupporter.None);
            }
        )
    ;

compressionProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [CompressionEndpointProtocolOption vResult = FragmentFactory.CreateFragment<CompressionEndpointProtocolOption>()]
    : tCompression:Identifier EqualsSign tEnabledDisabled:Identifier
        {
            vResult.Kind = EndpointProtocolOptions.HttpCompression;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tCompression);
            if (TryMatch(tEnabledDisabled, CodeGenerationSupporter.Enabled))
                vResult.IsEnabled = true;
            else
            {
                Match(tEnabledDisabled, CodeGenerationSupporter.Disabled);
                vResult.IsEnabled = false;
            }
            UpdateTokenInfo(vResult,tEnabledDisabled);
        }
    ;

listenerIpProtocolOption [EndpointProtocol protocol, EndpointProtocolOptions encountered] returns [ListenerIPEndpointProtocolOption vResult = FragmentFactory.CreateFragment<ListenerIPEndpointProtocolOption>()]
{
    IPv4 vIp;
    Literal vIpV6;
}
    : tListnerIp:Identifier EqualsSign 
        {
            vResult.Kind = EndpointProtocolOptions.TcpListenerIP;
            CheckIfEndpointOptionAllowed(encountered,vResult.Kind,protocol,tListnerIp);
        }
        ( 
            (LeftParenthesis 
                (vIp = ipAddressV4 
                    {
                        vResult.IPv4PartOne = vIp;
                    }
                    (Colon vIp = ipAddressV4
                        {
                            vResult.IPv4PartTwo = vIp;
                        }
                    )?
                | vIpV6 = stringLiteral
                    {
                        vResult.IPv6 = vIpV6;
                    }
                )
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
          )
        | All
            {
                vResult.IsAll = true;            
            }
        )
    ;
    
ipAddressV4 returns [IPv4 vResult = FragmentFactory.CreateFragment<IPv4>()]
{
    Literal vInt1, vInt2;
    Literal fragment1, fragment2;
}
  :    tNumeric1:Numeric
    (
        tNumeric2:Numeric
        (
            tNumeric3:Numeric
            (
                vInt1 = integer
                    {
                        // All numerics are 1.
                        vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                        vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric2);
                        vResult.OctetThree = GetIPv4FragmentFromNumberDotNumeric(tNumeric3);
                        vResult.OctetFour = vInt1;
                    }
                |
                /* nothing */
                    {
                        // Ok, one of the numerics is 1.1
                        if (SplitNumericIntoIpParts(tNumeric1, out fragment1, out fragment2))
                        {
                            vResult.OctetOne = fragment1;
                            vResult.OctetTwo = fragment2;
                            vResult.OctetThree = GetIPv4FragmentFromDotNumberNumeric(tNumeric2);
                            vResult.OctetFour = GetIPv4FragmentFromDotNumberNumeric(tNumeric3);
                        }
                        else if (SplitNumericIntoIpParts(tNumeric2, out fragment1, out fragment2))
                        {
                            vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                            vResult.OctetTwo = fragment1;
                            vResult.OctetThree = fragment2;
                            vResult.OctetFour = GetIPv4FragmentFromDotNumberNumeric(tNumeric3);
                        }
                        else
                        {
                            GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric3, out fragment1, out fragment2);
                            vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                            vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric2);
                            vResult.OctetThree = fragment1;
                            vResult.OctetFour = fragment2;
                        }
                    }
            )
            |
            Dot vInt1 = integer
                {
                    // One of two numerics is 1.1
                    if (SplitNumericIntoIpParts(tNumeric1, out fragment1, out fragment2))
                    {
                        vResult.OctetOne = fragment1;
                        vResult.OctetTwo = fragment2;
                        vResult.OctetThree = GetIPv4FragmentFromDotNumberNumeric(tNumeric2);
                    }
                    else
                    {
                        GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric2, out fragment1, out fragment2);
                        vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                        vResult.OctetTwo = fragment1;
                        vResult.OctetThree = fragment2;
                    }
                    
                    vResult.OctetFour = vInt1;
                }
            |
            vInt1 = integer ipV4DotNumberTail[vResult]
                {
                    vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                    vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric2);
                    vResult.OctetThree = vInt1;
                }
        )
        |
        ipV4DotLastTwoPartsTail[vResult]
            {
                GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric1, out fragment1, out fragment2);
                vResult.OctetOne = fragment1;
                vResult.OctetTwo = fragment2;
            }
        |
        vInt1 = integer
            {
                vResult.OctetOne = GetIPv4FragmentFromNumberDotNumeric(tNumeric1);
                vResult.OctetTwo = vInt1;
            }
        (
            tNumeric4:Numeric ipV4DotNumberTail[vResult]
                {
                    vResult.OctetThree = GetIPv4FragmentFromDotNumberNumeric(tNumeric4);
                }
            |
            ipV4DotLastTwoPartsTail[vResult]
        )
    )
    |
    vInt1 = integer
        {
            vResult.OctetOne = vInt1;
        }
    (
        tNumeric5:Numeric
            {
                vResult.OctetTwo = GetIPv4FragmentFromDotNumberNumeric(tNumeric5);
            }
        (
            tNumeric6:Numeric ipV4DotNumberTail[vResult]
                {
                    vResult.OctetThree = GetIPv4FragmentFromDotNumberNumeric(tNumeric6);
                }
            |
            ipV4DotLastTwoPartsTail[vResult]
        )
        |
        Dot
        (
            tNumeric7:Numeric
            (
                tNumeric8:Numeric
                (
                    vInt2 = integer
                        {
                            // Both numerics are 1.
                            vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric7);
                            vResult.OctetThree = GetIPv4FragmentFromNumberDotNumeric(tNumeric8);
                            vResult.OctetFour = vInt2;
                        }
                    | /* nothing */
                        {
                            // One of two numerics is 1.1
                            if (SplitNumericIntoIpParts(tNumeric7, out fragment1, out fragment2))
                            {
                                vResult.OctetTwo = fragment1;
                                vResult.OctetThree = fragment2;
                                vResult.OctetFour = GetIPv4FragmentFromDotNumberNumeric(tNumeric8);
                            }
                            else
                            {
                                GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric8, out fragment1, out fragment2);
                                vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric7);
                                vResult.OctetThree = fragment1;
                                vResult.OctetFour = fragment2;
                            }
                        }
                )
                |
                Dot vInt2 = integer
                    {
                        GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric7, out fragment1, out fragment2);
                        vResult.OctetTwo = fragment1;
                        vResult.OctetThree = fragment2;
                        vResult.OctetFour = vInt2;
                    }
                |
                vInt2 = integer ipV4DotNumberTail[vResult]
                    {
                        vResult.OctetTwo = GetIPv4FragmentFromNumberDotNumeric(tNumeric7);
                        vResult.OctetThree = vInt2;
                    }
            )
            |
            vInt2 = integer
                {
                    vResult.OctetTwo = vInt2;
                }
            (
                tNumeric9:Numeric ipV4DotNumberTail[vResult]
                    {
                        vResult.OctetThree = GetIPv4FragmentFromDotNumberNumeric(tNumeric9);
                    }
                |
                ipV4DotLastTwoPartsTail[vResult]
            )
        )
    )
  ;    

ipV4DotLastTwoPartsTail [IPv4 vParent]
{
    Literal vInteger;
}
    : 
        Dot
        (
            ipV4NumericOrNumericIntegerTail[vParent]
            |
            vInteger = integer ipV4DotNumberTail[vParent]
                {
                    vParent.OctetThree = vInteger;
                }
        )
    ;

ipV4DotNumberTail [IPv4 vParent]
{
    Literal vInteger;
}
    :    
        tNumeric:Numeric
        {
            // Numeric .1 
            vParent.OctetFour = GetIPv4FragmentFromDotNumberNumeric(tNumeric);
        }
    |
        Dot vInteger = integer    
        {
            vParent.OctetFour = vInteger;
        }
    ;
    
ipV4NumericOrNumericIntegerTail [IPv4 vParent]
{
    Literal vInteger;
}
    : tNumeric:Numeric
        (vInteger = integer
            {
                // Numeric 1. Integer
                vParent.OctetThree = GetIPv4FragmentFromNumberDotNumeric(tNumeric);
                vParent.OctetFour = vInteger;
            }
        |
            /* nothing */
            {
                // Numeric 1.1
                Literal fragment3, fragment4;
                GetIPv4FragmentsFromNumberDotNumberNumeric(tNumeric, out fragment3, out fragment4);
                vParent.OctetThree = fragment3;
                vParent.OctetFour = fragment4;
            }
        )
    ;

payloadInfo [AlterCreateEndpointStatementBase vParent]
{
    PayloadOption vOption;
    PayloadOptionKinds encountered = PayloadOptionKinds.None;
}
    : For tEndpointType:Identifier 
        {
            if (TryMatch(tEndpointType, CodeGenerationSupporter.DataMirroring))
                vParent.EndpointType = EndpointType.DatabaseMirroring;
            else
                vParent.EndpointType = EndpointTypesHelper.Instance.ParseOption(tEndpointType);
        }
        LeftParenthesis 
        (vOption = payloadOption[vParent.EndpointType, encountered]
            {
                AddAndUpdateTokenInfo(vParent, vParent.PayloadOptions,vOption);
                encountered = vOption.Kind;
            }
            (Comma vOption = payloadOption[vParent.EndpointType, encountered]
                {
                    AddAndUpdateTokenInfo(vParent, vParent.PayloadOptions,vOption);
                    encountered |= vOption.Kind;
                }
            )*
        )? 
        {
            if ((vParent.EndpointType == EndpointType.DatabaseMirroring) && 
                ((encountered & PayloadOptionKinds.Role) != PayloadOptionKinds.Role))
            {
                ThrowParseErrorException("SQL46080", tEndpointType, TSqlParserResource.SQL46080Message);
            }
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
payloadOption [EndpointType type, PayloadOptionKinds encountered] returns [PayloadOption vResult = null]
    : vResult = soapMethod[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Batches)}?
      vResult = batchesPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Wsdl)}?
      vResult = wsdlPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Sessions)}?
      vResult = sessionsPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.LoginType)}?
      vResult = loginTypePayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.SessionTimeout)}?
      vResult = sessionTimeoutPayloadOption[type, encountered]
    | vResult = databasePayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Namespace)}?
      vResult = namespacePayloadOption[type, encountered]
    | vResult = schemaPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.CharacterSet)}?
      vResult = characterSetPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.HeaderLimit)}?
      vResult = headerLimitPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Authentication)}?
      vResult = authenticationPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.Encryption)}?
      vResult = encryptionPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.MessageForwarding)}?
      vResult = messageForwardingPayloadOption[type, encountered]
    | {NextTokenMatches(CodeGenerationSupporter.MessageForwardSize)}?
      vResult = messageForwardSizePayloadOption[type, encountered]
    | vResult = rolePayloadOption[type, encountered]
    ;

batchesPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [EnabledDisabledPayloadOption vResult = FragmentFactory.CreateFragment<EnabledDisabledPayloadOption>()]
    : tBatches:Identifier EqualsSign enabledDisabled[vResult]
        {
            vResult.Kind = PayloadOptionKinds.Batches;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tBatches);
        }
    ;
    
enabledDisabled [EnabledDisabledPayloadOption vParent]
    : tEnabledDisabled:Identifier
        {
            if (TryMatch(tEnabledDisabled,CodeGenerationSupporter.Enabled))
                vParent.IsEnabled = true;
            else
            {
                Match(tEnabledDisabled,CodeGenerationSupporter.Disabled);
                vParent.IsEnabled = false;
            }
        }
    ;
    
wsdlPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [WsdlPayloadOption vResult = FragmentFactory.CreateFragment<WsdlPayloadOption>()]
{
    Literal vValue;
}
    : tWsdl:Identifier EqualsSign 
        {
            vResult.Kind = PayloadOptionKinds.Wsdl;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tWsdl);
        }
        ( tNone:Identifier
            {
                Match(tNone,CodeGenerationSupporter.None);
                vResult.IsNone = true;
            }
        | vValue = defaultLiteral
            {
                vResult.Value = vValue;
            }
        | vValue = stringLiteral
            {
                vResult.Value = vValue;
            }
        )
    ;
    
sessionsPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [EnabledDisabledPayloadOption vResult = FragmentFactory.CreateFragment<EnabledDisabledPayloadOption>()]
    : tSessions:Identifier EqualsSign enabledDisabled[vResult]
        {
            vResult.Kind = PayloadOptionKinds.Sessions;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tSessions);
        }
    ;
    
loginTypePayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [LoginTypePayloadOption vResult = FragmentFactory.CreateFragment<LoginTypePayloadOption>()]
    : tLoginType:Identifier EqualsSign tMixedWindows:Identifier
        {
            vResult.Kind = PayloadOptionKinds.LoginType;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tLoginType);
            if (TryMatch(tMixedWindows,CodeGenerationSupporter.Windows))
                vResult.IsWindows = true;
            else
            {
                Match(tMixedWindows,CodeGenerationSupporter.Mixed);
                vResult.IsWindows = false;
            }
        }
    ;
    
sessionTimeoutPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [SessionTimeoutPayloadOption vResult = FragmentFactory.CreateFragment<SessionTimeoutPayloadOption>()]
{
    Literal vValue;
}
    : tSessionTimeout:Identifier EqualsSign 
        {
            vResult.Kind = PayloadOptionKinds.SessionTimeout;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tSessionTimeout);
        }
        (tNever:Identifier
            {
                Match(tNever,CodeGenerationSupporter.Never);
                vResult.IsNever = true;
            }
        | vValue = integer
            {
                vResult.Timeout = vValue;
            }
        )
    ;
    
databasePayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [LiteralPayloadOption vResult = FragmentFactory.CreateFragment<LiteralPayloadOption>()]
{
    Literal vValue;
}
    : tDatabase:Database EqualsSign
        {
            vResult.Kind = PayloadOptionKinds.Database;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tDatabase);
        }
        (vValue = stringLiteral | vValue = defaultLiteral)
        {
            vResult.Value = vValue;
        }
    ;
    
namespacePayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [LiteralPayloadOption vResult = FragmentFactory.CreateFragment<LiteralPayloadOption>()]
{
    Literal vValue;
}
    : tNamespace:Identifier EqualsSign 
        {
            vResult.Kind = PayloadOptionKinds.Namespace;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tNamespace);
        }
        (vValue = stringLiteral | vValue = defaultLiteral)
        {
            vResult.Value = vValue;
        }
    ;
    
schemaPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [SchemaPayloadOption vResult = FragmentFactory.CreateFragment<SchemaPayloadOption>()]
    : tSchema:Schema EqualsSign tNoneStandard:Identifier
        {
            vResult.Kind = PayloadOptionKinds.Schema;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tSchema);
            if (TryMatch(tNoneStandard,CodeGenerationSupporter.Standard))
                vResult.IsStandard = true;
            else
            {
                Match(tNoneStandard,CodeGenerationSupporter.None);
                vResult.IsStandard = false;
            }
        }
    ;
    
characterSetPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [CharacterSetPayloadOption vResult = FragmentFactory.CreateFragment<CharacterSetPayloadOption>()]
    : tCharacterSet:Identifier EqualsSign tSqlXml:Identifier
        {
            vResult.Kind = PayloadOptionKinds.CharacterSet;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tCharacterSet);
            if (TryMatch(tSqlXml,CodeGenerationSupporter.Sql))
                vResult.IsSql = true;
            else
            {
                Match(tSqlXml,CodeGenerationSupporter.Xml);
                vResult.IsSql = false;
            }
        }
    ;
    
headerLimitPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [LiteralPayloadOption vResult = FragmentFactory.CreateFragment<LiteralPayloadOption>()]
{
    Literal vValue;
}
    : tHeaderLimit:Identifier EqualsSign vValue = integer
        {
            vResult.Kind = PayloadOptionKinds.HeaderLimit;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tHeaderLimit);
            vResult.Value = vValue;
        }
    ;
    
authenticationPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [AuthenticationPayloadOption vResult = FragmentFactory.CreateFragment<AuthenticationPayloadOption>()]
{
    Identifier vIdentifier2, vIdentifier3, vIdentifier4;
}
    : tAuthentication:Identifier EqualsSign 
        {
            vResult.Kind = PayloadOptionKinds.Authentication;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tAuthentication);
        }
        tWindowsCertificate:Identifier 
        ( 
            /* empty */
            {
                Match(tWindowsCertificate,CodeGenerationSupporter.Windows);
                vResult.Protocol = AuthenticationProtocol.Windows;
            }
        | 
            vIdentifier2 = identifier
            {
                if (TryMatch(tWindowsCertificate,CodeGenerationSupporter.Certificate))
                {
                    vResult.Certificate = vIdentifier2;
                    vResult.TryCertificateFirst = true;
                }
                else
                {
                    Match(tWindowsCertificate, CodeGenerationSupporter.Windows);
                    vResult.Protocol = RecognizeAuthenticationProtocol(vIdentifier2,tWindowsCertificate);
                }
            }
        | 
            (vIdentifier2 = identifier vIdentifier3 = identifier 
                ( 
                    /* empty */
                    {
                        if (TryMatch(tWindowsCertificate,CodeGenerationSupporter.Certificate))
                        {
                            vResult.Certificate = vIdentifier2;
                            Match(vIdentifier3,CodeGenerationSupporter.Windows,tWindowsCertificate);
                            vResult.TryCertificateFirst = true;
                        }
                        else
                        {
                            Match(tWindowsCertificate, CodeGenerationSupporter.Windows);
                            Match(vIdentifier2, CodeGenerationSupporter.Certificate,tWindowsCertificate);
                            vResult.Certificate = vIdentifier3;
                        }
                        vResult.Protocol = AuthenticationProtocol.Windows;
                    }
                | 
                    vIdentifier4 = identifier
                    {
                        if (TryMatch(tWindowsCertificate,CodeGenerationSupporter.Certificate))
                        {
                            vResult.Certificate = vIdentifier2;
                            Match(vIdentifier3,CodeGenerationSupporter.Windows,tWindowsCertificate);
                            vResult.Protocol = RecognizeAuthenticationProtocol(vIdentifier4,tWindowsCertificate);
                            vResult.TryCertificateFirst = true;
                        }
                        else
                        {
                            Match(tWindowsCertificate, CodeGenerationSupporter.Windows);
                            vResult.Protocol = RecognizeAuthenticationProtocol(vIdentifier2,tWindowsCertificate);
                            Match(vIdentifier3, CodeGenerationSupporter.Certificate,tWindowsCertificate);
                            vResult.Certificate = vIdentifier4;
                        }
                    }
                )
            )
        )
    ;
    
encryptionPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [EncryptionPayloadOption vResult = FragmentFactory.CreateFragment<EncryptionPayloadOption>()]
{
    Identifier vAlgorithm;
    Identifier vAlg1, vAlg2;
}
    : tEncryption:Identifier EqualsSign 
        {
            vResult.Kind = PayloadOptionKinds.Encryption;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tEncryption);
        }
        tOption:Identifier 
        {
            vResult.EncryptionSupport = EndpointEncryptionSupportHelper.Instance.ParseOption(tOption);
        }
        (vAlgorithm = identifier
            {
                if (vResult.EncryptionSupport == EndpointEncryptionSupport.Disabled ||
                    !String.Equals(Unquote(vAlgorithm.Value), CodeGenerationSupporter.Algorithm, StringComparison.OrdinalIgnoreCase))
                    throw GetUnexpectedTokenErrorException(tOption);                    
            }
            ( vAlg1 = identifier
                {
                    vResult.AlgorithmPartOne = RecognizeAesOrRc4(vAlg1,tOption);
                }
            | (vAlg1 = identifier vAlg2 = identifier
                {
                    vResult.AlgorithmPartOne = RecognizeAesOrRc4(vAlg1,tOption);
                    vResult.AlgorithmPartTwo = RecognizeAesOrRc4(vAlg2,tOption);
                    if (vResult.AlgorithmPartOne == vResult.AlgorithmPartTwo)
                        throw GetUnexpectedTokenErrorException(tOption);                    
                }
              )
            )
        )?
    ;
        
messageForwardingPayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [EnabledDisabledPayloadOption vResult = FragmentFactory.CreateFragment<EnabledDisabledPayloadOption>()]
    : tMessageForwarding:Identifier EqualsSign enabledDisabled[vResult]
        {
            vResult.Kind = PayloadOptionKinds.MessageForwarding;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tMessageForwarding);
        }
    ;
    
messageForwardSizePayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [LiteralPayloadOption vResult = FragmentFactory.CreateFragment<LiteralPayloadOption>()]
{
    Literal vValue;
}
    : tMessageForwardSize:Identifier EqualsSign vValue = integer
        {
            vResult.Kind = PayloadOptionKinds.MessageForwardSize;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tMessageForwardSize);
            vResult.Value = vValue;
        }
    ;
    
rolePayloadOption [EndpointType type, PayloadOptionKinds encountered] returns [RolePayloadOption vResult = FragmentFactory.CreateFragment<RolePayloadOption>()]
    : tRole:Identifier EqualsSign
        {
            Match(tRole,CodeGenerationSupporter.Role);
            vResult.Kind = PayloadOptionKinds.Role;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tRole);
        }
        (tWitnessPartner:Identifier
            {
                if (TryMatch(tWitnessPartner,CodeGenerationSupporter.Witness))
                    vResult.Role = DatabaseMirroringEndpointRole.Witness;                
                else
                {
                    Match(tWitnessPartner,CodeGenerationSupporter.Partner);
                    vResult.Role = DatabaseMirroringEndpointRole.Partner;                
                }
            }
        | All
            {
                vResult.Role = DatabaseMirroringEndpointRole.All;                
            }
        )
    ;
        
soapMethod [EndpointType type, PayloadOptionKinds encountered] returns [SoapMethod vResult = FragmentFactory.CreateFragment<SoapMethod>()]
    : (
        ( Add
            {
                vResult.Action = SoapMethodAction.Add;
            }
        | Alter 
            {
                vResult.Action = SoapMethodAction.Alter;
            }
        )?
        tWebMethod:Identifier 
        {
            Match(tWebMethod, CodeGenerationSupporter.WebMethod);
            vResult.Kind = PayloadOptionKinds.WebMethod;
            CheckIfPayloadOptionAllowed(encountered,vResult.Kind,type,tWebMethod);
        }
        soapMethodAlias[vResult] 
        LeftParenthesis 
        soapMethodOption[vResult] (Comma soapMethodOption[vResult])*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
      )
    | Drop tWebMethod2:Identifier soapMethodAlias[vResult]
        {
            Match(tWebMethod2, CodeGenerationSupporter.WebMethod);
            vResult.Action = SoapMethodAction.Drop;
        }
    ;
    
soapMethodAlias [SoapMethod vParent]
{
    Literal vAlias, vNamespace;
}
    : vAlias = stringLiteral
        {
            vParent.Alias = vAlias;
        }
    | vNamespace = stringLiteral Dot vAlias = stringLiteral
        {
            vParent.Alias = vAlias;
            vParent.Namespace = vNamespace;
        }
    ;
    
soapMethodOption [SoapMethod vParent]
{
    Literal vName;
}
    : tSchema:Schema EqualsSign 
        {
            if (vParent.Schema != SoapMethodSchemas.NotSpecified)
                throw GetUnexpectedTokenErrorException(tSchema);
        }
        (tNoneStandard:Identifier 
            {
                if (TryMatch(tNoneStandard,CodeGenerationSupporter.None))
                    vParent.Schema = SoapMethodSchemas.None;
                else
                {
                    Match(tNoneStandard,CodeGenerationSupporter.Standard);
                    vParent.Schema = SoapMethodSchemas.Standard;
                }
            }
        | Default
            {
                vParent.Schema = SoapMethodSchemas.Default;
            }
        )
    | tFormatName:Identifier EqualsSign 
        (tFormat:Identifier
            {
                if (vParent.Format != SoapMethodFormat.NotSpecified)
                    throw GetUnexpectedTokenErrorException(tFormatName);
                Match(tFormatName,CodeGenerationSupporter.Format);
                vParent.Format = SoapMethodFormatsHelper.Instance.ParseOption(tFormat);
            }
        |vName = stringLiteral
            {
                Match(tFormatName,CodeGenerationSupporter.Name);
                if (vParent.Name != null)
                    throw GetUnexpectedTokenErrorException(tFormatName);
                vParent.Name = vName;
            }
        )
    ;
    
createMasterKeyStatement returns [CreateMasterKeyStatement vResult = this.FragmentFactory.CreateFragment<CreateMasterKeyStatement>()]
{
    Literal vLiteral;
}
    :
        tMaster:Identifier 
        {
            Match(tMaster, CodeGenerationSupporter.Master);
        }
        Key tEncryption:Identifier
        {
            Match(tEncryption, CodeGenerationSupporter.Encryption);
        }
        By tPassword:Identifier
        {
            Match(tPassword, CodeGenerationSupporter.Password);
        }
        EqualsSign vLiteral=stringLiteral
        {
            vResult.Password = vLiteral;
        }
    ;


createEventStatement returns [TSqlStatement vResult = null]
    : tEvent:Identifier
        {
            Match(tEvent, CodeGenerationSupporter.Event);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Notification)}?
            vResult = createEventNotificationStatement        
        |
            {NextTokenMatches(CodeGenerationSupporter.Session)}?
            vResult = createEventSessionStatement
        )
    ;

createEventNotificationStatement returns [CreateEventNotificationStatement vResult = this.FragmentFactory.CreateFragment<CreateEventNotificationStatement>()]
{
    Identifier vIdentifier;
    Literal vLiteral;
    EventNotificationObjectScope vScope;
    EventTypeGroupContainer vEventTypeGroup;
}
    : tNotification:Identifier vIdentifier=identifier
        {
            Match(tNotification, CodeGenerationSupporter.Notification);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vScope = eventNotificationObjectScope
        {
            vResult.Scope = vScope;
        }
        (
            With tFanIn:Identifier
            {
                Match(tFanIn, CodeGenerationSupporter.FanIn);
                vResult.WithFanIn = true;
            }
        )?
        For vEventTypeGroup = eventTypeGroupContainer
        {
            AddAndUpdateTokenInfo(vResult, vResult.EventTypeGroups,vEventTypeGroup);
        }
        (
            Comma vEventTypeGroup = eventTypeGroupContainer
            {
                AddAndUpdateTokenInfo(vResult, vResult.EventTypeGroups,vEventTypeGroup);                      
            }            
        )*
        To tService:Identifier
        {
            Match(tService, CodeGenerationSupporter.Service);
        }
        vLiteral=stringLiteral
        {
            vResult.BrokerService = vLiteral;
        }
        Comma vLiteral=stringLiteral
        {
            vResult.BrokerInstanceSpecifier = vLiteral;
        }       
    ;

eventTypeGroupContainer returns [EventTypeGroupContainer vResult = null]
    : 
        tIdentifier:Identifier
        {
            EventNotificationEventType eventTypeValue;
            EventNotificationEventGroup eventGroupValue;
            if (TriggerEventTypeHelper.Instance.TryParseOption(tIdentifier, SqlVersionFlags.TSql120, out eventTypeValue))
            {
                vResult = CreateEventTypeContainer(eventTypeValue, tIdentifier);
            }
            else if (AuditEventTypeHelper.Instance.TryParseOption(tIdentifier, SqlVersionFlags.TSql120, out eventTypeValue))
            {
                vResult = CreateEventTypeContainer(eventTypeValue, tIdentifier);
            }
            else if (TriggerEventGroupHelper.Instance.TryParseOption(tIdentifier, SqlVersionFlags.TSql120, out eventGroupValue))
            {
                vResult = CreateEventGroupContainer(eventGroupValue, tIdentifier);
            }
            else
            {
                eventGroupValue = AuditEventGroupHelper.Instance.ParseOption(tIdentifier, SqlVersionFlags.TSql120);
                vResult = CreateEventGroupContainer(eventGroupValue, tIdentifier);
            }
        }
    ;
    
eventNotificationObjectScope returns [EventNotificationObjectScope vResult = FragmentFactory.CreateFragment<EventNotificationObjectScope>()]
{
    SchemaObjectName vQueueName;
}
    : On 
        (
            tDatabase:Database
            {
                vResult.Target = EventNotificationTarget.Database;
                UpdateTokenInfo(vResult,tDatabase);
            }
        |
            {NextTokenMatches(CodeGenerationSupporter.Queue)}?
            tQueue:Identifier vQueueName=schemaObjectThreePartName
            {
                vResult.Target = EventNotificationTarget.Queue;
                vResult.QueueName = vQueueName;
            }
        |
            tServer:Identifier
            {
                Match(tServer, CodeGenerationSupporter.Server);
                vResult.Target = EventNotificationTarget.Server;
                UpdateTokenInfo(vResult,tServer);
            }
        )
    ;

createEventSessionStatement returns [CreateEventSessionStatement vResult = this.FragmentFactory.CreateFragment<CreateEventSessionStatement>()]
{
    Identifier vIdentifier;
}
    : tSession:Identifier vIdentifier=identifier
        {
            Match(tSession, CodeGenerationSupporter.Session);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        On tServer:Identifier 
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        eventDeclarationList[vResult]    
        (
            options {greedy = true; }: // conflicts with Add statements
            targetDeclarationList[vResult])?
        optSessionOptionList[vResult]
    ;

alterEventSessionStatement returns [AlterEventSessionStatement vResult = this.FragmentFactory.CreateFragment<AlterEventSessionStatement>()]
{
    Identifier vIdentifier;      
}
    : tEvent:Identifier tSession:Identifier vIdentifier=identifier
        {
            Match(tEvent, CodeGenerationSupporter.Event);
            Match(tSession, CodeGenerationSupporter.Session);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        On tServer:Identifier 
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        (
            (
                (
                    {NextTokenMatches(CodeGenerationSupporter.Add) && NextTokenMatches((CodeGenerationSupporter.Event),2)}? 
                    eventDeclarationList[vResult]
                    {
                        vResult.StatementType = AlterEventSessionStatementType.AddEventDeclarationOptionalSessionOptions;
                    }
                |
                    {NextTokenMatches(CodeGenerationSupporter.Add) && NextTokenMatches((CodeGenerationSupporter.Target),2)}? 
                    targetDeclarationList[vResult]
                    {
                        vResult.StatementType = AlterEventSessionStatementType.AddTargetDeclarationOptionalSessionOptions;
                    }
                )
                optSessionOptionList[vResult]
            )
        |
            (
                (
                    {NextTokenMatches(CodeGenerationSupporter.Drop) && NextTokenMatches((CodeGenerationSupporter.Event),2)}? 
                    dropEventDeclarationList[vResult]
                    {
                            vResult.StatementType = AlterEventSessionStatementType.DropEventSpecificationOptionalSessionOptions;
                    }
                |
                    {NextTokenMatches(CodeGenerationSupporter.Drop) && NextTokenMatches((CodeGenerationSupporter.Target),2)}? 
                    dropTargetDeclarationList[vResult]
                    {
                        vResult.StatementType = AlterEventSessionStatementType.DropTargetSpecificationOptionalSessionOptions;                    
                    }
                )            
                optSessionOptionList[vResult]
            )
        |
            (
                sessionOptionList[vResult]
                {
                    vResult.StatementType = AlterEventSessionStatementType.RequiredSessionOptions;                                    
                }
                
            )
        |
            (
                (
                    tState:Identifier EqualsSign tStartStop:Identifier
                    {
                        Match(tState,CodeGenerationSupporter.State);
                        if (TryMatch(tStartStop, CodeGenerationSupporter.Start))
                            vResult.StatementType = AlterEventSessionStatementType.AlterStateIsStart;
                        else if (TryMatch(tStartStop, CodeGenerationSupporter.Stop))
                            vResult.StatementType = AlterEventSessionStatementType.AlterStateIsStop;
                        else
                            ThrowIncorrectSyntaxErrorException(tStartStop);
                    }
                )
            )
        )    
    ;

eventDeclarationList [EventSessionStatement vParent]
{
    EventDeclaration eventDeclarationValue;
}
    :
        Add tEvent:Identifier eventDeclarationValue = eventDeclaration
        {
            Match(tEvent,CodeGenerationSupporter.Event);
            AddAndUpdateTokenInfo(vParent,vParent.EventDeclarations,eventDeclarationValue);
        }
        (
            Comma Add tEvent2:Identifier eventDeclarationValue = eventDeclaration
            {
                Match(tEvent2,CodeGenerationSupporter.Event);            
                AddAndUpdateTokenInfo(vParent,vParent.EventDeclarations,eventDeclarationValue);
            }
        )*
    ;    

targetDeclarationList [EventSessionStatement vParent]
{
    TargetDeclaration targetDeclarationValue;
}
    :
        Add tTarget:Identifier targetDeclarationValue = targetDeclaration
        {
            Match(tTarget,CodeGenerationSupporter.Target);
            AddAndUpdateTokenInfo(vParent,vParent.TargetDeclarations,targetDeclarationValue);                
        }
        (
            Comma Add tTarget2:Identifier targetDeclarationValue = targetDeclaration
            {
                Match(tTarget2,CodeGenerationSupporter.Target);
                AddAndUpdateTokenInfo(vParent,vParent.TargetDeclarations,targetDeclarationValue);
            }
        )*
    ;

sessionOptionList [EventSessionStatement vParent]
{
    SessionOption sessionOptionValue;
}
    :
        (
            With 
            (
                options {greedy = true; }: // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                LeftParenthesis sessionOptionValue = sessionOption
                {
                    AddAndUpdateTokenInfo(vParent,vParent.SessionOptions,sessionOptionValue);
                }
                (
                    Comma sessionOptionValue = sessionOption
                    {
                        AddAndUpdateTokenInfo(vParent,vParent.SessionOptions,sessionOptionValue);
                    }
                )*
                tRParen:RightParenthesis
                {
                    UpdateTokenInfo(vParent,tRParen);
                }
            )
        )
    ;

optSessionOptionList [EventSessionStatement vParent]
    :    
        (
            options {greedy = true; }: // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            sessionOptionList[vParent]            
        )?
    ;

eventDeclaration returns [EventDeclaration vResult = this.FragmentFactory.CreateFragment<EventDeclaration>()]
{
    EventSessionObjectName vName;
    BooleanExpression eventDeclarationPredicate;
}
    :   vName=eventSessionNonEmptyThreePartObjectName
        {
            vResult.ObjectName = vName;            
        }
        (
            options { greedy = true; } :  // Greedy due to conflict with Select statements
            tLParen:LeftParenthesis    
            (eventDeclarationSetParameters[vResult])?
            (eventDeclarationActionParameters[vResult])?
            (
                Where eventDeclarationPredicate=eventBooleanExpression
                {
                    vResult.EventDeclarationPredicateParameter = eventDeclarationPredicate;
                }            
            )?
            tRParen:RightParenthesis     
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?        
    ;

eventDeclarationSetParameters [EventDeclaration vParent]
{
    EventDeclarationSetParameter eventDeclarationSet;
}
    :
        (
            Set eventDeclarationSet=eventDeclarationSetParameter
            {
                AddAndUpdateTokenInfo(vParent,vParent.EventDeclarationSetParameters,eventDeclarationSet);
            }
            (
                Comma eventDeclarationSet=eventDeclarationSetParameter
                {
                    AddAndUpdateTokenInfo(vParent,vParent.EventDeclarationSetParameters,eventDeclarationSet);
                }            
            )*
        )
    ;
    
eventDeclarationActionParameters [EventDeclaration vParent]
{
    EventSessionObjectName eventDeclarationAction;
}
    :
        (
                tAction:Identifier 
                {
                    Match(tAction, CodeGenerationSupporter.Action);
                }
                LeftParenthesis eventDeclarationAction=eventSessionNonEmptyThreePartObjectName
                {
                    AddAndUpdateTokenInfo(vParent,vParent.EventDeclarationActionParameters,eventDeclarationAction);
                }
                (
                    Comma eventDeclarationAction=eventSessionNonEmptyThreePartObjectName
                    {
                        AddAndUpdateTokenInfo(vParent,vParent.EventDeclarationActionParameters,eventDeclarationAction);
                    }
                )*
                tRParen:RightParenthesis     
                {
                    UpdateTokenInfo(vParent,tRParen);
                }
        )
    ;    
    
targetDeclaration returns [TargetDeclaration vResult = this.FragmentFactory.CreateFragment<TargetDeclaration>()]
{
    EventSessionObjectName vName;
    EventDeclarationSetParameter eventDeclarationSet;
}
    :   vName=eventSessionNonEmptyThreePartObjectName
        {
            vResult.ObjectName = vName;            
        }    
        (
            options { greedy = true; } :  // Greedy due to conflict with Select, which can start from (
            LeftParenthesis 
            (
                Set eventDeclarationSet=eventDeclarationSetParameter
                {
                    AddAndUpdateTokenInfo(vResult,vResult.TargetDeclarationParameters,eventDeclarationSet);
                }    
                (
                    Comma eventDeclarationSet=eventDeclarationSetParameter
                    {
                        AddAndUpdateTokenInfo(vResult,vResult.TargetDeclarationParameters,eventDeclarationSet);
                    }            
                )*
            )
            tRParen:RightParenthesis     
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;

sessionOption returns [SessionOption vResult = null]
{
}
    :   {NextTokenMatches(CodeGenerationSupporter.MaxMemory)}?
        vResult=maxMemorySessionOption            
    |    {NextTokenMatches(CodeGenerationSupporter.MaxEventSize)}? 
        vResult=maxEventSizeSessionOption            
    |   {NextTokenMatches(CodeGenerationSupporter.TrackCausality)}?
        vResult=trackCausalitySessionOption
    |    {NextTokenMatches(CodeGenerationSupporter.StartupState)}?
        vResult=startupStateSessionOption
    |    {NextTokenMatches(CodeGenerationSupporter.EventRetentionMode)}?
        vResult=eventRetentionSessionOption
    |    {NextTokenMatches(CodeGenerationSupporter.MemoryPartitionMode)}?
        vResult=memoryPartitionSessionOption
    |   {NextTokenMatches(CodeGenerationSupporter.MaxDispatchLatency)}?
        vResult=maxDispatchLatencySessionOption
    ;

maxMemorySessionOption returns [LiteralSessionOption vResult= this.FragmentFactory.CreateFragment<LiteralSessionOption>()]
    :    tName:Identifier integerSessionOptionValue[vResult]
        {
            vResult.OptionKind=SessionOptionKind.MaxMemory;
        }    
    ;
    
integerSessionOptionValue [LiteralSessionOption vParent]
{
    Literal vValue;
}
    :
        EqualsSign vValue = integer tMemoryUnit:Identifier
        {
            vParent.Value = vValue;
            vParent.Unit = SessionOptionUnitHelper.Instance.ParseOption(tMemoryUnit);
        }
    ;

maxEventSizeSessionOption returns [LiteralSessionOption vResult= this.FragmentFactory.CreateFragment<LiteralSessionOption>()]
    :    tName:Identifier integerSessionOptionValue[vResult]    
        {
            vResult.OptionKind=SessionOptionKind.MaxEventSize;
        }    
    ;

maxDispatchLatencySessionOption returns [MaxDispatchLatencySessionOption vResult = this.FragmentFactory.CreateFragment<MaxDispatchLatencySessionOption>()]
{
    Literal vValue;
}    
    :
        tName:Identifier EqualsSign
        {
            vResult.OptionKind=SessionOptionKind.MaxDispatchLatency;    
        }
        (
                        
            tValue:Identifier
            {
                Match(tValue,CodeGenerationSupporter.Infinite);
                vResult.IsInfinite = true;
            }        
        |    
            vValue=integer tSeconds:Identifier
            {
                Match(tSeconds,CodeGenerationSupporter.Seconds);
                vResult.IsInfinite = false;
                vResult.Value = vValue;
            }            
        )    
    ;
    
trackCausalitySessionOption returns [OnOffSessionOption vResult = this.FragmentFactory.CreateFragment<OnOffSessionOption>()]
    :
        tName:Identifier onOffSessionOption[vResult]
        {
            vResult.OptionKind=SessionOptionKind.TrackCausality;    
        }
    ;

startupStateSessionOption returns [OnOffSessionOption vResult = this.FragmentFactory.CreateFragment<OnOffSessionOption>()]
    :
        tName:Identifier onOffSessionOption[vResult]
        {
            vResult.OptionKind=SessionOptionKind.StartUpState;    
        }
    ;    

onOffSessionOption [OnOffSessionOption vParent]
{
    OptionState onOffValue;
}
    :    EqualsSign onOffValue = optionOnOff[vParent]
        {
            vParent.OptionState = onOffValue;
        }
    ;

eventRetentionSessionOption returns [EventRetentionSessionOption vResult = this.FragmentFactory.CreateFragment<EventRetentionSessionOption>()]
    :
        tName:Identifier EqualsSign tValue:Identifier
        {
            vResult.OptionKind=SessionOptionKind.EventRetention;    
            vResult.Value=EventSessionEventRetentionModeTypeHelper.Instance.ParseOption(tValue);
        }
    ;        
    
memoryPartitionSessionOption returns [MemoryPartitionSessionOption vResult = this.FragmentFactory.CreateFragment<MemoryPartitionSessionOption>()]
    :
        tName:Identifier EqualsSign tValue:Identifier
        {
            vResult.OptionKind=SessionOptionKind.MemoryPartition;    
            vResult.Value=EventSessionMemoryPartitionModeTypeHelper.Instance.ParseOption(tValue);
        }
    ;        
            
eventDeclarationSetParameter returns [EventDeclarationSetParameter vResult = this.FragmentFactory.CreateFragment<EventDeclarationSetParameter>()]
{
    Identifier vIdentifier;
    ScalarExpression eventValue;
}    
    :
        vIdentifier = identifier EqualsSign eventValue = eventDeclarationValue
        {
            vResult.EventField = vIdentifier;
            vResult.EventValue = eventValue;
        }    
    ;

eventDeclarationValue returns [ScalarExpression vResult = null]
{
    ScalarExpression vExpression = null;
    UnaryExpression vUnaryExpression = null;
}
    :  
        tMinus:Minus vExpression=integerOrRealOrNumeric
        {
            vUnaryExpression = this.FragmentFactory.CreateFragment<UnaryExpression>();
            UpdateTokenInfo(vUnaryExpression, tMinus);
            vUnaryExpression.UnaryExpressionType = UnaryExpressionType.Negative;
            vUnaryExpression.Expression = vExpression;
            vResult = vUnaryExpression;
        }
    |
        vResult = eventSessionExpressionParenthesis
    |
        vResult = eventSessionLiteral
    ;

eventSessionLiteral returns [Literal vResult]
    : vResult = integer
    | vResult = numeric
    | vResult = real
    | vResult = stringLiteral
    ;  

eventSessionExpressionParenthesis returns [ParenthesisExpression vResult = this.FragmentFactory.CreateFragment<ParenthesisExpression>()]
{
    ScalarExpression vExpression;
}
    :
        tLParen:LeftParenthesis vExpression=eventDeclarationValue tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Expression = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;    

eventBooleanExpression returns [BooleanExpression vResult = null]
    :
        vResult = eventBooleanExpressionOr
    ;

eventBooleanExpressionOr returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;    
}
    :
        vResult = eventBooleanExpressionAnd
        (
            Or vExpression = eventBooleanExpressionAnd
            {
                AddBinaryExpression(ref vResult, vExpression, BooleanBinaryExpressionType.Or);
            }
        )*
    ;

eventBooleanExpressionAnd returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;    
}
    :
        vResult = eventBooleanExpressionUnary
        (
            And vExpression = eventBooleanExpressionUnary
            {
                AddBinaryExpression(ref vResult, vExpression, BooleanBinaryExpressionType.And);
            }
        )*
    ;    

eventBooleanExpressionUnary returns [BooleanExpression vResult = null]
{
    BooleanExpression vExpression;
}
    :
        tNot:Not vExpression = eventBooleanExpressionUnary
        {
            BooleanNotExpression vUnaryExpression = this.FragmentFactory.CreateFragment<BooleanNotExpression>();
            vResult = vUnaryExpression;
            UpdateTokenInfo(vUnaryExpression, tNot);
            vUnaryExpression.Expression = vExpression;
        }
    |
        vResult = eventBooleanExpressionParenthesis
    |
        vResult = eventDeclarationPredicateParameter
    ;    

eventBooleanExpressionParenthesis returns [BooleanParenthesisExpression vResult = this.FragmentFactory.CreateFragment<BooleanParenthesisExpression>()]
{
    BooleanExpression vExpression;
}
    :
        tLParen:LeftParenthesis vExpression=eventBooleanExpression tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Expression = vExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

eventDeclarationPredicateParameter returns [BooleanExpression vResult = null]
{
    EventSessionObjectName vName;
    SourceDeclaration vSourceDeclaration = this.FragmentFactory.CreateFragment<SourceDeclaration>();
    EventDeclarationCompareFunctionParameter eventDeclarationCompareFunctionValue = this.FragmentFactory.CreateFragment<EventDeclarationCompareFunctionParameter>();
    BooleanComparisonExpression eventDeclarationComparisonPredicateValue = this.FragmentFactory.CreateFragment<BooleanComparisonExpression>();
}    
    :
        vName = eventSessionOneOrTwoOrThreePartObjectName
        (
            (
                tLparen:LeftParenthesis
                (
                    //check if vName is a nonemptythreepartname
                    {
                        if (vName.MultiPartIdentifier.Count < 2)
                            ThrowIncorrectSyntaxErrorException(tLparen);                                        
                        vSourceDeclaration.Value = vName;
                        eventDeclarationCompareFunctionValue.Name = vName;
                    }
                    eventDeclarationCompareFunction[eventDeclarationCompareFunctionValue]
                    {
                        vResult = eventDeclarationCompareFunctionValue;
                    }
                )
                tRParen:RightParenthesis     
                {
                    UpdateTokenInfo(vResult,tRParen);
                }                
            )
        |
            (
                eventDeclarationComparisonPredicate[eventDeclarationComparisonPredicateValue,vName]
                {
                    vResult = eventDeclarationComparisonPredicateValue;
                }
            )
        )
    ;

eventDeclarationCompareFunction [EventDeclarationCompareFunctionParameter vParent]
{
    EventSessionObjectName vName;
    SourceDeclaration vSourceDeclaration = this.FragmentFactory.CreateFragment<SourceDeclaration>();
    ScalarExpression eventValue;
}
    :    vName = eventSessionOneOrTwoOrThreePartObjectName Comma eventValue = eventDeclarationValue
        {
            vSourceDeclaration.Value = vName;        
            vParent.SourceDeclaration = vSourceDeclaration;
            vParent.EventValue = eventValue;
        }        
    ;

eventDeclarationComparisonPredicate [BooleanComparisonExpression vParent, EventSessionObjectName vSource]
{
    SourceDeclaration vSourceDeclaration = this.FragmentFactory.CreateFragment<SourceDeclaration>();
    BooleanComparisonType vType = BooleanComparisonType.Equals;
    ScalarExpression eventValue;
}
    :    vType = comparisonOperator eventValue = eventDeclarationValue
        {
            vSourceDeclaration.Value = vSource;
            vParent.FirstExpression = vSourceDeclaration;
            vParent.ComparisonType = vType;
            vParent.SecondExpression = eventValue;
        }
    ;    
        
dropEventDeclarationList [AlterEventSessionStatement vParent]
{
    EventSessionObjectName vDropEventDeclaration;
}
    :
        Drop tEvent:Identifier vDropEventDeclaration = eventSessionNonEmptyThreePartObjectName
        {
            Match(tEvent,CodeGenerationSupporter.Event);        
            AddAndUpdateTokenInfo(vParent,vParent.DropEventDeclarations,vDropEventDeclaration);
        }
        (
            Comma Drop tEvent2:Identifier vDropEventDeclaration = eventSessionNonEmptyThreePartObjectName
            {
                Match(tEvent2,CodeGenerationSupporter.Event);
                AddAndUpdateTokenInfo(vParent,vParent.DropEventDeclarations,vDropEventDeclaration);
            }
        )*
    ;        

dropTargetDeclarationList [AlterEventSessionStatement vParent]
{
    EventSessionObjectName vDropTargetDeclaration;
}
    :
        Drop tTarget:Identifier vDropTargetDeclaration = eventSessionNonEmptyThreePartObjectName
        {
            Match(tTarget,CodeGenerationSupporter.Target);
            AddAndUpdateTokenInfo(vParent,vParent.DropTargetDeclarations,vDropTargetDeclaration);
        }
        (
            Comma Drop tTarget2:Identifier vDropTargetDeclaration = eventSessionNonEmptyThreePartObjectName
            {
                Match(tTarget2,CodeGenerationSupporter.Target);
                AddAndUpdateTokenInfo(vParent,vParent.DropTargetDeclarations,vDropTargetDeclaration);
            }
        )*
    ;            
        
createFulltextStatement returns [TSqlStatement vResult = null]
    : tFulltext:Identifier
        {
            Match(tFulltext, CodeGenerationSupporter.Fulltext);
        }
        (vResult = createFulltextCatalogStatement
        |vResult = createFulltextIndexStatement
        |vResult = createFulltextStoplistStatement
        )
    ;

createFulltextCatalogStatement returns [CreateFullTextCatalogStatement vResult = FragmentFactory.CreateFragment<CreateFullTextCatalogStatement>()]
{
    Identifier vName, vFileGroup;
    Literal vPath;
    FullTextCatalogOption vOption;
}
    : tCatalog:Identifier vName=identifier
        {
            Match(tCatalog,CodeGenerationSupporter.Catalog);
            vResult.Name = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (On tFilegroup:Identifier vFileGroup = identifier
            {
                Match(tFilegroup, CodeGenerationSupporter.Filegroup);
                vResult.FileGroup = vFileGroup;
            }
        )?
        (In tPath:Identifier vPath = stringLiteral
            {
                Match(tPath, CodeGenerationSupporter.Path);
                vResult.Path = vPath;                
            }
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With vOption=accentSensitivity
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )?
        (As tDefault:Default
            {
                UpdateTokenInfo(vResult,tDefault);
                vResult.IsDefault = true;
            }
        )?
        authorizationOpt[vResult]        
    ;
    
accentSensitivity returns [OnOffFullTextCatalogOption vResult = FragmentFactory.CreateFragment<OnOffFullTextCatalogOption>()]
{
    OptionState vOptionState;
}
    : tAccentSensitivity:Identifier EqualsSign vOptionState = optionOnOff[vResult]
        {
            Match(tAccentSensitivity, CodeGenerationSupporter.AccentSensitivity);
            vResult.OptionKind=FullTextCatalogOptionKind.AccentSensitivity;
            vResult.OptionState = vOptionState;
        }
    ;
    
fulltextIndexColumn returns [FullTextIndexColumn vResult = this.FragmentFactory.CreateFragment<FullTextIndexColumn>()]
{
    Identifier vIdentifier;
    IdentifierOrValueExpression vLanguageTerm;
}
    :
        vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Type)}?
            Identifier Column vIdentifier=identifier
            {
                
                vResult.TypeColumn = vIdentifier;
            }
        )?
        (
            {NextTokenMatches(CodeGenerationSupporter.Language)}?
            vLanguageTerm = languageTerm
            {
                vResult.LanguageTerm = vLanguageTerm;
            } 
        )?
        (
            {NextTokenMatches(CodeGenerationSupporter.StatisticalSemantics)}?
            tIdentifier:Identifier
            {
                Match(tIdentifier, CodeGenerationSupporter.StatisticalSemantics);
                vResult.StatisticalSemantics=true;
            }
        )?
    ;

createFulltextIndexStatement returns [CreateFullTextIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateFullTextIndexStatement>()]
{
    SchemaObjectName vObjectName;
    FullTextIndexColumn vFulltextIndexColumn;
    Identifier vIdentifier;
    FullTextCatalogAndFileGroup vCatalogAndFileGroup;
}
    :   Index On vObjectName=schemaObjectFourPartName
        {
            vResult.OnName = vObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            LeftParenthesis vFulltextIndexColumn=fulltextIndexColumn
            {
                AddAndUpdateTokenInfo(vResult, vResult.FullTextIndexColumns,vFulltextIndexColumn);
            }
            (
                Comma vFulltextIndexColumn=fulltextIndexColumn
                {
                    AddAndUpdateTokenInfo(vResult, vResult.FullTextIndexColumns,vFulltextIndexColumn);
                }
            )*
            RightParenthesis
        )?
        Key Index vIdentifier=identifier
        {
            vResult.KeyIndexName = vIdentifier;
        }
        (
            vCatalogAndFileGroup = fullTextCatalogAndFileGroup
            {
                vResult.CatalogAndFileGroup = vCatalogAndFileGroup;
            }
        )?
        createFulltextOptions[vResult]
    ;
    
fullTextCatalogAndFileGroup returns [FullTextCatalogAndFileGroup vResult = FragmentFactory.CreateFragment<FullTextCatalogAndFileGroup>()]
{
    Identifier vCatalogName;
    Identifier vFilegroupName;
}
    :   tOn:On
        {
            UpdateTokenInfo(vResult,tOn);
        }
        (
            vCatalogName=identifier
            {
                vResult.CatalogName = vCatalogName;
            }
        |
            LeftParenthesis
            (
                (
                    vCatalogName = identifier
                    {
                        vResult.CatalogName = vCatalogName;
                        vResult.FileGroupIsFirst = false;
                    }
                    (Comma tFilegroup1:Identifier vFilegroupName = identifier
                        {
                            Match(tFilegroup1, CodeGenerationSupporter.Filegroup);
                            vResult.FileGroupName = vFilegroupName;
                        }                    
                    )?
                )
            |
                (
                    tFilegroup2:Identifier vFilegroupName = identifier
                    {
                        Match(tFilegroup2, CodeGenerationSupporter.Filegroup);
                        vResult.FileGroupName = vFilegroupName;
                        vResult.FileGroupIsFirst = true;
                    }
                    (Comma vCatalogName = identifier
                        {
                            vResult.CatalogName = vCatalogName;
                        }
                    )?
                )            
            )
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen);
            }
        )        
    ;
    
createFulltextOptions [CreateFullTextIndexStatement vParent]
    :   
        (   // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With 
            (
                createFulltextOptionsList[vParent]
            |
                LeftParenthesis createFulltextOptionsList[vParent] tRParen:RightParenthesis
                {
                    UpdateTokenInfo(vParent, tRParen);
                }
            )
        )?    
    ;

createFulltextOptionsList [CreateFullTextIndexStatement vParent]
{
    FullTextIndexOption vOption;
    long encounteredOptions = 0;
}
    :   
        vOption=fullTextIndexOption
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (
            Comma vOption = fullTextIndexOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
    ;

fullTextIndexOption returns [FullTextIndexOption vResult]
    :  
        {NextTokenMatches(CodeGenerationSupporter.ChangeTracking)}?
        vResult=changeTrackingFulltextIndexOption
      | vResult=stoplistFulltextIndexOption
      | vResult=searchPropertyListFullTextIndexOption
    ;

changeTrackingFulltextIndexOption returns [ChangeTrackingFullTextIndexOption vResult = FragmentFactory.CreateFragment<ChangeTrackingFullTextIndexOption>()]
    :
        tChangeTracking:Identifier (EqualsSign)?
        {
            Match(tChangeTracking, CodeGenerationSupporter.ChangeTracking);
            vResult.OptionKind=FullTextIndexOptionKind.ChangeTracking;
            UpdateTokenInfo(vResult, tChangeTracking);
        }
        (
            tOff:Off
            {
                UpdateTokenInfo(vResult,tOff);
                vResult.Value = ChangeTrackingOption.Off;
            }
            (   // Greedy due to conflict with identifierStatements
                options {greedy = true; } :
                Comma tNo:Identifier tPopulation:Identifier
                {
                    Match(tNo, CodeGenerationSupporter.No);
                    Match(tPopulation, CodeGenerationSupporter.Population);
                    UpdateTokenInfo(vResult,tPopulation);
                    vResult.Value = ChangeTrackingOption.OffNoPopulation;
                }
            )?
        |
            tOption:Identifier
            {
                if (TryMatch(tOption, CodeGenerationSupporter.Manual))
                    vResult.Value = ChangeTrackingOption.Manual;
                else
                {
                    Match(tOption, CodeGenerationSupporter.Auto);
                    vResult.Value = ChangeTrackingOption.Auto;
                }                    
                UpdateTokenInfo(vResult,tOption);
            }
        )
    ;
    
stoplistFulltextIndexOption returns [StopListFullTextIndexOption vResult = FragmentFactory.CreateFragment<StopListFullTextIndexOption>()]
{
    Identifier vIdentifier;
}
    :   tStoplist:StopList
        {
            UpdateTokenInfo(vResult, tStoplist);
            vResult.OptionKind=FullTextIndexOptionKind.StopList;
        }
        (EqualsSign)?
        (
            vIdentifier = identifier
            {
                vResult.StopListName = vIdentifier;
                vResult.IsOff = false;
            }
        |
            tOff:Off
            {
                vResult.IsOff = true;
                UpdateTokenInfo(vResult, tOff);
            }
        )
    ;

searchPropertyListFullTextIndexOption returns [SearchPropertyListFullTextIndexOption vResult = FragmentFactory.CreateFragment<SearchPropertyListFullTextIndexOption>()]
{
    Identifier vName;
}
    : tSearch:Identifier tProperty:Identifier tList:Identifier 
        {
            Match(tSearch, CodeGenerationSupporter.Search);
            Match(tProperty, CodeGenerationSupporter.Property);
            Match(tList, CodeGenerationSupporter.List);
            vResult.OptionKind=FullTextIndexOptionKind.SearchPropertyList;
        }
        (EqualsSign)?
        (
            vName=identifier
            {
                vResult.PropertyListName=vName;
            }
          | 
            tOff:Off
            {
                vResult.IsOff=true;
                UpdateTokenInfo(vResult, tOff);
            }
        )
    ;
    
createFulltextStoplistStatement returns [CreateFullTextStopListStatement vResult = FragmentFactory.CreateFragment<CreateFullTextStopListStatement>()]
{
    Identifier vName;
    Identifier vDatabaseName;
    Identifier vSourceStoplistName;
}
    : StopList vName=identifier
        {
            vResult.Name = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
         From 
            (
                (
                    vDatabaseName=identifier Dot
                    {
                        vResult.DatabaseName = vDatabaseName;
                    }
                )?
                vSourceStoplistName=identifier
                {
                    vResult.SourceStopListName = vSourceStoplistName;
                    vResult.IsSystemStopList = false;
                }
            |
                tSystem:Identifier StopList
                {
                    Match(tSystem,CodeGenerationSupporter.System);
                    vResult.IsSystemStopList = true;
                    vResult.SourceStopListName = null;
                }
            )
        )?
        authorizationOpt[vResult]
        requiredSemicolon[vResult, "FullText Stoplist"]
    ;

alterFulltextStoplistStatement returns [AlterFullTextStopListStatement vResult = FragmentFactory.CreateFragment<AlterFullTextStopListStatement>()]
{
    Identifier vName;
    FullTextStopListAction vFulltextStoplistAction;
}
    : StopList vName=identifier
        {
            vResult.Name = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            vFulltextStoplistAction = fulltextStoplistAction
            {
                vResult.Action = vFulltextStoplistAction;
            }
         )
       requiredSemicolon[vResult, "FullText Stoplist"]
     ;     

fulltextStoplistAction returns [FullTextStopListAction vResult = null]
{
}     
    : vResult=addFulltextStoplistAction
    | vResult=dropFulltextStoplistAction
    ;

languageTerm returns [IdentifierOrValueExpression vResult = this.FragmentFactory.CreateFragment<IdentifierOrValueExpression>()]
{
    Identifier vIdentifier;
    Literal vLiteral;    
}
    :
        tLanguage:Identifier
        {
            Match(tLanguage,CodeGenerationSupporter.Language);
        }
        (            
            vLiteral=stringLiteral
            {
                CheckIfValidLanguageString(vLiteral);
                vResult.ValueExpression = vLiteral;
            }
        |
            vLiteral=integer
            {
                CheckIfValidLanguageInteger(vLiteral);
                vResult.ValueExpression = vLiteral;
            }
        |
            vIdentifier=identifier
            {
                CheckIfValidLanguageIdentifier(vIdentifier);
                vResult.Identifier = vIdentifier;
            }
        |
            vLiteral=binary
            {
                CheckIfValidLanguageHex(vLiteral);
                vResult.ValueExpression = vLiteral;
            }
        )        
    ;

addFulltextStoplistAction returns [FullTextStopListAction vResult = this.FragmentFactory.CreateFragment<FullTextStopListAction>()]    
{
    Literal vStopWord;
    IdentifierOrValueExpression vLanguageTerm;    
}
    :
        Add
        {
            vResult.IsAdd = true;
        }
        (
            vStopWord = stringLiteral
            {
                vResult.StopWord=vStopWord;
            }
        )
        vLanguageTerm = languageTerm
        {
            vResult.LanguageTerm = vLanguageTerm;
        }
    ;

dropFulltextStoplistAction returns [FullTextStopListAction vResult = FragmentFactory.CreateFragment<FullTextStopListAction>()]    
{
    Literal vStopword;
    IdentifierOrValueExpression vLanguageTerm;
}    
    :
        Drop
        {
            vResult.IsAdd = false;
        }
        (
            vStopword = stringLiteral
            {
                vResult.StopWord=vStopword;
            }
            vLanguageTerm = languageTerm
            {
                vResult.LanguageTerm = vLanguageTerm;
            }
        |
            (
                All
                {
                    vResult.IsAll = true;
                }
                (
                    options {greedy=true;} :
                    vLanguageTerm = languageTerm
                    {
                        vResult.LanguageTerm = vLanguageTerm;
                    }
                )?
            )
        )
    ;

dropFulltextStoplistStatement returns [DropFullTextStopListStatement vResult = FragmentFactory.CreateFragment<DropFullTextStopListStatement>()]
{
    Identifier vName;
}
    : StopList vName = identifier
        {
            vResult.Name = vName;
        }
      requiredSemicolon[vResult, "FullText Stoplist"]
    ;

createSearchPropertyListStatement returns [CreateSearchPropertyListStatement vResult = FragmentFactory.CreateFragment<CreateSearchPropertyListStatement>()]
{
    Identifier vName;
    MultiPartIdentifier vSourceSearchPropertyList;
}
    :   tSearch:Identifier tProperty:Identifier tList:Identifier vName=identifier
        {
            Match(tSearch, CodeGenerationSupporter.Search);
            Match(tProperty, CodeGenerationSupporter.Property);
            Match(tList, CodeGenerationSupporter.List);
            vResult.Name = vName;
        }
        (
            tFrom:From vSourceSearchPropertyList=multiPartIdentifier[2]
            {
                vResult.SourceSearchPropertyList = vSourceSearchPropertyList;
            }
        )?
        authorizationOpt[vResult] 
       requiredSemicolon[vResult, "Search Property List"]
    ;

alterSearchPropertyListStatement returns [AlterSearchPropertyListStatement vResult = FragmentFactory.CreateFragment<AlterSearchPropertyListStatement>()]
{
    Identifier vName;
    SearchPropertyListAction vSearchPropertyListAction;
}
    :   tSearch:Identifier tProperty:Identifier tList:Identifier vName=identifier
        {
            Match(tSearch, CodeGenerationSupporter.Search);
            Match(tProperty, CodeGenerationSupporter.Property);
            Match(tList, CodeGenerationSupporter.List);
            vResult.Name = vName;
        }
        vSearchPropertyListAction=searchPropertyListAction
        {
            vResult.Action=vSearchPropertyListAction;
        }
        requiredSemicolon[vResult, "Search Property List"]
    ;

searchPropertyListAction returns [SearchPropertyListAction vResult]
    :
         vResult=addSearchPropertyListAction
      |  vResult=dropSearchPropertyListAction
    ;

addSearchPropertyListAction returns [AddSearchPropertyListAction vResult = FragmentFactory.CreateFragment<AddSearchPropertyListAction>()]
{
    StringLiteral vName;
    StringLiteral vPropertySetGuid;
    IntegerLiteral vPropertyIntId;
    StringLiteral vDescription;
}
    :   tAdd:Add vName=stringLiteral
        {
            vResult.PropertyName = vName;
            UpdateTokenInfo(vResult, tAdd);
        }
        With LeftParenthesis tPropertySetGuid:Identifier EqualsSign vPropertySetGuid=stringLiteral
        {
            Match(tPropertySetGuid, CodeGenerationSupporter.PropertySetGuid);
            vResult.Guid = vPropertySetGuid;
        }
        Comma tPropertyIntId:Identifier EqualsSign vPropertyIntId=integer
        {
            Match(tPropertyIntId, CodeGenerationSupporter.PropertyIntId);
            vResult.Id = vPropertyIntId;
        }
        (
            Comma tPropertyDescription:Identifier EqualsSign vDescription=stringLiteral
            {
                vResult.Description=vDescription;
            }
        )?
        tRightParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParen);
        }
    ;

dropSearchPropertyListAction returns [DropSearchPropertyListAction vResult = FragmentFactory.CreateFragment<DropSearchPropertyListAction>()]
{
    StringLiteral vPropertyName;
}
    : tDrop:Drop vPropertyName=stringLiteral
        {
            UpdateTokenInfo(vResult, tDrop);
            vResult.PropertyName=vPropertyName;
        }
    ;

dropSearchPropertyListStatement returns [DropSearchPropertyListStatement vResult = FragmentFactory.CreateFragment<DropSearchPropertyListStatement>()]
{
    Identifier vName;
}
    : tSearch:Identifier tProperty:Identifier tList:Identifier vName=identifier
        {
            Match(tSearch, CodeGenerationSupporter.Search);
            Match(tProperty, CodeGenerationSupporter.Property);
            Match(tList, CodeGenerationSupporter.List);
            vResult.Name = vName;
        }
        requiredSemicolon[vResult, "Search Property List"]
    ;

createPrimaryXmlIndexStatement returns [IndexStatement vResult = null]
    : Primary tXml:Identifier 
        {
            Match(tXml, CodeGenerationSupporter.Xml);
        }
        vResult=createXmlIndexStatement[true]
    ;


createSelectiveXmlIndexStatement returns [CreateSelectiveXmlIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateSelectiveXmlIndexStatement>()]
{
    SelectiveXmlIndexPromotedPath vSelectiveXmlIndexPath;
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    XmlNamespaces vXmlNamespaces;
    vResult.IsSecondary = false;
}
    : tSelective:Identifier
        {
            Match(tSelective, CodeGenerationSupporter.Selective);
        }
    tXml:Identifier
        {
            Match(tXml, CodeGenerationSupporter.Xml);
        }
    Index vIdentifier=identifier On vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.Name = vIdentifier;
            vResult.OnName = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
    LeftParenthesis vIdentifier=identifier
        {
            vResult.XmlColumn = vIdentifier;
        }
    tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    (
    tWith:With
        {
            UpdateTokenInfo(vResult,tWith);
        }
    vXmlNamespaces=xmlNamespaces
        {
            vResult.XmlNamespaces = vXmlNamespaces;
        }
    )?
    For LeftParenthesis vSelectiveXmlIndexPath = promotedSelectiveXmlIndexPath 
    {
        AddAndUpdateTokenInfo(vResult, vResult.PromotedPaths, vSelectiveXmlIndexPath);
    }
    (
     Comma vSelectiveXmlIndexPath = promotedSelectiveXmlIndexPath 
        {
            AddAndUpdateTokenInfo(vResult, vResult.PromotedPaths, vSelectiveXmlIndexPath);
        }
     )*
    tRParen2:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen2);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With indexOptionList[IndexAffectingStatement.CreateXmlIndex, vResult.IndexOptions, vResult]
        )?
;
// TODO, sacaglar: currently this statement is PhaseOne only.
createXmlStatements returns [TSqlStatement vResult = null]
    : tXml:Identifier 
        {
            Match(tXml,CodeGenerationSupporter.Xml);
        }
        (
            vResult=createXmlIndexStatement[false]
        |  
            vResult=createXmlSchemaCollectionStatement
        )
    ;

createXmlSchemaCollectionStatement returns [CreateXmlSchemaCollectionStatement vResult = this.FragmentFactory.CreateFragment<CreateXmlSchemaCollectionStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    ScalarExpression vExpression;
}
    :
        Schema tCollection:Identifier vSchemaObjectName=schemaObjectNonEmptyTwoPartName
        {
            Match(tCollection,CodeGenerationSupporter.Collection);
            vResult.Name = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        As vExpression = expression
        {
            vResult.Expression = vExpression;
        }        
    ;

createXmlIndexStatement[bool vPrimary] returns [IndexStatement vResult = null]
{
    Identifier vName;
    Identifier vXmlColumn;
    Identifier vSecondaryXmlIndexName;
    SchemaObjectName vSchemaObjectName;
}
    :
        Index vName=identifier On vSchemaObjectName=schemaObjectThreePartName LeftParenthesis vXmlColumn=identifier tRParen:RightParenthesis 
        (
            {vPrimary}?
            /* empty */
            {
                CreateXmlIndexStatement vCreateXmlIndexStatement = FragmentFactory.CreateFragment<CreateXmlIndexStatement>();
                vCreateXmlIndexStatement.Primary = vPrimary;
                vCreateXmlIndexStatement.Name = vName;
                vCreateXmlIndexStatement.OnName = vSchemaObjectName;
                ThrowPartialAstIfPhaseOne(vCreateXmlIndexStatement);
                vCreateXmlIndexStatement.XmlColumn = vXmlColumn;
                UpdateTokenInfo(vCreateXmlIndexStatement,tRParen);
                vResult = vCreateXmlIndexStatement;
            }
        |   tUsing:Identifier
            {
                Match(tUsing, CodeGenerationSupporter.Using);
            }
            tXml:Identifier Index
            {
                Match(tXml, CodeGenerationSupporter.Xml);
            }
            vSecondaryXmlIndexName=identifier
            (
                vResult = secondaryXmlIndexStatementBody[vName, vSchemaObjectName, vXmlColumn, tRParen, vSecondaryXmlIndexName]
            |   vResult = secondarySelectiveXmlIndex [vName, vSchemaObjectName, vXmlColumn, tRParen, vSecondaryXmlIndexName]
            )
        )
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With indexOptionList[IndexAffectingStatement.CreateXmlIndex, vResult.IndexOptions, vResult]
        )?
    ;

secondaryXmlIndexStatementBody [Identifier vName, SchemaObjectName vSchemaObjectName, Identifier vXmlColumn, IToken tRParen, Identifier vSecondaryXmlIndexName] returns [CreateXmlIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateXmlIndexStatement>()]
{
    vResult.Name = vName;
    vResult.OnName = vSchemaObjectName;
    ThrowPartialAstIfPhaseOne(vResult);
    vResult.XmlColumn = vXmlColumn;
    UpdateTokenInfo(vResult,tRParen);
    vResult.SecondaryXmlIndexName = vSecondaryXmlIndexName;
}
    :   For tOption:Identifier
        {
            vResult.SecondaryXmlIndexType = SecondaryXmlIndexTypeHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult, tOption);
        }
;

secondarySelectiveXmlIndex [Identifier vName, SchemaObjectName vSchemaObjectName, Identifier vXmlColumn, IToken tRParen, Identifier vSecondaryXmlIndexName] returns [CreateSelectiveXmlIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateSelectiveXmlIndexStatement>()]
{
    Identifier vIdentifier;
    vResult.IsSecondary = true;
    vResult.Name = vName;
    vResult.OnName = vSchemaObjectName;
    ThrowPartialAstIfPhaseOne(vResult);
    vResult.XmlColumn = vXmlColumn;
    UpdateTokenInfo(vResult,tRParen);
    vResult.UsingXmlIndexName = vSecondaryXmlIndexName;
}
    : For LeftParenthesis vIdentifier=identifier
        {
            vResult.PathName = vIdentifier;
        }
    tRParenthesis:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParenthesis);
        }
;

promotedSelectiveXmlIndexPath returns [SelectiveXmlIndexPromotedPath vResult = FragmentFactory.CreateFragment<SelectiveXmlIndexPromotedPath>()]
{
    Identifier vPathName;
    Identifier vSQL;
    Identifier vXQuery = null;
    StringLiteral vPath;
    StringLiteral vXQueryType = null;
    DataTypeReference vSQLDataType;
    bool vSingleton = false;
    IntegerLiteral vMaxLength = null;
}
    : vPathName = identifier EqualsSign vPath = stringLiteral
        {
            vResult.Path = vPath;
            vResult.Name = vPathName;
        }
    (
    As
    (
        {NextTokenMatches(CodeGenerationSupporter.XQuery)}?
        vXQuery = identifier 
        (
            vXQueryType = stringLiteral
                {
                    vResult.XQueryDataType = vXQueryType;
                }
        )?
        vMaxLength = maxlengthOption[vResult]
            {
                vResult.MaxLength = vMaxLength;
            }
        | {NextTokenMatches(CodeGenerationSupporter.Sql)}?
        vSQL = identifier
            {
                Match(vSQL, CodeGenerationSupporter.Sql);
            }
        vSQLDataType = scalarDataType
            {
                vResult.SQLDataType = vSQLDataType;
            }
    )
    vSingleton = singletonOption
        {
            vResult.IsSingleton = vSingleton;
        }
    )?
    {
        if(vXQuery != null && vXQueryType == null && vMaxLength == null && !vSingleton)
        {
            ThrowIncorrectSyntaxErrorException(vXQuery);
        }
    }
;

singletonOption returns [bool vIsSingleton = false]
{
    Identifier vSingleton;
}
    :
    (
        {NextTokenMatches(CodeGenerationSupporter.Singleton)}?
        vSingleton = identifier
        {
                vIsSingleton = true;
        }
    )?
;

maxlengthOption[TSqlFragment vParent] returns [IntegerLiteral vResult = null]
{
    Identifier vMaxLength;
    IntegerLiteral vIntLiteral;
}
    :
    (
        {NextTokenMatches(CodeGenerationSupporter.MaxLength)}?
        vMaxLength = identifier LeftParenthesis vIntLiteral = integer
            {
                vResult = vIntLiteral;
            }
        tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent,tRParen);
            }
    )?
;

promotedSelectiveXmlIndexPathInAlter returns [SelectiveXmlIndexPromotedPath vResult = null]
{
    SelectiveXmlIndexPromotedPath vAlterPromotedXMLIndexPath;
    Identifier vPathName;
}
    : Add vAlterPromotedXMLIndexPath = promotedSelectiveXmlIndexPath
        {
            vResult = vAlterPromotedXMLIndexPath;
        }
    | tRemove : Identifier 
        {
            Match(tRemove, CodeGenerationSupporter.Remove);
        }
    vPathName = identifier
        {
            vResult = FragmentFactory.CreateFragment<SelectiveXmlIndexPromotedPath>();
            vResult.Name = vPathName;
        }
;

createLoginStatement returns [CreateLoginStatement vResult = FragmentFactory.CreateFragment<CreateLoginStatement>()]
{
    Identifier vIdentifier;
    CreateLoginSource vLoginSource;
}
    : tLogin:Identifier vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (   vLoginSource = passwordLoginSource
            {
                vResult.Source = vLoginSource;
            }
        |
            From
            (   {NextTokenMatches(CodeGenerationSupporter.Windows)}?
                vLoginSource = windowsLoginSource
            |    vLoginSource = certificateLoginSource
            |   vLoginSource =  asymmetricKeyLoginSource
            )
            {
                vResult.Source = vLoginSource;
            }
        )
    ;
    
passwordLoginSource returns [PasswordCreateLoginSource vResult = FragmentFactory.CreateFragment<PasswordCreateLoginSource>()]
{
    PrincipalOption vParam;
    Literal vPassword;
    long encounteredOptions = 0;
}
    : With tPassword:Identifier EqualsSign vPassword = loginPassword 
        {
            Match(tPassword,CodeGenerationSupporter.Password);
            vResult.Password = vPassword;            
        }
        (options {greedy = true; } : createLoginPasswordOption[vResult])* 
        (Comma vParam = createLoginParam
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vParam.OptionKind, vParam);
                AddAndUpdateTokenInfo(vResult, vResult.Options,vParam);
            }
        )*
    ;

loginPassword returns [Literal vResult]
    : vResult = binary
    | vResult = stringLiteral
    ;

createLoginPasswordOption [PasswordCreateLoginSource vParent]
    : tHashedMustChange:Identifier
        {
            if (TryMatch(tHashedMustChange, CodeGenerationSupporter.Hashed))
            {
                if (vParent.Hashed)
                    throw GetUnexpectedTokenErrorException(tHashedMustChange);
                else
                    vParent.Hashed = true;
            }
            else
            {
                Match(tHashedMustChange, CodeGenerationSupporter.MustChange);
                if (vParent.MustChange)
                    throw GetUnexpectedTokenErrorException(tHashedMustChange);
                else
                    vParent.MustChange = true;
            }
            UpdateTokenInfo(vParent,tHashedMustChange);
        }
    ;

createLoginParam returns [PrincipalOption vResult = null]
{
    Literal vSid;
    Identifier vIdentifier;
}
    : tOption:Identifier EqualsSign
        ( 
          vResult = onOffPrincipalOption[tOption]
        | vSid = binary
            {
                Match(tOption,CodeGenerationSupporter.Sid);
                LiteralPrincipalOption vSidCreateLoginOption = FragmentFactory.CreateFragment<LiteralPrincipalOption>();
                vSidCreateLoginOption.OptionKind = PrincipalOptionKind.Sid;
                vSidCreateLoginOption.Value = vSid;
                vResult = vSidCreateLoginOption;
            }
        | vIdentifier = identifier
            {
                IdentifierPrincipalOption vIdentifierLoginOption = FragmentFactory.CreateFragment<IdentifierPrincipalOption>();
                vIdentifierLoginOption.OptionKind = IdentifierCreateLoginOptionsHelper.Instance.ParseOption(tOption);
                vIdentifierLoginOption.Identifier = vIdentifier;
                vResult = vIdentifierLoginOption;
            }
        )
        {
            UpdateTokenInfo(vResult, tOption);
        }
    ;

onOffPrincipalOption[IToken tOption] returns [OnOffPrincipalOption vResult = FragmentFactory.CreateFragment<OnOffPrincipalOption>()]
{
    OptionState vOptionState;
}
    : vOptionState = optionOnOff[vResult]
        {
            vResult.OptionKind = SecurityLoginOptionsHelper.Instance.ParseOption(tOption);
            vResult.OptionState = vOptionState;
        }
    ;
    
windowsLoginSource returns [WindowsCreateLoginSource vResult = FragmentFactory.CreateFragment<WindowsCreateLoginSource>()]
{
    long encounteredOptions = 0;
    IdentifierPrincipalOption vOption;
}
    : tWindows:Identifier
        {
            Match(tWindows, CodeGenerationSupporter.Windows);
            UpdateTokenInfo(vResult,tWindows);
        }
        (
        // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        options {greedy = true; } : 
        With vOption = createLoginParamWin
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            }
            (Comma vOption = createLoginParamWin
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                    CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                }
            )*
        )?
    ;
    
createLoginParamWin returns [IdentifierPrincipalOption vResult = FragmentFactory.CreateFragment<IdentifierPrincipalOption>()]
{
    Identifier vValue;
}
    : tOption:Identifier EqualsSign vValue = identifier
        {
            vResult.OptionKind = IdentifierCreateLoginOptionsHelper.Instance.ParseOption(tOption);
            if (vResult.OptionKind == PrincipalOptionKind.Credential)
                throw GetUnexpectedTokenErrorException(tOption);
            vResult.Identifier = vValue;                
        }
    ;

certificateLoginSource returns [CertificateCreateLoginSource vResult = FragmentFactory.CreateFragment<CertificateCreateLoginSource>()]
{
    Identifier vIdentifier, vCredential;
}
    : tCertificate:Identifier vIdentifier = identifier vCredential = createLoginParamCertOpt
        {
            Match(tCertificate,CodeGenerationSupporter.Certificate);
            vResult.Certificate = vIdentifier;
            if (vCredential != null)
                vResult.Credential = vCredential;            
        }
    ;
    
createLoginParamCertOpt returns [Identifier vResult = null]
    : ( // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        options {greedy = true; } : 
        With tCredential:Identifier EqualsSign vResult = identifier
        {
            Match(tCredential,CodeGenerationSupporter.Credential);
        }
      )?
    ;

asymmetricKeyLoginSource returns [AsymmetricKeyCreateLoginSource vResult = FragmentFactory.CreateFragment<AsymmetricKeyCreateLoginSource>()]
{
    Identifier vKey, vCredential;
}
    : tAsymmetric:Identifier Key vKey = identifier vCredential = createLoginParamCertOpt
    {
        Match(tAsymmetric,CodeGenerationSupporter.Asymmetric);
        vResult.Key = vKey;
        if (vCredential != null)
            vResult.Credential = vCredential;            
    }
    ;

createMessageTypeStatement returns [CreateMessageTypeStatement vResult = FragmentFactory.CreateFragment<CreateMessageTypeStatement>()]
{
    Identifier vIdentifier;
}
    : tMessage:Identifier tType:Identifier vIdentifier=identifier
        {
            Match(tMessage, CodeGenerationSupporter.Message);
            Match(tType, CodeGenerationSupporter.Type);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult] (messageTypeValidation[vResult])?        
    ;
    
messageTypeValidation [MessageTypeStatementBase vParent]
{
    SchemaObjectName vSchemaObjectName;
}
    : tValidation:Identifier EqualsSign tOption:Identifier 
        {
            Match(tValidation, CodeGenerationSupporter.Validation);
            vParent.ValidationMethod = MessageValidationMethodsHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vParent,tOption);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            tWith:With Schema tCollection:Identifier vSchemaObjectName=schemaObjectNonEmptyTwoPartName
            {
                if (vParent.ValidationMethod != MessageValidationMethod.ValidXml)
                    throw GetUnexpectedTokenErrorException(tWith);
                    
                Match(tCollection, CodeGenerationSupporter.Collection);
                vParent.XmlSchemaCollectionName = vSchemaObjectName;
            }
        )?
        (
            {
                if (vParent.ValidationMethod == MessageValidationMethod.ValidXml &&
                    vParent.XmlSchemaCollectionName == null)
                {
                    ThrowIncorrectSyntaxErrorException(tOption);
                }
            }
        )
    ;

createAvailabilityGroupStatement returns [CreateAvailabilityGroupStatement vResult = FragmentFactory.CreateFragment<CreateAvailabilityGroupStatement>()]
{
    Identifier vName;
    AvailabilityGroupOption vAvailabilityGroupOption;
    Identifier vDatabaseName;
    AvailabilityReplica vAvailabilityReplica;
}
    : 
        tAvailability:Identifier Group vName=identifier
        {
            Match(tAvailability, CodeGenerationSupporter.Availability);
            vResult.Name=vName;
        }
        (
            tWith:With LeftParenthesis
            vAvailabilityGroupOption=availabilityGroupOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vAvailabilityGroupOption);
            }
            RightParenthesis
        )?
        For 
        (
            Database vDatabaseName=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
            }
            (
                Comma vDatabaseName=identifier
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
                }
            )*
        )?
        tReplica:Identifier On
        vAvailabilityReplica=availabilityReplica
        {
            Match(tReplica, CodeGenerationSupporter.Replica);
            AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
        }
        (
            Comma vAvailabilityReplica=availabilityReplica
            {
                AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            }
        )*
    ;

alterAvailabilityGroupStatement returns [AlterAvailabilityGroupStatement vResult = FragmentFactory.CreateFragment<AlterAvailabilityGroupStatement>()]
{
    Identifier vName;
}
    :
        tAvailability:Identifier Group vName=identifier
        {
            Match(tAvailability, CodeGenerationSupporter.Availability);
            vResult.Name=vName;
        }
     (
        alterAvailabilityGroupAddDatabase[vResult]
    |    alterAvailabilityGroupRemoveDatabase[vResult]
    |    alterAvailabilityGroupAddReplica[vResult]
    |    
        {NextTokenMatches(CodeGenerationSupporter.Modify)}?
        alterAvailabilityGroupModifyReplica[vResult]
    |    
        {NextTokenMatches(CodeGenerationSupporter.Remove)}?
        alterAvailabilityGroupRemoveReplica[vResult]
    |    alterAvailabilityGroupSetOption[vResult]
    |    alterAvailabilityGroupTakeAction[vResult]
     )
    ;

alterAvailabilityGroupAddDatabase[AlterAvailabilityGroupStatement vResult]
{
    Identifier vDatabaseName;
}
    : Add Database vDatabaseName=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.AddDatabase;
        }
        (
            Comma vDatabaseName=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
            }
        )*
    ;

alterAvailabilityGroupRemoveDatabase[AlterAvailabilityGroupStatement vResult]
{
    Identifier vDatabaseName;
}
    : tRemove:Identifier Database vDatabaseName=identifier
        {
            Match(tRemove, CodeGenerationSupporter.Remove);
            AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.RemoveDatabase;
        }
        (
            Comma vDatabaseName=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Databases, vDatabaseName);
            }
        )*
    ;

alterAvailabilityGroupAddReplica[AlterAvailabilityGroupStatement vResult]
{
    AvailabilityReplica vAvailabilityReplica;
}
    : Add tReplica:Identifier On vAvailabilityReplica=availabilityReplica
        {
            Match(tReplica, CodeGenerationSupporter.Replica);
            AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.AddReplica;
        }
        (
            Comma vAvailabilityReplica=availabilityReplica
            {
                AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            }
        )*
    ;

alterAvailabilityGroupModifyReplica[AlterAvailabilityGroupStatement vResult]
{
    AvailabilityReplica vAvailabilityReplica;
}
    : tModify:Identifier tReplica:Identifier On vAvailabilityReplica=availabilityReplica
        {
            Match(tModify, CodeGenerationSupporter.Modify);
            Match(tReplica, CodeGenerationSupporter.Replica);
            AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.ModifyReplica;
        }
        (
            Comma vAvailabilityReplica=availabilityReplica
            {
                AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            }
        )*
    ;

alterAvailabilityGroupRemoveReplica[AlterAvailabilityGroupStatement vResult]
{
    AvailabilityReplica vAvailabilityReplica;
}
    : tRemove:Identifier tReplica:Identifier On vAvailabilityReplica=availabilityReplicaName
        {
            Match(tRemove, CodeGenerationSupporter.Remove);
            Match(tReplica, CodeGenerationSupporter.Replica);
            AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.RemoveReplica;
        }
        (
            Comma vAvailabilityReplica=availabilityReplicaName
            {
                AddAndUpdateTokenInfo(vResult, vResult.Replicas, vAvailabilityReplica);
            }
        )*
    ;

alterAvailabilityGroupSetOption[AlterAvailabilityGroupStatement vResult]
{
    AvailabilityGroupOption vAvailabilityGroupOption;
}
    : Set LeftParenthesis vAvailabilityGroupOption=availabilityGroupOption tRightParenthesis:RightParenthesis
        {
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.Set;
            AddAndUpdateTokenInfo(vResult, vResult.Options, vAvailabilityGroupOption);
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
    ;

alterAvailabilityGroupTakeAction[AlterAvailabilityGroupStatement vResult]
{
    AlterAvailabilityGroupAction vAlterAvailabilityGroupAction;
}
    :    
    (
        {NextTokenMatches(CodeGenerationSupporter.Failover)}?
        vAlterAvailabilityGroupAction=alterAvailabilityGroupFailoverAction
        | vAlterAvailabilityGroupAction=alterAvailabilityGroupAction
    )
        {
            vResult.AlterAvailabilityGroupStatementType = AlterAvailabilityGroupStatementType.Action;
            vResult.Action=vAlterAvailabilityGroupAction;
        }
    ;

alterAvailabilityGroupFailoverAction returns [AlterAvailabilityGroupFailoverAction vResult = FragmentFactory.CreateFragment<AlterAvailabilityGroupFailoverAction>()]
{
    AlterAvailabilityGroupFailoverOption vAlterAvailabilityGroupFailoverOption;
}
    : tFailover:Identifier
      {
        Match(tFailover, CodeGenerationSupporter.Failover);
        vResult.ActionType=AlterAvailabilityGroupActionType.Failover;
        UpdateTokenInfo(vResult, tFailover);
      }
      (
        // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        options {greedy = true; } :
        With LeftParenthesis vAlterAvailabilityGroupFailoverOption=alterAvailabilityGroupFailoverOption tRightParenthesis:RightParenthesis
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options, vAlterAvailabilityGroupFailoverOption);
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
      )?
    ;

alterAvailabilityGroupFailoverOption returns [AlterAvailabilityGroupFailoverOption vResult=FragmentFactory.CreateFragment<AlterAvailabilityGroupFailoverOption>()]
{
    Literal vServerName;
}    
    : tTarget:Identifier EqualsSign vServerName=stringLiteral
      {
        Match(tTarget, CodeGenerationSupporter.Target);
        vResult.OptionKind=FailoverActionOptionKind.Target;
        vResult.Value=vServerName;
      }
    ;

alterAvailabilityGroupAction returns [AlterAvailabilityGroupAction vResult = FragmentFactory.CreateFragment<AlterAvailabilityGroupAction>()]
    : tAction:Identifier
      {
        vResult.ActionType=AlterAvailabilityGroupActionTypeHelper.Instance.ParseOption(tAction);
        UpdateTokenInfo(vResult, tAction);
      }
    | 
      tJoin:Join
      {
        vResult.ActionType=AlterAvailabilityGroupActionType.Join;
        UpdateTokenInfo(vResult, tJoin);
      }
    ;

availabilityGroupOption returns [LiteralAvailabilityGroupOption vResult = FragmentFactory.CreateFragment<LiteralAvailabilityGroupOption>()]
{
    Literal vLiteral;
}
    : tRequiredCopiesToCommit:Identifier EqualsSign vLiteral=integer
        {
            Match(tRequiredCopiesToCommit, CodeGenerationSupporter.RequiredCopiesToCommit);
            vResult.Value=vLiteral;
        }
    ;

availabilityReplicaName returns [AvailabilityReplica vResult = FragmentFactory.CreateFragment<AvailabilityReplica>()]
{
    StringLiteral vServerName;
}   : vServerName=stringLiteral
        {
            vResult.ServerName=vServerName;
        }
    ;

availabilityReplica returns [AvailabilityReplica vResult = FragmentFactory.CreateFragment<AvailabilityReplica>()]
{
    StringLiteral vServerName;
    AvailabilityReplicaOption vAvailabilityReplicaOption;
    long encounteredOptions = 0;
}
    : vServerName=stringLiteral
        {
            vResult.ServerName=vServerName;
        }
        With LeftParenthesis
        (
            vAvailabilityReplicaOption = availabilityReplicaOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vAvailabilityReplicaOption.OptionKind, vAvailabilityReplicaOption);
                AddAndUpdateTokenInfo(vResult, vResult.Options, vAvailabilityReplicaOption);
            }
            (
                Comma vAvailabilityReplicaOption = availabilityReplicaOption
                {
                    CheckOptionDuplication(ref encounteredOptions, (int)vAvailabilityReplicaOption.OptionKind, vAvailabilityReplicaOption);
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vAvailabilityReplicaOption);
                }
            )*
        )
        tRightParenthesis:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
    ;

availabilityReplicaOption returns [AvailabilityReplicaOption vResult]
    : 
        {NextTokenMatches(CodeGenerationSupporter.AvailabilityMode)}?
        vResult = availabilityModeReplicaOption
    |
        {NextTokenMatches(CodeGenerationSupporter.FailoverMode)}?
        vResult = failoverModeReplicaOption
    |
        {NextTokenMatches(CodeGenerationSupporter.SecondaryRole)}?
        vResult = secondaryRoleReplicaOption
    |
        {NextTokenMatches(CodeGenerationSupporter.PrimaryRole)}?
        vResult = primaryRoleReplicaOption
    |
        vResult = literalReplicaOption
    ;

availabilityModeReplicaOption returns [AvailabilityModeReplicaOption vResult = FragmentFactory.CreateFragment<AvailabilityModeReplicaOption>()]
    : 
        tAvailabilityMode:Identifier EqualsSign tOption:Identifier
        {
            Match(tAvailabilityMode, CodeGenerationSupporter.AvailabilityMode);
            vResult.OptionKind=AvailabilityReplicaOptionKind.AvailabilityMode;
            if(TryMatch(tOption, CodeGenerationSupporter.SynchronousCommit))
            {
                vResult.Value = AvailabilityModeOptionKind.SynchronousCommit;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.AsynchronousCommit);
                vResult.Value = AvailabilityModeOptionKind.AsynchronousCommit;
            }
            UpdateTokenInfo(vResult, tOption);
        }
    ;

failoverModeReplicaOption returns [FailoverModeReplicaOption vResult = FragmentFactory.CreateFragment<FailoverModeReplicaOption>()]
    : 
        tFailoverMode:Identifier EqualsSign tOption:Identifier
        {
            Match(tFailoverMode, CodeGenerationSupporter.FailoverMode);
            vResult.OptionKind=AvailabilityReplicaOptionKind.FailoverMode;
            if(TryMatch(tOption, CodeGenerationSupporter.Automatic))
            {
                vResult.Value = FailoverModeOptionKind.Automatic;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.Manual);
                vResult.Value = FailoverModeOptionKind.Manual;
            }
            UpdateTokenInfo(vResult, tOption);
        }
    ;

primaryRoleReplicaOption returns [PrimaryRoleReplicaOption vResult = FragmentFactory.CreateFragment<PrimaryRoleReplicaOption>()]
    : 
        tPrimaryRole:Identifier LeftParenthesis tAllowConnections:Identifier EqualsSign
        {
            Match(tPrimaryRole, CodeGenerationSupporter.PrimaryRole);
            Match(tAllowConnections, CodeGenerationSupporter.AllowConnections);
            vResult.OptionKind=AvailabilityReplicaOptionKind.PrimaryRole;
        }
        (
            tOption:Identifier
            {
                if(TryMatch(tOption, CodeGenerationSupporter.No))
                {
                    vResult.AllowConnections = AllowConnectionsOptionKind.No;
                }
                else 
                {
                    Match(tOption, CodeGenerationSupporter.ReadWrite);
                    vResult.AllowConnections = AllowConnectionsOptionKind.ReadWrite;
                }
                UpdateTokenInfo(vResult, tOption);
            }
        |
            tAll:All
            {
                vResult.AllowConnections = AllowConnectionsOptionKind.All;
                UpdateTokenInfo(vResult, tAll);
            }
        )
        tRightParenthesis:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
    ;

secondaryRoleReplicaOption returns [SecondaryRoleReplicaOption vResult = FragmentFactory.CreateFragment<SecondaryRoleReplicaOption>()]
    : 
        tSecondaryRole:Identifier LeftParenthesis tAllowConnections:Identifier EqualsSign 
        {
            Match(tSecondaryRole, CodeGenerationSupporter.SecondaryRole);
            Match(tAllowConnections, CodeGenerationSupporter.AllowConnections);
            vResult.OptionKind=AvailabilityReplicaOptionKind.SecondaryRole;
        }
        (
            tOption:Identifier
            {
                if(TryMatch(tOption, CodeGenerationSupporter.No))
                {
                    vResult.AllowConnections = AllowConnectionsOptionKind.No;
                }
                else 
                {
                    Match(tOption, CodeGenerationSupporter.ReadOnly);
                    vResult.AllowConnections = AllowConnectionsOptionKind.ReadOnly;
                }
                UpdateTokenInfo(vResult, tOption);
            }
        |
            tAll:All
            {
                vResult.AllowConnections = AllowConnectionsOptionKind.All;
                UpdateTokenInfo(vResult, tAll);
            }
        )
        tRightParenthesis:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParenthesis);
        }
    ;

literalReplicaOption returns [LiteralReplicaOption vResult = FragmentFactory.CreateFragment<LiteralReplicaOption>()]
{
    Literal vLiteral;
}
    :    
        tOption:Identifier EqualsSign 
        (
            vLiteral=stringLiteral
            {
                Match(tOption, CodeGenerationSupporter.EndpointUrl);
                vResult.OptionKind=AvailabilityReplicaOptionKind.EndpointUrl;
                vResult.Value=vLiteral;
            }
        |
            vLiteral=integer
            {
                if(TryMatch(tOption, CodeGenerationSupporter.SessionTimeout))
                {
                    vResult.OptionKind=AvailabilityReplicaOptionKind.SessionTimeout;
                }
                else
                {
                    Match(tOption, CodeGenerationSupporter.ApplyDelay);
                    vResult.OptionKind=AvailabilityReplicaOptionKind.ApplyDelay;
                }
                vResult.Value=vLiteral;
            }
        )
    ;    

createFederationStatement returns [CreateFederationStatement vResult = FragmentFactory.CreateFragment<CreateFederationStatement>()]
{
    Identifier vName;
    Identifier vDistributionName;
    DataTypeReference vDataType;
}
    : 
        tCreate:Create 
        {
            UpdateTokenInfo(vResult,tCreate);
        }
        tFederation:Identifier vName = identifier
        {
            Match(tFederation, CodeGenerationSupporter.Federation);
            vResult.Name = vName;
        }
        LeftParenthesis
        vDistributionName = identifier
        {
            vResult.DistributionName = vDistributionName;
        }
        vDataType = scalarDataType
        {
            vResult.DataType = vDataType;
        }
        tRange : Identifier
        {
            Match(tRange, CodeGenerationSupporter.Range);
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }        
    ;

createPartitionStatement returns [TSqlStatement vResult = null]
    : 
        tPartition:Identifier
        {
            Match(tPartition, CodeGenerationSupporter.Partition);
        }
        ( 
            vResult = createPartitionFunction
        | 
            vResult = createPartitionScheme
        )
    ;
    
createPartitionFunction returns [CreatePartitionFunctionStatement vResult = FragmentFactory.CreateFragment<CreatePartitionFunctionStatement>()]
{
    Identifier vIdentifier;
    PartitionParameterType vParamType;
}
    :    Function vIdentifier = identifier 
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        LeftParenthesis 
        vParamType = partitionParameterType 
            {
                vResult.ParameterType = vParamType;
            }
        RightParenthesis As partitionFunctionTypes[vResult]
    ;
    
partitionParameterType returns [PartitionParameterType vResult = FragmentFactory.CreateFragment<PartitionParameterType>()]
{
    DataTypeReference vDataType;
}
    : vDataType = scalarDataType
        {
            vResult.DataType = vDataType;
        }
        collationOpt[vResult]    
    ;
    
partitionFunctionTypes [CreatePartitionFunctionStatement vParent]
{
    ScalarExpression vExpression;
}
    : tRange:Identifier partitionFunctionRange[vParent] For Values LeftParenthesis 
        {
            Match(tRange, CodeGenerationSupporter.Range);
        }
        (vExpression = expression 
            {
                AddAndUpdateTokenInfo(vParent, vParent.BoundaryValues,vExpression);
            }
            (Comma vExpression = expression
                {
                    AddAndUpdateTokenInfo(vParent, vParent.BoundaryValues,vExpression);
                }
            )* 
        )? 
      tRParen:RightParenthesis
      {
        UpdateTokenInfo(vParent,tRParen);
      }
//  | tHash:Identifier integer // Supported only under traceflag 3817 for development purposes, so, not implementing...
    ;
    
partitionFunctionRange [CreatePartitionFunctionStatement vParent]
    : /* empty */
    | tLeft:Left
        {
            vParent.Range = PartitionFunctionRange.Left;
            UpdateTokenInfo(vParent,tLeft);
        }
    | tRight:Right
        {
            vParent.Range = PartitionFunctionRange.Right;
            UpdateTokenInfo(vParent,tRight);
        }
    ;
    
createPartitionScheme returns [CreatePartitionSchemeStatement vResult = FragmentFactory.CreateFragment<CreatePartitionSchemeStatement>()]
{
    Identifier vIdentifier;
    IdentifierOrValueExpression vFileGroup;
}
    : tScheme:Identifier vIdentifier = identifier 
        {
            Match(tScheme, CodeGenerationSupporter.Scheme);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        As tPartition:Identifier vIdentifier = identifier
        {
            Match(tPartition,CodeGenerationSupporter.Partition);
            vResult.PartitionFunction = vIdentifier;
        }
        (All
            {
                vResult.IsAll = true;
            }
        )? 
        To LeftParenthesis vFileGroup = stringOrIdentifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.FileGroups,vFileGroup);
        }
        (Comma vFileGroup = stringOrIdentifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.FileGroups,vFileGroup);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

createQueueStatement returns [CreateQueueStatement vResult = this.FragmentFactory.CreateFragment<CreateQueueStatement>()]
{
    SchemaObjectName vQueueName;
    IdentifierOrValueExpression vTSqlFragment;
}
    : tQueue:Identifier vQueueName=schemaObjectThreePartName
        {
            Match(tQueue, CodeGenerationSupporter.Queue);
            vResult.Name = vQueueName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            queueOptionList[vResult, false]
        )?
        (
            On
            vTSqlFragment=stringOrIdentifier
            {
                vResult.OnFileGroup = vTSqlFragment;
            }
        )?
    ;

createRemoteServiceBindingStatement returns [CreateRemoteServiceBindingStatement vResult = FragmentFactory.CreateFragment<CreateRemoteServiceBindingStatement>()]
{
    Identifier vIdentifier;
    Literal vService;
    RemoteServiceBindingOption vOption;
}
    : tRemote:Identifier tService:Identifier tBinding:Identifier vIdentifier=identifier
        {
            Match(tRemote, CodeGenerationSupporter.Remote);
            Match(tService, CodeGenerationSupporter.Service);
            Match(tBinding, CodeGenerationSupporter.Binding);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        To tService2:Identifier vService=stringLiteral 
        {
            Match(tService2,CodeGenerationSupporter.Service);
            vResult.Service = vService;
        }
        With
        (
            (
                vOption=bindingUserOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                } 
                (
                    Comma vOption=bindingAnonymousOption
                    {
                        AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                    }
                )?
            )
        |
            (
                vOption=bindingAnonymousOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                } 
                (
                    Comma vOption=bindingUserOption
                    {
                        AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                    }
                )?
            )
        )
    ;
    
bindingUserOption returns [UserRemoteServiceBindingOption vResult = FragmentFactory.CreateFragment<UserRemoteServiceBindingOption>()]
{
    Identifier vUser;
}
    : User EqualsSign vUser = identifier
        {
            vResult.OptionKind = RemoteServiceBindingOptionKind.User;
            vResult.User= vUser;
        }
    ;
    
bindingAnonymousOption returns [OnOffRemoteServiceBindingOption vResult = FragmentFactory.CreateFragment<OnOffRemoteServiceBindingOption>()]
{
    OptionState vOptionState;
}
    : tAnonymous:Identifier EqualsSign vOptionState = optionOnOff[vResult]
        {
            Match(tAnonymous, CodeGenerationSupporter.Anonymous);
            vResult.OptionKind = RemoteServiceBindingOptionKind.Anonymous;
            vResult.OptionState = vOptionState;
        }
    ;

createRoleStatement returns [CreateRoleStatement vResult = this.FragmentFactory.CreateFragment<CreateRoleStatement>()]
{
    Identifier vIdentifier;
}
    :   tRole:Identifier vIdentifier=identifier
        {
            Match(tRole, CodeGenerationSupporter.Role);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
    ;

createServerRoleStatement returns [CreateServerRoleStatement vResult = this.FragmentFactory.CreateFragment<CreateServerRoleStatement>()]
{
    Identifier vIdentifier;
}
    :   tRole:Identifier vIdentifier=identifier
        {
            Match(tRole, CodeGenerationSupporter.Role);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
    ;

createRouteStatement returns [CreateRouteStatement vResult = this.FragmentFactory.CreateFragment<CreateRouteStatement>()]
{
    Identifier vIdentifier;
}
    : tRoute:Identifier vIdentifier=identifier
        {
            Match(tRoute, CodeGenerationSupporter.Route);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        routeOptionList[vResult]
    ;

createServiceStatement returns [CreateServiceStatement vResult = FragmentFactory.CreateFragment<CreateServiceStatement>()]
{
    Identifier vIdentifier;
    ServiceContract vServiceContract;
}
    : tService:Identifier vIdentifier=identifier
        {
            Match(tService, CodeGenerationSupporter.Service);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        onQueueClause[vResult]
        // Greedy due to conflict with Select, which can start with '(' as well
        (options {greedy = true; } : LeftParenthesis vServiceContract = serviceContract
            {
                AddAndUpdateTokenInfo(vResult, vResult.ServiceContracts,vServiceContract);
            }
            (Comma vServiceContract = serviceContract
                {
                    AddAndUpdateTokenInfo(vResult, vResult.ServiceContracts,vServiceContract);
                }
            )*
        tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;
    
onQueueClause [AlterCreateServiceStatementBase vParent]
{
    SchemaObjectName vQueueName;
}
    : On tQueue:Identifier vQueueName = schemaObjectThreePartName
        {
            Match(tQueue, CodeGenerationSupporter.Queue);
            CheckTwoPartNameForSchemaObjectName(vQueueName, CodeGenerationSupporter.Queue);
            vParent.QueueName = vQueueName;
        }
    ;
    
serviceContract returns [ServiceContract vResult = FragmentFactory.CreateFragment<ServiceContract>()]
{
    Identifier vIdentifier;
}
    : vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
        }
    ;
    
alterServiceStatement returns [AlterServiceStatement vResult = FragmentFactory.CreateFragment<AlterServiceStatement>()]
{
    Identifier vIdentifier;
}
    : vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            (onQueueClause[vResult] 
                // Greedy due to conflict with Select, which can start with '(' as well
                (options {greedy = true; } : addDropServiceContractList[vResult])?
            )
        |
            addDropServiceContractList[vResult]
        )
    ;

addDropServiceContractList [AlterServiceStatement vParent]
{
    ServiceContract vServiceContract;
}
    : LeftParenthesis vServiceContract = addDropServiceContract
        {
            AddAndUpdateTokenInfo(vParent, vParent.ServiceContracts,vServiceContract);
        }
        (Comma vServiceContract = addDropServiceContract
            {
                AddAndUpdateTokenInfo(vParent, vParent.ServiceContracts,vServiceContract);
            }
        )*
    tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

addDropServiceContract returns [ServiceContract vResult = FragmentFactory.CreateFragment<ServiceContract>()]
{
    Identifier vIdentifier;
}
    :(tDrop:Drop
        {
            UpdateTokenInfo(vResult,tDrop);
            vResult.Action = AlterAction.Drop;
        }
    | tAdd:Add 
        {
            UpdateTokenInfo(vResult,tAdd);
            vResult.Action = AlterAction.Add;
        }
    ) 
    tContract:Identifier vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
        }
    ;

createSymmetricKeyStatement returns [CreateSymmetricKeyStatement vResult = FragmentFactory.CreateFragment<CreateSymmetricKeyStatement>()]
{
    Identifier vIdentifier;
    Identifier vProvider;
}
    : tSymmetric:Identifier Key vIdentifier = identifier
        {
            Match(tSymmetric, CodeGenerationSupporter.Symmetric);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        authorizationOpt[vResult]
        (
            (
                With keySpecList[vResult]
                encryptorListWithNoPassword[vResult]
            )
        |
            (
                From tProvider:Identifier vProvider = identifier providerKeySourceOptionsOpt[vResult.KeyOptions, vResult]
                {
                    Match(tProvider, CodeGenerationSupporter.Provider);
                    vResult.Provider = vProvider;
                }
                (   // Greedy due to conflict with identifierStatements
                    options {greedy=true;} :                 
                    encryptorListWithNoPassword[vResult]
                )?
            )
        )
    ;

keySpecList [CreateSymmetricKeyStatement vParent]
{
    KeyOption vOption;
    bool keySourceSpecified = false;
    bool algorithmSpecified = false;
    bool identityValueSpecified = false;
}
    :   vOption = keySpec[ref keySourceSpecified, ref algorithmSpecified, ref identityValueSpecified]
        {
            AddAndUpdateTokenInfo(vParent, vParent.KeyOptions, vOption);
        }
        (Comma vOption = keySpec[ref keySourceSpecified, ref algorithmSpecified, ref identityValueSpecified]
            {
                AddAndUpdateTokenInfo(vParent, vParent.KeyOptions, vOption);
            }
        )* 
    ;

keySpec [ref bool keySourceSpecified, ref bool algorithmSpecified, ref bool identityValueSpecified] returns [KeyOption vResult]
    : 
        {NextTokenMatches(CodeGenerationSupporter.KeySource)}?
        vResult = keySourceKeySpec[ref keySourceSpecified]
    |   
        {NextTokenMatches(CodeGenerationSupporter.Algorithm)}?
        vResult = algorithmKeySpec[ref algorithmSpecified]
    |
        vResult = identityValueKeySpec[ref identityValueSpecified]
    ;
    
keySourceKeySpec [ref bool keySourceSpecified] returns [KeySourceKeyOption vResult = FragmentFactory.CreateFragment<KeySourceKeyOption>()]
{
    Literal vPassPhrase;
}
    :   tKeySource:Identifier EqualsSign vPassPhrase = stringLiteral
        {
            Match(tKeySource, CodeGenerationSupporter.KeySource);
            vResult.OptionKind=KeyOptionKind.KeySource;
            if (keySourceSpecified)
                throw GetUnexpectedTokenErrorException(tKeySource);
            keySourceSpecified = true;
            UpdateTokenInfo(vResult, tKeySource);
            vResult.PassPhrase = vPassPhrase;
        }
    ;
    
identityValueKeySpec [ref bool identityValueSpecified] returns [IdentityValueKeyOption vResult = FragmentFactory.CreateFragment<IdentityValueKeyOption>()]
{
    Literal vIdentityPhrase;
}
    :   tIdentityValue:Identifier EqualsSign vIdentityPhrase = stringLiteral
        {
            Match(tIdentityValue, CodeGenerationSupporter.IdentityValue);
            vResult.OptionKind=KeyOptionKind.IdentityValue;
            if (identityValueSpecified)
                throw GetUnexpectedTokenErrorException(tIdentityValue);
            identityValueSpecified = true;
            UpdateTokenInfo(vResult, tIdentityValue);
            vResult.IdentityPhrase = vIdentityPhrase;
        }
    ;

algorithmKeySpec [ref bool algorithmSpecified] returns [AlgorithmKeyOption vResult = FragmentFactory.CreateFragment<AlgorithmKeyOption>()]
    :   
        tAlgorithm:Identifier EqualsSign tAlgorithmValue:Identifier
        {
            Match(tAlgorithm, CodeGenerationSupporter.Algorithm);
            vResult.OptionKind=KeyOptionKind.Algorithm;
            if (algorithmSpecified)
                throw GetUnexpectedTokenErrorException(tAlgorithm);
            algorithmSpecified = true;
            vResult.Algorithm = EncryptionAlgorithmsHelper.Instance.ParseOption(tAlgorithmValue);
            UpdateTokenInfo(vResult, tAlgorithm);
            UpdateTokenInfo(vResult, tAlgorithmValue);
        }
    ;
    
providerKeyNameSourceOption [ref bool providerKeyNameSpecified] returns [ProviderKeyNameKeyOption vResult = FragmentFactory.CreateFragment<ProviderKeyNameKeyOption>()]
{
    Literal vKeyName;
}
    :   tProviderKeyName:Identifier EqualsSign vKeyName = stringLiteral
        {
            vResult.OptionKind=KeyOptionKind.ProviderKeyName;
            Match(tProviderKeyName, CodeGenerationSupporter.ProviderKeyName);
            if (providerKeyNameSpecified)
                throw GetUnexpectedTokenErrorException(tProviderKeyName);
            providerKeyNameSpecified = true;
            vResult.KeyName = vKeyName;
            UpdateTokenInfo(vResult, tProviderKeyName);
        }
    ;

creationDispositionSourceOption [ref bool creationDispositionSpecified] returns [CreationDispositionKeyOption vResult = FragmentFactory.CreateFragment<CreationDispositionKeyOption>()]
    :   
        tCreationDisposition:Identifier EqualsSign tValue:Identifier
        {
            Match(tCreationDisposition, CodeGenerationSupporter.CreationDisposition);
            vResult.OptionKind=KeyOptionKind.CreationDisposition;
            if (creationDispositionSpecified)
                throw GetUnexpectedTokenErrorException(tCreationDisposition);
            creationDispositionSpecified = true;
            if (TryMatch(tValue, CodeGenerationSupporter.CreateNew))
                vResult.IsCreateNew = true;
            else
            {
                Match(tValue, CodeGenerationSupporter.OpenExisting);
                vResult.IsCreateNew = false;
            }
            UpdateTokenInfo(vResult, tCreationDisposition);
            UpdateTokenInfo(vResult, tValue);
        }
    ;
    
encryptorListWithNoPassword [SymmetricKeyStatement vParent]
{
    CryptoMechanism vCrypto;
}
    : tEncryption:Identifier By vCrypto = cryptoWithNoPasswordOrJustPassword 
        {
            Match(tEncryption,CodeGenerationSupporter.Encryption);
            AddAndUpdateTokenInfo(vParent, vParent.EncryptingMechanisms,vCrypto);
        }
        (Comma vCrypto = cryptoWithNoPasswordOrJustPassword
            {
                AddAndUpdateTokenInfo(vParent, vParent.EncryptingMechanisms,vCrypto);
            }
        )*
    ;
    
cryptoWithNoPasswordOrJustPassword returns [CryptoMechanism vResult = null]
    : vResult = certificateCrypto 
    | vResult = keyCrypto 
    | vResult = passwordCrypto
    ;
    
certificateCrypto returns [CryptoMechanism vResult = FragmentFactory.CreateFragment<CryptoMechanism>()]
{
    Identifier vIdentifier;
}
    : tCertificate:Identifier vIdentifier = identifier
        {
            Match(tCertificate,CodeGenerationSupporter.Certificate);
            vResult.Identifier = vIdentifier;
            vResult.CryptoMechanismType = CryptoMechanismType.Certificate;
        }
    ;
    
keyCrypto returns [CryptoMechanism vResult = FragmentFactory.CreateFragment<CryptoMechanism>()]
{
    Identifier vIdentifier;
}
    : tSymmetricAsymmetric:Identifier Key vIdentifier = identifier
        {
            if (TryMatch(tSymmetricAsymmetric,CodeGenerationSupporter.Symmetric))
                vResult.CryptoMechanismType = CryptoMechanismType.SymmetricKey;
            else
            {
                Match(tSymmetricAsymmetric,CodeGenerationSupporter.Asymmetric);
                vResult.CryptoMechanismType = CryptoMechanismType.AsymmetricKey;
            }
            vResult.Identifier = vIdentifier;
        }
    ;
    
passwordCrypto returns [CryptoMechanism vResult = FragmentFactory.CreateFragment<CryptoMechanism>()]
{
    Literal vLiteral;
}
    : tPassword:Identifier EqualsSign vLiteral = stringLiteral
        {
            Match(tPassword,CodeGenerationSupporter.Password);
            vResult.PasswordOrSignature = vLiteral;
            vResult.CryptoMechanismType = CryptoMechanismType.Password;
        }
    ;
    
providerKeySourceOptionsOpt [IList<KeyOption> optionsList, TSqlFragment vParent]
    :
        (   // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            providerKeySourceOptions[optionsList, vParent]
        )?
    ;
    
providerKeySourceOptions [IList<KeyOption> optionsList, TSqlFragment vParent]
{
    bool algorithmSpecified = false;
    bool providerKeyNameSpecified = false;
    bool creationDispositionSpecified = false;
    KeyOption vOption;
}
    :   With vOption = providerKeySourceOption[ref algorithmSpecified, ref providerKeyNameSpecified, ref creationDispositionSpecified]
        {
            AddAndUpdateTokenInfo(vParent, optionsList, vOption);
        }
        (Comma vOption = providerKeySourceOption[ref algorithmSpecified, ref providerKeyNameSpecified, ref creationDispositionSpecified]
            {
                AddAndUpdateTokenInfo(vParent, optionsList, vOption);
            }
        )*
    ;
    
providerKeySourceOption [ref bool algorithmSpecified, ref bool providerKeyNameSpecified, ref bool creationDispositionSpecified] returns [KeyOption vResult]
    :
        {NextTokenMatches(CodeGenerationSupporter.Algorithm)}?
        vResult = algorithmKeySpec[ref algorithmSpecified]
    |
        {NextTokenMatches(CodeGenerationSupporter.ProviderKeyName)}?
        vResult = providerKeyNameSourceOption[ref providerKeyNameSpecified]
    |
        vResult = creationDispositionSourceOption[ref creationDispositionSpecified]
    ;
    
    
alterSymmetricKeyStatement returns [AlterSymmetricKeyStatement vResult = FragmentFactory.CreateFragment<AlterSymmetricKeyStatement>()]
{
    Identifier vIdentifier;
}
    : tSymmetric:Identifier Key vIdentifier = identifier
    {
        Match(tSymmetric,CodeGenerationSupporter.Symmetric);
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    (Add 
        {
            vResult.IsAdd = true;
        }
    | Drop
        {
            vResult.IsAdd = false;
        }
    ) 
    encryptorListWithNoPassword[vResult]
    ;
    
createSynonymStatement returns [CreateSynonymStatement vResult = this.FragmentFactory.CreateFragment<CreateSynonymStatement>()]
{
    SchemaObjectName vSchemaObjectName;
}
    : tSynonym:Identifier 
        {
            Match(tSynonym, CodeGenerationSupporter.Synonym);
        }
        vSchemaObjectName=schemaObjectThreePartName
        {
           CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Synonym);
           vResult.Name = vSchemaObjectName;
           ThrowPartialAstIfPhaseOne(vResult);
        }
        For
        vSchemaObjectName=schemaObjectFourPartName
        {
            vResult.ForName = vSchemaObjectName;
        }
    ;

createTypeStatement returns [CreateTypeStatement vResult = null]
{
    SchemaObjectName vSchemaObjectName = null;
}
    :
        tType:Identifier vSchemaObjectName=schemaObjectThreePartName
        {
            Match(tType, CodeGenerationSupporter.Type);
            CheckTwoPartNameForSchemaObjectName(vSchemaObjectName, CodeGenerationSupporter.Type);
        }        
        (
            vResult=createTypeUddtStatement
        |
            vResult=createTypeUdtStatement
        |
            vResult=createTypeTableStatement
        )
        {
            vResult.Name = vSchemaObjectName;
        }
    ;
    exception
    catch[PhaseOnePartialAstException exception]
    {
        CreateTypeStatement vStatement = exception.Statement as CreateTypeStatement;
        Debug.Assert(vStatement != null);
        vStatement.Name = vSchemaObjectName;
        throw;
    }

createTypeUddtStatement returns [CreateTypeUddtStatement vResult = this.FragmentFactory.CreateFragment<CreateTypeUddtStatement>()]
{
    DataTypeReference vDataType;
    NullableConstraintDefinition vNullableConstraint;
}
    :
        tFrom:From
        {
            UpdateTokenInfo(vResult,tFrom);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vDataType=scalarDataType
        {
            vResult.DataType = vDataType;
        }
        (
            vNullableConstraint=nullableConstraint
            {
                vResult.NullableConstraint = vNullableConstraint;
            }
        )?
    ;

createTypeUdtStatement returns [CreateTypeUdtStatement vResult = this.FragmentFactory.CreateFragment<CreateTypeUdtStatement>()]
{
    AssemblyName vAssemblyName;
}
    :
        tExternal:External
        {
            UpdateTokenInfo(vResult,tExternal);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vAssemblyName=assemblyName
        {
            vResult.AssemblyName = vAssemblyName;
        }
    ;

createTypeTableStatement returns [CreateTypeTableStatement vResult = FragmentFactory.CreateFragment<CreateTypeTableStatement>()]
{
    TableDefinition vTableDefinition;
}
    : As Table LeftParenthesis
        vTableDefinition = tableDefinition[IndexAffectingStatement.CreateType, null]
        {
            vResult.Definition = vTableDefinition;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        (
            options {greedy = true; } :
            withTypeTableOptions[vResult]
        )?
    ;

withTypeTableOptions[CreateTypeTableStatement vParent]
{
    TableOption vTableOption;
}
    : With LeftParenthesis vTableOption=memoryOptimizedTableOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vTableOption);
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;
    
userLoginOption returns [UserLoginOption vResult = this.FragmentFactory.CreateFragment<UserLoginOption>()]
{
    Identifier vIdentifier;
}
    :
        (
            For
        |
            From
        )
        (
            tAsymmetric:Identifier Key
            {
                Match(tAsymmetric, CodeGenerationSupporter.Asymmetric);
                vResult.UserLoginOptionType = UserLoginOptionType.AsymmetricKey;
            }
        |
            tOption:Identifier
            {
                vResult.UserLoginOptionType = UserLoginOptionHelper.Instance.ParseOption(tOption);
            }
        )
        vIdentifier=identifier
        {
            vResult.Identifier = vIdentifier;
        }
    |
        tWithout:Identifier tLogin:Identifier
        {
            Match(tWithout, CodeGenerationSupporter.Without);
            Match(tLogin, CodeGenerationSupporter.Login);
            UpdateTokenInfo(vResult,tLogin);
            vResult.UserLoginOptionType = UserLoginOptionType.WithoutLogin;
        }
    ;

createUserStatement returns [CreateUserStatement vResult = this.FragmentFactory.CreateFragment<CreateUserStatement>()]
{
    Identifier vIdentifier;
    UserLoginOption vUserLoginOption;
    PrincipalOption vPrincipalOption;
    bool vHasUserLoginOption = false;
}
    : 
        User vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            // Greedy due to conflict with identifierStatements, tWithout causes the conflict.
            options {greedy=true;} : vUserLoginOption=userLoginOption
            {
                vResult.UserLoginOption = vUserLoginOption;
                vHasUserLoginOption = true;
            }
        )?
        (
            options {greedy = true; } :
            tWith:With vPrincipalOption=createUserOption[vHasUserLoginOption]
            {
                AddAndUpdateTokenInfo(vResult, vResult.UserOptions, vPrincipalOption);
            }
            (
                Comma vPrincipalOption=createUserOption[vHasUserLoginOption]
                {
                    AddAndUpdateTokenInfo(vResult, vResult.UserOptions, vPrincipalOption);
                }
            )*
        )?
    ;

createUserOption[bool vHasUserLoginOption] returns [PrincipalOption vResult]
    :  tOption:Identifier EqualsSign
    (
           vResult=identifierCreateUserOption[tOption, vHasUserLoginOption]
        |  vResult=literalCreateUserOption[tOption, vHasUserLoginOption]
    )
    {
        if(vHasUserLoginOption && vResult.OptionKind != PrincipalOptionKind.DefaultSchema)
        {
            ThrowParseErrorException("SQL46096", tOption, TSqlParserResource.SQL46096Message, tOption.getText());    
        }
    }
    ;

identifierCreateUserOption[IToken tOption, bool vHasUserLoginOption] returns [IdentifierPrincipalOption vResult=FragmentFactory.CreateFragment<IdentifierPrincipalOption>()]
{
    Identifier vIdentifier;
}
    :   vIdentifier=identifier
        {        
            if(TryMatch(tOption, CodeGenerationSupporter.DefaultSchema))
            {
                vResult.OptionKind = PrincipalOptionKind.DefaultSchema;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.DefaultLanguage);
                vResult.OptionKind=PrincipalOptionKind.DefaultLanguage;
            }
            vResult.Identifier=vIdentifier;
        }    
    ;

literalCreateUserOption[IToken tOption, bool vHasUserLoginOption] returns [LiteralPrincipalOption vResult=FragmentFactory.CreateFragment<LiteralPrincipalOption>()]
{
    Literal vLiteral;
    UpdateTokenInfo(vResult, tOption);
}
    :   vLiteral=binary
        {
            Match(tOption, CodeGenerationSupporter.Sid);
            vResult.OptionKind = PrincipalOptionKind.Sid;
            vResult.Value=vLiteral;
        }
    |    vLiteral=stringLiteral
        {
            Match(tOption, CodeGenerationSupporter.Password);
            vResult.OptionKind=PrincipalOptionKind.Password;
            vResult.Value=vLiteral;
        }
    |    vLiteral=integer
        {
            Match(tOption, CodeGenerationSupporter.DefaultLanguage);
            vResult.OptionKind=PrincipalOptionKind.DefaultLanguage;
            vResult.Value=vLiteral;
        }
    |    vLiteral=nullLiteral
        {
            Match(tOption, CodeGenerationSupporter.DefaultSchema);
            vResult.OptionKind=PrincipalOptionKind.DefaultSchema;
            vResult.Value=vLiteral;
        }
    ;

alter2005Statements returns [TSqlStatement vResult]
    :
        tAlter:Alter
        (
            {NextTokenMatches(CodeGenerationSupporter.Application)}?
            vResult=alterApplicationRoleStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Assembly)}?
            vResult=alterAssemblyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Asymmetric)}?
            vResult=alterAsymmetricKeyStatement
        |
            vResult=alterAuthorizationStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Availability)}?
            vResult=alterAvailabilityGroupStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Broker)}?
            vResult=alterBrokerPriorityStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Cryptographic)}?
            vResult=alterCryptographicProviderStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Event)}?
            vResult=alterEventSessionStatement            
        |
            {NextTokenMatches(CodeGenerationSupporter.Remote)}?
            vResult=alterRemoteServiceBindingStatement           
        |
            {NextTokenMatches(CodeGenerationSupporter.Resource)}?
            vResult=alterResourceStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Workload)}?
            vResult=alterWorkloadGroupStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Certificate)}?
            vResult=alterCertificateStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Credential)}?
            vResult=alterCredentialStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Endpoint)}?
            vResult=alterEndpointStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Fulltext)}?
            vResult=alterFulltextStatement // Index or CATALOG
        | 
            vResult = alterIndexStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Login)}?
            vResult=alterLoginStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Master)}?
            vResult=alterMasterKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Message)}?
            vResult=alterMessageTypeStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Partition)}?
            vResult=alterPartitionStatement // SCHEME or Function
        |
            {NextTokenMatches(CodeGenerationSupporter.Queue)}?
            vResult=alterQueueStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Role)}?
            vResult=alterRoleStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Route)}?
            vResult=alterRouteStatement
        | 
            {NextTokenMatches(CodeGenerationSupporter.Search)}?
            vResult=alterSearchPropertyListStatement
        | 
            vResult = alterSchemaStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Sequence)}?
            vResult=alterSequenceStatement 
        |
            {NextTokenMatches(CodeGenerationSupporter.Service)}?
            vResult=alterServiceStatements // SERVICE or SERVICE MASTER Key
        |
            {NextTokenMatches(CodeGenerationSupporter.Symmetric)}?
            vResult=alterSymmetricKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Server)}?
            vResult=alterServerStatements
        |
            vResult = alterUserStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Xml)}?
            vResult=alterXmlSchemaCollectionStatement
        )
        {
            UpdateTokenInfo(vResult,tAlter);
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;

applicationRoleOption[bool defaultSchemaAndPasswordOnly] returns [ApplicationRoleOption vResult = this.FragmentFactory.CreateFragment<ApplicationRoleOption>()]
{
    Literal vLiteral;
    Identifier vIdentifier;
}
    :
        {NextTokenMatches(CodeGenerationSupporter.Password)}?
        Identifier EqualsSign vLiteral=stringLiteral
        {
            vResult.OptionKind = ApplicationRoleOptionKind.Password;
            vResult.Value = IdentifierOrValueExpression(vLiteral);
        }
    |
        tOption:Identifier EqualsSign vIdentifier=identifier
        {
            vResult.OptionKind = ApplicationRoleOptionHelper.Instance.ParseOption(tOption);
            if (defaultSchemaAndPasswordOnly && vResult.OptionKind != ApplicationRoleOptionKind.DefaultSchema)
            {
                throw GetUnexpectedTokenErrorException(tOption);
            }
            vResult.Value = IdentifierOrValueExpression(vIdentifier);
        }
    ;

applicationRoleStatement[ApplicationRoleStatement vParent, bool defaultSchemaAndPasswordOnly]
{
    Identifier vIdentifier;
    ApplicationRoleOption vApplicationRoleOption;
}
    : tApplication:Identifier tRole:Identifier vIdentifier=identifier
        {
            Match(tApplication, CodeGenerationSupporter.Application);
            Match(tRole, CodeGenerationSupporter.Role);
            vParent.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vParent);
        }
        With vApplicationRoleOption=applicationRoleOption[defaultSchemaAndPasswordOnly]
        {
            AddAndUpdateTokenInfo(vParent, vParent.ApplicationRoleOptions,vApplicationRoleOption);
        }
        (
            Comma vApplicationRoleOption=applicationRoleOption[defaultSchemaAndPasswordOnly]
            {
                AddAndUpdateTokenInfo(vParent, vParent.ApplicationRoleOptions,vApplicationRoleOption);
            }
        )*
    ;

alterApplicationRoleStatement returns [AlterApplicationRoleStatement vResult = this.FragmentFactory.CreateFragment<AlterApplicationRoleStatement>()]
    : 
        applicationRoleStatement[vResult, false]
    ;

alterAssemblyStatement returns [AlterAssemblyStatement vResult = FragmentFactory.CreateFragment<AlterAssemblyStatement>()]
{
    Identifier vIdentifier;
    ScalarExpression vFrom;
    bool wereOptions = false;
}
    : tAssembly:Identifier vIdentifier=identifier
        {
            Match(tAssembly, CodeGenerationSupporter.Assembly);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (vFrom = alterAssemblyFromClause
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters,vFrom);
                wereOptions = true;
            }
        )?
        ( options {greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            alterAssemblyWith[vResult]
            {
                wereOptions = true;
            }
        )?
        ((Drop File) => alterAssemblyDropFile[vResult]
            {
                wereOptions = true;
            }
        )?
        (
            // Greedy due to linear approximation introduced after the rule securityStatementPermission
            options {greedy = true; } :
            alterAssemblyAddFile[vResult]
            {
                wereOptions = true;
            }
        )?
        {
            if (!wereOptions)
                throw GetUnexpectedTokenErrorException(vIdentifier);
        }
    ;

alterAssemblyFromClause returns [ScalarExpression vResult]
    : From vResult = expression
    ;
    
alterAssemblyWith [AlterAssemblyStatement vParent]
{
    AssemblyOption vOption;
    long encounteredOptions = 0;
}
    : With vOption=alterAssemblyWithItem
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        } 
        (
            Comma vOption=alterAssemblyWithItem
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options,    vOption);
            }
        )*
    ;
    
alterAssemblyWithItem returns [AssemblyOption vResult]
    :
        vResult=assemblyPermissionSetOrVisibilityOption
        | vResult = assemblyUncheckedDataOption
    ;

assemblyPermissionSetOrVisibilityOption returns [AssemblyOption vResult]
    : tPermissionSetVisibility:Identifier EqualsSign
    (
        vResult=assemblyPermissionSetOption[tPermissionSetVisibility]
        | vResult=assemblyVisibilityOption[tPermissionSetVisibility]
    )
    ;

assemblyPermissionSetOption[IToken tPermissionSetVisibility] returns [PermissionSetAssemblyOption vResult=FragmentFactory.CreateFragment<PermissionSetAssemblyOption>()]
    : 
        tOption:Identifier
            {
                Match(tPermissionSetVisibility,CodeGenerationSupporter.PermissionSet);
                UpdateTokenInfo(vResult,tPermissionSetVisibility);
                vResult.OptionKind = AssemblyOptionKind.PermissionSet;
                UpdateTokenInfo(vResult,tOption);
                vResult.PermissionSetOption = PermissionSetOptionHelper.Instance.ParseOption(tOption);
            }
        ;

assemblyVisibilityOption[IToken tPermissionSetVisibility] returns [OnOffAssemblyOption vResult=FragmentFactory.CreateFragment<OnOffAssemblyOption>()]
{
    OptionState vVisibility;
}
    :  vVisibility = optionOnOff[vResult]
       {
            Match(tPermissionSetVisibility,CodeGenerationSupporter.Visibility);
            UpdateTokenInfo(vResult,tPermissionSetVisibility);            
            vResult.OptionKind = AssemblyOptionKind.Visibility;
            vResult.OptionState = vVisibility;
        }
    ;

assemblyUncheckedDataOption returns [AssemblyOption vResult = FragmentFactory.CreateFragment<AssemblyOption>()]
    : tUnchecked:Identifier tData:Identifier
        {
            Match(tUnchecked,CodeGenerationSupporter.Unchecked);
            Match(tData,CodeGenerationSupporter.Data);
            UpdateTokenInfo(vResult,tUnchecked);
            vResult.OptionKind=AssemblyOptionKind.UncheckedData;
            UpdateTokenInfo(vResult,tData);
        }
    ;
        
alterAssemblyDropFile [AlterAssemblyStatement vParent]
{
    Literal vFile;
}
    : Drop File 
        (
            tAll:All
            {
                UpdateTokenInfo(vParent,tAll);
                vParent.IsDropAll = true;
            }
        |
            vFile = stringLiteral
            {
                AddAndUpdateTokenInfo(vParent, vParent.DropFiles,vFile);
            }
            (Comma vFile = stringLiteral
                {
                    AddAndUpdateTokenInfo(vParent, vParent.DropFiles,vFile);
                }
            )*
        )
    ;
    
alterAssemblyAddFile [AlterAssemblyStatement vParent]
{
    AddFileSpec vFileSpec;
}
    : Add File From vFileSpec = alterAssemblyAddFileSpec 
        {
            AddAndUpdateTokenInfo(vParent, vParent.AddFiles,vFileSpec);
        }
        (Comma vFileSpec = alterAssemblyAddFileSpec
            {
                AddAndUpdateTokenInfo(vParent, vParent.AddFiles,vFileSpec);
            }
        )*
    ;
    
alterAssemblyAddFileSpec returns [AddFileSpec vResult = FragmentFactory.CreateFragment<AddFileSpec>()]
{
    ScalarExpression vFile;
    Literal vFileName;
}
    : vFile = expression 
        {
            vResult.File = vFile;
        }
        (As vFileName = stringLiteral
            {
                vResult.FileName = vFileName;
            }
        )?
    ;
        
alterAsymmetricKeyStatement returns [AlterAsymmetricKeyStatement vResult = FragmentFactory.CreateFragment<AlterAsymmetricKeyStatement>()]
{
    Identifier vIdentifier;
    Literal vLiteral;
}
    : tAsymmetric:Identifier Key vIdentifier = identifier
    {
        Match(tAsymmetric, CodeGenerationSupporter.Asymmetric);
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    (
        tRemove:Identifier 
        {
            Match(tRemove,CodeGenerationSupporter.Remove);
        }
            (
                tPrivate:Identifier tKey:Key
                {
                    Match(tPrivate,CodeGenerationSupporter.Private);
                    vResult.Kind = AlterCertificateStatementKind.RemovePrivateKey;
                    UpdateTokenInfo(vResult,tKey);
                }
            |
                tAttested2:Identifier tOption:Option
                {
                    Match(tAttested2,CodeGenerationSupporter.Attested);
                    vResult.Kind = AlterCertificateStatementKind.RemoveAttestedOption;
                    UpdateTokenInfo(vResult,tOption);
                }
            )
    |
        With tPrivate2:Identifier Key 
        {
            Match(tPrivate2,CodeGenerationSupporter.Private);
            vResult.Kind = AlterCertificateStatementKind.WithPrivateKey;
        }
        LeftParenthesis passwordChangeOption[vResult] (Comma passwordChangeOption[vResult])* 
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    |
        tAttested:Identifier By vLiteral = stringLiteral
        {
            vResult.AttestedBy = vLiteral;
            vResult.Kind = AlterCertificateStatementKind.AttestedBy;
        }
    )
    ;
        
alterRemoteServiceBindingStatement returns [AlterRemoteServiceBindingStatement vResult = FragmentFactory.CreateFragment<AlterRemoteServiceBindingStatement>()]
{
    Identifier vIdentifier;
    RemoteServiceBindingOption vOption;
}
    : tRemote:Identifier tService:Identifier tBinding:Identifier vIdentifier = identifier
    {
        Match(tRemote, CodeGenerationSupporter.Remote);
        Match(tService, CodeGenerationSupporter.Service);
        Match(tBinding, CodeGenerationSupporter.Binding);
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    With
    (
        (
            vOption=bindingUserOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
            } 
            (
                Comma vOption=bindingAnonymousOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                }
            )?
        )
    |
        (
            vOption=bindingAnonymousOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
            } 
            (
                Comma vOption=bindingUserOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options,vOption);
                }
            )?
        )
    )
    ;

alterCertificateStatement returns [AlterCertificateStatement vResult = FragmentFactory.CreateFragment<AlterCertificateStatement>()]
{
    Identifier vIdentifier;
    Literal vAttestedBy;
}
    : tCertificate:Identifier vIdentifier = identifier
    {
        Match(tCertificate,CodeGenerationSupporter.Certificate);
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    ( tRemove:Identifier 
        {
            Match(tRemove,CodeGenerationSupporter.Remove);
        }
        (
            tPrivate:Identifier tKey:Key
            {
                Match(tPrivate,CodeGenerationSupporter.Private);
                vResult.Kind = AlterCertificateStatementKind.RemovePrivateKey;
                UpdateTokenInfo(vResult,tKey);
            }
        |
            tAttested:Identifier tOption:Option
            {
                Match(tAttested,CodeGenerationSupporter.Attested);
                vResult.Kind = AlterCertificateStatementKind.RemoveAttestedOption;
                UpdateTokenInfo(vResult,tOption);
            }
        )            
    | With 
        (
            privateKeySpec[vResult]
            {
                vResult.Kind = AlterCertificateStatementKind.WithPrivateKey;
            }
        |
            createCertificateActivityFlag[vResult]
            {
                vResult.Kind = AlterCertificateStatementKind.WithActiveForBeginDialog;
            }
        )
    | tAttested2:Identifier By vAttestedBy = stringLiteral
        {
            Match(tAttested2,CodeGenerationSupporter.Attested);
            vResult.Kind = AlterCertificateStatementKind.AttestedBy;
            vResult.AttestedBy = vAttestedBy;
        }
    )
    ;

alterCredentialStatement returns [AlterCredentialStatement vResult = FragmentFactory.CreateFragment<AlterCredentialStatement>()]
    : credentialStatementBody[vResult]
    ;

alterEndpointStatement returns [AlterEndpointStatement vResult = FragmentFactory.CreateFragment<AlterEndpointStatement>()]
{
    Identifier vIdentifier;
}
    : tEndpoint:Identifier vIdentifier = identifier
        {
            Match(tEndpoint,CodeGenerationSupporter.Endpoint);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (endpointOptions[vResult])? (protocolInfo[vResult])? (payloadInfo[vResult])?
    ;

alterFulltextStatement returns [TSqlStatement vResult]
    : tFulltext:Identifier
        {
            Match(tFulltext,CodeGenerationSupporter.Fulltext);
        }
    ( 
        vResult = alterFulltextCatalogStatement
    | 
        vResult = alterFulltextIndexStatement
    |
        vResult = alterFulltextStoplistStatement
    )
    ;
    
alterFulltextCatalogStatement returns [AlterFullTextCatalogStatement vResult = this.FragmentFactory.CreateFragment<AlterFullTextCatalogStatement>()]
{
    Identifier vIdentifier;
    FullTextCatalogOption vOption;
}
    : tCatalog:Identifier vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            (
                tAction:Identifier
                    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                (
                    options {greedy = true; } : 
                    With vOption=accentSensitivity
                    {
                        AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                        Match(tAction,CodeGenerationSupporter.Rebuild);
                        vResult.Action = AlterFullTextCatalogAction.Rebuild;
                    }
                )?
                {
                    // There was no accentSensitivity - so, we need to handle tAction...
                    if (vResult.Action == AlterFullTextCatalogAction.None) 
                    {
                        if (TryMatch(tAction,CodeGenerationSupporter.Reorganize))
                            vResult.Action = AlterFullTextCatalogAction.Reorganize;
                        else
                        {
                            Match(tAction,CodeGenerationSupporter.Rebuild);
                            vResult.Action = AlterFullTextCatalogAction.Rebuild;
                        }
                    }
                    UpdateTokenInfo(vResult,tAction);
                }
            )
        |
            As tDefault:Default
            {
                UpdateTokenInfo(vResult,tDefault);
                vResult.Action = AlterFullTextCatalogAction.AsDefault;
            }
        )
    ;
    
alterFulltextIndexStatement returns [AlterFullTextIndexStatement vResult = FragmentFactory.CreateFragment<AlterFullTextIndexStatement>()]
{
    SchemaObjectName vObjectName;
    AlterFullTextIndexAction vAction;
}
    : Index On vObjectName = schemaObjectThreePartName
        {
            vResult.OnName = vObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vAction = alterFulltextIndexAction
        {
            vResult.Action = vAction;
        }
    ;
    
alterFulltextIndexAction returns [AlterFullTextIndexAction vResult]
    : {NextTokenMatches(CodeGenerationSupporter.Enable) || NextTokenMatches(CodeGenerationSupporter.Disable)}?
      vResult = enableDisableAlterFulltextIndexAction
    | vResult = addAlterFulltextIndexAction
    | vResult = dropAlterFulltextIndexAction
    | {NextTokenMatches(CodeGenerationSupporter.Start)}?
      vResult = startPopulationAlterFulltextIndexAction
    | vResult = otherPopulationAlterFulltextIndexAction
    | vResult = setAlterFullTextIndexAction    
    | vResult = alterColumnAlterFullTextIndexAction
    ;

setAlterFullTextIndexAction returns [AlterFullTextIndexAction vResult]
    :
      Set
     (
         vResult = setStoplistAlterFulltextIndexAction
       | {NextTokenMatches(CodeGenerationSupporter.ChangeTracking)}?
         vResult = setChangeTrackingAlterFulltextIndexAction
       | {NextTokenMatches(CodeGenerationSupporter.Search)}?
         vResult = setSearchPropertyListAlterFullTextIndexAction
      )
    ;

setStoplistAlterFulltextIndexAction returns [SetStopListAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SetStopListAlterFullTextIndexAction>()]
{
    StopListFullTextIndexOption vOption;
    bool vWithNoPopulation;
}
    :   vOption = stoplistFulltextIndexOption
        {
            vResult.StopListOption = vOption;
        }
        vWithNoPopulation = populationOption[vResult]
        {
            vResult.WithNoPopulation = vWithNoPopulation;
        }
    ;

setSearchPropertyListAlterFullTextIndexAction returns [SetSearchPropertyListAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SetSearchPropertyListAlterFullTextIndexAction>()]
{
    SearchPropertyListFullTextIndexOption vOption;
    bool vWithNoPopulation;
}
    :   vOption = searchPropertyListFullTextIndexOption
        {
            vResult.SearchPropertyListOption = vOption;
        }
        vWithNoPopulation = populationOption[vResult]
        {
            vResult.WithNoPopulation = vWithNoPopulation;
        }
    ;
    
enableDisableAlterFulltextIndexAction returns [SimpleAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SimpleAlterFullTextIndexAction>()]
    : tEnableDisable:Identifier
        {
            vResult.ActionKind = EnableDisableMatcher(tEnableDisable, 
                        SimpleAlterFullTextIndexActionKind.Enable, SimpleAlterFullTextIndexActionKind.Disable);
            UpdateTokenInfo(vResult,tEnableDisable);
        }
    ;

setChangeTrackingAlterFulltextIndexAction returns [SimpleAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SimpleAlterFullTextIndexAction>()]
    :   tChangeTracking:Identifier (EqualsSign)?
        {
            Match(tChangeTracking, CodeGenerationSupporter.ChangeTracking);
        }
        (
            tManualAuto:Identifier
            {
                if (TryMatch(tManualAuto,CodeGenerationSupporter.Manual))
                    vResult.ActionKind = SimpleAlterFullTextIndexActionKind.SetChangeTrackingManual;
                else
                {
                    Match(tManualAuto,CodeGenerationSupporter.Auto);
                    vResult.ActionKind = SimpleAlterFullTextIndexActionKind.SetChangeTrackingAuto;
                }
                UpdateTokenInfo(vResult,tManualAuto);
            }
        |
            tOff:Off
            {
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.SetChangeTrackingOff;
                UpdateTokenInfo(vResult,tOff);
            }
        )
    ;

addAlterFulltextIndexAction returns [AddAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<AddAlterFullTextIndexAction>()]
{
    FullTextIndexColumn vColumn;
    bool vWithNoPopulation;
}
    : Add LeftParenthesis vColumn = fulltextIndexColumn
        {
            AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
        }
        (Comma vColumn = fulltextIndexColumn
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
        )* 
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        vWithNoPopulation = populationOption[vResult]
            {
                vResult.WithNoPopulation = vWithNoPopulation;
            }
    ;

populationOption [TSqlFragment vParent] returns [bool vWithNoPopulation = false]
    :   
        (   // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options { greedy = true; } : 
            With tNo:Identifier tPopulation:Identifier
            {
                Match(tNo,CodeGenerationSupporter.No);
                Match(tPopulation,CodeGenerationSupporter.Population);
                UpdateTokenInfo(vParent,tPopulation);
                vWithNoPopulation = true;
            }
      )?
    ;

dropAlterFulltextIndexAction returns [DropAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<DropAlterFullTextIndexAction>()]
{
    Identifier vColumnName;
    bool vWithNoPopulation;
}
    : Drop LeftParenthesis vColumnName = identifier 
        {
            AddAndUpdateTokenInfo(vResult, vResult.Columns,vColumnName);
        }
        (Comma vColumnName = identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Columns,vColumnName);
            }
        )* 
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        vWithNoPopulation = populationOption[vResult]
            {
                vResult.WithNoPopulation = vWithNoPopulation;
            }
    ;

startPopulationAlterFulltextIndexAction returns [SimpleAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SimpleAlterFullTextIndexAction>()]
    : tStart:Identifier 
        {
            Match(tStart,CodeGenerationSupporter.Start);
        }
        (   tIncremental:Identifier 
            {
                Match(tIncremental,CodeGenerationSupporter.Incremental);
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.StartIncrementalPopulation;
            }
        |    Update
            {
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.StartUpdatePopulation;
            }
        |   Full
            {
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.StartFullPopulation;
            }
        ) 
        tPopulation:Identifier
        {
            Match(tPopulation,CodeGenerationSupporter.Population);
            UpdateTokenInfo(vResult,tPopulation);
        }
    ;

otherPopulationAlterFulltextIndexAction returns [SimpleAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<SimpleAlterFullTextIndexAction>()]
    : tStopPauseResume:Identifier tPopulation:Identifier
        {
            if (TryMatch(tStopPauseResume,CodeGenerationSupporter.Stop))
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.StopPopulation;
            else if (TryMatch(tStopPauseResume,CodeGenerationSupporter.Pause))
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.PausePopulation;
            else
            {
                Match(tStopPauseResume,CodeGenerationSupporter.Resume);
                vResult.ActionKind = SimpleAlterFullTextIndexActionKind.ResumePopulation;
            }
            Match(tPopulation,CodeGenerationSupporter.Population);
            UpdateTokenInfo(vResult,tPopulation);
        }
    ;

alterColumnAlterFullTextIndexAction returns [AlterColumnAlterFullTextIndexAction vResult = FragmentFactory.CreateFragment<AlterColumnAlterFullTextIndexAction>()]
{
    FullTextIndexColumn vColumn;
    bool vWithNoPopulation;
}
    : tAlter:Alter Column 
        {
            UpdateTokenInfo(vResult, tAlter);
        }
        vColumn = alterFullTextIndexColumn
        {
            vResult.Column = vColumn;
        }
        vWithNoPopulation = populationOption[vResult]
        {
            vResult.WithNoPopulation = vWithNoPopulation;
        }
    ;

alterFullTextIndexColumn returns [FullTextIndexColumn vResult = FragmentFactory.CreateFragment<FullTextIndexColumn>()]
{
    Identifier vColumnName;
}    : 
        vColumnName=identifier
        {
            vResult.Name=vColumnName;
        }
        (
            tAdd:Add
        |    
            tDrop:Drop
        )
        tIdentifier:Identifier
        {
            Match(tIdentifier, CodeGenerationSupporter.StatisticalSemantics);
            if(tAdd != null)
            {
                vResult.StatisticalSemantics=true;
            }
            UpdateTokenInfo(vResult, tIdentifier);
        }
    ;

alterIndexStatement returns [AlterIndexStatement vResult = this.FragmentFactory.CreateFragment<AlterIndexStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vObjectName;
    PartitionSpecifier vPartition;
    IndexAffectingStatement statementKind = IndexAffectingStatement.AlterIndexSet;
    SelectiveXmlIndexPromotedPath vSelectiveXmlIndexPath;
    XmlNamespaces vXmlNamespaces;
}
    : Index 
        (
            vIdentifier=identifier
            {
                vResult.Name = vIdentifier;
            }
        | 
            All
            {
                vResult.All = true;
            }
        ) 
        On vObjectName=schemaObjectFourPartName
        {
            vResult.OnName = vObjectName;
        }
        (
            Set indexOptionList[statementKind, vResult.IndexOptions, vResult]
            {
                vResult.AlterIndexType = AlterIndexType.Set;
            }
        |
            {NextTokenMatches(CodeGenerationSupporter.Disable)}?
            tDisable:Identifier
            {
                vResult.AlterIndexType = AlterIndexType.Disable;
                UpdateTokenInfo(vResult,tDisable);
            }
        |
            tOption:Identifier /* Can be either Rebuild or Reorganize */
            {
                vResult.AlterIndexType = AlterIndexTypeHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
                UpdateTokenInfo(vResult,tOption);
            }
            (
                vPartition = partitionSpecifier
                {
                    vResult.Partition = vPartition;
                }
            )?
            {
                statementKind = GetAlterIndexStatementKind(vResult);
            }
            (
                // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } :
                With indexOptionList[statementKind, vResult.IndexOptions, vResult]
                {
                    if (vResult.AlterIndexType == AlterIndexType.Rebuild)
                        CheckPartitionAllSpecifiedForIndexRebuild(vResult.Partition, vResult.IndexOptions);
                }
            )?
        |
            (
                tWith:With
                {
                    UpdateTokenInfo(vResult,tWith);
                }
                vXmlNamespaces=xmlNamespaces
                {
                    vResult.XmlNamespaces = vXmlNamespaces;
                }
            )?
            For LeftParenthesis vSelectiveXmlIndexPath = promotedSelectiveXmlIndexPathInAlter
            {
                vResult.AlterIndexType = AlterIndexType.UpdateSelectiveXmlPaths;
                AddAndUpdateTokenInfo(vResult, vResult.PromotedPaths, vSelectiveXmlIndexPath);
            }
                (
                Comma vSelectiveXmlIndexPath = promotedSelectiveXmlIndexPathInAlter 
                {
                    AddAndUpdateTokenInfo(vResult, vResult.PromotedPaths, vSelectiveXmlIndexPath);
                }
            )*
            tRParen2:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen2);
            }
        )
    ;
    
partitionSpecifier returns [PartitionSpecifier vResult = FragmentFactory.CreateFragment<PartitionSpecifier>()]
{
    ScalarExpression vExpression;
}
    :    tPartition:Identifier EqualsSign 
        {
            Match(tPartition, CodeGenerationSupporter.Partition);
        }
        (
            vExpression = expression
            {
                vResult.Number = vExpression;
            }
        |
            tAll:All
            {
                vResult.All = true;
                UpdateTokenInfo(vResult, tAll);
            }
        )
    ;

alterLoginStatement returns [AlterLoginStatement vResult]
{
    Identifier vName;
}
    : tLogin:Identifier vName = identifier
        {
            Match(tLogin,CodeGenerationSupporter.Login);
        }
        ( 
            vResult = alterLoginEnableDisable[vName]
        |
            vResult = alterLoginOptions[vName]
        |
            vResult = alterLoginAddDropCredential[vName]
        )
    ;
    
alterLoginEnableDisable [Identifier vName] returns [AlterLoginEnableDisableStatement vResult = FragmentFactory.CreateFragment<AlterLoginEnableDisableStatement>()]
    :
        tEnableDisable:Identifier
        {
            vResult.Name = vName;
            vResult.IsEnable = EnableDisableMatcher(tEnableDisable, true, false);
            UpdateTokenInfo(vResult,tEnableDisable);
            ThrowPartialAstIfPhaseOne(vResult);
        }
    ;
    
alterLoginOptions [Identifier vName] returns [AlterLoginOptionsStatement vResult = FragmentFactory.CreateFragment<AlterLoginOptionsStatement>()]
{
    PrincipalOption vParam;
    long encounteredOptions = 0;
}
    :   With 
        {
            vResult.Name = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vParam = alterLoginParam
        {
            AddAndUpdateTokenInfo(vResult, vResult.Options,vParam);
            CheckOptionDuplication(ref encounteredOptions, (int)vParam.OptionKind, vParam);
        }
        (Comma vParam = alterLoginParam
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options,vParam);
                CheckOptionDuplication(ref encounteredOptions, (int)vParam.OptionKind, vParam);
            }
        )*
    ;    
    
alterLoginAddDropCredential[Identifier vName] returns [AlterLoginAddDropCredentialStatement vResult = FragmentFactory.CreateFragment<AlterLoginAddDropCredentialStatement>()]
{
    Identifier vCredentialName;
}
    :
        (
            Add 
            {
                vResult.IsAdd = true;   
            }
        |
            Drop
            {
                vResult.IsAdd = false;
            }
        )
        {
            vResult.Name = vName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        tCredential:Identifier vCredentialName = identifier
        {
            Match(tCredential, CodeGenerationSupporter.Credential);
            vResult.CredentialName = vCredentialName;
        }
    ;
    
alterLoginParam returns [PrincipalOption vResult  = null]
{
    Identifier vValue;
}
    : 
        (
            (tOption:Identifier EqualsSign 
                ( vResult = passwordAlterLoginOption
                    {
                        Match(tOption,CodeGenerationSupporter.Password);
                    }
                | vResult = onOffPrincipalOption[tOption]
                | vValue = identifier
                    {
                        IdentifierPrincipalOption vIdentifierLoginOption = FragmentFactory.CreateFragment<IdentifierPrincipalOption>();
                        if (TryMatch(tOption,CodeGenerationSupporter.Name))
                            vIdentifierLoginOption.OptionKind = PrincipalOptionKind.Name;
                        else
                            vIdentifierLoginOption.OptionKind = IdentifierCreateLoginOptionsHelper.Instance.ParseOption(tOption);
                        vIdentifierLoginOption.Identifier = vValue;
                        vResult = vIdentifierLoginOption;
                    }
                )
            )
            {
                UpdateTokenInfo(vResult, tOption);
            }
        | tNo:Identifier tCredential:Identifier
            {
                Match(tNo,CodeGenerationSupporter.No);
                Match(tCredential,CodeGenerationSupporter.Credential);
                PrincipalOption vIdentifierLoginOption = FragmentFactory.CreateFragment<PrincipalOption>();
                vIdentifierLoginOption.OptionKind = PrincipalOptionKind.NoCredential;
                UpdateTokenInfo(vIdentifierLoginOption, tCredential);
                vResult = vIdentifierLoginOption;
            }
        )
    ;
    
passwordAlterLoginOption returns [PasswordAlterPrincipalOption vResult = FragmentFactory.CreateFragment<PasswordAlterPrincipalOption>()]
{
    Literal vPassword, vOldPassword;
}
    : vPassword = loginPassword 
        {
            vResult.Password = vPassword;
            vResult.OptionKind=PrincipalOptionKind.Password;
        }
        (    
            tOption2:Identifier EqualsSign vOldPassword = stringLiteral
            {
                Match(tOption2,CodeGenerationSupporter.OldPassword);
                vResult.OldPassword = vOldPassword;
            }
        |   
            (options {greedy = true; } : tOption3:Identifier
                {
                    RecognizeAlterLoginSecAdminPasswordOption(tOption3,vResult);
                }
            )*
        )
    ;

alterMasterKeyStatement returns [AlterMasterKeyStatement vResult = FragmentFactory.CreateFragment<AlterMasterKeyStatement>()]
{
    Literal vValue;
}
    : tMaster0:Identifier Key
    {
        Match(tMaster0,CodeGenerationSupporter.Master);
    }
    (
        tRegenerate:Identifier With tEncryption:Identifier By tPassword:Identifier EqualsSign vValue = stringLiteral
        {
            Match(tRegenerate, CodeGenerationSupporter.Regenerate);
            Match(tEncryption, CodeGenerationSupporter.Encryption);
            Match(tPassword, CodeGenerationSupporter.Password);
            vResult.Option = AlterMasterKeyOption.Regenerate;
            vResult.Password = vValue;
        }
    |    tForce:Identifier tRegenerate2:Identifier With tEncryption2:Identifier By tPassword2:Identifier EqualsSign vValue = stringLiteral
        {
            Match(tForce, CodeGenerationSupporter.Force);
            Match(tRegenerate2, CodeGenerationSupporter.Regenerate);
            Match(tEncryption2, CodeGenerationSupporter.Encryption);
            Match(tPassword2, CodeGenerationSupporter.Password);
            vResult.Option = AlterMasterKeyOption.ForceRegenerate;
            vResult.Password = vValue;
        }
    |    Add tEncryption3:Identifier By 
        {
            Match(tEncryption3, CodeGenerationSupporter.Encryption);
        }
        (
            (tService:Identifier tMaster:Identifier tKey2:Key
                {
                    Match(tService, CodeGenerationSupporter.Service);
                    Match(tMaster, CodeGenerationSupporter.Master);
                    vResult.Option = AlterMasterKeyOption.AddEncryptionByServiceMasterKey;
                    UpdateTokenInfo(vResult,tKey2);
                }
            ) 
            |
            (tPassword3:Identifier EqualsSign vValue = stringLiteral
                {
                    Match(tPassword3, CodeGenerationSupporter.Password);
                    vResult.Option = AlterMasterKeyOption.AddEncryptionByPassword;
                    vResult.Password = vValue;
                }
            )
        )
    |    Drop tEncryption5:Identifier By 
        {
            Match(tEncryption5, CodeGenerationSupporter.Encryption);
        }
        (
            (tService2:Identifier tMaster2:Identifier tKey:Key
                {
                    Match(tService2, CodeGenerationSupporter.Service);
                    Match(tMaster2, CodeGenerationSupporter.Master);
                    vResult.Option = AlterMasterKeyOption.DropEncryptionByServiceMasterKey;
                    UpdateTokenInfo(vResult,tKey);
                }
            )
            |
            (tPassword4:Identifier EqualsSign vValue = stringLiteral
                {
                    Match(tPassword4, CodeGenerationSupporter.Password);
                    vResult.Option = AlterMasterKeyOption.DropEncryptionByPassword;
                    vResult.Password = vValue;
                }
            )
        )
    )
    ;

alterMessageTypeStatement returns [AlterMessageTypeStatement vResult = FragmentFactory.CreateFragment<AlterMessageTypeStatement>()]
{
    Identifier vIdentifier;
}
    : tMessage:Identifier tType:Identifier vIdentifier = identifier
    {
        Match(tMessage,CodeGenerationSupporter.Message);
        Match(tType,CodeGenerationSupporter.Type);
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    messageTypeValidation[vResult]
    ;

alterFederationStatement returns [AlterFederationStatement vResult = FragmentFactory.CreateFragment<AlterFederationStatement>()]
{
    Identifier vName;
    Identifier vDistributionName;
    ScalarExpression vExpression;
}
    : 
        tAlter:Alter 
        {
            UpdateTokenInfo(vResult,tAlter);
        }
        tFederation:Identifier vName = identifier
        {
            Match(tFederation, CodeGenerationSupporter.Federation);
            vResult.Name = vName;
        }
        (
            tSplit:Identifier tSplitAt:Identifier LeftParenthesis
            {
                Match(tSplit,CodeGenerationSupporter.Split);
                Match(tSplitAt,CodeGenerationSupporter.At);
                vResult.Kind = AlterFederationKind.Split;
            }
        |
            tDrop:Drop tDropAt:Identifier 
            {
                Match(tDropAt,CodeGenerationSupporter.At);
            }
            LeftParenthesis 
            tLowHigh:Identifier
            {
                if (TryMatch(tLowHigh,CodeGenerationSupporter.Low))
                {
                    vResult.Kind = AlterFederationKind.DropLow;
                }
                else
                {
                    Match(tLowHigh,CodeGenerationSupporter.High);
                    vResult.Kind = AlterFederationKind.DropHigh;
                }
            }
        )
        vDistributionName = identifier
        {
            vResult.DistributionName = vDistributionName;
        }
        EqualsSign
        vExpression = expression
        {
            vResult.Boundary = vExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }        
    ;

alterPartitionStatement returns [TSqlStatement vResult]
    : tPartition:Identifier
    {
        Match(tPartition,CodeGenerationSupporter.Partition);
    }
    (
        vResult = alterPartitionFunctionStatement
    |
        vResult = alterPartitionSchemeStatement
    )
    ;
    
alterPartitionFunctionStatement returns [AlterPartitionFunctionStatement vResult = FragmentFactory.CreateFragment<AlterPartitionFunctionStatement>()]
{
    ScalarExpression vExpression;
    Identifier vIdentifier;
}
    : Function vIdentifier = identifier LeftParenthesis RightParenthesis
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
        tSplit:Identifier 
            {
                Match(tSplit,CodeGenerationSupporter.Split);
                vResult.IsSplit = true;
                UpdateTokenInfo(vResult,tSplit);
            }
        | tMerge:Merge
            {
                vResult.IsSplit = false;
                UpdateTokenInfo(vResult,tMerge);
            }
        )
        (    options {greedy = true; } : // Greedy due to conflict with identifierStatements
            tRange:Identifier LeftParenthesis vExpression = expression tRParen:RightParenthesis
            {
                Match(tRange, CodeGenerationSupporter.Range);
                vResult.Boundary = vExpression;
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;
    
alterPartitionSchemeStatement returns [AlterPartitionSchemeStatement vResult = FragmentFactory.CreateFragment<AlterPartitionSchemeStatement>()]
{
    Identifier vIdentifier;
    IdentifierOrValueExpression vFragment;
}
    : tScheme:Identifier vIdentifier = identifier
        {
            Match(tScheme,CodeGenerationSupporter.Scheme);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        tNext:Identifier tUsed:Identifier 
            {
                Match(tNext,CodeGenerationSupporter.Next);
                Match(tUsed,CodeGenerationSupporter.Used);
                UpdateTokenInfo(vResult,tUsed);
            }
        ( options {greedy = true; } : // Greedy due to conflict with identifierStatements
          vFragment = stringOrIdentifier
            {
                vResult.FileGroup = vFragment;
            }
        )?
    ;

executeAsClause[bool vCallerProhibited, bool vOwnerProhibited] returns [ExecuteAsClause vResult = this.FragmentFactory.CreateFragment<ExecuteAsClause>()]
{
    Literal vLiteral;
}
    :
        (
            tExec:Exec
            {
                UpdateTokenInfo(vResult,tExec);
            }
        |
            tExecute:Execute
            {
                UpdateTokenInfo(vResult,tExecute);
            }
        )
        As
        (
            vLiteral=stringLiteral
            {
                vResult.ExecuteAsOption = ExecuteAsOption.String;
                vResult.Literal = vLiteral;
            }
        | 
            tOption:Identifier
            {
                vResult.ExecuteAsOption = ExecuteAsOptionHelper.Instance.ParseOption(tOption);
                if ((vCallerProhibited && vResult.ExecuteAsOption == ExecuteAsOption.Caller)
                    || (vOwnerProhibited && vResult.ExecuteAsOption == ExecuteAsOption.Owner))
                {
                    throw GetUnexpectedTokenErrorException(tOption);
                }
            }
        )
    ;

queueOptionList[QueueStatement vParent, bool vDropAccepted]
    :    
        With queueOption[vParent, vDropAccepted]
        (
            Comma queueOption[vParent, vDropAccepted]
        )*
    ;

queueOption[QueueStatement vParent, bool vDropAccepted]
{
    QueueOption vOption;
}
    :    vOption = stateQueueOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.QueueOptions, vOption);
        }
    |
        {NextTokenMatches(CodeGenerationSupporter.Activation)}?
        queueActivationOption[vParent, vDropAccepted]
    |
        vOption = queuePoisonMessageHandlingOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.QueueOptions, vOption);
        }
    ;

stateQueueOption returns [QueueStateOption vResult = FragmentFactory.CreateFragment<QueueStateOption>()]
{
    OptionState vOptionState;
}
    :    tOption:Identifier EqualsSign vOptionState=optionOnOff[vResult]
        {
            if (TryMatch(tOption, CodeGenerationSupporter.Status))
            {
                vResult.OptionKind = QueueOptionKind.Status;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.Retention);
                vResult.OptionKind = QueueOptionKind.Retention;
            }
            vResult.OptionState = vOptionState;
        }
    ;

queueActivationOption[QueueStatement vParent, bool vDropAccepted] 
{
    bool procedureName = false;
    bool maxQueueReaders = false;
    bool executeAs = false;
    QueueOption vOption;
}
    :    tActivation:Identifier LeftParenthesis
        {
            Match(tActivation, CodeGenerationSupporter.Activation);
        }
        vOption = activationQueueOptionArgument[vDropAccepted, ref procedureName, ref maxQueueReaders, ref executeAs]
        {
            AddAndUpdateTokenInfo(vParent, vParent.QueueOptions, vOption);
        }
        (
            Comma vOption = activationQueueOptionArgument[vDropAccepted, ref procedureName, ref maxQueueReaders, ref executeAs]
            {
                AddAndUpdateTokenInfo(vParent, vParent.QueueOptions, vOption);
            }
        )*        
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
            if (vParent is CreateQueueStatement && (procedureName || maxQueueReaders || executeAs))
            {
                if (!((procedureName) && (maxQueueReaders) && (executeAs)))
                    ThrowParseErrorException("SQL46069", vParent, TSqlParserResource.SQL46069Message);
            }
        }       
    ;

activationQueueOptionArgument[bool vDropAccepted, ref bool procedureName, ref bool maxQueueReaders, ref bool executeAs] returns [QueueOption vResult]
    :
        (tOption:Identifier EqualsSign 
            (
                vResult = statusQueueOptionArgument[tOption]
            |
                vResult = procedureNameQueueOptionArgument[tOption]
                {
                    procedureName = true;
                }
            |
                vResult = maxQueueReadersQueueOptionArgument[tOption]
                {
                    maxQueueReaders = true;
                }
            )
        )
        |
            vResult = dropQueueOptionArgument[vDropAccepted]
        |
            vResult = executeAsQueueOptionArgument
            {
                executeAs = true;
            }
    ;

statusQueueOptionArgument [IToken vStartToken] returns [QueueStateOption vResult = FragmentFactory.CreateFragment<QueueStateOption>()]
{
    OptionState vOptionState;
}
    :    vOptionState=optionOnOff[vResult]
        {
            Match(vStartToken, CodeGenerationSupporter.Status);
            UpdateTokenInfo(vResult,vStartToken);
            vResult.OptionState = vOptionState;
            vResult.OptionKind = QueueOptionKind.ActivationStatus;
        }
    ;

procedureNameQueueOptionArgument [IToken vStartToken] returns [QueueProcedureOption vResult = FragmentFactory.CreateFragment<QueueProcedureOption>()]
{
    SchemaObjectName vName;
}
    :    vName=schemaObjectThreePartName
        {
            Match(vStartToken, CodeGenerationSupporter.ProcedureName);
            UpdateTokenInfo(vResult,vStartToken);
            vResult.OptionValue = vName;
            vResult.OptionKind = QueueOptionKind.ActivationProcedureName;
        }
    ;

maxQueueReadersQueueOptionArgument [IToken vStartToken] returns [QueueValueOption vResult = FragmentFactory.CreateFragment<QueueValueOption>()]
{
    Literal vNumberReaders;
}
    :    vNumberReaders=integer
        {
            Match(vStartToken, CodeGenerationSupporter.MaxQueueReaders);
            UpdateTokenInfo(vResult,vStartToken);
            vResult.OptionValue = vNumberReaders;
            vResult.OptionKind = QueueOptionKind.ActivationMaxQueueReaders;
        }
    ;

dropQueueOptionArgument [bool vDropAccepted] returns [QueueOption vResult = FragmentFactory.CreateFragment<QueueOption>()]
    :
        tDrop:Drop
        {
            if (!vDropAccepted)
                throw GetUnexpectedTokenErrorException(tDrop);
            UpdateTokenInfo(vResult,tDrop);
            vResult.OptionKind = QueueOptionKind.ActivationDrop;
        }
    ;

executeAsQueueOptionArgument returns [QueueExecuteAsOption vResult = FragmentFactory.CreateFragment<QueueExecuteAsOption>()]
{
    ExecuteAsClause vExecuteAsClause;
}
    :    vExecuteAsClause=executeAsClause[true, false]
        {
            vResult.OptionValue = vExecuteAsClause;
            vResult.OptionKind = QueueOptionKind.ActivationExecuteAs;
        }
    ;

queuePoisonMessageHandlingOption returns [QueueStateOption vResult = FragmentFactory.CreateFragment<QueueStateOption>()]
{
    OptionState vOptionState;
}
    :    tPoisonMessageHandling:Identifier LeftParenthesis
        {
            Match(tPoisonMessageHandling, CodeGenerationSupporter.PoisonMessageHandling);
            UpdateTokenInfo(vResult,tPoisonMessageHandling);
        }
        tStatus:Identifier EqualsSign vOptionState=optionOnOff[vResult] tRParen:RightParenthesis
        {
            Match(tStatus, CodeGenerationSupporter.Status);
            vResult.OptionState = vOptionState;
            vResult.OptionKind = QueueOptionKind.PoisonMessageHandlingStatus;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

alterQueueStatement returns [AlterQueueStatement vResult = this.FragmentFactory.CreateFragment<AlterQueueStatement>()]
{
    SchemaObjectName vQueueName;
}
    :     tQueue:Identifier vQueueName = schemaObjectThreePartName
        {
            Match(tQueue,CodeGenerationSupporter.Queue);
            vResult.Name = vQueueName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        queueOptionList[vResult, true]
    ;

alterRoleStatement returns [AlterRoleStatement vResult = this.FragmentFactory.CreateFragment<AlterRoleStatement>()]
{
    AlterRoleAction vAlterRoleAction;
    Identifier vIdentifier;
}
    :   tRole:Identifier vIdentifier=identifier
        {
            Match(tRole, CodeGenerationSupporter.Role);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vAlterRoleAction=alterRoleAction
        {
            vResult.Action=vAlterRoleAction;
        }
    ;

alterServerRoleStatement returns [AlterServerRoleStatement vResult = this.FragmentFactory.CreateFragment<AlterServerRoleStatement>()]
{
    AlterRoleAction vAlterRoleAction;
    Identifier vIdentifier;
}
    :   tRole:Identifier vIdentifier=identifier
        {
            Match(tRole, CodeGenerationSupporter.Role);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vAlterRoleAction=alterRoleAction
        {
            vResult.Action=vAlterRoleAction;
        }
    ;

alterRoleAction returns [AlterRoleAction vResult]
    : 
        vResult=addMemberAlterRoleAction
      | vResult=dropMemberAlterRoleAction
      | vResult=renameAlterRoleAction
    ;

renameAlterRoleAction returns [RenameAlterRoleAction vResult = FragmentFactory.CreateFragment<RenameAlterRoleAction>()]
{
    Identifier vIdentifier;
}
    :   With tName:Identifier EqualsSign vIdentifier=identifier
        {
            Match(tName, CodeGenerationSupporter.Name);
            UpdateTokenInfo(vResult, tName);
            vResult.NewName = vIdentifier;
        }
    ;

addMemberAlterRoleAction returns [AddMemberAlterRoleAction vResult = FragmentFactory.CreateFragment<AddMemberAlterRoleAction>()]
{
    Identifier vIdentifier;
}
    :   tAdd:Add tMember:Identifier vIdentifier=identifier
        {
            UpdateTokenInfo(vResult, tAdd);
            Match(tMember, CodeGenerationSupporter.Member);
            vResult.Member = vIdentifier;
        }
    ;

dropMemberAlterRoleAction returns [DropMemberAlterRoleAction vResult = FragmentFactory.CreateFragment<DropMemberAlterRoleAction>()]
{
    Identifier vIdentifier;
}
    :   tDrop:Drop tMember:Identifier vIdentifier=identifier
        {
            UpdateTokenInfo(vResult, tDrop);
            Match(tMember, CodeGenerationSupporter.Member);
            vResult.Member = vIdentifier;
        }
    ;

routeOptionList[RouteStatement vParent]
{
    RouteOption vRouteOption;
    long encounteredOptions = 0;
}
    :   With vRouteOption=routeOption
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vRouteOption.OptionKind, vRouteOption);
            AddAndUpdateTokenInfo(vParent, vParent.RouteOptions,vRouteOption);
        }
        (
            Comma vRouteOption=routeOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vRouteOption.OptionKind, vRouteOption);
                AddAndUpdateTokenInfo(vParent, vParent.RouteOptions,vRouteOption);
            }
        )*
    ;

routeOption returns [RouteOption vResult = this.FragmentFactory.CreateFragment<RouteOption>()]
{
    Literal vLiteral;
}
    :
        {NextTokenMatches(CodeGenerationSupporter.LifeTime)}?
        tLifeTime:Identifier EqualsSign vLiteral=integer
        {
            vResult.OptionKind = RouteOptionKind.Lifetime;
            vResult.Literal = vLiteral;
            UpdateTokenInfo(vResult, tLifeTime);
        }
    |
        tOption:Identifier EqualsSign vLiteral=stringLiteral
        {
            vResult.OptionKind = RouteOptionHelper.Instance.ParseOption(tOption);
            vResult.Literal = vLiteral;
            UpdateTokenInfo(vResult, tOption);
        }
    ;

alterRouteStatement returns [AlterRouteStatement vResult = this.FragmentFactory.CreateFragment<AlterRouteStatement>()]
{
    Identifier vIdentifier;
}
    : 
        tRoute:Identifier vIdentifier=identifier
        {
            Match(tRoute,CodeGenerationSupporter.Route);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        routeOptionList[vResult]
    ;

alterSchemaStatement returns [AlterSchemaStatement vResult = FragmentFactory.CreateFragment<AlterSchemaStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vObjectName;
}
    : Schema vIdentifier = identifier
    {
        vResult.Name = vIdentifier;
        ThrowPartialAstIfPhaseOne(vResult);
    }
    tTransfer:Identifier
    {
        Match(tTransfer,CodeGenerationSupporter.Transfer);
    }
        (
            tObjectType:Identifier DoubleColon
            {
                if (TryMatch(tObjectType,CodeGenerationSupporter.Object))
                    vResult.ObjectKind = SecurityObjectKind.Object;
                else
                {
                    Match(tObjectType,CodeGenerationSupporter.Type);
                    vResult.ObjectKind = SecurityObjectKind.Type;
                }
            }
        |    
            tXml:Identifier Schema tCollection:Identifier DoubleColon
            {
                Match(tXml,CodeGenerationSupporter.Xml);
                Match(tCollection,CodeGenerationSupporter.Collection);
                vResult.ObjectKind = SecurityObjectKind.XmlSchemaCollection;
            }
        )?
    vObjectName = schemaObjectTwoPartName
    {
        vResult.ObjectName = vObjectName;
    }
    ;

alterSequenceStatement returns [AlterSequenceStatement vResult = this.FragmentFactory.CreateFragment<AlterSequenceStatement>()]
{
    SchemaObjectName vName;
}
    :   tSequence:Identifier vName = schemaObjectTwoPartName
        {
            Match(tSequence, CodeGenerationSupporter.Sequence);
            vResult.Name = vName;
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            alterSequenceOptionList[vResult]
        )?
    ;

alterSequenceOptionList [AlterSequenceStatement vParent]
{
    SequenceOption vSequenceOption;
    long encounteredOptions = 0; 
}
    :   
      (
        options {greedy=true;} :
        vSequenceOption = alterSequenceOptionListElement
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vSequenceOption.OptionKind, vSequenceOption);
            AddAndUpdateTokenInfo<SequenceOption>(vParent, vParent.SequenceOptions, vSequenceOption);    
        }
      )+
    ;
        

alterServiceStatements returns [TSqlStatement vResult = null]
    : tService:Identifier
    {
        Match(tService,CodeGenerationSupporter.Service);
    }
    ( vResult = alterServiceMasterKeyStatement
    | vResult = alterServiceStatement
    )
    ;
    
alterServiceMasterKeyStatement returns [AlterServiceMasterKeyStatement vResult = FragmentFactory.CreateFragment<AlterServiceMasterKeyStatement>()]
{
    Literal vAccount, vPassword;
}
    : tMaster:Identifier Key
        {
            Match(tMaster,CodeGenerationSupporter.Master);
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (    {NextTokenMatches(CodeGenerationSupporter.Force)}?
            tForce:Identifier tRegenerate:Identifier
            {
                Match(tForce,CodeGenerationSupporter.Force);
                Match(tRegenerate,CodeGenerationSupporter.Regenerate);
                vResult.Kind = AlterServiceMasterKeyOption.ForceRegenerate;
                UpdateTokenInfo(vResult,tRegenerate);
            }
        |
            tRegenerate2:Identifier
            {
                Match(tRegenerate2,CodeGenerationSupporter.Regenerate);
                vResult.Kind = AlterServiceMasterKeyOption.Regenerate;
                UpdateTokenInfo(vResult,tRegenerate2);
            }
        |
            With tOldNewAccount:Identifier EqualsSign vAccount = stringLiteral Comma tOldNewPassword:Identifier EqualsSign vPassword = stringLiteral
            {
                if (TryMatch(tOldNewAccount,CodeGenerationSupporter.OldAccount))
                {
                    Match(tOldNewPassword,CodeGenerationSupporter.OldPassword);
                    vResult.Kind = AlterServiceMasterKeyOption.WithOldAccount;
                }
                else
                {
                    Match(tOldNewAccount,CodeGenerationSupporter.NewAccount);
                    Match(tOldNewPassword,CodeGenerationSupporter.NewPassword);
                    vResult.Kind = AlterServiceMasterKeyOption.WithNewAccount;
                }
                vResult.Account = vAccount;
                vResult.Password = vPassword;
            }
        )
    ;

alterUserOption returns [PrincipalOption vResult]
    : tOption:Identifier EqualsSign
        (
            {TryMatch(tOption, CodeGenerationSupporter.Password)}?
            vResult=passwordAlterUserOption[tOption]
         |    vResult=identifierAlterUserOption[tOption]
         |    vResult=literalAlterUserOption[tOption]
        )
    ;

identifierAlterUserOption[IToken tOption] returns [IdentifierPrincipalOption vResult = FragmentFactory.CreateFragment<IdentifierPrincipalOption>()]
{
    Identifier vIdentifier;
}
    :   vIdentifier=identifier
        {
            vResult.OptionKind = UserOptionHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
            vResult.Identifier = vIdentifier;
            UpdateTokenInfo(vResult, tOption);
        }
    ;

literalAlterUserOption[IToken tOption] returns [LiteralPrincipalOption vResult = FragmentFactory.CreateFragment<LiteralPrincipalOption>()]
{
    Literal vLiteral;
}
    :   vLiteral=integer
        {
            Match(tOption, CodeGenerationSupporter.DefaultLanguage);
            vResult.OptionKind=PrincipalOptionKind.DefaultLanguage;
            vResult.Value = vLiteral;
            UpdateTokenInfo(vResult, tOption);
        }
    |    vLiteral=nullLiteral
        {
            Match(tOption, CodeGenerationSupporter.DefaultSchema);
            vResult.OptionKind=PrincipalOptionKind.DefaultSchema;
            vResult.Value=vLiteral;
        }
    ;

passwordAlterUserOption[IToken tOption] returns [PasswordAlterPrincipalOption vResult = FragmentFactory.CreateFragment<PasswordAlterPrincipalOption>()]
{
    Literal vPassword, vOldPassword;
}
    : vPassword = loginPassword 
        {
            Match(tOption, CodeGenerationSupporter.Password);
            UpdateTokenInfo(vResult, tOption);
            vResult.Password = vPassword;
            vResult.OptionKind=PrincipalOptionKind.Password;
        }
        (    
            tOption2:Identifier EqualsSign vOldPassword = stringLiteral
            {
                Match(tOption2,CodeGenerationSupporter.OldPassword);
                vResult.OldPassword = vOldPassword;
            }
        )?
    ;

alterUserStatement returns [AlterUserStatement vResult = this.FragmentFactory.CreateFragment<AlterUserStatement>()]
{
    Identifier vIdentifier;
    PrincipalOption vUserOption;
}
    :   User vIdentifier = identifier
        {
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        With vUserOption=alterUserOption
        {
            AddAndUpdateTokenInfo(vResult, vResult.UserOptions,vUserOption);
        }
        (   
            Comma vUserOption=alterUserOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.UserOptions,vUserOption);
            }
        )*
    ;

alterXmlSchemaCollectionStatement returns [AlterXmlSchemaCollectionStatement vResult = this.FragmentFactory.CreateFragment<AlterXmlSchemaCollectionStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    ScalarExpression vExpression;
}
    : 
        tXml:Identifier Schema tCollection:Identifier vSchemaObjectName=schemaObjectNonEmptyTwoPartName
        {
            Match(tXml,CodeGenerationSupporter.Xml);
            Match(tCollection,CodeGenerationSupporter.Collection);
            vResult.Name = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        Add vExpression=expression
        {
            vResult.Expression = vExpression;
        }  
    ;

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
            vResult=grantStatement90
        | 
            vResult=denyStatement90
        | 
            vResult=revokeStatement90
        )
        optSemicolons[vResult]
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
        scalarProcedureParameter[vResult, false, false]
    ;
    
functionReturnTypeAndBody [FunctionStatementBody vParent, out bool vParseErrorOccurred]
{
    DataTypeReference vDataType;
    DeclareTableVariableBody vDeclareTableBody;
    SelectFunctionReturnType vSelectReturn;
    BeginEndBlockStatement vCompoundStatement;
    MethodSpecifier vMethodSpecifier;
    vParseErrorOccurred = false;
}
    :
        // Scalar functions or CLR function with scalar return data type
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
        | 
            vMethodSpecifier = methodSpecifier optSemicolons[vParent]
            {
                vParent.MethodSpecifier = vMethodSpecifier;
            }
        )
    | 
        // Inline table-valued functions
        Table (functionAttributesNoExecuteAs[vParent])? (As)? Return vSelectReturn = functionReturnClauseRelational
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
    | 
        // CLR functions
        Table vDeclareTableBody = declareTableBodyMain[IndexAffectingStatement.CreateOrAlterFunction]
        {
            TableValuedFunctionReturnType vTableResult = FragmentFactory.CreateFragment<TableValuedFunctionReturnType>();
            vTableResult.DeclareTableVariableBody = vDeclareTableBody;
            vParent.ReturnType = vTableResult;
        }
        (functionAttributes[vParent])? 
        (clrTableValuedFunctionOrderHint[vParent])?
        (As)? vMethodSpecifier = methodSpecifier optSemicolons[vParent]
        {
            vParent.MethodSpecifier = vMethodSpecifier;
        }
    ;
    
functionReturnClauseRelational returns [SelectFunctionReturnType vResult = FragmentFactory.CreateFragment<SelectFunctionReturnType>()]
{
    WithCtesAndXmlNamespaces vWithCTEAndXmlNamespaces;
    QueryExpression vSubqueryExpression;
    SelectStatement vSelectStatement;
}
    : vSelectStatement = subqueryExpressionWithOptionalCTE 
        {
            vResult.SelectStatement = vSelectStatement;
        }
    | tLParen:LeftParenthesis vWithCTEAndXmlNamespaces=withCommonTableExpressionsAndXmlNamespaces vSubqueryExpression = subqueryExpression[SubDmlFlags.SelectNotForInsert] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            // check for invalid combination of CHANGE_TRACKING_CONTEXT and Select statement
            if ((vWithCTEAndXmlNamespaces != null) && (vWithCTEAndXmlNamespaces.ChangeTrackingContext != null))
                ThrowParseErrorException("SQL46072", vWithCTEAndXmlNamespaces.ChangeTrackingContext, TSqlParserResource.SQL46072Message);
            vSelectStatement = FragmentFactory.CreateFragment<SelectStatement>();
            vSelectStatement.QueryExpression = vSubqueryExpression;
            vSelectStatement.WithCtesAndXmlNamespaces = vWithCTEAndXmlNamespaces; 
            vResult.SelectStatement = vSelectStatement;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;
         
functionAttributes [FunctionStatementBody vParent]
{
    FunctionOption vOption;
    long encounteredOptions = 0;
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

functionAttributesNoExecuteAs [FunctionStatementBody vParent]
{
    FunctionOption vOption;
    long encounteredOptions = 0;
}
    : With vOption=functionAttributeNoExecuteAs
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);        
        }
        (
            Comma vOption=functionAttributeNoExecuteAs
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);        
            }
        )*
    ;
    
functionAttributeNoExecuteAs returns [FunctionOption vResult = FragmentFactory.CreateFragment<FunctionOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind=ParseAlterCreateFunctionWithOption(tOption);
            UpdateTokenInfo(vResult, tOption);
        }
    | tReturns:Identifier Null On Null tInput:Identifier
        {
            Match(tReturns,CodeGenerationSupporter.Returns);
            Match(tInput,CodeGenerationSupporter.Input);
            vResult.OptionKind = FunctionOptionKind.ReturnsNullOnNullInput;
            UpdateTokenInfo(vResult, tInput);
        }
    | tCalled:Identifier On Null tInput2:Identifier
        {
            Match(tCalled,CodeGenerationSupporter.Called);
            Match(tInput2,CodeGenerationSupporter.Input);
            vResult.OptionKind = FunctionOptionKind.CalledOnNullInput;
            UpdateTokenInfo(vResult, tInput2);
        }
    ;
    
functionAttribute returns [FunctionOption vResult]
    : vResult=functionAttributeNoExecuteAs
    | vResult=functionExecuteAsOption
    ;

functionExecuteAsOption returns [ExecuteAsFunctionOption vResult=FragmentFactory.CreateFragment<ExecuteAsFunctionOption>()]
{
    ExecuteAsClause vExecuteAs;
}
    :
        vExecuteAs=executeAsClause[false, false]
        {
            vResult.OptionKind=FunctionOptionKind.ExecuteAs;
            vResult.ExecuteAs = vExecuteAs;
        }
    ;
    
clrTableValuedFunctionOrderHint [FunctionStatementBody vParent]
{
    OrderBulkInsertOption vOrderHint;
}
    :   vOrderHint = bulkInsertSortOrderOption
        {
            vParent.OrderHint = vOrderHint;
        }
    ;    

createStatisticsStatement returns [CreateStatisticsStatement vResult = this.FragmentFactory.CreateFragment<CreateStatisticsStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    StatisticsOption vStatisticsOption;
    BooleanExpression vExpression;
    bool isConflictingOption = false;
}
    :   tCreate:Create tStatistics:Statistics vIdentifier=identifier
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
        (vExpression = filterClause[false]
            {
                vResult.FilterPredicate = vExpression;
            }
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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

statsStreamStatisticsOption returns [LiteralStatisticsOption vResult = this.FragmentFactory.CreateFragment<LiteralStatisticsOption>()]
{
    Literal vLiteral;
}
    :
        tStatsStream:Identifier EqualsSign vLiteral=binary 
        {
            Match(tStatsStream, CodeGenerationSupporter.StatsStream);
            vResult.OptionKind = StatisticsOptionKind.StatsStream;
            vResult.Literal = vLiteral;
        }
    ;

simpleStatisticsOption[ref bool isConflictingOption] returns [StatisticsOption vResult = FragmentFactory.CreateFragment<StatisticsOption>()]
    :
        tOption:Identifier 
        {
            if (TryMatch(tOption, CodeGenerationSupporter.FullScan)) 
            {
                 if (isConflictingOption == true)
                    ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                else
                    isConflictingOption = true;
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.OptionKind = ParseCreateStatisticsWithOption(tOption);            
        }
    ;

statisticsPartitionRange returns [StatisticsPartitionRange vResult = FragmentFactory.CreateFragment<StatisticsPartitionRange>()]
{
    IntegerLiteral vInteger;
}
    :   vInteger = integer
        {
            vResult.From = vInteger;
        }
        (
            To vInteger = integer
            {
                vResult.To = vInteger;
            }
        )?
    ;

resampleStatisticsOption returns [ResampleStatisticsOption vResult = this.FragmentFactory.CreateFragment<ResampleStatisticsOption>()]
{
    StatisticsPartitionRange vRange;
}
    : tOption:Identifier
        {
            Match(tOption, CodeGenerationSupporter.Resample);
            vResult.OptionKind = StatisticsOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult, tOption);
        }
        (
            On tPartitions:Identifier LeftParenthesis vRange = statisticsPartitionRange
            {
                Match(tPartitions, CodeGenerationSupporter.Partitions);
                AddAndUpdateTokenInfo(vResult, vResult.Partitions, vRange);
            }
            (
                Comma vRange = statisticsPartitionRange
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Partitions, vRange);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen);
            }
        )?
    ;

incrementalStatisticsOption returns [OnOffStatisticsOption vResult = this.FragmentFactory.CreateFragment<OnOffStatisticsOption>()]
{
    OptionState vOptionState;
}
    : tOption:Identifier EqualsSign vOptionState=optionOnOff[vResult]
        {
            Match(tOption, CodeGenerationSupporter.Incremental);            
            vResult.OptionKind = StatisticsOptionKind.Incremental;
            vResult.OptionState = vOptionState;
            UpdateTokenInfo(vResult, tOption);
        }
    ;

createStatisticsStatementWithOption[ref bool isConflictingOption] returns [StatisticsOption vResult]
    :  
        {NextTokenMatches(CodeGenerationSupporter.Incremental)}?
        vResult = incrementalStatisticsOption
    | 
        vResult = sampleStatisticsOption[ref isConflictingOption]
    |
        vResult = statsStreamStatisticsOption
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
    :   tUpdate:Update tStatistics:Statistics
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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

updateStatisticsStatementWithOption [ref bool isConflictingOption] returns [StatisticsOption vResult]
    :
        {NextTokenMatches(CodeGenerationSupporter.Resample)}? 
        vResult = resampleStatisticsOption
    |   {NextTokenMatches(CodeGenerationSupporter.Incremental)}?
        vResult = incrementalStatisticsOption
    |   vResult = sampleStatisticsOption[ref isConflictingOption]
    |
        {NextTokenMatches(CodeGenerationSupporter.StatsStream)}?
        vResult = statsStreamStatisticsOption
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
            if (TryMatch(tOption, CodeGenerationSupporter.FullScan)) 
            {
                 if (isConflictingOption)
                    ThrowParseErrorException("SQL46071", tOption, TSqlParserResource.SQL46071Message);
                else
                    isConflictingOption = true;
            }
            UpdateTokenInfo(vResult,tOption);
            vResult.OptionKind = StatisticsOptionHelper.Instance.ParseOption(tOption);
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
                {
                    vParseErrorHappened = true;
                }
            else
                {
                    vResult.ThenStatement = vStatement;
                }
        }
        ( 
            // The closest if claims the else...
            options {greedy = true; } :
            Else ( vStatement=statementOptSemi )
            {
            if (null == vStatement) // if a parse error happens
                {
                    vParseErrorHappened = true;
                }
            else
                {
                    vResult.ElseStatement = vStatement;
                }
            }
        )?
        {
            // Won't return the fragment with the error in it.
            // Because code generation will fail for this case.
            if (true == vParseErrorHappened)
                {
                    vResult = null;
                }
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

useFederationStatement returns [UseFederationStatement vResult = FragmentFactory.CreateFragment<UseFederationStatement>()]
{
    Identifier vName;
    Identifier vDistributionName;
    ScalarExpression vExpression;
}
    : 
        tUse:Use 
        {
            UpdateTokenInfo(vResult,tUse);
        }
        tFederation:Identifier
        {
            Match(tFederation, CodeGenerationSupporter.Federation);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Root)}?
            tRoot:Identifier
            {
                Match(tRoot, CodeGenerationSupporter.Root);
            }
            With
        |
            vName = identifier
            {
                vResult.FederationName = vName;
            }
            LeftParenthesis
            vDistributionName = identifier
            {
                vResult.DistributionName = vDistributionName;
            }
            EqualsSign
            vExpression = expression
            {
                vResult.Value = vExpression;
            }
            RightParenthesis With tFiltering:Identifier
            {
                Match(tFiltering, CodeGenerationSupporter.Filtering);
            }
            EqualsSign
            (
                On
                {
                    vResult.Filtering = true;
                }
            |
                Off
            )
            Comma
        )
        tReset:Identifier
        {
            Match(tReset, CodeGenerationSupporter.Reset);
            UpdateTokenInfo(vResult, tReset);
        }    
        ;

killStatements returns [TSqlStatement vResult = null]
    :
        tKill:Kill 
        (
            vResult=killStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Query)}?
            vResult=killQueryNotificationSubscriptionStatement
        |
            vResult=killStatsJobStatement
        )
        {
            UpdateTokenInfo(vResult,tKill);
        }

    ;

killStatement returns [KillStatement vResult = this.FragmentFactory.CreateFragment<KillStatement>()]
{
    ScalarExpression vExpression;
}
    :
        (
            vExpression=signedInteger
        |
            vExpression=stringLiteral
        )
        {
            vResult.Parameter = vExpression;
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With tStatusOnly:Identifier
            {
                Match(tStatusOnly, CodeGenerationSupporter.StatusOnly);
                vResult.WithStatusOnly = true;
                UpdateTokenInfo(vResult,tStatusOnly);
            }
        )?
    ;

killQueryNotificationSubscriptionStatement returns [KillQueryNotificationSubscriptionStatement vResult = this.FragmentFactory.CreateFragment<KillQueryNotificationSubscriptionStatement>()]
{
    Literal vLiteral;
}
    :
        tQuery:Identifier
        {
            Match(tQuery, CodeGenerationSupporter.Query);
        }
        tNotification:Identifier
        {
            Match(tNotification, CodeGenerationSupporter.Notification);
        }
        tSubscription:Identifier
        {
            Match(tSubscription, CodeGenerationSupporter.Subscription);
        }
        (
            tAll:All
            {
                vResult.All = true;
                UpdateTokenInfo(vResult,tAll);
            }
        |
            vLiteral=integer
            {
                vResult.SubscriptionId = vLiteral;
            }
        )
    ;

killStatsJobStatement returns [KillStatsJobStatement vResult = this.FragmentFactory.CreateFragment<KillStatsJobStatement>()]
{
    ScalarExpression vExpression;
}
    :
        tStats:Identifier
        {
            Match(tStats, CodeGenerationSupporter.Stats);
        }
        tJob:Identifier
        {
            Match(tJob, CodeGenerationSupporter.Job);
        }
        vExpression=signedInteger
        {
            vResult.JobId = vExpression;
        }
    ;

checkpointStatement returns [CheckpointStatement vResult = this.FragmentFactory.CreateFragment<CheckpointStatement>()]
{
    Literal vLiteral;
}
    :
        tCheckpoint:Checkpoint
        {
            UpdateTokenInfo(vResult,tCheckpoint);
        }
        (
            vLiteral=integer
            {
                vResult.Duration = vLiteral;
            }
        )?
    ;

reconfigureStatement returns [ReconfigureStatement vResult = this.FragmentFactory.CreateFragment<ReconfigureStatement>()]
    :
        tReconfigure:Reconfigure
        {
            UpdateTokenInfo(vResult,tReconfigure);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
                // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } :
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
    CompressionPartitionRange vRange;
}
    :
        tTruncate:Truncate Table vSchemaObjectName=schemaObjectThreePartName
        {
            UpdateTokenInfo(vResult,tTruncate);
            vResult.TableName = vSchemaObjectName;
        }
        (
            With LeftParenthesis  tPartitions:Identifier LeftParenthesis vRange = compressionPartitionRange
            {
                AddAndUpdateTokenInfo(vResult, vResult.PartitionRanges, vRange);
            }
            (
                Comma vRange = compressionPartitionRange
                {
                    AddAndUpdateTokenInfo(vResult, vResult.PartitionRanges, vRange);
                }
            )*
            RightParenthesis RightParenthesis
        )?
    ;

permission returns [Permission vResult = this.FragmentFactory.CreateFragment<Permission>()]
{
    Identifier vIdentifier;
}
    :
        (
            vIdentifier=securityStatementPermission
            {
                if (vResult.Identifiers.Count >= 5)
                {
                    throw GetUnexpectedTokenErrorException(vIdentifier);
                }
                AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
            }
        )+
        (
            columnNameList[vResult, vResult.Columns]
        )?
    ;

securityTargetObjectCommon[SecurityTargetObject vParent]
{
    Identifier vIdentifier1;
    Identifier vIdentifier2;
    Identifier vIdentifier3;
    MultiPartIdentifier vMultiPartIdentifier;
    SecurityTargetObjectName vObjectName = this.FragmentFactory.CreateFragment<SecurityTargetObjectName>();    
}
    :
        tOn:On
        {
            UpdateTokenInfo(vParent,tOn);
        }
        (
            ((securityStatementPermission)+ DoubleColon)=>
            (
                vIdentifier1=securityStatementPermission
                {
                    vParent.ObjectKind = ParseSecurityObjectKind(vIdentifier1);
                }
            |
                vIdentifier1=securityStatementPermission
                (
                    vIdentifier2=securityStatementPermission
                    {
                        vParent.ObjectKind = ParseSecurityObjectKind(vIdentifier1, vIdentifier2);
                    }
                |
                    vIdentifier2=securityStatementPermission vIdentifier3=securityStatementPermission 
                    {
                        vParent.ObjectKind = ParseSecurityObjectKind(vIdentifier1, vIdentifier2, vIdentifier3);
                    }
                )
            )
            DoubleColon
        )?
        vMultiPartIdentifier=multiPartIdentifier[-1]
        {
            vObjectName.MultiPartIdentifier = vMultiPartIdentifier;
            vParent.ObjectName = vObjectName;
        }        
    ;

securityTargetObject [bool vColumnsDisallowed] returns [SecurityTargetObject vResult = this.FragmentFactory.CreateFragment<SecurityTargetObject>()]
    :
        securityTargetObjectCommon[vResult]
        (
            columnNameList[vResult, vResult.Columns]
            {
                if(vColumnsDisallowed)
                {
                    ThrowIncorrectSyntaxErrorException(vResult.Columns[0]);
                }
            }
        )?        
    ;

authorizationTargetObject returns [SecurityTargetObject vResult = this.FragmentFactory.CreateFragment<SecurityTargetObject>()]
    :
        securityTargetObjectCommon[vResult]
    ;

principal returns [SecurityPrincipal vResult = this.FragmentFactory.CreateFragment<SecurityPrincipal>()]
{
    Identifier vIdentifier;
}
    :
        tPublic:Public
        {
            UpdateTokenInfo(vResult,tPublic);
            vResult.PrincipalType = PrincipalType.Public;            
        }
    |
        tNull:Null
        {
            UpdateTokenInfo(vResult,tNull);
            vResult.PrincipalType = PrincipalType.Null;            
        }
    |
        vIdentifier=identifier
        {
            vResult.PrincipalType = PrincipalType.Identifier;
            vResult.Identifier = vIdentifier;
        }
    ;

alterAuthorizationStatement returns [AlterAuthorizationStatement vResult = this.FragmentFactory.CreateFragment<AlterAuthorizationStatement>()]
{
    SecurityTargetObject vSecurityTargetObject;
    Identifier vIdentifier;
}
    :
        Authorization vSecurityTargetObject=authorizationTargetObject
        {
            vResult.SecurityTargetObject = vSecurityTargetObject;
        }
        To
        (
            Schema tOwner:Identifier
            {
                Match(tOwner, CodeGenerationSupporter.Owner);
                UpdateTokenInfo(vResult,tOwner);
                vResult.ToSchemaOwner = true;
            }
        |
            vIdentifier=identifier
            {
                vResult.PrincipalName = vIdentifier;
            }
        )
    ;

grantStatement90 returns [GrantStatement vResult = FragmentFactory.CreateFragment<GrantStatement>()]
{
    bool vPermissionContainsColumns = false;
}
    :    tGrant:Grant 
        {
            UpdateTokenInfo(vResult,tGrant);
        }
        permissionsList[vResult, ref vPermissionContainsColumns]
        securityStatementTargetObjectOpt[vResult, vPermissionContainsColumns]
        To securityStatementPrincipalList[vResult]
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With Grant tOption:Option
            {
                vResult.WithGrantOption = true;
                UpdateTokenInfo(vResult,tOption);
            }
        )?
        securityStatementAsClauseOpt[vResult]
    ;

denyStatement90 returns [DenyStatement vResult = FragmentFactory.CreateFragment<DenyStatement>()]
{
    bool vPermissionContainsColumns = false;
}
    :    tDeny:Deny 
        {
            UpdateTokenInfo(vResult,tDeny);
        }
        permissionsList[vResult, ref vPermissionContainsColumns]
        securityStatementTargetObjectOpt[vResult, vPermissionContainsColumns]
        To securityStatementPrincipalList[vResult]
        (
            tCascade:Cascade
            {
                vResult.CascadeOption = true;
                UpdateTokenInfo(vResult,tCascade);
            }
        )?
        securityStatementAsClauseOpt[vResult]
    ;

revokeStatement90 returns [RevokeStatement vResult = this.FragmentFactory.CreateFragment<RevokeStatement>()]
{
    bool vPermissionContainsColumns = false;
}
    :    tRevoke:Revoke 
        {
            UpdateTokenInfo(vResult,tRevoke);
        }
        (
            Grant Option For
            {
                vResult.GrantOptionFor = true;
            }
        )?
        permissionsList[vResult, ref vPermissionContainsColumns]
        securityStatementTargetObjectOpt[vResult, vPermissionContainsColumns]
        ( To | From ) securityStatementPrincipalList[vResult]
        (
            tCascade:Cascade
            {
                vResult.CascadeOption = true;
                UpdateTokenInfo(vResult,tCascade);
            }
        )?
        securityStatementAsClauseOpt[vResult]
    ;

securityStatementPrincipalList [SecurityStatement vParent]
{
    SecurityPrincipal vPrincipal;
}
    :    vPrincipal=principal
        {
            AddAndUpdateTokenInfo(vParent, vParent.Principals,vPrincipal);
        }
        (
            Comma vPrincipal=principal
            {
                AddAndUpdateTokenInfo(vParent, vParent.Principals,vPrincipal);
            }
        )*
    ;

permissionsList [SecurityStatement vParent, ref bool vContainsColumnList]
{
    Permission vPermission;
}
    :    vPermission=permission
        {
            AddAndUpdateTokenInfo(vParent, vParent.Permissions,vPermission);
            vContainsColumnList = vContainsColumnList || (vPermission.Columns != null && vPermission.Columns.Count > 0);
        }
        (
            Comma vPermission=permission
            {
                AddAndUpdateTokenInfo(vParent, vParent.Permissions,vPermission);
                vContainsColumnList = vContainsColumnList || (vPermission.Columns != null && vPermission.Columns.Count > 0);
            }
        )*
    ;    

securityStatementTargetObjectOpt  [SecurityStatement vParent, bool vColumnsDisallowed]
{
    SecurityTargetObject vSecurityTargetObject;
}
    :    (vSecurityTargetObject=securityTargetObject[vColumnsDisallowed]
            {
                vParent.SecurityTargetObject = vSecurityTargetObject;
            }
        )?
    ;
    
securityStatementAsClauseOpt [SecurityStatement vParent]
{
    Identifier vIdentifier;
}
    :    (As vIdentifier=identifier
            {
                vParent.AsClause = vIdentifier;
            }
        )?
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
    ScalarExpression vTimeout;
    WaitForSupportedStatement vStatement;
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
        |
            LeftParenthesis vStatement = waitforInnerStatement tRParen:RightParenthesis 
            {
                vResult.Statement = vStatement;
                vResult.WaitForOption = WaitForOption.Statement;
                UpdateTokenInfo(vResult,tRParen);
            }
            (Comma tTimeout:Identifier vTimeout = signedIntegerOrVariable
                {
                    Match(tTimeout,CodeGenerationSupporter.Timeout);
                    vResult.Timeout = vTimeout;
                }
            )?
        )
    ;

waitforInnerStatement returns [WaitForSupportedStatement vResult]
    :    {NextTokenMatches(CodeGenerationSupporter.Receive)}?
        vResult = receiveStatement
    |    vResult = getConversationGroupStatement
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } : 
            modificationTextStatementWithLog[vResult]
        )?
        (
            // Greedy due to conflict with identifierStatements
            options {greedy=true;} :         
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
        // Conflicts with select (which can start with '(' ) and enable/disable trigger (enable/disable are identifiers, not real keywords)
        ((expression) => vExpression = expression
            {
                vResult.Expression = vExpression;
            }
        |
        /* empty */
        )
    ;
    
openStatements returns [TSqlStatement vResult = null]
    :
        tOpen:Open
        (
            {NextTokenMatches(CodeGenerationSupporter.Master)}?
            vResult=openMasterKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Symmetric)}?
            vResult=openSymmetricKeyStatement
        |
            vResult=openCursorStatement
        )
        {
            UpdateTokenInfo(vResult,tOpen);
        }
    ;

openMasterKeyStatement returns [OpenMasterKeyStatement vResult = FragmentFactory.CreateFragment<OpenMasterKeyStatement>()]
{
    Literal vLiteral;
}
    : 
        tMaster:Identifier 
        {
            Match(tMaster, CodeGenerationSupporter.Master);
        }
        Key tDecryption:Identifier 
        {
            Match(tDecryption, CodeGenerationSupporter.Decryption);
        }
        By tPassword:Identifier 
        {
            Match(tPassword, CodeGenerationSupporter.Password);
        }
        EqualsSign vLiteral=stringLiteral
        {
            vResult.Password = vLiteral;
        }
    ;
    
decryptionMechanism returns [CryptoMechanism vResult]
    : 
        (vResult = passwordCrypto
        |vResult = certificateCrypto
            (
                // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } : 
                With decryptionMechanismPassword[vResult]
            )?
        | vResult = keyCrypto
            (
                // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } : 
                tWith:With decryptionMechanismPassword[vResult]
                {
                    if (vResult.CryptoMechanismType == CryptoMechanismType.SymmetricKey)
                        throw GetUnexpectedTokenErrorException(tWith);
                }
            )?
       )
    ;

decryptionMechanismPassword[CryptoMechanism vParent]
{
    Literal vLiteral;
}
    : tPassword:Identifier EqualsSign vLiteral=stringLiteral
        {
            Match(tPassword, CodeGenerationSupporter.Password);
            vParent.PasswordOrSignature = vLiteral;
        }
    ;

openSymmetricKeyStatement returns [OpenSymmetricKeyStatement vResult = FragmentFactory.CreateFragment<OpenSymmetricKeyStatement>()]
{
    Identifier vIdentifier;
    CryptoMechanism vDecryptionMechanism;
}
    : 
        tSymmetric:Identifier 
        {
            Match(tSymmetric, CodeGenerationSupporter.Symmetric);
        }
        Key vIdentifier=identifier 
        {
            vResult.Name = vIdentifier;
        }
        tDecryption:Identifier 
        {
            Match(tDecryption, CodeGenerationSupporter.Decryption);
        }
        By vDecryptionMechanism=decryptionMechanism
        {
            vResult.DecryptionMechanism = vDecryptionMechanism;
        }
    ;

openCursorStatement returns [OpenCursorStatement vResult = FragmentFactory.CreateFragment<OpenCursorStatement>()]
{
    CursorId vCursorId;
}
    : 
        vCursorId = cursorId
        {
            vResult.Cursor = vCursorId;
        }
    ;
    
closeStatements returns [TSqlStatement vResult = null]
    :
        tClose:Close
        (
            {NextTokenMatches(CodeGenerationSupporter.Master)}?
            vResult=closeMasterKeyStatement
        |
            {NextTokenMatches(CodeGenerationSupporter.Symmetric) || LA(1) == All}?
            vResult=closeSymmetricKeyStatement
        |
            vResult=closeCursorStatement
        )
        {
            UpdateTokenInfo(vResult,tClose);
        }
    ;

closeMasterKeyStatement returns [CloseMasterKeyStatement vResult = FragmentFactory.CreateFragment<CloseMasterKeyStatement>()]
    : 
        tMaster:Identifier 
        {
            Match(tMaster, CodeGenerationSupporter.Master);
        }
        tKey:Key
        {
            UpdateTokenInfo(vResult,tKey);
        }
    ;

closeSymmetricKeyStatement returns [CloseSymmetricKeyStatement vResult = FragmentFactory.CreateFragment<CloseSymmetricKeyStatement>()]
{
    Identifier vIdentifier;
}
    : 
        (
            tSymmetric:Identifier 
            {
                Match(tSymmetric, CodeGenerationSupporter.Symmetric);
            }
            Key vIdentifier=identifier 
            {
                vResult.Name = vIdentifier;
            }
        |
            All tSymmetric2:Identifier 
            {
                Match(tSymmetric2, CodeGenerationSupporter.Symmetric);
                vResult.All = true;
            }
            tKeys:Identifier
            {
                Match(tKeys, CodeGenerationSupporter.Keys);
                UpdateTokenInfo(vResult,tKeys);
            }
        )
    ;

closeCursorStatement returns [CloseCursorStatement vResult = FragmentFactory.CreateFragment<CloseCursorStatement>()]
{
    CursorId vCursorId;
}
    : 
        vCursorId = cursorId
        {
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
        (      vResult = dropDatabaseStatements
            | vResult = dropIndexStatement
            | vResult = dropStatisticsStatement
            | vResult = dropTableStatement 
            | vResult = dropProcedureStatement 
            | vResult = dropFunctionStatement 
            | vResult = dropViewStatement 
            | vResult = dropDefaultStatement 
            | vResult = dropRuleStatement 
            | vResult = dropTriggerStatement
            | vResult = dropSchemaStatement
            | {NextTokenMatches(CodeGenerationSupporter.Aggregate)}?
              vResult = dropAggregateStatement
            | {NextTokenMatches(CodeGenerationSupporter.Assembly)}?
              vResult = dropAssemblyStatement
            | {NextTokenMatches(CodeGenerationSupporter.Availability)}?
              vResult = dropAvailabilityGroupStatement
            | {NextTokenMatches(CodeGenerationSupporter.Partition)}?
              vResult = dropPartitionStatements
            | {NextTokenMatches(CodeGenerationSupporter.Synonym)}?
              vResult = dropSynonymStatement
            | {NextTokenMatches(CodeGenerationSupporter.Application)}?
              vResult = dropApplicationRoleStatement
            | {NextTokenMatches(CodeGenerationSupporter.Broker)}?
              vResult = dropBrokerPriorityStatement              
            | {NextTokenMatches(CodeGenerationSupporter.Cryptographic)}?
              vResult = dropCryptographicProviderStatement              
            | {NextTokenMatches(CodeGenerationSupporter.Fulltext)}?
              vResult = dropFulltextStatements
            | {NextTokenMatches(CodeGenerationSupporter.Login)}?
              vResult = dropLoginStatement
            | {NextTokenMatches(CodeGenerationSupporter.Resource)}?
              vResult = dropResourcePoolStatement
            | {NextTokenMatches(CodeGenerationSupporter.Workload)}?
              vResult = dropWorkloadGroupStatement
            | {NextTokenMatches(CodeGenerationSupporter.Role)}?
              vResult = dropRoleStatement
            | {NextTokenMatches(CodeGenerationSupporter.Master)}?
              vResult = dropMasterKeyStatement
            | {NextTokenMatches(CodeGenerationSupporter.Symmetric)}?
              vResult = dropSymmetricKeyStatement
            | {NextTokenMatches(CodeGenerationSupporter.Asymmetric)}?
              vResult = dropAsymmetricKeyStatement
            | {NextTokenMatches(CodeGenerationSupporter.Certificate)}?
              vResult = dropCertificateStatement
            | {NextTokenMatches(CodeGenerationSupporter.Credential)}?
              vResult = dropCredentialStatement
            | {NextTokenMatches(CodeGenerationSupporter.Type)}?
              vResult = dropTypeStatement
            | {NextTokenMatches(CodeGenerationSupporter.Xml)}?
              vResult = dropXmlSchemaCollectionStatement
            | {NextTokenMatches(CodeGenerationSupporter.Contract)}?
              vResult = dropContractStatement
            | {NextTokenMatches(CodeGenerationSupporter.Queue)}?
              vResult = dropQueueStatement
            | {NextTokenMatches(CodeGenerationSupporter.Service)}?
              vResult = dropServiceStatement
            | {NextTokenMatches(CodeGenerationSupporter.Route)}?
              vResult = dropRouteStatement
            | {NextTokenMatches(CodeGenerationSupporter.Message)}?
              vResult = dropMessageTypeStatement
            | {NextTokenMatches(CodeGenerationSupporter.Remote)}?
              vResult = dropRemoteServiceBindingStatement
            | {NextTokenMatches(CodeGenerationSupporter.Endpoint)}?
              vResult = dropEndpointStatement
            | {NextTokenMatches(CodeGenerationSupporter.Signature) || NextTokenMatches(CodeGenerationSupporter.Counter)}?
              vResult = dropSignatureStatement
            | {NextTokenMatches(CodeGenerationSupporter.Event)}?
              vResult = dropEventStatement //// NOTIFICATION or SESSION
            | {NextTokenMatches(CodeGenerationSupporter.Search)}?
              vResult = dropSearchPropertyListStatement
            | {NextTokenMatches(CodeGenerationSupporter.Sequence)}?
              vResult = dropSequenceStatement
            | {NextTokenMatches(CodeGenerationSupporter.Federation)}?
              vResult = dropFederationStatement
            | vResult = dropServerStatements
            | vResult = dropUserStatement
        )
        {
            UpdateTokenInfo(vResult,tDrop);
        }
    ;

// Supports both SQL 2000 & SQL 2005 syntax
dropSchemaStatement returns [DropSchemaStatement vResult = FragmentFactory.CreateFragment<DropSchemaStatement>()]
{
    SchemaObjectName vObject;
}
    : Schema vObject = schemaObjectThreePartName
        {
            vResult.Schema = vObject;
        }
        (tCascade:Cascade
            {
                vResult.DropBehavior = DropSchemaBehavior.Cascade;
                UpdateTokenInfo(vResult,tCascade);
            }
        | tRestrict:Restrict
            {
                vResult.DropBehavior = DropSchemaBehavior.Restrict;
                UpdateTokenInfo(vResult,tRestrict);
            }
        )?
        {
            // Schema name with more than one part must be followed by either Restrict or Cascade (since it is SQL 2000 syntax)
            if (vResult.Schema.SchemaIdentifier != null && vResult.DropBehavior == DropSchemaBehavior.None)
                throw GetUnexpectedTokenErrorException(vResult.Schema.SchemaIdentifier);
        }
    ;

dropServerStatements returns [TSqlStatement vResult]
    : tServer:Identifier 
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }
        (
        {NextTokenMatches(CodeGenerationSupporter.Audit)}?
        (   
            tAudit:Identifier
            (
                {NextTokenMatches(CodeGenerationSupporter.Specification)}?
                vResult = dropServerAuditSpecificationStatement
            |
                vResult = dropServerAuditStatement
            )
        )
        | vResult=dropServerRoleStatement
        )
    ;

dropServerRoleStatement returns [DropServerRoleStatement vResult = FragmentFactory.CreateFragment<DropServerRoleStatement>()]
{
    Identifier vName;
}
    :   tRole:Identifier vName=identifier
        {
            Match(tRole, CodeGenerationSupporter.Role);
            vResult.Name = vName;
        }
    ;
    
dropServerAuditSpecificationStatement returns [DropServerAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<DropServerAuditSpecificationStatement>()]
{
    Identifier vName;
}
    :    tSpecification:Identifier vName = identifier
        {
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.Name = vName;
        }
    ;
    
dropServerAuditStatement returns [DropServerAuditStatement vResult = FragmentFactory.CreateFragment<DropServerAuditStatement>()]
{
    Identifier vName;
}
    : vName = identifier
        {
            vResult.Name = vName;
        }
    ;
    
dropDatabaseStatements returns [TSqlStatement vResult]
    : Database 
        (
            {NextTokenMatches(CodeGenerationSupporter.Audit)}?
            vResult = dropDatabaseAuditSpecificationStatement
        |
            vResult = dropDatabaseEncryptionKeyStatement
        |
            vResult = dropDatabaseStatement
        )
    ;
    
dropDatabaseStatement returns [DropDatabaseStatement vResult = FragmentFactory.CreateFragment<DropDatabaseStatement>()]
{
    Identifier vIdentifier;
}
    : vIdentifier = identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Databases, vIdentifier);
        }
        (Comma vIdentifier = identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Databases, vIdentifier);
            }
        )*
    ;
    
dropDatabaseAuditSpecificationStatement returns [DropDatabaseAuditSpecificationStatement vResult = FragmentFactory.CreateFragment<DropDatabaseAuditSpecificationStatement>()]
{
    Identifier vName;
}
    :    tAudit:Identifier tSpecification:Identifier vName = identifier
        {
            Match(tAudit, CodeGenerationSupporter.Audit);
            Match(tSpecification, CodeGenerationSupporter.Specification);
            vResult.Name = vName;            
        }
    ;

dropDatabaseEncryptionKeyStatement returns [DropDatabaseEncryptionKeyStatement vResult = FragmentFactory.CreateFragment<DropDatabaseEncryptionKeyStatement>()]
    : tEncryption:Identifier tKey:Key
        {
            Match(tEncryption, CodeGenerationSupporter.Encryption);
            UpdateTokenInfo(vResult, tKey);
        }
    ;

dropIndexStatement returns [DropIndexStatement vResult = FragmentFactory.CreateFragment<DropIndexStatement>()]
{
    DropIndexClauseBase vClause;
}
    : Index vClause = dropIndexClause
        {
            AddAndUpdateTokenInfo(vResult, vResult.DropIndexClauses, vClause);
        }
        (Comma vClause = dropIndexClause
            {
                AddAndUpdateTokenInfo(vResult, vResult.DropIndexClauses, vClause);
            }
        )*
    ;
    
dropIndexClause returns [DropIndexClauseBase vResult]
    : vResult = indexDropObject
    | vResult = indexDropObjectNewNameFormat 
    ;

indexDropObjectNewNameFormat returns [DropIndexClause vResult = FragmentFactory.CreateFragment<DropIndexClause>()]
{
    Identifier vIndex;
    SchemaObjectName vObject;
}
    : vIndex = identifier On vObject = schemaObjectThreePartName 
        {
            vResult.Index = vIndex;
            vResult.Object = vObject;
        }
        // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        (options {greedy = true; } : dropClusteredIndexOptions[vResult])?
    ;
    
dropClusteredIndexOptions [DropIndexClause vParent]
{
    long encounteredOptions = 0;
}
    : With LeftParenthesis dropClusteredIndexOption[vParent, ref encounteredOptions] (Comma dropClusteredIndexOption[vParent, ref encounteredOptions])* tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
dropClusteredIndexOption [DropIndexClause vParent, ref long encounteredOptions]
{
    IndexOption vIndexOption;    
}
     : (
            {NextTokenMatches(CodeGenerationSupporter.MaxDop)}?
            vIndexOption = maxDopOption
       |
            {NextTokenMatches(CodeGenerationSupporter.Online)}?
            vIndexOption = onlineIndexOption
        |    
            {NextTokenMatches(CodeGenerationSupporter.Move)}?
            vIndexOption = dropIndexMoveToOption
        |    
            {NextTokenMatches(CodeGenerationSupporter.FileStreamOn)}?
            vIndexOption = dropIndexFileStreamOnOption
        |    
            {NextTokenMatches(CodeGenerationSupporter.DataCompression)}?
            vIndexOption = dataCompressionOption
        )
         {
            CheckOptionDuplication(ref encounteredOptions, (int)vIndexOption.OptionKind, vIndexOption);
            AddAndUpdateTokenInfo(vParent,vParent.Options,vIndexOption);
         }
    ;

dropIndexMoveToOption returns [MoveToDropIndexOption vResult = FragmentFactory.CreateFragment<MoveToDropIndexOption>()]
{
    FileGroupOrPartitionScheme vFileGroup;    
}
    : tMove:Identifier To vFileGroup = filegroupOrPartitionScheme
        {
            Match(tMove,CodeGenerationSupporter.Move);
            UpdateTokenInfo(vResult, tMove);
            vResult.OptionKind = IndexOptionKind.MoveTo;
            vResult.MoveTo = vFileGroup;
        }
    ;

dropIndexFileStreamOnOption returns [FileStreamOnDropIndexOption vResult = FragmentFactory.CreateFragment<FileStreamOnDropIndexOption>()]
    :
        fileStreamOn[vResult]
        {
            vResult.OptionKind = IndexOptionKind.FileStreamOn;
        }
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
    :   vIdentifiers = identifierList[4]
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
{
    TriggerScope vTriggerScope;
}
    : Trigger dropObjectList[vResult,true]
        (
            On vTriggerScope=triggerScope[vResult]
            {
                vResult.TriggerScope = vTriggerScope;
            }
        )?
    ;

dropObjectList [DropObjectsStatement vParent, bool onlyTwoPartNames]
{
    SchemaObjectName vObject;
}
    : vObject = dropObject[onlyTwoPartNames]
        {
            AddAndUpdateTokenInfo(vParent, vParent.Objects,vObject);
        }
        (Comma vObject = dropObject[onlyTwoPartNames]
            {
                AddAndUpdateTokenInfo(vParent, vParent.Objects,vObject);
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

dropPartitionStatements returns [TSqlStatement vResult]
    : tPartition:Identifier 
        {
            Match(tPartition,CodeGenerationSupporter.Partition);
        }
        (
            vResult = dropPartitionFunctionStatement
        |
            vResult = dropPartitionSchemeStatement        
        )
    ;

dropPartitionFunctionStatement returns [DropPartitionFunctionStatement vResult = FragmentFactory.CreateFragment<DropPartitionFunctionStatement>()]
{
    Identifier vName;
}
    : Function vName = identifier
        {
            vResult.Name = vName;
        }
    ;
    
dropAggregateStatement returns [DropAggregateStatement vResult = FragmentFactory.CreateFragment<DropAggregateStatement>()]
    : tAggregate:Identifier dropObjectList[vResult,true]
        {
            Match(tAggregate, CodeGenerationSupporter.Aggregate);
        }
    ;
    
dropAssemblyStatement returns [DropAssemblyStatement vResult = FragmentFactory.CreateFragment<DropAssemblyStatement>()]
    : tAssembly:Identifier dropObjectList[vResult,true]
        {
            Match(tAssembly, CodeGenerationSupporter.Assembly);
        }
        (
            options {greedy=true;} : // Because of CTE ambiguity
            With tNo:Identifier tDependents:Identifier
            {
                Match(tNo, CodeGenerationSupporter.No);
                Match(tDependents, CodeGenerationSupporter.Dependents);
                vResult.WithNoDependents = true;
                UpdateTokenInfo(vResult,tDependents);
            }
        )?
    ;

dropAvailabilityGroupStatement returns [DropAvailabilityGroupStatement vResult = FragmentFactory.CreateFragment<DropAvailabilityGroupStatement>()]
{
    Identifier vName;
}
    : tAvailability:Identifier Group vName=identifier
        {
            Match(tAvailability, CodeGenerationSupporter.Availability);
            vResult.Name=vName;
        }
    ;
    
dropPartitionSchemeStatement returns [DropPartitionSchemeStatement vResult = FragmentFactory.CreateFragment<DropPartitionSchemeStatement>()]
{
    Identifier vName;
}
    : tScheme:Identifier vName = identifier
        {
            Match(tScheme,CodeGenerationSupporter.Scheme);
            vResult.Name = vName;
        }
    ;
    
dropSynonymStatement returns [DropSynonymStatement vResult = FragmentFactory.CreateFragment<DropSynonymStatement>()]
    : tSynonym:Identifier dropObjectList[vResult,true]
        {
            Match(tSynonym,CodeGenerationSupporter.Synonym);
        }
    ;
    
dropApplicationRoleStatement returns [DropApplicationRoleStatement vResult = FragmentFactory.CreateFragment<DropApplicationRoleStatement>()]
{
    Identifier vName;
}
    : tApplication:Identifier tRole:Identifier vName = identifier
        {
            Match(tApplication,CodeGenerationSupporter.Application);
            Match(tRole,CodeGenerationSupporter.Role);
            vResult.Name = vName;
        }
    ;

dropFulltextStatements returns [TSqlStatement vResult]
    : tFulltext:Identifier
        {
            Match(tFulltext,CodeGenerationSupporter.Fulltext);
        }
        (
            vResult = dropFulltextCatalogStatement
        |
            vResult = dropFulltextIndexStatement
        |
            vResult = dropFulltextStoplistStatement
        )
    ;
        
dropFulltextCatalogStatement returns [DropFullTextCatalogStatement vResult = FragmentFactory.CreateFragment<DropFullTextCatalogStatement>()]
{
    Identifier vName;
}
    : tCatalog:Identifier vName = identifier
        {
            Match(tCatalog,CodeGenerationSupporter.Catalog);
            vResult.Name = vName;
        }
    ;
    
dropFulltextIndexStatement returns [DropFullTextIndexStatement vResult = FragmentFactory.CreateFragment<DropFullTextIndexStatement>()]
{
    SchemaObjectName vTable;
}
    : Index On vTable = dropObject[false]
        {
            vResult.TableName = vTable;
        }
    ;

dropLoginStatement returns [DropLoginStatement vResult = FragmentFactory.CreateFragment<DropLoginStatement>()]
{
    Identifier vName;
}
    : tLogin:Identifier vName = identifier
        {
            Match(tLogin,CodeGenerationSupporter.Login);
            vResult.Name = vName;
        }
    ;
    
dropRoleStatement returns [DropRoleStatement vResult = FragmentFactory.CreateFragment<DropRoleStatement>()]
{
    Identifier vName;
}
    : tRole:Identifier vName = identifier
        {
            Match(tRole,CodeGenerationSupporter.Role);
            vResult.Name = vName;
        }
    ;

dropMasterKeyStatement returns [DropMasterKeyStatement vResult = FragmentFactory.CreateFragment<DropMasterKeyStatement>()]
    : tMaster:Identifier tKey:Key
        {
            Match(tMaster, CodeGenerationSupporter.Master);
            UpdateTokenInfo(vResult,tKey);
        }
    ;

dropSymmetricKeyStatement returns [DropSymmetricKeyStatement vResult = FragmentFactory.CreateFragment<DropSymmetricKeyStatement>()]
{
    Identifier vName;
    bool vIsRemoveProvider;
}
    : tSymmetric:Identifier Key vName = identifier
        {
            Match(tSymmetric, CodeGenerationSupporter.Symmetric);
            vResult.Name = vName;
        }
        vIsRemoveProvider = removeProviderKeyOpt[vResult]
        {
            vResult.RemoveProviderKey = vIsRemoveProvider;
        }
    ;
    
dropAsymmetricKeyStatement returns [DropAsymmetricKeyStatement vResult = FragmentFactory.CreateFragment<DropAsymmetricKeyStatement>()]
{
    Identifier vName;
    bool vIsRemoveProvider;
}
    : tAsymmetric:Identifier Key vName = identifier
        {
            Match(tAsymmetric, CodeGenerationSupporter.Asymmetric);
            vResult.Name = vName;
        }
        vIsRemoveProvider = removeProviderKeyOpt[vResult]
        {
            vResult.RemoveProviderKey = vIsRemoveProvider;
        }
    ;

removeProviderKeyOpt [TSqlFragment vParent] returns [bool vIsRemove = false]
    :
        (   // Greedy due to conflict with identifierStatements
            options { greedy = true; } : 
            tRemove:Identifier tProvider:Identifier tKey:Key
            {
                Match(tRemove, CodeGenerationSupporter.Remove);
                Match(tProvider, CodeGenerationSupporter.Provider);
                UpdateTokenInfo(vParent, tKey);
                vIsRemove = true;
            }
        )?
    ;

dropCertificateStatement returns [DropCertificateStatement vResult = FragmentFactory.CreateFragment<DropCertificateStatement>()]
{
    Identifier vName;
}
    : tCertificate:Identifier vName = identifier
        {
            Match(tCertificate, CodeGenerationSupporter.Certificate);
            vResult.Name = vName;
        }
    ;

dropCredentialStatement returns [DropCredentialStatement vResult = FragmentFactory.CreateFragment<DropCredentialStatement>()]
{
    Identifier vName;
}
    : tCredential:Identifier vName = identifier
        {
            Match(tCredential, CodeGenerationSupporter.Credential);
            vResult.Name = vName;
        }
    ;
    
dropTypeStatement returns [DropTypeStatement vResult = FragmentFactory.CreateFragment<DropTypeStatement>()]
{
    SchemaObjectName vType;
}
    : tType:Identifier vType = dropObject[false] 
        {
            Match(tType,CodeGenerationSupporter.Type);
            vResult.Name = vType;
        }
    ;
    
dropXmlSchemaCollectionStatement returns [DropXmlSchemaCollectionStatement vResult = FragmentFactory.CreateFragment<DropXmlSchemaCollectionStatement>()]
{
    SchemaObjectName vSchemaObjectName;    
}
    :
        tXml:Identifier Schema tCollection:Identifier vSchemaObjectName=schemaObjectNonEmptyTwoPartName
        {
            Match(tXml,CodeGenerationSupporter.Xml);
            Match(tCollection,CodeGenerationSupporter.Collection);
            vResult.Name = vSchemaObjectName;
        }
    ;
    
dropUserStatement returns [DropUserStatement vResult = FragmentFactory.CreateFragment<DropUserStatement>()]
{
    Identifier vName;
}
    : User vName = identifier
        {
            vResult.Name = vName;
        }
    ;

dropContractStatement returns [DropContractStatement vResult = FragmentFactory.CreateFragment<DropContractStatement>()]
{
    Identifier vName;
}
    : tContract:Identifier vName = identifier
        {
            Match(tContract,CodeGenerationSupporter.Contract);
            vResult.Name = vName;
        }
    ;
    
dropQueueStatement returns [DropQueueStatement vResult = FragmentFactory.CreateFragment<DropQueueStatement>()]
{
    SchemaObjectName vName;
}
    : tQueue:Identifier vName = schemaObjectThreePartName
        {
            Match(tQueue,CodeGenerationSupporter.Queue);
            vResult.Name = vName;
        }
    ;
            
dropServiceStatement returns [DropServiceStatement vResult = FragmentFactory.CreateFragment<DropServiceStatement>()]
{
    Identifier vName;
}
    : tService:Identifier vName = identifier
        {
            Match(tService,CodeGenerationSupporter.Service);
            vResult.Name = vName;
        }
    ;
    
dropRouteStatement returns [DropRouteStatement vResult = FragmentFactory.CreateFragment<DropRouteStatement>()]
{
    Identifier vName;
}
    : tRoute:Identifier vName = identifier
        {
            Match(tRoute,CodeGenerationSupporter.Route);
            vResult.Name = vName;
        }
    ;
    
dropMessageTypeStatement returns [DropMessageTypeStatement vResult = FragmentFactory.CreateFragment<DropMessageTypeStatement>()]
{
    Identifier vName;
}
    : tMessage:Identifier tType:Identifier vName = identifier
        {
            Match(tMessage,CodeGenerationSupporter.Message);
            Match(tType,CodeGenerationSupporter.Type);
            vResult.Name = vName;
        }
    ;
    
dropRemoteServiceBindingStatement returns [DropRemoteServiceBindingStatement vResult = FragmentFactory.CreateFragment<DropRemoteServiceBindingStatement>()]
{
    Identifier vName;
}
    : tRemote:Identifier tService:Identifier tBinding:Identifier vName = identifier
        {
            Match(tRemote,CodeGenerationSupporter.Remote);
            Match(tService,CodeGenerationSupporter.Service);
            Match(tBinding,CodeGenerationSupporter.Binding);
            vResult.Name = vName;
        }
    ;
    
dropEndpointStatement returns [DropEndpointStatement vResult = FragmentFactory.CreateFragment<DropEndpointStatement>()]
{
    Identifier vName;
}
    : tEndpoint:Identifier vName = identifier
        {
            Match(tEndpoint,CodeGenerationSupporter.Endpoint);
            vResult.Name = vName;
        }
    ;
    
dropEventStatement returns [TSqlStatement vResult = null]
    : tEvent:Identifier
        {
            Match(tEvent, CodeGenerationSupporter.Event);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Notification)}?
            vResult = dropEventNotificationStatement        
        |
            {NextTokenMatches(CodeGenerationSupporter.Session)}?
            vResult = dropEventSessionStatement
        )
    ;

    
dropEventNotificationStatement returns [DropEventNotificationStatement vResult = FragmentFactory.CreateFragment<DropEventNotificationStatement>()]
{
    Identifier vEventNotification;
    EventNotificationObjectScope vScope;
}
    : tNotification:Identifier vEventNotification = identifier
        {
            Match(tNotification,CodeGenerationSupporter.Notification);
            AddAndUpdateTokenInfo(vResult, vResult.Notifications,vEventNotification);
        }
        (Comma vEventNotification = identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Notifications,vEventNotification);
            }
        )*
        vScope = eventNotificationObjectScope
        {
            vResult.Scope = vScope;
        }
    ;

dropEventSessionStatement returns [DropEventSessionStatement vResult = this.FragmentFactory.CreateFragment<DropEventSessionStatement>()]
{
    Identifier vIdentifier;
}
    : tSession:Identifier vIdentifier=identifier
        {
            Match(tSession, CodeGenerationSupporter.Session);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        On tServer:Identifier 
        {
            Match(tServer, CodeGenerationSupporter.Server);
        }        
    ;

dropSequenceStatement returns [DropSequenceStatement vResult = FragmentFactory.CreateFragment<DropSequenceStatement>()]
    : tSequence:Identifier dropObjectList[vResult,true]
        {
            Match(tSequence, CodeGenerationSupporter.Sequence);
        }
    ;
    
dropFederationStatement returns [DropFederationStatement vResult = FragmentFactory.CreateFragment<DropFederationStatement>()]
{
    Identifier vIdentifier;
}
    : tFederation:Identifier vIdentifier=identifier
        {
            Match(tFederation, CodeGenerationSupporter.Federation);
            vResult.Name = vIdentifier;
        }
    ;

//////////////////////////////////////////////////////////////
// Drop statements end
//////////////////////////////////////////////////////////////

// Add / Drop SIGNATURE statements
addSignatureStatement returns [AddSignatureStatement vResult = FragmentFactory.CreateFragment<AddSignatureStatement>()]
    : tAdd:Add signatureType[vResult] To signableElement[vResult] By cryptoListWithOptionalPasswordSignature[vResult]
        {
            UpdateTokenInfo(vResult,tAdd);
        }
    ;
    
dropSignatureStatement returns [DropSignatureStatement vResult = FragmentFactory.CreateFragment<DropSignatureStatement>()]
    : signatureType[vResult] From signableElement[vResult] By cryptoListWithOptionalPasswordSignature[vResult]
    ;

signatureType [SignatureStatementBase vParent]
    :    (tCounter:Identifier
            {
                Match(tCounter,CodeGenerationSupporter.Counter);
                vParent.IsCounter = true;
            }
        )? 
        tSignature:Identifier
        {
            Match(tSignature,CodeGenerationSupporter.Signature);
        }
    ;
    
signableElement [SignatureStatementBase vParent]
{
    SchemaObjectName vName;
}
    : vName = schemaObjectThreePartName
        {
            vParent.Element = vName;
        }
    | tKind:Identifier DoubleColon vName = schemaObjectThreePartName
        {
            if (TryMatch(tKind,CodeGenerationSupporter.Object))
                vParent.ElementKind = SignableElementKind.Object;
            else
            {
                Match(tKind,CodeGenerationSupporter.Assembly);
                vParent.ElementKind = SignableElementKind.Assembly;            
            }
            vParent.Element = vName;
        }
    | Database DoubleColon vName =schemaObjectThreePartName
        {
            vParent.ElementKind = SignableElementKind.Database;
            vParent.Element = vName;
        }
    ;
    
cryptoListWithOptionalPasswordSignature[SignatureStatementBase vParent]
{
    CryptoMechanism vCrypto;
}
    : vCrypto = cryptoWithOptionalPasswordSignature 
        {
            AddAndUpdateTokenInfo(vParent, vParent.Cryptos,vCrypto);
        }
        (Comma vCrypto = cryptoWithOptionalPasswordSignature
            {
                AddAndUpdateTokenInfo(vParent, vParent.Cryptos,vCrypto);
            }
        )*
    ;
    
cryptoWithOptionalPasswordSignature returns [CryptoMechanism vResult]
{
    Literal vWith;
}
    : vResult = certificateCrypto vWith = withSignatureOrPasswordOpt
            {
                if (vWith != null)
                    vResult.PasswordOrSignature = vWith;
            }
    | vResult = keyCrypto vWith = withSignatureOrPasswordOpt
            {
                if (vWith != null)
                    vResult.PasswordOrSignature = vWith;
            }
    | vResult = passwordCrypto
    ;
    
withSignatureOrPasswordOpt returns [Literal vResult = null]
    : (options { greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        With tPasswordSignature:Identifier EqualsSign 
        (
            vResult = stringLiteral 
        | 
            vResult = binary
        )
      )?
    ;

// Add / Drop SIGNATURE statments end

labelStatement returns [LabelStatement vResult = this.FragmentFactory.CreateFragment<LabelStatement>()]
    : tLabel:Label 
        {
            UpdateTokenInfo(vResult,tLabel);
            vResult.Value = tLabel.getText();
        }
    ;

gotoStatement returns [GoToStatement vResult = FragmentFactory.CreateFragment<GoToStatement>()]
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
        {NextTokenMatches(CodeGenerationSupporter.Try, 2)}?
        vResult=tryCatchStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Conversation, 2)}?
        vResult = beginConversationTimerStatement
    |    {NextTokenMatches(CodeGenerationSupporter.Dialog, 2)}?
        vResult = beginDialogStatement
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
{
    OptionState vValue;
}
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
        (
            tWith:With LeftParenthesis tDelayedDurability:Identifier EqualsSign vValue = optionOnOff[vResult] tRParen:RightParenthesis
            {
                Match(tDelayedDurability, CodeGenerationSupporter.DelayedDurability);
                vResult.DelayedDurabilityOption = vValue;
                UpdateTokenInfo(vResult, tRParen);
            }
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
        (   // Greedy due to conflict with identifierStatements
            options {greedy=true;} :  
            transactionName[vResult]
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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

tryCatchStatement returns [TryCatchStatement vResult = this.FragmentFactory.CreateFragment<TryCatchStatement>()]
{
    StatementList vStatementList;
    bool vParseErrorOccurred = false;
}
    :
        tBegin1:Begin tTry1:Identifier
        {
            UpdateTokenInfo(vResult,tBegin1);
            Match(tTry1, CodeGenerationSupporter.Try);
        }
        vStatementList = tryStatementList[ref vParseErrorOccurred]
        {
            vResult.TryStatements = vStatementList;
        }
        End tTry2:Identifier
        {
            Match(tTry2, CodeGenerationSupporter.Try);
        }
        tBegin2:Begin tCatch1:Identifier
        {
            Match(tCatch1, CodeGenerationSupporter.Catch);
        }
        vStatementList = catchStatementList[ref vParseErrorOccurred]
        {
            vResult.CatchStatements = vStatementList;
        }
        End tCatch2:Identifier
        {
            Match(tCatch2, CodeGenerationSupporter.Catch);
            UpdateTokenInfo(vResult,tCatch2);
            // Do not return a partial AST if there was a parse error.
            if (vParseErrorOccurred == true)
                vResult = null;
        }
    ;
    
tryStatementList [ref bool vParseErrorOccurred] returns [StatementList vResult = FragmentFactory.CreateFragment<StatementList>()]
{
    TSqlStatement vStatement;
}
    :
        (Semicolon)*
        (    options {generateAmbigWarnings=false;} :
            {IsStatementIsNext()}?
            vStatement=statementOptSemi
            {
                if (null != vStatement) // statement can be null if there was a parse error.
                    AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
                else 
                {
                    vParseErrorOccurred = true;
                    ThrowIfEndOfFileOrBatch();
                }
            }
        )+
    ;

// Slightly differs from tryStatementList - can be empty
catchStatementList [ref bool vParseErrorOccurred] returns [StatementList vResult = FragmentFactory.CreateFragment<StatementList>()]
{
    TSqlStatement vStatement;
}
    :
        (Semicolon)*
        (    options {generateAmbigWarnings=false;} :
            {IsStatementIsNext()}?
            vStatement=statementOptSemi
            {
                if (null != vStatement) // statement can be null if there was a parse error.
                    AddAndUpdateTokenInfo(vResult, vResult.Statements, vStatement);
                else 
                {
                    vParseErrorOccurred = true;
                    ThrowIfEndOfFileOrBatch();
                }
            }
        )*
    ;

beginEndBlockStatement returns [BeginEndBlockStatement vResult = this.FragmentFactory.CreateFragment<BeginEndBlockStatement>()]
{
    TSqlStatement vStatement;
    bool vParseErrorOccurred = false;
    StatementList vStatementList = FragmentFactory.CreateFragment<StatementList>();
    BeginEndAtomicBlockStatement vBeginEndAtomicBlockStatement;
}
    :
        tBegin:Begin 
        {
            UpdateTokenInfo(vResult,tBegin);
        }        
        (
            options {greedy = true; } :
            vBeginEndAtomicBlockStatement=beginEndAtomicBlock
            {
                vResult = vBeginEndAtomicBlockStatement;
                UpdateTokenInfo(vResult,tBegin);
            }
        )?        
        (tSemi:Semicolon)*
        (    options {generateAmbigWarnings=false;} :
            {IsStatementIsNext()}?
            vStatement=statementOptSemi
            {
                if (null != vStatement) // statement can be null if there was a parse error.
                    vStatementList.Statements.Add(vStatement);
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
            if (vParseErrorOccurred == true)
                vResult = null;
        }
    ;

beginEndAtomicBlock returns [BeginEndAtomicBlockStatement vResult = this.FragmentFactory.CreateFragment<BeginEndAtomicBlockStatement>()]
   :    
        tAtomic:Identifier atomicBlockOptions[vResult]
        {    
            Match(tAtomic, CodeGenerationSupporter.Atomic);             
        }
    ;

atomicBlockOptions [BeginEndAtomicBlockStatement vParent]
{
    AtomicBlockOption vOption;
    long encounteredOptions=0;
}
    : With LeftParenthesis vOption=atomicBlockOption
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
            AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
        }
        (
            Comma vOption=atomicBlockOption
            {
                CheckOptionDuplication(ref encounteredOptions, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vParent, vParent.Options, vOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

atomicBlockOption returns [AtomicBlockOption vResult]
    : 
        {NextTokenMatches(CodeGenerationSupporter.DateFirst)}?
        vResult=dateFirstOption
      |
        {NextTokenMatches(CodeGenerationSupporter.DateFormat)}?
        vResult=dateFormatOption      
      |
        {NextTokenMatches(CodeGenerationSupporter.DelayedDurability)}?        
        vResult=delayedDurabilityOption
      |  
        {NextTokenMatches(CodeGenerationSupporter.Language)}?        
        vResult=languageOption
      |
        vResult=isolationLevelIdentifierAtomicBlockOption
    ;

dateFirstOption returns [LiteralAtomicBlockOption vResult=FragmentFactory.CreateFragment<LiteralAtomicBlockOption>();]
{
    IntegerLiteral vLiteral;
}
    : tOption:Identifier EqualsSign vLiteral=integer
        {
            Match(tOption, CodeGenerationSupporter.DateFirst);
            vResult.OptionKind=AtomicBlockOptionKind.DateFirst;
            vResult.Value=vLiteral;
        }
    ;

dateFormatOption returns [LiteralAtomicBlockOption vResult=FragmentFactory.CreateFragment<LiteralAtomicBlockOption>();]
{
    StringLiteral vLiteral;
}
    : tOption:Identifier EqualsSign vLiteral=stringLiteral
        {
            Match(tOption, CodeGenerationSupporter.DateFormat);
            vResult.OptionKind=AtomicBlockOptionKind.DateFormat;
            vResult.Value=vLiteral;
        }
    ;

delayedDurabilityOption returns [OnOffAtomicBlockOption vResult=FragmentFactory.CreateFragment<OnOffAtomicBlockOption>()]
{
    OptionState vOptionState;
}
    : tOption:Identifier EqualsSign vOptionState=optionOnOff[vResult]
        {
            Match(tOption, CodeGenerationSupporter.DelayedDurability);            
            vResult.OptionKind=AtomicBlockOptionKind.DelayedDurability;
            vResult.OptionState = vOptionState;
        }
    ;

isolationLevelIdentifierAtomicBlockOption returns [IdentifierAtomicBlockOption vResult=FragmentFactory.CreateFragment<IdentifierAtomicBlockOption>();]
{
    IsolationLevel vIsolationLevel;
}
    : Transaction tIsolation:Identifier tLevel:Identifier EqualsSign vIsolationLevel=isolationLevel[vResult]
        {            
            Match(tIsolation, CodeGenerationSupporter.Isolation);
            Match(tLevel, CodeGenerationSupporter.Level);
            vResult.OptionKind=AtomicBlockOptionKind.IsolationLevel;
            vResult.Value=new Identifier();
                        
            CultureInfo culture = CultureInfo.InvariantCulture;
            switch(vIsolationLevel)
            {
                case IsolationLevel.ReadCommitted:
                    vResult.Value.Value = string.Format(culture, "{0} {1}", CodeGenerationSupporter.Read, CodeGenerationSupporter.Committed);
                    break;
                case IsolationLevel.ReadUncommitted:
                    vResult.Value.Value = string.Format(culture, "{0} {1}", CodeGenerationSupporter.Read, CodeGenerationSupporter.Uncommitted);
                    break;
                case IsolationLevel.RepeatableRead:
                    vResult.Value.Value = string.Format(culture, "{0} {1}", CodeGenerationSupporter.Repeatable, CodeGenerationSupporter.Read);
                    break;
                case IsolationLevel.Serializable:
                    vResult.Value.Value = CodeGenerationSupporter.Serializable;
                    break;
                case IsolationLevel.Snapshot:
                    vResult.Value.Value = CodeGenerationSupporter.Snapshot;
                    break;
            }            
        }
    ;

languageOption returns [LiteralAtomicBlockOption vResult=FragmentFactory.CreateFragment<LiteralAtomicBlockOption>();]
{
    StringLiteral vLiteral;
}
    : tOption:Identifier EqualsSign vLiteral=stringLiteral
        {
            Match(tOption, CodeGenerationSupporter.Language);
            vResult.OptionKind=AtomicBlockOptionKind.Language;
            vResult.Value=vLiteral;            
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
    ScalarExpression vExpression;
    NullableConstraintDefinition vNullableConstraintDefinition;
}
    : vIdentifier=identifierVariable 
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
        (
            vNullableConstraintDefinition=nullableConstraint
            {
                vResult.Nullable=vNullableConstraintDefinition;
            }
        )?
        (EqualsSign vExpression = expression
            {
                vResult.Value = vExpression;
            }
        )?
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
    VariableReference vVariable;
    Identifier vIdentifier;
    ScalarExpression vExpression;
    CursorDefinition vCursorDef;
    AssignmentKind vAssignmentKind;
}
    : vVariable=variable
        {
            vResult.Variable = vVariable;
        }
        (
            (
                Dot
                {
                    vResult.SeparatorType = SeparatorType.Dot;
                }
            |
                DoubleColon
                {
                    vResult.SeparatorType = SeparatorType.DoubleColon;
                }
            )
            vIdentifier=identifier
            {
                vResult.Identifier = vIdentifier;
            }
            (
                parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
                {
                    vResult.FunctionCallExists = true;
                }
            |
                vAssignmentKind = assignmentWithOptOp vExpression=expression
                {
                    vResult.Expression = vExpression;
                    vResult.AssignmentKind = vAssignmentKind;
                }
            )
        |
            (
                vAssignmentKind = assignmentWithOptOp vExpression=expression
                {
                    vResult.Expression = vExpression;
                    vResult.AssignmentKind = vAssignmentKind;
                }
            |
                EqualsSign vCursorDef = cursorDefinition
                {
                    vResult.CursorDefinition = vCursorDef;
                }
            )
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
{
    SetOptions option;
}
    : tOption:Identifier 
        {
            option = PredicateSetOptionsHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
            
            vResult.Options = option;
        }
        (Comma tOption2:Identifier
            {
                option = PredicateSetOptionsHelper.Instance.ParseOption(tOption2, SqlVersionFlags.TSql120);
                
                vResult.Options |= option;
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
            AddAndUpdateTokenInfo(vResult, vResult.Commands,vCommand);
        }
        (Comma vCommand = setCommand
            {
                AddAndUpdateTokenInfo(vResult, vResult.Commands,vCommand);
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
{
    IsolationLevel vIsolationLevel;
}
    : (Transaction | Tran) tIsolation:Identifier tLevel:Identifier vIsolationLevel=isolationLevel[vResult]
        {
            Match(tIsolation,CodeGenerationSupporter.Isolation);
            Match(tLevel,CodeGenerationSupporter.Level);
            vResult.Level=vIsolationLevel;            
        }
    ;

isolationLevel [TSqlFragment vParent] returns [IsolationLevel vResult=IsolationLevel.ReadCommitted]
    :
    (
        (Read tCommittedUncommitted:Identifier
            {
                if (TryMatch(tCommittedUncommitted,CodeGenerationSupporter.Committed))
                    vResult = IsolationLevel.ReadCommitted;
                else
                {
                    Match(tCommittedUncommitted, CodeGenerationSupporter.Uncommitted);
                    vResult = IsolationLevel.ReadUncommitted;
                }
                UpdateTokenInfo(vParent,tCommittedUncommitted);
            }
        )
        |
        (tRepeatable:Identifier tRead:Read
            {
                Match(tRepeatable, CodeGenerationSupporter.Repeatable);
                vResult = IsolationLevel.RepeatableRead;
                UpdateTokenInfo(vParent,tRead);
            }
        )
        |
        (tSerializableSnapshot:Identifier
            {
                if (TryMatch(tSerializableSnapshot, CodeGenerationSupporter.Snapshot))
                    vResult = IsolationLevel.Snapshot;
                else
                {
                    Match(tSerializableSnapshot, CodeGenerationSupporter.Serializable);
                    vResult = IsolationLevel.Serializable;
                }
                UpdateTokenInfo(vParent,tSerializableSnapshot);          
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
            vResult.Options = SetStatisticsOptionsHelper.Instance.ParseOption(tStatOption, SqlVersionFlags.TSql120);
        }
        (Comma tStatOption2:Identifier
            {
                vResult.Options |= SetStatisticsOptionsHelper.Instance.ParseOption(tStatOption2, SqlVersionFlags.TSql120);
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
    Identifier vIdentifier;
    bool vAsDefined = false;
}
    :    vIdentifier=identifierVariable 
        (
            As
            {
                vAsDefined = true;
            }
        )? 
        Table vResult=declareTableBodyMain[statementType]
        {
            vResult.VariableName = vIdentifier;
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
    : Cursor cursorOpts[false, vOptions] For vSelect=selectStatement[SubDmlFlags.SelectNotForInsert]
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
            UpdateTokenInfo(vResult, tOption);
        }
    ;

createIndexStatement returns [TSqlStatement vResult = null]
{
    bool? isClustered = null;
}
    :     tCreate:Create
        (
            tUnique:Unique
        )?
        (
            (
                (
                    Clustered
                    {
                        isClustered = true;
                    }
                |   NonClustered
                    {
                        isClustered = false;
                    }
                )
                (
                    vResult=createRelationalIndexStatement[tUnique, isClustered]
                    | vResult=createColumnStoreIndexStatement[tUnique, isClustered]
                )
            )
        | 
            vResult=createRelationalIndexStatement[tUnique, isClustered]
            //CREATE COLUMNSTORE INDEX is handled in create2005Statements
        )
        {
            UpdateTokenInfo(vResult, tCreate);
        }
    ;

createRelationalIndexStatement[IToken tUnique, bool? isClustered] returns [CreateIndexStatement vResult = FragmentFactory.CreateFragment<CreateIndexStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    ColumnWithSortOrder vColumnWithSortOrder;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
    BooleanExpression vExpression;

    vResult.Unique=(tUnique != null);
    vResult.Clustered = isClustered;
}
    :   Index vIdentifier=identifier On vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.Name = vIdentifier;
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
            // We need this to resolve the ambiguity with the statements that start with an Identifier
            {NextTokenMatches(CodeGenerationSupporter.Include)}?
            tInclude:Identifier 
            identifierColumnList[vResult, vResult.IncludeColumns]
        )?
        (vExpression = filterClause[vResult.Clustered ?? false]
            {
                vResult.FilterPredicate = vExpression;
            }
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
                options {greedy = true; } :
            With 
            (
                indexLegacyOptionList[vResult]
                {
                    vResult.Translated80SyntaxTo90 = true;
                }
            |
                indexOptionList[IndexAffectingStatement.CreateIndex, vResult.IndexOptions, vResult]
            )
        )?
        (
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        fileStreamOnOpt[vResult]
    ;

createColumnStoreIndexStatement [IToken tUnique, bool? isClustered] returns [CreateColumnStoreIndexStatement vResult = FragmentFactory.CreateFragment<CreateColumnStoreIndexStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;    
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;    
    if (tUnique != null)
    {
        ThrowIncorrectSyntaxErrorException(tUnique);
    }
    vResult.Clustered=isClustered;
}    
        : tColumnStore:Identifier tIndex:Index vIdentifier=identifier
        {            
            Match(tColumnStore, CodeGenerationSupporter.ColumnStore);
            vResult.Name = vIdentifier;
        }
        tOn:On vSchemaObjectName = schemaObjectFourPartName
        {
            vResult.OnName = vSchemaObjectName;
        }
        (    options {greedy = true; } :
            identifierColumnList[vResult, vResult.Columns]
            {
                if(isClustered.HasValue && isClustered==true && vResult.Columns.Count > 0)
                {
                    ThrowIncorrectSyntaxErrorException(vResult.Columns[0]);
                }
            }
        )?
    {
        if ((!isClustered.HasValue || isClustered==false) && vResult.Columns.Count == 0)
        {
        ThrowIncorrectSyntaxErrorException(vResult.OnName);
        }
    }
        (            
            options {greedy = true; } :
            With 
            (                
                indexOptionList[IndexAffectingStatement.CreateColumnStoreIndex, vResult.IndexOptions, vResult]
            )            
        )?
        (
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?        
    ;    
    
filterClause [bool filterProhibited] returns [BooleanExpression vResult]
    : tWhere:Where vResult = filterExpression
        {
            if (filterProhibited)
            {
                ThrowIncorrectSyntaxErrorException(tWhere);
            }
        }
    ;

filterExpressionPrimary returns [BooleanExpression vResult]
{
    ColumnReferenceExpression vExpression;
}
    :
        (
            vExpression = filterColumn
            (
                 vResult = filterNullPredicate[vExpression]
                | vResult = filterComparisonPredicate[vExpression]
                | vResult = filterInPredicate[vExpression]
            )
        | vResult = filterParenthesisExpression
        )
    ;

filterExpression returns [BooleanExpression vResult]
{
    BooleanExpression vSecond;
}
    :    vResult = filterExpressionPrimary 
        (And vSecond = filterExpression
            {
                BooleanBinaryExpression binaryExpr = FragmentFactory.CreateFragment<BooleanBinaryExpression>();
                binaryExpr.BinaryExpressionType = BooleanBinaryExpressionType.And;
                binaryExpr.SecondExpression = vSecond;
                binaryExpr.FirstExpression = vResult;
                vResult = binaryExpr;
            }
        )?
    ;
    
filterColumn returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :    (
            specialColumn[vResult]
        |             
            vMultiPartIdentifier=multiPartIdentifier[-1]
            {
                vResult.MultiPartIdentifier = vMultiPartIdentifier;
            }
            (
                Dot specialColumn[vResult]
            )?            
        )
        {     
            CheckSpecialColumn(vResult);
        }        
    ;

filterNullPredicate [ScalarExpression vColumn] returns [BooleanIsNullExpression vResult]
    : vResult = nullPredicate[vColumn]
    ;
    
filterComparisonPredicate[ScalarExpression vColumn] returns [BooleanComparisonExpression vResult = FragmentFactory.CreateFragment<BooleanComparisonExpression>()]
{
    ScalarExpression vRightExpr;
    BooleanComparisonType vType;
}
    :   vType = comparisonOperator vRightExpr = expression
        {
            CheckComparisonOperandForIndexFilter(vRightExpr, true);
            vResult.FirstExpression = vColumn;
            vResult.SecondExpression = vRightExpr;
            vResult.ComparisonType = vType;
        }
    ;

filterInPredicate[ScalarExpression vColumn] returns [InPredicate vResult = FragmentFactory.CreateFragment<InPredicate>()]
{
    ScalarExpression vExpression;
}
    :   In LeftParenthesis vExpression = expression
        {
            vResult.Expression = vColumn;
            CheckComparisonOperandForIndexFilter(vExpression, true);
            AddAndUpdateTokenInfo(vResult, vResult.Values, vExpression);
        }
        (Comma vExpression = expression
            {
                CheckComparisonOperandForIndexFilter(vExpression, true);
                AddAndUpdateTokenInfo(vResult, vResult.Values, vExpression);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

filterParenthesisExpression returns [BooleanParenthesisExpression vResult = FragmentFactory.CreateFragment<BooleanParenthesisExpression>()]
{
    BooleanExpression vInnerExpression;
}
    : tLParen:LeftParenthesis vInnerExpression = filterExpression tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tLParen);
            vResult.Expression = vInnerExpression;
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

filegroupOrPartitionScheme returns [FileGroupOrPartitionScheme vResult = this.FragmentFactory.CreateFragment<FileGroupOrPartitionScheme>()]
{
    IdentifierOrValueExpression vTSqlFragment;
}
    :   vTSqlFragment=stringOrIdentifier
        {
            vResult.Name = vTSqlFragment;
        }
        (    
            options { greedy = true; } :
            columnNameList[vResult, vResult.PartitionSchemeColumns]
        )?
    ;

indexLegacyOptionList[CreateIndexStatement vParent]
{
    IndexOption vIndexOption;
}
    :    vIndexOption=indexLegacyOption
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
    :    tFillFactor:FillFactor EqualsSign vValue=integer
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
            IndexStateOption vOption = FragmentFactory.CreateFragment<IndexStateOption>();
            vResult = vOption;
            vOption.OptionKind = ParseIndexLegacyWithOption(tId2);
            UpdateTokenInfo(vOption, tId2);
            vOption.OptionState = OptionState.On;
        }
    ;

indexOptionList[IndexAffectingStatement statement, IList<IndexOption> optionsList, TSqlFragment vParent]
{
    IndexOption vIndexOption;
}
    :
        LeftParenthesis vIndexOption=indexOption
        {
            VerifyAllowedIndexOption120(statement, vIndexOption);
            optionsList.Add(vIndexOption);
        }
        (
            Comma vIndexOption=indexOption
            {
                VerifyAllowedIndexOption120(statement, vIndexOption);
                optionsList.Add(vIndexOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

maxDopOption returns [IndexExpressionOption vResult = this.FragmentFactory.CreateFragment<IndexExpressionOption>()]
{
    Literal vLiteral;
}    
    :    tMaxDop:Identifier EqualsSign vLiteral=integer
        {
            ThrowIfMaxdopValueOutOfRange(vLiteral);
            Match(tMaxDop,CodeGenerationSupporter.MaxDop);
            UpdateTokenInfo(vResult, tMaxDop);
            vResult.OptionKind = IndexOptionKind.MaxDop;
            vResult.Expression = vLiteral;
        }
    ;


onlineIndexOption returns [OnlineIndexOption vResult = this.FragmentFactory.CreateFragment<OnlineIndexOption>()]
{
    OnlineIndexLowPriorityLockWaitOption vLowPriorityLockWaitOption = FragmentFactory.CreateFragment<OnlineIndexLowPriorityLockWaitOption>();
}
    :    tOption:Identifier 
        {
            Match(tOption, CodeGenerationSupporter.Online);
            vResult.OptionKind = IndexOptionKind.Online;
            UpdateTokenInfo(vResult, tOption);
        }
        EqualsSign 
        (
            tOn:On
                {
                    vResult.OptionState = OptionState.On;
                    UpdateTokenInfo(vResult, tOn);
                }
                // optional MLP suboption
                (   
                    LeftParenthesis lowPriorityLockWaitOption[vLowPriorityLockWaitOption.Options, vLowPriorityLockWaitOption]
                    {
                        vResult.LowPriorityLockWaitOption = vLowPriorityLockWaitOption;
                    }
                    tRParen:RightParenthesis
                    {
                        UpdateTokenInfo(vResult, tRParen);
                    }
                )?
          | tOff:Off
                {
                    vResult.OptionState = OptionState.Off;
                    UpdateTokenInfo(vResult, tOff);
                }
        )
    ;

lowPriorityLockWaitOption[IList<LowPriorityLockWaitOption> optionsList, TSqlFragment vParent]
{
    IntegerLiteral vMaxDuration;
    LowPriorityLockWaitMaxDurationOption vMaxDurationOption = FragmentFactory.CreateFragment<LowPriorityLockWaitMaxDurationOption>();
    LowPriorityLockWaitAbortAfterWaitOption vAbortAfterWaitOption = FragmentFactory.CreateFragment<LowPriorityLockWaitAbortAfterWaitOption>();
}
    :   tWaitAtLowPriority:Identifier
        {
            Match(tWaitAtLowPriority, CodeGenerationSupporter.WaitAtLowPriority);
            UpdateTokenInfo(vParent, tWaitAtLowPriority);
        }
        LeftParenthesis tMaxDuration:Identifier EqualsSign vMaxDuration=integer
        {
            Match(tMaxDuration, CodeGenerationSupporter.MaxDuration);
            UpdateTokenInfo(vMaxDurationOption, tMaxDuration);
            vMaxDurationOption.OptionKind = LowPriorityLockWaitOptionKind.MaxDuration;
            vMaxDurationOption.MaxDuration = vMaxDuration;
            optionsList.Add(vMaxDurationOption);
        }
        (   tMinutes:Identifier
            {
                Match(tMinutes, CodeGenerationSupporter.Minutes);
                vMaxDurationOption.Unit = TimeUnit.Minutes;
                UpdateTokenInfo(vMaxDurationOption, tMinutes);
            }
        )?
        Comma tAbortAfterWait:Identifier EqualsSign tAbortAfterWaitValue:Identifier
        {
            Match(tAbortAfterWait, CodeGenerationSupporter.AbortAfterWait);
            UpdateTokenInfo(vAbortAfterWaitOption, tAbortAfterWait);
            AbortAfterWaitType vAbortAfterWait = AbortAfterWaitTypeHelper.Instance.ParseOption(tAbortAfterWaitValue);
            CheckLowPriorityLockWaitValue(vMaxDuration, vAbortAfterWait);
            vAbortAfterWaitOption.OptionKind = LowPriorityLockWaitOptionKind.AbortAfterWait;
            vAbortAfterWaitOption.AbortAfterWait = vAbortAfterWait;
            optionsList.Add(vAbortAfterWaitOption);
            UpdateTokenInfo(vAbortAfterWaitOption, tAbortAfterWaitValue);
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

indexStateOption returns [IndexStateOption vResult = FragmentFactory.CreateFragment<IndexStateOption>()]
{
    OptionState vOptionState;
}
    :    tOption:Identifier 
        {
            vResult.OptionKind = IndexOptionHelper.Instance.ParseOption(tOption, SqlVersionFlags.TSql120);
            UpdateTokenInfo(vResult, tOption);
        }
        EqualsSign vOptionState=optionOnOff[vResult]
        {
            vResult.OptionState = vOptionState;
        }
    ;

bucketCountOption returns [IndexExpressionOption vResult = this.FragmentFactory.CreateFragment<IndexExpressionOption>()]
{
    Literal vValue;
}    
    :    tBucketCount:Identifier EqualsSign vValue=integer
        {            
            Match(tBucketCount, CodeGenerationSupporter.BucketCount);
            vResult.OptionKind = IndexOptionKind.BucketCount;
            vResult.Expression = vValue;
        }
    ;

indexOption returns [IndexOption vResult = null]
    :
        vResult=fillFactorOption
    |
        {NextTokenMatches(CodeGenerationSupporter.MaxDop)}?
        vResult=maxDopOption
    |
        {NextTokenMatches(CodeGenerationSupporter.DataCompression)}?
        vResult=dataCompressionOption
    |
        {NextTokenMatches(CodeGenerationSupporter.BucketCount)}?
        vResult=bucketCountOption
    |
        {NextTokenMatches(CodeGenerationSupporter.Online)}?
        vResult=onlineIndexOption
    |
        vResult=indexStateOption
    ;

withCommonTableExpressionsAndXmlNamespaces returns [WithCtesAndXmlNamespaces vResult = this.FragmentFactory.CreateFragment<WithCtesAndXmlNamespaces>()]
{
    XmlNamespaces vXmlNamespaces;
    CommonTableExpression vCommonTableExpression;
}
    :
        tWith:With
        {
            UpdateTokenInfo(vResult,tWith);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.XmlNamespaces)}?
            vXmlNamespaces=xmlNamespaces
            {
                vResult.XmlNamespaces = vXmlNamespaces;
            }  
        |
            // TODO, insivara: xmlnamespaces will be allowed to follow CHANGE_TRACKING_CONTEXT once SQL Server bug is fixed
            {NextTokenMatches(CodeGenerationSupporter.ChangeTrackingContext)}?
            ctContext[vResult]              
        |
            vCommonTableExpression=commonTableExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.CommonTableExpressions, vCommonTableExpression);
            }                  
        )
        (
            Comma vCommonTableExpression=commonTableExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.CommonTableExpressions, vCommonTableExpression);
            }
        )*
    ;

ctContext [WithCtesAndXmlNamespaces vParent]
{
    ValueExpression vLiteral;
}
    :
        tChangeTrackingContext:Identifier tLParen:LeftParenthesis vLiteral=binaryOrVariable tRParen:RightParenthesis
        {
            Match(tChangeTrackingContext, CodeGenerationSupporter.ChangeTrackingContext);
            vParent.ChangeTrackingContext = vLiteral;
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

commonTableExpression returns [CommonTableExpression vResult = this.FragmentFactory.CreateFragment<CommonTableExpression>()]
{
    Identifier vIdentifier;
    QueryExpression vQueryExpression;
}
    :   vIdentifier=identifier
        {
            vResult.ExpressionName = vIdentifier;
        }
        (columnNameList[vResult, vResult.Columns])?
        As tLParen:LeftParenthesis vQueryExpression=subqueryExpression[SubDmlFlags.SelectNotForInsert] tRParen:RightParenthesis
        {
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tLParen);
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

xmlNamespaces returns [XmlNamespaces vResult = this.FragmentFactory.CreateFragment<XmlNamespaces>()]
{
    XmlNamespacesElement vXmlNamespacesElement;
}
    :   tXmlNamespaces:Identifier
        {
            Match(tXmlNamespaces, CodeGenerationSupporter.XmlNamespaces);
        }
        LeftParenthesis vXmlNamespacesElement=xmlNamespacesElement
        {
            AddAndUpdateTokenInfo(vResult, vResult.XmlNamespacesElements, vXmlNamespacesElement);
        }
        (
            Comma vXmlNamespacesElement=xmlNamespacesElement
            {   
                AddAndUpdateTokenInfo(vResult, vResult.XmlNamespacesElements, vXmlNamespacesElement);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

xmlNamespacesElement returns [XmlNamespacesElement vResult = null]
    :
        vResult=xmlNamespacesDefaultElement
    |
        vResult=xmlNamespacesAliasElement
    ;

xmlNamespacesDefaultElement returns [XmlNamespacesDefaultElement vResult = this.FragmentFactory.CreateFragment<XmlNamespacesDefaultElement>()]
{
    StringLiteral vLiteral;
}
    :   tDefault:Default vLiteral=stringLiteral
        {
            UpdateTokenInfo(vResult,tDefault);
            vResult.String = vLiteral;
        }
    ;

xmlNamespacesAliasElement returns [XmlNamespacesAliasElement vResult = this.FragmentFactory.CreateFragment<XmlNamespacesAliasElement>()]
{
    StringLiteral vLiteral;
    Identifier vIdentifier;
}
    :
        vLiteral=stringLiteral
        {
            vResult.String = vLiteral;
        }
        As
        vIdentifier=identifier
        {
            vResult.Identifier = vIdentifier;
        }
    ;
    
subqueryExpressionWithOptionalCTE returns [SelectStatement vResult = FragmentFactory.CreateFragment<SelectStatement>()]
{
    WithCtesAndXmlNamespaces vWithCommonTableExpressionsAndXmlNamespaces;
    QueryExpression vQueryExpression;
}
    :
        (
            vWithCommonTableExpressionsAndXmlNamespaces=withCommonTableExpressionsAndXmlNamespaces
            {
                vResult.WithCtesAndXmlNamespaces = vWithCommonTableExpressionsAndXmlNamespaces;
            }
        )?
        vQueryExpression=subqueryExpression[SubDmlFlags.SelectNotForInsert]
        {
            vResult.QueryExpression = vQueryExpression;
        }
    ; 

selectStatement [SubDmlFlags subDmlFlags] returns [SelectStatement vResult = null]
{
    WithCtesAndXmlNamespaces vWithCTE = null;
}
    :
        (vWithCTE=withCommonTableExpressionsAndXmlNamespaces)?
        vResult=select[subDmlFlags]
        {
            vResult.WithCtesAndXmlNamespaces = vWithCTE;
        }
    ;

select [SubDmlFlags subDmlFlags] returns [SelectStatement vResult = this.FragmentFactory.CreateFragment<SelectStatement>()]
{
    QueryExpression vQueryExpression;
    OrderByClause vOrderByClause;
    OffsetClause vOffsetClause;
    ForClause vForClause;
}
    :   vQueryExpression=queryExpression[subDmlFlags, vResult]
        (
            vOrderByClause=orderByClause
            {
                vQueryExpression.OrderByClause = vOrderByClause;
            }
            (
                // Can end with Identifier - and that conflicts with identifierStatements, so making it greedy...
                options {greedy=true;} :
                vOffsetClause = offsetClause
                {
                    vQueryExpression.OffsetClause = vOffsetClause;
                }
            )?
        )?
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

derivedTable [SubDmlFlags subDmlFlags] returns [TableReferenceWithAliasAndColumns vResult]
    :   
        (
            vResult = queryDerivedTable[subDmlFlags]
        |
            vResult = inlinedDerivedTable
        )
        simpleTableReferenceAlias[vResult]
        (
            options {greedy=true;} :
            columnNameList[vResult, vResult.Columns]
        )?
    ;
    
queryDerivedTable [SubDmlFlags subDmlFlags] returns [QueryDerivedTable vResult = FragmentFactory.CreateFragment<QueryDerivedTable>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=subqueryExpression[subDmlFlags] tRParen:RightParenthesis
        {
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tLParen);
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

inlinedDerivedTable returns [InlineDerivedTable vResult = FragmentFactory.CreateFragment<InlineDerivedTable>()]
{
    RowValue vRowValue;
}
    :   tLParen:LeftParenthesis Values vRowValue = rowValueExpression 
        {
            UpdateTokenInfo(vResult, tLParen);
            AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
        }
        (Comma vRowValue = rowValueExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

subquery [SubDmlFlags subDmlFlags, ExpressionFlags expressionFlags] returns [ScalarSubquery vResult = this.FragmentFactory.CreateFragment<ScalarSubquery>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=subqueryExpression[subDmlFlags] tRParen:RightParenthesis
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

subqueryExpression [SubDmlFlags subDmlFlags] returns [QueryExpression vResult = null]
{
    BinaryQueryExpression vBinaryQueryExpression = null;
}
    :
        vResult=subqueryExpressionUnit[subDmlFlags]
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
            vResult=subqueryExpressionUnit[subDmlFlags]
            {
                vBinaryQueryExpression.SecondQueryExpression = vResult;
                vResult = vBinaryQueryExpression;
            }
        )*
    ;

subqueryExpressionUnit [SubDmlFlags subDmlFlags] returns [QueryExpression vResult]
    :
        vResult=subquerySpecification[subDmlFlags]
    |   
        vResult=subqueryParenthesis[subDmlFlags]
    ;

subqueryParenthesis [SubDmlFlags subDmlFlags] returns [QueryParenthesisExpression vResult = this.FragmentFactory.CreateFragment<QueryParenthesisExpression>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=subqueryExpression[subDmlFlags] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

queryExpression [SubDmlFlags subDmlFlags, SelectStatement vSelectStatement] returns [QueryExpression vResult = null]
{
    BinaryQueryExpression vBinaryQueryExpression = null;
}
    :   vResult=queryExpressionUnit[subDmlFlags, vSelectStatement]
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
            vResult=queryExpressionUnit[subDmlFlags, null]
            {
                vBinaryQueryExpression.SecondQueryExpression = vResult;
                vResult = vBinaryQueryExpression;
            }
        )*
    ;

queryExpressionUnit [SubDmlFlags subDmlFlags, SelectStatement vSelectStatement] returns [QueryExpression vResult = null]
    :
        vResult=querySpecification[subDmlFlags, vSelectStatement]
    |   
        vResult=queryParenthesis[subDmlFlags, vSelectStatement]
    ;

queryParenthesis [SubDmlFlags subDmlFlags, SelectStatement vSelectStatement] returns [QueryParenthesisExpression vResult = FragmentFactory.CreateFragment<QueryParenthesisExpression>()]
{
    QueryExpression vQueryExpression;
}
    :   tLParen:LeftParenthesis vQueryExpression=queryExpression[subDmlFlags, vSelectStatement] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.QueryExpression = vQueryExpression;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

subquerySpecification [SubDmlFlags subDmlFlags] returns [QuerySpecification vResult = FragmentFactory.CreateFragment<QuerySpecification>()]
{
    TopRowFilter vTopRowFilter;
    SelectElement vSelectColumn;
    FromClause vFromClause;
    WhereClause vWhereClause;
    GroupByClause vGroupByClause;
    HavingClause vHavingClause;
    BrowseForClause vBrowseForClause;
    OrderByClause vOrderByClause;
    OffsetClause vOffsetClause;
    XmlForClause vXmlForClause;
}
    :    tSelect:Select
        {
            UpdateTokenInfo(vResult,tSelect);            
        } 
        uniqueRowFilterOpt[vResult]
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
        vFromClause = fromClauseOpt[subDmlFlags]
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
            (
                 vOffsetClause = offsetClause
                {
                    vResult.OffsetClause = vOffsetClause;
                }
             )?
        )?
        (
             {LA(1) == For && LA(2) == Browse}?
            For vBrowseForClause = browseForClause
            {
                vResult.ForClause = vBrowseForClause;
            }
        | /* empty */
        )
        (
            tFor:For vXmlForClause=xmlForClause
            {
                if(vResult.ForClause != null)
                {
                    ThrowIncorrectSyntaxErrorException(tFor);
                }
                vResult.ForClause = vXmlForClause;
            }
        )?
        {
            if (vResult.OrderByClause != null && 
                vResult.TopRowFilter == null && vResult.ForClause == null && vResult.OffsetClause == null)
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

querySpecification [SubDmlFlags subDmlFlags, SelectStatement vSelectStatement] returns [QuerySpecification vResult = this.FragmentFactory.CreateFragment<QuerySpecification>()]
{
    TopRowFilter vTopRowFilter;
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
        uniqueRowFilterOpt[vResult]
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
        vFromClause = fromClauseOpt[subDmlFlags]
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

uniqueRowFilterOpt [QuerySpecification vParent]
{
    IToken token;
    UniqueRowFilter vUniqueRowFilter;
}
    :   (vUniqueRowFilter = uniqueRowFilter[out token]
            {
                vParent.UniqueRowFilter = vUniqueRowFilter;
            }
        )?
    ;

uniqueRowFilter [out IToken token] returns [UniqueRowFilter vResult = UniqueRowFilter.NotSpecified]
{
    token = null;
}
    :   tAll:All
        {
            vResult = UniqueRowFilter.All;
            token = tAll;
        }
    |
        tDistinct:Distinct
        {
            vResult = UniqueRowFilter.Distinct;
            token = tDistinct;
        }
    ;

// This rule corresponds to offset/fetch clause in Sql Server grammar.
offsetClause returns [OffsetClause vResult = this.FragmentFactory.CreateFragment<OffsetClause>()]
{
    ScalarExpression vExpression;
}
    :
        tOffset:Identifier vExpression = expression      
        {
            Match(tOffset, CodeGenerationSupporter.Offset);
            UpdateTokenInfo(vResult,tOffset);
            vResult.OffsetExpression = vExpression;
        }
        tOffsetRowOrRows:Identifier
        {
            Match(tOffsetRowOrRows, CodeGenerationSupporter.Row, CodeGenerationSupporter.Rows);
            UpdateTokenInfo(vResult,tOffsetRowOrRows);
        }
        (
            options {greedy=true;} : //Greedy due to conflict with FETCH statements
            tFetch:Fetch tFirstOrNext:Identifier vExpression = expression
            {
                UpdateTokenInfo(vResult,tFetch);
                Match(tFirstOrNext, CodeGenerationSupporter.First, CodeGenerationSupporter.Next);
                UpdateTokenInfo(vResult,tFirstOrNext);
                vResult.FetchExpression = vExpression;
            }
            tFetchRowOrRows:Identifier tOnly:Identifier
            {
                Match(tFetchRowOrRows, CodeGenerationSupporter.Row, CodeGenerationSupporter.Rows);
                Match(tOnly, CodeGenerationSupporter.Only);
                UpdateTokenInfo(vResult, tOnly);
            }
        )?
    ;

// This rule corresponds to topn in Sql Server grammar.
topRowFilter returns [TopRowFilter vResult = this.FragmentFactory.CreateFragment<TopRowFilter>()]
{
    ScalarExpression vExpression;
}
    :
        tTop:Top
        {
            UpdateTokenInfo(vResult,tTop);
        }
        (
            vExpression=integerOrRealOrNumeric
        |
            vExpression=parenthesisDisambiguatorForExpressions[ExpressionFlags.None]
        )
        {            
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

// This rule corresponds to dml_topn in Sql Server grammar.
dmlTopRowFilter returns [TopRowFilter vResult = this.FragmentFactory.CreateFragment<TopRowFilter>()]
{
    ScalarExpression vExpression;
}
    :   tTop:Top vExpression=parenthesisDisambiguatorForExpressions[ExpressionFlags.None]
        {
            UpdateTokenInfo(vResult,tTop);
            vResult.Expression = vExpression;
        }
        (
            tPercent:Percent
            {
                UpdateTokenInfo(vResult,tPercent);
                vResult.Percent = true;
            }
        )?
    ;

dmlTopRowFilterOpt [DataModificationSpecification vParent]
{
    TopRowFilter vTopRowFilter;
}
    :    (
            vTopRowFilter=dmlTopRowFilter
            {
                vParent.TopRowFilter = vTopRowFilter;
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
        (Variable assignmentWithOptOp)=>
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

outputClauseSelectElement returns [SelectElement vResult]
    :
        (selectStarExpression)=>
        vResult=selectStarExpression
        |
        vResult=outputClauseSelectColumn
    ;

outputClauseSelectColumn returns [SelectScalarExpression vResult = this.FragmentFactory.CreateFragment<SelectScalarExpression>()]
{
    ScalarExpression vExpression;
    IdentifierOrValueExpression vColumnName;
}   :
        vExpression=expression
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
            Comma vExpression=seedIncrement 
            {
                vResult.Seed = vExpression;
            }
            Comma vExpression=seedIncrement 
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
    AssignmentKind vAssignmentKind;
}
    : vLiteral=variable vAssignmentKind = assignmentWithOptOp vExpression=expression
        {
            vResult.Variable = vLiteral;
            vResult.Expression = vExpression;
            vResult.AssignmentKind = vAssignmentKind;
        }
    ;

tableSampleClause returns [TableSampleClause vResult = this.FragmentFactory.CreateFragment<TableSampleClause>()]
{
    ScalarExpression vExpression;
}
    :
        tTableSample:TableSample
        {
            UpdateTokenInfo(vResult,tTableSample);
        }
        (
            tSystem:Identifier
            {
                Match(tSystem, CodeGenerationSupporter.System);
                vResult.System = true;
            }
        )?
        LeftParenthesis
        vExpression=expression
        {
            vResult.SampleNumber = vExpression;
        }
        (
            tRows:Identifier
            {
                Match(tRows, CodeGenerationSupporter.Rows);
                vResult.TableSampleClauseOption = TableSampleClauseOption.Rows;
            }
        |
            Percent
            {
                vResult.TableSampleClauseOption = TableSampleClauseOption.Percent;
            }
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Repeatable)}?
            tRepeatable:Identifier
            LeftParenthesis vExpression=expression
            {
                vResult.RepeatSeed = vExpression;
            }
            tRParen2:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen2);
            }
        |
            /* empty */
        )
    ;

fromClauseOpt [SubDmlFlags subDmlFlags] returns [FromClause vResult = null]
    :   
        (
            vResult = fromClause[subDmlFlags]
        |
        )
    ;

fromClause [SubDmlFlags subDmlFlags] returns [FromClause vResult = this.FragmentFactory.CreateFragment<FromClause>()]
{
    TableReference vTableReference;
}   
    :   
        tFrom:From vTableReference=selectTableReferenceWithOdbc[subDmlFlags]
        {
            UpdateTokenInfo(vResult,tFrom);
            AddAndUpdateTokenInfo(vResult, vResult.TableReferences, vTableReference);
        }
        ( Comma vTableReference=selectTableReferenceWithOdbc[subDmlFlags]
            {
                AddAndUpdateTokenInfo(vResult, vResult.TableReferences, vTableReference);
            }
        )*
    ;

selectTableReferenceWithOdbc [SubDmlFlags subDmlFlags] returns [TableReference vResult]
    :
        vResult=selectTableReference[subDmlFlags]
    |
        vResult=odbcQualifiedJoin[subDmlFlags]
    ;

selectTableReference[SubDmlFlags subDmlFlags] returns [TableReference vResult]
    :
        vResult=selectTableReferenceElement[subDmlFlags]
        (
            selectTableReferenceAdditionalElement[subDmlFlags, ref vResult]
        )*
    ;

odbcInitiator
    :    tOdbcInitiator:OdbcInitiator
        {
            ThrowParseErrorException("SQL46036", tOdbcInitiator, TSqlParserResource.SQL46036Message);
        }
    ;

odbcQualifiedJoin[SubDmlFlags subDmlFlags] returns [OdbcQualifiedJoinTableReference vResult = this.FragmentFactory.CreateFragment<OdbcQualifiedJoinTableReference>()]
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
            vSelectTableReference=odbcQualifiedJoin[subDmlFlags]
        |
            vSelectTableReference=selectTableReference[subDmlFlags]
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

joinTableReference [SubDmlFlags subDmlFlags] returns [TableReference vResult = null]
{
    IToken tAfterJoinParenthesis = null;
}
    :        
        // Here we use save/skip optimization specific to nested lookaheads. That is because
        // rule "joinParenthesis" calls back to "joinTableReference" so the below lookahead 
        // has an exponential complexity. See method "TSql80ParserBaseInternal.SkipGuessing"
        // for details.
        ({ if (!SkipGuessing(tAfterJoinParenthesis)) }: 
            vResult=joinParenthesis[subDmlFlags] ({ SaveGuessing(out tAfterJoinParenthesis); }:))=>
        ({ if (!SkipGuessing(tAfterJoinParenthesis)) }: 
            vResult=joinParenthesis[subDmlFlags])
        (  // If the first element is a joinParenthesis more join's are optional
            joinElement[subDmlFlags, ref vResult]
        )*
    |
        // If the first element is not a joinParenthesis at least one join is mandatory.
        vResult=selectTableReferenceElementWithoutJoinParenthesis[subDmlFlags]
        (
            joinElement[subDmlFlags, ref vResult]
        )+
    ;

selectTableReferenceAdditionalElement[SubDmlFlags subDmlFlags, ref TableReference vResult]
    :
        joinElement[subDmlFlags, ref vResult]
    |
        pivotedTableReference[ref vResult]
    |
        unpivotedTableReference[ref vResult]
    ;

pivotedTableReference[ref TableReference vResult]
{
    PivotedTableReference vPivotedTableReference = this.FragmentFactory.CreateFragment<PivotedTableReference>();
    vPivotedTableReference.TableReference = vResult;
    vResult = vPivotedTableReference;
    
    ColumnReferenceExpression vColumn;
    MultiPartIdentifier vMultiPartIdentifier;
}
    :   Pivot LeftParenthesis vMultiPartIdentifier=multiPartIdentifier[4]
        {
            vPivotedTableReference.AggregateFunctionIdentifier=vMultiPartIdentifier;
        }
        (
            LeftParenthesis vColumn=fixedColumn
            {
                AddAndUpdateTokenInfo(vPivotedTableReference, vPivotedTableReference.ValueColumns, vColumn);
            }
            (Comma vColumn=fixedColumn
                {
                    AddAndUpdateTokenInfo(vPivotedTableReference, vPivotedTableReference.ValueColumns, vColumn);
                }
            )*
            RightParenthesis
        )        
        For vColumn=fixedColumn
        {
            vPivotedTableReference.PivotColumn = vColumn;
        }
        In columnNameList[vPivotedTableReference, vPivotedTableReference.InColumns]
        RightParenthesis
        simpleTableReferenceAlias[vPivotedTableReference]
    ;    
    
unpivotedTableReference[ref TableReference vResult]
{
    UnpivotedTableReference vUnpivotedTableReference = this.FragmentFactory.CreateFragment<UnpivotedTableReference>();
    vUnpivotedTableReference.TableReference = vResult;
    vResult = vUnpivotedTableReference;
    
    Identifier vColumnName;
}
    :   Unpivot LeftParenthesis vColumnName=identifier
        {
            vUnpivotedTableReference.ValueColumn = vColumnName;
        }
        For vColumnName=identifier
        {
            vUnpivotedTableReference.PivotColumn = vColumnName;
        }
        In columnListWithParenthesis[vUnpivotedTableReference, vUnpivotedTableReference.InColumns]
        RightParenthesis
        simpleTableReferenceAlias[vUnpivotedTableReference]
    ;    

fixedColumn returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
    vResult.ColumnType = ColumnType.Regular;
}
    :   vMultiPartIdentifier=multiPartIdentifier[4]
        {
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
            CheckTableNameExistsForColumn(vResult, false);
        }
    ;    

columnListWithParenthesis [TSqlFragment vParent, IList<ColumnReferenceExpression> columns]
{
    ColumnReferenceExpression vColumn;
}
    :    LeftParenthesis vColumn = fixedColumn
        {
            AddAndUpdateTokenInfo(vParent, columns, vColumn);
        }
        (Comma vColumn = fixedColumn
            {
                AddAndUpdateTokenInfo(vParent, columns, vColumn);
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

joinElement[SubDmlFlags subDmlFlags, ref TableReference vResult]
    :
        unqualifiedJoin[subDmlFlags, ref vResult]
    |   qualifiedJoin[subDmlFlags, ref vResult]
    ;

selectTableReferenceElement [SubDmlFlags subDmlFlags] returns [TableReference vResult = null]
    :        
        (joinParenthesis[subDmlFlags])=>
        vResult=joinParenthesis[subDmlFlags]
    |   vResult=selectTableReferenceElementWithoutJoinParenthesis[subDmlFlags]
    ;

selectTableReferenceElementWithoutJoinParenthesis[SubDmlFlags subDmlFlags] returns [TableReference vResult = null]
    :        
        {NextTokenMatches(CodeGenerationSupporter.ChangeTable)}?
        vResult=changeTableTableReference
    |   
        vResult=builtInFunctionTableReference
    |   vResult=variableTableReference
    |   vResult=variableMethodCallTableReference
    |   vResult=derivedTable[subDmlFlags]
    |   vResult=openRowset
    |   vResult=fulltextTableReference
    |   vResult=semanticTableReference
    |   vResult=openXmlTableReference
    |   vResult=subDmlTableReference[subDmlFlags]
    |   vResult=schemaObjectOrFunctionTableReference
    ;
    
changeTableTableReference returns [TableReferenceWithAliasAndColumns vResult]
{
    SchemaObjectName vTarget;
}
    :   tChangeTable:Identifier LeftParenthesis 
        {
            Match(tChangeTable, CodeGenerationSupporter.ChangeTable);
        }
        tChangesVersion:Identifier vTarget = schemaObjectFourPartName Comma
        (
            vResult = changesChangeTableParams[vTarget]
            {
                Match(tChangesVersion, CodeGenerationSupporter.Changes);
            }
        |
            vResult = versionChangeTableParams[vTarget]
            {
                Match(tChangesVersion, CodeGenerationSupporter.Version);
            }
        )
        {
            UpdateTokenInfo(vResult, tChangeTable);
        }
        RightParenthesis 
        simpleTableReferenceAlias[vResult]
        (   // Greedy due to conflict with select statement, which can start with '('
            options {greedy=true;} :
            columnNameList[vResult, vResult.Columns]
        )?
    ;
    
changesChangeTableParams [SchemaObjectName vTarget] returns [ChangeTableChangesTableReference vResult = FragmentFactory.CreateFragment<ChangeTableChangesTableReference>()]
{
    ValueExpression vSinceVersion;
    vResult.Target = vTarget;
}
    :   vSinceVersion = integerOrVariable
        {
            vResult.SinceVersion = vSinceVersion;
        }
    |   
        vSinceVersion = nullLiteral
        {
            vResult.SinceVersion = vSinceVersion;
        }
    ;
    
versionChangeTableParams [SchemaObjectName vTarget] returns [ChangeTableVersionTableReference vResult = FragmentFactory.CreateFragment<ChangeTableVersionTableReference>()]
{
    vResult.Target = vTarget;
}
    :   
        columnNameList[vResult, vResult.PrimaryKeyColumns]
        Comma
        LeftParenthesis expressionList[vResult, vResult.PrimaryKeyValues] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
subDmlTableReference [SubDmlFlags subDmlFlags] returns [DataModificationTableReference vResult = FragmentFactory.CreateFragment<DataModificationTableReference>()]
{
    DataModificationSpecification vSpec;
}
    :   tLParen:LeftParenthesis 
        {
            UpdateTokenInfo(vResult, tLParen);
            if ((subDmlFlags & SubDmlFlags.InsideSubDml) == SubDmlFlags.InsideSubDml)
                ThrowParseErrorException("SQL46075", tLParen, TSqlParserResource.SQL46075Message);            
        }
        vSpec = innerDmlStatement RightParenthesis
        {
            if ((subDmlFlags & SubDmlFlags.SelectNotForInsert) == SubDmlFlags.SelectNotForInsert)
                ThrowParseErrorException("SQL46076", vSpec, TSqlParserResource.SQL46076Message);
                
            if ((subDmlFlags & SubDmlFlags.UpdateDeleteFrom) == SubDmlFlags.UpdateDeleteFrom)
                ThrowParseErrorException("SQL46077", vSpec, TSqlParserResource.SQL46077Message);
                
            if ((subDmlFlags & SubDmlFlags.MergeUsing) == SubDmlFlags.MergeUsing)
                ThrowParseErrorException("SQL46078", vSpec, TSqlParserResource.SQL46078Message);
                
            vResult.DataModificationSpecification = vSpec;
        }
        simpleTableReferenceAlias[vResult]
        (   // Select can start from '(', so, making literal list greedy...        
            options {greedy=true;} :
            columnNameList[vResult, vResult.Columns]
        )?
    ;
    
innerDmlStatement returns [DataModificationSpecification vResult]
    :   vResult = insertSpecification[SubDmlFlags.InsideSubDml]
    |   vResult = updateSpecification[SubDmlFlags.InsideSubDml]
    |   vResult = deleteSpecification[SubDmlFlags.InsideSubDml]
    |   vResult = mergeSpecification[SubDmlFlags.InsideSubDml]
    ;

unqualifiedJoin[SubDmlFlags subDmlFlags, ref TableReference vResult]
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
        vSelectTableReference=selectTableReferenceElement[subDmlFlags]
        {
            vUnqualifiedJoin.FirstTableReference = vResult;
            vUnqualifiedJoin.SecondTableReference = vSelectTableReference;
            vResult = vUnqualifiedJoin;
        }
    ;    

qualifiedJoin[SubDmlFlags subDmlFlags, ref TableReference vResult]
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
        vSelectTableReference=selectTableReferenceWithOdbc[subDmlFlags]
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
    :    (
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
                tMerge:Merge
                {
                    Match(tId1,CodeGenerationSupporter.Local);
                    vParent.JoinHint = JoinHint.Merge;
                }
            |
                /* empty */
                {
                    vParent.JoinHint = JoinHintHelper.Instance.ParseOption(tId1);
                }
            )
        )
    |    
        Merge
        {
            vParent.JoinHint = JoinHint.Merge;
        }
    ;

joinParenthesis [SubDmlFlags subDmlFlags] returns [JoinParenthesisTableReference vResult = this.FragmentFactory.CreateFragment<JoinParenthesisTableReference>()]
{
    TableReference vTableReference;
}
    :   tLParen:LeftParenthesis vTableReference=joinTableReference[subDmlFlags] tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tLParen);
            vResult.Join = vTableReference;
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

builtInFunctionTableReference returns [BuiltInFunctionTableReference vResult = this.FragmentFactory.CreateFragment<BuiltInFunctionTableReference>()]
{
    Identifier vIdentifier;
    ScalarExpression vExpression;
}
    :    tDoubleColon:DoubleColon vIdentifier=identifier
        {
            UpdateTokenInfo(vResult,tDoubleColon);
            vResult.Name = vIdentifier;
        }
        LeftParenthesis
        ( 
            vExpression=expressionWithDefault
            {
                AddAndUpdateTokenInfo(vResult, vResult.Parameters, vExpression);
            }
            (
                Comma vExpression=expressionWithDefault
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

variableMethodCallTableReference returns [VariableMethodCallTableReference vResult = this.FragmentFactory.CreateFragment<VariableMethodCallTableReference>()]
{
    VariableReference vLiteral;
    Identifier vIdentifier;
}
    :   vLiteral=variable
        {
            vResult.Variable = vLiteral;
        }
        Dot vIdentifier=identifier
        {
            vResult.MethodName = vIdentifier;
        }
        parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
        (
            options {greedy=true;} :
            simpleTableReferenceAlias[vResult]
            columnNameList[vResult, vResult.Columns]
         )
    ;

raiseErrorStatement returns [RaiseErrorStatement vResult = this.FragmentFactory.CreateFragment<RaiseErrorStatement>()]
{
    ScalarExpression vExpression;
}
    :
        tRaiserror:Raiserror tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vResult,tRaiserror);
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

throwStatement returns [ThrowStatement vResult = FragmentFactory.CreateFragment<ThrowStatement>()]
{
    ValueExpression vErrorNumber;
    ValueExpression vMessage;
    ValueExpression vState;
}
    : tThrow:Identifier
        {
            Match(tThrow, CodeGenerationSupporter.Throw);
            UpdateTokenInfo(vResult, tThrow);
        }
        (
            vErrorNumber=integerOrVariable
            Comma
            vMessage=stringOrVariable
            Comma
            vState=integerOrVariable
            {
                vResult.ErrorNumber = vErrorNumber;
                vResult.Message = vMessage;
                vResult.State = vState;
            }
        )?
    ;

outputIntoOutputClause[DataModificationSpecification vParent]
{
    SelectElement vSelectColumn;
    List<SelectElement> vSelectElements = new List<SelectElement>();
    OutputIntoClause vOutputIntoClause;
    OutputClause vOutputClause;
}
    :   tOutput:Identifier
        {
            Match(tOutput, CodeGenerationSupporter.Output);
        }
        vSelectColumn=outputClauseSelectElement
        {
            vSelectElements.Add(vSelectColumn);
        }
        (
            Comma vSelectColumn=outputClauseSelectElement
            {
                vSelectElements.Add(vSelectColumn);
            }                
        )*
        (
            (
                vOutputIntoClause=outputIntoClause[tOutput, vSelectElements]
                {
                    vParent.OutputIntoClause = vOutputIntoClause;
                }
                (
                    {NextTokenMatches(CodeGenerationSupporter.Output)}?
                    vOutputClause = outputClause
                    {
                        vParent.OutputClause = vOutputClause;
                    }
                )?
            )
        |
            /* empty */
            {
                vOutputClause=FragmentFactory.CreateFragment<OutputClause>();
                UpdateTokenInfo(vOutputClause, tOutput);
                AddAndUpdateTokenInfo(vOutputClause, vOutputClause.SelectColumns,vSelectElements);
                vParent.OutputClause = vOutputClause;
            }
        )

    ;

outputIntoClause[IToken tOutput, IList<SelectElement> vSelectElements] returns [OutputIntoClause vResult = this.FragmentFactory.CreateFragment<OutputIntoClause>()]
{
    TableReference vIntoTable;
    UpdateTokenInfo(vResult,tOutput);
    AddAndUpdateTokenInfo(vResult, vResult.SelectColumns,vSelectElements);
}
    :
        Into
        (
            vIntoTable=variableDmlTarget
        |
            vIntoTable=intoSchemaObjectTable
        )
        {
            vResult.IntoTable = vIntoTable;
        }
        (
            options {greedy=true;} : // Because of the select that can start with LeftParenthesis
            identifierColumnList[vResult, vResult.IntoTableColumns]
        )?
    ;

outputClause returns [OutputClause vResult = this.FragmentFactory.CreateFragment<OutputClause>()]
{
    SelectElement vSelectColumn;
}
    :
        tOutput:Identifier
        {
            Match(tOutput, CodeGenerationSupporter.Output);
            UpdateTokenInfo(vResult,tOutput);
        }
        vSelectColumn=outputClauseSelectElement
        {
            AddAndUpdateTokenInfo(vResult, vResult.SelectColumns,vSelectColumn);
        }
        (
            Comma vSelectColumn=outputClauseSelectElement
            {
                AddAndUpdateTokenInfo(vResult, vResult.SelectColumns,vSelectColumn);
            }                
        )*
    ;

intoSchemaObjectTable returns [NamedTableReference vResult = this.FragmentFactory.CreateFragment<NamedTableReference>()]
{
    SchemaObjectName vSchemaObjectName;
}   
    :   vSchemaObjectName = schemaObjectFourPartName
        {            
            vResult.SchemaObject = vSchemaObjectName;            
        } 
    ;

outputClauseOpt [SubDmlFlags subDmlFlags, DataModificationSpecification vParent]
    :    (
            {NextTokenMatches(CodeGenerationSupporter.Output)}?
            outputIntoOutputClause[vParent]
        |
            /* empty */
            {
                if ((subDmlFlags & SubDmlFlags.InsideSubDml) == SubDmlFlags.InsideSubDml)
                    ThrowParseErrorException("SQL46079", vParent, TSqlParserResource.SQL46079Message);
            }
        )
    ;

deleteStatement [SubDmlFlags subDmlFlags] returns [DeleteStatement vResult = this.FragmentFactory.CreateFragment<DeleteStatement>()]
{
    DeleteSpecification vDeleteSpecification;
}
    :
        vDeleteSpecification = deleteSpecification [subDmlFlags]
        {
            vResult.DeleteSpecification = vDeleteSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

deleteSpecification [SubDmlFlags subDmlFlags] returns [DeleteSpecification vResult = this.FragmentFactory.CreateFragment<DeleteSpecification>()]
{        
    TableReference vDmlTarget;
    FromClause vFromClause;
    WhereClause vWhereClause;
}
    :   tDelete:Delete
        {
            UpdateTokenInfo(vResult,tDelete);
        }
        dmlTopRowFilterOpt[vResult]
        (
            From
        )?
        vDmlTarget=dmlTarget[false]
        {
            vResult.Target = vDmlTarget;
        }
        outputClauseOpt[subDmlFlags, vResult]
        vFromClause = fromClauseOpt[subDmlFlags | SubDmlFlags.UpdateDeleteFrom]
        {
            vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=dmlWhereClause[subDmlFlags]
            {
                vResult.WhereClause = vWhereClause;
            }
        )?
    ;

insertStatement [SubDmlFlags subDmlFlags] returns [InsertStatement vResult = this.FragmentFactory.CreateFragment<InsertStatement>()]
{
    InsertSpecification vInsertSpecification;
}
    :
        vInsertSpecification = insertSpecification[subDmlFlags]
        {
            vResult.InsertSpecification = vInsertSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

insertSpecification [SubDmlFlags subDmlFlags] returns [InsertSpecification vResult = this.FragmentFactory.CreateFragment<InsertSpecification>()]
{
    TableReference vDmlTarget;
    InsertSource vTSqlFragment = null;
    ColumnReferenceExpression vColumn;
}
    :   tInsert:Insert
        {
            UpdateTokenInfo(vResult,tInsert);
        }
        dmlTopRowFilterOpt[vResult]
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
        vDmlTarget=dmlTarget[false]
        {
            vResult.Target = vDmlTarget;
        }
        (
            (LeftParenthesis (Dot | Identifier | QuotedIdentifier | PseudoColumn))=>  // Linear approximation conflicts with ( select ...)
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
        outputClauseOpt[subDmlFlags, vResult]
        (
            (
                vTSqlFragment=valuesInsertSource          
            )
        |
            vTSqlFragment=executeInsertSource
        |
            vTSqlFragment=selectInsertSource[subDmlFlags]
        )
        {
            vResult.InsertSource = vTSqlFragment;
        }
    ;

updateStatement [SubDmlFlags subDmlFlags] returns [UpdateStatement vResult = this.FragmentFactory.CreateFragment<UpdateStatement>()]
{
    UpdateSpecification vUpdateSpecification;
}
    :
        vUpdateSpecification = updateSpecification[subDmlFlags]
        {
            vResult.UpdateSpecification = vUpdateSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

updateSpecification [SubDmlFlags subDmlFlags] returns [UpdateSpecification vResult = this.FragmentFactory.CreateFragment<UpdateSpecification>()]
{
    TableReference vDmlTarget;
    FromClause vFromClause;
    WhereClause vWhereClause;
}
    :   tUpdate:Update
        {
            UpdateTokenInfo(vResult,tUpdate);
        }
        dmlTopRowFilterOpt[vResult]
        vDmlTarget=dmlTarget[false]
        {
            vResult.Target = vDmlTarget;
        }
        setClausesList[vResult, vResult.SetClauses]
        outputClauseOpt[subDmlFlags, vResult]
        vFromClause = fromClauseOpt[subDmlFlags | SubDmlFlags.UpdateDeleteFrom]
        {
            vResult.FromClause = vFromClause;
        }
        (
            vWhereClause=dmlWhereClause[subDmlFlags]
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

setClause returns [SetClause vResult]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :    
        vResult = setClauseStartingWithVariable
    |
        vMultiPartIdentifier=multiPartIdentifier[-1]
        (
            vResult = setClauseColumnAssignment[vMultiPartIdentifier]
        |
            vResult = setClauseFunctionCall[vMultiPartIdentifier]
        )
    |   
        vResult = setClauseSubItemSystemColumn
   ;

setClauseStartingWithVariable returns [AssignmentSetClause vResult = FragmentFactory.CreateFragment<AssignmentSetClause>()]
{
    VariableReference vLiteral;
    ScalarExpression vExpression;
    AssignmentKind vAssignmentKind;
    MultiPartIdentifier vMultiPartIdentifier;
}
    :   vLiteral=variable 
        {
            vResult.Variable = vLiteral;
        }
        (
            (EqualsSign
                (
                    (multiPartIdentifier[-1] assignmentWithOptOp)=>
                    vMultiPartIdentifier=multiPartIdentifier[-1] vAssignmentKind = assignmentWithOptOp 
                    {
                        CreateSetClauseColumn(vResult, vMultiPartIdentifier);
                        vResult.AssignmentKind = vAssignmentKind;
                    }
                    vExpression=expressionWithDefault
                    {
                        vResult.NewValue = vExpression;
                    }
                |
                    vExpression=expression
                    {
                        vResult.NewValue = vExpression;
                    }
                )
            )
        |
            (vAssignmentKind = assignmentWithOp vExpression=expression
                {
                    vResult.NewValue = vExpression;
                    vResult.AssignmentKind = vAssignmentKind;
                }
            )
        )
    ;
    
setClauseColumnAssignment [MultiPartIdentifier vMultiPartIdentifier] returns [AssignmentSetClause vResult = FragmentFactory.CreateFragment<AssignmentSetClause>()]
{
    ScalarExpression vExpression;
    AssignmentKind vAssignmentKind;
}
    :   vAssignmentKind = assignmentWithOptOp vExpression=expressionWithDefault
        {
            CreateSetClauseColumn(vResult, vMultiPartIdentifier);
            vResult.AssignmentKind = vAssignmentKind;
            vResult.NewValue = vExpression;
        }
    ;
    
setClauseFunctionCall [MultiPartIdentifier vMultiPartIdentifier] returns [FunctionCallSetClause vResult = FragmentFactory.CreateFragment<FunctionCallSetClause>()]
{
    FunctionCall vFunctionCall = FragmentFactory.CreateFragment<FunctionCall>();
    PutIdentifiersIntoFunctionCall(vFunctionCall, vMultiPartIdentifier);
}    
    :
        parenthesizedOptExpressionWithDefaultList[vFunctionCall, vFunctionCall.Parameters]                
        {
            vResult.MutatorFunction=vFunctionCall;
        }
    ;

setClauseSubItemSystemColumn returns [AssignmentSetClause vResult = FragmentFactory.CreateFragment<AssignmentSetClause>()]
{
    ColumnReferenceExpression vColumn = FragmentFactory.CreateFragment<ColumnReferenceExpression>();
    ScalarExpression vExpression;
    AssignmentKind vAssignmentKind;
}
    : systemColumn[vColumn] vAssignmentKind = assignmentWithOptOp vExpression=expressionWithDefault
        {
            vResult.Column = vColumn;
            vResult.AssignmentKind = vAssignmentKind;
            vResult.NewValue = vExpression;
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

selectInsertSource [SubDmlFlags subDmlFlags] returns [SelectInsertSource vResult = this.FragmentFactory.CreateFragment<SelectInsertSource>()]
{
    QueryExpression vQueryExpression;
    OrderByClause vOrderByClause;
    OffsetClause vOffsetClause;
}
    :
        vQueryExpression = queryExpression[subDmlFlags, null]
        (
            vOrderByClause=orderByClause
            {
                vQueryExpression.OrderByClause = vOrderByClause;
            }
            (
                vOffsetClause=offsetClause
                {
                    vQueryExpression.OffsetClause = vOffsetClause;
                }
            )?
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
        defaultValuesInsertSource[vResult]
    |
        tValues:Values vRowValue = rowValueExpressionWithDefault
        {
            UpdateTokenInfo(vResult,tValues);
            AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
        }
        (Comma vRowValue = rowValueExpressionWithDefault
            {
                AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
            }
        )*        
    ;
    
defaultValuesInsertSource [ValuesInsertSource vParent]
    :
        tDefault:Default tValues:Values
        {
            UpdateTokenInfo(vParent,tDefault);
            UpdateTokenInfo(vParent,tValues);
            vParent.IsDefaultValues = true;
        }
    ;
    
rowValueExpression returns [RowValue vResult = FragmentFactory.CreateFragment<RowValue>()]
    :   
        tLParen:LeftParenthesis 
        {
            UpdateTokenInfo(vResult,tLParen);
        }
        expressionList[vResult, vResult.ColumnValues]
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
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
            vResult.ColumnType = ColumnType.Regular;
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    |
        systemColumn[vResult]
    ;

dmlTarget[bool indexHintAllowed] returns [TableReference vResult]
    :      
        vResult=schemaObjectDmlTarget[indexHintAllowed]
        | vResult=openRowset
        | vResult=variableDmlTarget
    ;

mergeInsertDmlColumnListOpt [InsertMergeAction vParent]
{
    ColumnReferenceExpression vDmlTargetColumn;
}
    :    (
            LeftParenthesis vDmlTargetColumn=mergeInsertDmlColumn
            {
                AddAndUpdateTokenInfo(vParent, vParent.Columns, vDmlTargetColumn);
            }
            (
                Comma vDmlTargetColumn=mergeInsertDmlColumn
                {
                    AddAndUpdateTokenInfo(vParent, vParent.Columns, vDmlTargetColumn);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent,tRParen);
            }
        )?
    ;
    
mergeInsertDmlColumn returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
{
    MultiPartIdentifier vIdentifier;
}
    :   vIdentifier = multiPartIdentifier[1]
        {
            vResult.ColumnType = ColumnType.Regular;
            vResult.MultiPartIdentifier = vIdentifier;
        }
    |
        systemColumn[vResult]
    ;    

variableDmlTarget returns [VariableTableReference vResult = this.FragmentFactory.CreateFragment<VariableTableReference>()]
{
    VariableReference vLiteral;
}
    :
        vLiteral=variable
        {
            vResult.Variable = vLiteral;
        }
    ;

schemaObjectDmlTarget [bool indexHintAllowed] returns [TableReferenceWithAlias vResult = null]
    :
            (schemaObjectFourPartName LeftParenthesis (possibleNegativeConstantWithDefault|RightParenthesis))=> // necessary because select statement can start with LeftParenthesis
            vResult=schemaObjectFunctionDmlTarget
        |   vResult=schemaObjectTableDmlTarget[indexHintAllowed]
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


schemaObjectTableDmlTarget [bool indexHintAllowed] returns [NamedTableReference vResult = FragmentFactory.CreateFragment<NamedTableReference>()]
{
    SchemaObjectName vSchemaObjectName;
}   
    :   vSchemaObjectName = schemaObjectFourPartName
        {            
            vResult.SchemaObject = vSchemaObjectName;            
        } 
        (
            options {greedy=true;} : // Because of the select that can start with LeftParenthesis
            With tableHints[vResult, vResult.TableHints, indexHintAllowed]
        )?
    ;

schemaObjectOrFunctionTableReference returns [TableReference vResult]
{
    SchemaObjectName vSchemaObjectName;
}
    : 
        vSchemaObjectName=schemaObjectFourPartName
        (
            {IsTableReference(false)}?
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
    TableSampleClause vTableSampleClause;
    IndexTableHint vIndexTableHint;
}   
    :   (
            singleOldStyleTableHint[vResult, vResult.TableHints]
            (
                simpleTableReferenceAlias[vResult]
            )?
        )
    |
        (
            // The tableSampleClause is hard to handle in an LL grammar because of the
            // way it is defined, parsing at this level is bad if the input that comes after
            // is directly alias.  So we parse this here, and throw an exception if we see an
            // simpleTableReferenceAlias without seeing LeftParenthesis.
            vTableSampleClause=tableSampleClause
            {
                vResult.TableSampleClause = vTableSampleClause;
            }
        )?
        (
            options {greedy=true;} : 
            nonParameterTableHints[vResult, vResult.TableHints]
        | 
            {
                if (vResult.TableSampleClause != null)
                {
                    // If tableSampleClause was parsed, this is an error.
                    throw GetUnexpectedTokenErrorException();
                }
            }
            simpleTableReferenceAlias[vResult]
            (
                vTableSampleClause=tableSampleClause
                {
                    vResult.TableSampleClause = vTableSampleClause;
                }
            )?
            (
                (LeftParenthesis integer)=> // necessary because select statement can start with LeftParenthesis
                vIndexTableHint=oldForceIndex
                {
                    AddAndUpdateTokenInfo(vResult, vResult.TableHints, vIndexTableHint);
                }
            |   (With | HoldLock | LeftParenthesis (HoldLock|Index))=> // necessary because select statement can start with LeftParenthesis
                nonParameterTableHints[vResult, vResult.TableHints]
            |
                singleOldStyleTableHint[vResult, vResult.TableHints]
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
    StringLiteral vProperty;
}
    :  
        fulltextSemanticTableColumnList[vParent, vParent.Columns]
    |
        tProperty:Identifier LeftParenthesis vColumn=identifierColumnReferenceExpression
        {
            Match(tProperty, CodeGenerationSupporter.Property);
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vColumn);
        }
        Comma vProperty=stringLiteral RightParenthesis
        {
            vParent.PropertyName=vProperty;
        }
    ;

fulltextSemanticTableColumnList [TSqlFragment vParent, IList<ColumnReferenceExpression> vColumns]
{
    ColumnReferenceExpression vColumn;
}
    :   vColumn = identifierColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vColumns, vColumn);
        }
    |    vColumn = starColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vColumns, vColumn);
        }
    |    LeftParenthesis vColumn = starColumnReferenceExpression RightParenthesis
        {
            AddAndUpdateTokenInfo(vParent, vColumns, vColumn);
        }
    |    LeftParenthesis vColumn = identifierColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vColumns, vColumn);
        }
        (Comma vColumn = identifierColumnReferenceExpression
            {
                AddAndUpdateTokenInfo(vParent, vColumns, vColumn);
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
        (Comma vTopN = integerOrVariable
            {
                vParent.TopN = vTopN;
            }
        )?
    | Comma vTopN = integerOrVariable
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
    :
        tLanguage:Identifier
        {
            Match(tLanguage, CodeGenerationSupporter.Language);
        }
        vResult=binaryOrIntegerOrStringOrVariable
    ;
    
semanticTableReference returns [SemanticTableReference vResult = FragmentFactory.CreateFragment<SemanticTableReference>()]
    :
        semanticKeyPhraseTableReference[vResult] 
      | semanticSimilarityTableReference[vResult]
      | semanticSimilarityDetailsTableReference[vResult]
    ;

semanticKeyPhraseTableReference[SemanticTableReference vParent]
{
    SchemaObjectName vTableName;
    ScalarExpression vSourceKey;
}
    :   tKeyPhraseTable:SemanticKeyPhraseTable LeftParenthesis vTableName = schemaObjectFourPartName
        {
            UpdateTokenInfo(vParent,tKeyPhraseTable);
            vParent.SemanticFunctionType = SemanticFunctionType.SemanticKeyPhraseTable;
            vParent.TableName = vTableName;
        }
        Comma fulltextSemanticTableColumnList[vParent, vParent.Columns]
        (
            Comma vSourceKey=possibleNegativeConstant
            {
                vParent.SourceKey=vSourceKey;
            }
        )? 
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vParent,tRParen);
        }
        simpleTableReferenceAliasOpt[vParent]
    ;

semanticSimilarityTableReference[SemanticTableReference vParent]
{
    SchemaObjectName vTableName;
    ScalarExpression vSourceKey;
}
    :   tSimilarityTable:SemanticSimilarityTable LeftParenthesis vTableName = schemaObjectFourPartName
        {
            UpdateTokenInfo(vParent,tSimilarityTable);
            vParent.SemanticFunctionType = SemanticFunctionType.SemanticSimilarityTable;
            vParent.TableName = vTableName;
        }
        Comma fulltextSemanticTableColumnList[vParent, vParent.Columns]
        Comma vSourceKey=possibleNegativeConstant
        {
            vParent.SourceKey=vSourceKey;
        }
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vParent,tRParen);
        }
        simpleTableReferenceAliasOpt[vParent]
    ;

semanticSimilarityDetailsTableReference[SemanticTableReference vParent]
{
    SchemaObjectName vTableName;
    ColumnReferenceExpression vSourceColumn;
    ScalarExpression vSourceKey;
    ColumnReferenceExpression vMatchedColumn;
    ScalarExpression vMatchedKey;
}
    :   tSimilarityDetailsTable:SemanticSimilarityDetailsTable LeftParenthesis vTableName = schemaObjectFourPartName
        {
            UpdateTokenInfo(vParent,tSimilarityDetailsTable);
            vParent.SemanticFunctionType = SemanticFunctionType.SemanticSimilarityDetailsTable;
            vParent.TableName = vTableName;
        }
        Comma vSourceColumn=identifierColumnReferenceExpression
        {
            AddAndUpdateTokenInfo(vParent, vParent.Columns, vSourceColumn);
        }
        Comma vSourceKey=possibleNegativeConstant
        {
            vParent.SourceKey=vSourceKey;
        }
        Comma vMatchedColumn=identifierColumnReferenceExpression
        {
            vParent.MatchedColumn=vMatchedColumn;
        }
        Comma vMatchedKey=possibleNegativeConstant
        {
            vParent.MatchedKey=vMatchedKey;
        }
        tRParen:RightParenthesis 
        {
            UpdateTokenInfo(vParent,tRParen);
        }
        simpleTableReferenceAliasOpt[vParent]
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
    (Comma vFlags = integerOrVariable // openxml_flags
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
        | vResult = openRowsetBulk
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
    
openRowsetBulk returns [BulkOpenRowset vResult = FragmentFactory.CreateFragment<BulkOpenRowset>()]
{
    long encountered = TSql90ParserBaseInternal.BulkInsertOptionsProhibitedInOpenRowset;
    
    BulkInsertOption vOption;
    StringLiteral vDataFile;
}
    : Bulk vDataFile = stringLiteral 
        {
            AddAndUpdateTokenInfo(vResult, vResult.DataFiles, vDataFile);
        }
        (Comma vOption = openRowsetBulkHint
            {
                CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
        )*
        tRParen:RightParenthesis
        {
            CheckForFormatFileOptionInOpenRowsetBulk(encountered, vDataFile);
            UpdateTokenInfo(vResult,tRParen);
        }
        simpleTableReferenceAliasOpt[vResult]    
        (
            options {greedy = true; } : // Conflicts with Select, which can start from (
            columnNameList[vResult, vResult.Columns]
        )?
    ;

openRowsetBulkHint returns [BulkInsertOption vResult]
    : vResult = openRowsetBulkHintNoValue
    | vResult = simpleBulkInsertOptionWithValue
    | vResult = openRowsetBulkOrderHint
    ;    
    
openRowsetBulkHintNoValue returns [BulkInsertOption vResult = FragmentFactory.CreateFragment<BulkInsertOption>()]
    : tOption:Identifier
        {
            vResult.OptionKind = OpenRowsetBulkHintOptionsHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
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

// looks like ODBC stuff - commenting out for now
// binaryRowset returns [BinaryTableReference vResult = FragmentFactory.CreateFragment<BinaryTableReference>()]
//    : tLBrace:LBRACE tIRowset:Identifier tBinary:HexLiteral tRBrace:RBRACE
//        {
//            Match(tIRowset,CodeGenerationSupporter.IRowset);
//            
//            vResult.Binary = FragmentFactory.CreateFragment<Literal>();
//            vResult.Binary.LiteralType = LiteralType.Binary;
//            vResult.Binary.Value = tBinary.getText();
//            vResult.Binary.UpdatePositionInfo(tBinary);
//            
//            UpdateTokenInfo(vResult,tLBrace);
//            UpdateTokenInfo(vResult,tRBrace);
//        }
//    ;
    
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
            AddAndUpdateTokenInfo(vResult, vResult.IndexValues, IdentifierOrValueExpression(vLiteral));
            UpdateTokenInfo(vResult,tRParen);
            vResult.HintKind=TableHintKind.Index;
        }
    ;

nonParameterTableHints [TSqlFragment vParent, IList<TableHint> hints]
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
            simpleTableHints[vParent, hints]
        )?
    |   simpleTableHints[vParent, hints]
    ;

// In sql server yacc grammar, this rule equals simple_tablehints
simpleTableHints[TSqlFragment vParent, IList<TableHint> hints]
{
    IndexTableHint vOldForceIndex;
}
    :   tWith:With 
        {
            UpdateTokenInfo(vParent,tWith);
        }
        (
            vOldForceIndex = oldForceIndex
            {
                AddAndUpdateTokenInfo(vParent, hints, vOldForceIndex);
            }
        |
            tableHints[vParent, hints, true]
        )
    ;

tableHints[TSqlFragment vParent, IList<TableHint> hints, bool tableHintAllowed]
    :
        tLParen:LeftParenthesis
        {
            UpdateTokenInfo(vParent,tLParen);
        }
        tableHintsBody[vParent, hints, tableHintAllowed]
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
tableHintsBody [TSqlFragment vParent, IList<TableHint> hints, bool tableHintAllowed]
{
    TableHint vHint;
}
    :   vHint = tableHint[tableHintAllowed]
        {
            AddAndUpdateTokenInfo(vParent, hints, vHint);
        }
        (
            ( Comma )? vHint = tableHint[tableHintAllowed]
            {
                AddAndUpdateTokenInfo(vParent, hints, vHint);
            }
        )*
    ;

tableHint [bool indexHintAllowed] returns [TableHint vResult]
    :
        {NextTokenMatches(CodeGenerationSupporter.ForceSeek)}? 
        vResult = forceSeekTableHint[indexHintAllowed]
    |
        vResult = simpleTableHint
    |
        vResult = indexTableHint[indexHintAllowed]
    |    
        vResult = literalTableHint    
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
            vResult.HintKind = TableHintOptionsHelper.Instance.ParseOption(tId, SqlVersionFlags.TSql120);
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

literalTableHint returns [LiteralTableHint vResult = FragmentFactory.CreateFragment<LiteralTableHint>()]
{
    Literal vLiteral;
}
    : tOption:Identifier EqualsSign vLiteral=integer
        {
            Match(tOption, CodeGenerationSupporter.SpatialWindowMaxCells);
            vResult.HintKind=TableHintKind.SpatialWindowMaxCells;
            UpdateTokenInfo(vResult, tOption);
            vResult.Value=vLiteral;
        }
    ;

forceSeekTableHint [bool indexHintAllowed] returns [ForceSeekTableHint vResult = FragmentFactory.CreateFragment<ForceSeekTableHint>()]
{
    IdentifierOrValueExpression vIndexValue;
    ColumnReferenceExpression vColumnValue;
}
:   tForceSeek:Identifier
        {
            Match(tForceSeek, CodeGenerationSupporter.ForceSeek);
            UpdateTokenInfo(vResult,tForceSeek);
            vResult.HintKind = TableHintKind.ForceSeek;
        }      
        (
            LeftParenthesis vIndexValue = identifierOrInteger
            {
                if (!indexHintAllowed)
                    ThrowParseErrorException("SQL46074", tForceSeek, TSqlParserResource.SQL46074Message);
                
                vResult.IndexValue = vIndexValue;
            }
            LeftParenthesis vColumnValue = identifierColumnReferenceExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.ColumnValues, vColumnValue);
            }
            (
                Comma vColumnValue = identifierColumnReferenceExpression
                {
                    AddAndUpdateTokenInfo(vResult, vResult.ColumnValues, vColumnValue);
                }
            )*
            RightParenthesis tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?        
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
    
dmlWhereClause[SubDmlFlags subDmlFlags] returns [WhereClause vResult]
    :
        vResult = whereClause
    |
        vResult = whereCurrentOfCursorClause[subDmlFlags]
    ;
    
whereCurrentOfCursorClause [SubDmlFlags subDmlFlags] returns [WhereClause vResult = FragmentFactory.CreateFragment<WhereClause>()]
{
    CursorId vCursorId;
}
    :   tWhere:Where Current Of vCursorId = cursorId
        {
            if ((subDmlFlags & SubDmlFlags.InsideSubDml) == SubDmlFlags.InsideSubDml)
                ThrowParseErrorException("SQL46083", tWhere, TSqlParserResource.SQL46083Message);
                
            UpdateTokenInfo(vResult,tWhere);
            vResult.Cursor = vCursorId;
        }
    ;

//////////////////////////////////////////////////////////////////////
// Group By clause

groupByClause returns [GroupByClause vResult = FragmentFactory.CreateFragment<GroupByClause>()]
{
    GroupingSpecification vGroupingItem;
    bool encounteredCubeRollupGroupingSets = false;
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
        vGroupingItem = groupByItem[vResult.All, ref encounteredCubeRollupGroupingSets]
        {
            AddAndUpdateTokenInfo(vResult, vResult.GroupingSpecifications, vGroupingItem);
        }
        (Comma vGroupingItem = groupByItem[vResult.All, ref encounteredCubeRollupGroupingSets]
            {
                AddAndUpdateTokenInfo(vResult, vResult.GroupingSpecifications, vGroupingItem);
            }
        )*
        (   
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } : 
            With tOption:Identifier
            {
                if (vResult.All)
                    ThrowParseErrorException("SQL46084", tOption, TSqlParserResource.SQL46084Message);
                    
                if (encounteredCubeRollupGroupingSets)
                    ThrowParseErrorException("SQL46085", tOption, TSqlParserResource.SQL46085Message);
                
                UpdateTokenInfo(vResult,tOption);
                vResult.GroupByOption = GroupByOptionHelper.Instance.ParseOption(tOption);
            }
        )?
    ;

groupByItem [bool isAll, ref bool encounteredCubeRollupGroupingSets] returns [GroupingSpecification vResult]
    :
        {NextTokenMatches(CodeGenerationSupporter.Cube)}?
        vResult = cubeSpec[isAll]
        {
            encounteredCubeRollupGroupingSets = true;
        }
    |   {NextTokenMatches(CodeGenerationSupporter.Rollup)}?
        vResult = rollupSpec[isAll]
        {
            encounteredCubeRollupGroupingSets = true;
        }
    |   {NextTokenMatches(CodeGenerationSupporter.Grouping)}?
        vResult = groupingSetsSpec[isAll]
        {
            encounteredCubeRollupGroupingSets = true;
        }
    |   (LeftParenthesis RightParenthesis)=>
        vResult = grandTotal
    |   vResult = simpleGroupByItem
    ;
    
groupingSetsSpec [bool isAll] returns [GroupingSetsGroupingSpecification vResult = FragmentFactory.CreateFragment<GroupingSetsGroupingSpecification>()]
{
    GroupingSpecification vGroupingSet;
}
    :   tGrouping:Identifier tSets:Identifier LeftParenthesis vGroupingSet = groupingSet
        {
            Match(tGrouping, CodeGenerationSupporter.Grouping);
            Match(tSets, CodeGenerationSupporter.Sets);        
            if (isAll)
                ThrowParseErrorException("SQL46084", tGrouping, TSqlParserResource.SQL46084Message);
                
            UpdateTokenInfo(vResult, tGrouping);
            AddAndUpdateTokenInfo(vResult, vResult.Sets, vGroupingSet);
        }
        (Comma vGroupingSet = groupingSet
            {
                AddAndUpdateTokenInfo(vResult, vResult.Sets, vGroupingSet);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
    ;

groupingSet returns [GroupingSpecification vResult]
    :
        (LeftParenthesis RightParenthesis) =>
        vResult = grandTotal
    |
        (LeftParenthesis) =>
        vResult = composingGroupingSet
    |
        vResult = groupingSetItem
    ;
    
composingGroupingSet returns [CompositeGroupingSpecification vResult = FragmentFactory.CreateFragment<CompositeGroupingSpecification>()]
{
    GroupingSpecification vItem;
}
    :   LeftParenthesis vItem = groupingSetItem
        {
            AddAndUpdateTokenInfo(vResult, vResult.Items, vItem);
        }
        (Comma vItem = groupingSetItem
            {
                AddAndUpdateTokenInfo(vResult, vResult.Items, vItem);
            }
        )*
        RightParenthesis
    ;
    
grandTotal returns [GrandTotalGroupingSpecification vResult = FragmentFactory.CreateFragment<GrandTotalGroupingSpecification>()]
    :
        tLParen:LeftParenthesis tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tLParen);
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
groupingSetItem returns [GroupingSpecification vResult]
    :
        {NextTokenMatches(CodeGenerationSupporter.Cube)}?
        vResult = cubeSpec[false]
    |   {NextTokenMatches(CodeGenerationSupporter.Rollup)}?
        vResult = rollupSpec[false]
    |   
        vResult = simpleGroupByItem
    ;
    
rollupSpec [bool isAll] returns [RollupGroupingSpecification vResult = FragmentFactory.CreateFragment<RollupGroupingSpecification>()]
    :
        tRollup:Identifier LeftParenthesis cubeOrRollupArgumentList[vResult, vResult.Arguments] tRParen:RightParenthesis
        {
            Match(tRollup, CodeGenerationSupporter.Rollup);
            
            if (isAll)
                ThrowParseErrorException("SQL46084", tRollup, TSqlParserResource.SQL46084Message);
            
            UpdateTokenInfo(vResult, tRollup);
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
cubeSpec [bool isAll] returns [CubeGroupingSpecification vResult = FragmentFactory.CreateFragment<CubeGroupingSpecification>()]
    :
        tCube:Identifier LeftParenthesis cubeOrRollupArgumentList[vResult, vResult.Arguments] tRParen:RightParenthesis
        {
            Match(tCube, CodeGenerationSupporter.Cube);
            
            if (isAll)
                ThrowParseErrorException("SQL46084", tCube, TSqlParserResource.SQL46084Message);
            
            UpdateTokenInfo(vResult, tCube);
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
cubeOrRollupArgumentList [TSqlFragment vParent, IList<GroupingSpecification> specs]
{
    GroupingSpecification vArgument;
}
    :   vArgument = cubeOrRollupArgument
        {
            AddAndUpdateTokenInfo(vParent, specs, vArgument);
        }
        (Comma vArgument = cubeOrRollupArgument
            {
                AddAndUpdateTokenInfo(vParent, specs, vArgument);
            }
        )*
    ;
    
cubeOrRollupArgument returns [GroupingSpecification vResult]
    :
        (LeftParenthesis) =>
        vResult = compositeGroupByItem
    |
        vResult = simpleGroupByItem
    ;
    
compositeGroupByItem returns [CompositeGroupingSpecification vResult = FragmentFactory.CreateFragment<CompositeGroupingSpecification>()]
{
    ExpressionGroupingSpecification vItem;
}
    :   tLParen:LeftParenthesis vItem = simpleGroupByItem
        {
            UpdateTokenInfo(vResult, tLParen);
            AddAndUpdateTokenInfo(vResult, vResult.Items, vItem);
        }
        (Comma vItem = simpleGroupByItem
            {
                AddAndUpdateTokenInfo(vResult, vResult.Items, vItem);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
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
    
// End of Group By clause
///////////////////////////////////////////////////////////////////////

havingClause returns [HavingClause vResult = this.FragmentFactory.CreateFragment<HavingClause>()]
{
    BooleanExpression vExpression;
}
    :
        tHaving:Having
        {
            UpdateTokenInfo(vResult,tHaving);
        }
        vExpression=booleanExpression
        {
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
        (    options { greedy = true; } : // Conflicts with Select, which can also start from '('. Also, identifierStatements
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
        )* tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;
    
hint returns [OptimizerHint vResult]
    : vResult = literalOptimizerHint
    | vResult = simpleOptimizerHint
    | vResult = usePlanOptimizerHint
    | vResult = optimizeForOptimizerHint
    | vResult = tableHintsOptimizerHint
    ;

tableHintsOptimizerHint returns [TableHintsOptimizerHint vResult = FragmentFactory.CreateFragment<TableHintsOptimizerHint>()]
{
    SchemaObjectName vSchemaObjectName;
}
    :   tTable:Table tHint:Identifier LeftParenthesis vSchemaObjectName = schemaObjectFourPartName
        {
            UpdateTokenInfo(vResult, tTable);
            Match(tHint, CodeGenerationSupporter.Hint);
            vResult.HintKind=OptimizerHintKind.TableHints;
            vResult.ObjectName = vSchemaObjectName;
        }
        (Comma tableHintsBody[vResult, vResult.TableHints, true])?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }
    ;
    
simpleOptimizerHint returns [OptimizerHint vResult = FragmentFactory.CreateFragment<OptimizerHint>()]
    : tHashLoop:Identifier Join
        {
            vResult.HintKind = ParseJoinOptimizerHint(tHashLoop);
        }
    | Merge Join
        {
            vResult.HintKind = OptimizerHintKind.MergeJoin;
        }
    | tConcatHashKeep:Identifier Union 
        {
            vResult.HintKind = ParseUnionOptimizerHint(tConcatHashKeep);
        }
    | Merge Union 
        {
            vResult.HintKind = OptimizerHintKind.MergeUnion;
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
            vResult.HintKind = PlanOptimizerHintHelper.Instance.ParseOption(tPlan, SqlVersionFlags.TSql120);
        }
    | tFirstWord:Identifier tSecondWord:Identifier
        ( 
            /* empty */
            {
                if (TryMatch(tFirstWord, CodeGenerationSupporter.Expand))
                {
                    Match(tSecondWord, CodeGenerationSupporter.Views);
                    vResult.HintKind = OptimizerHintKind.ExpandViews;
                }
                else if (TryMatch(tFirstWord, CodeGenerationSupporter.Parameterization))
                {
                    if (TryMatch(tSecondWord,CodeGenerationSupporter.Simple))
                        vResult.HintKind = OptimizerHintKind.ParameterizationSimple;
                    else
                    {
                        Match(tSecondWord, CodeGenerationSupporter.Forced);
                        vResult.HintKind = OptimizerHintKind.ParameterizationForced;
                    }
                }
                else
                {
                    Match(tFirstWord, CodeGenerationSupporter.Bypass);
                    Match(tSecondWord, CodeGenerationSupporter.OptimizerQueue);
                    vResult.HintKind = OptimizerHintKind.BypassOptimizerQueue;
                }        
            }
        | tUnion2:Union All
            {
                Match(tFirstWord, CodeGenerationSupporter.Optimize);
                Match(tSecondWord, CodeGenerationSupporter.Correlated);
                vResult.HintKind = OptimizerHintKind.OptimizeCorrelatedUnionAll;
            }
        )
    | tRecompileOrColumnStore:Identifier
        {
            if (TryMatch(tRecompileOrColumnStore, CodeGenerationSupporter.Recompile))
            {
                Match(tRecompileOrColumnStore, CodeGenerationSupporter.Recompile);
                vResult.HintKind = OptimizerHintKind.Recompile;
            }
            else
            {
                Match(tRecompileOrColumnStore, CodeGenerationSupporter.IgnoreNonClusteredColumnStoreIndex);
                vResult.HintKind = OptimizerHintKind.IgnoreNonClusteredColumnStoreIndex;
            }
        }
    ;

literalOptimizerHint returns [LiteralOptimizerHint vResult = FragmentFactory.CreateFragment<LiteralOptimizerHint>()]
{
    Literal vValue;
}    
    : tIntegerHint:Identifier vValue = integer
        {
            vResult.HintKind = IntegerOptimizerHintHelper.Instance.ParseOption(tIntegerHint, SqlVersionFlags.TSql120);
            vResult.Value = vValue;
        }
    ;

usePlanOptimizerHint returns [LiteralOptimizerHint vResult = FragmentFactory.CreateFragment<LiteralOptimizerHint>()]
{
    Literal vValue;
}
    : tUse:Use Plan vValue = stringLiteral
        {
            vResult.HintKind = OptimizerHintKind.UsePlan;
            vResult.Value = vValue;
        }
    ;
    
optimizeForOptimizerHint returns [OptimizeForOptimizerHint vResult = FragmentFactory.CreateFragment<OptimizeForOptimizerHint>()]
{
    VariableValuePair vPair;
}
    :   tOptimize:Identifier For 
        {
            Match(tOptimize,CodeGenerationSupporter.Optimize);
            vResult.HintKind=OptimizerHintKind.OptimizeFor;
        }
        (
            LeftParenthesis vPair = variableValuePair 
            {
                vResult.IsForUnknown = false;
                AddAndUpdateTokenInfo(vResult, vResult.Pairs, vPair);
            }
            (Comma vPair = variableValuePair
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Pairs, vPair);
                }
            )* 
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        |
            tUnknown:Identifier
            {
                Match(tUnknown, CodeGenerationSupporter.Unknown);
                vResult.IsForUnknown = true;
                UpdateTokenInfo(vResult, tUnknown);
            }
        )
    ;
    
variableValuePair returns [VariableValuePair vResult = FragmentFactory.CreateFragment<VariableValuePair>()]
{
    VariableReference vVariable;
    ScalarExpression vValue;
}
    : vVariable = variable 
        (
            EqualsSign vValue = possibleNegativeConstant
            {
                vResult.Variable = vVariable;
                vResult.Value = vValue;
                vResult.IsForUnknown = false;
            }
        |
            tUnknown:Identifier
            {
                Match(tUnknown, CodeGenerationSupporter.Unknown);
                vResult.Variable = vVariable;
                vResult.IsForUnknown = true;
                UpdateTokenInfo(vResult, tUnknown);
            }
        )
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

createCryptographicProviderStatement returns [CreateCryptographicProviderStatement vResult = this.FragmentFactory.CreateFragment<CreateCryptographicProviderStatement>()]
{
    Identifier vIdentifier;   
    Literal vFileName;
}
    :   tCryptographic:Identifier tProvider:Identifier vIdentifier=identifier
        {
            Match(tCryptographic, CodeGenerationSupporter.Cryptographic);
            Match(tProvider, CodeGenerationSupporter.Provider);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        vFileName = cryptographicProviderFile
        {
            vResult.File = vFileName;
        }
    ;

cryptographicProviderFile returns [Literal vResult = null]
{
    Literal vFile;
}
    : From File EqualsSign vFile=stringLiteral
        {
            vResult = vFile;
        }
    ;

alterCryptographicProviderStatement returns [AlterCryptographicProviderStatement vResult = this.FragmentFactory.CreateFragment<AlterCryptographicProviderStatement>()]
{
    Identifier vIdentifier;   
    Literal vFileName;
}
    :
        tCryptographic:Identifier tProvider:Identifier vIdentifier=identifier
        {
            Match(tCryptographic, CodeGenerationSupporter.Cryptographic);
            Match(tProvider, CodeGenerationSupporter.Provider);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            vFileName=cryptographicProviderFile
            {
                vResult.File = vFileName;
            }
        | 
            tOption:Identifier
            {
                vResult.Option = EnableDisableOptionTypeHelper.Instance.ParseOption(tOption);
            }
        )        
    ;
    
dropCryptographicProviderStatement returns [DropCryptographicProviderStatement vResult = this.FragmentFactory.CreateFragment<DropCryptographicProviderStatement>()]
{
    Identifier vIdentifier;   
}
    :   tCryptographic:Identifier tProvider:Identifier vIdentifier=identifier
        {
            Match(tCryptographic, CodeGenerationSupporter.Cryptographic);
            Match(tProvider, CodeGenerationSupporter.Provider);
            vResult.Name = vIdentifier;
        }    
    ;

alterResourceStatement returns [TSqlStatement vResult = null]
    : tResource:Identifier
        {
            Match(tResource, CodeGenerationSupporter.Resource);
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Pool)}?
            vResult = alterResourcePoolStatement        
        |
            {NextTokenMatches(CodeGenerationSupporter.Governor)}?
            vResult = alterResourceGovernorStatement
        )
    ;  

alterResourceGovernorStatement returns [AlterResourceGovernorStatement vResult = this.FragmentFactory.CreateFragment<AlterResourceGovernorStatement>()]
{
    SchemaObjectName vName;
}
    :    tGovernor:Identifier
        {
            Match(tGovernor, CodeGenerationSupporter.Governor);            
        }
        (
            tDisable:Identifier
            {
                Match(tDisable,CodeGenerationSupporter.Disable);
                vResult.Command = AlterResourceGovernorCommandType.Disable;
                UpdateTokenInfo(vResult,tDisable);
            }
        |
            tReconfigure:Reconfigure
            {
                vResult.Command = AlterResourceGovernorCommandType.Reconfigure;
                UpdateTokenInfo(vResult,tReconfigure);
            }
        |
            tWith:With LeftParenthesis tClassifierFunction:Identifier EqualsSign
            {
                Match(tClassifierFunction, CodeGenerationSupporter.ClassifierFunction);
                vResult.Command = AlterResourceGovernorCommandType.ClassifierFunction;
            }
            (
                vName=schemaObjectNonEmptyTwoPartName
                {
                    vResult.ClassifierFunction = vName;
                }
            |
                tNull:Null
                {
                    vResult.ClassifierFunction = null;
                }
            )
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        |
            tReset:Identifier tStatistics:Statistics
            {
                Match(tReset, CodeGenerationSupporter.Reset);
                vResult.Command = AlterResourceGovernorCommandType.ResetStatistics;
                UpdateTokenInfo(vResult,tStatistics);
            }
        )                
    ;      
        
createResourcePoolStatement returns [CreateResourcePoolStatement vResult = this.FragmentFactory.CreateFragment<CreateResourcePoolStatement>()]
{
    Identifier vIdentifier;   
}
    :
        tResource:Identifier tPool:Identifier vIdentifier=identifier
        {
            Match(tResource, CodeGenerationSupporter.Resource);
            Match(tPool, CodeGenerationSupporter.Pool);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        resourcePoolStatementBody[vResult]
    ;

resourcePoolStatementBody[ResourcePoolStatement vParent]        
{
    ResourcePoolParameter vResourcePoolParameter;
}
    :    (   // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } : 
            tWith:With LeftParenthesis vResourcePoolParameter = resourcePoolParameter
                {
                    UpdateTokenInfo(vParent,tWith);
                    AddAndUpdateTokenInfo(vParent, vParent.ResourcePoolParameters, vResourcePoolParameter);
                }
            (Comma vResourcePoolParameter = resourcePoolParameter
                {
                    AddAndUpdateTokenInfo(vParent, vParent.ResourcePoolParameters, vResourcePoolParameter);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent,tRParen);
            }
        )?    
    ;

resourcePoolParameter returns [ResourcePoolParameter vResult = this.FragmentFactory.CreateFragment<ResourcePoolParameter>()]
{
    Literal vPercent;
    ResourcePoolAffinitySpecification vAffinitySpec;
}
        :
            {NextTokenMatches(CodeGenerationSupporter.Affinity)}?
            vAffinitySpec=resourcePoolAffinitySpecification
            {
                vResult.ParameterType = ResourcePoolParameterType.Affinity;
                vResult.AffinitySpecification = vAffinitySpec;
            }
        |
            tPercent:Identifier 
            {
                vResult.ParameterType = ResourcePoolParameterHelper.Instance.ParseOption(tPercent, SqlVersionFlags.TSql120);
            }
            EqualsSign vPercent=integer 
            {
                Int32 value;
                if (Int32.TryParse(vPercent.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                {
                    if ((vResult.ParameterType == ResourcePoolParameterType.MinCpuPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.MinMemoryPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.MinIoPercent))
                    {
                        if ((value < 0) || (value >100))
                            ThrowParseErrorException("SQL46045", tPercent, TSqlParserResource.SQL46045Message, tPercent.getText());    
                    }
                    else if ((vResult.ParameterType == ResourcePoolParameterType.MaxCpuPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.MaxMemoryPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.CapCpuPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.TargetMemoryPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.MaxIoPercent)
                        || (vResult.ParameterType == ResourcePoolParameterType.CapIoPercent))
                    {
                        if ((value < 1) || (value >100))
                            ThrowParseErrorException("SQL46045", tPercent, TSqlParserResource.SQL46045Message, tPercent.getText());    
                    }
                    else if ((vResult.ParameterType == ResourcePoolParameterType.MinIopsPerVolume)
                        || (vResult.ParameterType == ResourcePoolParameterType.MaxIopsPerVolume))
                    {
                        if (value < 0)
                            ThrowParseErrorException("SQL46045", tPercent, TSqlParserResource.SQL46045Message, tPercent.getText());
                    }
                    vResult.ParameterValue = vPercent;        
                }
                else
                    ThrowIncorrectSyntaxErrorException(vPercent);
            }
    ;    

resourcePoolAffinitySpecification returns [ResourcePoolAffinitySpecification vResult = this.FragmentFactory.CreateFragment<ResourcePoolAffinitySpecification>()]
{
    LiteralRange vPoolAffinityRangeElement;
}
        :   tAffinity:Identifier
            {
                Match(tAffinity, CodeGenerationSupporter.Affinity);
            }
            tAffinityType:Identifier EqualsSign
            {
                vResult.AffinityType = ResourcePoolAffinityHelper.Instance.ParseOption(tAffinityType);
            }
            (
                tAuto:Identifier
                {
                    if (vResult.AffinityType != ResourcePoolAffinityType.Scheduler)
                        ThrowIncorrectSyntaxErrorException(tAuto);
                    Match(tAuto, CodeGenerationSupporter.Auto);
                    vResult.IsAuto = true;
                    UpdateTokenInfo(vResult, tAuto);
                }
            |
                tLParen:LeftParenthesis vPoolAffinityRangeElement=poolAffinityRange
                {
                    UpdateTokenInfo(vResult, tLParen);
                    AddAndUpdateTokenInfo(vResult, vResult.PoolAffinityRanges, vPoolAffinityRangeElement);
                }
                (
                    Comma vPoolAffinityRangeElement=poolAffinityRange
                    {
                        AddAndUpdateTokenInfo(vResult, vResult.PoolAffinityRanges, vPoolAffinityRangeElement);
                    }
                )*
                tRParen:RightParenthesis
                {
                    UpdateTokenInfo(vResult, tRParen);
                }
            )
    ;

poolAffinityRange returns [LiteralRange vResult = this.FragmentFactory.CreateFragment<LiteralRange>()]
{
    IntegerLiteral vFrom;
    IntegerLiteral vTo;
}
        : vFrom=integer
        {
            vResult.From = vFrom;
        }
        (
            To vTo=integer
            {
                vResult.To = vTo;
            }
        )?
    ;

alterResourcePoolStatement returns [AlterResourcePoolStatement vResult = this.FragmentFactory.CreateFragment<AlterResourcePoolStatement>()]
{
    Identifier vIdentifier;
}
    :    tPool:Identifier vIdentifier=identifier
        {
            Match(tPool, CodeGenerationSupporter.Pool);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        resourcePoolStatementBody[vResult]
    ;

dropResourcePoolStatement returns [DropResourcePoolStatement vResult = this.FragmentFactory.CreateFragment<DropResourcePoolStatement>()]
{
    Identifier vIdentifier;
}
    : 
        tResource:Identifier tPool:Identifier vIdentifier=identifier
        {
            Match(tResource, CodeGenerationSupporter.Resource);
            Match(tPool, CodeGenerationSupporter.Pool);
            vResult.Name = vIdentifier;
        }
    ;

createWorkloadGroupStatement returns [CreateWorkloadGroupStatement vResult = this.FragmentFactory.CreateFragment<CreateWorkloadGroupStatement>()]
    :
        workloadGroupStatementBody[vResult]
    ;

alterWorkloadGroupStatement returns [AlterWorkloadGroupStatement vResult = this.FragmentFactory.CreateFragment<AlterWorkloadGroupStatement>()]
    :
        workloadGroupStatementBody[vResult]
    ;

dropWorkloadGroupStatement returns [DropWorkloadGroupStatement vResult = this.FragmentFactory.CreateFragment<DropWorkloadGroupStatement>()]
{
    Identifier vIdentifier;
}
    :    tWorkload:Identifier tGroup:Group vIdentifier=identifier
        {
            Match(tWorkload, CodeGenerationSupporter.Workload);
            vResult.Name = vIdentifier;
        }
    ;

workloadGroupStatementBody[WorkloadGroupStatement vParent]        
{
    Identifier vIdentifier;   
    WorkloadGroupParameter vWorkloadGroupParameter;
    Identifier vPool;   
    long encounteredOptions = 0;
}
    :    tWorkload:Identifier tGroup:Group vIdentifier=identifier
        {
            Match(tWorkload, CodeGenerationSupporter.Workload);
            vParent.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vParent);
        }
        ( 
            options {greedy = true; }: // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            tWith:With 
            {
                UpdateTokenInfo(vParent,tWith);
            }               
            LeftParenthesis vWorkloadGroupParameter = workloadGroupParameter
                {
                    CheckOptionDuplication(ref encounteredOptions, (int)vWorkloadGroupParameter.ParameterType, vWorkloadGroupParameter);
                    AddAndUpdateTokenInfo(vParent, vParent.WorkloadGroupParameters, vWorkloadGroupParameter);
                }
            (tComma:Comma vWorkloadGroupParameter = workloadGroupParameter
                {
                    CheckOptionDuplication(ref encounteredOptions, (int)vWorkloadGroupParameter.ParameterType, vWorkloadGroupParameter);
                    AddAndUpdateTokenInfo(vParent, vParent.WorkloadGroupParameters, vWorkloadGroupParameter);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent, tRParen);
            }            
        )?    
        (        
            // We need this to resolve the ambiguity with the statements that start with an Identifier
            {NextTokenMatches(CodeGenerationSupporter.Using)}?
            tUsing:Identifier
            {
                Match(tUsing, CodeGenerationSupporter.Using);
                UpdateTokenInfo(vParent,tUsing);
            }
            vPool=identifier   
            {
                vParent.PoolName = vPool;
            }            
        )?
    ;
  
workloadGroupParameter returns [WorkloadGroupParameter vResult = null]
{
    Literal vParameter;
}
        : 
            tParameter:Identifier EqualsSign 
                (
                    tImpValue:Identifier
                    {                
                        Match(tParameter,CodeGenerationSupporter.Importance);
                        WorkloadGroupImportanceParameter workloadGroupImportance = this.FragmentFactory.CreateFragment<WorkloadGroupImportanceParameter>();
                        workloadGroupImportance.ParameterType = WorkloadGroupParameterType.Importance;
                        workloadGroupImportance.ParameterValue = ImportanceParameterHelper.Instance.ParseOption(tImpValue);
                        UpdateTokenInfo(workloadGroupImportance, tParameter);
                        UpdateTokenInfo(workloadGroupImportance, tImpValue);
                        vResult = workloadGroupImportance;
                    }
                |
                    vParameter=integer
                    {
                        WorkloadGroupResourceParameter workloadGroupResource = this.FragmentFactory.CreateFragment<WorkloadGroupResourceParameter>();
                        workloadGroupResource.ParameterType = WorkloadGroupResourceParameterHelper.Instance.ParseOption(tParameter, SqlVersionFlags.TSql120);
                        Int32 value;
                        if (Int32.TryParse(vParameter.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out value))
                        {
                            if ((workloadGroupResource.ParameterType == WorkloadGroupParameterType.RequestMaxMemoryGrantPercent)
                                || (workloadGroupResource.ParameterType == WorkloadGroupParameterType.GroupMinMemoryPercent))
                            {
                                if ((value < 0) || (value >100))
                                    ThrowParseErrorException("SQL46045", tParameter, TSqlParserResource.SQL46045Message, tParameter.getText());    
                            }
                            else if ((workloadGroupResource.ParameterType == WorkloadGroupParameterType.RequestMaxCpuTimeSec)
                                || (workloadGroupResource.ParameterType == WorkloadGroupParameterType.RequestMemoryGrantTimeoutSec)
                                || (workloadGroupResource.ParameterType == WorkloadGroupParameterType.GroupMaxRequests))
                            {
                                if (value < 0)
                                    ThrowParseErrorException("SQL46045", tParameter, TSqlParserResource.SQL46045Message, tParameter.getText());    
                            }
                            else if (workloadGroupResource.ParameterType == WorkloadGroupParameterType.MaxDop)
                            {
                                if ((value < 0) || (value >64))
                                    ThrowParseErrorException("SQL46045", tParameter, TSqlParserResource.SQL46045Message, tParameter.getText());    
                            }
                            workloadGroupResource.ParameterValue = vParameter;       
                            UpdateTokenInfo(workloadGroupResource, tParameter);
                            vResult = workloadGroupResource;                                                    
                        }
                        else
                            ThrowIncorrectSyntaxErrorException(vParameter);        
                    }
                )
    ;    

createBrokerPriorityStatement returns [CreateBrokerPriorityStatement vResult = this.FragmentFactory.CreateFragment<CreateBrokerPriorityStatement>()]
{
}
    :
        brokerPriorityStatementBody[vResult]
    ;

alterBrokerPriorityStatement returns [AlterBrokerPriorityStatement vResult = this.FragmentFactory.CreateFragment<AlterBrokerPriorityStatement>()]
{
}
    :
        brokerPriorityStatementBody[vResult]
    ;    

dropBrokerPriorityStatement returns [DropBrokerPriorityStatement vResult = this.FragmentFactory.CreateFragment<DropBrokerPriorityStatement>()]
{
    Identifier vIdentifier;
}
    : 
        tBroker:Identifier tPriority:Identifier vIdentifier=identifier
        {
            Match(tBroker, CodeGenerationSupporter.Broker);
            Match(tPriority, CodeGenerationSupporter.Priority);
            vResult.Name = vIdentifier;
        }
    ;

brokerPriorityStatementBody[BrokerPriorityStatement vParent]        
{
    Identifier vIdentifier;   
    BrokerPriorityParameter vBrokerPriorityParameter;
    int encounteredOptions = 0;
}
        :
        tBroker:Identifier tPriority:Identifier vIdentifier=identifier
        {
            Match(tBroker, CodeGenerationSupporter.Broker);
            Match(tPriority, CodeGenerationSupporter.Priority);
            vParent.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vParent);
        }
        tFor:For tForClause:Identifier
        {
            Match(tForClause, CodeGenerationSupporter.Conversation);
        }
        ( 
            options {greedy = true; }: // Greedy due to conflict with setStatements
            tSet:Set 
            {
                UpdateTokenInfo(vParent,tSet);
            }               
            LeftParenthesis vBrokerPriorityParameter = brokerPriorityParameter[encounteredOptions]
                {
                    UpdateBrokerPriorityEncounteredOptions(ref encounteredOptions, vBrokerPriorityParameter);
                    AddAndUpdateTokenInfo(vParent, vParent.BrokerPriorityParameters, vBrokerPriorityParameter);
                }
            (tComma:Comma vBrokerPriorityParameter = brokerPriorityParameter[encounteredOptions]
                {
                    UpdateBrokerPriorityEncounteredOptions(ref encounteredOptions, vBrokerPriorityParameter);
                    AddAndUpdateTokenInfo(vParent, vParent.BrokerPriorityParameters, vBrokerPriorityParameter);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vParent,tRParen);
            }
        )?    
    ;

brokerPriorityParameter [int encountered]returns [BrokerPriorityParameter vResult = this.FragmentFactory.CreateFragment<BrokerPriorityParameter>()]
{
    Literal vParameter;
    Identifier vParameter1;
    Literal vParameter2;
}
    : 
        tParameter:Identifier 
        {
            vResult.ParameterType = BrokerPriorityParameterHelper.Instance.ParseOption(tParameter);
            CheckBrokerPriorityParameterDuplication(encountered,vResult.ParameterType,tParameter);
        }
        EqualsSign 
        (
            vParameter1=identifier
            {
                // should not be remote service name or priority_level
                if ((vResult.ParameterType == BrokerPriorityParameterType.ContractName)
                    || (vResult.ParameterType == BrokerPriorityParameterType.LocalServiceName))
                {
                    vResult.ParameterValue = IdentifierOrValueExpression(vParameter1);
                    vResult.IsDefaultOrAny = BrokerPriorityParameterSpecialType.None;
                }
                else
                    ThrowIncorrectSyntaxErrorException(tParameter);
            }
        |
            vParameter2=stringLiteral
            {
                if (vResult.ParameterType == BrokerPriorityParameterType.RemoteServiceName)
                {
                    vResult.ParameterValue = IdentifierOrValueExpression(vParameter2);
                    vResult.IsDefaultOrAny = BrokerPriorityParameterSpecialType.None;
                }
                else
                    ThrowIncorrectSyntaxErrorException(tParameter);
                    
            }
        |
            vParameter=integer
            {
                if (vResult.ParameterType == BrokerPriorityParameterType.PriorityLevel)
                {
                    vResult.ParameterValue = IdentifierOrValueExpression(vParameter);
                    vResult.IsDefaultOrAny = BrokerPriorityParameterSpecialType.None;
                }                            
                else
                    ThrowIncorrectSyntaxErrorException(tParameter);
            }
        |
            tDefault:Default
            {
                if (vResult.ParameterType == BrokerPriorityParameterType.PriorityLevel)
                {
                    vResult.IsDefaultOrAny = BrokerPriorityParameterSpecialType.Default;
                }
                else
                    ThrowIncorrectSyntaxErrorException(tParameter);                            
            }
        |
            tAny:Any
            {
                if (vResult.ParameterType != BrokerPriorityParameterType.PriorityLevel)
                {
                    vResult.IsDefaultOrAny = BrokerPriorityParameterSpecialType.Any;
                }
                else
                    ThrowIncorrectSyntaxErrorException(tParameter);
            }                                        
        )
    ;
    
createSequenceStatement returns [CreateSequenceStatement vResult = this.FragmentFactory.CreateFragment<CreateSequenceStatement>()]
{
    SchemaObjectName vName = null;
 }
    :   tSequence:Identifier vName = schemaObjectTwoPartName
        {
            Match(tSequence, CodeGenerationSupporter.Sequence);
            vResult.Name = vName;
        }
        (
            options {greedy = true; } :
            createSequenceOptionList[vResult]
        )?
    ;

createSequenceOptionList [CreateSequenceStatement vParent]
{
    SequenceOption vSequenceOption;
    long encounteredOptions = 0;    
}   :
      (
        options {greedy = true; } :
        vSequenceOption = createSequenceOptionListElement
        {
            CheckOptionDuplication(ref encounteredOptions, (int)vSequenceOption.OptionKind, vSequenceOption);
            AddAndUpdateTokenInfo<SequenceOption>(vParent, vParent.SequenceOptions, vSequenceOption);    
        }
      )+
    ;
            

alterSequenceOptionListElement returns [SequenceOption vResult]
    :       
            {NextTokenMatches(CodeGenerationSupporter.Restart)}?
            vResult=sequenceRestartOptionListElement
        | 
            vResult=commonSequenceOptionListElement
    ;

createSequenceOptionListElement returns [SequenceOption vResult]
    :      vResult=sequenceDatatypeOptionListElement
        |
           {NextTokenMatches(CodeGenerationSupporter.Start)}?
            vResult=sequenceStartOptionListElement
        |
            vResult=commonSequenceOptionListElement
    ;
            

commonSequenceOptionListElement returns [SequenceOption vResult]
    :
        (
            {NextTokenMatches(CodeGenerationSupporter.Cache)}?
            vResult=sequenceCacheOptionListElement
        | 
            {NextTokenMatches(CodeGenerationSupporter.Cycle)}?
            vResult=sequenceCycleOptionListElement
        |
            {NextTokenMatches(CodeGenerationSupporter.Increment)}?
            vResult=sequenceIncrementOptionListElement
        |
            {NextTokenMatches(CodeGenerationSupporter.No)}?
            vResult=sequenceNoOptionListElement
        |
            vResult=sequenceMinMaxOptionListElement
        )
    ;

sequenceDatatypeOptionListElement returns [DataTypeSequenceOption vResult = this.FragmentFactory.CreateFragment<DataTypeSequenceOption>()]
{
    DataTypeReference vDataType = null;
}
    :    tAs:As vDataType=scalarDataType 
        {
            UpdateTokenInfo(vResult, tAs);
            vResult.OptionKind = SequenceOptionKind.As;                
            vResult.DataType = vDataType;
        }
    ;

sequenceCacheOptionListElement returns [ScalarExpressionSequenceOption vResult = this.FragmentFactory.CreateFragment<ScalarExpressionSequenceOption>()]
{
    ScalarExpression vExpression = null;
}
    :    tCache:Identifier (vExpression=seedIncrement)?
        {
            UpdateTokenInfo(vResult,tCache);
            vResult.OptionValue = vExpression;
            vResult.OptionKind = SequenceOptionKind.Cache;                            
        }
    ;

sequenceCycleOptionListElement returns [SequenceOption vResult = this.FragmentFactory.CreateFragment<SequenceOption>()]
    :    tCycle:Identifier
        {    
            UpdateTokenInfo(vResult,tCycle);
            vResult.OptionKind = SequenceOptionKind.Cycle;                
        }
    ;

sequenceIncrementOptionListElement returns [ScalarExpressionSequenceOption vResult = this.FragmentFactory.CreateFragment<ScalarExpressionSequenceOption>()]
{
    ScalarExpression vExpression = null;
}
    :    tIncrement:Identifier tBy:By vExpression=seedIncrement
        {
            UpdateTokenInfo(vResult, tIncrement);
            vResult.OptionValue = vExpression;
            vResult.OptionKind = SequenceOptionKind.Increment;                
        }
    ;

sequenceNoOptionListElement returns [SequenceOption vResult = this.FragmentFactory.CreateFragment<SequenceOption>()]
    :    tNo:Identifier tNoOption:Identifier
        {
            if (TryMatch(tNoOption, CodeGenerationSupporter.MinValue))
            {
                vResult.OptionKind = SequenceOptionKind.MinValue;
            }
            else if (TryMatch(tNoOption, CodeGenerationSupporter.MaxValue))
            {
                vResult.OptionKind = SequenceOptionKind.MaxValue;
            }
            else if (TryMatch(tNoOption, CodeGenerationSupporter.Cache))
            {
                vResult.OptionKind = SequenceOptionKind.Cache;
            }                
            else
            {
                Match(tNoOption, CodeGenerationSupporter.Cycle);
                vResult.OptionKind = SequenceOptionKind.Cycle;
            }
            vResult.NoValue = true;
            UpdateTokenInfo(vResult, tNo);
            UpdateTokenInfo(vResult, tNoOption);
        }
    ;

sequenceStartOptionListElement returns [ScalarExpressionSequenceOption vResult = this.FragmentFactory.CreateFragment<ScalarExpressionSequenceOption>()]
{
    ScalarExpression vExpression = null;
}
    :    tStart:Identifier tWith:With vExpression=seedIncrement
        {
            UpdateTokenInfo(vResult, tStart);
            vResult.OptionKind = SequenceOptionKind.Start;                
            vResult.OptionValue = vExpression;
        }
    ;

sequenceRestartOptionListElement returns [ScalarExpressionSequenceOption vResult = this.FragmentFactory.CreateFragment<ScalarExpressionSequenceOption>()]
{
    ScalarExpression vExpression = null;
}
    :    tRestart:Identifier 
        {
            UpdateTokenInfo(vResult, tRestart);
            vResult.OptionKind = SequenceOptionKind.Restart;   
        }
        (
            options {greedy=true;} : //greedy due to conflict with WITH statements
            tRestartWith:With 
            vExpression=seedIncrement
            {             
                vResult.OptionValue = vExpression;
            }
        )?
    ;

sequenceMinMaxOptionListElement returns [ScalarExpressionSequenceOption vResult = this.FragmentFactory.CreateFragment<ScalarExpressionSequenceOption>()]
{
    ScalarExpression vExpression = null;
}
    :    tOption:Identifier vExpression=seedIncrement
        {
            if (TryMatch(tOption, CodeGenerationSupporter.MinValue))
            {
                vResult.OptionKind = SequenceOptionKind.MinValue;
            }
            else
            {
                Match(tOption, CodeGenerationSupporter.MaxValue);
                vResult.OptionKind = SequenceOptionKind.MaxValue;
            }
            UpdateTokenInfo(vResult, tOption);
            vResult.OptionValue = vExpression;
        }
    ;

createSpatialIndexStatement returns [CreateSpatialIndexStatement vResult = this.FragmentFactory.CreateFragment<CreateSpatialIndexStatement>()]
{
    Identifier vIdentifier;   
    SchemaObjectName vObject;
    Identifier vSpatialColumnName;
    IdentifierOrValueExpression vTSqlFragment;
}
    :   tSpatial:Identifier tIndex:Index vIdentifier=identifier
        {
            Match(tSpatial, CodeGenerationSupporter.Spatial);
            vResult.Name = vIdentifier;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        tOn:On vObject = schemaObjectFourPartName
        {
            vResult.Object = vObject;
        }
        tLParen:LeftParenthesis vSpatialColumnName=identifier tRParen:RightParenthesis
        {
            vResult.SpatialColumnName = vSpatialColumnName;
            UpdateTokenInfo(vResult,tRParen);
        }
        (
            // Can end with Identifier - and that conflicts with identifierStatements, so making it greedy...
            options {greedy=true;} :
            spatialIndexingScheme[vResult]
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            tWith:With spatialIndexOptionsList[vResult]
        )?            
        (
            On vTSqlFragment=stringOrIdentifier
            {
                vResult.OnFileGroup = vTSqlFragment;
            }
        )?
    ;

spatialIndexingScheme [CreateSpatialIndexStatement vParent]
    :    tUsing:Identifier 
        {    
            Match(tUsing, CodeGenerationSupporter.Using);
        }
        tIndexingScheme:Identifier
        {
            vParent.SpatialIndexingScheme = SpatialIndexingSchemeTypeHelper.Instance.ParseOption(tIndexingScheme, SqlVersionFlags.TSql120);
            UpdateTokenInfo(vParent, tIndexingScheme);            
        }
    ;

spatialIndexOptionsList [CreateSpatialIndexStatement vParent]
{
    SpatialIndexOption vSpatialIndexOption;    
    bool isRegularIndexOption = false;
}
    :    
        LeftParenthesis vSpatialIndexOption=spatialIndexOption[vParent.SpatialIndexingScheme, ref isRegularIndexOption]
        {    
            AddAndUpdateTokenInfo(vParent,vParent.SpatialIndexOptions,vSpatialIndexOption);
        }
        (
            Comma vSpatialIndexOption=spatialIndexOption[vParent.SpatialIndexingScheme, ref isRegularIndexOption]
            {    
                AddAndUpdateTokenInfo(vParent,vParent.SpatialIndexOptions,vSpatialIndexOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

spatialIndexOption [SpatialIndexingSchemeType indexingScheme, ref bool isRegularIndexOption] returns [SpatialIndexOption vResult = null]
    :
        (
            {NextTokenMatches(CodeGenerationSupporter.BoundingBox)}?
            vResult=boundingBoxSpatialIndexOption[isRegularIndexOption, indexingScheme]
        |
            {NextTokenMatches(CodeGenerationSupporter.Grids)}?
            vResult=gridsSpatialIndexOption[isRegularIndexOption]
        |
            {NextTokenMatches(CodeGenerationSupporter.CellsPerObject)}?
            vResult=cellsPerObjectSpatialIndexOption[isRegularIndexOption]
        |
            vResult = spatialIndexRegularOption[IndexAffectingStatement.CreateSpatialIndex]
            {
                isRegularIndexOption = true;
            }
        )
    ;

spatialIndexRegularOption [IndexAffectingStatement statement] returns [SpatialIndexRegularOption vResult = this.FragmentFactory.CreateFragment<SpatialIndexRegularOption>()]
{
    IndexOption vIndexOption;
}    
    :    (
            vIndexOption = indexOption
            {
                VerifyAllowedIndexOption120(statement, vIndexOption);
                CheckIfValidSpatialIndexOptionValue(statement, vIndexOption);
                vResult.Option = vIndexOption;
            }
        )
    ;

boundingBoxSpatialIndexOption [bool isRegularIndexOption, SpatialIndexingSchemeType indexingScheme] returns [BoundingBoxSpatialIndexOption vResult = this.FragmentFactory.CreateFragment<BoundingBoxSpatialIndexOption>()]
    :    tIdentifier:Identifier EqualsSign 
        {
            Match(tIdentifier,CodeGenerationSupporter.BoundingBox);    
            if (isRegularIndexOption == true)
                ThrowParseErrorException("SQL46081", tIdentifier, 
                    TSqlParserResource.SQL46081Message, tIdentifier.getText());
            if (indexingScheme == SpatialIndexingSchemeType.GeographyGrid)
                ThrowParseErrorException("SQL46067", tIdentifier, TSqlParserResource.SQL46067Message, tIdentifier.getText());            
        }
        boundingBoxParameter[vResult]
        {
            if (vResult.BoundingBoxParameters.Count != 4)
                ThrowParseErrorException("SQL46066", tIdentifier, TSqlParserResource.SQL46066Message, tIdentifier.getText());
        }            
    ;    

boundingBoxParameter [BoundingBoxSpatialIndexOption vParent]
    :    tLParen:LeftParenthesis
        (
            boundingBoxParameterListByname[vParent]
        |
            boundingBoxParameterListByord[vParent]
        )            
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

boundingBoxParameterListByname [BoundingBoxSpatialIndexOption vParent]
{
    BoundingBoxParameter vBoundingBoxParameter;
    int encounteredOptions = 0;
}
    :    vBoundingBoxParameter=boundingBoxParameterByName[encounteredOptions]
        {    
            UpdateBoundingBoxParameterEncounteredOptions(ref encounteredOptions, vBoundingBoxParameter);
            AddAndUpdateTokenInfo(vParent,vParent.BoundingBoxParameters,vBoundingBoxParameter);
        }
        (
            Comma vBoundingBoxParameter=boundingBoxParameterByName[encounteredOptions]
            {    
                UpdateBoundingBoxParameterEncounteredOptions(ref encounteredOptions, vBoundingBoxParameter);
                AddAndUpdateTokenInfo(vParent,vParent.BoundingBoxParameters,vBoundingBoxParameter);
            }
        )*
    ;

boundingBoxParameterByName [int encountered] returns [BoundingBoxParameter vResult = this.FragmentFactory.CreateFragment<BoundingBoxParameter>()]
{
    ScalarExpression vValue;
}
    :    tIdentifier:Identifier EqualsSign vValue = signedIntegerOrReal
        {
                vResult.Parameter = BoundingBoxParameterTypeHelper.Instance.ParseOption(tIdentifier);
                CheckBoundingBoxParameterDuplication(encountered, vResult.Parameter, tIdentifier);
                vResult.Value = vValue;
        }        
    ;        

boundingBoxParameterListByord [BoundingBoxSpatialIndexOption vParent]
{
    BoundingBoxParameter vBoundingBoxParameter;
}
    : vBoundingBoxParameter=boundingBoxParameterByOrd
        {    
            AddAndUpdateTokenInfo(vParent,vParent.BoundingBoxParameters,vBoundingBoxParameter);
        }
        (
            Comma vBoundingBoxParameter=boundingBoxParameterByOrd
            {    
                AddAndUpdateTokenInfo(vParent,vParent.BoundingBoxParameters,vBoundingBoxParameter);
            }
        )*
    ;    

boundingBoxParameterByOrd returns [BoundingBoxParameter vResult = this.FragmentFactory.CreateFragment<BoundingBoxParameter>()]
{
    ScalarExpression vValue;
}
    :    vValue = signedIntegerOrReal
        {
            vResult.Parameter = BoundingBoxParameterType.None;
            vResult.Value = vValue;
        }        
    ;            

gridsSpatialIndexOption[bool isRegularIndexOption] returns [GridsSpatialIndexOption vResult = this.FragmentFactory.CreateFragment<GridsSpatialIndexOption>()]
    :    tIdentifier:Identifier EqualsSign
        {
            Match(tIdentifier,CodeGenerationSupporter.Grids);
            if (isRegularIndexOption == true)
                ThrowParseErrorException("SQL46081", tIdentifier, 
                    TSqlParserResource.SQL46081Message, tIdentifier.getText());
        }
        gridParameter[vResult]
    ;        

gridParameter [GridsSpatialIndexOption vParent]
    :    tLParen:LeftParenthesis
        (
            gridParameterListByName[vParent]
        |
            gridParameterListByOrd[vParent]
        )    
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
    ;

gridParameterListByName [GridsSpatialIndexOption vParent]
{
    GridParameter vGridParameter;
}
    :    vGridParameter=gridParameterByName
        {    
            AddAndUpdateTokenInfo(vParent,vParent.GridParameters,vGridParameter);
        }
        (
            Comma vGridParameter=gridParameterByName
            {    
                AddAndUpdateTokenInfo(vParent,vParent.GridParameters,vGridParameter);
            }
        )*
    ;    

gridParameterByName returns [GridParameter vResult = this.FragmentFactory.CreateFragment<GridParameter>()]
    :
        tParameter:Identifier EqualsSign tValue:Identifier
        {
            vResult.Parameter = GridParameterTypeHelper.Instance.ParseOption(tParameter);
            vResult.Value = ImportanceParameterHelper.Instance.ParseOption(tValue);
        }
    ;

gridParameterListByOrd [GridsSpatialIndexOption vParent]
{
    GridParameter vGridParameter;
}
    :    vGridParameter=gridParameterByOrd
        {    
            AddAndUpdateTokenInfo(vParent,vParent.GridParameters,vGridParameter);
        }
        (
            Comma vGridParameter=gridParameterByOrd
            {    
                AddAndUpdateTokenInfo(vParent,vParent.GridParameters,vGridParameter);
            }
        )*
    ;        
    
gridParameterByOrd returns [GridParameter vResult = this.FragmentFactory.CreateFragment<GridParameter>()]
    :
        tValue:Identifier
        {
            vResult.Parameter = GridParameterType.None;
            vResult.Value = ImportanceParameterHelper.Instance.ParseOption(tValue);
        }
    ;    

cellsPerObjectSpatialIndexOption[bool isRegularIndexOption] returns [CellsPerObjectSpatialIndexOption vResult = this.FragmentFactory.CreateFragment<CellsPerObjectSpatialIndexOption>()]
{
    Literal vValue;
}
    :    tCellsPerObject:Identifier EqualsSign vValue = integer
        {
            Match(tCellsPerObject,CodeGenerationSupporter.CellsPerObject);
            if (isRegularIndexOption == true)
                ThrowParseErrorException("SQL46081", tCellsPerObject, 
                    TSqlParserResource.SQL46081Message, tCellsPerObject.getText());
            UpdateTokenInfo(vResult,tCellsPerObject);
            CheckForCellsPerObjectValueRange(vValue);
            vResult.Value = vValue;
        }
    ;        
        
createDefaultStatement returns [CreateDefaultStatement vResult = this.FragmentFactory.CreateFragment<CreateDefaultStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    ScalarExpression vExpression;
}
    :    tCreate:Create Default vSchemaObjectName=schemaObjectThreePartName
        {
            UpdateTokenInfo(vResult,tCreate);
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
    long encounteredOptions=0;
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
        vSelectStatement = subqueryExpressionWithOptionalCTE
        {
            vResult.SelectStatement = vSelectStatement;
        }
        (With Check tOption:Option
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

methodSpecifier returns [MethodSpecifier vResult = this.FragmentFactory.CreateFragment<MethodSpecifier>()]
{
    Identifier vIdentifier;
}
    :
        tExternal:External tName:Identifier
        {
            Match(tName,CodeGenerationSupporter.Name);
        }
        vIdentifier=identifier
        {
            vResult.AssemblyName = vIdentifier;
        }
        Dot vIdentifier=identifier
        {
            vResult.ClassName = vIdentifier;
        }
        Dot vIdentifier=identifier
        {
            vResult.MethodName = vIdentifier;
        }
    ;

triggerOption[bool vOwnerProhibited] returns [TriggerOption vResult = null]
{
    ExecuteAsTriggerOption vExecuteAsTriggerOption;
    ExecuteAsClause vExecuteAsClause;
}
    :
        tOption:Identifier
        {
            vResult = this.FragmentFactory.CreateFragment<TriggerOption>();
            vResult.OptionKind = TriggerOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    |
        vExecuteAsClause=executeAsClause[false, vOwnerProhibited]
        {
            vExecuteAsTriggerOption = this.FragmentFactory.CreateFragment<ExecuteAsTriggerOption>();
            vExecuteAsTriggerOption.OptionKind = TriggerOptionKind.ExecuteAsClause;
            vExecuteAsTriggerOption.ExecuteAsClause = vExecuteAsClause;
            vResult = vExecuteAsTriggerOption;
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

procedureOption returns [ProcedureOption vResult=null]
{
    ExecuteAsClause vExecuteAs;
    ExecuteAsProcedureOption vExecuteAsProcedureOption;
}
    : tOption:Identifier
        {
            vResult=FragmentFactory.CreateFragment<ProcedureOption>();
            vResult.OptionKind=ProcedureOptionHelper.Instance.ParseOption(tOption);
            UpdateTokenInfo(vResult,tOption);
        }
    | vExecuteAs=executeAsClause[false, false]
        {
            vExecuteAsProcedureOption=FragmentFactory.CreateFragment<ExecuteAsProcedureOption>();
            vExecuteAsProcedureOption.ExecuteAs = vExecuteAs;
            vExecuteAsProcedureOption.OptionKind=ProcedureOptionKind.ExecuteAs;
            vResult=vExecuteAsProcedureOption;
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
    MethodSpecifier vMethodSpecifier;
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
        (
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
        )
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
            optSemicolons[vResult]
            (
                vStatementList = statementList[ref vParseErrorOccurred]
                {
                    vResult.StatementList = vStatementList;
                }
            )?
        |
            vMethodSpecifier=methodSpecifier  optSemicolons[vResult]
            {
                vResult.MethodSpecifier = vMethodSpecifier;
            }
        )
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
            scalarProcedureParameter[vResult, true, true]
        )
    ;
    
scalarProcedureParameter[ProcedureParameter vParent, bool outputAllowed, bool nullableAllowed]
{
    DataTypeReference vDataType;
    ScalarExpression vDefault;
    NullableConstraintDefinition vNullableConstraintDefinition;
}
    :    vDataType=scalarDataType
        {
            vParent.DataType = vDataType;
        }
        (
            vNullableConstraintDefinition=nullableConstraint
            {
                if (nullableAllowed)
                    vParent.Nullable=vNullableConstraintDefinition;
                else
                    ThrowParseErrorException("SQL46039", vNullableConstraintDefinition, TSqlParserResource.SQL46039Message);                
            }
        )? 
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
                if (TryMatch(tId2, CodeGenerationSupporter.ReadOnlyOld))
                {
                    vParent.Modifier = ParameterModifier.ReadOnly;
                }
                else
                {
                    Match(tId2,CodeGenerationSupporter.Output,CodeGenerationSupporter.Out);
                    if (outputAllowed)
                        vParent.Modifier = ParameterModifier.Output;
                    else
                        ThrowParseErrorException("SQL46039", tId2, TSqlParserResource.SQL46039Message);
                }
                    
                UpdateTokenInfo(vParent,tId2);
            }
        )?
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
    MethodSpecifier vMethodSpecifier;
    bool vDdlTrigger = false;
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
            if (vResult.TriggerObject.TriggerScope != TriggerScope.Normal)
            {
                vDdlTrigger = true;
            }
        }
        ( With 
            vTriggerOption=triggerOption[vDdlTrigger]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vTriggerOption);
            } 
            (
                Comma vTriggerOption=triggerOption[vDdlTrigger]
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vTriggerOption);
                } 
            )*
        )?
        (
            {vDdlTrigger}?
            ddlTriggerMidSection[vResult]
        |
            dmlTriggerMidSection[vResult]
        )
        tAs:As
        {
            UpdateTokenInfo(vResult,tAs);
        }
        (    
            ( (tSemi:Semicolon)* vStatementList = statementList[ref vParseErrorOccurred]
                {
                    vResult.StatementList = vStatementList;
                }
            )
        |
            vMethodSpecifier=methodSpecifier  optSemicolons[vResult]
            {
                vResult.MethodSpecifier = vMethodSpecifier;
            }
        )
    ;
    
dmlTriggerMidSection[TriggerStatementBody vParent]
{
    bool ofAppeared = false;
    TriggerAction vAction;
    int encounteredOptions = 0;    
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
            CheckDmlTriggerActionDuplication(encounteredOptions, vAction);        
            AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
            UpdateDmlTriggerActionEncounteredOptions(ref encounteredOptions, vAction);            
        }
        ( 
            Comma vAction=dmlTriggerAction
            {
                CheckDmlTriggerActionDuplication(encounteredOptions, vAction);
                AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
                UpdateDmlTriggerActionEncounteredOptions(ref encounteredOptions, vAction);                
            }
        )*
        (    Not For Replication 
            {
                vParent.IsNotForReplication = true;
            }
        )?
    ;

ddlTriggerMidSection[TriggerStatementBody vParent]
{
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
            {
                Match(tId2,CodeGenerationSupporter.After);
                vParent.TriggerType = TriggerType.After;
            }   
        )
        (
            {!NextTokenMatches(CodeGenerationSupporter.Logon)}?
            (
                vAction=ddlTriggerAction
                {
                    AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
                }
                (     
                    Comma vAction=ddlTriggerAction
                    {
                        AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
                    }
                )*
            )
        |
            tLogon:Identifier
            {
                Match(tLogon, CodeGenerationSupporter.Logon);
                if (vParent.TriggerObject.TriggerScope == TriggerScope.AllServer)
                {
                    vAction = FragmentFactory.CreateFragment<TriggerAction>();
                    vAction.TriggerActionType = TriggerActionType.LogOn;
                    AddAndUpdateTokenInfo(vParent, vParent.TriggerActions, vAction);
                }        
                else
                    ThrowParseErrorException("SQL46044", tLogon, TSqlParserResource.SQL46044Message);
            }
        )      
    ;


triggerObject returns [TriggerObject vResult = FragmentFactory.CreateFragment<TriggerObject>()]
{
    SchemaObjectName vSchemaObjectName;
    TriggerScope vTriggerScope;
}
    : 
        vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.Name = vSchemaObjectName;
            vResult.TriggerScope = TriggerScope.Normal;
        }
    |
        vTriggerScope=triggerScope[vResult]
        {
            vResult.TriggerScope = vTriggerScope;
        }
    ;

triggerScope[TSqlFragment vParent] returns [TriggerScope vResult = TriggerScope.Normal]
    :
        tDatabase:Database
        {
            UpdateTokenInfo(vParent,tDatabase);
            vResult = TriggerScope.Database;
        }
    | 
        tAll:All tServer:Identifier
        {
            Match(tServer,CodeGenerationSupporter.Server);
            UpdateTokenInfo(vParent,tAll);
            UpdateTokenInfo(vParent,tServer);
            vResult = TriggerScope.AllServer;
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

ddlTriggerAction returns [TriggerAction vResult = FragmentFactory.CreateFragment<TriggerAction>()]
    :    
        tIdentifier:Identifier
        {
            vResult.TriggerActionType = TriggerActionType.Event;
            
            EventNotificationEventType eventTypeValue;
            if (TriggerEventTypeHelper.Instance.TryParseOption(tIdentifier, SqlVersionFlags.TSql120, out eventTypeValue))
            {
                vResult.EventTypeGroup = CreateEventTypeContainer(eventTypeValue, tIdentifier);
            }
            else
            {
                EventNotificationEventGroup eventGroupValue = TriggerEventGroupHelper.Instance.ParseOption(tIdentifier, SqlVersionFlags.TSql120);
                vResult.EventTypeGroup = CreateEventGroupContainer(eventGroupValue, tIdentifier);
            }
        }
    ;

// CONVERSATION-related stuff (Begin/End/MOVE, GET CONVERSATION Group)

endConversationStatement returns [EndConversationStatement vResult = FragmentFactory.CreateFragment<EndConversationStatement>()]
{
    ScalarExpression vConv;
}
    : End tConversation:Identifier vConv = expression 
        {
            Match(tConversation,CodeGenerationSupporter.Conversation);
            vResult.Conversation = vConv;
        }
        endConversationArgumentsOpt[vResult]
    ;
    
endConversationArgumentsOpt [EndConversationStatement vParent]
{
    ValueExpression vErrorCode, vErrorDescription;
}
    : (options {greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        With tErrorCleanup:Identifier
        (
            /* empty */
            {
                Match(tErrorCleanup,CodeGenerationSupporter.Cleanup);
                vParent.WithCleanup = true;
                UpdateTokenInfo(vParent,tErrorCleanup);
            }
        |
            EqualsSign vErrorCode = integerOrVariable tDescription:Identifier EqualsSign vErrorDescription = stringOrVariable
            {
                Match(tDescription,CodeGenerationSupporter.Description);
                vParent.ErrorCode = vErrorCode;
                vParent.ErrorDescription = vErrorDescription;
            }
        )
      )?
    ;
    
moveConversationStatement returns [MoveConversationStatement vResult = FragmentFactory.CreateFragment<MoveConversationStatement>()]
{
    ScalarExpression vConv, vGroup;
}
    : tMove:Identifier tConversation:Identifier vConv = expression To vGroup = expression
        {
            Match(tMove,CodeGenerationSupporter.Move);
            Match(tConversation,CodeGenerationSupporter.Conversation);
            UpdateTokenInfo(vResult,tMove);
            vResult.Conversation = vConv;
            vResult.Group = vGroup;
        }
    ;
    
getConversationGroupStatement returns [GetConversationGroupStatement vResult = FragmentFactory.CreateFragment<GetConversationGroupStatement>()]
{
    VariableReference vGroupId;
    SchemaObjectName vQueue;
}
    : tGet:Identifier tConversation:Identifier Group vGroupId = variable From vQueue = schemaObjectThreePartName
        {
            Match(tGet,CodeGenerationSupporter.Get);
            Match(tConversation,CodeGenerationSupporter.Conversation);
            UpdateTokenInfo(vResult,tGet);
            vResult.GroupId = vGroupId;
            vResult.Queue = vQueue;
        }
    ;
    
receiveStatement returns [ReceiveStatement vResult = FragmentFactory.CreateFragment<ReceiveStatement>()]
{
    ScalarExpression vExpression;
    SelectElement vSelectElement;
    SchemaObjectName vQueue;
    VariableTableReference vInto;
    ValueExpression vWhere;
}
    : tReceive:Identifier
        {
            Match(tReceive, CodeGenerationSupporter.Receive);
            UpdateTokenInfo(vResult,tReceive);
        }
        (Top LeftParenthesis vExpression = expression RightParenthesis
            {
                vResult.Top = vExpression;
            }
        )? 
        vSelectElement = receiveSelectExpression 
        {
            AddAndUpdateTokenInfo(vResult, vResult.SelectElements, vSelectElement);
        }
        (Comma vSelectElement = receiveSelectExpression
            {
                AddAndUpdateTokenInfo(vResult, vResult.SelectElements, vSelectElement);
            }
        )* 
        From vQueue = schemaObjectThreePartName
        {
            vResult.Queue = vQueue;
        }
        (Into vInto = variableDmlTarget
            {
                vResult.Into = vInto;
            }
        )?
        (Where tGroupIdOrHandle:Identifier EqualsSign vWhere = stringOrVariable
            {
                if (TryMatch(tGroupIdOrHandle, CodeGenerationSupporter.ConversationGroupId))
                    vResult.IsConversationGroupIdWhere = true;
                else
                {
                    Match(tGroupIdOrHandle, CodeGenerationSupporter.ConversationHandle);
                    vResult.IsConversationGroupIdWhere = false;
                }
                vResult.Where = vWhere;
            }
        )?
        ;

receiveSelectExpression returns [SelectElement vResult]
    : 
      vResult = selectSetVariable
    |     
      (selectStarExpression)=>
      vResult = selectStarExpression    
    | vResult = receiveColumnSelectExpression
    ;
    
receiveColumnSelectExpression returns [SelectScalarExpression vResult = FragmentFactory.CreateFragment<SelectScalarExpression>()]
{
    ScalarExpression vExpression;
    IdentifierOrValueExpression vColumnName;
}
    : vExpression = expression 
        {
                vResult.Expression = vExpression;            
        }
        ((As)? vColumnName = stringOrIdentifier
            {
                vResult.ColumnName = vColumnName;
            }
        )?
    ;

sendStatement returns [SendStatement vResult = FragmentFactory.CreateFragment<SendStatement>()]
{
    ScalarExpression vExpression;
    IdentifierOrValueExpression vMessageType;
}
    : tSend:Identifier On tConversation:Identifier
        {
            Match(tSend,CodeGenerationSupporter.Send);
            Match(tConversation,CodeGenerationSupporter.Conversation);
            UpdateTokenInfo(vResult,tSend);
        }
        (   
            options {greedy = true;} : // Conflicts with Select, which can also start from '(', so making it greedy...
            {LA(1)==LeftParenthesis}?
            LeftParenthesis vExpression = expression
            {
                AddAndUpdateTokenInfo(vResult, vResult.ConversationHandles, vExpression);
            }
            (
                Comma vExpression = expression
                {
                    AddAndUpdateTokenInfo(vResult, vResult.ConversationHandles, vExpression);
                }
            )*
            RightParenthesis
        |   
            vExpression = expression
            {
                AddAndUpdateTokenInfo(vResult, vResult.ConversationHandles, vExpression);
            }
        )
        (   
            {NextTokenMatches(CodeGenerationSupporter.Message)}?
            tMessage:Identifier tType:Identifier vMessageType = identifierOrVariable
            {
                Match(tMessage,CodeGenerationSupporter.Message);
                Match(tType,CodeGenerationSupporter.Type);
                vResult.MessageTypeName = vMessageType;
            }
        )?
        (options {greedy = true;} : // Conflicts with Select, which can also start from '(', so making it greedy...
            LeftParenthesis vExpression = expression tRParen:RightParenthesis
            {
                vResult.MessageBody = vExpression;
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;
    
beginConversationTimerStatement returns [BeginConversationTimerStatement vResult = FragmentFactory.CreateFragment<BeginConversationTimerStatement>()]
{
    ScalarExpression vHandle, vTimeout;
}
    : tBegin:Begin tConversation:Identifier tTimer:Identifier 
        {
            Match(tConversation,CodeGenerationSupporter.Conversation);
            Match(tTimer,CodeGenerationSupporter.Timer);
            UpdateTokenInfo(vResult,tBegin);
        }
        LeftParenthesis vHandle = expression RightParenthesis tTimeout:Identifier EqualsSign vTimeout = expression
        {
            vResult.Handle = vHandle;
            Match(tTimeout,CodeGenerationSupporter.Timeout);
            vResult.Timeout = vTimeout;
        }
    ;
    
beginDialogStatement returns [BeginDialogStatement vResult = FragmentFactory.CreateFragment<BeginDialogStatement>()]
{
    VariableReference vHandle;
    ValueExpression vInstanceSpec, vTargetService;
    IdentifierOrValueExpression vInitiatorName, vContract;
    DialogOption vOption;
    long encounteredOptions = 0;
}
    : tBegin:Begin tDialog:Identifier 
        {
            Match(tDialog,CodeGenerationSupporter.Dialog);
            UpdateTokenInfo(vResult,tBegin);
        }
        (tConversation:Identifier
            {
                Match(tConversation,CodeGenerationSupporter.Conversation);
                vResult.IsConversation = true;
            }
        )? 
        vHandle = variable From tService:Identifier vInitiatorName = identifierOrVariable To tService2:Identifier vTargetService = stringOrVariable
        {
            Match(tService,CodeGenerationSupporter.Service);
            Match(tService2,CodeGenerationSupporter.Service);
            vResult.Handle = vHandle;
            vResult.InitiatorServiceName = vInitiatorName;
            vResult.TargetServiceName = vTargetService;
        }
        (Comma vInstanceSpec = stringOrVariable
            {
                vResult.InstanceSpec = vInstanceSpec;
            }
        )?
        (On tContract:Identifier vContract = identifierOrVariable
            {
                Match(tContract,CodeGenerationSupporter.Contract);
                vResult.ContractName = vContract;
            }
        )?
        (    options {greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            With 
            vOption=beginConversationArgument[ref encounteredOptions]
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
            }
            (
                Comma vOption=beginConversationArgument[ref encounteredOptions]
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                }
            )*
        )?
    ;

beginConversationArgument[ref long encounteredOptions] returns [DialogOption vResult]
    : tOption:Identifier EqualsSign
        (
            vResult=scalarExpressionBeginDialogConversationArgument[tOption, ref encounteredOptions]
            | vResult=onOffBeginDialogConversationArgument[tOption, ref encounteredOptions]
        )
    ;

scalarExpressionBeginDialogConversationArgument[IToken tOption, ref long encounteredOptions] returns [ScalarExpressionDialogOption vResult=FragmentFactory.CreateFragment<ScalarExpressionDialogOption>()]
{
    ScalarExpression vValue;
}
    :    vValue = expression
        {
            if (TryMatch(tOption,CodeGenerationSupporter.RelatedConversation))
            {
                vResult.OptionKind=DialogOptionKind.RelatedConversation;
                vResult.Value = vValue;
                CheckOptionDuplication(ref encounteredOptions, (int)DialogOptionKind.RelatedConversationGroup, tOption);
                CheckOptionDuplication(ref encounteredOptions, (int)DialogOptionKind.RelatedConversation, tOption);
            }
            else if (TryMatch(tOption,CodeGenerationSupporter.RelatedConversationGroup))
            {
                vResult.OptionKind=DialogOptionKind.RelatedConversationGroup;
                vResult.Value = vValue;
                CheckOptionDuplication(ref encounteredOptions, (int)DialogOptionKind.RelatedConversationGroup, tOption);
                CheckOptionDuplication(ref encounteredOptions, (int)DialogOptionKind.RelatedConversation, tOption);
            }
            else
            {
                Match(tOption,CodeGenerationSupporter.LifeTime);
                vResult.OptionKind=DialogOptionKind.Lifetime;
                vResult.Value = vValue;
                CheckOptionDuplication(ref encounteredOptions, (int)vResult.OptionKind, tOption);
            }
        }
    ;

onOffBeginDialogConversationArgument[IToken tOption, ref long encounteredOptions] returns [OnOffDialogOption vResult=FragmentFactory.CreateFragment<OnOffDialogOption>()]
{
    OptionState vOptionState;
}
    : vOptionState = optionOnOff[vResult]
        {
            Match(tOption,CodeGenerationSupporter.Encryption);
            vResult.OptionKind=DialogOptionKind.Encryption;
            CheckOptionDuplication(ref encounteredOptions, (int)vResult.OptionKind, tOption);
            vResult.OptionState = vOptionState;
        }
    ;
    
// Merge STATEMENT
mergeStatement [SubDmlFlags subDmlFlags] returns [MergeStatement vResult]
    :
        vResult = mergeStatementWithoutSemicolon[subDmlFlags]
        requiredSemicolon[vResult, CodeGenerationSupporter.Merge]
    ;

requiredSemicolon[TSqlFragment vParent, string vStatement]
    :
        (
            options { warnWhenFollowAmbig=false; } :
            tSemi:Semicolon
            {
                UpdateTokenInfo(vParent,tSemi);
            }
        )?
        {
            if(tSemi==null)
            {
                ThrowParseErrorException("SQL46097", vParent,TSqlParserResource.SQL46097Message, vStatement);
            }
        }
    ;

mergeStatementWithoutSemicolon [SubDmlFlags subDmlFlags] returns [MergeStatement vResult = FragmentFactory.CreateFragment<MergeStatement>()]
{
    MergeSpecification vMergeSpecification;
}
    :
        vMergeSpecification = mergeSpecification [subDmlFlags]
        {
            vResult.MergeSpecification = vMergeSpecification;
        }
        (
            optimizerHints[vResult, vResult.OptimizerHints]
        )?
    ;

mergeSpecification [SubDmlFlags subDmlFlags] returns [MergeSpecification vResult = this.FragmentFactory.CreateFragment<MergeSpecification>()]
{
    TableReference vDmlTarget;
    Identifier vAlias;
    BooleanExpression vSearchCondition;
    TableReference vTableReference;
    MergeActionClause vActionClause;
}
    :    tMerge:Merge dmlTopRowFilterOpt[vResult] (Into)?
        {
            UpdateTokenInfo(vResult,tMerge);
        }
        vDmlTarget=dmlTarget[true]
        {
            vResult.Target = vDmlTarget;
        }
        (
            (
                As vAlias = identifier
                {
                    vResult.TableAlias = vAlias;
                }
            )
            | 
            {!NextTokenMatches(CodeGenerationSupporter.Using)}?
            (
                vAlias = identifier
                {
                    vResult.TableAlias = vAlias;
                }
            )
            |
            /* empty */
        )
        tUsing:Identifier
        {
            Match(tUsing, CodeGenerationSupporter.Using);
        }
        vTableReference = selectTableReferenceWithOdbc[subDmlFlags | SubDmlFlags.MergeUsing]
        {
            vResult.TableReference = vTableReference;
        }
        On vSearchCondition=booleanExpression
        {
            vResult.SearchCondition = vSearchCondition;
        }
        (vActionClause = mergeActionClause
            {
                AddAndUpdateTokenInfo(vResult, vResult.ActionClauses, vActionClause);
            }
        )+
        outputClauseOpt[subDmlFlags, vResult]
    ;    
    
mergeActionClause returns [MergeActionClause vResult = FragmentFactory.CreateFragment<MergeActionClause>()]
{
    BooleanExpression vSearchCondition;
    MergeAction vAction;
    MergeCondition vCondition;
}
    :
        When vCondition = mergeCondition
        {
            vResult.Condition = vCondition;
        }
        (
            And vSearchCondition = booleanExpression
            {
                vResult.SearchCondition = vSearchCondition;
            }
        )?
        Then vAction = mergeAction[vCondition]
        {
            vResult.Action = vAction;
        }
    ;
    
mergeCondition returns [MergeCondition vCondition = MergeCondition.NotSpecified]
    :
        tMatched:Identifier
        {
            Match(tMatched, CodeGenerationSupporter.Matched);
            vCondition = MergeCondition.Matched;
        }
    |
        (
            Not tMatched2:Identifier
            {
                Match(tMatched2, CodeGenerationSupporter.Matched);
            }
            (
                By tTargetSource:Identifier
                {
                    if (TryMatch(tTargetSource, CodeGenerationSupporter.Target))
                        vCondition = MergeCondition.NotMatchedByTarget;
                    else
                    {
                        Match(tTargetSource, CodeGenerationSupporter.Source);
                        vCondition = MergeCondition.NotMatchedBySource;
                    }
                }
            |
                /* empty */
                {
                    vCondition = MergeCondition.NotMatched;
                }
            )
        )
    ;
    
mergeAction [MergeCondition condition] returns [MergeAction vResult]
    :    vResult = updateMergeAction[condition]
    |    vResult = insertMergeAction[condition]
    |    vResult = deleteMergeAction[condition]
    ;
    
updateMergeAction [MergeCondition condition] returns [UpdateMergeAction vResult = FragmentFactory.CreateFragment<UpdateMergeAction>()]
    :    
        tUpdate:Update setClausesList[vResult, vResult.SetClauses]
        {
            if (condition == MergeCondition.NotMatched || condition == MergeCondition.NotMatchedByTarget)
            {
                ThrowParseErrorException("SQL46041", tUpdate,TSqlParserResource.SQL46041Message, "Update");
            }
            UpdateTokenInfo(vResult,tUpdate);
        }
    ;

insertMergeAction [MergeCondition condition] returns [InsertMergeAction vResult = FragmentFactory.CreateFragment<InsertMergeAction>()]
{
    ValuesInsertSource vInsertSource;
}
    :    tInsert:Insert mergeInsertDmlColumnListOpt[vResult] vInsertSource = mergeInsertSource
        {
            if (condition == MergeCondition.Matched || 
                condition == MergeCondition.NotMatchedBySource)
            {
                ThrowParseErrorException("SQL46040", tInsert,TSqlParserResource.SQL46040Message);
            }
            UpdateTokenInfo(vResult, tInsert);
            vResult.Source = vInsertSource;
        }
    ;
    
deleteMergeAction [MergeCondition condition] returns [DeleteMergeAction vResult = FragmentFactory.CreateFragment<DeleteMergeAction>()]
    :
        tDelete:Delete
        {
            if (condition == MergeCondition.NotMatched || condition == MergeCondition.NotMatchedByTarget)
            {
                ThrowParseErrorException("SQL46041", tDelete,TSqlParserResource.SQL46041Message, "Delete");
            }
            UpdateTokenInfo(vResult, tDelete);
        }
    ;
    
mergeInsertSource returns [ValuesInsertSource vResult = FragmentFactory.CreateFragment<ValuesInsertSource>()]
{
    RowValue vRowValue;
}
    :
        defaultValuesInsertSource[vResult]
    |
        tValues:Values vRowValue = rowValueExpressionWithDefault
        {
            UpdateTokenInfo(vResult,tValues);
            AddAndUpdateTokenInfo(vResult, vResult.RowValues, vRowValue);
        }
    ;    
    
// Execute As STATEMENT
executeAsStatement returns [ExecuteAsStatement vResult = FragmentFactory.CreateFragment<ExecuteAsStatement>()]
{
    ExecuteContext vContext;
}
    : execStart[vResult] As vContext = executeAsStatementContext
        {
            vResult.ExecuteContext = vContext;
        }
        executeContextStatementOptionsOpt[vResult]
    ;
    
executeAsStatementContext returns [ExecuteContext vResult = FragmentFactory.CreateFragment<ExecuteContext>()]
{
    ScalarExpression vPrincipal;
}
    : User EqualsSign vPrincipal = expression
        {
            vResult.Kind = ExecuteAsOption.User;
            vResult.Principal = vPrincipal;
        }        
    | tLoginCaller:Identifier
        ( /* empty */
            {
                Match(tLoginCaller,CodeGenerationSupporter.Caller);
                vResult.Kind = ExecuteAsOption.Caller;
                UpdateTokenInfo(vResult,tLoginCaller);
            }
        | 
            EqualsSign vPrincipal = expression
            {
                Match(tLoginCaller,CodeGenerationSupporter.Login);
                vResult.Kind = ExecuteAsOption.Login;
                vResult.Principal = vPrincipal;
            }    
        )
    ;
    
executeContextStatementOptionsOpt [ExecuteAsStatement vParent]
{
    VariableReference vVariable;
}
    : (options {greedy = true; } : // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        With 
            (
                tNo:Identifier tRevert:Revert
                {
                    Match(tNo,CodeGenerationSupporter.No);
                    vParent.WithNoRevert = true;
                    UpdateTokenInfo(vParent,tRevert);
                }
            | 
                tCookie:Identifier Into vVariable = variable
                {
                    Match(tCookie,CodeGenerationSupporter.Cookie);
                    vParent.Cookie = vVariable;
                }
            )
        )?
    ;
    
/////////////////////////////////////////////////
// Execute STATEMENT
/////////////////////////////////////////////////

executeStatement returns [ExecuteStatement vResult = FragmentFactory.CreateFragment<ExecuteStatement>()]
{
    ExecuteSpecification vExecuteSpecification;
    ExecuteOption vOption;
    long encountered = 0;
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
                CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
            }
            (
                Comma vOption = executeOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options, vOption);
                    CheckOptionDuplication(ref encountered, (int)vOption.OptionKind, vOption);
                }
            )*
        )?
    ;

executeOption returns [ExecuteOption vResult]
    : {NextTokenMatches(CodeGenerationSupporter.Recompile)}?
        vResult=executeOptionRecompile
      | vResult=executeOptionResultSets
    ;

executeOptionRecompile returns [ExecuteOption vResult = FragmentFactory.CreateFragment<ExecuteOption>()]
    : tRecompile:Identifier // RECOMPILE
        {
            Match(tRecompile, CodeGenerationSupporter.Recompile);
            vResult.OptionKind = ExecuteOptionKind.Recompile;
            UpdateTokenInfo(vResult, tRecompile);
        }
    ;

executeOptionResultSets returns [ResultSetsExecuteOption vResult = FragmentFactory.CreateFragment<ResultSetsExecuteOption>()]
{
    ResultSetDefinition vResultSetDefinition;
}
    : tResult:Identifier tSets:Identifier
        {
            Match(tResult, CodeGenerationSupporter.Result);
            Match(tSets, CodeGenerationSupporter.Sets);
            vResult.OptionKind = ExecuteOptionKind.ResultSets;
        }
        (
            tIdentifier:Identifier
            {
                if(TryMatch(tIdentifier, CodeGenerationSupporter.Undefined))
                {
                    vResult.ResultSetsOptionKind = ResultSetsOptionKind.Undefined;
                }
                else
                {
                    Match(tIdentifier, CodeGenerationSupporter.None);
                    vResult.ResultSetsOptionKind = ResultSetsOptionKind.None;
                }
                UpdateTokenInfo(vResult, tIdentifier);
            }
        | 
            LeftParenthesis 
            vResultSetDefinition=resultSetDefinition
            {
                vResult.ResultSetsOptionKind = ResultSetsOptionKind.ResultSetsDefined;
                AddAndUpdateTokenInfo(vResult, vResult.Definitions, vResultSetDefinition);
            }
            (
                Comma vResultSetDefinition=resultSetDefinition
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Definitions, vResultSetDefinition);
                }
            )*
            tRightParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRightParen);
            }
        )
    ;

resultSetDefinition returns [ResultSetDefinition vResult]
    :     vResult=inlineResultSetDefinition
      |   vResult=asForXmlResultSetDefinition
      |   vResult=asSchemaObjectResultSetDefinition
    ;

inlineResultSetDefinition returns [InlineResultSetDefinition vResult=FragmentFactory.CreateFragment<InlineResultSetDefinition>()]
{
    ResultColumnDefinition vResultColumnDefinition;
}
    : LeftParenthesis vResultColumnDefinition=resultColumnDefinition
        {
            AddAndUpdateTokenInfo(vResult, vResult.ResultColumnDefinitions, vResultColumnDefinition);
        }
        (
            Comma vResultColumnDefinition=resultColumnDefinition
            {
                AddAndUpdateTokenInfo(vResult, vResult.ResultColumnDefinitions, vResultColumnDefinition);
            }    
        )*
        tRightParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRightParen);
        }
    ;

resultColumnDefinition returns [ResultColumnDefinition vResult = FragmentFactory.CreateFragment<ResultColumnDefinition>()]
{
    ColumnDefinitionBase vColumnDefinition;
    NullableConstraintDefinition vNullableConstraintDefinition;
}
    :   vColumnDefinition=columnDefinitionBasic
        {
            vResult.ColumnDefinition = vColumnDefinition;
        }
        (
            vNullableConstraintDefinition=nullableConstraint
            {
                vResult.Nullable=vNullableConstraintDefinition;
            }
        )?
    ;
                
asForXmlResultSetDefinition returns [ResultSetDefinition vResult = FragmentFactory.CreateFragment<ResultSetDefinition>()]
    : As For tXml:Identifier
        {
            Match(tXml, CodeGenerationSupporter.Xml);
            vResult.ResultSetType=ResultSetType.ForXml;
            UpdateTokenInfo(vResult, tXml);
        }
    ;

asSchemaObjectResultSetDefinition returns [SchemaObjectResultSetDefinition vResult = FragmentFactory.CreateFragment<SchemaObjectResultSetDefinition>()]
{
    SchemaObjectName vSchemaObjectName;
}
    : As tIdentifier:Identifier vSchemaObjectName=schemaObjectFourPartName
        {
            if(TryMatch(tIdentifier, CodeGenerationSupporter.Object))
            {
                vResult.ResultSetType=ResultSetType.Object;
            }
            else
            {
                Match(tIdentifier, CodeGenerationSupporter.Type);
                vResult.ResultSetType=ResultSetType.Type;
            }
            vResult.Name=vSchemaObjectName;
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
    ExecuteContext vExecuteCtxt;
    VariableReference vVar;
    Identifier vLinkedSvr;
    ExecutableEntity vExecutable;
}
    :    LeftParenthesis vExecutable = execStrTypes tRparen:RightParenthesis 
        {
            UpdateTokenInfo(vParent,tRparen);
            vParent.ExecutableEntity = vExecutable;
        }
        (vExecuteCtxt = execStrExecCtxt
            {
                vParent.ExecuteContext = vExecuteCtxt;
            }
        )? 
        (
            {NextTokenMatches(CodeGenerationSupporter.At)}?
            vLinkedSvr = linkedServer
            {
                vParent.LinkedServer = vLinkedSvr;
            }
        |  /* empty */
        )
    |    vExecutable = execProcEx
        {
            vParent.ExecutableEntity = vExecutable;
        }
    |   vVar = variable EqualsSign vExecutable = execProcEx
        {
            vParent.Variable = vVar;
            vParent.ExecutableEntity = vExecutable;
        }
    ;
    
execStrExecCtxt returns [ExecuteContext vResult]
    :    As vResult = execCtxtStmt
    ;
    
execCtxtStmt returns [ExecuteContext vResult = FragmentFactory.CreateFragment<ExecuteContext>()]
{
    Literal vPrincipal;
}
    : execCtxtStmtType[vResult]
    (vPrincipal = execCtxtStmtPrincipal
        {
            vResult.Principal = vPrincipal;
        }
    )?
    ;

execCtxtStmtPrincipal returns [Literal vResult = null]
    :    EqualsSign vResult = stringLiteral
    ;

execStrTypes returns [ExecutableEntity vResult]
    :    vResult = execSqlList (Comma setParamList[vResult])?
    
// TODO, olegr: Check if we need this and when...
//    |   tDecrypt:Identifier LeftParenthesis vResult = execEncryptedList RightParenthesis // ID = DECRYPT / DECRYPT_A
//        {
//            Match(tDecrypt, CodeGenerationSupporter.Decrypt);
//        }
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

linkedServer returns [Identifier vResult]
    :   tAt : Identifier vResult = identifier    // ID = AT
        {
            Match(tAt, CodeGenerationSupporter.At);
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
    | vResult = numeric
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
    
execCtxtStmtType [ExecuteContext vParent]
    : tUser:User
        {
            UpdateTokenInfo(vParent,tUser);
            vParent.Kind = ExecuteAsOption.User;
        }
    | tLogin:Identifier    // ID = LOGIN
        {
            Match(tLogin, CodeGenerationSupporter.Login);
            UpdateTokenInfo(vParent,tLogin);
            vParent.Kind = ExecuteAsOption.Login;
        }
    ;    

/////////////////////////////////////////////////
// Execute STATEMENT ENDS
/////////////////////////////////////////////////

createTableStatement returns [CreateTableStatement vResult = this.FragmentFactory.CreateFragment<CreateTableStatement>()]
{
    SchemaObjectName vSchemaObjectName;
    TableDefinition vTableDefinition;
    FederationScheme vFederationScheme;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
}
    : tCreate:Create Table vSchemaObjectName=schemaObjectThreePartName 
        {
            UpdateTokenInfo(vResult,tCreate);
            vResult.SchemaObjectName = vSchemaObjectName;
            ThrowPartialAstIfPhaseOne(vResult);
        }
        (
            (
                LeftParenthesis 
                vTableDefinition = tableDefinitionCreateTable
                {
                    vResult.Definition = vTableDefinition;
                }
                tRParen:RightParenthesis
                {
                    UpdateTokenInfo(vResult,tRParen);
                }
            )
            | 
                As tFileTable:Identifier
                {
                    Match(tFileTable, CodeGenerationSupporter.FileTable);
                    vResult.AsFileTable=true;
                    UpdateTokenInfo(vResult, tFileTable);
                }
        )

        // Default is not used as a keyword after on or textimage_on (even though it is a keyword)
        // So we don't need to check for an optional Default keyword, default will always be quoted
        ( 
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        (
            {NextTokenMatches(CodeGenerationSupporter.Federated)}?
            vFederationScheme = federatedOn[vResult]
            {
                vResult.FederationScheme = vFederationScheme;
            }
        )?
        largeDataOnOpt[vResult]
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            withTableOptions[vResult]
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

federatedOn [CreateTableStatement vParent] returns [FederationScheme vResult = this.FragmentFactory.CreateFragment<FederationScheme>()]
{
    Identifier vDistribution;
    Identifier vColumnName;
}
    : identifier On LeftParenthesis 
        vDistribution = identifier
        {
            vResult.DistributionName = vDistribution;
        }
        EqualsSign vColumnName = identifier
        {
            vResult.ColumnName = vColumnName;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

largeDataOnOpt [CreateTableStatement vParent]
    : 
        (    // Greedy due to conflict with identifierStatements
            options {greedy = true; } : largeDataOn[vParent]
            
            (
                // Greedy due to conflict with identifierStatements
                options {greedy = true; } : largeDataOn[vParent]
            )?
        )?
    ;
    
largeDataOn [CreateTableStatement vParent]
{
    IdentifierOrValueExpression vValue;
}
    :    tTextImageFileStreamOn:Identifier vValue = stringOrIdentifier
        {
            if (TryMatch(tTextImageFileStreamOn, CodeGenerationSupporter.TextImageOn))
            {
                if (vParent.TextImageOn != null)
                {
                    ThrowParseErrorException("SQL46058", tTextImageFileStreamOn, 
                        TSqlParserResource.SQL46058Message, tTextImageFileStreamOn.getText());
                }
                vParent.TextImageOn = vValue;
            }
            else
            {
                Match(tTextImageFileStreamOn, CodeGenerationSupporter.FileStreamOn);
                if (vParent.FileStreamOn != null)
                {
                    ThrowParseErrorException("SQL46058", tTextImageFileStreamOn, 
                        TSqlParserResource.SQL46058Message, tTextImageFileStreamOn.getText());
                }
                vParent.FileStreamOn = vValue;
            }
        }
    ;

withTableOptions[CreateTableStatement vParent]
{
    TableOption vTableOption;
}
    : With LeftParenthesis vTableOption=createTableOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.Options, vTableOption);
        }
        (Comma vTableOption = createTableOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.Options, vTableOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

createTableOption returns [TableOption vResult]
    : 
        {NextTokenMatches(CodeGenerationSupporter.DataCompression)}?
        vResult = tableDataCompressionOption
      | 
        {NextTokenMatches(CodeGenerationSupporter.FileTableDirectory)}?
        vResult = fileTableDirectoryTableOption
      | 
        {NextTokenMatches(CodeGenerationSupporter.FileTableCollateFileName)}?
        vResult = fileTableCollateFileNameTableOption
      | 
        {NextTokenMatchesOneOf(new string[] {CodeGenerationSupporter.FileTablePrimaryKeyConstraintName,
                                 CodeGenerationSupporter.FileTableStreamIdUniqueConstraintName,
                                 CodeGenerationSupporter.FileTableFullPathUniqueConstraintName})}?
        vResult = fileTableConstraintNameTableOption
      |      
        {NextTokenMatches(CodeGenerationSupporter.MemoryOptimized)}?
        vResult = memoryOptimizedTableOption
      |      
        {NextTokenMatches(CodeGenerationSupporter.Durability)}?
        vResult = durabilityTableOption
    ;

memoryOptimizedTableOption returns [MemoryOptimizedTableOption vResult =  FragmentFactory.CreateFragment<MemoryOptimizedTableOption>()]
{
    OptionState vOptionState;
}
    : tMemoryOptimized:Identifier EqualsSign vOptionState=optionOnOff[vResult]
        {
            Match(tMemoryOptimized, CodeGenerationSupporter.MemoryOptimized);
            vResult.OptionKind = TableOptionKind.MemoryOptimized; 
            vResult.OptionState = vOptionState;
        }
    ;

durabilityTableOption returns [DurabilityTableOption vResult =  FragmentFactory.CreateFragment<DurabilityTableOption>()]
    :   tDurability:Identifier EqualsSign tDurabilityTableOptionKind:Identifier
        {
            Match(tDurability, CodeGenerationSupporter.Durability);
            vResult.OptionKind = TableOptionKind.Durability;
            vResult.DurabilityTableOptionKind = DurabilityTableOptionHelper.Instance.ParseOption(tDurabilityTableOptionKind);                        
        }
    ;

fileTableDirectoryTableOption returns [FileTableDirectoryTableOption vResult = FragmentFactory.CreateFragment<FileTableDirectoryTableOption>()]
{
    Literal vDirectoryName;
}
    : tFileTableDirectory:Identifier EqualsSign vDirectoryName=stringLiteralOrNull
        {
            Match(tFileTableDirectory, CodeGenerationSupporter.FileTableDirectory);
            vResult.OptionKind=TableOptionKind.FileTableDirectory;
            vResult.Value=vDirectoryName;
        }
    ;

fileTableCollateFileNameTableOption returns [FileTableCollateFileNameTableOption vResult = FragmentFactory.CreateFragment<FileTableCollateFileNameTableOption>()]
{
    Identifier vCollation;
}
    : tFileTableCollateFileName:Identifier EqualsSign vCollation=nonQuotedIdentifier
        {
            Match(tFileTableCollateFileName, CodeGenerationSupporter.FileTableCollateFileName);
            vResult.OptionKind=TableOptionKind.FileTableCollateFileName;
            vResult.Value=vCollation;
        }
    ;

fileTableConstraintNameTableOption returns [FileTableConstraintNameTableOption vResult = FragmentFactory.CreateFragment<FileTableConstraintNameTableOption>()]
{
    Identifier vConstraint;
}
    : tFileTableConstraintName:Identifier EqualsSign vConstraint=identifier
        {
            vResult.Value=vConstraint;
            if (TryMatch(tFileTableConstraintName, CodeGenerationSupporter.FileTablePrimaryKeyConstraintName))
            {
                vResult.OptionKind=TableOptionKind.FileTablePrimaryKeyConstraintName;
            }
            else if (TryMatch(tFileTableConstraintName, CodeGenerationSupporter.FileTableStreamIdUniqueConstraintName))
            {
                vResult.OptionKind=TableOptionKind.FileTableStreamIdUniqueConstraintName;
            }
            else if (TryMatch(tFileTableConstraintName, CodeGenerationSupporter.FileTableFullPathUniqueConstraintName))
            {
                vResult.OptionKind=TableOptionKind.FileTableFullPathUniqueConstraintName;
            }
            else
            {
                ThrowIncorrectSyntaxErrorException(tFileTableConstraintName);
            }
        }
    ;

tableDataCompressionOption returns [TableDataCompressionOption vResult = FragmentFactory.CreateFragment<TableDataCompressionOption>()]
{
    DataCompressionOption vOption;
}
    : vOption=dataCompressionOption
        { 
            vResult.DataCompressionOption=vOption;
            vResult.OptionKind=TableOptionKind.DataCompression;
        }
    ;
    
dataCompressionOption returns [DataCompressionOption vResult = FragmentFactory.CreateFragment<DataCompressionOption>()]
{
    CompressionPartitionRange vRange;
}
    : tDataCompression:Identifier EqualsSign tCompressionLevel:Identifier
        {
            Match(tDataCompression, CodeGenerationSupporter.DataCompression);
            vResult.CompressionLevel = DataCompressionLevelHelper.Instance.ParseOption(tCompressionLevel);
            vResult.OptionKind = IndexOptionKind.DataCompression;
            UpdateTokenInfo(vResult, tDataCompression);
            UpdateTokenInfo(vResult, tCompressionLevel);
        }
        ( On tPartitions:Identifier LeftParenthesis vRange = compressionPartitionRange
            {
                Match(tPartitions, CodeGenerationSupporter.Partitions);
                AddAndUpdateTokenInfo(vResult, vResult.PartitionRanges, vRange);
            }
            (Comma vRange = compressionPartitionRange
                {
                    AddAndUpdateTokenInfo(vResult, vResult.PartitionRanges, vRange);
                }
            )*
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen);
            }
        )?
    ;
    
compressionPartitionRange returns [CompressionPartitionRange vResult = FragmentFactory.CreateFragment<CompressionPartitionRange>()]
{
    ScalarExpression vExpression;
}
    :    vExpression = expression
        {
            vResult.From = vExpression;
        }
        (To vExpression = expression
            {
                vResult.To = vExpression;
            }
        )?
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
        |   {NextTokenMatches(CodeGenerationSupporter.Switch)}?
            vResult=alterTableSwitchStatement
        |   (
                (With vExistingRowsCheck = constraintEnforcement)?
                (
                    vResult=alterTableAddTableElementStatement[vExistingRowsCheck]
                |   
                    vResult=alterTableConstraintModificationStatement[vExistingRowsCheck]
                )
            )
        |    {NextTokenMatches(CodeGenerationSupporter.Rebuild)}?
            vResult=alterTableRebuildStatement
        |   
            {NextTokenMatches(CodeGenerationSupporter.ChangeTracking, 2)}? 
            vResult=alterTableChangeTrackingModificationStatement
        | 
            {NextTokenMatches(CodeGenerationSupporter.FileTableNamespace, 2)}? 
            vResult=alterTableFileTableNamespaceStatement
        |   vResult=alterTableSetStatement 
        )
        {   
            // Update position later, because instantiation is lazy
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

alterTableRebuildStatement returns [AlterTableRebuildStatement vResult = FragmentFactory.CreateFragment<AlterTableRebuildStatement>()]
{
    PartitionSpecifier vPartition = null;
    IndexAffectingStatement statementKind = IndexAffectingStatement.AlterTableRebuildAllPartitions;
}
    :    tRebuild:Identifier
        {
            Match(tRebuild, CodeGenerationSupporter.Rebuild);
            UpdateTokenInfo(vResult,tRebuild);
        }
        (
            vPartition = partitionSpecifier
            {
                vResult.Partition = vPartition;
                if (!vPartition.All)
                    statementKind = IndexAffectingStatement.AlterTableRebuildOnePartition;
            }
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With indexOptionList[statementKind, vResult.IndexOptions, vResult]
            {
                CheckPartitionAllSpecifiedForIndexRebuild(vResult.Partition, vResult.IndexOptions);
            }
        )?
    ;    

alterTableChangeTrackingModificationStatement returns [AlterTableChangeTrackingModificationStatement vResult = FragmentFactory.CreateFragment<AlterTableChangeTrackingModificationStatement>()]
{
    OptionState vOptionState;
}
    :    tEnableDisable:Identifier tChangeTracking:Identifier
        {
            Match(tChangeTracking, CodeGenerationSupporter.ChangeTracking);
            vResult.IsEnable = EnableDisableMatcher(tEnableDisable, true, false);
            UpdateTokenInfo(vResult,tChangeTracking);
        }
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With LeftParenthesis tTrackColumnsUpdated:Identifier EqualsSign vOptionState = optionOnOff[vResult] tRParen:RightParenthesis
            {
                Match(tTrackColumnsUpdated, CodeGenerationSupporter.TrackColumnsUpdated);
                if (!vResult.IsEnable)
                {
                    ThrowIncorrectSyntaxErrorException(tEnableDisable);
                }
                vResult.TrackColumnsUpdated = vOptionState;
                UpdateTokenInfo(vResult, tRParen);
            }
        )?
    ;

alterTableFileTableNamespaceStatement returns [AlterTableFileTableNamespaceStatement vResult = FragmentFactory.CreateFragment<AlterTableFileTableNamespaceStatement>()]
    : tEnableDisable:Identifier tFileTableNamespace:Identifier
        {
            Match(tFileTableNamespace, CodeGenerationSupporter.FileTableNamespace);
            vResult.IsEnable = EnableDisableMatcher(tEnableDisable, true, false);
            UpdateTokenInfo(vResult, tFileTableNamespace);
        }
    ;
        

alterTableSetStatement returns [AlterTableSetStatement vResult = FragmentFactory.CreateFragment<AlterTableSetStatement>()]
{
    TableOption vTableOption;
}
    :    Set LeftParenthesis vTableOption = tableOption
            {
                AddAndUpdateTokenInfo(vResult, vResult.Options,vTableOption);
            }
            (Comma vTableOption = tableOption
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Options,vTableOption);
                }
            )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult, tRParen);
        }        
    ;
    
tableOption returns [TableOption vResult = null]
    :
        {NextTokenMatches(CodeGenerationSupporter.LockEscalation)}?
        vResult = lockEscalationTableOption
    |
        {NextTokenMatches(CodeGenerationSupporter.FileStreamOn)}?
        vResult = fileStreamOnTableOption
    | 
        {NextTokenMatches(CodeGenerationSupporter.FileTableDirectory)}?
        vResult = fileTableDirectoryTableOption
    ;
    
lockEscalationTableOption returns [LockEscalationTableOption vResult = FragmentFactory.CreateFragment<LockEscalationTableOption>()]
    :
        tLockEscalation:Identifier EqualsSign
        {
            Match(tLockEscalation, CodeGenerationSupporter.LockEscalation);
            vResult.OptionKind=TableOptionKind.LockEscalation;
        }
        (
            tAutoDisable:Identifier
            {
                vResult.Value = LockEscalationMethodHelper.Instance.ParseOption(tAutoDisable);
                UpdateTokenInfo(vResult,tAutoDisable);
            }
        |
            tTable:Table
            {
                vResult.Value = LockEscalationMethod.Table;
                UpdateTokenInfo(vResult,tTable);
            }
        )
    ;

fileStreamOnTableOption returns [FileStreamOnTableOption vResult = FragmentFactory.CreateFragment<FileStreamOnTableOption>()]
{
    IdentifierOrValueExpression vValue;
}
    :    tFileStreamOn:Identifier EqualsSign vValue = stringOrIdentifier
        {
            Match(tFileStreamOn, CodeGenerationSupporter.FileStreamOn);
            vResult.OptionKind=TableOptionKind.FileStreamOn;
            vResult.Value = vValue;
        }
    ;
    
alterTableAlterColumnStatement returns [AlterTableAlterColumnStatement vResult = this.FragmentFactory.CreateFragment<AlterTableAlterColumnStatement>()]
{
    Identifier vIdentifier;
    DataTypeReference vDataType;
    bool vIsNull;
    bool vAddDefined = false;
    ColumnStorageOptions vStorageOptions;
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
            (
                options {greedy = true; } : // Due to conflict with identifierStatements
                vStorageOptions = columnStorage[IndexAffectingStatement.AlterTableAddElement, vDataType]
                {
                    vResult.StorageOptions = vStorageOptions;
                }
            )?
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
            |
                tPersistedSparse:Identifier
                {
                    if (TryMatch(tPersistedSparse, CodeGenerationSupporter.Persisted))
                    {
                        vResult.AlterTableAlterColumnOption = vAddDefined ? AlterTableAlterColumnOption.AddPersisted : AlterTableAlterColumnOption.DropPersisted;
                    }
                    else
                    {
                        Match(tPersistedSparse, CodeGenerationSupporter.Sparse);
                        vResult.AlterTableAlterColumnOption = vAddDefined ? AlterTableAlterColumnOption.AddSparse : AlterTableAlterColumnOption.DropSparse;
                    }
                    UpdateTokenInfo(vResult,tPersistedSparse);
                }
            )
        )
    ;

alterTableTriggerModificationStatement returns [AlterTableTriggerModificationStatement vResult = this.FragmentFactory.CreateFragment<AlterTableTriggerModificationStatement>()]
{
    Identifier vIdentifier;
}
    :
        tId:Identifier
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
    : 
        Drop vAlterTableDropTableElement=alterTableDropTableElement
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
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            alterTableDropTableElementWithOptions[vResult]
        )?
    |
        Column 
        {
            vResult.TableElementType = TableElementType.Column;
        }
        vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
    ;

alterTableDropTableElementWithOptions[AlterTableDropTableElement vParent]
{
    DropClusteredConstraintOption vDropClusteredConstraintOption;
}
    :
        With LeftParenthesis
        vDropClusteredConstraintOption=dropClusteredConstraintOption
        {
            AddAndUpdateTokenInfo(vParent, vParent.DropClusteredConstraintOptions,vDropClusteredConstraintOption);
        }
        (
            Comma vDropClusteredConstraintOption=dropClusteredConstraintOption
            {
                AddAndUpdateTokenInfo(vParent, vParent.DropClusteredConstraintOptions,vDropClusteredConstraintOption);
            }
        )*
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent, tRParen);
        }
    ;

dropClusteredConstraintOption returns [DropClusteredConstraintOption vResult = null]
{
    Literal vLiteral;
    OptionState vOptionState;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
    DropClusteredConstraintStateOption vDropClusteredConstraintStateOption = null;
}
    :
        {NextTokenMatches(CodeGenerationSupporter.MaxDop)}?
        Identifier EqualsSign vLiteral=integer
        {
            DropClusteredConstraintValueOption vDropClusteredConstraintFragmentOption = this.FragmentFactory.CreateFragment<DropClusteredConstraintValueOption>();
            vDropClusteredConstraintFragmentOption.OptionValue = vLiteral;
            vResult = vDropClusteredConstraintFragmentOption;
            vResult.OptionKind = DropClusteredConstraintOptionKind.MaxDop;
        }
    |
        {NextTokenMatches(CodeGenerationSupporter.Online)}?
        Identifier EqualsSign 
        {
            vDropClusteredConstraintStateOption = this.FragmentFactory.CreateFragment<DropClusteredConstraintStateOption>();
            vResult = vDropClusteredConstraintStateOption;
            vResult.OptionKind = DropClusteredConstraintOptionKind.Online;
        }
        vOptionState=optionOnOff[vResult] // Split because fragment created just above...
        {
            vDropClusteredConstraintStateOption.OptionState = vOptionState;
        }
    |
        {NextTokenMatches(CodeGenerationSupporter.Move)}?
        Identifier To vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
        {
            DropClusteredConstraintMoveOption vDropClusteredConstraintFragmentOption = this.FragmentFactory.CreateFragment<DropClusteredConstraintMoveOption>();
            vDropClusteredConstraintFragmentOption.OptionValue = vFileGroupOrPartitionScheme;
            vResult = vDropClusteredConstraintFragmentOption;
            vResult.OptionKind = DropClusteredConstraintOptionKind.MoveTo;
        }        
    ;

alterTableSwitchStatement returns [AlterTableSwitchStatement vResult = this.FragmentFactory.CreateFragment<AlterTableSwitchStatement>()]
{
    ScalarExpression vExpression;
    SchemaObjectName vSchemaObjectName;
}
    :    tSwitch:Identifier
        {
            Match(tSwitch, CodeGenerationSupporter.Switch);
        }
        (
            tPartition:Identifier vExpression=expression
            {
                Match(tPartition, CodeGenerationSupporter.Partition);
                vResult.SourcePartitionNumber = vExpression;
            }
        )?
        To vSchemaObjectName=schemaObjectThreePartName
        {
            vResult.TargetTable = vSchemaObjectName;
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Partition)}?
            Identifier vExpression=expression
            {
                vResult.TargetPartitionNumber = vExpression;
            }
        )?
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :            
            With LeftParenthesis tableSwitchOptionList[vResult.Options, vResult]
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult, tRParen);
            }
        )?
    ;

tableSwitchOptionList[IList<TableSwitchOption> optionList, TSqlFragment vParent]
{
    // currently supports only a low priority lock wait option
    //
    LowPriorityLockWaitTableSwitchOption vOption = FragmentFactory.CreateFragment<LowPriorityLockWaitTableSwitchOption>();
}
    :   lowPriorityLockWaitOption[vOption.Options, vOption]
        {
            vOption.OptionKind = TableSwitchOptionKind.LowPriorityLockWait;
            optionList.Add(vOption);
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

inlineIndexType returns [IndexType vResult = null]
{
    bool? vIsClustered = null;
    bool vIsHash = false;
    IndexTypeKind? vIndexTypeKind = null;
}
    :
        (            
            Clustered
            {
                vIsClustered = true;
            }
        |   NonClustered
            {
                vIsClustered = false;
            }
        )?
        (
            options {greedy = true; } :
            tHash:Identifier
            {            
                Match(tHash, CodeGenerationSupporter.Hash);
                vIsHash = true;
            }
        )?
        {
            if(vIsHash)
            {
                if(vIsClustered.HasValue && vIsClustered.Value)
                {
                    ThrowIncorrectSyntaxErrorException(tHash);
                }
                
                vIndexTypeKind = IndexTypeKind.NonClusteredHash;
            }
            else if(vIsClustered.HasValue)
            {
                if(vIsClustered.Value)
                {
                    vIndexTypeKind = IndexTypeKind.Clustered;
                }
                else
                {
                    vIndexTypeKind = IndexTypeKind.NonClustered;
                }
            }
            
            vResult = new IndexType() { IndexTypeKind = vIndexTypeKind };        
        }
    ;
    
// Warning: highly similar code with inlineIndexColumnDefinition
// The only difference is that in inlineIndexColumnDefinition the column list is optional
// whereas in inlineIndexTableDefinition they are required
inlineIndexTableDefinition [IndexAffectingStatement statement] returns [IndexDefinition vResult = FragmentFactory.CreateFragment<IndexDefinition>()]
{   
    ColumnWithSortOrder vColumnWithSortOrder;
    Identifier vIndexIdentifier;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
    IndexType vIndexType = null;
}
    :
        tIndex:Index
        {
            UpdateTokenInfo(vResult,tIndex);            
        }
        vIndexIdentifier=identifier
        {
            vResult.Name=vIndexIdentifier;
        }
        vIndexType=inlineIndexType
        {
            vResult.IndexType=vIndexType;
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
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With 
            (
                indexOptionList[statement, vResult.IndexOptions, vResult]
            )
        )?
        (
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        fileStreamOnOpt[vResult]
    ;
    
// Warning: highly similar code with inlineIndexTableDefinition
// The only difference is that in inlineIndexColumnDefinition the column list is optional
// whereas in inlineIndexTableDefinition they are required
inlineIndexColumnDefinition returns [IndexDefinition vResult = FragmentFactory.CreateFragment<IndexDefinition>()]
{    
    ColumnWithSortOrder vColumnWithSortOrder;
    Identifier vIndexIdentifier;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
    IndexType vIndexType = null;
}
    :
        tIndex:Index
        {
            UpdateTokenInfo(vResult,tIndex);            
        }
        ( 
            vIndexIdentifier=identifier
            {
                vResult.Name=vIndexIdentifier;
            }
        )
        vIndexType=inlineIndexType
        {
            vResult.IndexType=vIndexType;
        }
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
        (
            // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
            With 
            (
                indexOptionList[IndexAffectingStatement.CreateTableInlineIndex, vResult.IndexOptions, vResult]
            )
        )?
        (
            On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        fileStreamOnOpt[vResult]
    ;

tableElement[IndexAffectingStatement statementType, TableDefinition vParent, AlterTableAddTableElementStatement vStatement]
{
    ColumnDefinition vColumnDefinition;
    ConstraintDefinition vConstraint;
    IndexDefinition vIndexDefinition;
}
    : vColumnDefinition=columnDefinition[statementType, vStatement]
        {
            AddAndUpdateTokenInfo(vParent, vParent.ColumnDefinitions, vColumnDefinition);
        }
    | vConstraint=tableConstraint[statementType, vStatement]
        {
            AddAndUpdateTokenInfo(vParent, vParent.TableConstraints, vConstraint);
        }
    |
        vIndexDefinition=inlineIndexTableDefinition [statementType]
        {
            AddAndUpdateTokenInfo(vParent, vParent.Indexes, vIndexDefinition);
        }
    ;

// When changing this rule, also look at alterTableAlterColumnStatement rule - it has parts of column definition inlined
columnDefinition [IndexAffectingStatement statementType, AlterTableAddTableElementStatement vStatement] returns [ColumnDefinition vResult = FragmentFactory.CreateFragment<ColumnDefinition>()]
{
    Identifier vIdentifier;
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
            (   // Computed column
                computedColumnBody[vResult]
                computedColumnConstraintListOpt[statementType, vResult]
            )
        |    
            (    // Regular column
                regularColumnBody[statementType, vResult]
                columnConstraintListOpt[statementType, vResult]
            )
        )        
    ;
    
computedColumnBody [ColumnDefinition vParent]
{
    ScalarExpression vExpression;
}
    :    As vExpression=expressionWithFlags[ExpressionFlags.ScalarSubqueriesDisallowed]
        {
            vParent.ComputedColumnExpression = vExpression;
        }
        (
            {NextTokenMatches(CodeGenerationSupporter.Persisted)}?
            tPersisted:Identifier
            {
                vParent.IsPersisted = true;
                UpdateTokenInfo(vParent, tPersisted);
            }
        )?
    ;
    
regularColumnBody [IndexAffectingStatement statementType, ColumnDefinition vParent]
{
    DataTypeReference vDataType = null;
    ColumnStorageOptions vStorageOptions;    
}
    :    ( options {greedy=true;} :
            vDataType=scalarDataType 
            {
                vParent.DataType = vDataType;
            }
            collationOpt[vParent]
            (
                options {greedy = true; } : // Due to conflict with identifierStatements
                vStorageOptions = columnStorage[statementType, vDataType]
                {
                    vParent.StorageOptions = vStorageOptions;
                }
            )?                  
        )?
        {
            VerifyColumnDataType(vParent);
        }
    ;

columnStorage [IndexAffectingStatement statementType, DataTypeReference columnType] returns [ColumnStorageOptions vResult = FragmentFactory.CreateFragment<ColumnStorageOptions>()] 
    :
        tStorageAttribute1:Identifier 
        (    // Due to conflict with identifierStatements
            (Identifier) =>
            tStorageAttribute2:Identifier
            {
                if (TryMatch(tStorageAttribute1, CodeGenerationSupporter.Sparse))
                {
                    SetSparseStorageOption(vResult, SparseColumnOption.Sparse, tStorageAttribute1, statementType);
                    
                    Match(tStorageAttribute2, CodeGenerationSupporter.FileStream);
                    SetFileStreamStorageOption(vResult, tStorageAttribute2, columnType, statementType);
                }
                else
                {
                    Match(tStorageAttribute1, CodeGenerationSupporter.FileStream);
                    SetFileStreamStorageOption(vResult, tStorageAttribute1, columnType, statementType);

                    Match(tStorageAttribute2, CodeGenerationSupporter.Sparse);
                    SetSparseStorageOption(vResult, SparseColumnOption.Sparse, tStorageAttribute2, statementType);
                }
                
                UpdateTokenInfo(vResult, tStorageAttribute2);
            }
        |
            For tAllSparseColumns:Identifier
            {
                Match(tStorageAttribute1, CodeGenerationSupporter.ColumnSet);
                Match(tAllSparseColumns, CodeGenerationSupporter.AllSparseColumns);
                
                XmlDataTypeReference xmlDataType = columnType as XmlDataTypeReference;
                if (xmlDataType != null && 
                    xmlDataType.XmlDataTypeOption == XmlDataTypeOption.None &&
                    xmlDataType.XmlSchemaCollection == null)
                {
                    SetSparseStorageOption(vResult, SparseColumnOption.ColumnSetForAllSparseColumns, 
                        tStorageAttribute1, statementType);
                }
                else
                    ThrowIncorrectSyntaxErrorException(tStorageAttribute1);
                    
                UpdateTokenInfo(vResult, tAllSparseColumns);
            }
        |
            /* empty */
            {
                if (TryMatch(tStorageAttribute1, CodeGenerationSupporter.Sparse))
                {
                    SetSparseStorageOption(vResult, SparseColumnOption.Sparse, tStorageAttribute1, statementType);
                }
                else
                {
                    Match(tStorageAttribute1, CodeGenerationSupporter.FileStream);
                    SetFileStreamStorageOption(vResult, tStorageAttribute1, columnType, statementType);
                }
                UpdateTokenInfo(vResult, tStorageAttribute1);
            }
        )
    ;    

fileStreamOnOpt [IFileStreamSpecifier vParent]
    :    (    // Greedy due to conflict with identifierStatements
            options {greedy = true; } :
            fileStreamOn[vParent]
        )?
    ;    

fileStreamOn [IFileStreamSpecifier vParent]
{
    IdentifierOrValueExpression vFragment;
}
    :   tFileStreamOn:Identifier vFragment = stringOrIdentifier
        {
            Match(tFileStreamOn, CodeGenerationSupporter.FileStreamOn);
            UpdateTokenInfo((TSqlFragment)vParent, tFileStreamOn);
            vParent.FileStreamOn = vFragment;
        }
    ;        

computedColumnConstraintListOpt [IndexAffectingStatement statementType, ColumnDefinition vResult]
{
    ConstraintDefinition vConstraint;
}
    :    ( vConstraint=columnConstraint[statementType]
            {
                AddConstraintToComputedColumn(vConstraint, vResult);
            }
        )*
    ;
    
columnConstraintListOpt [IndexAffectingStatement statementType, ColumnDefinition vResult]
{
    ConstraintDefinition vConstraint;
    IdentityOptions vIdentityOptions;
    IndexDefinition vIndexDefinition;
}
    : ( 
          rowguidcolConstraint[vResult]
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
        | vIndexDefinition=inlineIndexColumnDefinition
            {
                vResult.Index=vIndexDefinition;
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
        | vResult=uniqueColumnConstraint[statementType]
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
        ( vResult=uniqueTableConstraint[statementType]
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
        Debug.Assert(PhaseOne);
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
            // Greedy due to linear approximation introduced after the rule securityStatementPermission
            options {greedy = true; } :
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
defaultTableConstraint [IndexAffectingStatement statementType] returns [DefaultConstraintDefinition vResult = this.FragmentFactory.CreateFragment<DefaultConstraintDefinition>()]
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
            // Greedy due to linear approximation introduced after the rule securityStatementPermission
            options {greedy = true; } :
            With tValues:Values
            {
                UpdateTokenInfo(vResult,tValues);
                vResult.WithValues = true;
            }
        )?
    ;

// Warning: highly similar code with uniqueTableConstraint
uniqueColumnConstraint [IndexAffectingStatement statementType] returns [UniqueConstraintDefinition vResult = FragmentFactory.CreateFragment<UniqueConstraintDefinition>()]
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
        uniqueConstraintTailOpt[statementType, vResult]
    ;

// Warning: highly similar code with uniqueColumnConstraint
uniqueTableConstraint [IndexAffectingStatement statementType] returns [UniqueConstraintDefinition vResult = this.FragmentFactory.CreateFragment<UniqueConstraintDefinition>()]
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
        uniqueConstraintTailOpt[statementType, vResult]
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
                vParent.IndexType = new IndexType() { IndexTypeKind = IndexTypeKind.Clustered };
            }
        | NonClustered tHash:Identifier
            {
                Match(tHash, CodeGenerationSupporter.Hash);
                UpdateTokenInfo(vParent,tHash);                
                vParent.IndexType = new IndexType() { IndexTypeKind = IndexTypeKind.NonClusteredHash };
            }
        | tNonclustered:NonClustered
            {
                UpdateTokenInfo(vParent,tNonclustered);
                vParent.Clustered = false;
                vParent.IndexType = new IndexType() { IndexTypeKind = IndexTypeKind.NonClustered };
            }
        )?
    ;

uniqueConstraintTailOpt [IndexAffectingStatement statementType, UniqueConstraintDefinition vParent]
{
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
}
    :
        uniqueConstraintIndexOptionsOpt[statementType, vParent]
        ( tOn:On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
            {
                if (statementType == IndexAffectingStatement.CreateType)
                {
                    ThrowIncorrectSyntaxErrorException(tOn);
                }
                   
                vParent.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
            }
        )?
        fileStreamOnOpt[vParent]
    ;

uniqueConstraintIndexOptionsOpt[IndexAffectingStatement statement, UniqueConstraintDefinition vParent]
{
    IndexOption vIndexOption;
}
    :    (    // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
            options {greedy = true; } :
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
            |
                indexOptionList[statement, vParent.IndexOptions, vParent]
            )
        )?
    ;

sortedDataOptions
    :    tOption:Identifier
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
                tOn:On Delete vDeleteUpdateAction=deleteUpdateAction[vParent]
                {
                    if (vOnDeleteParsed)
                        throw GetUnexpectedTokenErrorException(tOn); 
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
    :    ( 
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

xmlDataType [SchemaObjectName vName] returns [XmlDataTypeReference vResult = FragmentFactory.CreateFragment<XmlDataTypeReference>()]
{
    vResult.Name = vName;
    vResult.UpdateTokenInfo(vName);
    
    SchemaObjectName vSchemaObjectName;
}
    :
        (
            (LeftParenthesis identifier)=>
            LeftParenthesis
            (
                tOption:Identifier
                {
                    vResult.XmlDataTypeOption = XmlDataTypeOptionHelper.Instance.ParseOption(tOption);
                }
            )?
            vSchemaObjectName=dataTypeSchemaObjectName
            {
                vResult.XmlSchemaCollection = vSchemaObjectName;
            }
            tRParen:RightParenthesis
            {
                UpdateTokenInfo(vResult,tRParen);
            }
        )?
    ;

scalarDataType returns [DataTypeReference vResult = null]
{
    SchemaObjectName vName;
    SqlDataTypeOption typeOption = SqlDataTypeOption.None;
    bool isXmlDataType = false;
}
    : 
        (
            vName=dataTypeSchemaObjectName
            {
                if (vName.SchemaIdentifier == null ||
                    (vName.SchemaIdentifier != null && IsSys(vName.SchemaIdentifier)))
                {
                    typeOption = ParseDataType100(vName.BaseIdentifier.Value);
                    isXmlDataType = IsXml(vName.BaseIdentifier);
                }
            }
            (
                {isXmlDataType}?
                vResult = xmlDataType[vName]
            |
                {typeOption != SqlDataTypeOption.None}?
                vResult = sqlDataTypeWithoutNational[vName, typeOption]
            |
                vResult = userDataType[vName]
            )
        )
    |
        vResult = doubleDataType
    |
        vResult = sqlDataTypeWithNational
    ; 

sqlDataTypeWithNational returns [SqlDataTypeReference vResult = FragmentFactory.CreateFragment<SqlDataTypeReference>()]
{
    SchemaObjectName vName;
    bool vVarying = false;
}
   :    tNational:National vName=dataTypeSchemaObjectName
        {
            vResult.SqlDataTypeOption = ParseDataType100(vName.BaseIdentifier.Value);
            
            if (vResult.SqlDataTypeOption == SqlDataTypeOption.None ||
                (vName.SchemaIdentifier != null && !IsSys(vName.SchemaIdentifier)))
            {
                ThrowParseErrorException("SQL46003", tNational, 
                    TSqlParserResource.SQL46003Message, TSqlParserResource.UserDefined);
            }
            vResult.Name = vName;
            UpdateTokenInfo(vResult, tNational);
            vResult.UpdateTokenInfo(vName);
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
    :
        (   (LeftParenthesis (integer | Identifier))=> // necessary because select statement can start with LeftParenthesis
            LeftParenthesis 
            (
                vLiteral=max
                {
                    AddAndUpdateTokenInfo(vParent, vParent.Parameters, vLiteral);
                }
            |
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
    :   tDouble:Double tPrecision:Identifier
        {
            Match(tPrecision, CodeGenerationSupporter.Precision);
            SetNameForDoublePrecisionType(vResult, tDouble, tPrecision);
            vResult.SqlDataTypeOption = SqlDataTypeOption.Float;
        }
        (   (LeftParenthesis (integer | Identifier))=> // necessary because select statement can start with LeftParenthesis
            LeftParenthesis 
            (
                vLiteral=max
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Parameters, vLiteral);
                }
            |
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

dataTypeSchemaObjectName returns [SchemaObjectName vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>()]
{
    Identifier vIdentifier;
}
    :
        vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
        (
            Dot vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
            }
        )?
    ;

// NonEmpty means  .xxx is not allowed it should be either a.b or a.
schemaObjectNonEmptyTwoPartName returns [SchemaObjectName vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>()]
{
    Identifier vIdentifier;
}
    :
        vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
        (
            Dot vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
            }
        )?
    ;

schemaObjectTwoPartName returns [SchemaObjectName vResult = null]
{
    Identifier vIdentifier;
}
    :
        vResult=schemaObjectNonEmptyTwoPartName
    |
        Dot vIdentifier=identifier
        {
            vResult = this.FragmentFactory.CreateFragment<SchemaObjectName>();
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
    ;

// NonEmpty means x or .xxx is not allowed it should be either a.b or a.b.c
eventSessionNonEmptyThreePartObjectName returns [EventSessionObjectName vResult = this.FragmentFactory.CreateFragment<EventSessionObjectName>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        vMultiPartIdentifier = nonEmptyThreePartObjectName
        {
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;

nonEmptyThreePartObjectName returns [MultiPartIdentifier vResult = this.FragmentFactory.CreateFragment<MultiPartIdentifier>()]
{
    Identifier vIdentifier;
}
    :
        vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
        Dot vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
        (
            Dot vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
            }
        )?
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

eventSessionOneOrTwoOrThreePartObjectName returns [EventSessionObjectName vResult = this.FragmentFactory.CreateFragment<EventSessionObjectName>()]
{
    MultiPartIdentifier vMultiPartIdentifier;
}
    :
        vMultiPartIdentifier = oneOrTwoOrThreePartObjectName
        {
            vResult.MultiPartIdentifier = vMultiPartIdentifier;
        }
    ;

oneOrTwoOrThreePartObjectName returns [MultiPartIdentifier vResult = this.FragmentFactory.CreateFragment<MultiPartIdentifier>()]
{
    Identifier vIdentifier;
}
    :
        vIdentifier=identifier
        {
            AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
        }
        (
            Dot vIdentifier=identifier
            {
                AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
            }
            (
                Dot vIdentifier=identifier
                {
                    AddAndUpdateTokenInfo(vResult, vResult.Identifiers, vIdentifier);
                }
            )?
        )?
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
            vResult=nullPredicate[vExpressionFirst]
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
    :
        tTSEqual:TSEqual LeftParenthesis vExpression=expression
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
    :   tUpdate:Update LeftParenthesis vIdentifier=identifier tRParen:RightParenthesis
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
    StringLiteral vProperty;    
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
        |
            tProperty:Identifier LeftParenthesis vColumn=identifierColumnReferenceExpression
            {
                Match(tProperty, CodeGenerationSupporter.Property);
                AddAndUpdateTokenInfo(vResult, vResult.Columns, vColumn);
            }
            Comma vProperty=stringLiteral RightParenthesis
            {
                vResult.PropertyName=vProperty;
            }
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
    :   Exists vSubquery=subquery[SubDmlFlags.SelectNotForInsert, expressionFlags]
        {
            vResult.Subquery = vSubquery;
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
        vSubquery=subquery[SubDmlFlags.SelectNotForInsert, expressionFlags]
        {
            vResult.ComparisonType = vType;
            vResult.Expression = vExpressionFirst;
            vResult.SubqueryComparisonPredicateType = vSubqueryComparisonPredicateType;
            vResult.Subquery = vSubquery;
        }
    ;

nullPredicate[ScalarExpression vExpressionFirst] returns [BooleanIsNullExpression vResult = this.FragmentFactory.CreateFragment<BooleanIsNullExpression>()]
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
            vSubquery=subquery[SubDmlFlags.SelectNotForInsert, expressionFlags]
            {
                vResult.Subquery = vSubquery;
            }
        |
            LeftParenthesis expressionList[vResult, vResult.Values] tRParen:RightParenthesis
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

escapeExpression [LikePredicate vParent, ExpressionFlags expressionFlags]
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
        vResult=expressionWithClrElements[expressionFlags]
    ;

expressionWithClrElements [ExpressionFlags expressionFlags] returns [PrimaryExpression vResult]
{
    Identifier vIdentifier;
    FunctionCall vFunctionCall = null;
    ExpressionCallTarget vCallTarget = null;
}
    :
        (
            (schemaObjectTwoPartName DoubleColon)=>
            vResult=udtExpression
        |   
            vResult=expressionPrimary[expressionFlags]
        )
        (
            Dot vIdentifier=identifier
            {
                vCallTarget = FragmentFactory.CreateFragment<ExpressionCallTarget>();
                vCallTarget.Expression = vResult;
            }
            (
                // TODO, olegr: Investigate why we need this
                (LeftParenthesis)=>
                vFunctionCall = expressionWithClrElementsFunctionCallPart
                {        
                    vFunctionCall.CallTarget = vCallTarget;
                    vFunctionCall.FunctionName = vIdentifier;
                    
                    vResult = vFunctionCall;
                }
                collationOpt[vResult]
            |
                {
                    UserDefinedTypePropertyAccess vPropertyAccess = FragmentFactory.CreateFragment<UserDefinedTypePropertyAccess>();
                    vPropertyAccess.CallTarget = vCallTarget;
                    vPropertyAccess.PropertyName = vIdentifier;
                    
                    vResult = vPropertyAccess;
                }
                collationOpt[vResult]
            )
        )*
    ;
    
expressionWithClrElementsFunctionCallPart returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
    :
        parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]                
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
            {NextTokenMatches(CodeGenerationSupporter.TryCast) && (LA(2) == LeftParenthesis)}?        
            vResult=tryCastCall
        |
            {NextTokenMatches(CodeGenerationSupporter.Parse) && (LA(2) == LeftParenthesis)}?        
            vResult=parseCall
        |
            {NextTokenMatches(CodeGenerationSupporter.TryParse) && (LA(2) == LeftParenthesis)}?        
            vResult=tryParseCall
        |
            {NextTokenMatches(CodeGenerationSupporter.IIf) && (LA(2) == LeftParenthesis)}?        
            vResult=iIfCall
        |
            (Identifier LeftParenthesis)=>
            vResult=builtInFunctionCall
        |
            vResult = leftFunctionCall
        |
            vResult = rightFunctionCall
        |
            // Syntactic predicate does not give the required k==3 behavior so using semantic instead
            {((LA(1) == Identifier || LA(1) == QuotedIdentifier) && LA(2) == Dot && LA(3) == DollarPartition) || LA(1) == DollarPartition}?
            vResult=partitionFunctionCall
        |
            {NextTokenMatches(CodeGenerationSupporter.Next) && NextTokenMatches(CodeGenerationSupporter.Value, 2)}?        
            vResult=nextValueForCall
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
            vResult=tryConvertCall
        |
            vResult=parameterlessCall
        |
            vResult=parenthesisDisambiguatorForExpressions[expressionFlags]
        )
        collationOpt[vResult]
    ;

parenthesisDisambiguatorForExpressions [ExpressionFlags expressionFlags] returns [PrimaryExpression vResult]

    :
        {IsNextRuleSelectParenthesis()}?
        vResult=subquery[SubDmlFlags.SelectNotForInsert, expressionFlags]       
    | 
        vResult=expressionParenthesis[expressionFlags]
    ;

udtExpression returns [PrimaryExpression vResult]
{
    UserDefinedTypeCallTarget vCallTarget;
}
    :    vCallTarget = userDefinedTypeCallTarget
        (
            // Even though it is k==2 without this predicate Antlr complains.
            (identifier LeftParenthesis)=>
            vResult=udtFunctionExpression[vCallTarget]
        |
            vResult=udtPropertyExpression[vCallTarget]
        )
    ;

userDefinedTypeCallTarget returns [UserDefinedTypeCallTarget vResult = FragmentFactory.CreateFragment<UserDefinedTypeCallTarget>()]
{
    SchemaObjectName vSchemaObjectName;
}
    :    vSchemaObjectName=schemaObjectTwoPartName tDoubleColon:DoubleColon
        {
            vResult.SchemaObjectName = vSchemaObjectName;
            UpdateTokenInfo(vResult, tDoubleColon);
        }
    ;

udtPropertyExpression [UserDefinedTypeCallTarget vCallTarget] returns [UserDefinedTypePropertyAccess vResult = FragmentFactory.CreateFragment<UserDefinedTypePropertyAccess>()]
{
    Identifier vIdentifier;
}
    :    vIdentifier=identifier collationOpt[vResult]
        {
            vResult.CallTarget = vCallTarget;
            vResult.PropertyName = vIdentifier;
        }
    ;

udtFunctionExpression [UserDefinedTypeCallTarget vCallTarget] returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
{
    Identifier vIdentifier;
}
    :   vIdentifier=identifier 
        {
            vResult.CallTarget = vCallTarget;
            vResult.FunctionName = vIdentifier;
        }
        parenthesizedOptExpressionWithDefaultList[vResult, vResult.Parameters]
    ;

basicFunctionCall returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
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

tryConvertCall returns [TryConvertCall vResult = this.FragmentFactory.CreateFragment<TryConvertCall>()]
{
    DataTypeReference vDataType;
    ScalarExpression vExpression;
}
    :
        tTryConvert:TryConvert LeftParenthesis vDataType=scalarDataType Comma vExpression=expression 
        {
            UpdateTokenInfo(vResult,tTryConvert);
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

parseCall returns [ParseCall vResult = this.FragmentFactory.CreateFragment<ParseCall>()]
{
    ScalarExpression vExpression;
    DataTypeReference vDataType;
    ScalarExpression vCulture;
}
    :
        tParse:Identifier LeftParenthesis vExpression=expression As vDataType=scalarDataType
        {
            Match(tParse, CodeGenerationSupporter.Parse);
            UpdateTokenInfo(vResult,tParse);
            vResult.StringValue = vExpression;
            vResult.DataType = vDataType;
        }
        (
            tUsing:Identifier
            {
                Match(tUsing, CodeGenerationSupporter.Using);
            }
            vCulture=expression
            {
                vResult.Culture = vCulture;
            }
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

tryParseCall returns [TryParseCall vResult = this.FragmentFactory.CreateFragment<TryParseCall>()]
{
    ScalarExpression vExpression;
    DataTypeReference vDataType;
    ScalarExpression vCulture;
}
    :
        tTryParse:Identifier LeftParenthesis vExpression=expression As vDataType=scalarDataType
        {
            Match(tTryParse, CodeGenerationSupporter.TryParse);
            UpdateTokenInfo(vResult,tTryParse);
            vResult.StringValue = vExpression;
            vResult.DataType = vDataType;
        }
        (
            tUsing:Identifier
            {
                Match(tUsing, CodeGenerationSupporter.Using);
            }
            vCulture=expression
            {
                vResult.Culture = vCulture;
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

tryCastCall returns [TryCastCall vResult = this.FragmentFactory.CreateFragment<TryCastCall>()]
{
    DataTypeReference vDataType;
    ScalarExpression vExpression;
}
    :
        tTryCast:Identifier LeftParenthesis vExpression=expression As vDataType=scalarDataType tRParen:RightParenthesis
        {
            Match(tTryCast, CodeGenerationSupporter.TryCast);
            UpdateTokenInfo(vResult,tTryCast);
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
    
expressionList [TSqlFragment vParent, IList<ScalarExpression> expressions]
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

overClause returns [OverClause vResult]
{
    OrderByClause vOrderByClause;
    WindowFrameClause vWindowFrameClause;
}
    :   vResult = overClauseBeginning
        (
            vOrderByClause=orderByClause
            {
                vResult.OrderByClause = vOrderByClause;
            }
            (
                vWindowFrameClause=windowFrameClause
                {
                    CheckWindowFrame(vWindowFrameClause);
                    vResult.WindowFrameClause = vWindowFrameClause;
                }
            )?
        )?
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

overClauseNoOrderBy returns [OverClause vResult]
    :   
        vResult = overClauseBeginning
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

overClauseBeginning returns [OverClause vResult = FragmentFactory.CreateFragment<OverClause>()]
    :
        tOver:Over
        {
            UpdateTokenInfo(vResult,tOver);
        }
        LeftParenthesis
        (
            tPartition:Identifier
            {
                Match(tPartition, CodeGenerationSupporter.Partition);
            }
            By expressionList[vResult, vResult.Partitions]
        )?
    ;

windowFrameClause returns [WindowFrameClause vResult = FragmentFactory.CreateFragment<WindowFrameClause>()]
    : 
        tRowsRange:Identifier
        {
            Match(tRowsRange, CodeGenerationSupporter.Rows, CodeGenerationSupporter.Range);
            
            if (TryMatch(tRowsRange, CodeGenerationSupporter.Rows))
            {
                vResult.WindowFrameType = WindowFrameType.Rows;
            }
            else
            {
                Match(tRowsRange, CodeGenerationSupporter.Range);
                vResult.WindowFrameType = WindowFrameType.Range;
            }

            UpdateTokenInfo(vResult,tRowsRange);
        }
        windowFrameExtent[vResult]
    ;

windowFrameExtent [WindowFrameClause vParent]
{
    WindowDelimiter vWindowDelimiterTop, vWindowDelimiterBottom;
}
    :
        tBetween:Between vWindowDelimiterTop=windowFrameDelimiter And vWindowDelimiterBottom=windowFrameDelimiter
        {
            vParent.Top = vWindowDelimiterTop;
            vParent.Bottom = vWindowDelimiterBottom;
            UpdateTokenInfo(vParent,tBetween);
        }
    |
        vWindowDelimiterTop=windowFrameDelimiter
        {
            vParent.Top = vWindowDelimiterTop;
        }
    ;

windowFrameDelimiter returns [WindowDelimiter vResult = FragmentFactory.CreateFragment<WindowDelimiter>()]
{
    Literal vOffset;
}
    :
        tCurrent:Current tRow:Identifier
        {
            vResult.WindowDelimiterType = WindowDelimiterType.CurrentRow;
            UpdateTokenInfo(vResult,tCurrent);
        }
    |
        vOffset=integer tDirection:Identifier
        {
            Match(tDirection, CodeGenerationSupporter.Preceding, CodeGenerationSupporter.Following);
            
            if (TryMatch(tDirection, CodeGenerationSupporter.Preceding))
            {
                vResult.WindowDelimiterType = WindowDelimiterType.ValuePreceding;
            }
            else
            {
                Match(tDirection, CodeGenerationSupporter.Following);
                vResult.WindowDelimiterType = WindowDelimiterType.ValueFollowing;
            }

            vResult.OffsetValue = vOffset;
            UpdateTokenInfo(vResult,tDirection);
        }
    |
        tUnbounded:Identifier tUnboundedDirection:Identifier
        {
            Match(tUnbounded, CodeGenerationSupporter.Unbounded);
            Match(tUnboundedDirection, CodeGenerationSupporter.Preceding, CodeGenerationSupporter.Following);
            
            if (TryMatch(tUnboundedDirection, CodeGenerationSupporter.Preceding))
            {
                vResult.WindowDelimiterType = WindowDelimiterType.UnboundedPreceding;
            }
            else
            {
                Match(tUnboundedDirection, CodeGenerationSupporter.Following);
                vResult.WindowDelimiterType = WindowDelimiterType.UnboundedFollowing;
            }

            UpdateTokenInfo(vResult,tUnbounded);
            UpdateTokenInfo(vResult,tUnboundedDirection);
        }
    ;

withinGroupClause returns [WithinGroupClause vResult = FragmentFactory.CreateFragment<WithinGroupClause>()]
{
    OrderByClause vOrderByClause;
}
    :
        tWithin:Identifier
        Group
        {
            Match(tWithin, CodeGenerationSupporter.Within);
            UpdateTokenInfo(vResult,tWithin);
        }
        tLParen:LeftParenthesis
        vOrderByClause=orderByClause
        {
            vResult.OrderByClause = vOrderByClause;
            UpdateTokenInfo(vResult,tLParen);
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;


// TODO, olegr: Add more checks for allowed functions here - there are quite some in SQL Server parser
builtInFunctionCall returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
{
    Identifier vIdentifier;
}
    :    vIdentifier=nonQuotedIdentifier
        {
            vResult.FunctionName = vIdentifier;
        }
        LeftParenthesis 
        (
            regularBuiltInFunctionCall[vResult]
        |
            aggregateBuiltInFunctionCall[vResult]
        )
    ;

regularBuiltInFunctionCall [FunctionCall vParent]
{
    ColumnReferenceExpression vColumn;
}
    :
        (
            expressionList[vParent, vParent.Parameters]
        |
            vColumn=starColumnReferenceExpression
            {
                AddAndUpdateTokenInfo(vParent, vParent.Parameters, vColumn);
            }
        |   
            /* empty */
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vParent,tRParen);
        }
        (
            {(LA(1)==Over && LA(2)==LeftParenthesis) || 
             (NextTokenMatches(CodeGenerationSupporter.Within) && LA(2)==Group && LA(3)==LeftParenthesis) }?
            withinGroupOrOverClause [vParent]
        )?
    ;

withinGroupOrOverClause [FunctionCall vParent]
{
    OverClause vOverClause;
    WithinGroupClause vWithinGroupClause;
}
    :
        vWithinGroupClause=withinGroupClause vOverClause=overClauseNoOrderBy
        {
            vParent.WithinGroupClause = vWithinGroupClause;
            vParent.OverClause = vOverClause;
        }
    |
        vOverClause=overClause
        {
            vParent.OverClause = vOverClause;
        }
    ;

starColumnReferenceExpression returns [ColumnReferenceExpression vResult = FragmentFactory.CreateFragment<ColumnReferenceExpression>()]
    : tStar:Star
      {
        vResult.ColumnType = ColumnType.Wildcard;
        UpdateTokenInfo(vResult, tStar);
      }
    ;

aggregateBuiltInFunctionCall [FunctionCall vParent]
{
    UniqueRowFilter vUniqueRowFilter;
    ScalarExpression vParameter;
    OverClause vOverClause;
    IToken distinctToken = null;
}
    :    vUniqueRowFilter=uniqueRowFilter[out distinctToken] vParameter=expression tRParen:RightParenthesis
        {
            vParent.UniqueRowFilter = vUniqueRowFilter;
            AddAndUpdateTokenInfo(vParent, vParent.Parameters, vParameter);
            UpdateTokenInfo(vParent,tRParen);
        }
        (
            vOverClause=overClauseNoOrderBy
            {
                vParent.OverClause = vOverClause;
                CheckForDistinctInWindowedAggregate(vParent, distinctToken);
            }
        )?
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
        LeftParenthesis
        vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.FirstExpression = vExpression;
        }
        Comma
        vExpression=expressionWithFlags[expressionFlags]
        {
            vResult.SecondExpression = vExpression;
        }
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }
    ;

iIfCall returns [IIfCall vResult = this.FragmentFactory.CreateFragment<IIfCall>()]
{
    BooleanExpression vExpression = null;
    ScalarExpression vThenExpression = null;
    ScalarExpression vElseExpression = null;
}
    :
        tIIf:Identifier
        {
            Match(tIIf, CodeGenerationSupporter.IIf);
            UpdateTokenInfo(vResult,tIIf);
        }
        LeftParenthesis
        vExpression=booleanExpression 
        {
            vResult.Predicate = vExpression;
        }
        Comma
        vThenExpression=expression
        {
            vResult.ThenExpression = vThenExpression;
        }
        Comma
        vElseExpression=expression
        {
            vResult.ElseExpression = vElseExpression;
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
        LeftParenthesis
        vExpression=expressionWithFlags[expressionFlags]
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

caseExpression [ExpressionFlags expressionFlags] returns [CaseExpression vResult]
{
    ScalarExpression vExpression = null;
}
    :
        tCase:Case
        (
            vExpression=expression
            vResult = simpleCaseExpression[vExpression, expressionFlags]
        |
             vResult = searchedCaseExpression[expressionFlags]
        )
        (
            Else vExpression=expression
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
    
systemColumn [ColumnReferenceExpression vParent]
    :
        tPseudoColumn:PseudoColumn
        {
            UpdateTokenInfo(vParent,tPseudoColumn);
            vParent.ColumnType = PseudoColumnHelper.Instance.ParseOption(tPseudoColumn, SqlVersionFlags.TSql120);
        }
    ;

specialColumn [ColumnReferenceExpression vParent]
    : 
        tIdentityCol:IdentityColumn
        {
            UpdateTokenInfo(vParent,tIdentityCol);
            vParent.ColumnType = ColumnType.IdentityCol;
        }
    | 
        tRowguidCol:RowGuidColumn
        {
            UpdateTokenInfo(vParent,tRowguidCol);
            vParent.ColumnType = ColumnType.RowGuidCol;
        }
    |
        systemColumn[vParent]
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
            // however it works because table name always has to be written. 
            vMultiPartIdentifier=multiPartIdentifier[-1]
            {
                vResult.MultiPartIdentifier = vMultiPartIdentifier;
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

nextValueForCall returns [NextValueForExpression vResult = this.FragmentFactory.CreateFragment<NextValueForExpression>()]
{
    SchemaObjectName vSchemaObjectName;
    OverClause vOverClause;
}
    : tNext:Identifier tValue:Identifier tFor:For vSchemaObjectName=schemaObjectThreePartName 
        {
            Match(tNext, CodeGenerationSupporter.Next);
            Match(tValue, CodeGenerationSupporter.Value);            
            UpdateTokenInfo(vResult,tNext);
            vResult.SequenceName = vSchemaObjectName;
        }
        (
            vOverClause=overClause
            {
                vResult.OverClause = vOverClause;
            }
        )? 
    ;

userFunctionCall[MultiPartIdentifier vIdentifiers] returns [FunctionCall vResult = FragmentFactory.CreateFragment<FunctionCall>()]
{
    OverClause vOverClause;
    UniqueRowFilter vUniqueRowFilter;
    IToken distinctToken = null;
}
    :   LeftParenthesis
        {
            PutIdentifiersIntoFunctionCall(vResult, vIdentifiers);
        }
        (
            (expressionWithDefaultList[vResult, vResult.Parameters])?
        |
            vUniqueRowFilter=uniqueRowFilter[out distinctToken]
            {
                vResult.UniqueRowFilter = vUniqueRowFilter;
            }
            expressionList[vResult, vResult.Parameters]
        )
        tRParen:RightParenthesis
        {
            UpdateTokenInfo(vResult,tRParen);
        }      
        (
            vOverClause=overClauseNoOrderBy
            {
                vResult.OverClause = vOverClause;
                CheckForDistinctInWindowedAggregate(vResult, distinctToken);
            }
        )?
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
                vColumn.MultiPartIdentifier = vMultiPartIdentifier;
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
                vResult.Qualifier = vMultiPartIdentifier;
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

stringLiteralOrNull returns [Literal vResult]
    : vResult=stringLiteral
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

signedIntegerOrReal returns [ScalarExpression vResult = null]
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
            
        vLiteral=integerOrRealOrNumeric
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

objectOrVariable returns [SchemaObjectNameOrValueExpression vResult = this.FragmentFactory.CreateFragment<SchemaObjectNameOrValueExpression>()]
{
    SchemaObjectName vSchemaObjectName;
    VariableReference vVariable;
}
    : vSchemaObjectName = schemaObjectThreePartName 
      {
        vResult.SchemaObjectName = vSchemaObjectName;
      }
    | vVariable = variable
      {
        vResult.ValueExpression=vVariable;
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

assemblyName returns [AssemblyName vResult = this.FragmentFactory.CreateFragment<AssemblyName>()]
{
    Identifier vIdentifier;
}
    :
        tName:Identifier
        {
            Match(tName, CodeGenerationSupporter.Name);
        }
        vIdentifier=identifier
        {
            vResult.Name = vIdentifier;
        }
        (
            Dot vIdentifier=identifier
            {
                vResult.ClassName = vIdentifier;
            }
        )?
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
    UnaryExpression unaryExpr = null;
    Literal vLiteral;
}
    : 
        (
            tMinus:Minus 
            {
                unaryExpr = FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(unaryExpr,tMinus);
                unaryExpr.UnaryExpressionType = UnaryExpressionType.Negative;
            }
        |
            tPlus:Plus
            {
                unaryExpr = FragmentFactory.CreateFragment<UnaryExpression>();
                UpdateTokenInfo(unaryExpr,tPlus);
                unaryExpr.UnaryExpressionType = UnaryExpressionType.Positive;
           }
        )?
        vLiteral=integerOrNumeric
        {            
            if (unaryExpr == null)
                vResult = vLiteral;
            else
            {
                unaryExpr.Expression = vLiteral;
                vResult = unaryExpr;
            }
        }
    ;

subroutineParameterLiteral returns [Literal vResult]
    : vResult = integer
    | vResult = numeric
    | vResult = real
    | vResult = moneyLiteral
    ;

max returns [MaxLiteral vResult = this.FragmentFactory.CreateFragment<MaxLiteral>()]
    :   
        tMax:Identifier
        {
            Match(tMax, CodeGenerationSupporter.Max);
            UpdateTokenInfo(vResult,tMax);
            vResult.Value = tMax.getText();
        }
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
    | vResult = numeric
    | vResult = real
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
            vResult.IsNational = true;
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
            vResult.IsLargeObject=IsBinaryLiteralLob(vResult.Value);;
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

onOffOptionValue returns [OnOffOptionValue vResult = FragmentFactory.CreateFragment<OnOffOptionValue>()]
{
    OptionState vState;
}
    :
        vState=optionOnOff[vResult]
        {
            vResult.OptionState = vState;
        }
    ;

stringOrDefaultLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        (
            vValue=stringLiteral
            {
                vResult.Value = vValue;
            }
        |
            vValue=defaultLiteral
            {
                vResult.Value = vValue;
            }
        )
    ;

stringLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        vValue=stringLiteral
        {
            vResult.Value = vValue;
        }
    ;

integerOrDefaultLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        (
            vValue=integer
            {
                vResult.Value = vValue;
            }
        |
            vValue=defaultLiteral
            {
                vResult.Value = vValue;
            }
        )
    ;

integerLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        vValue=integer
        {
            vResult.Value = vValue;
        }
    ;

defaultLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        vValue=defaultLiteral
        {
            vResult.Value = vValue;
        }
    ;

binaryOrDefaultLiteralOptionValue returns [LiteralOptionValue vResult = FragmentFactory.CreateFragment<LiteralOptionValue>()]
{
    Literal vValue;
}
    :
        (
            vValue=binary
            {
                vResult.Value = vValue;
            }
        |
            vValue=defaultLiteral
            {
                vResult.Value = vValue;
            }
        )
    ;

assignmentWithOptOp returns [AssignmentKind vResult = AssignmentKind.Equals]
    :
        EqualsSign
        {      
            vResult = AssignmentKind.Equals;
        }
        | 
        vResult = assignmentWithOp
    ;
    
assignmentWithOp returns [AssignmentKind vResult = AssignmentKind.Equals]
    :    AddEquals
            {
                vResult = AssignmentKind.AddEquals;
            }
        | SubtractEquals
            {
                vResult = AssignmentKind.SubtractEquals;
            }
        | MultiplyEquals
            {
                vResult = AssignmentKind.MultiplyEquals;
            }
        | DivideEquals
            {
                vResult = AssignmentKind.DivideEquals;
            }
        | ModEquals
            {
                vResult = AssignmentKind.ModEquals;
            }
        | BitwiseAndEquals
            {
                vResult = AssignmentKind.BitwiseAndEquals;
            }
        | BitwiseOrEquals
            {
                vResult = AssignmentKind.BitwiseOrEquals;
            }
        | BitwiseXorEquals
            {
                vResult = AssignmentKind.BitwiseXorEquals;
            }
    ;

// This rule has the Identifier token, and all the keyword in the tokens list except: To, From, Grant, Where, On
securityStatementPermission returns [Identifier vResult = this.FragmentFactory.CreateFragment<Identifier>()]
{
    // Use the next tokens information.
    UpdateTokenInfo(vResult,LT(1));
    vResult.SetUnquotedIdentifier(LT(1).getText());
}
    :
        tId:Identifier 
    |   Add
    |   All
    |   Alter
    |   And
    |   Any
    |   As
    |   Asc
    |   Authorization
    |   Backup
    |   Begin
    |   Between
    |   Break
    |   Browse
    |   Bulk
    |   By
    |   Cascade
    |   Case
    |   Check
    |   Checkpoint
    |   Close
    |   Clustered
    |   Coalesce
    |   Collate
    |   Column
    |   Commit
    |   Compute
    |   Constraint
    |   Contains
    |   ContainsTable
    |   Continue
    |   Convert
    |   Create
    |   Cross
    |   Current
    |   CurrentDate
    |   CurrentTime
    |   CurrentTimestamp
    |   CurrentUser
    |   Cursor
    |   Database
    |   Dbcc
    |   Deallocate
    |   Declare
    |   Default
    |   Delete
    |   Deny
    |   Desc
    |   Disk
    |   Distinct
    |   Distributed
    |   Double
    |   Drop
    |   Else
    |   End
    |   Errlvl
    |   Escape
    |   Except
    |   Exec
    |   Execute
    |   External
    |   Exists
    |   Exit
    |   Fetch
    |   File
    |   FillFactor
    |   For
    |   Foreign
    |   FreeText
    |   FreeTextTable
    |   Full
    |   Function
    |   GoTo
    |   Group
    |   Having
    |   HoldLock
    |   Identity
    |   IdentityInsert
    |   IdentityColumn
    |   If
    |   In
    |   Index
    |   Inner
    |   Insert
    |   Intersect
    |   Into
    |   Is
    |   Join
    |   Key
    |   Kill
    |   Left
    |   Like
    |   LineNo
    |    Merge
    |   National
    |   NoCheck
    |   NonClustered
    |   Not
    |   Null
    |   NullIf
    |   Of
    |   Off
    |   Offsets
    |   Open
    |   OpenDataSource
    |   OpenQuery
    |   OpenRowSet
    |   OpenXml
    |   Option
    |   Or
    |   Order
    |   Outer
    |   Over
    |   Percent
    |   Pivot
    |   Plan
    |   Precision
    |   Primary
    |   Print
    |   Proc
    |   Procedure
    |   Public
    |   Raiserror
    |   Read
    |   ReadText
    |   Reconfigure
    |   References
    |   Replication
    |   Restore
    |   Restrict
    |   Return
    |   Revert
    |   Revoke
    |   Right
    |   Rollback
    |   RowCount
    |   RowGuidColumn
    |   Rule
    |   Save
    |   Schema
    |   Select
    |   SessionUser
    |   Set
    |   SetUser
    |   Shutdown
    |   Some
    |   Statistics
    |   StopList
    |   SystemUser
    |   Table
    |   TableSample
    |   TextSize
    |   Then
    |   Top
    |   Tran
    |   Transaction
    |   Trigger
    |   Truncate
    |   TSEqual
    |   Union
    |   Unique
    |   Unpivot
    |   Update
    |   UpdateText
    |   Use
    |   User
    |   Values
    |   Varying
    |   View
    |   WaitFor
    |   When
    |   While
    |   With
    |   WriteText
    ;

///////////////////////////////////////////////////////////////////////////////////////////
// End of simple utility stuff
///////////////////////////////////////////////////////////////////////////////////////////

{
    #pragma warning disable 618, 219
}

class TSql120LexerInternal extends Lexer("TSqlLexerBaseInternal");

options {
    k = 2;
    charVocabulary = '\u0000'..'\uFFFF';
    testLiterals = false;
    caseSensitive = false;
    caseSensitiveLiterals = false;
    classHeaderPrefix = "internal partial";
    importVocab = TSql;
}

// If this list is changed please maintain the parser rule: securityStatementPermission
tokens {

        // Version-specific keywords
    // T-SQL 2005
    External = "external";
    Pivot = "pivot";
    Revert = "revert";
    TableSample = "tablesample";
    Unpivot = "unpivot";
    
    // T-SQL 2008
    Merge = "merge";
    StopList = "stoplist";

    // T-SQL 2012
    SemanticKeyPhraseTable = "semantickeyphrasetable";
    SemanticSimilarityTable = "semanticsimilaritytable";
    SemanticSimilarityDetailsTable = "semanticsimilaritydetailstable";
    TryConvert = "try_convert";

    // T-SQL vNext
    
}

{
    public TSql120LexerInternal() 
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
RightOuterJoin                : "=*";
GreaterThan        : '>' ;
Circumflex        : '^' ;
VerticalLine    : '|' ;
Tilde            : '~' ;
AddEquals        : "+=";
SubtractEquals    : "-=";
DivideEquals    : "/=";
ModEquals        : "%=";
BitwiseAndEquals: "&=";
BitwiseOrEquals : "|=";
BitwiseXorEquals: "^=";

protected
Semicolon        : ';' ;

protected
Digit
    :    '0'..'9'
    ;

protected
FirstLetter
    :    'a'..'z' 
    |    '_' 
    |    '#' 
    |    CharHighNotWhitespace
    ;

protected
Letter
    :    'a'..'z' 
    |    '_' 
    |    '#'
    |    '@' 
    |    '$' 
    |    CharHighNotWhitespace
    ;

protected 
CharHighNotWhitespace
    // !! Note that the lexer has a very simplified and incorrect understanding of what T-SQL 
    // !! identifier is. It simply accepts all characters between 0x0080 and 0xfffe, while the 
    // !! engine lexer accepts characters basing on Unicode classes (i.e. Lu, as Ll, Lt, Lm, 
    // !! Lo and Nl as the first character). In particular, this simplification leads to a 
    // !! conflict with the WS_CHAR_WO_NEWLINE rule, which we temporary resolve by excluding 
    // !! the corresponding characters from the 0x0080..0xfffe range. Please see VSTS#710711 
    // !! for details.
    :    '\u0080'..'\u0084'
    //   '\u0085'               // Cc: NEL, NExt Line
    |    '\u0086'..'\u009f'
    //   '\u00a0'               // Zs: No-Break Space
    |    '\u00a1'..'\u1679'
    //   '\u1680'               // Zs: Ogham Space Mark
    |    '\u1681'..'\u1fff'
    //   '\u2000'..'\u200a'     // Zs: En Quad .. Hair Space
    //     '\u200b'               // Cf: Zero Width Space
    |    '\u200c'..'\u2027'
    //   '\u2028'               // Zs: Line Separator
    //   '\u2029'               // Zs: Paragraph Separator
    |    '\u202a'..'\u202e'
    //   '\u202f'               // Zs: Narrow No-Break Space
    |    '\u2030'..'\u205e'
    //   '\u205f'               // Zs: Medium Mathematical Space
    |    '\u2060'..'\u2fff'
    //   '\u3000'               // Zs: Ideographic Space
    |    '\u3001'..'\ufffe'
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
    // The low range characters (i.e. < 0x80),
    // see "CharTab" in "sql/ntdbms/msql/parser/parslex.cpp".
    :    '\u0000'..'\u0008'     // Cc: Null character .. Backspace
    |    '\u0009'               // Cc: HT, Horizontal tab
    //   '\u000a'               // Cc: LF, Line Feed       => handled by EndOfLine
    |    '\u000b'               // Cc: VT, Vertical tab
    |    '\u000c'               // Cc: FF, Form Feed
    //   '\u000d'               // Cc: CR, Carriage Return => handled by EndOfLine
    |    '\u000e'..'\u001f'     // Cc: Shift Out .. Unit Separator
    |    '\u0020'               // Zs: Space
    // The high range characters (i.e. >= 0x80), 
    // see "categorySql32Whitespace" in "sql/common/regext/regext.cpp".
    |    '\u0085'               // Cc: NEL, NExt Line
    |    '\u00a0'               // Zs: No-Break Space
    |    '\u1680'               // Zs: Ogham Space Mark
    |    '\u2000'..'\u200a'     // Zs: En Quad .. Hair Space
    |     '\u200b'               // Cf: Zero Width Space
    |    '\u2028'               // Zs: Line Separator
    |    '\u2029'               // Zs: Paragraph Separator
    |    '\u202f'               // Zs: Narrow No-Break Space
    |    '\u205f'               // Zs: Medium Mathematical Space
    |    '\u3000'               // Zs: Ideographic Space
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
    :    {CurrentOffset==_acceptableGoOffset}? 
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
Real :;

protected
Numeric :;

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
            {   checkEOF(TokenKind.String); } 
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
            {   checkEOF(TokenKind.SqlCommandIdentifier); }
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
    :    
        { beginComplexToken(); } '['
        (
            {   checkEOF(TokenKind.QuotedIdentifier); } 
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
                {   checkEOF(TokenKind.QuotedIdentifier); } 
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
