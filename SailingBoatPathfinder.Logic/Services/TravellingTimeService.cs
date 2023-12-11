using Geolocation;
using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class TravellingTimeService
{
    private readonly WindProviderService _windProviderService;
    private readonly PolarDiagramService _polarDiagramService;

    public TravellingTimeService(WindProviderService windProviderService, PolarDiagramService polarDiagramService)
    {
        _windProviderService = windProviderService;
        _polarDiagramService = polarDiagramService;
    }

    public double TimeToTravel(Coordinate from, Coordinate to, DateTime timeOfTravel, SailingBoat boat)
    {
        WindDto wind = _windProviderService.GetWindData(from, timeOfTravel);
        
        double bearing = GeoCalculator.GetBearing(from, to);
        
        double relativeWindAngle = 180 - Math.Abs(180 - Math.Abs(bearing - wind.AngleFrom));
        
        double boatSpeedMps = _polarDiagramService.GetBoatSpeed(relativeWindAngle, wind.Velocity, boat);
        
        double distance = GeoCalculator.GetDistance(from, to, 4, DistanceUnit.Meters);

        if (boatSpeedMps == 0)
        {
            return double.PositiveInfinity;
        }
        
        return distance / boatSpeedMps + wind.Delay;
    }
}