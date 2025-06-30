using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BizPilot.DataAccess;
using BizPilot.Models;

namespace BizPilot.Views
{
    public partial class AppointmentsPage : Page
    {
        private List<Appointment> _allAppointments = new();
        private readonly Frame _parentFrame;

        public AppointmentsPage(Frame parentFrame)
        {
            InitializeComponent();
            _parentFrame = parentFrame;
            LoadAppointments();
            Loaded += AppointmentsPage_Loaded;
            
            
        }

        private void AppointmentsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadAppointments();
            ApplyFilters();
        }

        private void LoadAppointments()
        {
            using (var db = new AppDbContext())
            {
                _allAppointments = db.Appointments.ToList();

                foreach (var appt in _allAppointments)
                {
                    if (appt.Status == AppointmentStatus.Upcoming &&
                        DateTime.TryParse($"{appt.AppointmentDate:yyyy-MM-dd} {appt.AppointmentTime}", out var apptDateTime) &&
                        apptDateTime < DateTime.Now)
                    {
                        appt.Status = AppointmentStatus.Completed;
                        db.Update(appt);
                    }
                }

                db.SaveChanges();
            }
        }

        private void ApplyFilters()
        {
            if (AppointmentsGrid == null) return; // Ασφάλεια

            string query = SearchBox.Text?.ToLower() ?? "";
            string selectedStatus = (StatusFilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            var filtered = _allAppointments
                .Where(a =>
                    (string.IsNullOrEmpty(query) || a.CustomerName.ToLower().Contains(query)) &&
                    (selectedStatus == "Όλα" ||
                     (selectedStatus == "Επερχόμενα" && a.Status == AppointmentStatus.Upcoming) ||
                     (selectedStatus == "Ολοκληρωμένα" && a.Status == AppointmentStatus.Completed) ||
                     (selectedStatus == "Ακυρωμένα" && a.Status == AppointmentStatus.Cancelled)))
                .ToList();

            AppointmentsGrid.ItemsSource = filtered;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void StatusFilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void AddAppointment_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            _parentFrame.Navigate(new AddAppointmentPage(_parentFrame));
        }

        private void AppointmentsGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AppointmentsGrid.SelectedItem is Appointment selected)
            {
                var mainWindow = Application.Current.MainWindow as MainWindow;
                _parentFrame.Navigate(new EditAppointmentPage(selected, _parentFrame ));
            }
        }
    }
}
