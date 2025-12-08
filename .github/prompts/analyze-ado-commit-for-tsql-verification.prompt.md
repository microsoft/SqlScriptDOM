---
title: Analyze ADO Commit for T-SQL Verification in ScriptDOM
description: Automated workflow to analyze Azure DevOps commits for new T-SQL syntax and verify support in ScriptDOM parser
tags: [automation, ado, tsql, verification, parser, testing, bug-tracking]
---

# Analyze ADO Commit for T-SQL Verification in ScriptDOM

This prompt automates the complete workflow of analyzing Azure DevOps commits for new T-SQL syntax, verifying ScriptDOM support, and creating bug reports for missing functionality.

## Prompt Input

**Required**: Provide the commit hash from Azure DevOps DsMainDev repository that you want to analyze.

```
Commit Hash: [COMMIT_HASH]
```

**Example**:
```
Commit Hash: 15b0ead69fc5a8ba9eb1d4d84735c4e9d6ab5718
```

**Default Settings**:
- Project: Database Systems
- Repository: DsMainDev

## Automated Workflow Steps

### Step 1: Analyze ADO Commit for Parser Test Changes

**Task**: Search the specified commit for changes in XML parser unit test files.

**Search Criteria**:
- Path: `Sql/Ntdbms/frontend/parser/parserunittests/Tests`
- File Type: `*.xml` files
- Change Type: Any modifications in the commit

**Search Commands**:
```bash
# 1. Search for XML files in the parserunittests directory
mcp_ado_search_code with:
- fileType: "xml"
- path: ["Sql/Ntdbms/frontend/parser/parserunittests/Tests"]
- project: ["Database Systems"]
- repository: ["DsMainDev"]

# 2. Search for specific SQL keywords if known from commit message
mcp_ado_search_code with:
- searchText: "[EXTRACTED_KEYWORD]" (e.g., "VECTOR_SEARCH", "JSON_OBJECTAGG")
- path: ["Sql/Ntdbms/frontend/parser/parserunittests/Tests"]
- project: ["Database Systems"] 
- repository: ["DsMainDev"]
```

**Expected Output**: List of XML test files containing new T-SQL syntax patterns.

### Step 2: Extract T-SQL Statements from XML Test Files

**Task**: Parse each identified XML file to extract T-SQL test cases.

**Extraction Pattern**:
```xml
<Test name="test_name">
    <Description>Test description</Description>
    <Sql>
        [EXTRACT_THIS_TSQL_STATEMENT]
    </Sql>
    <!-- Optional error expectations -->
    <ExpectedError major="PARSER" minor="P_SYNTAXERR2" severity="EX_SYNTAX" state="1" />
</Test>
```

**Focus Areas**:
1. **Valid Syntax Tests**: `<Test>` elements without `<ExpectedError>` 
2. **Error Tests**: `<Test>` elements with `<ExpectedError>` (especially subquery blocking)
3. **Edge Cases**: Complex syntax combinations and parameter variations

**Categorize T-SQL Statements**:
- **✅ Valid Syntax**: Should parse successfully in ScriptDOM
- **❌ Expected Errors**: Should fail parsing (e.g., subquery restrictions)  
- **🔧 Complex Cases**: May require special handling

### Step 3: Verify Each T-SQL Statement in ScriptDOM

**Task**: For each extracted T-SQL statement, execute the verification workflow.

**For Each T-SQL Statement**:

#### 3.1: Run Initial Verification
Execute: `#file:verify-and-test-tsql-syntax.prompt.md`

**Input for Each Statement**:
```
T-SQL Statement: [EXTRACTED_SQL_FROM_XML]
Expected Behavior: [SHOULD_PASS | SHOULD_FAIL_WITH_ERROR]
Source Context: ADO Commit [COMMIT_HASH] - File: [XML_FILE_PATH] - Test: [TEST_NAME]
```

