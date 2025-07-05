using GP.Areas.Admin.Models;
using GP.Models;
using IdentityDemo.Areas.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserManagementController : Controller
    {

        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserManagementController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }


        public async Task<IActionResult> IndexAsync()
        {
            var users = await userManager.Users.ToListAsync();
            var userViewModels = new List<UserViewModel>();

            foreach (var user in users)
            {
                var userRoles = await userManager.GetRolesAsync(user);
                var userViewModel = new UserViewModel
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Roles = userRoles.ToList(),
                    IsLockedOut = await userManager.IsLockedOutAsync(user)
                };

                userViewModels.Add(userViewModel);
            }

            return View(userViewModels);

        }

        [HttpGet]
        //get 
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            var userRoles = await userManager.GetRolesAsync(user);

            var roles = roleManager.Roles.Select(r => new SelectListItem 
            {
                Text = r.Name,
                Value = r.Name,
                Selected = userRoles.Contains(r.Name)
            }).ToList();

            var viewModel = new UserRoleViewModel
            {
                UserId = user.Id,
                UserName = user.Email, 
                Roles = roles,
                SelectedRoles = userRoles.ToArray()
            };
            return View(viewModel);
        }

        //Post
        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Edit(UserRoleViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);

            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await userManager.GetRolesAsync(user);

            if(User.Identity.Name == user.Email &&
                currentRoles.Contains("Admin") &&
                (model.SelectedRoles == null || !model.SelectedRoles.Contains("Admin")))
            {
                TempData["ErrorMessage"] = "You cannot remove yourself from the Admin role.";
                return RedirectToAction(nameof(Index));
            }

            var selectedRoles = model.SelectedRoles ?? new string[] { };

            var rolesToRemove = currentRoles.Except(selectedRoles);
            if (rolesToRemove.Any())
            {
                var removeResult = await userManager.RemoveFromRolesAsync(user, rolesToRemove);
                if (!removeResult.Succeeded)
                {
                    ModelState.AddModelError("", "Failed to remove user from some roles");
                    return View(model);
                }
            }

            var newRolesToAdd = selectedRoles.Except(currentRoles);
            if (newRolesToAdd.Any())
            {
                var addResult = await userManager.AddToRolesAsync(user, newRolesToAdd);
                if (!addResult.Succeeded)
                {
                    ModelState.AddModelError("", "Faild to add user to some roles");
                    return View(model);
                }
            }
            TempData["SuccessMessage"] = "User roles updated successfully.";
            return RedirectToAction(nameof(Index));

        }

    }
}
