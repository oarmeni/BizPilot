using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class ServicesPage : Page
    {
        private List<Service> _allServices = new();
        private readonly Frame _parentFrame;

        public ServicesPage(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            LoadServices();

        }

        private void LoadServices()
        {
            using (var db = new AppDbContext())
            {
                _allServices = db.Services.ToList();
                ServicesGrid.ItemsSource = _allServices;
            }
        }

        private void AddService_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AddServicePage(_parentFrame));
        }
        private void ServicesGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (ServicesGrid.SelectedItem is Service selectedService)
            {
                NavigationService.Navigate(new EditServicePage(selectedService, _parentFrame));
            }
        }
        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = _allServices
                .Where(s => s.Name.Contains(SearchBox.Text, System.StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            ServicesGrid.ItemsSource = filtered;
        }
    }
}