**Verification Process**:
1. **Add Debug Unit Test**: Create `DebugExactScriptTest()` method with exact T-SQL
2. **Test Multiple Parser Versions**: TSql170, TSql160, older versions as needed
3. **Document Results**: ✅ Success, ❌ Parse Error, ⚠️ Unexpected Behavior

#### 3.2: Expected Results Classification

**✅ Already Supported**: 
- T-SQL parses without errors
- Script generation works correctly  
- Round-trip parsing succeeds
- **Action**: Add comprehensive test coverage (continue with Steps 4-6 of verification guide)

**❌ Not Supported**: 
- Parse errors occur
- Syntax not recognized
- Grammar rules missing
- **Action**: Create bug report (proceed to Step 4)

**⚠️ Partial Support**:
- Basic syntax works but edge cases fail
- Some parameter combinations unsupported
- **Action**: Create enhancement bug report

**🔧 Expected Errors**:
- T-SQL correctly fails parsing (e.g., subquery blocking features)
- Error matches XML `<ExpectedError>` specification
- **Action**: Verify error handling is correct, no bug needed

### Step 4: Create ScriptDOM Bug Reports for Unsupported Syntax

**Task**: For each T-SQL statement that fails verification, create a comprehensive bug report.

**Bug Report Template**:

```markdown
# [Feature Name] T-SQL Syntax Not Supported in ScriptDOM Parser

## Source Information
- **ADO Commit**: [COMMIT_HASH]
- **ADO Project**: Database Systems / DsMainDev  
- **Source File**: `Sql/Ntdbms/frontend/parser/parserunittests/Tests/[PATH]/[FILENAME].xml`
- **Test Case**: [TEST_NAME]
- **SQL Server Version**: [VERSION] (based on parser test context)

## T-SQL Statement(s) Not Supported

### Statement 1:
```sql
[EXACT_TSQL_FROM_XML]
```

**Expected Behavior**: Should parse successfully and generate correct script output
**Current Behavior**: Parse error - [ERROR_MESSAGE]

### Statement 2: (if multiple)
```sql
[ADDITIONAL_TSQL_STATEMENTS]
```

## Parser Testing Results

### TSql170 Parser (SQL Server 2025):
- **Status**: ❌ Parse Error
- **Error Count**: [NUMBER]
- **Error Messages**: 
  ```
  [DETAILED_ERROR_MESSAGES]
  ```

### TSql160 Parser (SQL Server 2022):
- **Status**: ❌ Parse Error  
- **Error Count**: [NUMBER]
- **Error Messages**: 
  ```
  [DETAILED_ERROR_MESSAGES]
  ```

### Older Versions:
- **Expected**: Should fail (feature not in older SQL Server versions)
- **Actual**: [CONFIRMATION_OF_EXPECTED_FAILURE]

## Context and Usage

**Feature Description**: [BRIEF_DESCRIPTION_OF_TSQL_FEATURE]

**SQL Server Documentation**: [LINK_TO_MICROSOFT_DOCS_IF_AVAILABLE]

**Test Context from ADO**: 
```xml
<Test name="[TEST_NAME]">
    <Description>[TEST_DESCRIPTION]</Description>
    <Sql>
        [TSQL_STATEMENT]
    </Sql>
