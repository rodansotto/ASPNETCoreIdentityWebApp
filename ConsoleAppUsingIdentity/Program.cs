// See https://aka.ms/new-console-template for more information
//Console.WriteLine("Hello, World!");

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateDefaultBuilder(args);
var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
var connectionString = config.GetValue<string>("ConnectionStrings:ASPNETCoreIdentityWebAppContextConnection") ?? throw new InvalidOperationException("Connection string 'ASPNETCoreIdentityWebAppContextConnection' not found.");
Console.WriteLine(connectionString);

builder.ConfigureServices(services =>
{
    services.AddDbContext<ASPNETCoreIdentityWebAppContext>(options =>
        options.UseSqlServer(connectionString));

    services.AddIdentityCore<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ASPNETCoreIdentityWebAppContext>();
});

var host = builder.Build();

var userManager = host.Services.GetRequiredService<UserManager<IdentityUser>>();
var logger = host.Services.GetRequiredService<ILogger<Program>>();
var user = await userManager.FindByEmailAsync("john.smith@email.com");
var result = await userManager.CheckPasswordAsync(user, "xPassword1!");
if (result)
{
    logger.LogInformation("User logged in.");
}
else
{
    logger.LogInformation("Invalid login attempt.");
}