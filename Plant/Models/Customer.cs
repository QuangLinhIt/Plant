using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int City { get; set; }
        public int District { get; set; }
        public int Ward { get; set; }
        public string Road { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
