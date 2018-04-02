using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.Models;
using FilmSearch.Models.View;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class AuthController : Controller
    {
        UserManager<AppUser> userManager;
        SignInManager<AppUser> signInManager;

        public AuthController(UserManager<AppUser> userMgr, SignInManager<AppUser> signInMgr)
        {
            userManager = userMgr;
            signInManager = signInMgr;
        }

        public IActionResult Register()
        {
            RegisterViewModel toRegister = new RegisterViewModel();

            return View(toRegister);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel toRegister)
        {
            if (!ModelState.IsValid)
            {
                return View(toRegister);
            }

            AppUser newUser = new AppUser()
            {
               Email = toRegister.Email,
               UserName = toRegister.Name,
               Surname = toRegister.Surname
            };

            IdentityResult result = await userManager.CreateAsync(newUser, toRegister.Password);

            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return View(toRegister);
            }
        }

        public ActionResult Login(string returnUrl)
        {
            LoginViewModel model = new LoginViewModel();

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            AppUser user = await userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await signInManager.SignOutAsync();

                Microsoft.AspNetCore.Identity.SignInResult result =
                await signInManager.PasswordSignInAsync(
                user, model.Password, false, false);

                if (result.Succeeded)
                {
                    return Redirect(returnUrl ?? "/");
                }
            }

            ModelState.AddModelError(nameof(LoginViewModel.Email), "Invalid email or password");

            return View(model);
        }

        public async Task<ActionResult> Logout()
        {
            await signInManager.SignOutAsync();

            return RedirectToAction("Index", "Home");
        }
    }
}