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

        public AuthController(UserManager<AppUser> mgr)
        {
            userManager = mgr;
        }

        public IActionResult Register()
        {
            RegisterViewModel toRegister = new RegisterViewModel();

            return View(toRegister);
        }

        [HttpPost]
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
    }
}