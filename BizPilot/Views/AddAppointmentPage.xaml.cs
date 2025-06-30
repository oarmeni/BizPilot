using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class AddAppointmentPage : Page
    {
        private readonly Frame _parentFrame;
        private List<Customer> _allCustomers = new();
        private Customer? _selectedCustomer;

        public AddAppointmentPage(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            LoadCustomers();
            LoadServices();
            LoadTimes();
        }

        private void LoadCustomers()
        {
            using var db = new AppDbContext();
            _allCustomers = db.Customers.ToList();
        }

        private void LoadServices()
        {
            using var db = new AppDbContext();
            var services = db.Services.ToList();
            ServiceComboBox.ItemsSource = services;
            ServiceComboBox.DisplayMemberPath = "Name";
        }

        private void LoadTimes()
        {
            for (int hour = 8; hour <= 20; hour++)
            {
                TimeComboBox.Items.Add($"{hour:00}:00");
                TimeComboBox.Items.Add($"{hour:00}:30");
            }
        }
        private void CustomerSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = CustomerSearchBox.Text.ToLower();

            if (query.Length < 10)
            {
                _selectedCustomer = null;
                PhoneBox.Text = "";
                EmailBox.Text = "";
                return;
            }

            var match = _allCustomers.FirstOrDefault(c =>
                (!string.IsNullOrEmpty(c.FirstName) && c.FirstName.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.LastName) && c.LastName.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.Phone) && c.Phone.ToLower().Contains(query)) ||
                (!string.IsNullOrEmpty(c.Email) && c.Email.ToLower().Contains(query))
            );

            if (match != null)
            {
                _selectedCustomer = match;

                CustomerSearchBox.TextChanged -= CustomerSearchBox_TextChanged;
                CustomerSearchBox.Text = $"{match.FirstName} {match.LastName}";
                CustomerSearchBox.CaretIndex = CustomerSearchBox.Text.Length;
                CustomerSearchBox.TextChanged += CustomerSearchBox_TextChanged;

                PhoneBox.Text = match.Phone;
                EmailBox.Text = match.Email;
            }
            else
            {
                _selectedCustomer = null;
                PhoneBox.Text = "";
                EmailBox.Text = "";

                var result = MessageBox.Show("Ο πελάτης δεν βρέθηκε. Θέλετε να τον προσθέσετε;", "Μη διαθέσιμος πελάτης", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _parentFrame.Navigate(new AddCustomerPage(_parentFrame));
                }
            }
        }
        private void ServiceComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServiceComboBox.SelectedItem is Service selectedService)
            {
                CostBox.Text = selectedService.Cost.ToString("0.00");
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedCustomer == null ||
                AppointmentDatePicker.SelectedDate == null ||
                string.IsNullOrWhiteSpace(TimeComboBox.Text) ||
                ServiceComboBox.SelectedItem is not Service service)
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα απαιτούμενα πεδία!", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var appointment = new Appointment
            {
                CustomerName = _selectedCustomer.FullName,
                AppointmentDate = AppointmentDatePicker.SelectedDate.Value,
                AppointmentTime = TimeComboBox.Text,
                Title = service.Name,
                Notes = NotesBox.Text,
                Status = AppointmentStatus.Upcoming
            };

            using var db = new AppDbContext();
            db.Appointments.Add(appointment);
            db.SaveChanges();

            MessageBox.Show("Το ραντεβού καταχωρήθηκε με επιτυχία!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            _parentFrame.Navigate(new AppointmentsPage(_parentFrame));
        }
        private void AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AddCustomerPage(_parentFrame));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AppointmentsPage(_parentFrame));
        }
    }
}
