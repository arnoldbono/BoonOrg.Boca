namespace Boca;

using BoonOrg.Commands;
using BoonOrg.Registrations.Domain;
using BoonOrg.Scripting;
using BoonOrg.Storage;

class Program
{
    static void Main(string[] args)
    {
        var builder = Resolver.Builder();
        builder.Build("BFCS", ["BoonOrg.glTF"]);

        var resolver = Resolver.Instance();

        for (int i = 0; i < args.Length; i++)
        {
            if (args[i].Equals("--file", StringComparison.OrdinalIgnoreCase) ||
                args[i].Equals("-f", StringComparison.OrdinalIgnoreCase))
            {
                if (i + 1 < args.Length)
                {
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
                        return;
                    }

                    Console.WriteLine($"Processing file: {filePath}");
                    var documentServer = resolver.Resolve<IDocumentServer>();
                    var document = documentServer.Prepare(filePath);
                    Console.WriteLine($"Prepared: {filePath}");

                    document.Initialize(filePath);

                    var scriptCommandFileReader = resolver.Resolve<IScriptCommandFileReader>();
                    scriptCommandFileReader.Read(document, filePath);

                    var scriptCommandFile = scriptCommandFileReader.ScriptCommandFile;
                    Console.WriteLine($"Read {scriptCommandFile.Lines.Count()} lines");
                    Console.WriteLine($"Read {scriptCommandFile.Commands.Count()} commands");

                    var scriptCommandExecutor = resolver.Resolve<IScriptCommandExecutor>();
                    var commandExecuter = resolver.Resolve<ICommandExecuter>();

                    scriptCommandFileReader.ResultMessageHandler += CommandFileReader_ResultMessage;
                    scriptCommandFileReader.ErrorMessageHandler += CommandFileReader_ErrorMessage;

                    scriptCommandExecutor.Execute(commandExecuter, scriptCommandFile, scriptCommandFileReader);

                    scriptCommandFileReader.ResultMessageHandler -= CommandFileReader_ResultMessage;
                    scriptCommandFileReader.ErrorMessageHandler -= CommandFileReader_ErrorMessage;
                    Console.WriteLine("Script command file execution completed.");
                }
                else
                {
                    Console.WriteLine("No file specified after --file or -f option.");
                }
                return;
            }
        }

        Console.WriteLine("BFCS application got loaded!");
    }

    private static void CommandFileReader_ErrorMessage(IMessage message)
    {
        Console.WriteLine($@"{Environment.NewLine}Error: {message.Text}{Environment.NewLine}");
    }

    private static void CommandFileReader_ResultMessage(IMessage message)
    {
        Console.WriteLine($@"{message.Text}{Environment.NewLine}");
    }
}
