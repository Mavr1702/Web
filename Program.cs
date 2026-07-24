using Microsoft.EntityFrameworkCore;
using Web.Data;
using Web.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Dịch vụ MVC
builder.Services.AddControllersWithViews();

// 2. Dịch vụ DbContext (Sử dụng In-Memory Database)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("WebECommerceDB"));

// 3. Dịch vụ Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// 4. Khởi tạo dữ liệu mẫu (Data Seeding)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    if (!db.Categories.Any())
    {
        var catLaptop = new Category { Name = "Laptop" };
        var catPhone = new Category { Name = "Điện thoại & Tablet" };
        var catAccessory = new Category { Name = "Phụ kiện" };
        var catAudio = new Category { Name = "Thiết bị Âm thanh" };

        db.Categories.AddRange(catLaptop, catPhone, catAccessory, catAudio);
        db.SaveChanges();

        db.Products.AddRange(
            new Product { Name = "Laptop Dell XPS 15", Price = 35000000, CategoryId = catLaptop.Id },
            new Product { Name = "MacBook Pro M2", Price = 32000000, CategoryId = catLaptop.Id },
            new Product { Name = "iPhone 14 Pro Max", Price = 28000000, CategoryId = catPhone.Id },
            new Product { Name = "Samsung Galaxy S23 Ultra", Price = 26000000, CategoryId = catPhone.Id },
            new Product { Name = "iPad Air 5", Price = 15000000, CategoryId = catPhone.Id },
            new Product { Name = "Chuột Logitech G Pro", Price = 2500000, CategoryId = catAccessory.Id },
            new Product { Name = "Bàn phím cơ Keychron", Price = 1800000, CategoryId = catAccessory.Id },
            new Product { Name = "Màn hình LG 27 inch 4K", Price = 8500000, CategoryId = catAccessory.Id },
            new Product { Name = "Ốp lưng iPhone chống sốc", Price = 250000, CategoryId = catAccessory.Id },
            new Product { Name = "Pin sạc dự phòng Anker 10000mAh", Price = 800000, CategoryId = catAccessory.Id },
            new Product { Name = "Webcam Logitech C920", Price = 1500000, CategoryId = catAccessory.Id },
            new Product { Name = "Tai nghe Sony WH-1000XM5", Price = 7500000, CategoryId = catAudio.Id },
            new Product { Name = "Đồng hồ Apple Watch Series 8", Price = 10000000, CategoryId = catAccessory.Id },
            new Product { Name = "Loa Bluetooth JBL Charge 5", Price = 3500000, CategoryId = catAudio.Id },
            new Product { Name = "Microphone HyperX QuadCast", Price = 3200000, CategoryId = catAudio.Id }
        );
        db.SaveChanges();
    }
}

// 5. Cấu hình HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

// Sử dụng Session
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
