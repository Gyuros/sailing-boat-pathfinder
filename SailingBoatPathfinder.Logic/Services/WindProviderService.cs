using System.Numerics;
using Geolocation;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class WindProviderService
{
    public WindData GetWindData(Coordinate at, DateTime when)
    {
        return new WindData(20, 135);
    }
}