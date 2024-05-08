using System;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcPustok.Areas.Manage.ViewModels;
using MvcPustok.Models;

namespace MvcPustok.Areas.Manage.Controllers
{
    [Area("manage")]
	public class AccountController:Controller
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
		{
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<IActionResult> CreateAdmin()
        {
            AppUser appUser = new AppUser()
            {
                UserName = "admin"
            };
            var result = await _userManager.CreateAsync(appUser, "Admin123");

            return Json(result);
        }
     
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(AdminLoginViewModel admin, string returnUrl)
        {
            AppUser appUser = await _userManager.FindByNameAsync(admin.UserName);

            if (appUser == null)
            {
                ModelState.AddModelError("", "UserName or Password is not true");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(appUser, admin.Password, false, false);


            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "UserName or Password is not true");
                return View();
            }

            return returnUrl != null ? Redirect(returnUrl) : RedirectToAction("index", "dashboard");
        }
       
    }
}

