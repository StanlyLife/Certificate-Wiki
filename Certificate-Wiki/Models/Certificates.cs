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

		[Required(ErrorMessage = "Please enter a certificate name")]
		[MinLength(2, ErrorMessage = "certificate name must be longer than 2 characters")]
		[MaxLength(40, ErrorMessage = "certificate name must be less than 40 characters")]
		public string CertificateName { get; set; }

		[Required(ErrorMessage = "Please enter a subject, eg. computer science")]
		[MinLength(2, ErrorMessage = "subject must be longer than 2 characters")]
		[MaxLength(20, ErrorMessage = "subject must be less than 20 characters")]
		public string Subject { get; set; }

		[Required(ErrorMessage = "Please enter an admissioner eg. udemy, linkedin or your school")]
		[MinLength(2, ErrorMessage = "Admissioner must be longer than 2 characters")]
		[MaxLength(20, ErrorMessage = "Admissioner must be less than 20 characters")]
		public string Admissioner { get; set; }

		[Required(ErrorMessage = "Please enter a certificate description eg. I recieved this certificate by...")]
		[MinLength(10, ErrorMessage = "Description must be longer than 10 characters")]
		[MaxLength(500, ErrorMessage = "Description must be less than 500 characters")]
		public string Description { get; set; }

		//File OR Url
		public byte[] CertificateFile { get; set; }

		public string CertificateFileName { get; set; }

		public string CertificateUrl { get; set; }
	}
}