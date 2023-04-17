using System;

namespace Test.SqlStudio.AssemblyTools
{
    [Flags]
    public enum SqlPermissionLevel
    {     
        ViewDefinition = 1,
        References = 2,
        SelectOnExpressionDependencies = 4,
        Full = 8,
        ViewDefinitionWithReferences = ViewDefinition|References,      
        ViewDefinitionWithSelectOnExpressionDependencies = ViewDefinition|SelectOnExpressionDependencies,       
        ViewDefinitionWithReferencesAndSelectOnExpressionDependencies = ViewDefinitionWithSelectOnExpressionDependencies|References     
    }
}
