using SailingBoatPathfinder.Data.Models;
using System.Text.Json;

namespace SailingBoatPathfinder.Data.Services;

public class BoatLoaderService
{
    private const string FileName = "sailing-boats.json";

    public async Task<IEnumerable<SailingBoat>> ReadFromFileAsync(CancellationToken cancellationToken)
    {
        await using FileStream jsonStream = File.OpenRead(FileName);
        IEnumerable<SailingBoat>? boats = await JsonSerializer.DeserializeAsync<IEnumerable<SailingBoat>>(jsonStream, JsonSerializerOptions.Default, cancellationToken);
        return boats ?? Enumerable.Empty<SailingBoat>();
    }
}