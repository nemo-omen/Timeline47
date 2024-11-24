using System.Text.Json;

namespace Timeline47.Shared;

public static class SeedDataHelper
{
    public static string GetSeedDataPath(string fileName)
    {
        var baseDirectory = AppContext.BaseDirectory;
        return Path.Combine(baseDirectory, "SeedData", fileName);
    }
    
    public static List<T> LoadSeedData<T>(string filePath)
    {
        string? jsonData = null;
        try
        {
            jsonData = File.ReadAllText(filePath);
        }
        catch (Exception e)
        {
            throw new Exception($"Error reading news source data: {filePath}", e);
        }
        
        if(jsonData is null)
        {
            throw new Exception($"News source data is empty: {filePath}");
        }
        
        var data = JsonSerializer.Deserialize<List<T>>(jsonData, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        if (data is null)
        {
            throw new Exception($"News source data is empty or invalid: {filePath}");
        }

        return data;
    }
}