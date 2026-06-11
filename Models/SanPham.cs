using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QL_BANTHUCPHAM.Models
{
    public class SanPham
    {
        [Key]
        public int MaSanPham { get; set; }
        public string TenSanPham { get; set; } = string.Empty;
        public decimal GiaBan { get; set; }
        public string? HinhAnh { get; set; }
        public int SoLuongTonKho { get; set; }
        public string? MoTaChiTiet { get; set; }
        public DateTime? HanSuDung { get; set; }

        public int? MaDanhMuc { get; set; }
        [ForeignKey("MaDanhMuc")]
        public DanhMuc? DanhMuc { get; set; }
    }
}