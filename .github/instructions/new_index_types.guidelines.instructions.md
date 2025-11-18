# Guidelines for Adding New Index Types to SqlScriptDOM

This guide provides step-by-step instructions for adding support for new SQL Server index types to the SqlScriptDOM parser. This pattern was established from the JSON and Vector index implementations found in SQL Server 2025 (TSql170).

## When to Use This Guide

Use this pattern when:
- ✅ Adding a **completely new SQL Server index type** (e.g., JSON INDEX, VECTOR INDEX, SPATIAL INDEX)
- ✅ The index type has **specialized syntax** not handled by standard CREATE INDEX
- ✅ The index type requires **custom parsing logic** for type-specific clauses or options
- ✅ The index type is **introduced in a specific SQL Server version**

**Do NOT use this guide for:**
- ❌ Adding new index options to existing index types (use [validation_fix.guidelines.instructions.md](validation_fix.guidelines.instructions.md))
- ❌ Adding standard indexes with new keywords (use [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md))
- ❌ Adding function or data type syntax (use respective guides)

## Real-World Examples: JSON and Vector Indexes

### JSON Index Implementation
```sql
-- Basic JSON index
CREATE JSON INDEX IX_JSON_Basic ON dbo.Users (JsonData);

-- JSON index with FOR clause (multiple paths)
CREATE JSON INDEX IX_JSON_Paths ON dbo.Users (JsonData)
FOR ('$.name', '$.email', '$.age');

-- JSON index with WITH options
CREATE JSON INDEX IX_JSON_Options ON dbo.Users (JsonData)
WITH (OPTIMIZE_FOR_ARRAY_SEARCH = ON, MAXDOP = 4);
```

### Vector Index Implementation  
```sql
-- Basic vector index
CREATE VECTOR INDEX IX_Vector_Basic ON dbo.Documents (VectorData);

-- Vector index with metric and type
CREATE VECTOR INDEX IX_Vector_Complete ON dbo.Documents (VectorData)
WITH (METRIC = 'cosine', TYPE = 'DiskANN');

-- Vector index with filegroup
CREATE VECTOR INDEX IX_Vector_FG ON dbo.Documents (VectorData)
WITH (METRIC = 'dot')
ON [PRIMARY];
```

### Key Challenges Solved
- **Type-specific syntax**: JSON INDEX has `FOR (paths)` clause, VECTOR INDEX has `METRIC`/`TYPE` options
- **Custom columns**: Single column specification instead of column lists
- **Specialized options**: New index options specific to each index type
- **Grammar integration**: Seamless integration with existing CREATE INDEX patterns

## Step-by-Step Implementation Guide

### 1. Define AST Node Structure (`Ast.xml`)

Add a new class inheriting from `IndexStatement`:

```xml
<!-- Location: SqlScriptDom/Parser/TSql/Ast.xml -->
<!-- Add near other IndexStatement classes (~line 4640) -->

<Class Name="CreateYourTypeIndexStatement" Base="IndexStatement" Summary="Represents the create YOUR_TYPE index statement.">
    <InheritedMember Name="Name" ContainerClass="IndexStatement" />
    <InheritedMember Name="OnName" ContainerClass="IndexStatement" />
    <Member Name="SpecializedColumn" Type="Identifier" Summary="The specialized column for the index."/>
    <Member Name="TypeSpecificProperty" Type="StringLiteral" Collection="true" Summary="Type-specific properties. Optional may have zero elements."/>
    <Member Name="OnFileGroupOrPartitionScheme" Type="FileGroupOrPartitionScheme" Summary="Optional filegroup or partition scheme."/>
    <InheritedMember Name="IndexOptions" ContainerClass="IndexStatement" />
</Class>
```

**Design Principles**:
- **Inherit from `IndexStatement`**: All index types inherit from this base class
- **Reuse standard properties**: `Name`, `OnName`, `IndexOptions` come from base class
- **Add type-specific members**: Properties unique to your index type
- **Collections for lists**: Use `Collection="true"` for array-like properties
- **Optional members**: Members can be null for optional syntax elements

### 2. Add Grammar Rule (`TSql*.g`)

Create a specialized parsing rule for your index type:

