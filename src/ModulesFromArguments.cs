namespace Boca;

using System;

class ModulesFromArguments
{
    static public string[] GetModules(string[] args)
    {
        var modules = new List<string>();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--module", StringComparison.OrdinalIgnoreCase) ||
                args[i].Equals("-m", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    Console.WriteLine("No file specified after --module or -m option.");
                    break;
                }

                modules.Add(args[++i].Trim());
            }
        }

        return [.. modules];
    }
}