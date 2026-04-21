---
name: verify-and-test-tsql-syntax
description: Verify whether an exact T-SQL script is already supported by SqlScriptDOM, then add or update the correct parser tests and baselines. Use when a user asks if syntax already works, or when adding regression coverage for a specific script.
argument-hint: "Exact T-SQL script or syntax to verify"
user-invocable: true
---

## Overview

This skill determines whether a specific T-SQL script is already supported and then guides the follow-up testing work.

Use it for:
- verifying whether a script already parses
- confirming which parser version should support the syntax
- adding or updating regression coverage in the existing ScriptDOM test framework

Do not use it for broad feature design. If the verification shows missing parser support, route the implementation work to the appropriate repo instruction:
- [bug_fixing.guidelines.instructions.md](../../../.github/instructions/bug_fixing.guidelines.instructions.md)
- [grammar_validation.guidelines.instructions.md](../../../.github/instructions/grammar_validation.guidelines.instructions.md)
- [parser.guidelines.instructions.md](../../../.github/instructions/parser.guidelines.instructions.md)
- [testing.guidelines.instructions.md](../../../.github/instructions/testing.guidelines.instructions.md)

## Core Rules

- Always verify the exact T-SQL text first, character for character.
- For first-pass verification, add a debug unit test method to an existing test file such as `Only180SyntaxTests.cs` when the syntax targets vNext or the latest parser.
- Do not create standalone console apps, ad hoc parsers, or extra test projects.
- After verification, add proper test coverage through `TestScripts`, baselines, and the existing syntax test classes.
- Remove any temporary debug-only test methods once the result is confirmed.

## Workflow

### 1. Gather the minimum inputs

Ask for:
- the exact T-SQL script to verify
- the SQL Server version or parser version expected to support it
- the current behavior and the expected behavior

If the user has not supplied the exact script yet, stop and request it before continuing.

### 2. Verify the exact script first

Add a temporary debug unit test method to the appropriate existing test class and test the exact script as provided.

Use this sequence:

```bash
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
dotnet test --filter "DebugExactScriptTest" Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

Verification expectations:
- If parsing succeeds, move directly to comprehensive test coverage.
- If parsing fails, capture the exact parse errors and classify the failure.

### 3. Classify the gap if verification fails

Use the failure mode to route the next step:
- Grammar issue: parser does not recognize the syntax at all.
- Validation issue: syntax parses but is rejected as unsupported or invalid in context.
- Predicate recognition issue: identifier-based predicate fails in parentheses.

When similar syntax already works in another context, prefer validation analysis before changing grammar.

### 4. Add comprehensive coverage

After verification, create or update the proper permanent tests using the existing framework:

- Add the input script under `Test/SqlDom/TestScripts/`.
- Add the matching baseline under `Test/SqlDom/Baselines<version>/`.
- Add the `ParserTest` entry or permanent test method in the correct `Only<version>SyntaxTests.cs` file.
- Preserve the exact user syntax in the test script before adding broader coverage.

Determine the parser version using the repo mapping:
- SQL Server 2014 -> TSql120
- SQL Server 2016 -> TSql130
- SQL Server 2017 -> TSql140
- SQL Server 2019 -> TSql150
- SQL Server 2022 -> TSql160
- SQL Server 2025 -> TSql170
- SQL Server vNext/latest -> TSql180

### 5. Validate the testing slice

Run validation in this order:

```bash
dotnet test --filter "FullyQualifiedName~YourFeatureTest" Test/SqlDom/UTSqlScriptDom.csproj -c Debug
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug
```

If the targeted test fails because of a baseline mismatch:
- use the generated output from the failure log to update the baseline
- rerun the same targeted test
- only then run the full suite

## File Targets

Use these repo paths when doing the work:
- Grammar files: `SqlScriptDom/Parser/TSql/TSql*.g`
- AST definition: `SqlScriptDom/Parser/TSql/Ast.xml`
- Validation code: `SqlScriptDom/Parser/TSql/TSql80ParserBaseInternal.cs`
- Test scripts: `Test/SqlDom/TestScripts/`
- Baselines: `Test/SqlDom/Baselines<version>/`
- Syntax tests: `Test/SqlDom/Only<version>SyntaxTests.cs`

## Completion Checklist

- [ ] Verified the exact script first in an existing test file
- [ ] Classified the result as already supported or missing support
- [ ] Added or updated permanent test coverage in the standard test framework
- [ ] Preserved the exact user syntax in the new test script
- [ ] Ran targeted validation for the touched test slice
- [ ] Ran the full `UTSqlScriptDom` suite before finishing