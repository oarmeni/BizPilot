using System.Windows;

namespace BizPilot
{
    public partial class App : Application
    {
        public static void ApplyTheme(string themeName)
        {
            string themePath = $"Themes/{themeName}Theme.xaml";
            var dict = new ResourceDictionary { Source = new Uri(themePath, UriKind.Relative) };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }


        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var theme = Settings.Default.AppTheme ?? "Light";
            ApplyTheme(theme);
        }
    }
}