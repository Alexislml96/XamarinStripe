using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProyectoComub;
using Stripe;

namespace Server_PAgos.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post([FromBody]PaymentModel payment)
        {
            StripeConfiguration.ApiKey = "sk_test_1fkshXMQqrBjU8e4u7pghwAt001ToqSHEy";
            // You can optionally create a customer first, and attached this to the CustomerId
            var charge = new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(payment.Amount * 100), // In cents, not dollars, times by 100 to convert
                Currency = "mxn", // or the currency you are dealing with
                Description = "Algo bien HD",
                Source = payment.Token,
            };

            //sk_test_1fkshXMQqrBjU8e4u7pghwAt001ToqSHEy
            //var service = new StripeChargeService("sk_test_xxxxxxxxxxxxx");
            var service = new ChargeService();

            try
            {
                var response = service.Create(charge);
                // Record or do something with the charge information
                return Ok(response);
            }
            catch (StripeException ex)
            {
                StripeError stripeError = ex.StripeError;

                // Handle error
                return Ok(false);
            }

            // Ideally you would put in additional information, but you can just return true or false for the moment.
            
        }
    }
}
