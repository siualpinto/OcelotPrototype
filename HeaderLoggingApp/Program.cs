namespace HeaderLoggingApp;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        if (builder.Environment.IsDevelopment())
        {
            builder.Logging.AddConsole();
        }
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        // app.UseHttpsRedirection();
        app.UseAuthorization();
        app.UseMiddleware<HeaderLoggingMiddleware>();

        app.MapControllers();
        app.Run();
    }
}