```antlr
// Location: SqlScriptDom/Parser/TSql/TSql170.g (or appropriate version)
// Add after existing index statement rules (~line 17021)

createYourTypeIndexStatement [IToken tUnique, bool? isClustered] returns [CreateYourTypeIndexStatement vResult = FragmentFactory.CreateFragment<CreateYourTypeIndexStatement>()]
{
    Identifier vIdentifier;
    SchemaObjectName vSchemaObjectName;
    Identifier vSpecializedColumn;
    StringLiteral vProperty;
    FileGroupOrPartitionScheme vFileGroupOrPartitionScheme;
    
    if (tUnique != null)
    {
        ThrowIncorrectSyntaxErrorException(tUnique);
    }
    if (isClustered.HasValue)
    {
        ThrowIncorrectSyntaxErrorException(LT(1));
    }
}
    : tYourType:Identifier tIndex:Index vIdentifier=identifier
    {
        Match(tYourType, CodeGenerationSupporter.YourType);
        vResult.Name = vIdentifier;
    }
    tOn:On vSchemaObjectName=schemaObjectThreePartName
    {
        vResult.OnName = vSchemaObjectName;
    }
    LeftParenthesis vSpecializedColumn=identifier tRParen:RightParenthesis
    {
        vResult.SpecializedColumn = vSpecializedColumn;
        UpdateTokenInfo(vResult, tRParen);
    }
    (
        tFor:For LeftParenthesis
        vProperty=stringLiteral
        {
            AddAndUpdateTokenInfo(vResult, vResult.TypeSpecificProperty, vProperty);
        }
        (
            Comma vProperty=stringLiteral
            {
                AddAndUpdateTokenInfo(vResult, vResult.TypeSpecificProperty, vProperty);
            }
        )*
        RightParenthesis
    )?
    (
        // Greedy due to conflict with withCommonTableExpressionsAndXmlNamespaces
        options {greedy = true; } :
        With
        indexOptionList[IndexAffectingStatement.CreateIndex, vResult.IndexOptions, vResult]
    )?
    (
        On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
        {
            vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
        }
    )?
    ;
```

**Grammar Pattern Explanation**:
- **Parameter validation**: Reject UNIQUE and CLUSTERED if not supported
- **Standard index structure**: Name, ON table, column specification
- **Type-specific clauses**: Optional FOR, WITH, ON clauses as appropriate
- **Token matching**: Use `Match()` to verify keywords
- **Collection building**: Use `AddAndUpdateTokenInfo()` for lists

### 3. Integrate with Main Index Grammar

Add your index type to the main CREATE INDEX rule:

```antlr
// Location: SqlScriptDom/Parser/TSql/TSql170.g
// Find createIndexStatement rule (~line 16880) and add integration

createIndexStatement [IToken tUnique, bool? isClustered] returns [TSqlStatement vResult]
    : // ... existing alternatives ...
    | vResult=createYourTypeIndexStatement[tUnique, isClustered]
    ;

// Also add to ddlStatement if needed (~line 885)
ddlStatement returns [TSqlStatement vResult]
    : // ... existing alternatives ...
    | vResult=createYourTypeIndexStatement[null, null]
    // ... rest of alternatives ...
    ;
```

**Integration Requirements**:
- **Add to `createIndexStatement`**: Main CREATE INDEX dispatch rule
- **Add to `ddlStatement`**: Top-level DDL statement recognition
- **Parameter passing**: Pass `tUnique` and `isClustered` tokens
- **Consistent ordering**: Place appropriately among other index types

### 4. Add String Constants

Add necessary keywords to `CodeGenerationSupporter.cs`:

```csharp
// Location: SqlScriptDom/Parser/TSql/CodeGenerationSupporter.cs
// Add alphabetically in the constants section

internal const string YourType = "YOUR_TYPE";
internal const string YourTypeSpecificKeyword1 = "KEYWORD1";
internal const string YourTypeSpecificKeyword2 = "KEYWORD2";
```

**Naming Convention**:
- **Use exact SQL keyword casing**: `Json`, `Vector`, `Metric`
- **Group related constants**: Keep index-specific constants together
- **Follow existing patterns**: Match existing naming conventions

### 5. Add Index Options (if needed)

If your index type requires new index options, add them:

```csharp
// Location: SqlScriptDom/ScriptDom/SqlServer/IndexOptionKind.cs
// Add to the enum
public enum IndexOptionKind
{
    // ... existing options ...
    YourTypeSpecificOption1,
    YourTypeSpecificOption2,
    // ... rest of enum ...
}

// Location: SqlScriptDom/Parser/TSql/IndexOptionHelper.cs
// Add option mappings in the constructor
AddOptionMapping(IndexOptionKind.YourTypeSpecificOption1, CodeGenerationSupporter.YourTypeSpecificKeyword1, SqlVersionFlags.TSql170AndAbove);
AddOptionMapping(IndexOptionKind.YourTypeSpecificOption2, CodeGenerationSupporter.YourTypeSpecificKeyword2, SqlVersionFlags.TSql170AndAbove);
```

