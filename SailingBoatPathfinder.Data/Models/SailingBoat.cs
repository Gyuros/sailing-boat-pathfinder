namespace SailingBoatPathfinder.Data.Models;

public class SailingBoat
{
    public string Type { get; set; } = string.Empty;

    // TODO: rendezve visszaadni
    public List<SailingBoatPolarData> PolarData { get; set; } = new List<SailingBoatPolarData>();
}