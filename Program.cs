using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using QL_BANTHUCPHAM.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Cấu hình Database kết nối thẳng tới SQL Server bằng chuỗi kết nối của bạn
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer("Server=MSI;Database=QL_BanThucPham;Trusted_Connection=True;TrustServerCertificate=True;"));

// Thêm dịch vụ Xác thực bằng Cookie cho chức năng Đăng ký / Đăng nhập
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Đường dẫn đến trang đăng nhập nếu chưa login
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Thời gian hết hạn cookie (20 phút)
    });
var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication(); // Thêm dòng này (Xác thực)
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=SanPham}/{action=Index}/{id?}")
    .WithStaticAssets();
app.UseStaticFiles();

app.Run();