**Option Guidelines**:
- **Add to enum**: Define new `IndexOptionKind` values
- **Register mappings**: Map keywords to enum values with version flags
- **Version compatibility**: Use appropriate `SqlVersionFlags`

### 6. Create Script Generator

Implement the visitor method to convert AST back to T-SQL:

```csharp
// Location: SqlScriptDom/ScriptDom/SqlServer/ScriptGenerator/SqlScriptGeneratorVisitor.CreateYourTypeIndexStatement.cs
// Create new file following naming convention

//------------------------------------------------------------------------------
// <copyright file="SqlScriptGeneratorVisitor.CreateYourTypeIndexStatement.cs" company="Microsoft">
//         Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------
using System.Collections.Generic;
using Microsoft.SqlServer.TransactSql.ScriptDom;

namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator
{
    partial class SqlScriptGeneratorVisitor
    {
        public override void ExplicitVisit(CreateYourTypeIndexStatement node)
        {
            GenerateKeyword(TSqlTokenType.Create);

            GenerateSpaceAndIdentifier(CodeGenerationSupporter.YourType);

            GenerateSpaceAndKeyword(TSqlTokenType.Index);

            // name
            GenerateSpaceAndFragmentIfNotNull(node.Name);

            NewLineAndIndent();
            GenerateKeyword(TSqlTokenType.On);
            GenerateSpaceAndFragmentIfNotNull(node.OnName);

            // Specialized column
            if (node.SpecializedColumn != null)
            {
                GenerateSpace();
                GenerateSymbol(TSqlTokenType.LeftParenthesis);
                GenerateFragmentIfNotNull(node.SpecializedColumn);
                GenerateSymbol(TSqlTokenType.RightParenthesis);
            }

            // Type-specific clause
            if (node.TypeSpecificProperty != null && node.TypeSpecificProperty.Count > 0)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.For);
                GenerateSpace();
                GenerateParenthesisedCommaSeparatedList(node.TypeSpecificProperty);
            }

            GenerateIndexOptions(node.IndexOptions);

            if (node.OnFileGroupOrPartitionScheme != null)
            {
                NewLineAndIndent();
                GenerateKeyword(TSqlTokenType.On);

                GenerateSpaceAndFragmentIfNotNull(node.OnFileGroupOrPartitionScheme);
            }
        }
    }
}
```

**Script Generation Patterns**:
- **Use `GenerateKeyword()`**: For T-SQL keywords like CREATE, INDEX, ON
- **Use `GenerateIdentifier()`**: For type-specific keywords
- **Use `NewLineAndIndent()`**: For proper formatting with line breaks
- **Use `GenerateIndexOptions()`**: Reuse existing index option generation
- **Handle collections**: Use `GenerateParenthesisedCommaSeparatedList()` for arrays

### 7. Build and Test Grammar Changes

Build the project to regenerate parser files:

```bash
# Build the ScriptDOM project to regenerate parser from grammar
dotnet build SqlScriptDom/Microsoft.SqlServer.TransactSql.ScriptDom.csproj -c Debug
```

**Common Build Issues**:
- **Grammar syntax errors**: Check ANTLR syntax in `.g` files
- **Missing constants**: Ensure all referenced constants exist in `CodeGenerationSupporter.cs`
- **AST node mismatches**: Verify AST class names match grammar return types
- **Option registration**: Ensure new index options are properly registered

### 8. Create Comprehensive Test Scripts

Create test script covering all syntax variations:

