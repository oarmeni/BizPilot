using System.Windows;
using System.Windows.Controls;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class SettingsPage : Page
    {
        private readonly Frame _parentFrame;
        private readonly User _currentUser;

        public SettingsPage(Frame parentFrame, User currentUser)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _currentUser = currentUser;
        }
        private void UserSettings_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
        }
        private void AppearanceSettings_Click(Object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AppearanceSettingsPage(_parentFrame, _currentUser));
        }
    }
}
