using LiteDB;

namespace Levenshteiner.DataModel;

#nullable disable

public record NodeSummaryInfo
{
    [BsonId]
    public ObjectId Id { get; set; }

    public WordToNodeModel[] WordToNodes { get; set; }
}
