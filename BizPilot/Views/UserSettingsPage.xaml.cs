using BizPilot.DataAccess;
using BizPilot.Models;
using System.Windows;
using System.Windows.Controls;

namespace BizPilot.Views
{
    public partial class UserSettingsPage : Page
    {
        private List<User> _allUsers = new();
        private readonly Frame _parentFrame;
        private readonly User _currentUser;


        public UserSettingsPage(Frame parentFrame, User currentUser)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            _currentUser = currentUser;
            LoadUsers();
        }

        private void LoadUsers()
        {
            using (var db = new AppDbContext())
            {
                _allUsers = db.Users.ToList();

                // Απόκρυψη του πραγματικού κωδικού για εμφάνιση
                foreach (var user in _allUsers)
                {
                    user.HiddenPassword = new string('•', user.Password.Length);
                }

                UsersGrid.ItemsSource = _allUsers;
            }
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var filtered = _allUsers
                .Where(u => u.Username.Contains(SearchBox.Text, System.StringComparison.InvariantCultureIgnoreCase))
                .ToList();
            UsersGrid.ItemsSource = filtered;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AddUserPage(_parentFrame, _currentUser));
        }

        private void UsersGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (UsersGrid.SelectedItem is User selectedUser)
            {
                // Admins can edit anyone, users only themselves
                if (_currentUser.IsAdmin || selectedUser.Id == _currentUser.Id)
                {
                    _parentFrame.Navigate(new EditUserPage(selectedUser, _parentFrame, _currentUser));
                }
                else
                {
                    MessageBox.Show("Μπορείς να επεξεργαστείς μόνο το δικό σου λογαριασμό.", "Περιορισμός", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}