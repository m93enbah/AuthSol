using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public HomeController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Secret() 
        {
            return View(); 
        }

        [HttpGet]
        public IActionResult Login() 
        {
            
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(string userName,string password) 
        {
            //login functionaility
            var user = await _userManager.FindByNameAsync(userName);
            if (user != null) 
            {
                //sign in 
                //we see that it will sign in the user and password and if success it will inform idenity to 
                //create cookie with infinite time to live
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
                if (signInResult.Succeeded) 
                {
                    return RedirectToAction("Index");
                }
            
            
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(string userName, string password) 
        {


            var user = new IdentityUser
            {
                UserName = userName,
                Email = ""
            };


            var result = await _userManager.CreateAsync(user,password);

            if (result.Succeeded) 
            {
                //sign user here
            }
            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }


    }
}
