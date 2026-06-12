using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }
        public int? MaNguoiDung { get; set; } // NULL = gửi tất cả
        public string TieuDe { get; set; } = string.Empty;
        public string NoiDung { get; set; } = string.Empty;
        public string LoaiThongBao { get; set; } = string.Empty; // 'DON_HANG', 'KHUYEN_MAI', 'HE_THONG'
        public bool DaDoc { get; set; } = false;
        public DateTime NgayTao { get; set; } = DateTime.Now;
    }
}