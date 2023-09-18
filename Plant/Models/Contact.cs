using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Contact
    {
        public int ContactId { get; set; }
        public string Name { get; set; }
        public int Phone { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
