using ConnectDB.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- 1. CẤU HÌNH CORS (Thêm phần này) ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()  // Cho phép bất kỳ nguồn nào (domain nào) gọi vào
                  .AllowAnyMethod()  // Cho phép tất cả các phương thức (GET, POST, PUT, DELETE)
                  .AllowAnyHeader(); // Cho phép tất cả các Header
        });
});

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// --- 2. SỬ DỤNG CORS (Thêm dòng này TRƯỚC UseAuthorization) ---
app.UseCors("AllowAll");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseDefaultFiles(); // Tự động tìm file index.html khi vào trang chủ
app.UseStaticFiles();

app.MapControllers();

app.Run();