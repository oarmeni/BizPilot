using System.Windows;
using System.Windows.Controls;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class AppearanceSettingsPage : Page
    {
        private readonly Frame _parentFrame;
        private readonly User _currentUser;

        public AppearanceSettingsPage(Frame parentFrame, User currentUser)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _currentUser = currentUser;

            ThemeToggle.IsChecked = false; 
            LanguageToggle.IsChecked = false; 
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            // Dark
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // Light
        }

        private void LanguageToggle_Checked(object sender, RoutedEventArgs e)
        {
            // EN
        }

        private void LanguageToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            // EL
        }
    }
}

