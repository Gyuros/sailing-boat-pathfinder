namespace SailingBoatPathfinder.Data.Models
{
    public class Boat
    {
        public string Type { get; set; } = string.Empty;

        public IEnumerable<PolarData> PolarData { get; set; } = Enumerable.Empty<PolarData>();

        public IEnumerable<BoatProperty> Properties { get; set; } = Enumerable.Empty<BoatProperty>();
    }
}
