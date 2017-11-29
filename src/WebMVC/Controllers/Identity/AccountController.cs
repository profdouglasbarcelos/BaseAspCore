namespace WebMVC.Controllers.Identity
{
    using ApplicationCore.Interfaces;
    using Infrastructure.Identity;
    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using WebMVC.ViewModels;

    [Route("[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAppLogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                 IAppLogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: /Account/SignIn 
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(string returnUrl = null)
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ViewData["ReturnUrl"] = returnUrl;

            return View();
        }

        // POST: /Account/SignIn
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(LoginViewModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ViewData["ReturnUrl"] = returnUrl;

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            //if (result.RequiresTwoFactor)
            //{
            //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberMe });
            //}

            //if (result.Succeeded)
            //{
            //    string anonymousBasketId = Request.Cookies[Constants.BASKET_COOKIENAME];
            //    if (!String.IsNullOrEmpty(anonymousBasketId))
            //    {
            //        await _basketService.TransferBasketAsync(anonymousBasketId, model.Email);
            //        Response.Cookies.Delete(Constants.BASKET_COOKIENAME);
            //    }
            //    return RedirectToLocal(returnUrl);
            //}
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}