using LiteDB;

using Levenshteiner.DataModel;
namespace Levenshteiner;

public class Levenshteiner : IDisposable
{
    private readonly string _dataPath;
    private readonly uint _nodeSize;

    private readonly LiteDatabase _nodesDb;
    private readonly ILiteCollection<NodeInfoModel> _nodesInfoDb;
    private readonly List<NodeInfoModel> _nodesInfo;
    private readonly Dictionary<string, ObjectId> _wordToNode;
    private readonly Dictionary<ObjectId, WordModel> _nodeToWords = new();

    public Levenshteiner(string dataPath, uint nodeSize)
    {
        _dataPath = dataPath;
        _nodeSize = nodeSize;

        string nodesDbPath = Path.Combine(dataPath, "nodes.data");
        _nodesDb = new(nodesDbPath);
        _nodesInfoDb = _nodesDb.GetCollection<NodeInfoModel>("nodes");
        var summaryCol = _nodesDb.GetCollection<NodeSummaryInfo>("summary");

        _nodesInfo = _nodesInfoDb.FindAll().ToList();

        var summary = summaryCol.FindAll().AsEnumerable().Single();
        _wordToNode = summary.WordToNodes.ToDictionary(wn => wn.BaseWord, wn => new ObjectId(wn.NodeId));
    }

    public IEnumerable<string> Find(IEnumerable<string> query, int maxEdits, int items = 1)
    {
        
    }

    private IEnumerable<string> FindWord(string word, int maxEdits, int items)
    {

    }

    public void Dispose()
    {
        _nodesDb.Dispose();
    }
}
