using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using QL_BANTHUCPHAM.Models;
using QL_BANTHUCPHAM.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

[Authorize]
public class ThanhToanController : Controller
{
    private readonly AppDbContext _context;

    public ThanhToanController(AppDbContext context)
    {
        _context = context;
    }


    [HttpGet]
    public IActionResult Index()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        // Thêm .Include(x => x.SanPham) vào đây
        var cartItems = _context.DongGioHang
            .Include(x => x.SanPham) 
            .Where(x => x.MaNguoiDung == userId)
            .ToList();

        return View(cartItems ?? new List<DongGioHang>());
    }
    [HttpPost]
    public IActionResult XacNhanThanhToan(ThanhToanViewModel model)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        
        using (var transaction = _context.Database.BeginTransaction())
        {
            try
            {
                // 1. Tạo đơn hàng
                var donHang = new DonHang
                {
                    MaNguoiDung = userId,
                    NgayDatHang = DateTime.Now,
                    PhuongThucThanhToan = model.PhuongThuc,
                    TrangThai = model.PhuongThuc == "TraSau" ? "Chưa giao hàng" : "Đang xử lý",
                    TrangThaiThanhToan = model.PhuongThuc == "TraTruoc" ? "Đã thanh toán" : "Chưa thanh toán",
                    MaGiaoDich = model.PhuongThuc == "TraTruoc" ? "MBB-" + Guid.NewGuid().ToString().Substring(0, 8).ToUpper() : null,
                    DiaChiGiaoHang = model.DiaChi // Lưu địa chỉ vào đơn hàng
                };

                _context.DonHang.Add(donHang);
                _context.SaveChanges();

                // 2. Chuyển từ giỏ hàng sang chi tiết đơn hàng & trừ kho
                var cartItems = _context.DongGioHang.Include(x => x.SanPham)
                .Where(x => x.MaNguoiDung == userId).ToList();
                                   
                if (!cartItems.Any()) throw new Exception("Giỏ hàng trống!");

                foreach (var item in cartItems)
                {
                    if (item.SanPham != null && item.SanPham.SoLuongTonKho >= item.SoLuongChon)
                    {
                        item.SanPham.SoLuongTonKho -= item.SoLuongChon;
                        _context.ChiTietDonHang.Add(new ChiTietDonHang {
                            MaDonHang = donHang.MaDonHang,
                            MaSanPham = item.MaSanPham,
                            SoLuongMua = item.SoLuongChon,
                            GiaLucMua = item.SanPham.GiaBan
                        });
                    }
                    else throw new Exception($"Sản phẩm {item.SanPham?.TenSanPham} không đủ số lượng!");
                }

                _context.DongGioHang.RemoveRange(cartItems);
                _context.SaveChanges();
                transaction.Commit();

                return RedirectToAction("ThanhCong", new { id = donHang.MaDonHang });
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                ViewBag.Error = ex.Message;
                // Trả về đúng view cũ dựa trên phương thức
                return View(model.PhuongThuc == "TraTruoc" ? "TraTruoc" : "TraSau", model);
            }
        }
    }
    public IActionResult ChonPhuongThuc(string phuongThuc)
    {
        if (phuongThuc == "TraTruoc")
            return RedirectToAction("TraTruoc");
        else
            return RedirectToAction("TraSau");
    }
    // View cho trả trước
    public IActionResult TraTruoc() => View();

    // View cho trả sau
    public IActionResult TraSau() => View();
    public IActionResult ThanhCong(int id)
    {
        ViewBag.MaDonHang = id; // Hiển thị mã đơn hàng cho khách biết
        return View();
    }
}