/*
* FILE : MainWindow.xaml.cs
* PROJECT : SENG3120 - Assignment #1
* PROGRAMMER : Anthony Phan
* FIRST VERSION : January 19, 2025
* DESCRIPTION :
* The functions in this file are used to display a Pareto diagram using a Defect category data set.
* The chart tool used in this application is OxyPlot https://oxyplot.github.io/
*/

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;

namespace SENG3120_A01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // allow data binding
        private ObservableCollection<DataModel> Data { get; set; }
        public MainWindow()
        {
            InitializeComponent();

            // data set was inspired by M02-BI Charts - Pareto Diagram and modified to have more points
            var datasets = new[]
            {
                new Dataset("5 Points", new[] { "Holes", "Poor Mix", "Stains", "Not Enough Component", "Torn" }, new[] { 27, 11, 8, 7, 5 }),
                new Dataset("8 Points", new[] { "Holes", "Poor Mix", "Stains", "Not Enough Component", "Torn", "Scratched", "Wrong Colour", "Chipped" }, new[] { 27, 11, 8, 7, 5, 3, 17, 20 }),
                new Dataset("10 Points", new[] { "Holes", "Poor Mix", "Stains", "Not Enough Component", "Torn", "Scratched", "Wrong Colour", "Chipped", "Melted", "Other"  }, new[] { 27, 11, 8, 7, 5, 3, 17, 20, 25, 6 })
            };

            // fill combo box with values from dataset
            PointsSelector.ItemsSource = datasets;
            PointsSelector.DisplayMemberPath = "Name";

            // set the default dataset to the first one (5 points)
            PointsSelector.SelectedIndex = 0;
        }

        // FUNCTION    : UpdateChart
        // DESCRIPTION : This function is used to update the Pareto Diagram.
        // PARAMETERS  : None
        // RETURNS     : Nothing
        private void UpdateChart()
        {
            // -- title --
            var plotModel = new PlotModel { Title = "Pareto Diagram" };

            // -- legend --
            plotModel.Legends.Add(new Legend
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.TopRight,
                LegendFontSize = 12
            });

            // -- axis titles --
            // category axis
            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "Categories",
                ItemsSource = Data.Select(d => d.Category).ToArray(),
                Title = "Defect Category",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                MinimumPadding = 0,
                MaximumPadding = 0.1
            });

            // frequency axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left,
                Key = "FrequencyAxis",
                Minimum = 0,
                Maximum = 27,
                Title = "Freq.",
                MajorStep = 10,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            // percentage axis
            plotModel.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Right,
                Key = "PercentageAxis",
                Minimum = 0,
                Maximum = 100,
                Title = "Cum. %",
                MajorStep = 20,
                IsPanEnabled = false,
                IsZoomEnabled = false
            });

            // bar series 
            var linearBarSeries = new LinearBarSeries
            {
                Title = "Frequency",
                BarWidth = 50,
                FillColor = OxyColors.SteelBlue,
            };

            // plot bar series
            for (int i = 0; i < Data.Count; i++)
            {
                linearBarSeries.Points.Add(new DataPoint(i, Data[i].Frequency));
            }
            // plot values
            plotModel.Series.Add(linearBarSeries);


            // line series
            var lineSeries = new LineSeries
            {
                Title = "Cumulative Percentage",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Red,
                YAxisKey = "PercentageAxis"
            };

            // plot line series
            for (int i = 0; i < Data.Count; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, Data[i].Percentage));
            }
            // plot values
            plotModel.Series.Add(lineSeries);

            // -- axis values --
            // create graph
            MyModel.Model = plotModel;
        }


        // FUNCTION    : LoadDataset
        // DESCRIPTION : This function loads the selected dataset
        // PARAMETERS  : Dataset dataset
        // RETURNS     : Nothing
        private void LoadDataset(Dataset dataset)
        {
            // create new data model
            var dataModels = new List<DataModel>();
            for (int i = 0; i < dataset.Categories.Length; i++)
            {
                dataModels.Add(new DataModel
                {
                    Category = dataset.Categories[i],
                    Frequency = dataset.Frequencies[i]
                });
            }

            // sort data by descending for pareto diagrams
            var sortedData = dataModels.OrderByDescending(x => x.Frequency).ToList();

            // calculate the cumlative percentages
            var total = sortedData.Sum(x => x.Frequency);
            double cumulative = 0;

            foreach (var item in sortedData)
            {
                cumulative += item.Frequency;
                item.Percentage = (cumulative / total) * 100;
            }

            // bind the data
            Data = new ObservableCollection<DataModel>(sortedData);
            DataGrid.ItemsSource = Data;

            // update the diagram
            UpdateChart();
        }

        // FUNCTION    : PointsSelector_SelectionChanged
        // DESCRIPTION : This function loads the associated dataset when the points selector is changed
        // PARAMETERS  : object sender, SelectionChangedEventArgs e
        // RETURNS     : Nothing
        private void PointsSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PointsSelector.SelectedItem is Dataset selectedDataset)
            {
                LoadDataset(selectedDataset);
            }
        }
    }
}
