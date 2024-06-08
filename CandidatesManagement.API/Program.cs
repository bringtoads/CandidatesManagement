using CandidatesManagement.API;
using CandidatesManagement.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Serilog;

Log.Logger = new LoggerConfiguration()
       .MinimumLevel.Debug()
       .WriteTo.Console()
       .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Day)
       .CreateLogger();

try
{
    Log.Information("Starting web host");

    var builder = WebApplication.CreateBuilder(args);
    {
        builder.Host.UseSerilog((context, services, configuration) => configuration
       .ReadFrom.Configuration(context.Configuration)
       .ReadFrom.Services(services)
       .Enrich.FromLogContext());

        // Add services to the container.
        builder.Services
            .AddPresentationCore()
            .AddInfrastrructureCore();
      
    }

    var app = builder.Build();
    {
        app.UseSerilogRequestLogging(configure =>
        {
            configure.MessageTemplate = "HTTP {RequestMethod} {RequestPath} ({UserId}) responded {StatusCode} in {Elapsed:0.0000}ms";
        });
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var pd = new ProblemDetails
                    {
                        Type = "https://demo.api.com/errors/internal-server-error",
                        Title = "An unrecoverable error occurred",
                        Status = StatusCodes.Status500InternalServerError,
                        Detail = "This is a demo error used to demonstrate problem details",
                    };
                    pd.Extensions.Add("RequestId", context.TraceIdentifier);
                    await context.Response.WriteAsJsonAsync(
                        pd,
                        pd.GetType(),
                        options: null, 
                        contentType: "application/problem+json"
                    );
                });
            });
        }
 

        app.Run();
    }

}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

return 0;
