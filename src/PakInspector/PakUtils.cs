using PakInspector.Data;

namespace PakInspector;

internal static class PakUtils
{
    public static IEnumerable<PakFileEntry> GetFiles(Pak.PakEntry entry, string basePath)
    {
        var currentPath = Path.Combine(basePath, entry.Name);
        var info = entry.Info;
        switch (info)
        {
            case Pak.PakEntry.PakFileInfo file:
                yield return new PakFileEntry(currentPath, file);
                break;
            case Pak.PakEntry.PakFolderInfo folder:
                foreach (var child in folder.Children)
                {
                    foreach (var file in GetFiles(child, currentPath))
                    {
                        yield return file;
                    }
                }
                break;
        }
    }

}