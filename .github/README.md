# SqlScriptDOM Documentation Guide

Welcome to the SqlScriptDOM documentation! This folder contains comprehensive guides for understanding, developing, and debugging the SQL Server T-SQL parser.

## 🚀 Quick Start

**New to the project?** Start here:
1. Read [copilot-instructions.md](copilot-instructions.md) - Main project documentation
2. Browse [debugging_workflow.guidelines.instructions.md](instructions/debugging_workflow.guidelines.instructions.md) - Visual quick reference

**Fixing a bug?** Start here:
1. Open [debugging_workflow.guidelines.instructions.md](instructions/debugging_workflow.guidelines.instructions.md) - Identify bug type
2. Follow the flowchart to the appropriate guide
3. Use the step-by-step instructions

## 📚 Documentation Map

### Core Documentation

#### [copilot-instructions.md](copilot-instructions.md) - **START HERE**
**Purpose**: Main project documentation and overview  
**Contains**:
- Project structure and key files
- Build and test commands
- Developer workflow
- Bug fixing triage
- Debugging tips
- Grammar gotchas and pitfalls

**When to read**: First time working on the project, or for general context

---

### Quick Reference

#### [debugging_workflow.guidelines.instructions.md](instructions/debugging_workflow.guidelines.instructions.md) - **QUICK REFERENCE**
**Purpose**: Visual guide for quick bug diagnosis  
**Contains**:
- Diagnostic flowchart
- Error pattern recognition
- Investigation steps
- Testing commands reference
- Key files reference
- Common pitfalls

**When to use**: When you have a bug and need to quickly identify what type of fix is needed

---

### Specialized Fix Guides

#### [Validation_fix.guidelines.instructions.md](instructions/Validation_fix.guidelines.instructions.md) - Most Common Fix Type ⭐
**Purpose**: Fixing validation-based bugs  
**When to use**:
- ✅ Error: "Option 'X' is not valid..." or "Feature not supported..."
- ✅ Same syntax works in different context (e.g., ALTER INDEX vs ALTER TABLE)
- ✅ SQL Server version-specific features

**Contains**:
- Real-world example (ALTER TABLE ADD CONSTRAINT RESUMABLE)
- Version flag patterns
- Validation logic modification
- Testing strategy

**Complexity**: ⭐ Easy  
**Typical time**: 1-2 hours

---

#### [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md) - Grammar Changes
**Purpose**: Adding new syntax or modifying parser grammar  
**When to use**:
- ✅ Error: "Incorrect syntax near..." or "Unexpected token..."
- ✅ Parser doesn't recognize new T-SQL features
- ✅ Need to add new keywords, operators, or statements

**Contains**:
- Complete bug-fixing workflow
- Grammar modification process
- AST updates
- Script generator changes
- Baseline generation
- Decision tree for bug types

**Complexity**: ⭐⭐⭐ Medium to Hard  
**Typical time**: 4-8 hours

---

#### [parser.guidelines.instructions.md](instructions/parser.guidelines.instructions.md)
**Purpose**: Fixing parentheses recognition issues  
**When to use**:
- ✅ `WHERE PREDICATE(...)` works
- ❌ `WHERE (PREDICATE(...))` fails with syntax error
- ✅ Identifier-based boolean predicates

**Contains**:
- `IsNextRuleBooleanParenthesis()` modification
- Predicate detection patterns
- Real example (REGEXP_LIKE)

**Complexity**: ⭐⭐ Easy-Medium  
**Typical time**: 1-3 hours

---

#### [grammer.guidelines.instructions.md](instructions/grammer.guidelines.instructions.md)
**Purpose**: Common patterns for extending existing grammar  
**When to use**:
- ✅ Need to extend literal types to accept expressions
- ✅ Adding new enum members
- ✅ Creating new function/statement types

**Contains**:
- Literal to expression pattern
- Real example (VECTOR_SEARCH TOP_N)
- Context-specific grammar rules
- Shared rule warnings

**Complexity**: ⭐⭐⭐ Medium  
**Typical time**: 3-6 hours

---

### Meta Documentation

#### [documentation.guidelines.instructions.md](instructions/documentation.guidelines.instructions.md)
**Purpose**: Summary of documentation improvements  
**Contains**:
- What was improved and why
- Before/after comparison
- Real-world validation (ALTER TABLE RESUMABLE)
- Lessons learned

**When to read**: If you want to understand the documentation structure and evolution

---

## 🎯 Bug Type Decision Tree

```
┌─────────────────────────────────┐
│   You have a parsing bug        │
└───────────┬─────────────────────┘
            │
            ▼
    ┌───────────────┐
    │ What's the    │
    │ error message?│
    └───────┬───────┘
            │
   ┌────────┼────────┐
   │        │        │
   ▼        ▼        ▼
┌──────┐ ┌──────┐ ┌──────┐
│Option│ │Syntax│ │Parens│
│error │ │error │ │break │
└──┬───┘ └──┬───┘ └──┬───┘
   │        │        │
   ▼        ▼        ▼
┌──────┐ ┌──────┐ ┌──────┐
│VALID-│ │BUG   │ │PARSER│
│ATION │ │FIXING│ │PRED  │
│FIX   │ │GUIDE │ │RECOG │
└──────┘ └──────┘ └──────┘
```

## 📋 Quick Reference Table

