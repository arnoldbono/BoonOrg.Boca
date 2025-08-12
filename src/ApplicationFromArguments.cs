namespace Boca;

using System;

class ApplicationFromArguments
{
    static public string GetApplication(string[] args)
    {
        var application = "BFCS"; // Default application name

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--application", StringComparison.OrdinalIgnoreCase) ||
                args[i].Equals("-a", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    Console.WriteLine("No application specified after --application or -a option.");
                    break;
                }

                application = args[++i].Trim();
                break;
            }
        }

        return application;
    }
}