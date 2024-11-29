using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using TECH.Data.DatabaseEntity;
using TECH.Reponsitory;
using TECH.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.PropertyNamingPolicy = null;
    o.JsonSerializerOptions.DictionaryKeyPolicy = null;
});
builder.Services.AddControllers();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

builder.Services.AddDbContext<DataBaseEntityContext>(options =>
{
    // Đọc chuỗi kết nối
    string connectstring = builder.Configuration.GetConnectionString("AppDbContext");
    options.UseSqlServer(connectstring);
});
builder.Services.AddScoped(typeof(IUnitOfWork), typeof(EFUnitOfWork));
builder.Services.AddScoped(typeof(IRepository<,>), typeof(EFRepository<,>));

builder.Services.AddScoped<IHoaDonRepository, HoaDonRepository>();
builder.Services.AddScoped<IChiTietHoaDonRepository, ChiTietHoaDonRepository>();
builder.Services.AddScoped<IPhongRepository, PhongRepository>();
builder.Services.AddScoped<INhanVienRepository, NhanVienRepository>();
builder.Services.AddScoped<ILoiPhamRepository, LoiPhamRepository>();
builder.Services.AddScoped<IDichVuRepository, DichVuRepository>();
builder.Services.AddScoped<IHopDongRepository, HopDongRepository>();
builder.Services.AddScoped<INhaRepository, NhaRepository>();
builder.Services.AddScoped<IKhachHangRepository, KhachHangRepository>();
builder.Services.AddScoped<IPhuongXaRepository, PhuongXaRepository>();
builder.Services.AddScoped<IThanhPhoRepository, ThanhPhoRepository>();
builder.Services.AddScoped<IQuanHuyenRepository, QuanHuyenRepository>();
builder.Services.AddScoped<IDichVuPhongRepository, DichVuPhongRepository>();
builder.Services.AddScoped<IThanhVienPhongRepository, ThanhVienPhongRepository>();
builder.Services.AddScoped<ITheKiTucXaRepository, TheKiTucXaRepository>();
builder.Services.AddScoped<ITheSinhVienRepository, TheSinhVienRepository>();
builder.Services.AddScoped<ISuaChuaRepository, SuaChuaRepository>();

builder.Services.AddScoped<IVnPayService, VnPayService>();
builder.Services.AddScoped<ISuaChuaService, SuaChuaService>();
builder.Services.AddScoped<ITheSinhVienService, TheSinhVienService>();
builder.Services.AddScoped<ITheKiTucXaService, TheKiTucXaService>();
builder.Services.AddScoped<ILoiPhamService, LoiPhamService>();
builder.Services.AddScoped<IChiTietHoaDonService, ChiTietHoaDonService>();
builder.Services.AddScoped<IThanhVienPhongService, ThanhVienPhongService>();
builder.Services.AddScoped<IDichVuPhongService, DichVuPhongService>();
builder.Services.AddScoped<IHoaDonService, HoaDonService>();
builder.Services.AddScoped<IHopDongService, HopDongService>();
builder.Services.AddScoped<IPhongService, PhongService>();
builder.Services.AddScoped<INhaService, NhaService>();
builder.Services.AddScoped<IDichVuService, DichVuService>();
builder.Services.AddScoped<INhanVienService, NhanVienService>();
builder.Services.AddScoped<IKhachHangService, KhachHangService>();
builder.Services.AddScoped<IThanhPhoService, ThanhPhoService>();
builder.Services.AddScoped<IQuanHuyenService, QuanHuyenService>();
builder.Services.AddScoped<IPhuongXaService, PhuongXaService>();

//builder.Services.AddMemoryCache();

