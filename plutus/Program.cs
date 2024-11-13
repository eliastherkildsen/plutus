using plutus.Entity;
using plutus.IRepository;
using plutus.Repository;

namespace plutus;

public class Program
{
    public static void Main(string[] args)
    {
        
        var builder = WebApplication.CreateBuilder(args);
        //builder.WebHost.ConfigureKestrel(options => options.ListenLocalhost(5000));

        builder.Services.AddAuthentication();
        
        // Add services to the container.
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        builder.Services.Configure<ParcelDatabase>(
            builder.Configuration.GetSection("DatabaseSettings"));
        builder.Services.AddSingleton<IParcelRepository, ParcelRepository>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        // This endpoint is simply implemented as a way of testing where or not a connection can be established.
        // the endpoint is not dependent on anything other than the core API itself.  
        app.MapGet("/", () => "Hello world from the core API");

     
        app.MapGet("/parcel/", async (string id, IParcelRepository parcelRepository) =>
        {
            return await parcelRepository.Get(id);
        });
        
        app.MapGet("/parcels/", async (IParcelRepository parcelRepository) => await parcelRepository.GetAll());
        
        
        app.MapPost("/parcel/", async (Parcel parcel, IParcelRepository parcelRepository) =>
        {
            await parcelRepository.Add(parcel);
        });
        
        app.MapPut("/parcel/", async (string id, Parcel parcel, IParcelRepository parcelRepository) => 
            await parcelRepository.Update(id, parcel));
        
        app.Run();
    }    
}