using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using TripCalculator.ViewModels;
using Unity;

namespace TripCalculator
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            IUnityContainer container = new UnityContainer();

            container.RegisterType<IMainWindow, MainWindow>();
            container.RegisterType<IMainViewModel, MainViewModel>();

            var mainWindow = container.Resolve<MainWindow>(); // Creating Main window
            mainWindow.Show();
        }
    }
}
