using Application.Interfaces.SeedWorks;
using Application.Interfaces.Services;
using Application.MappingProfiles;
using Application.Services;
using Domain.Entities.ApplicationIdentity;
using Infrastructure.Data;
using Infrastructure.DataSeeds;
using Infrastructure.SeedWorks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//ConnectionString
builder.Services.AddDbContext<TodoDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//AddIdentity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<TodoDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cấu hình JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,  // Kiểm tra Issuer
        ValidateAudience = true, // Kiểm tra Audience
        ValidateLifetime = true, // 🔥 Bật kiểm tra thời hạn hết hạn của token
        ValidateIssuerSigningKey = true, // Kiểm tra khóa ký
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),

        ClockSkew = TimeSpan.Zero // 🔥 Không cho phép trễ thời gian, mặc định là 5 phút
    };
});

//ConfigRedis
builder.Services.AddStackExchangeRedisCache(options =>
{
    var redisConfig = builder.Configuration.GetValue<string>("Redis:ConnectionString");
    options.Configuration = redisConfig;
});

//DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<ITodoService, TodoService>();
builder.Services.AddScoped<ITokenService, TokenService>();

//Mapping
builder.Services.AddAutoMapper(typeof(TodoMappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(builder =>
{
    builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//Seeder
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    await DataSeeder.SeedAsync(service);
}

app.Run();
