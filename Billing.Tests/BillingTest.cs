using System.Threading.Tasks;
using Billing.Entities;
using Billing.Services;
using Billing.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Billing.Tests
{
    [TestClass]
    public class BillingTest
    {
        BillingDetails billing;
        ReceiptDetails receipt;
        private readonly Mock<IConfiguration> _configMock = new Mock<IConfiguration> ();

        [TestInitialize]
        public void TestInitialize ()
        {
            billing = new BillingDetails ()
            {
                UserId = 1234,
                OptionalDescription = "Description",
                OrderNumber = 4321,
                PayableAmount = 12.12,
                PaymentGateway = 44388
            };

            receipt = new ReceiptDetails ()
            {
                PayableAmount = 12.12,
                ReceiptNumber = 432121,
                UserId = 1234,
                PaymentStatus = 1
            };
        }

        [TestMethod]
        public void GetPaymentGatewayResponseTest ()
        {
            _configMock.SetupGet ( x => x [billing.PaymentGateway.ToString ()] )
                .Returns ( "f58f688a-2b28-4036-9cfe-2913fff67100" );

            var bilService = new BillingService ( _configMock.Object );
            var response = bilService.ProcessPayment ( billing );

            Assert.AreEqual ( receipt.PayableAmount, response.PayableAmount );
            Assert.AreEqual ( receipt.UserId, response.UserId );
        }

        [TestMethod]
        public void GetBadRequestPaymentGatewayResponseTest ()
        {
            _configMock.SetupGet ( x => x [billing.PaymentGateway.ToString ()] )
                .Returns ( "d58f681a-2b23-1036-9ffe-2913fff67100" );

            var bilService = new BillingService ( _configMock.Object );
            var response = bilService.ProcessPayment ( billing );

            Assert.IsNull ( response );
        }
    }
}
