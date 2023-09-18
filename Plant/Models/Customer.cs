using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Customer
    {
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Phone { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public string Road { get; set; }
    }
}
