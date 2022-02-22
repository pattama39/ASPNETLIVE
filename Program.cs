using ASPNETLIVE.Data;
using ASPNETLIVE.Services.ThaiDate;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ASPNETLIVE.Areas.Identity.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// CORS
builder.Services.AddCors();

// Add services to the container.
builder.Services.AddDbContext<APIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext")));

// Identity
builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<IdentityContext>(); // options.SignIn.RequireConfirmedAccount = false => ไม่ต้องยืนยันอีเมล | AddRoles<IdentityRole>() => เรื่องสิทธิ

builder.Services.AddDbContext<IdentityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.Configure<IdentityOptions>(options =>
    {
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    }); // กำหนดการตั้งค่ารหัสผ่านของตัว Identity
// end Identity

// add Jwt Service สำหรับ validate token
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    config =>
    {
        config.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("odn051PvFMtRTBZsqmWkGJl8CHbKceQz")),
            ValidateIssuer = false,
            ValidateAudience = false,
            // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Custom Service
builder.Services.AddScoped<IThaiDate, ThaiDate>();

var app = builder.Build();

// global cors policy
app.UseCors(options => options
    // ระบุ
    //.WithOrigins("https://example.com", "https://codingthailand.com")
    //.WithHeaders()
    //.WithOrigins()

    // คุณคนใช้ได้
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
    );

app.UseStaticFiles(); //upload file

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
//app.Run("http://localhost:5000");
