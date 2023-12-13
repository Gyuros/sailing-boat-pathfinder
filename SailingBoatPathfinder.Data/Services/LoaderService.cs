using System.Text.Json;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace SailingBoatPathfinder.Data.Services;

public class LoaderService
{
    public async Task<T?> ReadFromFileAsync<T>(string fileName, CancellationToken cancellationToken)
    {
        await using FileStream jsonStream = File.OpenRead(fileName);
        T? obj = await JsonSerializer.DeserializeAsync<T>(jsonStream, new JsonSerializerOptions(){PropertyNameCaseInsensitive = true}, cancellationToken);
        return obj;
    }

    public async Task WriteOutput<T>(T output) where T : new()
    {
        string outputJson = JsonConvert.SerializeObject(output);
        await File.WriteAllTextAsync("output.json", outputJson);
    }
}