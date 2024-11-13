namespace plutus.Entity;

public static class ParcelValidator
{
    // Compiled Regex - initialized once for better performance.
    private static readonly System.Text.RegularExpressions.Regex AlphanumericRegex = 
        new System.Text.RegularExpressions.Regex(@"^[a-zA-Z0-9]*$", System.Text.RegularExpressions.RegexOptions.Compiled);

    private static bool IsParcelIdValid(string parcelId)
    {
        // Check if the Parcel ID is null or empty
        if (string.IsNullOrEmpty(parcelId)) return false;
        
        // Validate against the regex (only alphanumeric characters)
        return AlphanumericRegex.IsMatch(parcelId);
    }

    private static bool IsWorldCoordinateValid(double longitude, double latitude)
    {
        
        //Console.WriteLine($"Longitude: {longitude}, Latitude: {latitude}");
        
        // Longitude: -180 to 180, Latitude: -90 to 90
        return longitude >= -180 && longitude <= 180 && latitude >= -90 && latitude <= 90;
    }
    
    public static void ValidateParcel(Parcel parcel)
    {
        
        //Console.WriteLine($"Parcel ID: {parcel.Id}");
        
        if (!ParcelValidator.IsParcelIdValid(parcel.Id))
        {
            throw new ArgumentException($"Parcel id does not adhere to the naming convention: {parcel.Id}");
        }
    
        if (!ParcelValidator.IsWorldCoordinateValid(parcel.Longitude, parcel.Latitude))
        {
            throw new ArgumentException($"Parcel longitude or latitude are invalid:" +
                                        $" Longitude = {parcel.Longitude}, Latitude = {parcel.Latitude}" + 
                                        "Longitude must be between -180 and 180 and +180 " 
                                        + "Latitude must be between -90 and 90");
        }
    }
    
    
    
}
