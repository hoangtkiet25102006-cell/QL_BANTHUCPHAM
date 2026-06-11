using System.ComponentModel.DataAnnotations;

namespace QL_BANTHUCPHAM.Models
{
    public class DanhMuc
    {
        [Key]
        public int MaDanhMuc { get; set; }
        public string TenDanhMuc { get; set; } = string.Empty;
        public string? MoTa { get; set; }

        // Mối quan hệ: Một danh mục có nhiều sản phẩm
        public ICollection<SanPham> SanPhams { get; set; } = new List<SanPham>();
    }
}