using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TokenListGenerator
{
    public class TokenListGenerator
    {
        private const Char Separator = '=';
        private const Char TrimChar = '"';
        private const String Tab = "    ";
        private static readonly String[] Libraries = { "System" };
        private const String NamespaceHeader = "namespace Microsoft.SqlServer.TransactSql.ScriptDom.ScriptGenerator\r\n{";
        private const String NamespaceFooter = "}";
        private const String ClassDeclHeader = Tab + "internal static partial class ScriptGeneratorSupporter \r\n" + Tab + "{";
        private const String ClassDeclFooter = Tab + "}";
        private const String ArrayDeclHeader = Tab + Tab + "private static string[] _typeStrings = {";
        private const String ArrayDeclFooter = Tab + Tab + "};";

        private static Dictionary<String, String> _map;

        public struct Definition
        {
            public Definition(String name, String info)
            {
                _name = name;
                _info = info;
            }

            public String Name
            {
                get { return _name; }
            }

            public String info
            {
                get { return _info; }
            }

            public void Print()
            {
                Console.WriteLine("{0}{0}{0}\"{1}\", // {2}", Tab, _name, _info);
            }

            private readonly String _name;
            private readonly String _info;
        }

        static TokenListGenerator()
        {
            _map = new Dictionary<String, String>();

            _map.Add("Disk", "disk");
            _map.Add("Precision", "precision");
            _map.Add("External", "external");
            _map.Add("Revert", "revert");
            _map.Add("Pivot", "pivot");
            _map.Add("Unpivot", "unpivot");
            _map.Add("TableSample", "tablesample");
            _map.Add("Bang", "!");
            _map.Add("PercentSign", "%");
            _map.Add("Ampersand", "&");
            _map.Add("LeftParenthesis", "(");
            _map.Add("RightParenthesis", ")");
            _map.Add("LeftCurly", "{");
            _map.Add("RightCurly", "}");
            _map.Add("Star", "*");
            _map.Add("MultiplyEquals", "*=");
            _map.Add("Plus", "+");
            _map.Add("Comma", ",");
            _map.Add("Minus", "-");
            _map.Add("Dot", ".");
            _map.Add("Divide", "/");
            _map.Add("Colon", ":");
            _map.Add("DoubleColon", "::");
            _map.Add("Semicolon", ";");
            _map.Add("LessThan", "<");
            _map.Add("EqualsSign", "=");
            _map.Add("RightOuterJoin", "=*");
            _map.Add("GreaterThan", ">");
            _map.Add("Circumflex", "^");
            _map.Add("VerticalLine", "|");
            _map.Add("Tilde", "~");
            _map.Add("Go", "go");            
            _map.Add("Merge", "merge");
            _map.Add("StopList", "stoplist");
            _map.Add("SemanticKeyPhraseTable", "semantickeyphrasetable");
            _map.Add("SemanticSimilarityTable", "semanticsimilaritytable");
            _map.Add("SemanticSimilarityDetailsTable", "semanticsimilaritydetailstable");
            _map.Add("TryConvert", "try_convert");
            _map.Add("Dump", "dump");
            _map.Add("Load", "load");
            _map.Add("AddEquals", "+=");
            _map.Add("SubtractEquals", "-=");
            _map.Add("DivideEquals", "/=");
            _map.Add("ModEquals", "%=");
            _map.Add("BitwiseAndEquals", "&=");
            _map.Add("BitwiseOrEquals", "|=");
            _map.Add("BitwiseXorEquals", "^=");
            _map.Add("LeftShift", "<<");
            _map.Add("RightShift", ">>");
            _map.Add("Concat", "||");
            _map.Add("ConcatEquals", "||=");
        }

        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: TokenListGenerator <input filename>");
                return;
            }

            try
            {
                using (StreamReader reader = new StreamReader(args[0]))
                {
                    IList<Definition> definitions = Parse(reader);

                    foreach (var item in Libraries)
                    {
                        Console.WriteLine("using {0};", item);
                    }

                    Console.WriteLine(NamespaceHeader);
                    Console.WriteLine(ClassDeclHeader);
                    Console.WriteLine(ArrayDeclHeader);

                    foreach (var item in definitions)
                    {
                        item.Print();
                    }

                    Console.WriteLine(ArrayDeclFooter);
                    Console.WriteLine(ClassDeclFooter);
                    Console.WriteLine(NamespaceFooter);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static IList<Definition> Parse(StreamReader reader)
        {
            List<Definition> definitions = new List<Definition>();

            // add the predefined ones
            for (int i = 0; i < 4; i++)
            {
                definitions.Add(new Definition(String.Empty, i.ToString()));
            }

            for (String line = reader.ReadLine(); line != null; line = reader.ReadLine())
            {
                String[] parts = line.Split(Separator);
                if (parts.Length == 3) // found a regular one
                {
                    definitions.Add(new Definition(parts[1].Trim(TrimChar), String.Format("{0};", parts[2])));
                }
                else if (parts.Length == 2) // found an informational one
                {
                    String value;

                    if (_map.TryGetValue(parts[0], out value))
                    {
                        definitions.Add(new Definition(value, String.Format("{0}; {1}", parts[1], parts[0])));
                    }
                    else
                    {
                        definitions.Add(new Definition(String.Empty, String.Format("{0}; {1}", parts[1], parts[0])));
                    }
                }
            }

            return definitions;
        }
    }
}
