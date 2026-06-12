using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class Voucher
    {
        [Key]
        public int MaVoucher { get; set; }
        public string MaCode { get; set; } = string.Empty;
        public string TenVoucher { get; set; } = string.Empty;
        public string LoaiGiamGia { get; set; } = string.Empty; // 'PHANTRAM' hoặc 'SOTIEN'
        public decimal GiaTriGiam { get; set; }
        public decimal DonHangToiThieu { get; set; } = 0;
        public int SoLuong { get; set; }
        public int DaSuDung { get; set; } = 0;
        public DateTime NgayBatDau { get; set; }
        public DateTime NgayKetThuc { get; set; }
        public bool IsActive { get; set; } = true;
    }
}