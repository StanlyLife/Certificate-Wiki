using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class ProfileController : Controller {
		private readonly UserManager<CertificateUser> userManager;
		private readonly SignInManager<CertificateUser> signInManager;
		private readonly ICertificateHandler certificateHandler;

		public ProfileController(
			UserManager<CertificateUser> userManager,
			SignInManager<CertificateUser> signInManager,
			ICertificateHandler certificateHandler
			) {
			this.userManager = userManager;
			this.signInManager = signInManager;
			this.certificateHandler = certificateHandler;
		}

		[Route("Profile/User/{userId}")]
		public async Task<IActionResult> IndexAsync(string userId) {
			if (string.IsNullOrWhiteSpace(userId)) { return View(new ProfileModel()); }

			ProfileModel viewModel = new ProfileModel();
			viewModel.profile = await userManager.FindByIdAsync(userId);
			if (viewModel.profile == null) { return View(new ProfileModel()); }

			viewModel.facm.certificate = certificateHandler.GetByUserId(viewModel.profile.Id);
			if(viewModel.profile.ProfilePicture == null) {
				return View(viewModel);
			}
			viewModel.profilePicture = GetImageUrl(viewModel.profile);
			return View(viewModel);
		}

		[HttpGet]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync() {
			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			if (Profile == null) { return View(); }
			return View(Profile);
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync([FromForm]CertificateUser model, [FromForm] string cropped) {
			if (!ModelState.IsValid) { return View(model); }
			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			if (Profile == null) { return View(); }

			//Update database
			Profile.FirstName = model.FirstName;
			Profile.LastName = model.LastName;
			Profile.Description = model.Description;
			Profile.Country = model.Country;
			Profile.Occupation = model.Occupation;
			Profile.Website = model.Website;
			Profile.isPrivate = model.isPrivate;

			//convert base64 image to byte array
			if (!String.IsNullOrWhiteSpace(cropped)) {
				cropped = cropped.Replace("data:image/png;base64,", "");
				cropped = cropped.Trim();
				byte[] imageBytes = Convert.FromBase64String(cropped);
				Profile.ProfilePicture = imageBytes;
			}

			await userManager.UpdateAsync(Profile);

			return RedirectToAction("User", new { Profile.Id });
		}

		[HttpPost]
		public async Task<IActionResult> DeleteAccountAsync() {
			var user = await userManager.FindByNameAsync(User.Identity.Name);
			await userManager.DeleteAsync(user);
			await signInManager.SignOutAsync();
			return RedirectToAction("index", "home");
			//TODO
			//Delete certificates
		}

		public string GetImageUrl(CertificateUser profile) {
			string imageDataBytes = Convert.ToBase64String(profile.ProfilePicture);
			string imageUrl = string.Format("data:/image/jpeg;base64,{0}", imageDataBytes);
			return imageUrl;
		}
	}
}