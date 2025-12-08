---
description: ScriptDOM parser development assistant for newcomers and experienced developers. Helps diagnose bugs, add features, and navigate the parser codebase. Perfect for fixing ADO work items or adding new T-SQL syntax support.
name: scriptdom-parser-agent
model: Claude Sonnet 4.5
tools: ['edit', 'search', 'runCommands', 'bluebird-mcp-ai-starter/*']
argument-hint: Bug ID, feature request, or task description
target: vscode

handoffs:
  - label: 🐛 Start Bug Fix Workflow
    agent: scriptdom-parser-agent
    prompt: |
      I want to fix a bug in ScriptDOM. Help me understand the issue, find relevant code using bluebird-mcp-ai-starter, determine if it's a grammar issue or validation issue, and guide me through the fix with step-by-step instructions. 
      
      Reference these guides based on bug type:
      - Validation issues: .github/instructions/Validation_fix.guidelines.instructions.md
      - Grammar issues: .github/instructions/bug_fixing.guidelines.instructions.md
      - Predicate issues: .github/instructions/parser.guidelines.instructions.md
      - Debug workflow: .github/instructions/debugging_workflow.guidelines.instructions.md
      
      Ask me for the bug details or ADO work item link.
    send: true

  - label: ✨ Start New Feature Workflow
    agent: scriptdom-parser-agent
    prompt: |
      I want to add new T-SQL syntax support to ScriptDOM. Help me understand what needs to be modified (grammar, AST, script generators, tests), find similar examples using bluebird-mcp-ai-starter, and create a step-by-step implementation plan.
      
      Reference these guides based on feature type:
      - General grammar: .github/instructions/bug_fixing.guidelines.instructions.md and .github/instructions/grammer.guidelines.instructions.md
      - New functions: .github/instructions/function.guidelines.instructions.md
      - New data types: .github/instructions/new_data_types.guidelines.instructions.md
      - New index types: .github/instructions/new_index_types.guidelines.instructions.md
      - Testing: .github/instructions/testing.guidelines.instructions.md
      
      Ask me for the SQL syntax I want to add.
    send: true

  - label: 🔍 Diagnose Parse Error
    agent: scriptdom-parser-agent
    prompt: |
      Help me diagnose why specific T-SQL syntax fails to parse. Analyze the error message, search for similar patterns in the codebase using bluebird-mcp-ai-starter, identify whether it's a grammar, validation, or predicate issue, and recommend the appropriate fix guide.
      
      Available diagnostic guides:
      - .github/instructions/debugging_workflow.guidelines.instructions.md - General debugging approach
      - .github/instructions/Validation_fix.guidelines.instructions.md - "Option X is not valid" errors
      - .github/instructions/bug_fixing.guidelines.instructions.md - "Incorrect syntax near" errors
      - .github/instructions/parser.guidelines.instructions.md - Parentheses/predicate recognition issues
      
      Ask me to paste the failing SQL syntax and error message.
    send: true

  - label: 📝 Create Test Cases
    agent: scriptdom-parser-agent
    prompt: |
      I've made changes to grammar or validation code. Help me create comprehensive test cases following the patterns in .github/instructions/testing.guidelines.instructions.md. Generate test scripts, baselines, and test configuration code. Show me exactly what files to create and what to put in them.
      
      Reference: .github/instructions/testing.guidelines.instructions.md for complete testing patterns and best practices.
    send: true

  - label: 🏗️ Build & Test Workflow
    agent: scriptdom-parser-agent
    prompt: |
      Guide me through building the parser and running tests. Show me the exact commands to regenerate the parser, run specific tests, and validate my changes. Check if I need to regenerate baselines and help me do it correctly.
      
      Reference: .github/instructions/testing.guidelines.instructions.md for test execution patterns and baseline generation.
    send: true

  - label: 📊 Validate My Changes
    agent: scriptdom-parser-agent
    prompt: |
      Review my branch changes before creating a PR. Check test coverage, verify baselines exist, ensure error counts are configured, validate that all grammar versions are updated, and run the full test suite. Tell me what's missing or incorrect.
      
      References:
      - .github/instructions/testing.guidelines.instructions.md - Test validation checklist
      - .github/instructions/bug_fixing.guidelines.instructions.md - Pre-commit requirements
      - .github/copilot-instructions.md - Project overview and validation rules
    send: true

  - label: 🔗 Compare File to Main
    agent: scriptdom-parser-agent
    prompt: |
      Compare the current file to the main branch version using bluebird-mcp-ai-starter. Show me exactly what changed with line numbers. For grammar files, explain what the changes mean. For test files, show baseline differences.
      
      Use bluebird-mcp-ai-starter get_source_code to retrieve the main branch version for comparison.
    send: true

  - label: 📚 Find Examples
    agent: scriptdom-parser-agent
    prompt: |
      I need examples of how to implement something in ScriptDOM. Use bluebird-mcp-ai-starter to search the main branch for similar patterns, grammar rules, AST nodes, or test cases. Show me concrete examples I can learn from.
      
      Use bluebird-mcp-ai-starter do_vector_search to find relevant examples in the codebase. Reference appropriate instruction files based on what's being implemented.
    send: true

  - label: 🧪 Verify T-SQL Syntax & Add Tests
    agent: scriptdom-parser-agent
    prompt: |
      I have T-SQL syntax that I want to verify if it's already supported, or I need to add comprehensive test coverage for new/existing syntax. Follow the step-by-step workflow from .github/prompts/verify-and-test-tsql-syntax.prompt.md:
      
      1. **CRITICAL FIRST STEP**: Test the exact T-SQL script provided to confirm if it works or fails
      2. Create a debug test method to verify current parser status
      3. Determine the SQL Server version where syntax was introduced
      4. Search existing tests and grammar files to check current support
      5. Create comprehensive test scripts (MUST include exact script provided)
      6. Configure test expectations with proper error counts
      7. Generate baseline files from actual parser output
      8. Build, run tests, and validate with full test suite
      
      Guide me through each step, checking actual parser behavior before making assumptions. Show me exact commands to run and files to create.
      
      Reference: .github/prompts/verify-and-test-tsql-syntax.prompt.md for complete verification and testing workflow.
    send: true
