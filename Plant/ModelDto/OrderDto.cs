using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ModelDto
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentName { get; set; }
        public DateTime CreateDate { get; set; }
        public decimal Money { get; set; }
        public decimal ShipFee { get; set; }
        public decimal Total { get; set; }
        public string OrderStatus { get; set; }
        public bool Deleted { get; set; }
        public int CustomerId { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int City { get; set; }
        public int District { get; set; }
        public int Ward { get; set; }
        public string Road { get; set; }
        public List<ListCartDto> ListCart { get; set; }
    }
}
