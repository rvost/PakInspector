// See https://aka.ms/new-console-template for more information
using PakInspector;

var file = Iff.FromFile(args[0]);

Console.WriteLine(file.FormType);
Console.WriteLine(file.Chunks.Count);
