using Geolocation;
using SailingBoatPathfinder.Logic.Models;

namespace SailingBoatPathfinder.Logic.Services;

public class CoordinateProviderService
{
    private readonly int _decimalPrecision;
    private readonly int _directionResolutionLevel;
    private readonly double _nodeDivergence;

    // irányok száma = 8 * directionResolutionLevel
    // decimalPrecision => fordítottan arányos a csúcsok közti távolsággal
    public CoordinateProviderService(int decimalPrecision = 4, int directionResolutionLevel = 3)
    {
        _decimalPrecision = decimalPrecision;
        _directionResolutionLevel = directionResolutionLevel;
        _nodeDivergence = Math.Round(Math.Pow(0.1, decimalPrecision), decimalPrecision);
    }

    public List<Coordinate> GetNeighbours(Coordinate origin)
    {
        List<Coordinate> neighbours = new List<Coordinate>();

        for (int i = 0; i < _directionResolutionLevel * 2; i++)
        {
            neighbours.Add(
                RoundCoordinate(
                    new Coordinate(
                        origin.Latitude + _nodeDivergence * _directionResolutionLevel,
                        origin.Longitude + _nodeDivergence * (i - _directionResolutionLevel)
                    )
                )
            );
        }

        for (int i = 0; i < _directionResolutionLevel * 2; i++)
        {
            neighbours.Add(
                RoundCoordinate(
                    new Coordinate(
                        origin.Latitude - _nodeDivergence * (i - _directionResolutionLevel),
                        origin.Longitude + _nodeDivergence * _directionResolutionLevel
                    )
                )
            );
        }

        for (int i = 0; i < _directionResolutionLevel * 2; i++)
        {
            neighbours.Add(
                RoundCoordinate(
                    new Coordinate(
                        origin.Latitude - _nodeDivergence * _directionResolutionLevel,
                        origin.Longitude - _nodeDivergence * (i - _directionResolutionLevel)
                    )
                )
            );
        }

        for (int i = 0; i < _directionResolutionLevel * 2; i++)
        {
            neighbours.Add(
                RoundCoordinate(
                    new Coordinate(
                        origin.Latitude + _nodeDivergence * (i - _directionResolutionLevel),
                        origin.Longitude - _nodeDivergence * _directionResolutionLevel
                    )
                )
            );
        }

        return neighbours;
    }

    public Coordinate RoundCoordinate(Coordinate coordinate)
    {
        coordinate.Latitude = Math.Round(coordinate.Latitude, _decimalPrecision);
        coordinate.Longitude = Math.Round(coordinate.Longitude, _decimalPrecision);
        return coordinate;
    }
}