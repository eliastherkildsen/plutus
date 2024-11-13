using plutus.Entity;

namespace plutus.IRepository;

public interface IParcelRepository
{
    Task<Parcel> Add(Parcel parcel);
    Task<Parcel> Get(string id);
    Task<List<Parcel>> GetAll();
    Task<Parcel> Update(string id, Parcel parcel);
}