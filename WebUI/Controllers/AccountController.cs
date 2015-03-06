using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using WebUI.Infrastructure;
using WebUI.Models;

namespace WebUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        [AllowAnonymous]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> SignUp(AppUser user, string password, string confirmPassword)
        {
            if (ModelState.IsValid)
            {
                if (user != null)
                {
                    if (password != confirmPassword)
                    {
                        ModelState.AddModelError("", "Hasła nie są takie same");
                        return View(user);
                    }

                    IdentityResult result = await UserManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        IdentityResult roleResult = await UserManager.AddToRoleAsync(user.Id, "Users");
                        if (roleResult.Succeeded)
                        {
                            //Send confirmation e-mail
                            string token = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                            string callback = Url.Action("SignUpConfirmation", "Account", new
                            {
                                userId = user.Id,
                                token = token
                            }, Request.Url.Scheme);

                            string subject = "ComputerStore - Potwierdzenie rejestracji";
                            string body = "Aby potwierdzić rejestrację kliknij na poniższy link\n<a href=\"" + callback + "\">link</a>";
                            await UserManager.SendEmailAsync(user.Id, subject, body);


                            return View("Informations", new MvcHtmlString("<h2>Witaj, "
                                + user.UserName
                                + "</h2>" 
                                + "<br/><h4>E-mail z linkiem potwierdzającym rejestrację został wysłany. Sprawdź swoją pocztę."
                                + "<br/>Powrót na "
                                + "<a href=\"" + Url.Action("Index", "Product") + "\">stronę główną</a></h4>"));
                        }
                        else
                        {
                            AddErrorsFromResult(roleResult);
                        }
                    }
                    else
                        AddErrorsFromResult(result);
                }
            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<ActionResult> SignUpConfirmation(string userId, string token)
        {
            if (userId != null || token != null)
            {
                AppUser user = await UserManager.FindByIdAsync(userId);
                if (user != null)
                {
                    IdentityResult result = await UserManager.ConfirmEmailAsync(userId, token);
                    if (result.Succeeded)
                    {
                        return View("Informations", new MvcHtmlString("<br/><h4>Konto zostało aktywowane, " + "<a href=\"" + Url.Action("Login", "Account") + "\">zaloguj się.</a></h4>"));
                    }
                }
            }
            return View("Informations", new MvcHtmlString("Błąd w potwierdzeniu"));
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(Login login, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindAsync(login.Username, login.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Nieprawidłowa nazwa użytkownika lub hasło");
                }
                else
                {
                    if (!user.EmailConfirmed)
                    {
                        ModelState.AddModelError("", "Konto nie zostało aktywowane. Aktywuj je klikając na link wysłany w e-mailu podczas rejestracji.");
                        return View(login);
                    }
                    ClaimsIdentity identity =
                        await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = false
                    }, identity);
                    returnUrl = returnUrl ?? @Url.Action("Panel", "Account");
                    return Redirect(returnUrl);
                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(login);
        }

        public ActionResult Logout(string returnUrl)
        {
            AuthenticationManager.SignOut();
            return Redirect(returnUrl);
        }

        public ActionResult Panel()
        {
            return View();
        }

        public async Task<ActionResult> Edit()
        {
            AppUser user = await UserManager.FindByNameAsync(HttpContext.User.Identity.Name);

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(FormCollection collection)
        {
            string userName = collection["UserName"];

            AppUser user = await UserManager.FindByNameAsync(userName);
            user.FirstName = collection["FirstName"];
            user.LastName = collection["LastName"];
            user.Street = collection["Street"];
            user.City = collection["City"];
            user.ZipCode = collection["ZipCode"];
            user.Email = collection["Email"];

            IdentityResult emailResult = await UserManager.UserValidator.ValidateAsync(user);
            if (emailResult.Succeeded)
            {
                IdentityResult result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Panel");
                }
                else
                {
                    AddErrorsFromResult(result);
                }
            }
            else
            {
                AddErrorsFromResult(emailResult);
            }

            return View(user);
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePassword password)
        {
            AppUser user = await UserManager.FindByNameAsync(User.Identity.Name);
            if (user == null)
            {
                ModelState.AddModelError("", "Nie można znaleźć użytkownika");
                return View();
            }
            if (ModelState.IsValid)
            {
                PasswordVerificationResult passresult =
                    UserManager.PasswordHasher.VerifyHashedPassword(user.PasswordHash, password.OldPassword);
                if (passresult == PasswordVerificationResult.Success)
                {
                    IdentityResult result = await UserManager.ChangePasswordAsync(user.Id, password.OldPassword, password.NewPassword);
                    if (result.Succeeded)
                    {
                        return View("Panel", null, "Zmieniono hasło!");
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }

            }
            return View();
        }

        [AllowAnonymous]
        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(EmailAddressViewModel email)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByEmailAsync(email.EmailAddress);
                if (user != null)
                {
                    string token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
                    string callback = Url.Action("ResetPasswordAfterTokenSend", "Account", new
                    {
                        userId = user.Id,
                        token = token
                    }, Request.Url.Scheme);
                    string subject = "ComputerStore - Reset hasła";
                    string body = "Aby zresetować hasło kliknij na poniższy link\n<a href=\"" + callback + "\">link</a>";
                    await UserManager.SendEmailAsync(user.Id, subject, body);
                    return View("Informations", new MvcHtmlString("<br/><h5>E-mail z linkiem zmieniającym hasło został wysłany. Sprawdź swoją pocztę.</h5>" +
                        "<a href=\"" + Url.Action("Index", "Product") + "\">Powrót na stronę główną.</a>"));
                }
                ModelState.AddModelError("", "Nie można znaleźć użytkownika");
            }
            return View(email);
        }

        [AllowAnonymous]
        public ActionResult ResetPasswordAfterTokenSend(string userId, string token)
        {
            return View(new ResetPasswordViewModel { UserId = userId, Token = token });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPasswordAfterTokenSend(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = await UserManager.FindByIdAsync(model.UserId);
                if (user.Email == model.Email)
                {
                    IdentityResult result =
                        await UserManager.ResetPasswordAsync(model.UserId, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("Informations", new MvcHtmlString("Hasło zostało zmienione, " + "<a href=\"" + Url.Action("Login", "Account") + "\">zaloguj się.</a>"));
                    }
                    else
                    {
                        AddErrorsFromResult(result);
                    }
                }
                ModelState.AddModelError("", "Błąd, spróbuj jeszcze raz");
            }
            return View(model);
        }

        private AppRoleManager RoleManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppRoleManager>(); }
        }

        private AppUserManager UserManager
        {
            get { return HttpContext.GetOwinContext().GetUserManager<AppUserManager>(); }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }
    }
}