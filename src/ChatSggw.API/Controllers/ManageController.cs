using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System.Web.Http;
using System.Net.Http;
using Swashbuckle.Swagger.Annotations;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace ChatSggw.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/manage")]
    public class ManageController : ApiController
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationSignInManager signInManager;
        private readonly IAuthenticationManager authenticationManager;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authenticationManager = authenticationManager;
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [AllowAnonymous]
        [Route("removelogin")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> RemoveLogin(string loginProvider, string providerKey)
        {
            //ManageMessageId? message;
            var result = await userManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                //message = ManageMessageId.RemoveLoginSuccess;
                // necessary \/ ?
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                //message = ManageMessageId.Error;
            }
            //return RedirectToAction("ManageLogins", new { Message = message });
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [Route("addphonenumber")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> AddPhoneNumber(AddPhoneNumberParams model)
        {
            if (!ModelState.IsValid)
            {
                //return View(model);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            // Generate the token and send it
            var code = await userManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (userManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await userManager.SmsService.SendAsync(message);
            }
            //return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [AllowAnonymous]
        [Route("enabletwofactorauthentication")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> EnableTwoFactorAuthentication()
        {
            await userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            // return RedirectToAction("Index", "Manage");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [AllowAnonymous]
        [Route("disabletwofactorauthentication")]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> DisableTwoFactorAuthentication()
        {
            await userManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            //return RedirectToAction("Index", "Manage");
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [Route("verifyphonenumber")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> VerifyPhoneNumber(VerifyPhoneNumberParams model)
        {
            if (!ModelState.IsValid)
            {
                //return View(model);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var result = await userManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                //return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [AllowAnonymous]
        [Route("removephonenumber")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> RemovePhoneNumber()
        {
            var result = await userManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                //return RedirectToAction("Index", new { Message = ManageMessageId.Error });
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            //return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [AllowAnonymous]
        [Route("changepassword")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordParams model)
        {
            if (!ModelState.IsValid)
            {
                //return View(model);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            var result = await userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            AddErrors(result);
            //return View(model);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [AllowAnonymous]
        [Route("setpassword")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> SetPassword(SetPasswordParams model)
        {
            if (ModelState.IsValid)
            {
                var result = await userManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    //return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    return Request.CreateResponse(HttpStatusCode.OK);

                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            // return View(model);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [AllowAnonymous]
        [Route("linklogin")]
        [SwaggerResponse(HttpStatusCode.NotImplemented)]
        public HttpResponseMessage LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            // return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
            return Request.CreateResponse(HttpStatusCode.NotImplemented);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = userManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion

        public class VerifyPhoneNumberParams
        {
            [Required]
            [Display(Name = "Code")]
            public string Code { get; set; }

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
        }

        public class AddPhoneNumberParams
        {
            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string Number { get; set; }
        }

        public class ChangePasswordParams
        {
            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Current password")]
            public string OldPassword { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public class SetPasswordParams
        {
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "New password")]
            public string NewPassword { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm new password")]
            [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

    }
}