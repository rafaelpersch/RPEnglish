using System;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RPEnglish.API.DatabaseContext;
using RPEnglish.API.Tools;
using Microsoft.IdentityModel.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using RPEnglish.API.Entities;

namespace RPEnglish.API
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
            services.AddControllers();

            services.AddSwaggerGen(c => {

                c.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "RPEnglish.API",
                        Version = "v1",
                        Description = "API REST criada com o ASP.NET Core 3.1",
                        Contact = new OpenApiContact
                        {
                            Name = "Rafael Persch",
                            Url = new Uri("https://github.com/rafaelpersch")
                        }
                    });
            });

            services.AddDbContext<MyDbContext>(options => options.UseSqlite(Configuration.GetConnectionString("Sqlite")));

            var key = Encoding.ASCII.GetBytes(TokenService.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });

            IdentityModelEventSource.ShowPII = true; //To show detail of error and see the problem
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // Ativando middlewares para uso do Swagger
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "RPEnglish.API V1");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
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