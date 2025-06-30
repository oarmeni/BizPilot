using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{

    public partial class AddServicePage : Page
    {
        private readonly Frame _parentFrame;
        public AddServicePage(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string description = DescriptionBox.Text.Trim();
            string costText = CostBox.Text.Trim();

            if(string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Η ονομασία είναι υποχρεωτική", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!decimal.TryParse(costText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal cost))
            {
                MessageBox.Show("Η τιμή πρέπει να είναι έγκυρος αριθμός (π.χ. 10.50).", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var newService = new Service
            {
                Name = name,
                Description = description,
                Cost = cost
            };
            using (var db = new AppDbContext())
            {
                db.Services.Add(newService);
                db.SaveChanges();
            }
            MessageBox.Show("Η υπηρεσία προστέθηκε επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            _parentFrame.Navigate(new ServicesPage(_parentFrame));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new ServicesPage(_parentFrame));
        }
    }
}
