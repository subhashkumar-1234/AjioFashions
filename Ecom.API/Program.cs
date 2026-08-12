using Ecom.Application.Interfaces;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Application.Services;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Ecom.Infrastructure.Repositories;
using Ecom.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
builder.Services.AddScoped<IAddItemService, AddItemService>();
builder.Services.AddScoped<IAddItemRepository, AddItemRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAddQuantityService, AddQuantityService>();
builder.Services.AddScoped<IAddQuantityRepository, AddQuantityRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

// Phase 2 Premium Services
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IUserAddressRepository, UserAddressRepository>();
builder.Services.AddScoped<IUserAddressService, UserAddressService>();
builder.Services.AddScoped<ICouponRepository, CouponRepository>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IEmailService, EmailService>();

// Phase 3 Premium Services
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
builder.Services.AddScoped<IWishlistService, WishlistService>();
builder.Services.AddScoped<IProductSizeStockRepository, ProductSizeStockRepository>();
builder.Services.AddScoped<IProductSizeStockService, ProductSizeStockService>();



// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Database
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("SqlCon")));

// JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("9Xv$7mK#2QpL@9Bn!4RtYw&6HsJcD3FgUa5BeVm1"))
    };
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReact",
        policy =>
        {
            //policy.WithOrigins("http://localhost:5174") // Vite React
            //      .AllowAnyHeader()
            //      .AllowAnyMethod();
            policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
        });
});


var app = builder.Build();
// Enable CORS
app.UseCors("AllowReact");

// Swagger
app.UseSwagger();
app.UseSwaggerUI();

// HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// Authentication (JWT Token Check)
app.UseAuthentication();

// Authorization (Role/User Access Check)
app.UseAuthorization();

// Controllers
app.MapControllers();

// Seed size stocks if empty
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.ProductSizeStocks.Any())
    {
        var items = context.AddItems.ToList();
        foreach (var item in items)
        {
            context.ProductSizeStocks.AddRange(new[]
            {
                new ProductSizeStock { ProductId = item.Id, Size = "S", Stock = 10 },
                new ProductSizeStock { ProductId = item.Id, Size = "M", Stock = 15 },
                new ProductSizeStock { ProductId = item.Id, Size = "L", Stock = 20 },
                new ProductSizeStock { ProductId = item.Id, Size = "XL", Stock = 5 }
            });
        }
        context.SaveChanges();
    }

    // Seed Seller Role if missing
    if (!context.Roles.Any(r => r.RoleName == "Seller"))
    {
        context.Roles.Add(new Role { RoleName = "Seller", Created = DateTime.UtcNow, Updated = DateTime.UtcNow });
        context.SaveChanges();
    }

    // Seed DeliveryAgent Role if missing
    if (!context.Roles.Any(r => r.RoleName == "DeliveryAgent"))
    {
        context.Roles.Add(new Role { RoleName = "DeliveryAgent", Created = DateTime.UtcNow, Updated = DateTime.UtcNow });
        context.SaveChanges();
    }
}

app.Run();