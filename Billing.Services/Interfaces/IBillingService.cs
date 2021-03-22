using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Billing.Entities;

namespace Billing.Services.Interfaces
{
    public interface IBillingService
    {
        ReceiptDetails ProcessPayment(BillingDetails billingDetails);
        string GetProperGatewayUrl(int identificator);
    }
}
