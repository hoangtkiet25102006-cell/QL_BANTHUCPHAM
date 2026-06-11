using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QL_BANTHUCPHAM.Models;

namespace QL_BANTHUCPHAM.Controllers
{
    public class SanPhamController : Controller
    {
        private readonly AppDbContext _context;

        public SanPhamController(AppDbContext context)
        {
            _context = context;
        }

        // 1. TRANG TRUNG TÂM: Hiện tất cả Danh mục đồ ăn dưới dạng Card
        public async Task<IActionResult> Index()
        {
            var cacDanhMuc = await _context.DanhMuc.ToListAsync();
            return View(cacDanhMuc);
        }

        // 2. TRANG SẢN PHẨM THEO LOẠI: Hiện danh sách món ăn khi ấn vào Card
        // Lấy số "id" (chính là MaDanhMuc) từ URL truyền xuống
        public async Task<IActionResult> TheoLoai(int id)
        {
            var danhMucDuocChon = await _context.DanhMuc
                .Include(dm => dm.SanPhams) // Nạp kèm các sản phẩm của danh mục đó
                .FirstOrDefaultAsync(dm => dm.MaDanhMuc == id);

            if (danhMucDuocChon == null)
            {
                return NotFound();
            }

            return View(danhMucDuocChon);
        }
    }
}