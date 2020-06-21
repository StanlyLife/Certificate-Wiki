using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.LoginRegister;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Threading;
using System.Drawing;
using System.IO;
using Certificate_Wiki.Services;
using System.Reflection.Metadata.Ecma335;
using System.Reflection.Metadata;

namespace Certificate_Wiki.Controllers {

	public class AuthController : Controller {
		private readonly UserManager<CertificateUser> userManager;
		private readonly IUserClaimsPrincipalFactory<CertificateUser> claimsPrincipalFactory;
		private readonly SignInManager<CertificateUser> signInManager;
		private readonly IEmailSender emailSender;

		public AuthController(
			UserManager<CertificateUser> UserManager,
			IUserClaimsPrincipalFactory<CertificateUser> ClaimsPrincipalFactory,
			SignInManager<CertificateUser> signInManager,
			IEmailSender emailSender
			) {
			userManager = UserManager;
			claimsPrincipalFactory = ClaimsPrincipalFactory;
			this.signInManager = signInManager;
			this.emailSender = emailSender;
		}

		[HttpGet]
		[Route("login")]
		public IActionResult login() {
			return View();
		}

		[HttpPost]
		[Route("login")]
		public async Task<IActionResult> loginAsync(LoginModel model) {
			if (!ModelState.IsValid) { Console.WriteLine("Modelstate invalid"); return View(model); }

			var user = await userManager.FindByNameAsync(model.UserName);

			if (user == null) {
				ModelState.AddModelError("All", "No user with that username exists");
				return View(model);
			}

			var result = await userManager.CheckPasswordAsync(user, model.Password);
			if (!result) {
				ModelState.AddModelError("All", "Password did not match user");
				return View(model);
			}

			//2FA

			await signInManager.PasswordSignInAsync(user, model.Password, true, false);
			//Login

			// ADD lockout
			//var principal = await claimsPrincipalFactory.CreateAsync(user);
			//await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);

			Console.WriteLine("Logged in!");
			return RedirectToAction("index", "home");
		}

		[HttpGet]
		[Route("Register")]
		public IActionResult register() {
			return View();
		}

		[HttpPost]
		[Route("Register")]
		public async Task<IActionResult> registerAsync(RegisterModel model) {
			if (!ModelState.IsValid) {
				Console.WriteLine("modelstate failed"); //Remove
				return View(model);
			}
			var user = await userManager.FindByEmailAsync(model.Email);
			if (user != null) { ModelState.AddModelError("Email", "User Already exists"); return View(model); }

			//Register user
			user = new CertificateUser {
				Id = Guid.NewGuid().ToString(),
				UserName = model.UserName, /*Username cannot be null in Identity*/
				NormalizedEmail = userManager.NormalizeEmail(model.Email), /*Username cannot be null in Identity*/
				Email = model.Email,
				isPrivate = true
			};

			user.ProfilePicture = RandomProfilePicture();

			var result = await userManager.CreateAsync(user, model.Password);

			if (!result.Succeeded) {
				foreach (var error in result.Errors.ToList())
					ModelState.AddModelError("All", error.Description.ToString());
				return View(model);
			}

			Console.WriteLine("registered!");
			return RedirectToAction("login");
		}

		public IActionResult logout() {
			signInManager.SignOutAsync();
			return RedirectToAction("index", "home");
		}

		[HttpGet]
		[Route("Forgotpassword")]
		public IActionResult forgotpassword() {
			return View();
		}

		[HttpPost]
		[Route("Forgotpassword")]
		public async Task<IActionResult> forgotpasswordAsync(ForgotPasswordModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			CertificateUser user = null;
			if (!string.IsNullOrWhiteSpace(model.email)) {
				user = await userManager.FindByEmailAsync(model.email);
			} else if (!string.IsNullOrWhiteSpace(model.userName)) {
				user = await userManager.FindByNameAsync(model.userName);
			} else {
				ModelState.AddModelError("All", "Enter a valid email or username");
			}

			if (user == null) {
				ModelState.AddModelError("All", "No user found with that mail or username");
				return View(model);
			}

			var token = await userManager.GeneratePasswordResetTokenAsync(user);
			var resetUrl = Url.Action("ResetPassword", "Auth", new { token = token, email = user.Email }, Request.Scheme);

			//System.IO.File.WriteAllText("resetLink.txt", resetUrl);
			//Send email to user
			List<string> emailList = new List<string> { user.Email };
			await emailSender.SendEmailAsync(emailList, "Certificate.Wiki PASSWORD RESET",
				"<h1>Reset password</h1> <br> <hr> <br>" +
				$" <a href=\"{resetUrl}\"> <h3> Click here to reset password </h3>  </a>"+
				"<style>" +
				"h3 {" +
				"color: cyan;" +
				"}" +
				"</style>"
				);
			Console.WriteLine("reset password email sent");
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string token, string email) {
			ResetPasswordModel model = new ResetPasswordModel {
				token = token,
				email = email
			};
			return View(model);
		}

		//[HttpPost]
		public async Task<IActionResult> ResetPasswordAsync(ResetPasswordModel model) {
			if (!ModelState.IsValid) {
				return View(model);
			}

			CertificateUser user = null;
			user = await userManager.FindByEmailAsync(model.email);
			if (user == null) {
				ModelState.AddModelError("All", $"user not found, email may be incorrect {model.email}");
			}

			var result = await userManager.ResetPasswordAsync(user, model.token, model.password);

			if (!result.Succeeded) {
				foreach (var error in result.Errors) {
					ModelState.AddModelError("All", error.ToString());
				}
			}

			return RedirectToAction("login", "auth");
		}

		public byte[] RandomProfilePicture() {
			Random randomNumber = new Random();
			int imageNumber = randomNumber.Next(1, 70);
			return FileToBytes(Image.FromFile($@"wwwroot\images\profile\profile-picture\avatar ({imageNumber}).png"));
		}

		public byte[] FileToBytes(Image img) {
			using (var ms = new MemoryStream()) {
				img.Save(ms, img.RawFormat);
				return ms.ToArray();
			}
		}
	}
}