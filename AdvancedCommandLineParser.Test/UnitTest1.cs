using CommandLine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;

namespace AdvancedCommandLineParser.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public async Task Test_Macro_and_Verb_string()
        {
            var teststring = new string[] { "macro", "verb" };

            await CommandLine.ParseAndRunAsync(teststring);


        }
    }


  

}
