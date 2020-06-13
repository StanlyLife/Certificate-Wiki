using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Certificate {

	public class FavoriteCertificate {

		[Key]
		public int Id { get; set; }

		[NotNull]
		[ForeignKey("Certificates")]
		public int certificateId { get; set; }

		[NotNull]
		[ForeignKey("CertificateUser")]
		public string UserId { get; set; }
	}
}