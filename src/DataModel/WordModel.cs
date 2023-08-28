using LiteDB;

namespace Levenshteiner.DataModel;

#nullable disable
public record WordModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Word { get; set; }
    public int BaseDistance { get; set; } = 0;
    public IEnumerable<string> References { get; set; }
}
