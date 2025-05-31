using System.Text.Json.Serialization;

namespace PakInspector.Data;

internal record PakReport(string Head, List<PakFileEntry> Files)
{
    [JsonPropertyOrder(-1)]
    public int FilesCount { get; } = Files.Count;
}
