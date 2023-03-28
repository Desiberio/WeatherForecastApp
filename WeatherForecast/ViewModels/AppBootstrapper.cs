using Newtonsoft.Json;
using ReactiveUI;
using Splat;
using System;
using System.Configuration;
using System.IO;
using WeatherForecastBackend;
using WeatherForecastBackend.Models;
using WeatherForecastUI.Models;
using WeatherForecastUI.Services;

namespace WeatherForecastUI.ViewModels
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; private set; }
        private string _city = "Москва";

        public AppBootstrapper(IMutableDependencyResolver dependencyResolver = null, RoutingState testRouter = null)
        {
            Router = testRouter ?? new RoutingState();
            dependencyResolver = dependencyResolver ?? Locator.CurrentMutable;

            LoadConfiguraion(dependencyResolver);

            RegisterParts(dependencyResolver);

            Router.Navigate.Execute(new GraphChartViewModel(this));
        }

        private void LoadConfiguraion(IMutableDependencyResolver dependencyResolver)
        {
            Configuration local = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal);
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = local.FilePath;
            Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            dependencyResolver.RegisterConstant(configuration, typeof(Configuration));
            var settings = configuration.AppSettings.Settings;

            if (settings["CurrentLocation"] != null) _city = settings["CurrentLocation"].Value;
            else
            {
                settings.Add("CurrentLocation", "Москва");
                configuration.Save(ConfigurationSaveMode.Modified);
            }
        }

        private void RegisterParts(IMutableDependencyResolver dependencyResolver)
        {
            dependencyResolver.RegisterConstant(this, typeof(IScreen));

            //read token in a better way or make access via API
            dependencyResolver.RegisterConstant<IWeatherForecastService>(new WeatherForecastService(File.ReadAllText("./token.txt")));

            dependencyResolver.RegisterConstant(new NotificationService("WeatherForecastApp"), typeof(INotificationService));

            dependencyResolver.RegisterConstant(new LocationsProvider(), typeof(ILocationsProvider));

            dependencyResolver.RegisterConstant(new ForecastDataModel(
                                                    Locator.Current.GetService<IWeatherForecastService>(), 
                                                    Locator.Current.GetService<ILocationsProvider>().Locations.Find(x => x.City == _city)), typeof(IForecastDataModel));
            
        }
    }
}