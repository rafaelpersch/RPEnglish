using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RPEnglish.MVC.DatabaseContext;
using RPEnglish.MVC.Entities;
using RPEnglish.MVC.Tools;

namespace RPEnglish.MVC
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSession(options =>
            {
                options.Cookie.Name = "RPEnglish.MVC.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(700); //QUASE 12 HORAS
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            services.AddControllersWithViews();

            services.AddDbContext<MyDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            services.AddScoped<CustomAuthorizationFilter>();
            
            //services.AddHttpContextAccessor();

            services.AddRazorPages().AddRazorRuntimeCompilation(); //refresh views cshtml
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            var optionsBuilder = new DbContextOptionsBuilder<MyDbContext>().UseSqlite(Configuration.GetConnectionString("Sqlite"));

            using (var myDbContext = new MyDbContext(optionsBuilder.Options))
            {
                var user = myDbContext.Users.FirstOrDefault();

                if (user == null)
                {
                    User firstUser = new User()
                    {
                        Id = Guid.NewGuid(),
                        Name = "teste",
                        Email = "teste@teste.com",
                    };

                    firstUser.Validate();
                    myDbContext.Users.Add(firstUser);

                    UserPassword userPassword = new UserPassword()
                    {
                        Id = firstUser.Id,
                        UserId = firstUser.Id,
                        Password = Cryptography.Encrypt(firstUser.Email + "teste"),
                    };

                    userPassword.Validate();
                    myDbContext.UsersPassword.Add(userPassword);

                    myDbContext.SaveChanges();
                }
            }
        }
    }
}