using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class EditUserPage : Page
    {
        private readonly Frame _parentFrame;
        private readonly User _selectedUser;
        private readonly User _currentUser;

        public EditUserPage(User selectedUser, Frame parentFrame, User currentUser)
        {
            InitializeComponent();
            _selectedUser = selectedUser;
            _parentFrame = parentFrame;
            _currentUser = currentUser;

            LoadUserData();
        }
        private void LoadUserData()
        {
            UsernameBox.Text = _selectedUser.Username;
        }
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string newUsername = UsernameBox.Text.Trim();
            string newPassword = Password1Box.Password.Trim();
            string confirmPassword = Password1Box.Password.Trim();

            using (var db = new AppDbContext())
            {
                var userInDb = db.Users.FirstOrDefault(u => u.Id == _selectedUser.Id);
                if (userInDb == null)
                {
                    MessageBox.Show("Ο χρήστης δεν βρέθηκε.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Αν αλλάζει το username, έλεγχος αν υπάρχει ήδη
                if (newUsername != userInDb.Username &&
                    db.Users.Any(u => u.Username == newUsername))
                {
                    MessageBox.Show("Αυτό το όνομα χρήστη υπάρχει ήδη.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (!string.IsNullOrWhiteSpace(newPassword))
                {
                    if (newPassword != confirmPassword)
                    {
                        MessageBox.Show("Οι νέοι κωδικοί δεν ταιριάζουν.", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    userInDb.Password = newPassword;
                }

                userInDb.Username = newUsername;
                db.SaveChanges();
            }

            MessageBox.Show("Ο χρήστης ενημερώθηκε επιτυχώς.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
            _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (!_currentUser.IsAdmin)
            {
                MessageBox.Show("Μόνο διαχειριστής μπορεί να διαγράψει χρήστες.", "Απαγορεύεται", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show("Είσαι σίγουρος ότι θέλεις να διαγράψεις αυτόν τον χρήστη;", "Επιβεβαίωση", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var userToDelete = db.Users.FirstOrDefault(u => u.Id == _selectedUser.Id);
                    if (userToDelete != null)
                    {
                        db.Users.Remove(userToDelete);
                        db.SaveChanges();
                    }
                }

                MessageBox.Show("Ο χρήστης διαγράφηκε.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
                _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
            }
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new UserSettingsPage(_parentFrame, _currentUser));
        }
    }
}
