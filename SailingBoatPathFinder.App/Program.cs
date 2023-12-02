// See https://aka.ms/new-console-template for more information

using SailingBoatPathfinder.Data.Services;

var loader = new BoatLoaderService();
var boats = loader.ReadFromFileAsync(CancellationToken.None).Result;
Console.WriteLine("Hello, World!");