// Configure the HTTP request pipeline.
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapAreaControllerRoute(
     name: "TrangQuanTri",
     areaName: "Admin",
     pattern: "default",
     defaults: new { controller = "Home", action = "Index" });

  

    endpoints.MapAreaControllerRoute(
     name: "NhanVien",
     areaName: "Admin",
     pattern: "admin/quan-ly-nhan-vien",
     defaults: new { controller = "NhanVien", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "KhachHang",
     areaName: "Admin",
     pattern: "admin/quan-ly-khach-hang",
     defaults: new { controller = "KhachHang", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "DichVu",
     areaName: "Admin",
     pattern: "admin/quan-ly-dich-vu",
     defaults: new { controller = "DichVu", action = "Index" });

    endpoints.MapAreaControllerRoute(
    name: "Nha",
    areaName: "Admin",
    pattern: "admin/quan-ly-nha",
    defaults: new { controller = "Nha", action = "Index" });

    endpoints.MapAreaControllerRoute(
    name: "TheSinhVien",
    areaName: "Admin",
    pattern: "admin/quan-ly-the",
    defaults: new { controller = "TheSinhVien", action = "Index" });

    endpoints.MapAreaControllerRoute(
   name: "suachua",
   areaName: "Admin",
   pattern: "admin/sua-chua",
   defaults: new { controller = "SuaChua", action = "Index" });




    endpoints.MapAreaControllerRoute(
    name: "Phong",
    areaName: "Admin",
    pattern: "admin/quan-ly-phong",
    defaults: new { controller = "Phong", action = "Index" });


    endpoints.MapAreaControllerRoute(
    name: "HopDong",
    areaName: "Admin",
    pattern: "admin/quan-ly-hop-dong",
    defaults: new { controller = "HopDong", action = "Index" });

    endpoints.MapAreaControllerRoute(
    name: "HoaDonThanhToan",
    areaName: "Admin",
    pattern: "admin/quan-ly-hoa-don-thanh-toan",
    defaults: new { controller = "HoaDon", action = "Index" });

    endpoints.MapAreaControllerRoute(
   name: "TaoPhong",
   areaName: "Admin",
   pattern: "admin/tao-moi-phong",
   defaults: new { controller = "Phong", action = "AddView" });

    endpoints.MapAreaControllerRoute(
  name: "TaoPhong",
  areaName: "Admin",
  pattern: "admin/quan-ly-bao-cao",
  defaults: new { controller = "Phong", action = "BaoCao" });


    endpoints.MapAreaControllerRoute(
       name: "TaiKhoan",
       areaName: "Admin",
       pattern: "admin/quan-ly-tai-khoan",
       defaults: new { controller = "AppUsers", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "UserDetail",
      areaName: "Admin",
      pattern: "admin/thong-tin-ca-nhan",
      defaults: new { controller = "NhanVien", action = "ViewDetail" });

    endpoints.MapAreaControllerRoute(
      name: "ChangePass",
      areaName: "Admin",
      pattern: "admin/doi-mat-khau",
      defaults: new { controller = "NhanVien", action = "ChangePassWord" });

    endpoints.MapAreaControllerRoute(
       name: "LienHe",
       areaName: "Admin",
       pattern: "admin/quan-ly-lien-he-tu-van",
       defaults: new { controller = "Contract", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "DanhMuc",
       areaName: "Admin",
       pattern: "admin/quan-ly-danh-muc",
       defaults: new { controller = "Category", action = "Index" });

    endpoints.MapAreaControllerRoute(
       name: "ThemMoiSanPham",
       areaName: "Admin",
       pattern: "admin/tao-moi-san-pham",
       defaults: new { controller = "Product", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachSanPham",
      areaName: "Admin",
      pattern: "admin/quan-ly-san-pham",
      defaults: new { controller = "Product", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "ThemMoiBaiViet",
     areaName: "Admin",
     pattern: "admin/tao-moi-bai-viet",
     defaults: new { controller = "Post", action = "AddView" });

    endpoints.MapAreaControllerRoute(
      name: "DanhSachBaiViet",
      areaName: "Admin",
      pattern: "admin/quan-ly-bai-viet",
      defaults: new { controller = "Post", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-don-hang",
      defaults: new { controller = "Orders", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-danh-gia",
      defaults: new { controller = "Reviews", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "DonHang",
      areaName: "Admin",
      pattern: "admin/quan-ly-phi-van-chuyen",
      defaults: new { controller = "Fee", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "Advertisement",
      areaName: "Admin",
      pattern: "admin/quan-ly-quang-cao",
      defaults: new { controller = "Advertisement", action = "Index" });

    endpoints.MapAreaControllerRoute(
      name: "siders",
      areaName: "Admin",
      pattern: "admin/quan-ly-siders",
      defaults: new { controller = "Siders", action = "Index" });

    endpoints.MapAreaControllerRoute(
     name: "chitietdonhang",
     areaName: "Admin",
     pattern: "admin/chi-tiet-don-hang/{*orderId}",
     defaults: new { controller = "Orders", action = "OderDetail" });




    endpoints.MapAreaControllerRoute(
       name: "admin",
       areaName: "Admin",
       pattern: "admin/{controller=Home}/{action=Index}/{id?}");

    endpoints.MapAreaControllerRoute(
    name: "DangXuat",
    areaName: "Admin",
    pattern: "/dang-xuat",
    defaults: new { controller = "NhanVien", action = "LogOut" });



    //// Topic List
    //routes.MapControllerRoute(
    //    name: "topic_listing",
    //    pattern: "bo-suu-tap-du-an",
    //    defaults: new { controller = "Topic", action = "List" });

    endpoints.MapControllerRoute(
       name: "SanPhamChiTiet",
       pattern: "/chi-tiet-phong/{*phongId}",
       defaults: new { controller = "Phong", action = "PhongDetail" });

    endpoints.MapControllerRoute(
      name: "GioHang",
      pattern: "/gio-hang",
      defaults: new { controller = "Carts", action = "Index" });


    endpoints.MapControllerRoute(
      name: "DanhSachSanPhamW",
      pattern: "/nha/{*categoryId}",
      defaults: new { controller = "Phong", action = "PhongCategory" });


    endpoints.MapControllerRoute(
name: "suachuauser",
pattern: "/yeu-cau-sua-chua",
defaults: new { controller = "SuaChuaPhong", action = "Index" });

    endpoints.MapControllerRoute(
       name: "user",
       pattern: "/dang-ky",
       defaults: new { controller = "Users", action = "Register" });

    endpoints.MapControllerRoute(
       name: "userlogin",
       pattern: "/dang-nhap",
       defaults: new { controller = "Users", action = "Login" });
    endpoints.MapControllerRoute(
      name: "GioiThieu",
      pattern: "/gioi-thieu",
      defaults: new { controller = "Home", action = "About" });

    endpoints.MapControllerRoute(
      name: "CartPayment",
      pattern: "/thanh-toan",
      defaults: new { controller = "Carts", action = "OrderPay" });

    endpoints.MapControllerRoute(
     name: "BaiViet",
     pattern: "/bai-viet",
     defaults: new { controller = "Post", action = "Index" });

    endpoints.MapControllerRoute(
    name: "ChiTietBaiViet",
    pattern: "/chi-tiet-bai-viet/{*postId}",
    defaults: new { controller = "Post", action = "DetailPost" });

    endpoints.MapControllerRoute(
    name: "HoSoCaNhan",
    pattern: "/thong-tin-ca-nhan",
    defaults: new { controller = "Users", action = "Profile" });

    endpoints.MapControllerRoute(
    name: "LichSuMuaHang",
    pattern: "/lich-su-don-hang",
    defaults: new { controller = "Carts", action = "HistoryOrder" });

    endpoints.MapControllerRoute(
  name: "LienHeWe",
  pattern: "/lien-he-voi-chung-toi",
  defaults: new { controller = "Home", action = "Contact" });




    endpoints.MapControllerRoute(
  name: "TimKiemProduct",
  pattern: "/san-pham-tim-kiem/{*textSearch}",
  defaults: new { controller = "Phong", action = "ProductSearch" });



    endpoints.MapControllerRoute(
 name: "TimKiemPost",
 pattern: "/bai-viet-tim-kiem/{*textSearch}",
 defaults: new { controller = "Product", action = "ProductSearch" });

    endpoints.MapControllerRoute(
   name: "DoiMatKhauWeb",
   pattern: "/doi-mat-khau",
   defaults: new { controller = "Users", action = "ChangePass" });

    endpoints.MapControllerRoute(
 name: "QuenMatKhau",
 pattern: "/quen-mat-khau",
 defaults: new { controller = "Users", action = "ForgotPassword" });

    endpoints.MapControllerRoute(
name: "XacThuc",
pattern: "/xac-thuc",
defaults: new { controller = "Users", action = "Accuracy" });

    endpoints.MapControllerRoute(
name: "HoaDonThanhToaUser",
pattern: "/hoa-don-user",
defaults: new { controller = "HoaDon", action = "Index" });

    endpoints.MapControllerRoute(
    name: "TaoMatKhauMoi",
    pattern: "/tao-mat-khau-moi",
    defaults: new { controller = "Users", action = "ChangeNewPassWord" });

    endpoints.MapControllerRoute(
   name: "ChinhSachBaoMat",
   pattern: "/chinh-sach-bao-mat",
   defaults: new { controller = "Home", action = "About" });

    endpoints.MapControllerRoute(
 name: "ChinhSachThanhToan1",
 pattern: "/chinh-sach-thanh-toan",
 defaults: new { controller = "Home", action = "ThanhToan" });

    endpoints.MapControllerRoute(
 name: "DieuKhoanSuDung1",
 pattern: "/dieu-khoan-su-dung",
 defaults: new { controller = "Home", action = "DieuKhoanSuDung" });

    endpoints.MapControllerRoute(
 name: "GiaiQuyetKhieuNai1",
 pattern: "/giai-quyet-khieu-nai",
 defaults: new { controller = "Home", action = "GiaiQuyetKhieuNai" });

    endpoints.MapControllerRoute(
name: "HopDongUserNew",
pattern: "/hop-dong",
defaults: new { controller = "HopDong", action = "Index" });


    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");


});


//app.MapControllerRoute(

//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
