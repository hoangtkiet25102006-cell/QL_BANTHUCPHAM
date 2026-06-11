using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class NguoiDung
    {
        [Key]
        public int MaNguoiDung { get; set; } // Khớp với MaNguoiDung (Khóa chính)

        public string? HoTen { get; set; } = string.Empty; // Khớp với HoTen

        public string? Email { get; set; } // Khớp với Email

        // ĐÃ SỬA: Đổi TenDangNhap thành Email vì database của bạn lấy Email làm tài khoản đăng nhập
        public string? MatKhauMaHoa { get; set; } // Khớp chính xác với cột MatKhauMaHoa

        public string? SoDienThoai { get; set; } // Khớp chính xác với cột SoDienThoai

        public string? DiaChi { get; set; } // Khớp chính xác với cột DiaChi

        public string? VaiTro { get; set; } // Khớp chính xác với cột VaiTro
    }
}