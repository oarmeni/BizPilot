using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class EditServicePage : Page
    {
        private readonly Frame _parentFrame;
        private readonly Service _originalService;

        public EditServicePage(Service service, Frame parentFrame)
        {
            InitializeComponent();
            _originalService = service;
            _parentFrame = parentFrame;
            LoadServiceData();
        }

        private void LoadServiceData()
        {
            NameBox.Text = _originalService.Name;
            DescriptionBox.Text = _originalService.Description;
            CostBox.Text = _originalService.Cost.ToString("F2", CultureInfo.InvariantCulture);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = NameBox.Text.Trim();
            string description = DescriptionBox.Text.Trim();
            string costText = CostBox.Text.Trim();

            if (!decimal.TryParse(costText, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal cost))
            {
                MessageBox.Show("Η τιμή πρέπει να είναι έγκυρος αριθμός (π.χ. 10.50).", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (name == _originalService.Name &&
                description == _originalService.Description &&
                cost == _originalService.Cost)
            {
                // Καμία αλλαγή — απλή επιστροφή
                _parentFrame.Navigate(new ServicesPage(_parentFrame));
                return;
            }

            using (var db = new AppDbContext())
            {
                var serviceInDb = db.Services.Find(_originalService.Id);
                if (serviceInDb != null)
                {
                    serviceInDb.Name = name;
                    serviceInDb.Description = description;
                    serviceInDb.Cost = cost;
                    db.SaveChanges();
                }
            }

            MessageBox.Show("Η υπηρεσία ενημερώθηκε επιτυχώς.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            _parentFrame.Navigate(new ServicesPage(_parentFrame));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Είσαι σίγουρος ότι θέλεις να διαγράψεις αυτήν την υπηρεσία;", "Επιβεβαίωση", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var serviceInDb = db.Services.Find(_originalService.Id);
                    if (serviceInDb != null)
                    {
                        db.Services.Remove(serviceInDb);
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Η υπηρεσία διαγράφηκε.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
                _parentFrame.Navigate(new ServicesPage(_parentFrame));
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new ServicesPage(_parentFrame));
        }
    }
}
