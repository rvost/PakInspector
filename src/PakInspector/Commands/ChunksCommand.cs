using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PakInspector.Commands;

internal class ChunksCommand : Command<ChunksCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to file.")]
        [CommandArgument(0, "<filePath>")]
        public string FilePath { get; init; }

        public override ValidationResult Validate()
        {
            return Path.Exists(FilePath)
                ? ValidationResult.Success()
                : ValidationResult.Error($"File {FilePath} does not exist");
        }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var file = Iff.FromFile(settings.FilePath);

        var table = new Table();
        table.Title($"Form type: {file.FormType}");
        table.AddColumn("TypeId");
        table.AddColumn("Length");

        table.Border(TableBorder.Square);
        foreach (var chunk in file.Chunks)
        {
            table.AddRow(chunk.TypeId, chunk.Length.ToString());
        }
        AnsiConsole.Write(table);
        return 0;
    }
}
