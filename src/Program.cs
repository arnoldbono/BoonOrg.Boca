namespace Boca;

using BoonOrg.Registrations.Domain;

class Program
{
    static void Main(string[] args)
    {
        var builder = Resolver.Builder();

        var application = ApplicationFromArguments.GetApplication(args);
        var modules = ModulesFromArguments.GetModules(args);

        Console.WriteLine($"Loading {application} and addins {string.Join(",", modules)}...");
        builder.Build(application, modules);
        Console.WriteLine("Loaded");

        var resolver = Resolver.Instance();

        var filePath = PathFromArguments.GetPath(args);
        if (string.IsNullOrEmpty(filePath))
        {
            Console.WriteLine("No file specified. Use --file or -f option.");
            return;
        }

        var executerWithLogging = new ScriptCommandExecuterWithLogging(resolver);
        executerWithLogging.ReadAndExecute(filePath);
    }
}
