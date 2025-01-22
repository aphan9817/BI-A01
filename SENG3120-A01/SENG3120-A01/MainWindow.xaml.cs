using System;
using System.Collections.Generic;
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
        public MainWindow()
        {
            InitializeComponent();

            // test data
            var categories = new[] { "Holes", "Poor Mix", "Stains", "Not Enough Component", "Torn", "Others" };
            var frequencies = new[] { 27, 11, 8, 7, 5, 2 };
            int total = frequencies.Sum();
            double cumulative = 0;
            var dataList = new List<DataModel>();
            var percentages = new List<double>();

            // calculate each percentage
            for (int i = 0; i < frequencies.Length; i++)
            {
                cumulative += frequencies[i];
                double percentage = (cumulative / total) * 100;
                percentages.Add(percentage);

                // add data to list
                dataList.Add(new DataModel
                {
                    Category = categories[i],
                    Frequency = frequencies[i],
                    Percentage = percentage,
                });
            }

            // bind data to the data grid
            DataGrid.ItemsSource = dataList;

            // title
            var plotModel = new PlotModel { Title = "Pareto Diagram" };

            // legend
            plotModel.Legends.Add(new Legend
            {
                LegendTitle = "Legend",
                LegendPosition = LegendPosition.TopRight,
                LegendFontSize = 12
            });
            
            // axis titles
            // category axis
            plotModel.Axes.Add(new CategoryAxis
            {
                Position = AxisPosition.Bottom,
                Key = "Categories",
                ItemsSource = categories,
                Title = "Defect Category",
                IsPanEnabled = false,
                IsZoomEnabled = false,
                MinimumPadding = 0,
                MaximumPadding = 0.1,
                GapWidth = 0.2
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

            // column series 
            var linearBarSeries = new LinearBarSeries
            {
                Title = "Frequency",
                BarWidth = 50,
                FillColor = OxyColors.SteelBlue,
            };

            for (int i = 0; i < categories.Length; i++)
            {
                linearBarSeries.Points.Add(new DataPoint(i, frequencies[i]));
            }
            plotModel.Series.Add(linearBarSeries);


            // line series
            var lineSeries = new LineSeries
            {
                Title = "Cumulative Percentage",
                MarkerType = MarkerType.Circle,
                MarkerFill = OxyColors.Red,
                YAxisKey = "PercentageAxis"
            };

            for (int i = 0; i < categories.Length; i++)
            {
                lineSeries.Points.Add(new DataPoint(i, percentages[i]));
            }
            plotModel.Series.Add(lineSeries);

            // axis values
            MyModel.Model = plotModel;
        }
    }
}
