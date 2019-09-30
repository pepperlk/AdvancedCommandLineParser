using CommandLine;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AdvancedCommandLineParser
{
    public class CommandLine
    {
        public static async Task<int> ParseAndRunAsync(string[] args)
        {
            var typeswithgroups = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.GetCustomAttribute<VerbGroupAttribute>() != null)
               .GroupBy(t => t.GetCustomAttribute<VerbGroupAttribute>().Group)

               .ToArray();

            if (args.Length > 0 && typeswithgroups.Any(g => g.Key.ToLower() == args[0].ToLower()))
            {
                // try match groups
                var group = typeswithgroups.FirstOrDefault(g => g.Key.ToLower() == args[0].ToLower());

                args = args.Skip(1).ToArray();

                return Parser.Default.ParseArguments(args, group.ToArray()).MapResult((options) =>
                {

                    if (options is Command)
                    {
                        var cmd = options as Command;
                        return cmd.Run().GetAwaiter().GetResult();
                    }
                    else
                    {

                        var ruturnobj = options.GetType().GetMethod("Run").Invoke(options, null);
                        if (ruturnobj is int)
                        {
                            return (int)ruturnobj;
                        }
                    }
                    return 1;

                },
                  errs =>
                  {

                      return 1;

                  });


            }
            else
            {

                Parser.Default.ParseArguments<RootOptions>(args)
   .WithParsed<RootOptions>(opts => RunOptionsAndReturnExitCode(opts));


            }


            var groups = typeswithgroups.Select(g => g.Key);
            SuperConsole.Log("");
            SuperConsole.Log("Available Groups:");
            SuperConsole.Log("");
            foreach (var group in groups)
            {
                SuperConsole.Log(group);
                SuperConsole.Log("");
            }

            return 1;
        }

        private static void RunOptionsAndReturnExitCode(RootOptions opts)
        {
            // print all verbs... 

            var typeswithgroups = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).Where(t => t.GetCustomAttribute<VerbGroupAttribute>() != null)
              .GroupBy(t => t.GetCustomAttribute<VerbGroupAttribute>().Group)

              .ToArray();

            // blah

        }
    }
}
