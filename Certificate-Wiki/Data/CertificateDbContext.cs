using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Certificate_Wiki.Data {

	public class CertificateDbContext : IdentityDbContext<CertificateUser> {

		public CertificateDbContext(DbContextOptions<CertificateDbContext> options) : base(options) {
			Database.EnsureCreated();
		}
	}
}