using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Certificate_Wiki.Data;
using Certificate_Wiki.Interface;
using Certificate_Wiki.Interface.Implementation;
using Certificate_Wiki.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Certificate_Wiki {

	public class Startup {

		public Startup(IConfiguration configuration) {
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services) {
			services.AddScoped<ICertificateHandler, CertificateHandler>();
			services.AddScoped<IFavoriteHandler, FavoriteHandler>();
			services.AddControllersWithViews();

			services.AddDbContext<CertificateDbContext>(options => {
				var connectionString = "Data Source=(LocalDb)\\MSSQLLocalDB;" +
											   "database=LocalCertifyDb;" +
											   "trusted_connection=yes;";

				var myMigrationAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
				options.UseSqlServer(connectionString, sql => {
					sql.MigrationsAssembly(myMigrationAssembly);
				});
			});

			services.AddIdentity<CertificateUser, IdentityRole>(options => {
				//Change this before deployment
				options.SignIn.RequireConfirmedEmail = false;

				options.Password.RequireNonAlphanumeric = false;

				options.User.RequireUniqueEmail = true;

				options.Lockout.AllowedForNewUsers = true;
				options.Lockout.MaxFailedAccessAttempts = 5;
				//Increase?
				options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
			}).AddEntityFrameworkStores<CertificateDbContext>().AddDefaultTokenProviders();

			services.ConfigureApplicationCookie(options => options.LoginPath = "/Auth/login");
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
			if (env.IsDevelopment()) {
				app.UseDeveloperExceptionPage();
			} else {
				app.UseExceptionHandler("/Home/Error");
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints => {
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
			});
		}
	}
}