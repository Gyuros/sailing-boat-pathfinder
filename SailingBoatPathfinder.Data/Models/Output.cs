using Geolocation;

namespace SailingBoatPathfinder.Data.Models;

public class Output
{
    public Coordinate Coordinate { get; set; }
    public double TimeFromStart { get; set; }
}