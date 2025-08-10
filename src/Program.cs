namespace Boca;

using BoonOrg.Registrations.Domain;

class Program
{
    static void Main(string[] args)
    {
        var builder = Resolver.Builder();

        Console.WriteLine("Loading addins...");
        builder.Build("BFCS", ["BoonOrg.glTF", "BoonOrg.USD", "BoonOrg.Horn"]);
        Console.WriteLine("Addins loaded");

        var resolver = Resolver.Instance();

        var filePath = PathFromArguments.GetPath(args);
        if (!string.IsNullOrEmpty(filePath))
        {
            var executerWithLogging = new ScriptCommandExecuterWithLogging(resolver);
            executerWithLogging.ReadAndExecute(filePath);
        }
    }
}
