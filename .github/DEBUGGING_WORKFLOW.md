# ScriptDOM Debugging Workflow - Quick Reference

This is a visual guide for quickly diagnosing and fixing bugs in SqlScriptDOM. Use this as your first stop when encountering a parsing issue.

## 🔍 Quick Diagnosis Flowchart

```
┌─────────────────────────────────────┐
│  You have a parsing error/bug      │
└────────────┬────────────────────────┘
             │
             ▼
┌────────────────────────────────────────────────────────────┐
│ Step 1: Search for the error message                      │
│ Command: grep -r "SQL46057" SqlScriptDom/                 │
│          grep -r "your error text" SqlScriptDom/          │
└────────────┬───────────────────────────────────────────────┘
             │
             ▼
      ┌──────┴──────┐
      │ Error Type? │
      └──────┬──────┘
             │
    ┌────────┼────────┐
    │        │        │
    ▼        ▼        ▼
┌───────┐ ┌───────┐ ┌───────┐
│"Option│ │"Syntax│ │Parens │
│not    │ │error  │ │break  │
│valid" │ │near..." │predicte│
└───┬───┘ └───┬───┘ └───┬───┘
    │         │         │
    ▼         ▼         ▼
 ┌─────┐  ┌─────┐  ┌─────┐
 │VALID│  │GRAM │  │PRED │
 │ATION│  │MAR  │  │RECOG│
 └─────┘  └─────┘  └─────┘
```

## 📋 Error Pattern Recognition

### Pattern 1: Validation Error (Most Common)
**Symptoms:**
- ❌ `SQL46057: Option 'RESUMABLE' is not a valid index option in 'ALTER TABLE' statement`
- ❌ `Feature 'X' is not supported in SQL Server version Y`
- ✅ Same syntax works in different context (ALTER INDEX vs ALTER TABLE)

**Quick Check:**
```bash
# Search for similar working syntax
grep -r "RESUMABLE" Test/SqlDom/TestScripts/
# Found in AlterIndexTests but not AlterTableTests?
# → It's a validation issue!
```

**Solution:** [VALIDATION_FIX_GUIDE.md](VALIDATION_FIX_GUIDE.md)
**Files to Check:** `TSql80ParserBaseInternal.cs` (validation methods)

---

### Pattern 2: Grammar Error
**Symptoms:**
- ❌ `Incorrect syntax near keyword 'NEWKEYWORD'`
- ❌ Parser doesn't recognize new T-SQL feature at all
- ❌ Syntax has never been implemented

**Quick Check:**
```bash
# Search for the keyword in grammar files
grep -r "YourKeyword" SqlScriptDom/Parser/TSql/*.g
# Not found? → It's a grammar issue!
```

**Solution:** [BUG_FIXING_GUIDE.md](BUG_FIXING_GUIDE.md)
**Files to Modify:** `TSql*.g`, `Ast.xml`, Script generators

---

### Pattern 3: Predicate Recognition Error
**Symptoms:**
- ✅ `WHERE REGEXP_LIKE('a', 'pattern')` works
- ❌ `WHERE (REGEXP_LIKE('a', 'pattern'))` fails with syntax error
- ❌ Error near closing parenthesis or semicolon

**Quick Check:**
```bash
# Test both syntaxes
echo "SELECT 1 WHERE REGEXP_LIKE('a', 'b');" > test1.sql
echo "SELECT 1 WHERE (REGEXP_LIKE('a', 'b'));" > test2.sql
# Second one fails? → Predicate recognition issue!
```

**Solution:** [PARSER_PREDICATE_RECOGNITION_FIX.md](PARSER_PREDICATE_RECOGNITION_FIX.md)
**Files to Modify:** `TSql80ParserBaseInternal.cs` (`IsNextRuleBooleanParenthesis()`)

---

## 🛠️ Standard Investigation Steps

### Step 1: Reproduce Minimal Test Case
```bash
# Create minimal failing SQL
echo "ALTER TABLE t ADD CONSTRAINT pk PRIMARY KEY (id) WITH (RESUMABLE = ON);" > test.sql

# Try parsing (use existing test harness or create simple parser test)
```

### Step 2: Find Error Source
```bash
# Search for error code/message
grep -r "SQL46057" SqlScriptDom/
grep -r "is not a valid" SqlScriptDom/

# Common locations:
# - SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs (validation)
# - SqlScriptDom/Parser/TSql/TSql*.g (grammar rules)
# - SqlScriptDom/ScriptDom/SqlServer/*Helper.cs (option/type helpers)
```

### Step 3: Check Microsoft Documentation
```bash
# Search for: "Applies to: SQL Server 20XX (XX.x)"
# Verify exact version support
# Note: Different features may have different version requirements!
```

### Step 4: Locate Similar Working Code
```bash
# If ALTER INDEX works but ALTER TABLE doesn't:
grep -r "Resumable" Test/SqlDom/TestScripts/

# Find where option is registered:
grep -r "IndexOptionKind.Resumable" SqlScriptDom/

# Check validation paths:
grep -r "IndexAffectingStatement" SqlScriptDom/Parser/TSql/
```

## 🔧 Fix Implementation Checklist

### For Validation Fixes:
- [ ] Identify validation function (usually in `TSql80ParserBaseInternal.cs`)
- [ ] Check SQL Server version support in Microsoft docs
- [ ] Add version-gated validation (not unconditional rejection)
- [ ] Create test cases with version-specific expectations
- [ ] Build and run full test suite

