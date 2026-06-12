using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class TichDiem
    {
        [Key]
        public int MaTichDiem { get; set; }
        public int MaNguoiDung { get; set; }
        public int SoDiem { get; set; }
        public string LoaiGiaoDich { get; set; } = string.Empty;
        public string? MoTa { get; set; }
        public int? MaDonHang { get; set; }
        public DateTime NgayGiaoDich { get; set; }
    }
}