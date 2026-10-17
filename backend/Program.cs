
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Tracker.Data;

Env.Load();

string connectionString =
    Environment.GetEnvironmentVariable("TRACKER_CONNECTION_STRING")
    ?? throw new InvalidOperationException(
        "TRACKER_CONNECTION_STRING was not found.");

var services = new ServiceCollection();

services.AddDbContext<TrackerDbContext>(options =>
    options.UseNpgsql(connectionString));

using var serviceProvider = services.BuildServiceProvider();
using var scope = serviceProvider.CreateScope();

var context = scope.ServiceProvider
    .GetRequiredService<TrackerDbContext>();

bool connected = await context.Database.CanConnectAsync();

Console.WriteLine(connected
    ? "Database connection successful."
    : "Database connection failed.");
