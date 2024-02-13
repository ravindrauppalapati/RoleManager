using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using RoleManager.Models;

namespace RoleManager.Controllers
{
    public class AdministratorController : Controller
    { 

        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public AdministratorController(RoleManager<ApplicationRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;

        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(RoleViewModel role)
        {
            if(ModelState .IsValid)
            {
                bool isExisted = await _roleManager.RoleExistsAsync(role?.RoleName);
                if (isExisted)
                {
                    ModelState.AddModelError("", "Role Already Exists");
                }
                else 
                {
                    ApplicationRole iRole = new ApplicationRole { Name = role.RoleName,Description= role.Description };
                   var result =  await _roleManager.CreateAsync(iRole);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    foreach(IdentityError error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(role);
        }

        public async Task<IActionResult> ListRoles()
        {
            var roleList = await _roleManager.Roles.ToListAsync();

            return View(roleList);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string  Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);

            if(Id  == null)
            {
                return View("Error");
            }

            EditRoleViewModel model = new EditRoleViewModel {  roleId =role.Id, RoleName = role.Name ,Description= role.Description };

            model.Users = new List<string>();

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                { 
                    model.Users.Add(user.UserName);
                }
                
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(EditRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _roleManager.FindByIdAsync(model.roleId);
                if (role == null)
                {
                    ViewBag.ErrorMsg = $"Role with Id {model.roleId} can not be found";
                    return View("Error");
                }
                else
                {
                    role.Name = model.RoleName;
                    role.Description = model.Description;

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded) 
                    {
                        return RedirectToAction("ListRoles");
                    }
                    return View(model); 
                }
            }
            return View(model);
        }

     
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                ViewBag.ErrorMsg = $"Role with Id {Id} can not be found";
                return View("Error");
            }
            else 
            {
                var result = await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("ListRoles");
                } 
            }

            return View("ListRoles", await _roleManager.Roles.ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string RoleId)
        {
            ViewBag.roleId = RoleId;

            var role = await _roleManager.FindByIdAsync(RoleId);
            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role Id with {RoleId} can not be found";
                return View("Error");
            }
            ViewBag.RoleName = role.Name;

            var userModel = new List<UserRoleViewModel>();
           
            foreach (var user in _userManager.Users.ToList())
            {
                UserRoleViewModel model = new UserRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.IsSelected = true;
                }
                else model.IsSelected = false;

                userModel.Add(model);
            }

            return View(userModel);
        } 
     

        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Role with Id = {roleId} cannot be found";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult? result;


                if (model[i].IsSelected && !(await _userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                else { continue; }

                if (result.Succeeded)
                {
                    return RedirectToAction("EditRole", new { Id = roleId });
                }
                
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
    }
}
