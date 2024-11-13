using System.Numerics;
using MongoDB.Bson.Serialization.Attributes;

namespace plutus.Entity;

public class Parcel
{
    [BsonId] // tells mongodb that this is the primary key.
    public string Id { get; set; }
    public Vector2 Position { get; set; }
    public DateTime LastUpdate { get; set; }
}