using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Swashbuckle.Swagger.Annotations;

namespace ChatSggw.API.Controllers
{
    [Authorize]
    [RoutePrefix("api/manage")]
    public class ManageController : ApiController
    {
        private readonly ApplicationSignInManager _signInManager;
        private readonly ApplicationUserManager _userManager;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [Route("ChangePassword")]
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [SwaggerResponse(HttpStatusCode.OK)]
        public async Task<HttpResponseMessage> ChangePassword(ChangePasswordParams model)
        {
            if (!ModelState.IsValid)
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            var result =
                await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                    await _signInManager.SignInAsync(user, false, false);
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            AddErrors(result);
            //return View(model);
            return Request.CreateResponse(HttpStatusCode.BadRequest);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError("", error);
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
        }
    }
}