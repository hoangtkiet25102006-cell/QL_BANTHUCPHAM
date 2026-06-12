using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class DanhGia
    {
        [Key]
        public int MaDanhGia { get; set; }
        public int MaNguoiDung { get; set; }
        public int MaSanPham { get; set; }
        public int SoSao { get; set; } // 1 đến 5
        public string? NoiDung { get; set; }
        public DateTime NgayDanhGia { get; set; } = DateTime.Now;
        public bool IsHienThi { get; set; } = true;
    }
}