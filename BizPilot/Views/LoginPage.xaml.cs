using BizPilot.DataAccess;
using BizPilot.Models;
using System.Windows;
using System.Windows.Controls;

namespace BizPilot.Views
{
    public partial class LoginPage : Page
    {
        private readonly User _currentUser;

        // Default constructor για XAML και MainWindow χωρίς παραμέτρους
        public LoginPage() : this(new User { Username = "guest", Password = "", IsAdmin = false }) { }

        public LoginPage(User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameBox.Text;
            string password = PasswordBox.Password;

            using (var db = new AppDbContext())
            {
                var user = db.Users.FirstOrDefault(u => u.Username == username && u.Password == password);

                if (user != null)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    mainWindow?.MainFrame.Navigate(new DashboardPage(user));
                }
                else
                {
                    MessageBox.Show("Λάθος όνομα χρήστη ή κωδικός!", "Σφάλμα", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                

            }
        }
    }
}
