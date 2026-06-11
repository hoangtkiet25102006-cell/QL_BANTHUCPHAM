using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BANTHUCPHAM.Models
{
    public class DongGioHang
    {
        [Key] // Đánh dấu khóa chính
        public int MaDongGioHang { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuongChon { get; set; }
        [ForeignKey("MaSanPham")]
        public virtual SanPham? SanPham { get; set; }
    }
}