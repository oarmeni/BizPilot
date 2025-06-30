using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class CustomersPage : Page
    {
        private List<Customer> _allCustomers = new();
        private readonly Frame _parentFrame;
        public CustomersPage(Frame parentFrame)
        {
            InitializeComponent();
            LoadCustomers();
            _parentFrame = parentFrame;
        }

        private void LoadCustomers()
        {
            using (var db = new AppDbContext())
            {
                _allCustomers = db.Customers.ToList();
                CustomersGrid.ItemsSource = _allCustomers;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = SearchBox.Text.ToLower();
            var filtered = _allCustomers.Where(c =>
                (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.LastName) && c.LastName.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(query))
            ).ToList();

            CustomersGrid.ItemsSource = filtered;
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            _parentFrame.Navigate(new AddCustomerPage(_parentFrame));
        }

        private void CustomersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (CustomersGrid.SelectedItem is Customer selectedCustomer)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                _parentFrame.Navigate(new EditCustomerPage(_parentFrame, selectedCustomer));
            }
        }
    }
}
