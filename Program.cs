using ManageNotification.Data;
using ManageNotification.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// to enter record into database 
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Projects.Any())
    {
        db.Projects.AddRange(
            new ProjectModel { Name = "Project-1" },
            new ProjectModel { Name = "Project-2" },
            new ProjectModel { Name = "Project-3" },
            new ProjectModel { Name = "Project-4" },
            new ProjectModel { Name = "Project-5" },
            new ProjectModel { Name = "Project-6" },
            new ProjectModel { Name = "Project-7" }
        );
        db.SaveChanges();
    }
}

// default user username and password
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    if (!db.Users.Any())
    {
        db.Users.Add(new UserModel
        {
            Username = "admin",
            Password = "admin",
            IsActive = true
        });
        db.SaveChanges();
    }
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
