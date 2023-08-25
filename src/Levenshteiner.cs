using LiteDB;

using Levenshteiner.DataModel;
namespace Levenshteiner;

public class Levenshteiner
{
    private readonly string _dataPath;
    private readonly uint _nodeSize;

    private readonly ILiteCollection<NodeModel> _nodesInfoDb;
    private readonly List<NodeModel> _nodesInfo;

    public Levenshteiner(string dataPath, uint nodeSize)
    {
        _dataPath = dataPath;
        _nodeSize = nodeSize;

        string nodesDbPath = Path.Combine(dataPath, "_nodes.data");
        LiteDatabase nodesDb = new(nodesDbPath);
        _nodesInfoDb = nodesDb.GetCollection<NodeModel>("nodes");

        _nodesInfo = _nodesInfoDb.FindAll().ToList();
    }
}
