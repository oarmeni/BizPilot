using System;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class EditAppointmentPage : Page
    {
        private Appointment _appointment;
        private readonly Frame _parentFrame;

        public EditAppointmentPage(Appointment appointment, Frame parentFrame)
        {
            InitializeComponent();
            _appointment = appointment;
            _parentFrame = parentFrame;

            LoadAppointmentData();
            LoadTimeOptions();
            UpdateUIForStatus();
        }

        private void LoadAppointmentData()
        {
            CustomerNameBox.Text = _appointment.CustomerName;
            AppointmentDatePicker.SelectedDate = _appointment.AppointmentDate;
            AppointmentTimeBox.SelectedItem = _appointment.AppointmentTime;
            NotesBox.Text = _appointment.Notes;
        }

        private void LoadTimeOptions()
        {
            AppointmentTimeBox.Items.Clear();
            for (int hour = 8; hour <= 20; hour++)
            {
                AppointmentTimeBox.Items.Add($"{hour:00}:00");
                AppointmentTimeBox.Items.Add($"{hour:00}:30");
            }
        }

        private void UpdateUIForStatus()
        {
            if (_appointment.Status == AppointmentStatus.Cancelled)
            {
                CancelledMessage.Visibility = Visibility.Visible;
                CustomerNameBox.IsEnabled = false;
                AppointmentDatePicker.IsEnabled = false;
                AppointmentTimeBox.IsEnabled = false;
                NotesBox.IsEnabled = false;
                SaveButton.IsEnabled = false;
                CancelAppointmentButton.IsEnabled = false;
            }
            else
            {
                CancelledMessage.Visibility = Visibility.Collapsed;
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            using (var db = new AppDbContext())
            {
                var appointmentInDb = db.Appointments.Find(_appointment.Id);
                if (appointmentInDb != null)
                {
                    appointmentInDb.CustomerName = CustomerNameBox.Text;
                    appointmentInDb.AppointmentDate = AppointmentDatePicker.SelectedDate ?? DateTime.Now;
                    appointmentInDb.AppointmentTime = AppointmentTimeBox.SelectedItem?.ToString() ?? "";
                    appointmentInDb.Notes = NotesBox.Text;

                    db.SaveChanges();

                    MessageBox.Show("Οι αλλαγές αποθηκεύτηκαν επιτυχώς!", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
                    _parentFrame.Navigate(new AppointmentsPage(_parentFrame));
                }
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            _parentFrame.Navigate(new AppointmentsPage(_parentFrame));
        }

        private void CancelAppointment_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Είσαι σίγουρος ότι θέλεις να ακυρώσεις το ραντεβού;", "Επιβεβαίωση", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                using (var db = new AppDbContext())
                {
                    var appointmentInDb = db.Appointments.Find(_appointment.Id);
                    if (appointmentInDb != null)
                    {
                        appointmentInDb.Status = AppointmentStatus.Cancelled;
                        db.SaveChanges();
                    }
                }
                MessageBox.Show("Το ραντεβού ακυρώθηκε.", "Επιτυχία", MessageBoxButton.OK, MessageBoxImage.Information);
                _parentFrame.Navigate(new AppointmentsPage(_parentFrame));
            }
        }
    }
}
