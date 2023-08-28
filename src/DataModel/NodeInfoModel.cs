using LiteDB;

namespace Levenshteiner.DataModel;

#nullable disable

public record NodeInfoModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string NodeName { get; set; }
    public string NodeBaseWord { get; set; }
    public int Size { get; set; } = 0;
    public int[] BaseDistances { get; set; }
}