---

# ScriptDOM Quick Start Guide for Newcomers

## 🎯 I'm New Here - Where Do I Start?

### Step 1: Understand What ScriptDOM Does
ScriptDOM is a **T-SQL parser** that:
- Parses T-SQL scripts into an Abstract Syntax Tree (AST)
- Generates formatted T-SQL from the AST
- Supports all SQL Server versions (2000-2025) and Azure Synapse

### Step 2: Get the ADO Work Item Details
Example: https://msdata.visualstudio.com/SQLToolsAndLibraries/_workitems/edit/4843961/

1. Read the bug description carefully
2. Note the **error message** (e.g., "Incorrect syntax near...", "Option X is not valid...")
3. Copy the **failing T-SQL syntax**
4. Check which **SQL Server version** it should work in

### Step 3: Determine the Bug Type
Use this decision tree:

```
Does the error say "Option 'X' is not valid..." or "Feature not supported..."?
├─ YES → Validation Issue
│   └─ Guide: .github/instructions/Validation_fix.guidelines.instructions.md
│   └─ Quick fix: Update TSql*ParserBaseInternal.cs validation logic
│
├─ NO → Does the error say "Incorrect syntax near..." or parser doesn't recognize it?
    ├─ YES → Grammar Issue
    │   └─ Guide: .github/instructions/bug_fixing.guidelines.instructions.md
    │   └─ Quick fix: Update .g grammar files
    │
    └─ Does it work without parentheses but fail with parentheses?
        └─ YES → Predicate Recognition Issue
            └─ Guide: .github/instructions/parser.guidelines.instructions.md
```

---

## 🔧 Quick Bug Fix Workflow

### Example: Fixing ADO Work Item 4843961

#### 1. Reproduce the Issue
```powershell
# Create a test file with the failing SQL
@"
-- Paste the failing SQL syntax here
ALTER TABLE MyTable ADD CONSTRAINT pk PRIMARY KEY (id) 
WITH (RESUMABLE = ON);
"@ | Out-File -FilePath test.sql

# Try to parse it (will show the error)
# You can use existing test infrastructure to verify
```

