using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Migrations;
using Certificate_Wiki.Models;
using Certificate_Wiki.Models.Certificate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Certificate_Wiki.Controllers {

	public class CertificateController : Controller {
		private ICertificateHandler CertificateHandler;
		private readonly UserManager<CertificateUser> userManager;
		private readonly IFavoriteHandler favoriteHandler;
		private readonly SignInManager<CertificateUser> signInManager;

		private List<string> allowedFiles = new List<string> {
				"pdf",
				"png",
				"jpg",
				"jpeg",
			};

		public CertificateController(ICertificateHandler certificateHandler,
			UserManager<CertificateUser> userManager,
			IFavoriteHandler favoriteHandler,
			SignInManager<CertificateUser> signInManager) {
			this.CertificateHandler = certificateHandler;
			this.userManager = userManager;
			this.favoriteHandler = favoriteHandler;
			this.signInManager = signInManager;
		}

		[HttpGet]
		[Route("Certificate/View/{id:int}")]
		public async Task<IActionResult> IndexAsync(int id) {
			var model = new CertificateIndex { IsOwner = false };
			var certificate = CertificateHandler.GetById(id);
			ViewBag.story = JsonConvert.SerializeObject(new String(""));

			if (certificate == null) { return RedirectToAction("Index", "Home"); }
			model.Certificate = certificate;
			model.CertificateOwner = await userManager.FindByIdAsync(certificate.UserFk);

			if (model.CertificateOwner == null) { return RedirectToAction("Index", "Home"); }
			if (model.CertificateOwner.UserName == User.Identity.Name) { model.IsOwner = true; }

			if (model.Certificate.CertificateFile != null) {
				if (certificate.CertificateFileName.ToString().ToLower().EndsWith(".pdf")) {
					string imageDataBytes = Convert.ToBase64String(model.Certificate.CertificateFile);
					ViewBag.story = JsonConvert.SerializeObject(imageDataBytes);
				} else {
					model.CertificateUrl = GetImageUrl(model.Certificate.CertificateFile);
					model.CertificateFile = null;
				}
			} else {
				model.CertificateUrl = certificate.CertificateUrl;
			}

			model.IsPrivate = model.CertificateOwner.isPrivate;
			/*TODO
				Add link to profile in html
			 */
			return View(model);
		}

		[HttpGet]
		public IActionResult ViewCertificate() {
			return View();
		}

		[Authorize]
		[HttpGet]
		public async Task<IActionResult> UserAsync() {
			CertificateUser user = await userManager.FindByNameAsync(User.Identity.Name);
			//
			//	when database is reset the user may still be logged in
			//	If user tries to enter a restricted page, he or she will be
			//	signed out
			if (user == null) { return RedirectToAction("logout", "auth"); }
			FavoriteAndCertificateModel viewModel = new FavoriteAndCertificateModel {
				certificate = CertificateHandler.GetByUserId(user.Id),
				profile = user
			};
			foreach (var cert in viewModel.certificate.ToList()) {
				bool isFavorite = favoriteHandler.CheckUserFavortite(user.Id, cert.CertificateId);
				viewModel.isFavorite.Add(isFavorite);
			}

			if (user.ProfilePicture != null) {
				viewModel.ProfileImageUrl = GetImageUrl(user.ProfilePicture);
			}

			return View(viewModel);
		}

		[Authorize]
		[HttpGet]
		[Route("Certificate/e/{id:int?}")]
		public async Task<IActionResult> UploadAsync(int? id) {
			Certificates certificate = new Certificates();
			if (id.HasValue) {
				Console.WriteLine(id);
				certificate = CertificateHandler.GetById(id.Value);
				if (certificate == null || !await AuthorizeOwnerAsync(id.Value)) {
					return RedirectToAction("index", "home");
				}
			}
			return View(certificate);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[Route("Certificate/e/{id:int?}")]
		[Authorize]
		public async Task<IActionResult> UploadAsync(Certificates model, [FromForm(Name = "file")] IFormFile file, int? id) {
			//TODO
			//Change subject to a <select> stored in database
			//Add modelvalidators
			Certificates certificateEntity = new Certificates();
			if (id.HasValue) {
				certificateEntity = CertificateHandler.GetById(id.Value);
				if (certificateEntity.CertificateId == 0) { return RedirectToAction("Index", "Home"); }
			}

			if (file != null) {
				//file upload
				var fileType = ValidateFileName(file);
				var fileSize = ValidateFileSize(file);
				if (!fileType) {
					ModelState.AddModelError("All", "File can only be of type: pdf, png, jpg or jpeg");
				}
				if (!fileSize) {
					ModelState.AddModelError("All", "Max file size is 750kb");
				}
				model.CertificateUrl = null;
				model.CertificateFile = FileToBytes(file);
				model.CertificateFileName = file.FileName;
			} else if (model.CertificateUrl != null) {
				//url upload
				model.CertificateFile = null;
			}

			if (model.CertificateUrl == null && model.CertificateFile == null) {
				if (!id.HasValue || !HasCertificate(id.Value)) {
					ModelState.AddModelError("All", "You must upload or link to a certificate image");
				}
			}

			if (model.CertificateUrl != null && model.CertificateFile != null) {
				ModelState.AddModelError("All", "You cannot upload certificate and link a certificate");
			}

			if (!ModelState.IsValid) { return View(model); }

			//If the user is UPDATING a certificate and not creating a new one
			model.UserFk = userManager.GetUserId(User);
			if (id.HasValue && await AuthorizeOwnerAsync(id.Value)) {
				await TryUpdateModelAsync(certificateEntity);
				CertificateHandler.Update(certificateEntity);
				return RedirectToAction("user");
			}

			CertificateHandler.Create(model);
			//If user is CREATING
			return RedirectToAction("user");
		}

		private bool ValidateFileSize(IFormFile file) {
			if (file.Length > 750 * 1000) {
				//Max file size: 750kb
				return false;
			} else {
				return true;
			}
		}

		private bool ValidateFileName(IFormFile file) {
			foreach (string ending in allowedFiles) {
				if (file.FileName.ToString().ToLower().EndsWith(ending)) {
					return true;
				}
			}
			return false;
		}

		[ValidateAntiForgeryToken]
		[Authorize]
		[Route("Favorite/{id:int}")]
		public IActionResult Favorite(int id) {
			FavoriteCertificate model = new FavoriteCertificate {
				certificateId = id,
				UserId = userManager.GetUserId(User)
			};
			favoriteHandler.ToggleFavorite(model);
			return RedirectToAction("User");
		}

		[ValidateAntiForgeryToken]
		[Authorize]
		[Route("Certificate/Delete/{id:int}")]
		public async Task<IActionResult> DeleteAsync(int id) {
			if (await AuthorizeOwnerAsync(id)) {
				CertificateHandler.Delete(id);
			}
			return RedirectToAction("User");
		}

		public async Task<bool> AuthorizeOwnerAsync(int id) {
			var certificateAuthor = await CertificateHandler.GetAuthorByIdAsync(id);
			var user = await userManager.FindByNameAsync(User.Identity.Name);

			if (certificateAuthor != null && certificateAuthor.Email == user.Email) { return true; }
			Console.WriteLine("not author");

			return false;
		}

		public byte[] FileToBytes(IFormFile file) {
			MemoryStream ms = new MemoryStream();
			file.CopyTo(ms);
			var memoryStream = ms.ToArray();
			ms.Close();
			ms.Dispose();

			return memoryStream;
		}

		public bool HasCertificate(int certificateId) {
			var certificate = CertificateHandler.GetById(certificateId);
			if (certificate.CertificateFile != null || certificate.CertificateUrl != null) {
				return true;
			}
			return false;
		}

		public string GetImageUrl(byte[] image) {
			string imageDataBytes = Convert.ToBase64String(image);
			string imageUrl = string.Format("data:/image/jpeg;base64,{0}", imageDataBytes);
			return imageUrl;
		}
	}
}