using System;
using System.Collections.Generic;

#nullable disable

namespace Plant.Models
{
    public partial class PaymentTranslation
    {
        public int PaymentTranId { get; set; }
        public int PaymentId { get; set; }
        public int LangId { get; set; }
        public string PaymentName { get; set; }
        public string Description { get; set; }

        public virtual Language Lang { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
