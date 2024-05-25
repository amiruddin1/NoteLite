using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteLite.Controllers;
using NoteLite.Interface;
using NoteLite.Models;
using NoteLite.Repository;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<NoteDBContext>
    (option => option.UseSqlServer(builder.Configuration.GetConnectionString("default")));
builder.Services.AddIdentity<User, IdentityRole>()
    .AddEntityFrameworkStores<NoteDBContext>()
    .AddDefaultTokenProviders();
builder.Services.AddTransient<UserInterface, UserRepository>();
builder.Services.AddTransient<CategoryInterface,  CategoryRepository>();
builder.Services.AddTransient<NoteInterface, NoteRepository>();
builder.Services.AddSignalR();
builder.Services.AddHostedService<ReminderController>();

var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    //app.UseExceptionHandler("/Home/Error");
    //app.UseExceptionHandler("/H");
    app.UseStatusCodePagesWithRedirects("/Error/{0}");
    app.UseExceptionHandler("/Error/404");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Authentication}/{action=Login}/{id?}");
});

app.Run(async Context =>
{
    Context.Response.Redirect("/Error/NotFound404");
});

//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Authentication}/{action=Login}/{id?}");

app.Run();
