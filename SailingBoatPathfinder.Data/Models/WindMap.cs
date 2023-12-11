namespace SailingBoatPathfinder.Data.Models;

public class WindMap
{
    public Wind Default { get; set; }
    public List<WindData>? WindData { get; set; } = new List<WindData>();
}