</Test>
```

## Implementation Requirements

Based on error analysis:

### Grammar Changes Needed:
- [ ] Update `TSql170.g` (and potentially earlier versions)
- [ ] Add new grammar rules for [FEATURE_NAME]
- [ ] Create AST nodes in `Ast.xml`
- [ ] Add string constants in `CodeGenerationSupporter.cs`

### Script Generator Updates:
- [ ] Create `ExplicitVisit()` method for new AST nodes
- [ ] Add formatting logic for [FEATURE_NAME] syntax

### Test Coverage Required:
- [ ] Create comprehensive test script: `[FeatureName]Tests170.sql`
- [ ] Generate baseline file: `Baselines170/[FeatureName]Tests170.sql`
- [ ] Add test configuration in `Only170SyntaxTests.cs`
- [ ] Verify cross-version behavior

## Priority Assessment

**Severity**: [HIGH | MEDIUM | LOW]
- **HIGH**: Core SQL functionality used in production scenarios
- **MEDIUM**: Advanced features or edge cases
- **LOW**: Rarely used syntax or deprecated features

**Impact**: 
- **User Impact**: [DESCRIPTION_OF_USER_SCENARIOS]
- **ScriptDOM Completeness**: Missing [FEATURE_TYPE] support
- **SQL Server Version**: [VERSION] feature gap

## Related Work

**Similar Features**: [LIST_RELATED_TSQL_FEATURES_ALREADY_SUPPORTED]
**Implementation Patterns**: [REFERENCE_SIMILAR_IMPLEMENTATIONS]
**Dependencies**: [ANY_PREREQUISITES_OR_DEPENDENCIES]

## Acceptance Criteria

- [ ] T-SQL statement parses without errors in appropriate TSql parser version
- [ ] Script generation produces correctly formatted output
- [ ] Round-trip parsing (parse → generate → parse) succeeds
- [ ] Comprehensive test coverage added following testing guidelines
- [ ] Full test suite passes (no regressions)
- [ ] Cross-version compatibility verified

## Test Cases for Implementation

```sql
-- Test Case 1: Basic syntax (exact from ADO)
[EXACT_TSQL_STATEMENT]

-- Test Case 2: With variables/parameters (if applicable)
[TSQL_WITH_VARIABLES]

-- Test Case 3: Complex context (if applicable)
[TSQL_IN_COMPLEX_CONTEXT]

-- Test Case 4: Edge cases (if applicable)
[EDGE_CASE_SCENARIOS]
```
```

**Bug Filing Process**:

Use the configured ADO MCP server for ScriptDOM repository:

```bash
# Create bug report using ADO MCP configuration
mcp_ado_wit_create_work_item with:
- project: "SQLToolsAndLibraries"  # From mcp.json default config
- workItemType: "Bug"
- fields: [
    {
      "name": "System.Title",
      "value": "[Feature Name] T-SQL Syntax Not Supported in ScriptDOM Parser"
    },
    {
      "name": "System.Description", 
      "format": "Html",
      "value": "[Complete bug report content as HTML]"
    },
    {
      "name": "System.AreaPath",
      "value": "SQLToolsAndLibraries\\DacFx"  # From mcp.json default config
    },
    {
      "name": "Microsoft.VSTS.Common.Priority",
      "value": "2"  # High=1, Medium=2, Low=3
    },
    {
      "name": "Microsoft.VSTS.Common.Severity", 
      "value": "3 - Medium"  # Critical=1, High=2, Medium=3, Low=4
    },
    {
      "name": "System.Tags",
      "value": "ScriptDOM;TSql170;Parser;[Feature-Tags]"
    }
  ]

# Add technical details comment
mcp_ado_wit_add_work_item_comment with:
- project: "SQLToolsAndLibraries"
- workItemId: [RETURNED_WORK_ITEM_ID]
- comment: "[Technical implementation details]"
- format: "html"
```

**MCP Configuration**: Uses `mcp.json` settings for SQLToolsAndLibraries/ScriptDOM repository

### Step 5: Summary Report Generation

**Task**: Generate comprehensive analysis report.

**Report Template**:

