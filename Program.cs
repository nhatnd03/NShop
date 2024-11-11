using app1.Helper;
using app1.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Hshop2023Context>(option =>
{
	option.UseSqlServer(builder.Configuration.GetConnectionString("con"));
});
builder.Services.AddMemoryCache();
builder.Services.AddSession(option =>
{
	option.IdleTimeout = TimeSpan.FromMinutes(20);
	option.Cookie.HttpOnly = true;
	option.Cookie.IsEssential = true;
});
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{
	options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
	options.SlidingExpiration = true ;
	options.LoginPath = "/KhachHang/DangNhap";
	options.AccessDeniedPath = "/AcessDenied";
});

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

app.UseRouting();
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();


app.MapDefaultControllerRoute();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=HangHoa}/{action=Index}/{id?}");

app.Run();