```sql
-- File: Test/SqlDom/TestScripts/YourTypeIndexTests170.sql

-- Basic index creation
CREATE YOUR_TYPE INDEX IX_YourType_Basic ON dbo.Table1 (SpecializedColumn);

-- Index with type-specific clause
CREATE YOUR_TYPE INDEX IX_YourType_WithClause ON dbo.Table1 (SpecializedColumn)
FOR ('value1', 'value2', 'value3');

-- Index with WITH options
CREATE YOUR_TYPE INDEX IX_YourType_WithOptions ON dbo.Table1 (SpecializedColumn)
WITH (YOUR_TYPE_OPTION1 = 'value', MAXDOP = 4);

-- Index with type-specific clause and WITH options
CREATE YOUR_TYPE INDEX IX_YourType_Complete ON dbo.Table1 (SpecializedColumn)
FOR ('property1', 'property2')
WITH (YOUR_TYPE_OPTION1 = 'setting', YOUR_TYPE_OPTION2 = 'config');

-- Index on schema-qualified table
CREATE YOUR_TYPE INDEX IX_YourType_Schema ON MySchema.MyTable (Column1)
FOR ('path.value');

-- Index with quoted identifiers
CREATE YOUR_TYPE INDEX [IX YourType Index] ON [dbo].[Table1] ([Column Name])
FOR ('complex.path.expression');

-- Index with filegroup
CREATE YOUR_TYPE INDEX IX_YourType_Filegroup ON dbo.Table1 (Column1)
WITH (YOUR_TYPE_OPTION1 = 'setting')
ON [PRIMARY];

-- Index with complex options
CREATE YOUR_TYPE INDEX IX_YourType_AllOptions ON dbo.Table1 (Column1)
FOR ('value1', 'value2')
WITH (YOUR_TYPE_OPTION1 = 'config', YOUR_TYPE_OPTION2 = 'setting', MAXDOP = 8, ONLINE = OFF);
```

**Test Coverage Requirements**:
- **All syntax variations**: Basic, with clauses, with options, combinations
- **Schema qualification**: Different schema and table names
- **Quoted identifiers**: Test case sensitivity and special characters
- **Integration contexts**: Filegroups, partition schemes, standard index options
- **Edge cases**: Empty clauses, maximum option combinations

### 9. Generate Baseline Files

Create the expected output baseline:

1. **Create placeholder baseline**: `Test/SqlDom/Baselines170/YourTypeIndexTests170.sql`
2. **Run the test** (will fail initially):
   ```bash
   dotnet test --filter "YourTypeIndexTests170" Test/SqlDom/UTSqlScriptDom.csproj -c Debug
   ```
3. **Copy "Actual" output** from test failure into baseline file
4. **Verify formatting** matches parser's standard formatting

**Baseline Example** (JSON index):
```sql
CREATE JSON INDEX IX_JSON_Basic
ON dbo.Users (JsonData);

CREATE JSON INDEX IX_JSON_Paths
ON dbo.Users (JsonData) FOR ('$.name', '$.email', '$.age');

CREATE JSON INDEX IX_JSON_Options
ON dbo.Users (JsonData) WITH (OPTIMIZE_FOR_ARRAY_SEARCH = ON, MAXDOP = 4);
```

### 10. Configure Test Expectations

Add test configuration to version-specific test class:

```csharp
// Location: Test/SqlDom/Only170SyntaxTests.cs (or appropriate version)
// Add to the ParserTest170 array

new ParserTest170("YourTypeIndexTests170.sql"),
```

**Error Count Guidelines**:
- **Count all syntax instances**: Each CREATE INDEX statement in test script
- **Consider SQL version support**: When was the index type actually introduced?
- **Account for options**: New index options may add additional errors in older versions
- **Test validation**: Run tests to verify error counts are accurate

### 11. Full Test Suite Validation

Run complete test suite to ensure no regressions:

```bash
# Run all ScriptDOM tests
dotnet test Test/SqlDom/UTSqlScriptDom.csproj -c Debug

# Expected result: All tests pass, including new index type tests
# Total tests: 1,100+ (number increases with new features)
```

**Regression Prevention**:
- **Grammar changes can break existing functionality**: Shared rules affect multiple contexts
- **AST changes can break script generation**: Ensure all visitors are updated
- **Index option additions**: New options shouldn't conflict with existing ones
- **Version compatibility**: New syntax shouldn't break older version parsers

## Advanced Considerations

### Version-Specific Implementation

For index types introduced in specific SQL Server versions:

```antlr
// Different grammar files for different SQL versions
// TSql160.g - SQL Server 2022 features (if backporting)
// TSql170.g - SQL Server 2025 features  
// TSqlFabricDW.g - Azure Synapse features
```

**Guidelines**:
- **Target appropriate version**: Add to the SQL version where feature was introduced
- **Cascade to later versions**: Copy rules to all subsequent version grammar files
- **Version-specific testing**: Test error behavior in older parsers

### Complex Index Options

For index types requiring specialized index options:

