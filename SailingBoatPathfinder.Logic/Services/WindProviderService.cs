using System.Numerics;
using Geolocation;
using SailingBoatPathfinder.Data.Models;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class WindProviderService
{
    private readonly WindMap _windMap;

    public WindProviderService(WindMap runConfigurationWindMap)
    {
        _windMap = runConfigurationWindMap;
    }

    public WindDto GetWindData(Coordinate at, DateTime when)
    {
        List<WindData>? windData = _windMap.WindData?.Where(data => InsideArea(data.TopLeft, data.BottomRight, at)).ToList();
        WindData? currentWindData = windData?.FirstOrDefault(windData => (windData.From ?? DateTime.MinValue) <= when && (windData.To ?? DateTime.MaxValue) >= when);
        
        // if (currentWindData != null && currentWindData == windData?.LastOrDefault())
        // {
        //     ;
        // }

        if (when >= DateTime.Parse("2024-01-01T00:02:00"))
        {
            ;
        }

        if (currentWindData == null)
        {
            
            WindData? closest = windData?.Where(data => (data.From ?? DateTime.MinValue) >= when).MinBy(data => (data.From ?? DateTime.MinValue));

            if (closest != null && closest.From != null)
            {
                double delay = closest.From?.Subtract(when).TotalSeconds ?? 0;
                return new WindDto(closest.WindVelocityKts, closest.WindBearing) { Delay = delay };
            }
            
            return new WindDto(_windMap.Default.WindVelocityKts, _windMap.Default.WindBearing);
        }

        return new WindDto(currentWindData.WindVelocityKts, currentWindData.WindBearing);
    }

    private bool InsideArea(Coordinate topLeft, Coordinate bottomRight, Coordinate point)
    {
        return point.Latitude <= topLeft.Latitude &&
               point.Latitude >= bottomRight.Latitude &&
               point.Longitude <= bottomRight.Longitude &&
               point.Longitude >= topLeft.Longitude;
    }
}