using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models.Email;
using Certificate_Wiki.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Certificate_Wiki.Controllers {

	public class EmailController : Controller {
		private readonly IEmailSender _emailSender;

		public EmailController(IEmailSender emailSender) {
			_emailSender = emailSender;
		}

		[Authorize(Policy = "Owner")]
		[HttpGet]
		public IActionResult SendEmail() {
			return View();
		}

		[Authorize(Policy = "Owner")]
		[ValidateAntiForgeryToken]
		[HttpPost]
		public async Task<ViewResult> SendEmail(EmailModel model) {
			if (ModelState.IsValid) {
				var emails = new List<string>();
				emails.Add(model.Email);
				await _emailSender.SendEmailAsync(emails, model.Subject, model.Message);
				Console.WriteLine("email sent");
			}
			return View(model);
		}
	}
}