using Geolocation;

namespace SailingBoatPathfinder.Logic.Models
{
    public class BoatPosition
    {
        public BoatPosition(BoatPosition? from, double timeFromStart, double estimatedTimeToFinish, Coordinate coordinate)
        {
            From = from;
            TimeFromStart = timeFromStart;
            EstimatedTimeToFinish = estimatedTimeToFinish;
            Coordinate = coordinate;
        }

        public BoatPosition? From { get; set; }
        
        public Coordinate Coordinate { get; set; }
        
        public double TimeFromStart { get; set; }
        
        public double EstimatedTimeToFinish { get; set; }

        public double TimeFromStartToFinish => TimeFromStart + EstimatedTimeToFinish;
    }
}
