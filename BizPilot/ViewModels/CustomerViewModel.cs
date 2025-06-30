using System.Collections.ObjectModel;
using System.Linq;
using BizPilot.Models;
using BizPilot.DataAccess;

namespace BizPilot.ViewModels
{
    public class CustomerViewModel
    {
        public ObservableCollection<Customer> Customers { get; set; }

        public CustomerViewModel()
        {
            using (var db = new AppDbContext())
            {
                var customers = db.Customers.ToList();
                Customers = new ObservableCollection<Customer>(customers);
            }
        }
        public Customer? SelectedCustomer { get; set; }
    }
}

