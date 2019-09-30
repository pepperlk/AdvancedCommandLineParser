using CommandLine;
using System;
using System.Threading.Tasks;

namespace AdvancedCommandLineParser.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var teststring = new string[] { "macro", "verb" };

            CommandLine.ParseAndRunAsync(teststring).GetAwaiter().GetResult();
        }
    }

    [VerbGroup("macro"), Verb("verb")]
    public class MacroVerb : Command
    {
        public override async Task<int> Run()
        {
            return 0;
        }

    }
}
