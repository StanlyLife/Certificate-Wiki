using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models {

	public class CertificateUser : IdentityUser {
		public string Website { get; set; }
		public string Occupation { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Country { get; set; }
		public Byte[] ProfilePicture { get; set; }
	}
}