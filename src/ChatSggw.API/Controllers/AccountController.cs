using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ChatSggw.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Swashbuckle.Swagger.Annotations;

namespace ChatSggw.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private readonly IAuthenticationManager _authenticationManager;
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _authenticationManager = authenticationManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("register")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(RegisterResponseDTO))]
        public async Task<HttpResponseMessage> Register(RegisterParams model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser {UserName = model.Login, Email = model.Email};
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false, false);

                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return Request.CreateResponse(HttpStatusCode.OK, new RegisterResponseDTO
                    {
                        UserId = Guid.Parse(user.Id)
                    });
                }
                AddErrors(result);
            }
            // If we got this far, something failed, redisplay form
            return Request.CreateResponse((HttpStatusCode) 422, ModelState);
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("logoff")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public HttpResponseMessage LogOff()
        {
            _authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Request.CreateResponse(HttpStatusCode.OK);
        }


        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
        }
    }

    public class LoginParams
    {
        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }
    }

    public class LoginResponseDTO
    {
        public Guid? UserId { get; set; }
        public SignInStatus Status { get; set; }
    }

    public class VerifyCodeParams
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class VerifyCodeResponseDTO
    {
        public SignInStatus Status { get; set; }
    }

    public class RegisterParams
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Login { get; set; }
    }

    public class RegisterResponseDTO
    {
        public Guid UserId { get; set; }
    }

    public class ForgotPasswordParams
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class ResetPasswordParams
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ResetPasswordDTO
    {
        public Guid UserId { get; set; }
    }


    public class SendCodeParams
    {
        public string SelectedProvider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class SendCodeDTO
    {
        public string SelectedProvider { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; }
    }

    public class ExternalLoginConfirmationParams
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "Hometown")]
        public string Hometown { get; set; }
    }
}