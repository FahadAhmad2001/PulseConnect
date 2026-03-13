using PulseConnectServer.Utilities;
using PulseConnectServer.Utilities.DatabaseContexts;
using Scalar.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;
public partial class Program
{
    private static void Main(string[] args)
    {
        Log.StartLogging();
        //AuthenticationManager.LoadUserDetails();
        //StandAloneRRTManager.CreateDB();
        var builder = WebApplication.CreateBuilder(args);
        


        // Add services to the container.
        builder.Services.AddSingleton<IAuthenticationManager, AuthenticationManager>();
        builder.Services.AddControllers();
        builder.Services.AddDbContext<IntegratedServerUsersDBContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("IntegratedDBConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("IntegratedDBConnection")),
            options => options.SchemaBehavior(MySqlSchemaBehavior.Translate, ((schema, entity) => $"{schema ?? "dbo"}_{entity}"))));

        builder.Services.AddDbContext<StandAloneRRTMembersDBContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("StandaloneDBConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("StandaloneDBConnection"))));

        builder.Services.AddDbContext<StandAloneRRTEventsDBContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("StandaloneDBConnection"),
            ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("StandaloneDBConnection"))));
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
