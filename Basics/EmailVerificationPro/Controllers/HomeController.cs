using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using System.Threading.Tasks;
using System.Web;

namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        public readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IEmailService _emailService;

        public HomeController(UserManager<IdentityUser> userManager,SignInManager<IdentityUser> signInManager,IEmailService emailService) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
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
                //it will create user identity with create cookie contains all the user information
                if (signInResult.Succeeded) 
                {
                    //generation of the email token 


                    return RedirectToAction("Ubdex");
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
        public async Task<IActionResult> Register(User) 
        {
            var user = new IdentityUser
            {
                UserName = userName,
                Email = ""
            };
            var result = await _userManager.CreateAsync(user,email);
            //it will create user identity with create cookie contains all the user information
            if (result.Succeeded) 
            {
                //generation of the email Token
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                string codeHtmlVersion = HttpUtility.UrlEncode(code);

                //it will generate link to redirect ot the VerifyEmail
                var link = Url.Action(nameof(VerifyEmail),"Home",new {userId = user.Id,code = codeHtmlVersion },Request.Scheme,Request.Host.ToString());

                await _emailService.SendAsync("mohammedenbah93@gmail.com", "Email Verify",
                    $"<a href=\"{link}\"></a>");
                return RedirectToAction("EmailVerification");
            }
            return RedirectToAction("Index");
        }


        public IActionResult EmailVerification() => View();


        public async Task<IActionResult> VerifyEmail(string userId, string code) 
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return BadRequest();
            }

            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded) 
            {
                return View();
            }
            return BadRequest();
        }

        public ActionResult EmailVerificatoin() 
        {
            return View();
        }

        public async Task<IActionResult> Logout() 
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }
    }
}
