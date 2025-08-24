namespace Boca;

using BoonOrg.Commands;
using BoonOrg.Registrations;
using BoonOrg.Scripting;
using BoonOrg.Storage;

class ScriptCommandExecuterWithLogging
{
    private readonly IResolver m_resolver;
    
    public ScriptCommandExecuterWithLogging(IResolver resolver)
    {
        m_resolver = resolver;
    }

    public void ReadAndExecute(string filePath)
    {
        Console.WriteLine($"Processing file: {filePath}");
        var documentServer = m_resolver.Resolve<IDocumentServer>();
        var document = documentServer.Prepare(filePath);
        document.Initialize(filePath);

        var scriptCommandFileReader = m_resolver.Resolve<IScriptCommandFileReader>();
        scriptCommandFileReader.Read(document, filePath);
        Console.WriteLine($"Script read: {filePath}");

        var scriptCommandFile = scriptCommandFileReader.ScriptCommandFile;
        Console.WriteLine($"Read {scriptCommandFile.Lines.Count()} lines");
        Console.WriteLine($"Read {scriptCommandFile.Commands.Count()} commands");

        var scriptCommandExecutor = m_resolver.Resolve<IScriptCommandExecutor>();
        var commandExecuter = m_resolver.Resolve<ICommandExecuter>();

        scriptCommandFileReader.ResultMessageHandler += CommandFileReader_ResultMessage;
        scriptCommandFileReader.ErrorMessageHandler += CommandFileReader_ErrorMessage;

        scriptCommandExecutor.Execute(commandExecuter, scriptCommandFile, scriptCommandFileReader);

        scriptCommandFileReader.ResultMessageHandler -= CommandFileReader_ResultMessage;
        scriptCommandFileReader.ErrorMessageHandler -= CommandFileReader_ErrorMessage;
        Console.WriteLine("Script execution completed.");
    }

    private void CommandFileReader_ErrorMessage(IMessage message)
    {
        Console.WriteLine($@"{Environment.NewLine}Error: {message.Text}{Environment.NewLine}");
    }

    private void CommandFileReader_ResultMessage(IMessage message)
    {
        Console.WriteLine($@"{message.Text}{Environment.NewLine}");
    }
}
