using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Geolocation;
using SailingBoatPathfinder.Data.Services;
using SailingBoatPathfinder.Logic.Models;
using SailingBoatPathfinder.Logic.Services;

namespace SailingBoatPathfinder.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            topLeft = new Coordinate(47.024696, 18.009016);
            bottomRight = new Coordinate(46.996722, 18.067896);
            maxLat = topLeft.Latitude - bottomRight.Latitude;
            maxLon = bottomRight.Longitude - topLeft.Longitude;
        }
        
        private void Canvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                var path = Plan();
                path.ForEach(position =>
                {
                    Console.WriteLine($"{position.Coordinate.Latitude.ToString(CultureInfo.GetCultureInfo("en-NZ"))}, {position.Coordinate.Longitude.ToString(CultureInfo.GetCultureInfo("en-NZ"))}");
                    DrawCoordinate(position.Coordinate, true);
                }); 
                Console.WriteLine(path.Last().TimeFromStart);
            });
        }

        private List<BoatPosition> Plan()
        {
            var loaderService = new LoaderService();
            var runConfigurationService = new RunConfigurationService(loaderService);
            var runConfiguration = runConfigurationService.GetRunConfigurationAsync(CancellationToken.None).Result;
            
            var pathFinder = new PathfinderService(new CoordinateProviderService(),
                new TravellingTimeService(new WindProviderService(runConfiguration.WindMap), new PolarDiagramService()));
            
            runConfiguration.Coordinates.ForEach(x => DrawCoordinate(x, true));
            return pathFinder.FindPath(runConfiguration.Coordinates, runConfiguration.Boat, runConfiguration.DateTime, DrawCoordinate);
        }

        private Coordinate topLeft;
        private Coordinate bottomRight;
        private double maxLat;
        private double maxLon;
        
        
        private void DrawCoordinate(Coordinate coordinate, bool isCheckpoint = false)
        {
            var deltaLat = topLeft.Latitude - coordinate.Latitude;
            var deltaLon = coordinate.Longitude - topLeft.Longitude;

            if (deltaLat < 0 || deltaLon < 0)
            {
                return;
            }
            
            var y = deltaLat / maxLat * 1000;
            var x = deltaLon / maxLon * 1000;
            
            Canvas.Dispatcher.Invoke(new Action(() =>
            {
                var circle = new Ellipse();
                circle.Width = isCheckpoint ? 5 : 1;
                circle.Height = isCheckpoint ? 5 : 1;
                circle.Fill = new SolidColorBrush(isCheckpoint ? Colors.Red : Colors.Black);
                Canvas.SetLeft(circle, x);
                Canvas.SetTop(circle, y);
                Canvas.Children.Add(circle);
            }));
        }
    }
}