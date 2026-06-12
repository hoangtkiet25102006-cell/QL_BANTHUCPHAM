using Microsoft.EntityFrameworkCore;

namespace QL_BANTHUCPHAM.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ánh xạ các bảng tiếng Việt từ SQL Server vào code C#
        public DbSet<DanhMuc> DanhMuc { get; set; }
        public DbSet<SanPham> SanPham { get; set; }
        public DbSet<NguoiDung> NguoiDung { get; set; }
        
        public DbSet<DongGioHang> DongGioHang { get; set; }
        public DbSet<DonHang> DonHang { get; set; }
        public DbSet<ChiTietDonHang> ChiTietDonHang { get; set; }
        public DbSet<TichDiem> TichDiem { get; set; }
        public DbSet<Voucher> Voucher { get; set; }
        public DbSet<DanhGia> DanhGia { get; set; }
        public DbSet<ThongBao> ThongBao { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Cấu hình tên bảng thủ công để khớp chính xác với SQL Server nếu cần
            modelBuilder.Entity<DanhMuc>().ToTable("DanhMuc");
            modelBuilder.Entity<SanPham>().ToTable("SanPham");
            modelBuilder.Entity<NguoiDung>().ToTable("NguoiDung");

            modelBuilder.Entity<DongGioHang>().ToTable("GioHang");
            modelBuilder.Entity<DonHang>().ToTable("DonHang");
            modelBuilder.Entity<ChiTietDonHang>().ToTable("ChiTietDonHang");

            modelBuilder.Entity<TichDiem>().ToTable("TichDiem");
            modelBuilder.Entity<Voucher>().ToTable("Voucher");
            modelBuilder.Entity<DanhGia>().ToTable("DanhGia");
            modelBuilder.Entity<ThongBao>().ToTable("ThongBao");
        }
    }
}