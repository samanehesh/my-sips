using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sips.Data;
using Sips.Repositories;
using Sips.SipsModels;
using Sips.ViewModels;
using Microsoft.AspNetCore.Authorization;


namespace Sips.Controllers
{
    [Authorize(Roles = "Admin")]

    public class UserRoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;

        public UserRoleController(ApplicationDbContext context,
                                 UserManager<IdentityUser> userManager)
        {
            _db = context;
            _userManager = userManager;
        }

        public ActionResult Index(string sortOrder, string searchString, int? pageNumber, int pageSize = 10)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "nameSortDesc" : "Name";

            UserRepo userRepo = new UserRepo(_db);
            IEnumerable<UserVM> users = userRepo.GetAllUsers();

            if (!string.IsNullOrEmpty(searchString))
            {
                users =
                    users.Where(p => p.Email.Contains(searchString)).ToList();
            }

            switch (sortOrder)
            {
                case "nameSortDesc":
                    users = users.OrderByDescending(u => u.Email).ToList();
                    break;
                case "Name":
                    users = users.OrderBy(u => u.Email).ToList();
                    break;
            }
            int pageIndex = pageNumber ?? 1;
            var count = users.Count();
            var items = users.Skip((pageIndex - 1) * pageSize)
                                            .Take(pageSize).ToList();
            var paginatedUsers = new PaginatedList<UserVM>(items
                                                                , count
                                                                , pageIndex
                                                                , pageSize);
            return View(paginatedUsers);
        }

        public async Task<IActionResult> Detail(string userName,
                                                string message = "")
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);
            var roles = await userRoleRepo.GetUserRolesAsync(userName);

            ViewBag.Message = message;
            ViewBag.UserName = userName;

            return View(roles);
        }

        public ActionResult Create()
        {
            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();


            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(UserRoleVM userRoleVM)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            if (ModelState.IsValid)
            {
                try
                {
                    var addUR =
                    await userRoleRepo.AddUserRoleAsync(userRoleVM.Email,
                                                        userRoleVM.RoleName);

                    string message = $"{userRoleVM.RoleName} permissions" +
                                     $" successfully added to " +
                                     $"{userRoleVM.Email}.";

                    return RedirectToAction("Detail", "UserRole",
                                      new
                                      {
                                          userName = userRoleVM.Email,
                                          message = message
                                      });
                }
                catch
                {
                    ModelState.AddModelError("", "UserRole creation failed.");
                    ModelState.AddModelError("", "The Role may exist " +
                                                 "for this user.");
                }
            }

            RoleRepo roleRepo = new RoleRepo(_db);
            ViewBag.RoleSelectList = roleRepo.GetRoleSelectList();

            UserRepo userRepo = new UserRepo(_db);
            ViewBag.UserSelectList = userRepo.GetUserSelectList();

            return View();
        }

        public ActionResult Delete(string email, string roleName)
        {
            UserRoleVM userRole = new UserRoleVM
            {
                Email = email,
                RoleName = roleName
            };
            return View(userRole);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(UserRoleVM userRole)
        {
            UserRoleRepo userRoleRepo = new UserRoleRepo(_userManager);

            // Perform the deletion of UserRole
            var isSuccess = await userRoleRepo.RemoveUserRoleAsync(userRole.Email, userRole.RoleName);
            string message = "";

            if (isSuccess)
            {
                message = $"Role '{userRole.RoleName}' removed successfully for user '{userRole.Email}'.";
            }
            else
            {
                message = $"Failed to remove role '{userRole.RoleName}' for user '{userRole.Email}'.";
            }

            // Redirect to the Detail page with the appropriate message
            return RedirectToAction(nameof(Detail), new { userName = userRole.Email, message = message });
        }
    }
}
