using System;
using System.Text;
using System.Net.Http;
using System.Text.Json;
using Billing.Entities;
using System.Threading.Tasks;
using Billing.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Billing.Services
{
    public class BillingService : IBillingService
    {
        private readonly IConfiguration _configuration;

        public BillingService (  )
        {
        }

        public BillingService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ReceiptDetails ProcessPayment(BillingDetails billingDetails)
        {
            return GetPaymentGatewayAsyncResponse(billingDetails).GetAwaiter().GetResult();
        }

        public string GetProperGatewayUrl(int identificator)
        {
            return @$"https://localhost:{identificator}/Gateway";
        }

        public async Task<ReceiptDetails> GetPaymentGatewayAsyncResponse(BillingDetails billingDetails)
        {
            ReceiptDetails details = null;

            try
            {
                string json = string.Empty;
                var gatewayIndetificator = billingDetails.PaymentGateway;
                var url = GetProperGatewayUrl(gatewayIndetificator);

                var request = new HttpRequestMessage(HttpMethod.Get, url);
                request.Headers.Add("ApiKey", _configuration[gatewayIndetificator.ToString()]);
                request.Content = new StringContent(JsonSerializer.Serialize(billingDetails), Encoding.UTF8, "application/json");

                var httpClient = HttpClientFactory.Create ();
                var response = await httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;
                    details = JsonSerializer.Deserialize<ReceiptDetails>(json);
                }
            }
            catch (HttpRequestException e)
            {
            }

            return details;
        }
    }
}
