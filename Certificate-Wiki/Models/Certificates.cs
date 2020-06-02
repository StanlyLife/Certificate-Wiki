using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Certificate_Wiki.Models {

	public class Certificates {

		//TODO
		//Add model requirements
		public Certificates() {
		}

		[Key]
		public int CertificateId { get; set; }

		[ForeignKey("CertificateUser")]
		public string UserFk { get; set; }

		public string CertificateName { get; set; }
		public string Subject { get; set; }
		public string Admissioner { get; set; }
		public string Description { get; set; }

		//File OR Url
		public byte[] CertificateFile { get; set; }

		public string CertificateUrl { get; set; }
	}
}