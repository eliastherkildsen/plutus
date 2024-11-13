using plutus.Entity;

namespace plutus.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

internal interface IParcelRepository
{
    Task Add(Parcel parcel);
    Task<Parcel> Get(string id);
    Task<List<Parcel>> GetAll();
}
public class ParcelRepository : IParcelRepository
{
    private readonly IMongoCollection<Parcel> _parcel;

    public ParcelRepository(IOptions<ParcelDatabase> parcelDatabase)
    {
        var mc = new MongoClient(parcelDatabase.Value.ConnectionString);
        var mongoDB = mc.GetDatabase(parcelDatabase.Value.DatabaseName);
        _parcel = mongoDB.GetCollection<Parcel>(parcelDatabase.Value.CollectionName);
    }

    // Test connection to MongoDB
    public async Task<bool> TestMongoConnectionAsync()
    {
        try
        {
            // Attempt to list the databases as a simple connectivity test
            var client = new MongoClient();
            var databases = await client.ListDatabaseNamesAsync();
            return true; // Connection is successful
        }
        catch (Exception)
        {
            return false; // Connection failed
        }
    }

    // Other repository methods (Add, Get, GetAll)
    public async Task Add(Parcel parcel)
    {
        await _parcel.InsertOneAsync(parcel);
    }

    public async Task<Parcel> Get(string id)
    {
        return await _parcel.Find(parcel => parcel.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<Parcel>> GetAll()
    {
        return _parcel.Find(parcel => true).ToListAsync();
    }
}
