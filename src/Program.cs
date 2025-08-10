namespace Boca;

using BoonOrg.Registrations.Domain;

class Program
{
    static void Main(string[] args)
    {
        var builder = Resolver.Builder();
        builder.Build("BFCS", ["BoonOrg.glTF", "BoonOrg.USD"]);
        Console.WriteLine("BFCS application loaded");

        var resolver = Resolver.Instance();

        var filePath = PathFromArguments.GetPath(args);
        if (!string.IsNullOrEmpty(filePath))
        {
            var executerWithLogging = new ScriptCommandExecuterWithLogging(resolver);
            executerWithLogging.ReadAndExecute(filePath);
        }
    }
}
