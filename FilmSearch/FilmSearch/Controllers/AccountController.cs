using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> userManager;
        private IUserValidator<AppUser> userValidator;

        public AccountController(UserManager<AppUser> userMgr, IUserValidator<AppUser> uValidator)
        {
            userManager = userMgr;
            userValidator = uValidator;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            AppUser currUser = await userManager.GetUserAsync(User);

            AppUserViewModel viewModel = new AppUserViewModelConverter().Convert(currUser);

            return View(viewModel);
        }

        [Authorize(Roles ="Administrator")]
        public IActionResult Ban()
        {
            return View(userManager.Users.ToList());
        }

        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> EnableUser(string userId)
        {
            AppUser usrToUpdate = userManager.Users.Where(x => x.Id == userId).First();

            usrToUpdate.EmailConfirmed = true;

            await userManager.UpdateAsync(usrToUpdate);

            return RedirectToAction("Ban", "Account");
        }

        [Authorize(Roles ="Administrator")]
        public async Task<IActionResult> DisableUser(string userId)
        {
            AppUser usrToUpdate = userManager.Users.Where(x => x.Id == userId).First();

            usrToUpdate.EmailConfirmed = false;

            await userManager.UpdateAsync(usrToUpdate);

            return RedirectToAction("Ban", "Account");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AppUserViewModel viewModel)
        {
            AppUser appUser = await userManager.GetUserAsync(User);

            appUser.UserName = viewModel.Name;
            appUser.Surname = viewModel.Surname;

            IdentityResult result = await userValidator.ValidateAsync(userManager, appUser);

            if (result.Succeeded)
            {
                await userManager.UpdateAsync(appUser);
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(viewModel);
            }

            return RedirectToAction("Index", "Account");
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult PersonStats()
        {
            return View();
        }
    
    }
}