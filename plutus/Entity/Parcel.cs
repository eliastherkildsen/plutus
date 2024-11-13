using System.ComponentModel.DataAnnotations;
using MongoDB.Bson.Serialization.Attributes;

namespace plutus.Entity;

public class Parcel
{
    [BsonId] // tells mongodb that this is the primary key.
    public string Id { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public DateTime LastUpdate { get; set; }
}

