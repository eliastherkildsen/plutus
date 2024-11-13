using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using plutus.Repository;

namespace plutus.Entity;

public class Parcel
{
    [BsonId] // tells mongodb that this is the primary key.
    private ObjectId _id;

    public string Id
    {
        get{return _id.ToString();}
        set
        {
            if(IsParcelIdValid(value))
            {
                _id = new ObjectId(value);
            }
            else
            {
                throw new InvalidDataException($"Parcel id does not adhere to the naming convention: {value}");
            }
        }
    }

    private double _latitude;
    public double Latitude
    {
        get { return _latitude; }
        set
        {
            if(IsWorldCordinateValid(value))
            {
                _latitude = value;
            }
            else
            {
                throw new InvalidDataException($"The latitude value is invalid. {value}");
            }
        }
    }

    private double _longitude;
    public double Longitude
    {
        get { return _longitude; }
        set
        {
            if (IsWorldCordinateValid(value))
            {
                _longitude = value;
            }
            else
            {
                throw new InvalidDataException($"The longitude value is invalid. {value}");
            }
        }
    } 
    
    private DateTime _LastUpdate;
    private object _parcel;

    public DateTime LastUpdate
    {
        get { return _LastUpdate; }
        set { _LastUpdate = value; }
    }
    
    private bool IsParcelIdValid(string parcelId)
    {
        // checking if the passed parcel id is empty. 
        if (string.IsNullOrEmpty(parcelId)) return false;
        // ensuring that the parcel id does not contain any special chars. 
        return MyRegex().IsMatch(parcelId);
    }

    private bool IsWorldCordinateValid(double worldCordinate)
    {
        // checks if coordinate is negative. 
        if (worldCordinate < 0) return false;
        return true;
    }
    private static System.Text.RegularExpressions.Regex MyRegex()
    {
        return new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$");
    }
    
}