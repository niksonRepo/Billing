using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Billing.Entities;
using Billing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Billing.Api.Controllers
{
    [ApiController]
    [Route ( "[controller]" )]
    public class BillingController : Controller
    {
        private readonly IBillingService _billingService;

        public BillingController(IBillingService billingService)
        {
            _billingService = billingService;
        }

        [HttpGet]
        [Route ( "api/GetReceiptDetails" )]

        public IActionResult Get ( BillingDetails billingDetails )
        {
            var receipt = _billingService.ProcessPayment ( billingDetails );

            if ( receipt != null && receipt.PaymentStatus != 1 )
            {
                return Ok ( receipt );
            }

            return Ok ( "Did not processed payment. Pyment status: FAIL" );
        }
    }
}