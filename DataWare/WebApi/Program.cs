
using Serilog;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            DataAccess.ServiceLocator.RegisterDataAccessServices(builder.Services, builder.Configuration);
            Infrastructure.ServiceLocator.RegisterInfastructureServices(builder.Services);
            Application.ServiceLocator.RegisterApplicationServices(builder.Services);

            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration));

            builder.Services.AddAutoMapper(
                DataAccess.AssemblyReference.Assembly, 
                Infrastructure.AssemblyReference.Assembly,
                Application.AssemblyReference.Assembly);

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
            });

            var app = builder.Build();

            if (app.Configuration.GetValue("ConnectionSettings:MigrateDatabase", false))
            {
                using IServiceScope scope = app.Services.CreateScope();
                DataAccess.Initializers.DbInitialzier.ApplyMigration(scope);
            }

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.MapControllers();

            app.Run();
        }
    }
}
