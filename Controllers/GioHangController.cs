using Microsoft.AspNetCore.Mvc;
using QL_BANTHUCPHAM.Models;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace QL_BANTHUCPHAM.Controllers
{
    [Authorize] // Yêu cầu đăng nhập mới được vào giỏ hàng
    
    public class GioHangController : Controller
    {
        private readonly AppDbContext _context;

        private int GetSoLuongTon(int maSanPham)
        {
            // Tìm sản phẩm trong bảng SanPham (thay 'SanPham' bằng tên bảng của bạn)
            var sanPham = _context.SanPham.FirstOrDefault(s => s.MaSanPham == maSanPham);
            
            // Trả về số lượng nếu tìm thấy, nếu không thì trả về 0
            return sanPham != null ? sanPham.SoLuongTonKho : 0;
        }

        public GioHangController(AppDbContext context)
        {
            _context = context;
        }

        // 1. Hiển thị trang giỏ hàng chi tiết
        public IActionResult Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                    return RedirectToAction("Login", "Account");

                int userIdInt = int.Parse(userId);
                var items = _context.DongGioHang
                    .Where(x => x.MaNguoiDung == userIdInt)
                    .Join(_context.SanPham,
                        cart => cart.MaSanPham,
                        prod => prod.MaSanPham,
                        (cart, prod) => new { cart, prod })
                    .Select(x => new CartItemViewModel
                    {
                        MaSanPham = x.prod.MaSanPham,
                        TenSanPham = x.prod.TenSanPham ?? "san pham khong ten",
                        GiaBan = x.prod.GiaBan,
                        SoLuong = x.cart.SoLuongChon,  
                        HinhAnh = x.prod.HinhAnh ?? "Default.jpg"
                    }).ToList();

                return View(items);
            }
            catch (Exception ex)
            {
                // Hiện lỗi thẳng ra màn hình để debug
                return Content("LỖI: " + ex.Message + "\n\n" + ex.StackTrace);
            }
        }

        // 2. AJAX: Thêm vào giỏ hàng (Cập nhật số lượng trên icon)
        [AllowAnonymous]
        [HttpPost]
        public IActionResult UpdateCart(int maSanPham, int delta)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var item = _context.DongGioHang
                .FirstOrDefault(x => x.MaNguoiDung == userId && x.MaSanPham == maSanPham);

            if (item != null)
            {
                item.SoLuongChon += delta; // delta là +1 (tăng) hoặc -1 (giảm)
                
                if (item.SoLuongChon <= 0)
                    _context.DongGioHang.Remove(item);
                    
                _context.SaveChanges();
            }
            // Trả về tổng số lượng mới để update icon và đơn giá cho mượt mà
            int total = _context.DongGioHang.Where(x => x.MaNguoiDung == userId).Sum(x => x.SoLuongChon);
            return Json(new { success = true, 
                        totalItems = total,});
        }
        [HttpPost]
        public IActionResult AddToCart(int id)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null) 
                {
                    return Json(new { success = false, message = "Bạn chưa đăng nhập!" });
                }
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Unauthorized();
            int userId = int.Parse(userIdStr);
            // Kiểm tra sản phẩm đã có trong giỏ chưa
            var cartItem = _context.DongGioHang
                .FirstOrDefault(x => x.MaNguoiDung == userId && x.MaSanPham == id);

            if (cartItem != null)
            {
                cartItem.SoLuongChon += 1;
            }
            else
            {
                _context.DongGioHang.Add(new DongGioHang {
                    MaNguoiDung = userId,
                    MaSanPham = id,
                    SoLuongChon = 1
                });
            }
            
            _context.SaveChanges();
            int totalMoi = _context.DongGioHang  // Tính lại SAU khi lưu
                .Where(x => x.MaNguoiDung == userId)
                .Sum(x => x.SoLuongChon);
            int SoLuongTonMoi = GetSoLuongTon(id); // Lấy lại tồn kho MỚI sau khi cập nhật
            return Json(new {  success = true,
                            totalItems = totalMoi,
                            TonKhoMoi = SoLuongTonMoi });
            }

        // 3. AJAX: Lấy số lượng giỏ hàng ban đầu khi load trang
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetCartCount()
        {
            var userIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdStr)) return Json(new { totalItems = 0 });

            int userId = int.Parse(userIdStr);
            int total = _context.DongGioHang
                .Where(x => x.MaNguoiDung == userId)
                .Sum(x => x.SoLuongChon);

            return Json(new { totalItems = total });
        }
    }
    
    // ViewModel phụ trợ để hiển thị giỏ hàng đẹp hơn
    public class CartItemViewModel
    {
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public decimal GiaBan { get; set; }
        public int SoLuong { get; set; }
        public string HinhAnh { get; set; } = string.Empty;
        public decimal ThanhTien => GiaBan * SoLuong;
    }
    
}