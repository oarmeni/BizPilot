using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
   
    public partial class AddUserPage : Page
    {
        private readonly Frame _parentFrame;
        private readonly User _currentUser;
        public AddUserPage(Frame parentFrame, User currentUser)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _currentUser = currentUser;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text.Trim();
            string password = PasswordBox.Password.Trim();
            string confirmPassword = Password1Box.Password.Trim();

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Το όνομα χρήστη και ο κωδικός είναι υποχρεωτικά πεδία.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if ( password != confirmPassword )
            {
                MessageBox.Show("Οι δύο κωδικοί δεν ταιριάζουν.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            using (var db = new AppDbContext())
            {
                if(db.Users.Any(u =>  u.Username == username))
                {
                    MessageBox.Show("Αυτό το όνομα χρήστη χρησιμοποιείται ήδη.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                var newUser = new User
                {
                    Username = username,
                    Password = password,
                    IsAdmin = false,
                };
                db.Users.Add(newUser);
                db.SaveChanges();
            }
            MessageBox.Show("Ο χρήστης προστέθηκε επιτυχώς.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
        }
    }
}
