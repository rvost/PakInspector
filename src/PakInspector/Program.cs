using PakInspector.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<ChunksCommand>("chunks");
    config.AddCommand<PakInspectCommand>("inspect");
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif
});

app.Run(args);
