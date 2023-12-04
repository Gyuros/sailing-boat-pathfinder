using SailingBoatPathfinder.Data.Entities;
using System.Text.Json;

namespace SailingBoatPathfinder.Data.Services;

public class BoatLoaderService
{
    private const string FileName = "boats.json";

    public async Task<IEnumerable<Boat>> ReadFromFileAsync(CancellationToken cancellationToken)
    {
        await using FileStream jsonStream = File.OpenRead(FileName);
        IEnumerable<Boat>? boats = await JsonSerializer.DeserializeAsync<IEnumerable<Boat>>(jsonStream, JsonSerializerOptions.Default, cancellationToken);
        return boats ?? Enumerable.Empty<Boat>();
    }
}