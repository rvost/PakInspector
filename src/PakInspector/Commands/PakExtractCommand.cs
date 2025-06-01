using PakInspector.Data;
using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using System.IO.Compression;

namespace PakInspector.Commands;

internal class PakExtractCommand : Command<PakExtractCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to file.")]
        [CommandArgument(0, "<filePath>")]
        public string FilePath { get; init; }

        [Description("Path to output directory.")]
        [CommandArgument(1, "[outputDir]")]
        public string? OutputPath { get; init; }

        [Description("Files to extract. Use paths as specified in inspection results.")]
        [CommandOption("-f|--file <VALUES>")]
        public string[]? Files { get; init; }

        [Description("Extract files without processing.")]
        [CommandOption("-r|--raw")]
        public bool CopyRaw { get; init; }
        public override ValidationResult Validate()
        {
            return Path.Exists(FilePath)
                ? ValidationResult.Success()
                : ValidationResult.Error($"File {FilePath} does not exist");
        }
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        var pak = Pak.FromFile(settings.FilePath);

        var fileName = Path.GetFileNameWithoutExtension(settings.FilePath);
        var outputDir = string.IsNullOrEmpty(settings.OutputPath) ? fileName : settings.OutputPath;

        if (pak.Chunks.First(c => c.TypeId == Pak.Chunk.ChunkType.File).Body is not Pak.FileChunk fileChunk)
        {
            throw new Exception("Failed to parse file chunk");
        }

        var files = PakUtils.GetFiles(fileChunk.Root, "").ToDictionary(f => f.Path);
        List<PakFileEntry> filesToExtract = settings.Files is not null
            ? [.. settings.Files
                .Where(f => files.ContainsKey(f))
                .Select(f => files[f])]
            : [.. files.Values];

        ExtractFiles(outputDir, filesToExtract, settings.CopyRaw);

        return 0;
    }

    private static void ExtractFiles(string outputDir, List<PakFileEntry> files, bool copyRaw)
    {
        var progress = AnsiConsole.Progress()
                    .HideCompleted(false)
                    .Columns([
                        new TaskDescriptionColumn(),
                        new ProgressBarColumn(),
                        new PercentageColumn(),
                        new SpinnerColumn()
                    ]);

        progress.Start(ctx =>
        {
            var task = ctx.AddTask("Extracting Files", maxValue: files.Count);

            foreach (var file in files)
            {
                ExtractFile(outputDir, file, copyRaw);
                task.Increment(1);
            }

        });
    }

    private static void ExtractFile(string outputDir, PakFileEntry file, bool copyRaw)
    {
        var folder = Path.GetDirectoryName(file.Path);
        var fileName = Path.GetFileName(file.Path);
        var fullPath = Path.Combine(outputDir, folder!);

        Directory.CreateDirectory(fullPath);
        using var output = File.Create(Path.Combine(fullPath, fileName));

        if (copyRaw)
        {
            WriteUncompressedFile(output, file);
        }
        else
        {
            switch (file.CompressionType)
            {
                case 0:
                    WriteUncompressedFile(output, file);
                    break;
                case 0x106: //  File is compressed using the DEFLATE algorithm
                    WriteZLibCompressedFile(output, file);
                    break;
                default:
                    throw new Exception($"Unknown compression type: ${file.CompressionType} in file ${file.Path}");
            }
        }
    }

    private static void WriteUncompressedFile(FileStream output, PakFileEntry file)
    {
        using var input = new MemoryStream(file.Data);
        input.CopyTo(output);
    }

    private static void WriteZLibCompressedFile(FileStream output, PakFileEntry file)
    {
        using var compressed = new MemoryStream(file.Data[2..]); // Skip zlib header
        using var deflate = new DeflateStream(compressed, CompressionMode.Decompress);
        deflate.CopyTo(output);
    }

}