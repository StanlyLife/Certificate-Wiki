using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Certificate_Wiki.Models {

	public class CertificateUser : IdentityUser {

		[MinLength(2, ErrorMessage = "First name must be longer than 2 characters")]
		[MaxLength(20, ErrorMessage = "First name must be less than 20 characters")]
		public string FirstName { get; set; }
		[MaxLength(20, ErrorMessage = "Last name must be less than 64 characters")]
		public string LastName { get; set; }
		[MaxLength(30, ErrorMessage = "website url must be less than 30 characters")]
		public string Website { get; set; }

		[MinLength(2, ErrorMessage = "Please enter your occupation. eg. student")]
		[MaxLength(25, ErrorMessage = "Occupation must be less than 25 characters")]
		public string Occupation { get; set; }
		[MaxLength(63, ErrorMessage = "Country must be less than 63 characters")]
		public string Country { get; set; }
		[MinLength(10, ErrorMessage = "Please tell about yourself with a minimum of 10 characters ")]
		[MaxLength(2000, ErrorMessage = "Description must be less than 2000 characters")]
		public string Description { get; set; }

		[NotMapped]
		public String ProfilePictureUrl { get; set; }

		public Byte[] ProfilePicture { get; set; }
		public bool isPrivate { get; set; }
	}
}