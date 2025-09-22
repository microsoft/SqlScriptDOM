using System;
using System.IO;
using Microsoft.SqlServer.TransactSql.ScriptDom;

class Program
{
    static void Main(string[] args)
    {
        var sql = @"-- This is a single line comment
SELECT * FROM MyTable;";

        var parser = new TSql160Parser(true);
        var fragment = parser.ParseStatementList(new StringReader(sql), out var errors);
        
        Console.WriteLine($"Parse errors: {errors.Count}");
        Console.WriteLine($"Fragment type: {fragment.GetType().Name}");
        Console.WriteLine($"Script token stream count: {fragment.ScriptTokenStream?.Count ?? 0}");
        
        if (fragment.ScriptTokenStream != null)
        {
            Console.WriteLine("\nAll tokens in stream:");
            for (int i = 0; i < fragment.ScriptTokenStream.Count; i++)
            {
                var token = fragment.ScriptTokenStream[i];
                Console.WriteLine($"  [{i}] {token.TokenType}: '{token.Text}'");
            }
        }
        
        Console.WriteLine($"\nFragment FirstTokenIndex: {fragment.FirstTokenIndex}");
        Console.WriteLine($"Fragment LastTokenIndex: {fragment.LastTokenIndex}");
        
        // Generate script without comment preservation
        var generator = new Sql160ScriptGenerator(new SqlScriptGeneratorOptions { PreserveComments = false });
        generator.GenerateScript(fragment, out var generatedScript);
        Console.WriteLine($"\nGenerated script without comments:\n'{generatedScript}'");
        
        // Generate script with comment preservation
        var generatorWithComments = new Sql160ScriptGenerator(new SqlScriptGeneratorOptions { PreserveComments = true });
        generatorWithComments.GenerateScript(fragment, out var generatedScriptWithComments);
        Console.WriteLine($"\nGenerated script with comments:\n'{generatedScriptWithComments}'");
    }
}