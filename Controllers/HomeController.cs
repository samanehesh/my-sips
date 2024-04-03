using Microsoft.AspNetCore.Mvc;
using Sips.SipsModels;
using System.Diagnostics;

namespace Sips.Controllers
{
    public class HomeController : Controller
    {
        private readonly SipsdatabaseContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _configuration;

        public HomeController(SipsdatabaseContext db, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _db = db;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            string userEmail = User.Identity.Name;
            var user = _db.Contacts.Where(c => c.Email == userEmail).FirstOrDefault();

            if (user != null)
            {
                string userFirstName = user.FirstName;

                if (HttpContext.Session != null)
                {
                    HttpContext.Session.SetString("SessionUserName", userFirstName);
                }
            }

            var connectionString = _configuration["ConnectionStrings:DefaultConnection"];
            return View();
        }

       
        public IActionResult About()
        {

            return View();
        }
        public IActionResult Contact ()
        {
            return View();
        }

         public IActionResult Error ()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}