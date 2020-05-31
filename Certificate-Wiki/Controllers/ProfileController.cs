using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Profile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class ProfileController : Controller {
		private readonly UserManager<CertificateUser> userManager;

		public ProfileController(UserManager<CertificateUser> userManager) {
			this.userManager = userManager;
		}

		[Route("Profile")]
		public IActionResult Index() {
			return View();
		}

		[HttpGet]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync() {
			//var Profile = await userManager.FindByEmailAsync(User.Identity.Name);
			return View();
		}

		[ValidateAntiForgeryToken]
		[HttpPost]
		[Authorize]
		[Route("Profile/edit")]
		public async Task<IActionResult> EditAsync(EditProfileModel model) {
			//TODO
			//Remove CW from single-line if
			if (!ModelState.IsValid) { Console.WriteLine("Modelstate invalid"); return View(model); }

			var Profile = await userManager.FindByEmailAsync(User.Identity.Name);

			//Update database
			Profile.FirstName = model.FirstName;
			Profile.LastName = model.LastName;
			Profile.Website = model.Website;
			Profile.Occupation = model.Occupation;
			Profile.Country = model.Country;

			await userManager.UpdateAsync(Profile);

			Console.WriteLine("Update success");

			return View(model);
		}
	}
}