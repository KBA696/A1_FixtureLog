using System.Windows;

namespace FixtureLog
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Views.MainWindow window = new Views.MainWindow() { DataContext = new ViewModels.DataContext.MainWindow() };
            window.Show();
        }
    }
}
