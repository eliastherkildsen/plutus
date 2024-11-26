using plutus.Entity;

namespace plutus.IRepository;

public interface IParcelRepository
{
    Task Add(Parcel parcel);
    Task<Parcel> Get(string id);
    Task<List<Parcel>> GetAll();
    Task Update(string id, Parcel parcel);
    // test
}