using LiteDB;

namespace Levenshteiner.DataModel;

#nullable disable
public record WordToNodeModel
{
    public string NodeId { get; set; }
    public string BaseWord { get; set; }

    public WordToNodeModel() {}
    public WordToNodeModel(ObjectId id, string baseWord)
    {
        NodeId = id.ToString();
        BaseWord = baseWord;
    }
}

public record NodeCollectionModel
{
    [BsonId]
    public ObjectId Id { get; set; }

    public IEnumerable<WordToNodeModel> Words { get; set; }
}
