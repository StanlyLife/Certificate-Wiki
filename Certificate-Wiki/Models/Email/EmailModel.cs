using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Email {

	public class EmailModel {

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		public string Subject { get; set; }

		[Required]
		public string Message { get; set; }
	}
}