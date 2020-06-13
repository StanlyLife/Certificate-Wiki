using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models.Certificate {

	public class CertificateIndex {
		public Certificates Certificate { get; set; }

		public bool IsPrivate { get; set; }
		public bool IsOwner { get; set; }

		public CertificateUser CertificateOwner { get; set; }

		public byte[] CertificateFile { get; set; }
		public string CertificateUrl { get; set; }
	}
}