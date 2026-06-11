
using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class DonHang
    {
        [Key]
        public int MaDonHang { get; set; }
        public int MaNguoiDung { get; set; }
        public DateTime NgayDatHang { get; set; }
        public decimal TongTienHoaDon { get; set; }
        public string DiaChiGiaoHang { get; set; } = string.Empty;
        public string TrangThai { get; set; } = string.Empty;
        public string PhuongThucThanhToan{get; set; } = string.Empty;

        public string TrangThaiThanhToan { get; set; } =string.Empty;
        public string? MaGiaoDich { get; set; }
    }
}