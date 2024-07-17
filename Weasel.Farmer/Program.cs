using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Weasel.Farmer.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
string? connectionString = builder.Configuration.GetConnectionString("Connection");
builder.Services.AddDbContext<FarmerDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsql =>
    {
        npgsql.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
    });
    if (!builder.Environment.IsProduction())
    {
        options.EnableSensitiveDataLogging();
    }
});

if (!builder.Environment.IsProduction())
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();
}

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<FarmerDbContext>();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
