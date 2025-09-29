using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace OcelotPrototype.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

            builder.Configuration
               .SetBasePath(builder.Environment.ContentRootPath)
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            if (builder.Environment.IsDevelopment())
            {
                builder.Configuration
                    .SetBasePath(builder.Environment.ContentRootPath)
                    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
            }
            AddOcelotConfigurationFiles(builder);
            builder.Configuration.AddEnvironmentVariables();
            builder.Services
                .AddOcelot(builder.Configuration)
                .AddDelegatingHandler<MyReplaceTokenAuthorizationDelegatingHandler>(global: true);


            // Add services to the container.
            builder.Services.AddAuthorization();

            /////////////////////////////////////////////////////////////////////
            WebApplication app = builder.Build();

            // Configure the HTTP request pipeline.

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseRouting();
            //app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.MapWhen(
                (ctx) => !ctx.Request.Path.StartsWithSegments("/monitoring") &&
                !ctx.Request.Path.StartsWithSegments("/metrics"),
                async (app) =>
                {
                    await app.UseOcelot();
                });
            app.Run();
        }

        private static void AddOcelotConfigurationFiles(WebApplicationBuilder builder)
        {
            builder.Configuration
                .SetBasePath(builder.Environment.ContentRootPath)
                .AddOcelot();
        }

    }
}
