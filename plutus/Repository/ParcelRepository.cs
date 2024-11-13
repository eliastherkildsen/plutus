using plutus.Entity;
using plutus.IRepository;

namespace plutus.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;


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
    public async Task Update(string id, Parcel parcel)
    {
        await _parcel.FindOneAndReplaceAsync(p => p.Id == id, parcel);
    }
}
