// See https://aka.ms/new-console-template for more information

using Geolocation;
using SailingBoatPathfinder.Logic.Services;

double bearing = 90;
double windFrom = 270;
double windAngle = Math.Abs(bearing - windFrom);
windAngle = 180 - Math.Abs(180 - windAngle);
Console.WriteLine(windAngle);