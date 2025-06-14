﻿using PakInspector.Data;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.Text.Json;

namespace PakInspector.Commands;

internal class PakInspectCommand : Command<PakInspectCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to file.")]
        [CommandArgument(0, "<filePath>")]
        public string FilePath { get; init; }

        [Description("Display files as tree")]
        [CommandOption("-t|--tree")]
        public bool ShowTree { get; init; }

        [Description("Display all file info")]
        [CommandOption("-a|--all")]
        public bool ShowAllInfo { get; init; }

        [Description("Save inspection results")]
        [CommandOption("-s|--save")]
        public bool Save { get; init; }

        [Description("Do not print to the console")]
        [CommandOption("-q|--quiet")]
        public bool Quiet { get; init; }


        public override ValidationResult Validate()
        {
            return Path.Exists(FilePath)
                ? ValidationResult.Success()
                : ValidationResult.Error($"File {FilePath} does not exist");
        }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var file = AnsiConsole.Status().Start("Parsing .pak...", ctx => Pak.FromFile(settings.FilePath));

        var name = Path.GetFileNameWithoutExtension(settings.FilePath);

        if (file.Chunks.First(c => c.TypeId == Pak.Chunk.ChunkType.Head).Body is not Pak.HeadChunk headChunk)
        {
            throw new Exception("Failed to parse head chunk");
        }

        var headContent = Convert.ToBase64String(headChunk.Header);
        if (!settings.Quiet)
        {
            AnsiConsole.Write(new Markup($"Pak header:\t[orange1]{headContent}[/]\n\n"));
        }

        if (file.Chunks.First(c => c.TypeId == Pak.Chunk.ChunkType.File).Body is not Pak.FileChunk fileChunk)
        {
            throw new Exception("Failed to parse file chunk");
        }

        var files = AnsiConsole.Status().Start("Parsing file tree...", ctx => PakUtils.GetFiles(fileChunk.Root, "").ToList());
        AnsiConsole.Write(new Markup($"Pak contains [orange1]{files.Count}[/] file(s)\n\n"));

        if (!settings.Quiet)
        {
            if (settings.ShowTree)
            {
                DisplayFileTree(name, fileChunk);
            }
            else
            {
                DisplayFileList(files, settings.ShowAllInfo);
            }
        }

        if (settings.Save)
        {
            AnsiConsole.Status().Start("Saving inspection results...", ctx => SaveReport(name, new(headContent, files)));
        }

        return 0;
    }

    private static void DisplayFileList(IEnumerable<PakFileEntry> files, bool showAllInfo)
    {
        foreach (var f in files)
        {
            var info = showAllInfo ? f.GetInfo() : f.GetShortInfo();
            AnsiConsole.Write(info);
            AnsiConsole.WriteLine();
        }
    }

    private static void DisplayFileTree(string name, Pak.FileChunk fileChunk)
    {
        if (fileChunk.Root.Info is Pak.PakEntry.PakFolderInfo root)
        {
            var tree = new Tree(name);
            foreach (var child in root.Children)
            {
                BuildFileTree(child, tree);
            }
            AnsiConsole.Write(tree);
        }
    }

    private static void BuildFileTree(Pak.PakEntry entry, IHasTreeNodes parent)
    {
        var node = new TreeNode(new Markup(entry.Name));
        var info = entry.Info;
        switch (info)
        {
            case Pak.PakEntry.PakFileInfo:
                break;
            case Pak.PakEntry.PakFolderInfo folder:
                foreach (var child in folder.Children)
                {
                    BuildFileTree(child, node);
                }
                break;
            default:
                break;
        }
        parent.AddNode(node);
    }

    private static void SaveReport(string name, PakReport report)
    {
        using var output = File.Create($"{name}.json");
        var options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        JsonSerializer.Serialize(output, report, options);
    }

}
