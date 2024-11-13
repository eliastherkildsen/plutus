using System.Data;
using Microsoft.AspNetCore.Mvc;
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
        
        
        app.MapPost("/parcel/", async ([FromBody] Parcel parcel, IParcelRepository parcelRepository) =>
        {
            
            // setting the last updated time to now. 
            parcel.LastUpdate = DateTime.Now;
            
            try
            {
                await parcelRepository.Add(parcel);
            }

            catch (InvalidDataException ex)
            {
                return Results.BadRequest(new { Error = "InvalidData", Message = ex.Message });
            }
            catch (DuplicateNameException ex)
            {
                return Results.BadRequest(new { Error = "DuplicateName", Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "ArgumentException", Message = ex.Message });
            }

            // we can not ensure that the err messages is safe to displayed, therefor we don't. 
            catch (Exception err)
            {
                return Results.Problem(statusCode: 500, title: "An unexpected error occurred.");
            }
            
            return Results.Ok(parcel);
            
        });

        app.MapPut("/parcel/", async (string id, Parcel parcel, IParcelRepository parcelRepository) =>
        {
            
            // setting the last updated time to now, and id to the id passed in the header. This means that the id
            // can not be updated. 
            parcel.LastUpdate = DateTime.Now;
            parcel.Id = id;

            try
            {
                await parcelRepository.Update(id, parcel);
            }
            
            catch (InvalidDataException ex)
            {
                return Results.BadRequest(new { Error = "InvalidData", Message = ex.Message });
            }
            catch (DuplicateNameException ex)
            {
                return Results.BadRequest(new { Error = "DuplicateName", Message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(new { Error = "ArgumentException", Message = ex.Message });
            }

            // we can not ensure that the err messages is safe to displayed, therefor we don't. 
            catch (Exception err)
            {
                return Results.Problem(statusCode: 500, title: "An unexpected error occurred.");
            }
            
            return Results.Ok(parcel);
            
        });
        
        app.Run();
    }    
}