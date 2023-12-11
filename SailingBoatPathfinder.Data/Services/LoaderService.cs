using System.Text.Json;

namespace SailingBoatPathfinder.Data.Services;

public class LoaderService
{
    public async Task<T?> ReadFromFileAsync<T>(string fileName, CancellationToken cancellationToken)
    {
        await using FileStream jsonStream = File.OpenRead(fileName);
        T? obj = await JsonSerializer.DeserializeAsync<T>(jsonStream, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true}, cancellationToken);
        return obj;
    }
}