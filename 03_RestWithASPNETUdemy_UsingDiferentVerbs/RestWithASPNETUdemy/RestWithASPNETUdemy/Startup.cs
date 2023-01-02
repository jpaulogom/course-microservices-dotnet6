using RestWithASPNETUdemy.Services;
using RestWithASPNETUdemy.Services.Implementations;
using System.Runtime.CompilerServices;

namespace RestWithASPNETUdemy
{
    public class Startup : IStartup
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

            //Dependency Injection
            services.AddScoped<IPersonService, PersonServiceImplementation>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(WebApplication app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();
        }
    }

    public interface IStartup
    {        
        void ConfigureServices(IServiceCollection services);
        void Configure(WebApplication app, IWebHostEnvironment env);
    }

    public static class StartupExtensions
    {
        public static WebApplicationBuilder UseStartup<TStartup>(this WebApplicationBuilder  WebAppBuilder) where TStartup : IStartup
        {
            var startup = Activator.CreateInstance(typeof(TStartup), WebAppBuilder.Configuration) as IStartup;

            if (startup == null) throw new ArgumentException("Startup.cs Class is Invalid!");

            startup.ConfigureServices(WebAppBuilder.Services);

            var app = WebAppBuilder.Build();
            startup.Configure(app, app.Environment);

            app.Run();

            return WebAppBuilder;
        }
    }
}
