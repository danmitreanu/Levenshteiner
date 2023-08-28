using LiteDB;
using Fastenshtein;
using Levenshteiner.DataModel;
using System.Data.Common;

namespace Levenshteiner;

public sealed class DatasetBuilderOptions
{
    private string _splitChars = string.Empty;
    public string SplitCharacters
    {
        get => _splitChars;
        set => _splitChars = value + " ";
    }
}

public sealed class DatasetBuilder
{
    private readonly DatasetBuilderOptions _opts;
    private readonly ushort _nodeSize = 100;
    private readonly Dictionary<string, List<string>> _wordsToReferences = new();

    public DatasetBuilder(DatasetBuilderOptions opts, ushort nodeSize)
    {
        _opts = opts;
        _nodeSize = nodeSize;
    }

    public void AddPhrase(string phrase, string reference)
    {
        var splitChars = _opts.SplitCharacters.ToCharArray();
        string[] words = phrase.Split(splitChars, StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        foreach (string rawWord in words)
        {
            string word = rawWord.ToLower();

            if (!_wordsToReferences.ContainsKey(word))
            {
                _wordsToReferences.Add(word, new() { reference });
                continue;
            }

            _wordsToReferences[word].Add(reference);
        }
    }

    public void Build(string outPath)
    {
        using LiteDatabase infoDb = new(Path.Combine(outPath, "nodes.data"));
        var infoCol = infoDb.GetCollection<NodeInfoModel>("nodes");
        var summaryCol = infoDb.GetCollection<NodeSummaryInfo>("summary");

        List<WordToNodeModel> nodeSummaries = new();

        var allWords = _wordsToReferences.Keys.ToList();
        int nodeNo = 0;
        while (allWords.Count != 0)
        {
            string baseWord = allWords[0];
            Levenshtein leven = new(baseWord);

            var startTime = DateTime.UtcNow;
            var distanceSort = allWords
                .Select(w => new
                {
                    word = w,
                    distance = leven.DistanceFrom(w)
                })
                .OrderBy(o => o.distance)
                .Take(_nodeSize)
                .Select(o => new WordModel
                {
                    Word = o.word,
                    BaseDistance = o.distance,
                    References = _wordsToReferences[o.word]
                });

            foreach (var word in distanceSort)
                allWords.RemoveAt(allWords.IndexOf(word.Word));

            string nodeName = $"node{nodeNo++}";
            using LiteDatabase db = new(Path.Combine(outPath, nodeName));
            var col = db.GetCollection<WordModel>("words");
            col.InsertBulk(distanceSort);

            var endTime = DateTime.UtcNow;
            var nodeBuildDuration = endTime - startTime;

            Console.WriteLine($"Node {nodeName} build duration: {nodeBuildDuration.TotalMilliseconds}ms");

            NodeInfoModel info = new()
            {
                NodeName = nodeName,
                NodeBaseWord = baseWord,
                Size = distanceSort.Count(),
                BaseDistances = distanceSort.Select(w => w.BaseDistance).ToArray()
            };

            infoCol.Insert(info);
            nodeSummaries.Add(new(info.Id, baseWord));
        }

        NodeSummaryInfo summary = new()
        {
            WordToNodes = nodeSummaries.ToArray()
        };

        summaryCol.DeleteAll();
        summaryCol.Insert(summary);
    }
}