### For Grammar Fixes:
- [ ] Update grammar rules in `TSql*.g` files
- [ ] Update AST in `Ast.xml` if needed
- [ ] Update script generators
- [ ] Create test scripts and baselines
- [ ] Build parser and run tests

### For Predicate Recognition:
- [ ] Locate `IsNextRuleBooleanParenthesis()` in `TSql80ParserBaseInternal.cs`
- [ ] Add identifier detection logic
- [ ] Add test cases with parentheses
- [ ] Verify non-parentheses syntax still works

## 📊 Version Mapping Reference

Quick reference for SqlVersionFlags:

| Flag | SQL Server Version | Year | Common Features |
|------|-------------------|------|-----------------|
| TSql80AndAbove | 2000 | 2000 | Basic T-SQL |
| TSql90AndAbove | 2005 | 2005 | XML, CTEs |
| TSql100AndAbove | 2008 | 2008 | MERGE, FILESTREAM |
| TSql110AndAbove | 2012 | 2012 | Sequences, Window Functions |
| TSql120AndAbove | 2014 | 2014 | In-Memory OLTP, MAX_DURATION |
| TSql130AndAbove | 2016 | 2016 | JSON, Temporal Tables |
| TSql140AndAbove | 2017 | 2017 | Graph, STRING_AGG |
| TSql150AndAbove | 2019 | 2019 | UTF-8, Intelligent QP |
| TSql160AndAbove | 2022 | 2022 | RESUMABLE constraints, JSON improvements |
| TSql170AndAbove | 2025 | 2025 | VECTOR_SEARCH, AI features |

## 🧪 Testing Commands Reference

```bash
# Build parser only
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Build tests
dotnet build Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "FullyQualifiedName~YourTestName" -c Debug

# Run specific test file pattern
dotnet test --filter "DisplayName~AlterTableResumable" -c Debug

# Run full suite (ALWAYS do this before committing!)
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Run with detailed output
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug -v detailed
```

## 📂 Key Files Reference

| File | Purpose | When to Modify |
|------|---------|---------------|
| `TSql80ParserBaseInternal.cs` | Base validation logic | Validation fixes, common logic |
| `TSql160ParserBaseInternal.cs` | Version-specific overrides | Version-specific validation |
| `TSql*.g` | Grammar rules | New syntax, grammar changes |
| `Ast.xml` | AST node definitions | New nodes, type changes |
| `IndexOptionHelper.cs` | Option registration | New options, version mappings |
| `CodeGenerationSupporter.cs` | String constants | New keywords |
| `SqlScriptGeneratorVisitor*.cs` | Script generation | Generating SQL from AST |
| `Only*SyntaxTests.cs` | Test configuration | Test expectations per version |
| `TestScripts/*.sql` | Input test cases | New test SQL |
| `Baselines*/*.sql` | Expected output | Expected formatted SQL |

## 🚫 Common Pitfalls to Avoid

1. **❌ Modifying shared grammar rules** → Creates unintended side effects
   - ✅ Create context-specific rules instead

2. **❌ Not running full test suite** → Breaks existing functionality
   - ✅ Always run ALL 1,100+ tests before committing

3. **❌ Assuming same version for related features** → Incorrect validation
   - ✅ Check docs: MAX_DURATION (2014) ≠ RESUMABLE (2022)

4. **❌ Forgetting script generator updates** → Round-trip fails
   - ✅ Test parse → generate → parse cycle

5. **❌ Incorrect version flag logic** → Wrong validation behavior
   - ✅ Use `(flags & TSqlXXX) == 0` to check "NOT supported"

## 🎯 Quick Decision Matrix

| You Need To... | Use This Guide | Estimated Complexity |
|---------------|----------------|---------------------|
| Fix "option not valid" error | [VALIDATION_FIX_GUIDE.md](VALIDATION_FIX_GUIDE.md) | ⭐ Easy |
| Add new SQL keyword/operator | [BUG_FIXING_GUIDE.md](BUG_FIXING_GUIDE.md) | ⭐⭐⭐ Medium |
| Fix parentheses with predicates | [PARSER_PREDICATE_RECOGNITION_FIX.md](PARSER_PREDICATE_RECOGNITION_FIX.md) | ⭐⭐ Easy-Medium |
| Extend literal to expression | [GRAMMAR_EXTENSION_PATTERNS.md](GRAMMAR_EXTENSION_PATTERNS.md) | ⭐⭐⭐ Medium |
| Add new statement type | [BUG_FIXING_GUIDE.md](BUG_FIXING_GUIDE.md) | ⭐⭐⭐⭐ Hard |

## 📚 Related Documentation

- [copilot-instructions.md](copilot-instructions.md) - Main project documentation
- [VALIDATION_FIX_GUIDE.md](VALIDATION_FIX_GUIDE.md) - Version-gated validation fixes
- [BUG_FIXING_GUIDE.md](BUG_FIXING_GUIDE.md) - Grammar modifications and AST updates
- [GRAMMAR_EXTENSION_PATTERNS.md](GRAMMAR_EXTENSION_PATTERNS.md) - Common extension patterns
- [PARSER_PREDICATE_RECOGNITION_FIX.md](PARSER_PREDICATE_RECOGNITION_FIX.md) - Parentheses recognition

---

**Remember**: When in doubt, search for the error message first. Most bugs have been encountered before, and the error text will lead you to the right place in the code!