| Error Message | Bug Type | Guide | Complexity |
|--------------|----------|-------|------------|
| "Option 'X' is not valid in statement Y" | Validation | [Validation_fix.guidelines.instructions.md](instructions/Validation_fix.guidelines.instructions.md) | ⭐ Easy |
| "Feature 'X' not supported in version Y" | Validation | [Validation_fix.guidelines.instructions.md](instructions/Validation_fix.guidelines.instructions.md) | ⭐ Easy |
| "Incorrect syntax near keyword" | Grammar | [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md) | ⭐⭐⭐ Medium |
| "Unexpected token" | Grammar | [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md) | ⭐⭐⭐ Medium |
| Syntax error with parentheses only | Predicate Recognition | [parser.guidelines.instructions.md](instructions/parser.guidelines.instructions.md) | ⭐⭐ Easy-Medium |
| Need to extend literal to expression | Grammar Extension | [GRAMMAR_EXTENSION_PATTERNS](GRAMMAR_EXTENSION_PATTERNS.md) | ⭐⭐⭐ Medium |

## 🔍 Common Scenarios

### Scenario 1: New SQL Server Feature Not Recognized
**Example**: `ALTER TABLE ... WITH (RESUMABLE = ON)` fails  
**Likely Issue**: Validation blocking the option  
**Start With**: [VALIDATION_FIX_GUIDE.md](VALIDATION_FIX_GUIDE.md)

### Scenario 2: New T-SQL Keyword Not Parsed
**Example**: `CREATE EXTERNAL TABLE` not recognized  
**Likely Issue**: Grammar doesn't have rules for this syntax  
**Start With**: [BUG_FIXING_GUIDE.md](BUG_FIXING_GUIDE.md)

### Scenario 3: Function Works Sometimes, Fails with Parentheses
**Example**: `WHERE REGEXP_LIKE(...)` fails  
**Likely Issue**: Predicate recognition  
**Start With**: [PARSER_PREDICATE_RECOGNITION_FIX.md](PARSER_PREDICATE_RECOGNITION_FIX.md)

### Scenario 4: Parameter Support Needed
**Example**: `TOP_N = @parameter` should work  
**Likely Issue**: Need to extend from literal to expression  
**Start With**: [GRAMMAR_EXTENSION_PATTERNS.md](GRAMMAR_EXTENSION_PATTERNS.md)

## 🛠️ Essential Commands

```bash
# Build parser
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug

# Run specific test
dotnet test --filter "FullyQualifiedName~YourTest" -c Debug

# Run ALL tests (CRITICAL before committing!)
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Search for error code
grep -r "SQL46057" SqlScriptDom/

# Search for option usage
grep -r "RESUMABLE" Test/SqlDom/TestScripts/
```

## 📊 Documentation Statistics

- **Total Guides**: 6 comprehensive guides
- **Bug Types Covered**: 3 main types (validation, grammar, predicate recognition)
- **Real-World Examples**: 4 detailed examples with code
- **Code Samples**: 50+ practical bash/C#/SQL examples
- **Quick References**: 3 tables and 2 flowcharts

## 🎓 Learning Path

### Beginner Path (Understanding the Project)
1. [copilot-instructions.md](copilot-instructions.md) - Read "Key points" section
2. [debugging_workflow.guidelines.instructions.md](instructions/debugging_workflow.guidelines.instructions.md) - Understand bug types
3. [Validation_fix.guidelines.instructions.md](instructions/Validation_fix.guidelines.instructions.md) - Follow ALTER TABLE RESUMABLE example
4. Try fixing a validation bug yourself

**Time**: 2-3 hours

### Intermediate Path (Grammar Changes)
1. Review beginner path first
2. [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md) - Complete workflow
3. [grammer.guidelines.instructions.md](instructions/grammer.guidelines.instructions.md) - Common patterns
4. [copilot-instructions.md](copilot-instructions.md) - "Grammar Gotchas" section
5. Try adding a simple new keyword

**Time**: 4-6 hours

### Advanced Path (Complex Features)
1. Master beginner and intermediate paths
2. [bug_fixing.guidelines.instructions.md](instructions/bug_fixing.guidelines.instructions.md) - AST modifications
3. [grammer.guidelines.instructions.md](instructions/grammer.guidelines.instructions.md) - All patterns
4. Study existing complex features (e.g., VECTOR_SEARCH)
5. Implement a new statement type

**Time**: 8-16 hours

## 🚨 Critical Reminders

### Always Do This:
- ✅ **Run full test suite** before committing (1,100+ tests)
- ✅ **Check Microsoft docs** for exact version support
- ✅ **Search for error messages** first before coding
- ✅ **Create context-specific rules** instead of modifying shared ones
- ✅ **Test across all SQL Server versions** in test configuration

### Never Do This:
- ❌ Modify shared grammar rules without understanding impact
- ❌ Skip running the full test suite
- ❌ Assume version support - always verify documentation
- ❌ Edit generated files in `obj/` directory
- ❌ Commit without testing baseline generation

## 🤝 Contributing

When improving these docs:
1. Use real examples from actual bugs
2. Include complete code samples (not pseudo-code)
3. Add bash commands that actually work
4. Cross-reference related guides
5. Update this README if adding new guides

## 📞 Getting Help

If stuck:
1. Search error message in codebase: `grep -r "your error"`
2. Check similar working syntax: `grep -r "keyword" Test/SqlDom/`
3. Review relevant guide based on bug type
4. Check Git history for similar fixes: `git log --grep="RESUMABLE"`

## 🎉 Success Metrics

You know you've succeeded when:
- ✅ Your specific test passes
- ✅ **ALL 1,100+ tests pass** (critical!)
- ✅ Baseline matches generated output
- ✅ Version-specific behavior is correct
- ✅ No regressions in existing functionality

---

**Last Updated**: Based on ALTER TABLE RESUMABLE fix (October 2025)

**Contributors**: Documentation improved based on practical bug-fixing experience

**Feedback**: These guides are living documents. Please update them when you discover new patterns or better approaches!
