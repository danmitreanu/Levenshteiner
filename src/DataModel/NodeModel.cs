using LiteDB;

namespace Levenshteiner.DataModel;

#nullable disable

public record NodeModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string NodeId { get; set; }
    public string NodeAverage { get; set; }
    public int Size { get; set; } = 0;
}
