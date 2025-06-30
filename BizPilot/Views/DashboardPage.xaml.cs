using BizPilot.Models;
using System.Windows;
using System.Windows.Controls;

namespace BizPilot.Views
{
    public partial class DashboardPage : Page
    {
        private readonly User _currentUser;

        public DashboardPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new HomePage());
        }

        private void CustomersButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new CustomersPage(ContentFrame));
        }

        private void AppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new AppointmentsPage(ContentFrame));
        }

        private void ServicesButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new ServicesPage(ContentFrame));
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new SettingsPage(ContentFrame, _currentUser));
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Navigate(new LoginPage(_currentUser));
            }
        }
    }
}






