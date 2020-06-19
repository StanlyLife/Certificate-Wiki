using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Certificate_Wiki.Models.Email;
using Certificate_Wiki.Services;
using Microsoft.AspNetCore.Mvc;

namespace Certificate_Wiki.Controllers {

	public class EmailController : Controller {
		private readonly IEmailSender _emailSender;

		public EmailController(IEmailSender emailSender) {
			_emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult SendEmail() {
			return View();
		}

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