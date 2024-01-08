using Microsoft.AspNetCore.Http;
using Plant.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Plant.Services
{
    public interface IVnPayService
    {
        string CreatePaymentUrl(HttpContext context, VnPayRequestVM model);
        VnPayResponseVM PaymentExecute(IQueryCollection collections);
    }
}
