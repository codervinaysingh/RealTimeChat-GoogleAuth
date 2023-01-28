using GoogleAuthentication.Models.Payment;
using Microsoft.AspNetCore.Mvc;
using Razorpay.Api;
using System.Security.Cryptography;
using System.Text;

namespace GoogleAuthentication.Controllers
{
    public class PaymentController : Controller
    {
        private readonly string _key = "rzp_test_6Vb4Z0apv502SS";
        private readonly string _secret = "d3DCDX5gwavbEoYABlltSglz";
        public IActionResult Index()
        {
            //FetchAllPayment();
             var model = new PaymentModel() { Amount = 1, Name = "Vinay Singh", Mobile = "8957884575", Email = "vinaysingh.ct@gmail.com" };
            return View(model);
        }
        [HttpPost]
        public IActionResult SubmitPayment(PaymentModel obj)
        {

            OrderModel order = new OrderModel()
            {
                OrderAmount = obj.Amount,
                Currency = "INR",
                Payment_Capture = 0,
                Notes = new Dictionary<string, string>()
                {
                    {"note 1"," first note while creating Order" }
                }
            };
            var orderId = createOrder(order);
            RazorPayOptionsModel razorPayOptions = new RazorPayOptionsModel()
            {
                Key = _key,
                AmountInSubUnits = order.OrderAmountInSubUnits,
                Currency = order.Currency,
                Name = "VinaySingh",
                Description = "Test",
                ImagelogUr1 = "",
                OrderId = orderId,
                ProfileName = obj.Name,
                ProfileContact = obj.Mobile,
                ProfileEmail = obj.Email,
                Notes = new Dictionary<string, string>()
                {
                    {"note 1","This is test paymet Note" }
                }

            };
            return View(razorPayOptions);
        }
        private string createOrder(OrderModel order)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", order.OrderAmountInSubUnits);
                options.Add("currency", order.Currency);
                options.Add("payment_capture", order.Payment_Capture);
                options.Add("notes", order.Notes);

                // create a order

                Order orderResponse = client.Order.Create(options);
                var orderId = orderResponse.Attributes["id"].ToString();
                return orderId;
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public ViewResult AfterPayment()
        {

            var paymentStatus = Request.Form["paymentstatus"].ToString();
            if (paymentStatus=="Fail")
            {
                return View("Fail");
            }
            var orderId = Request.Form["orderid"].ToString();
            var paymentId = Request.Form["paymentid"].ToString();
            var signature = Request.Form["signature"].ToString();
            var validSignature = CompareSignature(orderId, paymentId, signature);
            if (validSignature)
            {
                ViewBag.Message = "Congratilation!! Payment was successful.";
                return View("Success");
            }
            else
            {
                return View("Fail");
            }
                
        }
        private bool CompareSignature(string orderId, string paymentId, string razorPaySignature)
        {
            var text = orderId + "|" + paymentId;
            var secret = _secret;
            var genratedSignature = CalculateSHA256(text, secret);

            if (genratedSignature == razorPaySignature)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        private string CalculateSHA256(string text, string secret)
        {
            string result = "";
            var enc = Encoding.Default;
            byte[]
            baText2BeHashed = enc.GetBytes(text),
              baSalt = enc.GetBytes(secret);
            System.Security.Cryptography.HMACSHA256 hasher = new HMACSHA256(baSalt);
            byte[] baHashedText = hasher.ComputeHash(baText2BeHashed);
            result = string.Join("", baHashedText.ToList().Select(b => b.ToString("x2")).ToArray());
            return result;
        }
        public ViewResult Capture()
        {
            return View();
        }
        public ViewResult CapturePayment(string paymentId)
        {
            try
            {
                RazorpayClient client = new RazorpayClient(_key, _secret);
                Razorpay.Api.Payment payment = client.Payment.Fetch(paymentId);
                var amount = payment.Attributes["amount"];
                var currency = payment.Attributes["currency"];
                Dictionary<string, object> options = new Dictionary<string, object>();
                options.Add("amount", amount);
                options.Add("currency", currency);
                Razorpay.Api.Payment paymentCaptured = payment.Capture(options);
                ViewBag.Message = "Payment captured!";
                return View("Success");
            }
            catch (Exception ex)
            {

                return View("Fail");
            }

        }
        public IActionResult FetchAllPayment()
        {
            RazorpayClient client = new RazorpayClient(_key, _secret);
           List<Razorpay.Api.Payment> payment = client.Payment.All();
            var res = Json(new { payment });
            return (res) ;
        }

    }
}