#### 2. Search for Error Message
```powershell
# Find where the error is thrown
Select-String -Path "SqlScriptDom/**/*.cs" -Pattern "SQL46057"
Select-String -Path "SqlScriptDom/**/*.cs" -Pattern "is not a valid"
```

#### 3. Find Similar Working Examples
```powershell
# Search for similar syntax that DOES work
Select-String -Path "Test/SqlDom/TestScripts/*.sql" -Pattern "RESUMABLE"
Select-String -Path "SqlScriptDom/Parser/TSql/*.g" -Pattern "RESUMABLE"
```

**Use bluebird-mcp-ai-starter tools:**
- `get_source_code` - Get files from main branch
- `do_vector_search` - Search for similar patterns

#### 4. Identify the Fix Type

**Validation Fix** (most common):
- File: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`
- Look for: `VerifyAllowedIndexOption()`, `VerifyAllowedIndexType()`
- Fix: Change unconditional rejection to version-gated validation

**Grammar Fix**:
- Files: `SqlScriptDom/Parser/TSql/TSql*.g` (e.g., TSql170.g)
- Look for: Similar grammar rules
- Fix: Add new grammar rule or modify existing one

#### 5. Make the Fix

**Example Validation Fix:**
```csharp
// BEFORE (in TSql80ParserBaseInternal.cs):
if (option.OptionKind == IndexOptionKind.Resumable)
{
    ThrowParseErrorException("SQL46057", "Not valid");
}

// AFTER:
if ((versionFlags & SqlVersionFlags.TSql160AndAbove) == 0 && 
    option.OptionKind == IndexOptionKind.Resumable)
{
    ThrowParseErrorException("SQL46057", "Not supported in this SQL version");
}
```

#### 6. Create Test Cases

**Test Script:** `Test/SqlDom/TestScripts/YourBugFixTest160.sql`
```sql
-- Test 1: Basic syntax
ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);

-- Test 2: With multiple options
ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) 
WITH (RESUMABLE = ON, MAXDOP = 2);

-- Test 3: RESUMABLE OFF
ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = OFF);
```

**Test Configuration:** `Test/SqlDom/Only160SyntaxTests.cs`
```csharp
// Add this line to the Only160TestInfos array:
new ParserTest160("YourBugFixTest160.sql", 
    nErrors80: 3, nErrors90: 3, nErrors100: 3, nErrors110: 3, 
    nErrors120: 3, nErrors130: 3, nErrors140: 3, nErrors150: 3),
    // nErrors160: 0 (implicit - should work in SQL 2022)
```

#### 7. Build and Test

```powershell
# Rebuild the parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run your specific test
dotnet test --filter "YourBugFixTest160" Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Generate baseline (first run will fail, copy "Actual" output)
# Paste output into: Test/SqlDom/Baselines160/YourBugFixTest160.sql

# Run test again (should pass now)
dotnet test --filter "YourBugFixTest160" Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# CRITICAL: Run FULL test suite
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

#### 8. Create PR

```powershell
# Create branch
git checkout -b dev/yourname/fix-bug-4843961

# Stage changes
git add SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs
git add Test/SqlDom/TestScripts/YourBugFixTest160.sql
git add Test/SqlDom/Baselines160/YourBugFixTest160.sql
git add Test/SqlDom/Only160SyntaxTests.cs

# Commit with proper format
git commit -m "[Parser] Fix RESUMABLE option validation for ALTER TABLE

Fixes ADO work item 4843961
- Updated validation logic to allow RESUMABLE in SQL 2022+
- Added comprehensive test coverage"

# Push and create PR
git push origin dev/yourname/fix-bug-4843961
```

---

## 📁 Key Files & Directories (Where to Look)

### Grammar Files (Parser Rules)
- `SqlScriptDom/Parser/TSql/TSql170.g` - SQL Server 2025 grammar
- `SqlScriptDom/Parser/TSql/TSql160.g` - SQL Server 2022 grammar
- Look for: `ruleName returns [Type vResult] : (alternatives);`

