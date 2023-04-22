using Microsoft.EntityFrameworkCore;
using SpaceShop_DataMigrations;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.UI.Services;
using SpaceShop_Utility;
using ShopM4_Utility.BrainTree;

using SpaceShop_DataMigrations.Repository.IRepository;
using SpaceShop_DataMigrations.Repository;
using SpaceShop_Utility.BrainTree;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "Winter";
    //options.IdleTimeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddHttpContextAccessor();
//builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddDefaultUI().AddDefaultTokenProviders().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication().AddGoogle(googleOptions =>
{
    googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
    googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
});
builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "157162407301753";
    options.AppSecret = "93231d072bdf20af32d3662db9649415";
});

builder.Services.AddTransient<IEmailSender, EmailSender>();

builder.Services.AddScoped<IRepositoryCategory, RepositoryCategory>();
builder.Services.AddScoped<IRepositoryMyModel, RepositoryMyModel>();
builder.Services.AddScoped<IRepositoryProduct, RepositoryProduct>();
builder.Services.AddScoped<IRepositoryConnectionProductMyModel, RepositoryConnectionProductMyModel>();
builder.Services.AddScoped<IRepositoryQueryHeader, RepositoryQueryHeader>();
builder.Services.AddScoped<IRepositoryQueryDetail, RepositoryQueryDetail>();
builder.Services.AddScoped<IRepositoryOrderHeader, RepositoryOrderHeader>();
builder.Services.AddScoped<IRepositoryOrderDetail, RepositoryOrderDetail>();
builder.Services.AddScoped<IRepositoryApplicationUser, RepositoryApplicationUser>();

builder.Services.AddDbContext<ApplicationDbContext>(
    options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
//   .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.Configure<SettingsBrainTree>(builder.Configuration.GetSection("BrainTree"));
builder.Services.AddSingleton<IBrainTreeBridge, BrainTreeBridge>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.MapRazorPages();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

/*app.Use((context, next) =>
{
    context.Items["name"] = "Dany";
    return next.Invoke();
});*/

app.UseSession(); //���������� �������������� �� ��� ������ � ��������

/*app.Run(x =>
{
    //return x.Response.WriteAsync("Hello " + x.Items["name"]);
    if (x.Session.Keys.Contains("name"))
    {
        return x.Response.WriteAsync("OK " + x.Session.GetString("name"));
    }
    else
    {
        x.Session.SetString("name", "Anton");
        return x.Response.WriteAsync("NO");
    }
});*/

app.Run();
