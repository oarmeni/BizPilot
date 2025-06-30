using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class EditCustomerPage : Page
    {
        private readonly Frame _parentFrame;
        private readonly Customer _customer;

        public EditCustomerPage(Frame parentFrame,Customer customer)
        {
            InitializeComponent();
            _customer = customer;
            LoadCustomerData();
            _parentFrame = parentFrame;
        }

        private void LoadCustomerData()
        {
            FirstNameBox.Text = _customer.FirstName;
            LastNameBox.Text = _customer.LastName;
            EmailBox.Text = _customer.Email;
            PhoneBox.Text = _customer.Phone;
            AddressBox.Text = _customer.Address;
            NotesBox.Text = _customer.Notes;

            if (_customer.DateOfBirth.HasValue)
                BirthDatePicker.SelectedDate = _customer.DateOfBirth.Value;

            // Επιλογή φύλου
            foreach (ComboBoxItem item in GenderComboBox.Items)
            {
                if (item.Content.ToString() == _customer.Gender)
                {
                    GenderComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(FirstNameBox.Text) ||
                string.IsNullOrWhiteSpace(LastNameBox.Text) ||
                string.IsNullOrWhiteSpace(EmailBox.Text) ||
                string.IsNullOrWhiteSpace(PhoneBox.Text))
            {
                MessageBox.Show("Παρακαλώ συμπληρώστε όλα τα υποχρεωτικά πεδία (Όνομα, Επώνυμο , Email, Τηλέφωνο).", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var db = new AppDbContext())
            {
                var customerInDb = db.Customers.Find(_customer.Id);
                if (customerInDb != null)
                {
                    customerInDb.FirstName = FirstNameBox.Text;
                    customerInDb.Email = EmailBox.Text;
                    customerInDb.Phone = PhoneBox.Text;
                    customerInDb.Notes = NotesBox.Text;

                    db.SaveChanges();

                    MessageBox.Show("Οι αλλαγές αποθηκεύτηκαν επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);

                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    _parentFrame.Navigate(new CustomersPage(_parentFrame));
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            _parentFrame.Navigate(new CustomersPage(_parentFrame));
        }
    }
}

