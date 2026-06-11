using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class ChiTietDonHang
    {
        [Key]
        public int MaChiTiet { get; set; }
        public int MaDonHang { get; set; }
        public int MaSanPham { get; set; }
        public int SoLuongMua { get; set; }
        public decimal GiaLucMua { get; set; }
    }
}