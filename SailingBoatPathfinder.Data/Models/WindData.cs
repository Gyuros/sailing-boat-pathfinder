using Geolocation;

namespace SailingBoatPathfinder.Data.Models;

public class WindData : Wind
{
    public Coordinate TopLeft { get; set; }
    public Coordinate BottomRight { get; set; }
    public DateTime? From { get; set; }
    public DateTime? To { get; set; }
}