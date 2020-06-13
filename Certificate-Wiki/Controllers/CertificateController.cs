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

namespace Certificate_Wiki.Controllers {

	public class CertificateController : Controller {
		private ICertificateHandler CertificateHandler;
		private readonly UserManager<CertificateUser> userManager;
		private readonly IFavoriteHandler favoriteHandler;

		public CertificateController(ICertificateHandler certificateHandler, UserManager<CertificateUser> userManager, IFavoriteHandler favoriteHandler) {
			this.CertificateHandler = certificateHandler;
			this.userManager = userManager;
			this.favoriteHandler = favoriteHandler;
		}

		[HttpGet]
		[Route("Certificate/View/{id:int}")]
		public async Task<IActionResult> IndexAsync(int id) {
			var model = new CertificateIndex { IsOwner = false };
			var certificate = CertificateHandler.GetById(id);

			if (certificate == null) { return RedirectToAction("Index", "Home"); }
			model.Certificate = certificate;
			model.CertificateOwner = await userManager.FindByIdAsync(certificate.UserFk);

			if (model.CertificateOwner == null) { return RedirectToAction("Index", "Home"); }
			if (model.CertificateOwner.UserName == User.Identity.Name) { model.IsOwner = true; }

			if (model.Certificate.CertificateFile != null) {
				model.CertificateUrl = GetImageUrl(model.Certificate);
				model.CertificateFile = null;
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
		public async Task<IActionResult> UserAsync(CertificateViewModel model) {
			CertificateViewModel certificates = new CertificateViewModel();
			CertificateUser user = await userManager.FindByNameAsync(User.Identity.Name);
			certificates.certificateList = CertificateHandler.GetByUserId(user.Id);
			return View(certificates);
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
				model.CertificateUrl = null;
				model.CertificateFile = FileToBytes(file);
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

		public string GetImageUrl(Certificates certificate) {
			string imageDataBytes = Convert.ToBase64String(certificate.CertificateFile);
			string imageUrl = string.Format("data:/image/jpeg;base64,{0}", imageDataBytes);
			return imageUrl;
		}
	}
}