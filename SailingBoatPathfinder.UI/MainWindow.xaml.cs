﻿using System;
using System.Collections.Generic;
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

            topLeft = new Coordinate(47.029757, 18.002024);
            bottomRight = new Coordinate(46.99, 18.072447);
            maxLat = topLeft.Latitude - bottomRight.Latitude;
            maxLon = bottomRight.Longitude - topLeft.Longitude;
        }
        
        private void Canvas_OnLoaded(object sender, RoutedEventArgs e)
        {
            Task.Run(() =>
            {
                DrawCoordinate(new Coordinate(47.0073, 18.0265), true);
                DrawCoordinate(new Coordinate(47.0083, 18.0465), true);
                var path = Plan(new Coordinate(47.0073, 18.0265), new Coordinate(47.0083, 18.0465));
                path.ForEach(position => DrawCoordinate(position.Coordinate, true)); 
            });
        }

        private List<BoatPosition> Plan(Coordinate from, Coordinate to)
        {
            var boatLoader = new BoatLoaderService();
            var pathFinder = new PathfinderService(new CoordinateProviderService(),
                new TravellingTimeService(new WindProviderService(), new PolarDiagramService()));
            var boats = boatLoader.ReadFromFileAsync(CancellationToken.None).Result.ToList();
            var boat = boats.First();

            var coordinates = new List<Coordinate>()
            {
                new Coordinate(47.0073, 18.0265),
                new Coordinate(47.0083, 18.0465),
            };
            return pathFinder.FindPath(coordinates, boat, DateTime.Now, DrawCoordinate);
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