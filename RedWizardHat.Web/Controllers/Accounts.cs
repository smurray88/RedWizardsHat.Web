using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.Extensions.CognitoAuthentication;
using Amazon.AspNetCore.Identity.Cognito;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RedWizardHat.Web.Models.Accounts;

namespace RedWizardHat.Web.Controllers
{
    public class Accounts : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _cognitoPool;
        public Accounts(SignInManager<CognitoUser> signInManager, UserManager<CognitoUser> userManager, CognitoUserPool pool)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _cognitoPool = pool;
        }

        public async Task<IActionResult> Signup()
        {
            var model = new SignupModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupModel signupModel)
        {
            if (ModelState.IsValid)
            {
                var user = _cognitoPool.GetUser(signupModel.Email);
                if (user.Status != null)
                {
                    ModelState.AddModelError("UserExists", "User with this email already exists");
                    return View(signupModel);
                }

                user.Attributes.Add("name", signupModel.Email);

                var createdUser = await _userManager.CreateAsync(user, signupModel.Password);
                if (createdUser.Succeeded)
                {
                    return RedirectToAction("Confirm");
                }
                else
                {
                    foreach (var error in createdUser.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return View(signupModel);
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                                             .SelectMany(v => v.Errors)
                                             .Select(e => e.ErrorMessage));

                ModelState.AddModelError("SignUpError", message);
                View(signupModel);
            }
            return View();
        }


        public async Task<IActionResult> Confirm()
        {
            var model = new ConfirmModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm_Post(ConfirmModel confirmModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(confirmModel.Email).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with the given email address was not found");
                    return View(confirmModel);
                }
                var result = await (_userManager as CognitoUserManager<CognitoUser>).ConfirmSignUpAsync(user, confirmModel.Code, true).ConfigureAwait(false);
                if (result != null)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(error.Code, error.Description);
                        }

                    }
                }
            }

            return View(confirmModel);
        }

        [HttpGet]
        public async Task<IActionResult> Login()
        {
            var loginModel = new LoginModel();
            return View(loginModel);
        }


        [HttpPost]
        [ActionName("Login")]
        public async Task<IActionResult> LoginPost(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(loginModel.Email, loginModel.Password, loginModel.RememberMe, false).ConfigureAwait(false);
                if (result != null)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("LoginError", "Email and Password do not match");
                    }
                }

            }
            return View("Login", loginModel);
        }



        public async Task<IActionResult> Signout()
        {
            if (User.Identity.IsAuthenticated) await _signInManager.SignOutAsync().ConfigureAwait(false);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            var forgotPasswordModel = new ForgotPasswordModel();
            return View(forgotPasswordModel);
        }

        [HttpPost]
        [ActionName("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordPost(ForgotPasswordModel forgotPasswordModel)
        {
            if (ModelState.IsValid)
            {
                //_cognitoPool.for
                var user = await _userManager.FindByEmailAsync(forgotPasswordModel.Email).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with the given email address was not found");
                    return View(forgotPasswordModel);
                }
                await user.ForgotPasswordAsync();
                return RedirectToAction("ResetPassword", "Accounts");
            }
            return View(forgotPasswordModel);
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            var resetPasswordModel = new ResetPasswordModel();
            return View(resetPasswordModel);
        }

        [HttpPost]
        [ActionName("ResetPassword")]
        public async Task<IActionResult> ResetPasswordPost(ResetPasswordModel resetPasswordModel)
        {
            if (ModelState.IsValid)
            {

                CognitoUser user = await _userManager.FindByEmailAsync(resetPasswordModel.Email).ConfigureAwait(false);
                if (user == null)
                {
                    ModelState.AddModelError("NotFound", "A user with the given email address was not found");
                    return View(resetPasswordModel);
                }

                var result = await _userManager.ResetPasswordAsync(user, resetPasswordModel.Code, resetPasswordModel.Password).ConfigureAwait(false);
                if (result != null)
                {
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Login", "Accounts");
                    }
                    else
                    {
                        ModelState.AddModelError("ResetError", "Information Specified is incorrect");
                    }
                }

            }
            return View("Login", resetPasswordModel);
        }

    }
}
