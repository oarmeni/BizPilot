using System.Windows;
using BizPilot.DataAccess;
using BizPilot.Models;
using BizPilot.Views;

namespace BizPilot
{
    public partial class MainWindow : Window
    {
        private readonly User? _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            App.ApplyTheme(Settings.Default.AppTheme ?? "Light");

            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            // Αν ο χρήστης είναι null, φόρτωσε τη LoginPage χωρίς παράμετρο
            if (_currentUser is not null)
                MainFrame.Navigate(new LoginPage(_currentUser));
            else
                MainFrame.Navigate(new LoginPage()); // <-- Απαιτεί constructor χωρίς παραμέτρους
        }

        public MainWindow(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            using (var db = new AppDbContext())
            {
                db.Database.EnsureCreated();
            }

            MainFrame.Navigate(new LoginPage(_currentUser));
        }
    }
}


