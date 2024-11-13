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
        
        
     
        app.MapGet("/parcel/", async (string id, IParcelRepository catRepo) =>
        {
            return await catRepo.Get(id);
        });
        
        app.MapGet("/parcels/", async (IParcelRepository catRepo) => await catRepo.GetAll());
        
        
        app.MapPost("/parcel/", async (Parcel parcel, IParcelRepository catRepo) =>
        {
            await catRepo.Add(parcel);
        });
        
        app.Run();
    }    
}