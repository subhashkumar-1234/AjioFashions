using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Ecom.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        [HttpPost("create-payment")]
        public IActionResult CreatePayment([FromBody] PaymentRequest request)
        {
            if (request.Amount <= 0)
                return BadRequest("Invalid amount");

            var orderId = "pay_ord_" + Guid.NewGuid().ToString().Substring(0, 8);
            return Ok(new
            {
                OrderId = orderId,
                Amount = request.Amount,
                Currency = "INR",
                Key = "rzp_test_mockkey12345"
            });
        }

        [HttpPost("verify")]
        public IActionResult VerifyPayment([FromBody] PaymentVerification verification)
        {
            if (string.IsNullOrEmpty(verification.OrderId) || string.IsNullOrEmpty(verification.PaymentId))
                return BadRequest("Verification failed: Missing OrderId or PaymentId");

            return Ok(new
            {
                Status = "SUCCESS",
                Message = "Payment verified successfully",
                TransactionId = "txn_" + Guid.NewGuid().ToString().Substring(0, 12)
            });
        }
    }

    public class PaymentRequest
    {
        public decimal Amount { get; set; }
    }

    public class PaymentVerification
    {
        public string OrderId { get; set; } = string.Empty;
        public string PaymentId { get; set; } = string.Empty;
        public string Signature { get; set; } = string.Empty;
    }
}
