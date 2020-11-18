using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using IdentityServer4.AccessTokenValidation;

namespace KaercherNet.API
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
            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed(origin => origin == "http://localhost:4200")
                    .AllowCredentials();
                });
            });

            //services.AddAuthentication("Bearer").AddJwtBearer("Bearer", options =>
            //{
            //    options.Authority = "http://localhost:5000";
            //    options.Audience = "kaerchernetApi";
            //    options.RequireHttpsMetadata = false;
            //    options.TokenValidationParameters = new TokenValidationParameters()
            //    {
            //        ValidateAudience = false
            //    };
            //});
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                       .AddIdentityServerAuthentication(options =>
                       {
                           options.Authority = "http://localhost:5000";
                           options.ApiName = "kaerchernet-api";
                           options.RequireHttpsMetadata = false;
                       });

            services.AddDbContext<KaercherNetDbContext>(options => options.UseInMemoryDatabase("KaercherNetDbContext"));
            services.AddControllers();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "KaercherNet API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("AllRequests");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "KaercherNet API");
            });
        }
    }
}
