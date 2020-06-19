using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Services {

	public class EmailAuthOptions {
		public string SendGridUser { get; set; }
		public string SendGridKey { get; set; }
	}
}