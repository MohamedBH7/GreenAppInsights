using GreenAppInsights.Data;
using GreenAppInsights.Middleware;
using GreenAppInsights.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

// ✅ Register GreenAppInsights services
builder.Services.AddScoped<MetricsCollector>();
builder.Services.AddScoped<EnergyEstimator>();
builder.Services.AddScoped<OptimizationEngine>();
builder.Services.AddScoped<ReportService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{

}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

    app.UseMigrationsEndPoint();
    app.UseSwagger();

    app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Use Metrics Middleware
app.UseMiddleware<MetricsTrackingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// ✅ Set Dashboard as startup controller
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Dashboard}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
