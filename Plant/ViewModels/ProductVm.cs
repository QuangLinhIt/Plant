using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.ViewModels
{
    public class ProductVm
    {
        public int ProductId { get; set; }
        public string Image { get; set; }
        public int? Voucher { get; set; }
        public int ProductTranslationId { get; set; }
        public int LangId { get; set; }
        public string SignLanguages { get; set; }
        public string CurrencyUnit { get; set; }

        public string ProductName { get; set; }
        public decimal? Price { get; set; }
        public decimal? OriginalPrice { get; set; }
        public string ShortDes { get; set; }
        public string Description { get; set; }
        public string TakeCare { get; set; }
        public string Application { get; set; }
        public int CategoryId { get; set; }
    }
}
