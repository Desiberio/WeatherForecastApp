using ReactiveUI;
using Splat;
using System.Reactive.Disposables;
using System.Windows;
using System.Windows.Controls;
using WeatherForecastUI.ViewModels;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;
using System.Windows.Input;

namespace WeatherForecastUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public AppBootstrapper AppBootstrapper { get; protected set; }
        private bool InSearch = false;

        public MainWindow()
        {
            InitializeComponent();

            AppBootstrapper = new AppBootstrapper();

            ViewModel = new MainWindowViewModel();
            DataContext = AppBootstrapper;

            this.WhenActivated(d =>
            {
                HandleBindings(this, d);
            });
        }

        private void HandleBindings(MainWindow mainWindow, CompositeDisposable disposableRegistration)
        {
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.geo_object.FullAddress,
                view => view.LocationTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.forecasts,
                view => view.ItemsContol.ItemsSource)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.tempFormatted,
                view => view.TemperatureTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.factCondiotionRU,
                view => view.ConditionTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.feelsLikeFormatted,
                view => view.FeelsLikeTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.windSummary,
                view => view.WindTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.pressureFormated,
                view => view.BarometerTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.UVIndexFormatted,
                view => view.UVIndexTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.WeatherForecast.fact.humidityFormatted,
                view => view.HumidityTextBlock.Text)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.Locations,
                view => view.LocationComboBox.ItemsSource)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.ShowWindowCommand,
                view => view.TrayIcon.DoubleClickCommand)
                .DisposeWith(disposableRegistration);
            mainWindow.OneWayBind(ViewModel,
                viewModel => viewModel.GetCurrentForecastCommand,
                view => view.TrayIcon.LeftClickCommand)
                .DisposeWith(disposableRegistration);
            mainWindow.BindCommand(ViewModel,
                viewModel => viewModel.ExitCommand,
                view => view.ExitMenuItem)
                .DisposeWith(disposableRegistration);
            mainWindow.BindCommand(ViewModel,
                viewModel => viewModel.ShowWindowCommand,
                view => view.ShowMenuItem)
                .DisposeWith(disposableRegistration);
        }

        private void Panel_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            FrameworkElement fe = e.OriginalSource as FrameworkElement;

            Locator.Current.GetService<IForecastDataModel>().SelectedForecast = (Forecast)fe.DataContext;
        }

        private void ReactiveWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (InSearch == false) return;
            var mousePostion = e.GetPosition(this);
            Point textBoxRelativePoint = LocationComboBox.TransformToAncestor(this)
                                  .Transform(new Point(0, 0));

            //if mouse clicks inside of ComboBox we don't care
            if (LocationComboBox.IsDropDownOpen)
            {
                if (mousePostion.X > textBoxRelativePoint.X && mousePostion.X < (textBoxRelativePoint.X + LocationComboBox.ActualWidth) &&
                    mousePostion.Y > textBoxRelativePoint.Y && mousePostion.Y < (textBoxRelativePoint.Y + LocationComboBox.ActualHeight + LocationComboBox.MaxDropDownHeight))
                {
                    return;
                }
            }
            if (mousePostion.X > textBoxRelativePoint.X && mousePostion.X < (textBoxRelativePoint.X + LocationComboBox.ActualWidth) &&
               mousePostion.Y > textBoxRelativePoint.Y && mousePostion.Y < (textBoxRelativePoint.Y + LocationComboBox.ActualHeight))
            {
                return;
            }

            //and if clicks outside we wrap ComboBox
            SearchAndWrapComboBox();
        }

        private async void SearchAndWrapComboBox()
        {
            WrapLocationComboBox();
            InSearch = false;
            if (LocationComboBox.SelectedItem == null) return;
            await ViewModel.ChangeLocation(LocationComboBox.SelectedItem as LocationModel);
        }

        private void WrapLocationComboBox()
        {
            searchButton.Visibility = Visibility.Visible;
            Keyboard.ClearFocus();
            LocationComboBox.BeginAnimation(TextBox.WidthProperty, ViewModel.WrapTextBox);
            LocationComboBox.Visibility = Visibility.Hidden;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            searchButton.Visibility = Visibility.Hidden;
            LocationComboBox.Focus();
            LocationComboBox.BeginAnimation(TextBox.WidthProperty, ViewModel.UnwrapTextBox);
            LocationComboBox.Visibility = Visibility.Visible;
            InSearch = true;
        }

        private void locationComboBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter || e.Key == Key.Escape)
            {
                SearchAndWrapComboBox();
            }
        }

        private void ReactiveWindow_StateChanged(object sender, System.EventArgs e)
        {
            if (WindowState == WindowState.Minimized) Hide();
        }

        private void ReactiveWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            TrayIcon.Dispose();
        }
    }
}
