using Spectre.Console;
using Spectre.Console.Rendering;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PakInspector.Data;

internal record PakFileEntry
{

    public string Path { get; init; }
    public uint Offset { get; init; }
    public uint CompressedLength { get; init; }
    public uint OriginalLength { get; init; }
    [JsonConverter(typeof(BytesToHexJsonConverter))]
    public byte[] Unknown1 { get; init; }
    public uint CompressionType { get; init; }
    [JsonConverter(typeof(BytesToHexJsonConverter))]
    public byte[] Unknown2 { get; init; }
    [JsonIgnore]
    public Lazy<byte[]> Data { get; init; }

    public PakFileEntry(string path, Pak.PakEntry.PakFileInfo info)
    {
        Path = path;
        Offset = info.Offset;
        CompressedLength = info.CompressedLength;
        OriginalLength = info.OriginalLength;
        Unknown1 = info.Unknown1;
        CompressionType = info.CompressionType;
        Unknown2 = info.Unknown2;
        Data = new(() => info.Data);
    }

    public IRenderable GetShortInfo() => new TextPath(Path)
                    .SeparatorColor(Color.Red)
                    .StemColor(Color.Orange1)
                    .LeafColor(Color.Blue);

    public IRenderable GetInfo()
    {
        var table = new Table().AddColumns("Prop", "Value");
        table.Title = new(Path);
        table.AddRow("offset", Offset.ToString());
        table.AddRow("compressed", CompressedLength.ToString());
        table.AddRow("raw", OriginalLength.ToString());
        table.AddRow("unkn1", BitConverter.ToString(Unknown1));
        table.AddRow("compression", CompressionType.ToString());
        table.AddRow("unkn2", BitConverter.ToString(Unknown2));
        table.Expand();
        return table;
    }
}

internal class BytesToHexJsonConverter : JsonConverter<byte[]>
{
    public override byte[]? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, byte[] value, JsonSerializerOptions options)
    {
        JsonSerializer.Serialize(writer, BitConverter.ToString(value).Replace("-", ""), options);
    }
}
