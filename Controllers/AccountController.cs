using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using QL_BANTHUCPHAM.Models; 

namespace QL_BANTHUCPHAM.Controllers
{
    public class AccountController : Controller
    {
        private readonly AppDbContext _db; 

        public AccountController(AppDbContext db)
        {
            _db = db;
        }

        // ================= ĐĂNG KÝ =================
        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(NguoiDung user)
        {
            if (ModelState.IsValid)
            {
                // ĐÃ SỬA: Đổi _db.NguoiDungs thành _db.NguoiDung (không có chữ s)
                var check = _db.NguoiDung.FirstOrDefault(s => s.HoTen == user.HoTen);
                if (check == null)
                {
                    _db.NguoiDung.Add(user);
                    _db.SaveChanges();
                    return RedirectToAction("Login");
                }
                else
                {
                    ViewBag.Error = "Tên đăng nhập này đã tồn tại!";
                    return View();
                }
            }
            return View();
        }

        // ================= ĐĂNG NHẬP =================
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            // ĐÃ SỬA: Đổi _db.NguoiDungs thành _db.NguoiDung (không có chữ s)
            var user = _db.NguoiDung.FirstOrDefault(u => u.HoTen == username && u.MatKhauMaHoa == password);

            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.MaNguoiDung.ToString()),
                    new Claim(ClaimTypes.Name, user.HoTen ?? ""),
                    new Claim("FullName", user.HoTen ?? ""), 
                    new Claim(ClaimTypes.Role, user.VaiTro ?? "Khách hàng") 
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                    {
                        IsPersistent = true, // Giúp đăng nhập duy trì ngay cả khi tắt trình duyệt
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                    });

                return RedirectToAction("Index", "SanPham"); 
            }

            ViewBag.Error = "Sai tên đăng nhập hoặc mật khẩu!";
            return View();
        }

        // ================= ĐĂNG XUẤT =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "SanPham");
        }
    }
}