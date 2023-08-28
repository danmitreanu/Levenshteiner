using Levenshteiner;

string? inputFile;
string? outDir;

if (args.Length != 2 || string.IsNullOrWhiteSpace(inputFile = args[0]) || string.IsNullOrWhiteSpace(outDir = args[1]))
{
    Console.WriteLine("Bad args");
    return 1;
}

string inFileContent = File.ReadAllText(inputFile);
string[] lines = inFileContent.Split('\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

DatasetBuilderOptions opts = new()
{
    SplitCharacters = "-"
};

DatasetBuilder builder = new(opts, 50);

foreach (string line in lines.Skip(1))
{
    string pattern = @"""\s*,\s*""";
    string[] tokens = System.Text.RegularExpressions.Regex.Split(
        line.Substring(1, line.Length - 2), pattern);

    string cityAscii = tokens[1];
    foreach (var token in tokens)
        builder.AddPhrase(token, cityAscii);
}

builder.Build(outDir);

return 0;
