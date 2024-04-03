using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sips.Data;
using Sips.Repositories;
using Sips.ViewModels;

namespace Sips.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public RoleController(ApplicationDbContext db)
        {
            _db = db;
        }
        public ActionResult Index(string message = "")
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.Message = message;
            return View(roleRepo.GetAllRoles());
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(RoleVM roleVM)
        {
            if (ModelState.IsValid)
            {
                RoleRepo roleRepo = new RoleRepo(_db);
                bool isSuccess =
                    roleRepo.CreateRole(roleVM.RoleName);

                if (isSuccess)
                {
                    string message = "Role created successfully.";
                    return RedirectToAction("Index", "Role",
                        new
                        {
                            message = message
                        });
                }
                else
                {
                    ModelState
                    .AddModelError("", "Role creation failed.");
                    ModelState
                    .AddModelError("", "The role may already" +
                                       " exist.");
                }
            }

            return View(roleVM);
        }
    }
}