### AST Definitions
- `SqlScriptDom/Parser/TSql/Ast.xml` - Defines AST node structure
- Look for: `<Class Name="...">` and `<Member Name="...">`

### Validation Logic
- `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs` - Core validation
- Look for: `VerifyAllowed*()`, `ThrowParseErrorException()`

### Script Generators
- `SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/*.cs`
- Look for: `ExplicitVisit()` methods

### Tests
- `Test/SqlDom/TestScripts/` - Input T-SQL test files
- `Test/SqlDom/Baselines160/` - Expected parser output (SQL 2022)
- `Test/SqlDom/Baselines170/` - Expected parser output (SQL 2025)
- `Test/SqlDom/Only160SyntaxTests.cs` - Test configurations

### Guidelines
- `.github/instructions/` - Detailed how-to guides
- `.github/copilot-instructions.md` - Project overview

---

## 🔍 Debugging Tips

### Finding the Right Code

**Search by error message:**
```powershell
Select-String -Path "SqlScriptDom/**/*.cs" -Pattern "SQL46057"
```

**Search by keyword:**
```powershell
Select-String -Path "SqlScriptDom/Parser/TSql/*.g" -Pattern "RESUMABLE"
```

**Find similar grammar rules:**
```powershell
Select-String -Path "SqlScriptDom/Parser/TSql/TSql170.g" -Pattern "indexOption"
```

**Find test examples:**
```powershell
Select-String -Path "Test/SqlDom/TestScripts/*.sql" -Pattern "ALTER TABLE.*WITH"
```

### Common Mistakes to Avoid

❌ **Modifying shared grammar rules**
- Don't change rules like `identifierColumnReferenceExpression` used everywhere
- Create context-specific rules instead

❌ **Forgetting test configuration**
- Test script exists but not in Only*SyntaxTests.cs
- Tests will never run!

❌ **Wrong baseline directory**
- TSql160 tests → Baselines160/
- TSql170 tests → Baselines170/

❌ **Not running full test suite**
- Grammar changes can break unrelated tests
- Always run: `dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug`

❌ **Incorrect error counts**
- Count EACH statement in your test script
- Older SQL versions should have errors for new syntax

---

## 🎓 Learning Path

### 1. Start Small
- Fix a validation bug (easiest - no grammar changes)
- Read: `Validation_fix.guidelines.instructions.md`

### 2. Simple Grammar Addition
- Add a new keyword or simple option
- Read: `bug_fixing.guidelines.instructions.md`

### 3. Complex Features
- Add new functions, data types, or index types
- Read: `function.guidelines.instructions.md`, `new_data_types.guidelines.instructions.md`

### 4. Test Everything
- Understand the test framework thoroughly
- Read: `testing.guidelines.instructions.md`

---

## 🆘 Getting Help

### Use the Agent Handoffs Above:
- 🐛 **Start Bug Fix Workflow** - Step-by-step bug fixing
- ✨ **Start New Feature Workflow** - Add new syntax
- 🔍 **Diagnose Parse Error** - Understand why SQL fails
- 📝 **Create Test Cases** - Generate test files
- 📚 **Find Examples** - Search for similar code

### Ask Specific Questions:
- "How do I add validation for option X in SQL Server 2025?"
- "Where is the grammar rule for ALTER TABLE statements?"
- "Show me examples of function tests in RETURN statements"
- "How do I regenerate parser baselines?"

### Search the Codebase:
- Use `bluebird-mcp-ai-starter` tools to search main branch
- Look at similar features for patterns
- Check existing tests for examples

---

## 📋 Pre-Commit Checklist

- [ ] Bug/feature is fully implemented
- [ ] Test script created in TestScripts/
- [ ] Baseline generated in Baselines*/
- [ ] Test configured in Only*SyntaxTests.cs
- [ ] Error counts correct for all SQL versions
- [ ] Parser builds successfully
- [ ] Specific test passes
- [ ] **FULL test suite passes** (ALL ~1,100+ tests)
- [ ] Branch name: `dev/[username]/[description]`
- [ ] Commit message: `[Component] description`
- [ ] PR description follows template

---

## 🚀 You're Ready!

Now try the **🐛 Start Bug Fix Workflow** handoff above with your ADO work item!


