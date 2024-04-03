using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Sips.Data;
using Sips.SipsModels;
using Sips.ViewModels;

namespace Sips.Repositories
{
    public class RoleRepo
    {
        private readonly ApplicationDbContext _db;

        public RoleRepo(ApplicationDbContext db)
        {
            this._db = db;
            CreateInitialRole();
        }

        public IEnumerable<RoleVM> GetAllRoles()
        {
            var roles =
                _db.Roles.Select(r => new RoleVM
                {
                    Id = r.Id,
                    RoleName = r.Name
                });

            return roles;
        }

        public RoleVM GetRole(string roleName)
        {
            var role = _db.Roles.Where(r => r.Name == roleName)
                                .FirstOrDefault();

            if (role != null)
            {
                return new RoleVM() { RoleName = role.Name };
            }
            return null;
        }

        public bool CreateRole(string roleName)
        {
            bool isSuccess = true;

            try
            {
                _db.Roles.Add(new IdentityRole
                {
                    Id = roleName.ToLower(),
                    Name = roleName,
                    NormalizedName = roleName.ToUpper()
                });
                _db.SaveChanges();
            }
            catch (Exception)
            {
                isSuccess = false;
            }

            return isSuccess;
        }

        public SelectList GetRoleSelectList()
        {
            var roles = GetAllRoles().Select(r => new
            SelectListItem
            {
                Value = r.RoleName,
                Text = r.RoleName
            });

            var roleSelectList = new SelectList(roles,
                                               "Value",
                                               "Text");
            return roleSelectList;
        }

        public void CreateInitialRole()
        {
            const string ADMIN = "Admin";

            var role = GetRole(ADMIN);

            if (role == null)
            {
                CreateRole(ADMIN);
            }
        }

        //public bool DeleteRole(string roleName)
        //{
        //    bool isSuccess = true;

        //    bool roleCount = _db.UserRoles.Any(ur => ur.RoleId == roleName.ToLower());
        //    if (roleCount)
        //    {
        //        isSuccess = false;
        //    }
        //    else
        //    {
        //        try
        //        {
        //            var role = _db.Roles
        //                               .Where(r => r.Name == roleName)
        //                               .FirstOrDefault();

        //            if (role != null)
        //            {
        //                _db.Roles.Remove(role);
        //                _db.SaveChanges();
        //            }
        //            else
        //            {
        //                isSuccess = false; // Role not found
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            isSuccess = false;
        //        }
        //    }

        //    return isSuccess;
        //}

        //public bool IsRoleAssigned(string roleName)
        //{
        //    // Check if any user is assigned to the given role
        //    return _db.UserRoles.Any(ur => ur.RoleId == roleName);
        //}

    }
}