```markdown
# ADO Commit [COMMIT_HASH] - ScriptDOM T-SQL Verification Report

**Analysis Date**: [CURRENT_DATE]
**Commit**: [COMMIT_HASH] 
**ADO Project**: Database Systems / DsMainDev

## Summary Statistics

- **XML Test Files Analyzed**: [COUNT]
- **T-SQL Statements Extracted**: [COUNT]  
- **Already Supported**: [COUNT] ✅
- **Missing Support**: [COUNT] ❌
- **Expected Errors**: [COUNT] 🔧
- **Bugs Created**: [COUNT] 🐛

## Files Analyzed

| XML File | Test Count | Supported | Missing | Errors |
|----------|------------|-----------|---------|--------|
| [FILE1.xml] | [N] | [N] | [N] | [N] |
| [FILE2.xml] | [N] | [N] | [N] | [N] |

## T-SQL Features Analysis

### ✅ Already Supported Features
1. **[Feature1]**: [Description] - No action needed
2. **[Feature2]**: [Description] - Add comprehensive tests

### ❌ Missing Features (Bugs Created)
1. **[Feature3]**: [Description] - Bug #[BUG_NUMBER]
2. **[Feature4]**: [Description] - Bug #[BUG_NUMBER]

### 🔧 Expected Error Behaviors
1. **[Restriction1]**: [Description] - Working as intended
2. **[Restriction2]**: [Description] - Correct error handling

## Implementation Priority

**High Priority** (Core functionality):
- [List high-priority missing features]

**Medium Priority** (Advanced features):
- [List medium-priority missing features]

**Low Priority** (Edge cases):
- [List low-priority missing features]

## Next Steps

1. **Immediate**: Address high-priority missing features
2. **Short-term**: Implement medium-priority features  
3. **Long-term**: Add comprehensive test coverage for all supported features
4. **Quality**: Ensure full regression testing for all changes

## Testing Recommendations

For each missing feature:
1. Follow [verify-and-test-tsql-syntax.prompt.md](verify-and-test-tsql-syntax.prompt.md)
2. Add comprehensive test coverage using testing guidelines
3. Verify cross-version compatibility
4. Run full test suite before committing

## ADO Integration

**Commit Context**: [BRIEF_DESCRIPTION_OF_COMMIT_PURPOSE]
**Related Features**: [LIST_RELATED_SQL_SERVER_FEATURES]
**Documentation**: [LINKS_TO_RELEVANT_DOCUMENTATION]
```

## Usage Instructions

### Basic Usage:
```
Execute this prompt with:
Commit Hash: 15b0ead69fc5a8ba9eb1d4d84735c4e9d6ab5718
```

### Expected Execution Time:
- **Analysis Phase**: 5-10 minutes (depending on commit size)
- **Verification Phase**: 2-5 minutes per T-SQL statement
- **Bug Creation**: 5-10 minutes per missing feature
- **Total**: 30-60 minutes for typical commits

### Prerequisites:
- Access to Azure DevOps Database Systems project
- ScriptDOM development environment set up
- Ability to create and run unit tests
- Bug tracking system access

### Output Artifacts:
1. **Analysis Report**: Comprehensive findings summary
2. **Bug Reports**: Individual bugs for each missing feature
3. **Test Scripts**: Debug unit tests for verification
4. **Implementation Roadmap**: Prioritized feature development plan

## Integration Points

**Previous Step**: Azure DevOps commit analysis
**Next Steps**: 
- Feature implementation using [bug_fixing.guidelines.instructions.md](../instructions/bug_fixing.guidelines.instructions.md)
- Test development using [testing.guidelines.instructions.md](../instructions/testing.guidelines.instructions.md)
- Validation fixes using [grammar_validation.guidelines.instructions.md](../instructions/grammar_validation.guidelines.instructions.md)

**Related Workflows**:
- `verify-and-test-tsql-syntax.prompt.md` (per-statement verification)
- ScriptDOM development guidelines (implementation)
- CI/CD testing pipelines (validation)

## Success Criteria

**Complete Analysis**: 
- ✅ All XML test files examined
- ✅ All T-SQL statements extracted and categorized
- ✅ ScriptDOM support status determined for each statement

**Actionable Results**:
- ✅ Bug reports created for unsupported features
- ✅ Priority assessment completed
- ✅ Implementation roadmap provided
- ✅ Test coverage recommendations documented

**Quality Assurance**:
- ✅ Each T-SQL statement tested with exact syntax from ADO
- ✅ Multiple parser versions tested for compatibility
- ✅ Error cases properly identified and documented
- ✅ Implementation complexity assessed

This automated workflow ensures comprehensive coverage of new T-SQL features and provides a clear path from ADO commit analysis to ScriptDOM feature implementation.