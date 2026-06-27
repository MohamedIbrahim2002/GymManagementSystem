using GymManagment.BLL.ViewModel.AccountViewModel;
using GymManagment.Controllers;
using GymManagment.DAL.Data.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GymManagment.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager ,
            SignInManager<ApplicationUser> signInManager,
            ILogger <AccountController> logger)
        {
           _userManager = userManager;
           _signInManager = signInManager;
             _logger = logger;
        }
        // get login sow form
        [HttpGet]

        public IActionResult Login() => View();


        // post login submit form
        [HttpPost]
        public async Task<IActionResult> Login (LoginViewModel model , CancellationToken ct )
        {

            if(!ModelState.IsValid) return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("InvalidLogin", "InValid Email Or Password");

                return View(model);

            }


           var result = await _signInManager.PasswordSignInAsync(user , model.Password,model.RememberMe , false);


            if(result.Succeeded)

            {
                _logger.LogInformation($"User {user.UserName} successful.signin");
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning($"User {user.UserName} is locked out");
                ModelState.AddModelError("InValidLogin", "this account is locked");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("InvalidLogin", "InValid Email Or Password");

                return View(model);
            }

            
        }

        // post logout
        // user be null when not authorize

        [HttpPost]
        [Authorize] 
        public async Task <IActionResult> Logout()

        {
           await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        //get access denaied

        [HttpGet]

        public IActionResult AccessDenied()
        {
            return View();
        }


    }
}
