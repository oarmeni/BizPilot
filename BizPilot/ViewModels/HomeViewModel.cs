using BizPilot.DataAccess;
using BizPilot.Models;
using System.ComponentModel;



namespace BizPilot.ViewModels
{
    public class HomeViewModel : INotifyPropertyChanged
    {
        public int TotalCustomers { get; set; }
        public int TotalAppointments { get; set; }
        public int TodaysAppointments { get; set; }
        public int CompletedAppointments { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public HomeViewModel()
        {
            using var db = new AppDbContext();

            TotalCustomers = db.Customers.Count();
            TotalAppointments = db.Appointments.Count();

            TodaysAppointments = db.Appointments
                .Where(a => a.AppointmentDate.Date == DateTime.Today)
                .Count();

            CompletedAppointments = db.Appointments
                .Count(a => a.Status == AppointmentStatus.Completed);
        }
    }
}
