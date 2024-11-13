using MongoDB.Bson.Serialization.Attributes;

namespace plutus.Entity;

public class Cat
{
    [BsonId] // tells mongodb that this is the primary key.
    public string Id { get; set; }
    public string? Name { get; set; }
    public string? Breed { get; set; }
}