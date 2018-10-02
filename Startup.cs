using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace WeTube
{
	public class Startup
    {
        public Startup(IConfiguration configuration) => Configuration = configuration;

	    public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			// Routing (?)
	        services.AddRouting();

			// Cookies
	        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
		        .AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
	        if (env.IsDevelopment())
		        app.UseDeveloperExceptionPage();
	        else
	        {
		        app.UseExceptionHandler("/Home/Error");
		        app.UseHsts();
	        }

	        //app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

	        app.UseAuthentication();

	        app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

	        app.UseMvc(routes =>
	        {
		        routes.MapRoute(
			        name: "default",
			        template: "{controller=Home}/{action=Index}");

		        routes.MapRoute(
			        name: "Rooms",
			        template: "Rooms/{id?}",
			        defaults: new
			        {
				        controller = "Rooms",
				        action = "Index"
			        });

		        routes.MapRoute(
			        name: "Api",
			        template: "Api/{action}",
			        defaults: new
			        {
				        controller = "Api",
				        action = "Index"
			        }
				);

		        routes.MapRoute(
			        name: "Error",
			        template: "/Home/Error/{code?}",
			        defaults: new
			        {
						controller = "Home",
						action = "Error"
			        });
	        });
		}
    }
}
