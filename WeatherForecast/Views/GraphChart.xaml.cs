using LiveCharts.Wpf;
using ReactiveUI;
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
using WeatherForecastUI.ViewModels;

namespace WeatherForecastUI
{
    /// <summary>
    /// Interaction logic for GraphChart.xaml
    /// </summary>
    public partial class GraphChart
    {
        public GraphChart()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                d(this.OneWayBind(ViewModel, vm => vm.GraphSeries, view => view.graph.Series));
                d(this.OneWayBind(ViewModel, vm => vm.Formatter, view => view.AxisX.LabelFormatter));
            });
        }
    }
}
