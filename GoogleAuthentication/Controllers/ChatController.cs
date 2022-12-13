using GoogleAuthentication.Models.UserMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoogleAuthentication.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private readonly IUserMaster _user;

        public ChatController(IUserMaster user)
        {
            _user = user;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ShowOnlineUser()
        {
            return View();
        }
        [HttpGet]
        public  IActionResult GetALLUser()
        {
            string userid = HttpContext.Session.GetString("UserName");
            var res =  _user.GetAllUser(userid);
            return Content(res);
        }
        [HttpGet]
        public IActionResult GetALLChat(string selectUser)
        {
            string userid = HttpContext.Session.GetString("UserName");    
            var res =  _user.GellAllChat(userid, selectUser);
            return Content(res);
        }
    }
}
