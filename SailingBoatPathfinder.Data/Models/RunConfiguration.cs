using Geolocation;
using Newtonsoft.Json;

namespace SailingBoatPathfinder.Data.Models;

public class RunConfiguration
{
    public string BoatType { get; set; }
    public string BoatsFileName { get; set; }
    public string WindMapFileName { get; set; }
    public List<Coordinate> Coordinates { get; set; }
    public DateTime DateTime { get; set; } = DateTime.Now;

    [JsonIgnore]
    public SailingBoat Boat { get; set; }
    [JsonIgnore]
    public WindMap WindMap { get; set; }
}