using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Orders = new HashSet<Order>();
            PaymentTranslations = new HashSet<PaymentTranslation>();
        }

        public int PaymentId { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentTranslation> PaymentTranslations { get; set; }
    }
}
