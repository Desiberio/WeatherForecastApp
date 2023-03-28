using LiveCharts;
using LiveCharts.Wpf;
using Newtonsoft.Json.Linq;
using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;

namespace WeatherForecastUI.ViewModels
{
    public class GraphChartViewModel : ReactiveObject, IRoutableViewModel
    {
        private Forecast _forecast;
        private SeriesCollection _series;

        public IScreen HostScreen { get; protected set; }
        public string UrlPathSegment => "graph";


        public Forecast Forecast
        {
            get { return _forecast; }
            set
            {
                this.RaiseAndSetIfChanged(ref _forecast, value);
                SetGraphSeries();
            }
        }
        public SeriesCollection GraphSeries
        {
            get { return _series; }
            set => this.RaiseAndSetIfChanged(ref _series, value);
        }

        public Func<double, string> Formatter { get; set; }

        public List<string> AxisXLabels { get; set; }

        public GraphChartViewModel(IScreen screen)
        {
            HostScreen = screen;

            GraphSeries = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<int>(),
                    Stroke = new SolidColorBrush(Color.FromArgb(255, 64, 46, 135)),
                    Fill = new SolidColorBrush(Color.FromArgb(60, 36, 29, 115)),
                    LabelPoint = point => point.Y + "°C",
                    DataLabels = true,
                    FontFamily = new FontFamily("Segoe UI"),
                    LineSmoothness = 0
                }
            };

            Forecast = Locator.Current.GetService<IForecastDataModel>().SelectedForecast;
            Locator.Current.GetService<IForecastDataModel>().ObservableForProperty(f => f.SelectedForecast).Subscribe(f => Forecast = f.GetValue());
            Formatter = value => DateTime.Today.AddHours(value).ToString("HH:mm");
        }

        private void SetGraphSeries()
        {
            var lineSeries = (LineSeries)GraphSeries[0];
            lineSeries.Values.Clear();
            lineSeries.Values.AddRange(_forecast.hours.Select(x => (object)x.temp));
        }
    }
}
