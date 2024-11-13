using plutus.Entity;

namespace plutus.Repository;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

internal interface ICatRepository
{
    Task Add(Cat cat);
    Task<Cat> Get(string id);
    Task<List<Cat>> GetAll();
}
public class CatRepository : ICatRepository
{
    private readonly IMongoCollection<Cat> _cats;

    public CatRepository(IOptions<CatDatabase> catDatabase)
    {
        var mc = new MongoClient(catDatabase.Value.ConnectionString);
        var mongoDB = mc.GetDatabase(catDatabase.Value.DatabaseName);
        _cats = mongoDB.GetCollection<Cat>(catDatabase.Value.CollectionName);
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
    public async Task Add(Cat cat)
    {
        await _cats.InsertOneAsync(cat);
    }

    public async Task<Cat> Get(string id)
    {
        return await _cats.Find(cat => cat.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<Cat>> GetAll()
    {
        return _cats.Find(cat => true).ToListAsync();
    }
}
