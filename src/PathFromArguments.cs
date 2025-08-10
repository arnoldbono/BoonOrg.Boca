namespace Boca;

using System;
using System.IO;

class PathFromArguments
{
    static public string? GetPath(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Missing option --file or -f.");
            return null;
        }

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--file", StringComparison.OrdinalIgnoreCase) ||
                args[i].Equals("-f", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 >= args.Length)
                {
                    Console.WriteLine("No file specified after --file or -f option.");
                    return null;
                }

                var filePath = args[i + 1];
                if (filePath.StartsWith('~'))
                {
                    var homeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
                    filePath = Path.Combine(homeDirectory, filePath.Substring(1));
                }

                filePath = Path.GetFullPath(filePath);
                if (!File.Exists(filePath))
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return null;
                }

                return filePath;
            }
        }

        Console.WriteLine("Missing option --file or -f.");
        return null;
    }
}