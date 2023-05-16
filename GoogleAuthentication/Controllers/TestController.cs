using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthentication.Controllers
{
    public class TestController : Controller
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public TestController(IWebHostEnvironment hostEnvironment )
        {
            this.hostEnvironment = hostEnvironment;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            var  test = new Test(this.hostEnvironment);
           var resp= test.Main();
            var res = File(resp, "application/octet-stream", "era.jpg");
            return res;
            //return View();
        }
    }
}
