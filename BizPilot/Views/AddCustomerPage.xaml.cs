using System;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class AddCustomerPage : Page
    {
        private readonly Frame _parentFrame;
        public AddCustomerPage(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα απαραίτητα πεδία.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                var customer = new Customer
                {
                    FirstName = FirstNameBox.Text,
                    LastName = LastNameBox.Text,
                    Email = EmailBox.Text,
                    Phone = PhoneBox.Text,
                    Address = AddressBox.Text,
                    Notes = NotesBox.Text,
                    DateOfBirth = BirthDatePicker.SelectedDate ?? null,
                    Gender = (GenderComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "",
                    DateAdded = DateTime.Now
                };

                db.Customers.Add(customer);
                db.SaveChanges();
            }

            MessageBox.Show("Ο πελάτης αποθηκεύτηκε επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);

            var mainWindow = Application.Current.MainWindow as MainWindow;
            _parentFrame.Navigate(new CustomersPage(_parentFrame));
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            _parentFrame.Navigate(new CustomersPage(_parentFrame));
        }
    }
}