```xml
<!-- Advanced AST index option nodes -->
<Class Name="YourTypeIndexOption" Base="IndexOption" Summary="Represents specialized index option.">
    <Member Name="OptionValue" Type="StringLiteral" Summary="The option value"/>
</Class>
```

**When to use complex options**:
- **Type-specific options**: Options that only apply to your index type
- **Complex values**: Options with structured values or multiple parameters
- **Validation requirements**: Options that need special validation logic

### Filegroup and Partition Support

For index types that support filegroups or partitioning:

```antlr
// Add filegroup support to your grammar rule
(
    On vFileGroupOrPartitionScheme=filegroupOrPartitionScheme
    {
        vResult.OnFileGroupOrPartitionScheme = vFileGroupOrPartitionScheme;
    }
)?
```

**Filegroup considerations**:
- **Optional support**: Not all index types support filegroups
- **Partition schemes**: Some index types may support partitioning
- **Storage options**: Consider FILESTREAM or in-memory storage

## Common Pitfalls and Solutions

### 1. Forgetting Script Generator Implementation
**Problem**: AST node created but no script generation visitor
**Solution**: Always implement `ExplicitVisit()` method for new index statement nodes

### 2. Incorrect Grammar Integration  
**Problem**: Index type not recognized in all CREATE INDEX contexts
**Solution**: Ensure integration with both `createIndexStatement` and `ddlStatement` rules

### 3. Missing Index Option Registration
**Problem**: New index options not recognized by parser
**Solution**: Add options to `IndexOptionKind` enum and register in `IndexOptionHelper`

### 4. Incomplete Test Coverage
**Problem**: Edge cases not covered in testing
**Solution**: Test all syntax variations, option combinations, and integration contexts

### 5. Grammar Conflicts
**Problem**: New keywords conflict with existing grammar
**Solution**: Use proper semantic predicates and context-specific matching

### 6. Version Compatibility Issues
**Problem**: New index type breaks older version parsers unexpectedly
**Solution**: Add proper version checks and test all SQL Server versions

## Validation Checklist

- [ ] **AST Definition**: New class inherits from `IndexStatement` with appropriate members
- [ ] **Grammar Rules**: Specialized parsing rule handles all syntax variations  
- [ ] **Grammar Integration**: Connected to `createIndexStatement` and `ddlStatement`
- [ ] **String Constants**: All keywords added to `CodeGenerationSupporter.cs`
- [ ] **Index Options**: New options added to enum and registered with version flags
- [ ] **Script Generator**: `ExplicitVisit()` method generates correct T-SQL output
- [ ] **Test Scripts**: Comprehensive test coverage including edge cases
- [ ] **Baseline Files**: Generated output matches expected formatted T-SQL
- [ ] **Test Configuration**: Error counts configured for all SQL Server versions
- [ ] **Build Success**: Project builds without errors and regenerates parser
- [ ] **Full Test Suite**: All existing tests continue to pass (no regressions)

## Related Guides

- [bug_fixing.guidelines.instructions.md](bug_fixing.guidelines.instructions.md) - For general grammar modifications
- [new_data_types.guidelines.instructions.md](new_data_types.guidelines.instructions.md) - For adding new data types
- [validation_fix.guidelines.instructions.md](validation_fix.guidelines.instructions.md) - For validation-only issues
- [testing.guidelines.instructions.md](testing.guidelines.instructions.md) - For comprehensive testing strategies

## Real-World Examples

### JSON Index (SQL Server 2025)
- **AST Class**: `CreateJsonIndexStatement` with `JsonColumn` and `ForJsonPaths` members
- **Syntax**: `CREATE JSON INDEX name ON table (column) FOR (paths)`
- **Special Features**: FOR clause with path specifications, array search optimization
- **Challenge**: Multiple JSON path support in FOR clause

### Vector Index (SQL Server 2025)  
- **AST Class**: `CreateVectorIndexStatement` with `VectorColumn` member
- **Syntax**: `CREATE VECTOR INDEX name ON table (column) WITH (METRIC = value)`
- **Special Features**: Vector-specific metrics (cosine, dot, euclidean), DiskANN type
- **Challenge**: Specialized index options for vector operations

### Future Examples  
This pattern can be applied to other SQL Server index types like:
- **SPATIAL INDEX**: Geometry/geography data indexing
- **FULLTEXT INDEX**: Text search indexing  
- **XML INDEX**: XML document indexing
- **Custom Index Types**: Future SQL Server indexing technologies

The JSON and Vector implementations serve as canonical examples for this pattern and should be referenced for similar future index type additions.