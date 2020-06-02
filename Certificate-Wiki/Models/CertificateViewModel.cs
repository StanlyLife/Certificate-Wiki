using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Models {

	public class CertificateViewModel {
		public IEnumerable<Certificates> certificateList { get; set; }
	}
}